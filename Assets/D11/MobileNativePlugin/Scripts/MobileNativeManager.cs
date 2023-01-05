using D11;
using System;
using UnityEngine;

namespace D11
{
    [Serializable]
    public class ImageData
    {
        public bool status;
        public string message;
        public int errorCode;
        public int width;
        public int height;
        public string uri;
        public string path;
        public string imageBase64;
    }

    public class MobileNativeManager : SingletonMonoBehaviour<MobileNativeManager>
    {
        #region Events
#pragma warning disable 67
        public static event Action<bool> OnUpdateAvailable;         // Event triggered when the update is available or not
        public static event Action<int> OnUpdateVersionCode;        // Event triggered with version code available
        public static event Action<int> OnUpdateStalenessDays;      // Event triggered with staleness days available to download
        public static event Action<InstallStatus> OnUpdateInstallState;       // Event triggered with install state during update
        public static event Action<long, long> OnUpdateDownloading; // Event triggered with downloading value
        public static event Action<int, string> OnUpdateError;      // Event triggered with error during update
        public static event Action<string[], bool> OnPermissionGranted;      // Event triggered with permissions granted
        public static event Action<string[]> OnPermissionDenied;      // Event triggered with permissions denied
        public static event Action<string> OnPermissionError;      // Event triggered with error when during permissions
        public static event Action<byte[], int, int> OnImagePicked;      // Event triggered with image picked
        public static event Action<int, string> OnImagePickedError;      // Event triggered with image picked
        public static event Action<bool> OnClickAction;      // Event triggered when dialog buttons are clicked
        public static event Action<string> OnSelectedAction;      // Event triggered with value when selected
#pragma warning restore 67
        #endregion

#pragma warning disable 0414
        [Tooltip("Android Launcher Activity")]
        [SerializeField]
        private string m_unityMainActivity = "com.unity3d.player.UnityPlayer";
#pragma warning restore 0414
#if UNITY_ANDROID && !UNITY_EDITOR
        private AndroidJavaObject mContext = null;
        private AndroidJavaObject mUpdateManager = null;
        private AndroidJavaObject mPermissionManager = null;
        private AndroidJavaObject mImagePickerManager = null;
        private AndroidJavaClass mAndroidBridge = null;

        class OnUpdateListener : AndroidJavaProxy
        {
            public OnUpdateListener() : base("com.onedevapp.nativeplugin.inappupdate.OnUpdateListener") { }

            // Invoked when Google Play Services returns a response 
            public void onUpdateAvailable(bool isUpdateAvailable, bool isUpdateTypeAllowed)
            {
                if (isUpdateAvailable && isUpdateTypeAllowed)
                {
                    if (OnUpdateAvailable != null)
                    {
                        OnUpdateAvailable.Invoke(true);
                    }
                }
                else
                {
                    if (OnUpdateAvailable != null)
                    {
                        OnUpdateAvailable.Invoke(false);
                    }
                }
            }

            // Invoked when the update is available with version code
            public void onUpdateVersionCode(int versionCode)
            {
                if (OnUpdateVersionCode != null)
                {
                    OnUpdateVersionCode.Invoke(versionCode);
                }
            }

            // Invoked when the update is available with staleness days
            public void onUpdateStalenessDays(int days)
            {
                if (OnUpdateStalenessDays != null)
                {
                    OnUpdateStalenessDays.Invoke(days);
                }
            }

            // Invoked when install status of the update
            public void onUpdateInstallState(int state)
            {
                if (OnUpdateInstallState != null)
                {
                    OnUpdateInstallState.Invoke((InstallStatus)state);
                }
            }

            // Invoked during downloading
            public void onUpdateDownloading(long bytesDownloaded, long bytesTotal)
            {
                if (OnUpdateDownloading != null)
                {
                    OnUpdateDownloading.Invoke(bytesDownloaded, bytesTotal);
                }
            }

            // Invoked when the update encounter error
            public void onUpdateError(int code, string error)
            {
                if (OnUpdateError != null)
                {
                    OnUpdateError.Invoke(code, error);
                }
            }
        }

        class OnPermissionListener : AndroidJavaProxy
        {
            public OnPermissionListener() : base("com.onedevapp.nativeplugin.rt_permissions.OnPermissionListener") { }


            public void onPermissionGranted(string[] grantPermissions, bool all)
            {
                if (OnPermissionGranted != null)
                {
                    OnPermissionGranted.Invoke(grantPermissions, all);
                }
            }

            public void onPermissionDenied(string[] deniedPermissions)
            {
                if (OnPermissionDenied != null)
                {
                    OnPermissionDenied.Invoke(deniedPermissions);
                }
            }

            public void onPermissionError(string errorMessage)
            {
                if (OnPermissionError != null)
                {
                    OnPermissionError.Invoke(errorMessage);
                }
            }
        }

        class OnClickPositiveListener : AndroidJavaProxy
        {
            public OnClickPositiveListener() : base("com.onedevapp.nativeplugin.AndroidBridge$OnClickListener") { }

            public void onClick()
            {
                        LoggerUtils.Log("Possitive click");

                if (OnClickAction != null)
                {
                    OnClickAction.Invoke(true);
                }
            }
        }

        class OnClickNegativeListener : AndroidJavaProxy
        {
            public OnClickNegativeListener() : base("com.onedevapp.nativeplugin.AndroidBridge$OnClickListener") { }

            public void onClick()
            {
                        LoggerUtils.Log("Negative click");
                if (OnClickAction != null)
                {
                    OnClickAction.Invoke(false);
                }
            }
        }

        class OnSelectedListener : AndroidJavaProxy
        {
            public OnSelectedListener() : base("com.onedevapp.nativeplugin.AndroidBridge$OnSelectListener") { }

            public void onSelected(string selectedValue)
            {
                if (OnSelectedAction != null)
                {
                    OnSelectedAction.Invoke(selectedValue);
                }
            }
        }
#endif

        protected override void Awake()
        {
            base.Awake();

#if UNITY_ANDROID && !UNITY_EDITOR
            if (Application.platform == RuntimePlatform.Android)
            {
                mContext = new AndroidJavaClass(m_unityMainActivity).GetStatic<AndroidJavaObject>("currentActivity");
                mAndroidBridge = new AndroidJavaClass("com.onedevapp.nativeplugin.AndroidBridge");
            }
#else
            LoggerUtils.Log("Platform not supported");
#endif
        }


#region App Update
        /// <summary>
		/// Check for update and returns OnUpdateListener.onUpdateAvailable true or false
		/// </summary>
        /// <param name="updateMode">update mode</param>
        /// <param name="updatetype">update type</param>
        /// <param name="apkLink">new apk link</param>
        public void CheckForUpdate(UpdateMode updateMode= UpdateMode.PLAY_STORE, UpdateType updateType = UpdateType.FLEXIBLE, string thirdPartyLink  = "")
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            // Initialize Update Manager
            var manager = new AndroidJavaClass("com.onedevapp.nativeplugin.inappupdate.UpdateManager");

            mUpdateManager = manager.CallStatic<AndroidJavaObject>("Builder", mContext);

            mUpdateManager.Call<AndroidJavaObject>("updateMode", (int)updateMode)
                .Call<AndroidJavaObject>("handler", new OnUpdateListener())
                .Call<AndroidJavaObject>("updateType", (int)updateType);

            if (!string.IsNullOrEmpty(thirdPartyLink))
                mUpdateManager.Call<AndroidJavaObject>("updateLink", thirdPartyLink);

            mUpdateManager.Call("checkUpdate");
#else
            LoggerUtils.Log("Platform not supported");
#endif
        }

        /// <summary>
        /// To start the instalation of the update, you should call this function after OnUpdateListener.onUpdate(isUpdateAvailable, isUpdateTypeAllowed)
        /// and only if both isUpdateAvailable and isUpdateTypeAllowed are true 
        /// </summary>
        public void StartUpdate()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (mUpdateManager != null)
                mUpdateManager.Call("startUpdate");
#else
            LoggerUtils.Log("Platform not supported");
#endif
        }

        /// <summary>
        /// To complete the instalation of the update, you should call this function after OnUpdateListener.onUpdateInstallState().
        /// This has no impact while using third party link update
        /// </summary>
        public void CompleteUpdate()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (mUpdateManager != null)
                mUpdateManager.Call("completeUpdate");
#else
            LoggerUtils.Log("Platform not supported");
#endif
        }

        /// <summary>
        /// To continue update and must can called only on app onresume to complete the pending update of previous update
        /// </summary>
        public void ContinueUpdate()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (mUpdateManager != null)
                mUpdateManager.Call("continueUpdate");
#else
            LoggerUtils.Log("Platform not supported");
#endif
        }

#endregion

#region App Permission
        /// <summary>
        /// Check for permission and returns status in OnPermissionListener.onPermissionGranted()
        /// </summary>
        /// <param name="permissions">Array of requested permissions</param>
        public void RequestPermission(params string[] permissions)
        {

#if UNITY_ANDROID && !UNITY_EDITOR
            // Initialize Permission Manager
            var manager = new AndroidJavaClass("com.onedevapp.nativeplugin.rt_permissions.PermissionManager");

            mPermissionManager = manager.CallStatic<AndroidJavaObject>("Builder", mContext);
            mPermissionManager
                .Call<AndroidJavaObject>("handler", new OnPermissionListener())
                .Call<AndroidJavaObject>("addPermissions", javaArrayFromCS(permissions))
                .Call("requestPermission");
#else
            LoggerUtils.Log("Platform not supported");
#endif
        }

        /// <summary>
        /// Check for permission and returns status in OnPermissionListener.onPermissionGranted()
        /// </summary>
        /// <param name="permissions">Array of requested permissions</param>
        public void RequestPermission(string permission)
        {

#if UNITY_ANDROID && !UNITY_EDITOR
            // Initialize Permission Manager
            var manager = new AndroidJavaClass("com.onedevapp.nativeplugin.rt_permissions.PermissionManager");

            mPermissionManager = manager.CallStatic<AndroidJavaObject>("Builder", mContext);
            mPermissionManager
                .Call<AndroidJavaObject>("handler", new OnPermissionListener())
                .Call<AndroidJavaObject>("addPermission", permission)
                .Call("requestPermission");
#else
            LoggerUtils.Log("Platform not supported");
#endif
        }

        /// <summary>
        /// Check whether permission is granted or not
        /// </summary>
        /// <param name="permissions">Array of requested permissions</param>
        /// <returns>Returns True if already permission granted for this permission else false.</returns>
        public bool CheckPermission(string permission)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return mAndroidBridge.CallStatic<bool>("CheckPermission", mContext, permission);
#else
            return true;
#endif
        }

        /// <summary>
        /// Check whether permission permission can show rationale dialog
        /// </summary>
        /// <returns>Returns True if permission is requested but not granted and can show rationale dialog else false.</returns>
        public bool CheckPermissionRationale(string permission)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return mAndroidBridge.CallStatic<bool>("CheckPermissionRationale", mContext, permission);
#else
            return true;
#endif
        }
#endregion

#region ImagePicker

        /// <summary>
        /// Check for permission and returns status in OnPermissionListener.onPermissionGranted()
        /// </summary>
        public void GetImageFromDevice(ImagePickerType pickerType = ImagePickerType.CHOICE, int maxWidth = 612, int maxHeight = 816, int quality = 80)
        {

#if UNITY_ANDROID && !UNITY_EDITOR
            // Initialize ImagePicker Manager
            var mImagePickerManagerJavaClass = new AndroidJavaClass("com.onedevapp.nativeplugin.imagepicker.ImagePickerManager");

            mImagePickerManager = mImagePickerManagerJavaClass.CallStatic<AndroidJavaObject>("Builder", mContext);
            mImagePickerManager
                .Call<AndroidJavaObject>("setPickerType", (int) pickerType)
                .Call<AndroidJavaObject>("setMaxWidth", maxWidth)
                .Call<AndroidJavaObject>("setMaxHeight", maxHeight)
                .Call<AndroidJavaObject>("setQuality", quality)
                .Call("openImagePicker");
#else
            LoggerUtils.Log("Platform not supported");
#endif
        }

        #endregion

#region Share
        /// <summary>
        /// Enable Location.
        /// </summary>
        public void ShareOnWhatsApp(string message, string mobileNo = "", string filePath = "")
        {

#if UNITY_ANDROID && !UNITY_EDITOR
            mAndroidBridge.CallStatic("ShareOnWhatsApp", mContext, message, mobileNo, filePath);
#else
            LoggerUtils.Log("Platform not supported");
#endif
        }

        public void ShareContent(string message, string filePath = "", string shareSubject = "", string shareHeader = "Share to")
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            mAndroidBridge.CallStatic("ShareData", mContext, message, filePath, shareSubject, shareHeader);
#else
            LoggerUtils.Log("Platform not supported");
#endif
        }
        #endregion

#region Location
        /// <summary>
        /// Enable Location.
        /// </summary>
        public void EnableLocation()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (mAndroidBridge.CallStatic<bool>("CheckPermission", mContext, "android.permission.ACCESS_FINE_LOCATION"))
            {
                mAndroidBridge.CallStatic("EnableLocation", mContext);
            }
            else
            {
                LoggerUtils.Log("Permisson not granted");
                RequestPermission("android.permission.ACCESS_FINE_LOCATION");
            }
#else
            LoggerUtils.Log("Platform not supported");
#endif
        }
#endregion

#region Toast
        /// <summary>
        /// Shows the toast.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="Length">Toast Duration, For Short 0,For Long 1.</param>
        public void ShowToast(string message, int Length = 0)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            mAndroidBridge.CallStatic("ShowToast", mContext, message, Length);
#else
            LoggerUtils.Log("Platform not supported");
#endif
        }
#endregion

#region AlertDialogs
        /// <summary>
        /// Shows alert dialog
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="message">Message.</param>
        /// <param name="positiveButtonName">Positive Button Name.</param>
        public void ShowAlertMessage(string title, string message, string positiveButtonName = "OK")
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            mAndroidBridge.CallStatic("ShowAlertMessage", mContext, title, message, positiveButtonName, new OnClickPositiveListener());
#else
            LoggerUtils.Log("Platform not supported");
#endif
        }

        /// <summary>
        /// Shows confirmation dialog
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="message">Message.</param>
        /// <param name="positiveButtonName">Positive Button Name.</param>
        /// <param name="negativeButtonName">Negative Button Name.</param>
        public void ShowConfirmationMessage(string title, string message, string positiveButtonName = "OK", string negativeButtonName = "Cancel")
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            mAndroidBridge.CallStatic("ShowConfirmationMessage", mContext, title, message, positiveButtonName, new OnClickPositiveListener(), negativeButtonName, new OnClickNegativeListener());
#else
            LoggerUtils.Log("Platform not supported");
#endif
        }

        /// <summary>
        /// Shows the progress bar.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="message">Message.</param>
        /// <param name="cancelable">If set true then on clicking outside disable ProgressBar</param>
        public void ShowProgressBar(string title, string message, bool cancelable = true)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            mAndroidBridge.CallStatic("ShowProgressBar", mContext, title, message, cancelable);
#else
            LoggerUtils.Log("Platform not supported");
#endif
        }

        /// <summary>
        /// Dismiss the progress bar
        /// </summary>
        public void DismissProgressBar()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            mAndroidBridge.CallStatic("DismissProgressBar");
#else
            LoggerUtils.Log("Platform not supported");
#endif
        }

        /// <summary>
        /// Shows the time picker dialog
        /// </summary>
        public void ShowTimePicker()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            mAndroidBridge.CallStatic("ShowTimePicker", mContext, new OnSelectedListener());
#else
            LoggerUtils.Log("Platform not supported");
#endif
        }

        /// <summary>
        /// Shows the time picker dialog with requested hour, mins and format
        /// </summary>
        /// <param name="hour">Hour.</param>
        /// <param name="minutes">Mintues.</param>
        /// <param name="is24HourFormat">If set true then time picker dialog shows with 24 hours format else 12 hours</param>
        public void ShowTimePicker(int hour, int minutes, bool is24HourFormat)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            mAndroidBridge.CallStatic("ShowTimePicker", mContext, hour, minutes, is24HourFormat, new OnSelectedListener());
#else
            LoggerUtils.Log("Platform not supported");
#endif
        }

        /// <summary>
        /// Shows the date picker dialog
        /// </summary>
        public void ShowDatePicker()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            mAndroidBridge.CallStatic("ShowDatePicker", mContext, new OnSelectedListener());
#else
            LoggerUtils.Log("Platform not supported");
#endif
        }

        /// <summary>
        /// Shows the date picker dialog with requested date
        /// </summary>
        /// <param name="year">Year.</param>
        /// <param name="month">Month.</param>
        /// <param name="day">Date.</param>
        public void ShowDatePicker(int year, int month, int day)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            mAndroidBridge.CallStatic("ShowDatePicker", mContext, year, month, day, new OnSelectedListener());
#else
            LoggerUtils.Log("Platform not supported");
#endif
        }
#endregion

#region Options
        /*public string GetPackageName()
        {
#if UNITY_ANDROID
            return mContext.Call<string>("getPackageName");
#else
            return "";
#endif
        }*/

        /// <summary>
        /// Check whether device is rooted or not
        /// </summary>
        public bool IsDeviceRooted()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return mAndroidBridge.CallStatic<bool>("IsDeviceRooted");
#else
            return false;
#endif
        }

        /// <summary>
        /// Opens app settings page
        /// </summary>
        public void OpenSettingScreen()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            mAndroidBridge.CallStatic("OpenSettings", mContext);
#else
            LoggerUtils.Log("Platform not supported");
#endif
        }
#endregion

#region Debug
        /// <summary>
        /// By default puglin console log will be diabled, but can be enabled
        /// </summary>
        /// <param name="showLog">If set true then log will be displayed else disabled</param>
        public void PluginDebug(bool showLog = true)
        {
#if UNITY_ANDROID && !UNITY_EDITOR

            var constantClass = new AndroidJavaClass("com.onedevapp.nativeplugin.Constants");
            constantClass.SetStatic("enableLog", showLog);
#else
            LoggerUtils.Log("Platform not supported");
#endif
        }
#endregion


#if UNITY_ANDROID && !UNITY_EDITOR
        /// <summary>
        /// Converts Csharp array to java array
        /// https://stackoverflow.com/questions/42681410/androidjavaobject-call-array-passing-error-unity-for-android
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private AndroidJavaObject javaArrayFromCS(string[] values)
        {
            AndroidJavaClass arrayClass = new AndroidJavaClass("java.lang.reflect.Array");
            AndroidJavaObject arrayObject = arrayClass.CallStatic<AndroidJavaObject>("newInstance", new AndroidJavaClass("java.lang.String"), values.Length);
            for (int i = 0; i < values.Length; ++i)
            {
                arrayClass.CallStatic("set", arrayObject, i, new AndroidJavaObject("java.lang.String", values[i]));
            }

            return arrayObject;
        }
        
#endif


        public void OnImagePickedResult(string param)
        {
            //LoggerUtils.Log(param);
            ImageData imageData = JsonUtility.FromJson<ImageData>(param);

            bool success = imageData.status;
            int errorCode = imageData.errorCode;
            string message = imageData.message;

            if (success)
            {
#if (UNITY_ANDROID && !UNITY_EDITOR)

                byte[] bArray = Convert.FromBase64String(imageData.imageBase64);

                int w = imageData.width;
                int h = imageData.height;

                OnImagePicked.Invoke(bArray, w , h);
#endif
            }
            else
            {
                OnImagePickedError.Invoke(errorCode, message);
            }
        }
    }

}