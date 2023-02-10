using D11;
using System;
using UnityEngine;
using UnityEngine.UI;

public class NativePluginDemo : MonoBehaviour
{

    public Text infoText;

    public Button showToastBtn;
    public Button showAlertBtn;
    public Button showConfirmBtn;
    public Button showProgressBarBtn;
    public Button dismissProgressBarBtn;
    public Button showTimePickerBtn;
    public Button showDatePickerBtn;
    public Button RootStatusBtn;
    public Button openSettingsBtn;
    public Button checkPermissionBtn;
    public Button checkUpdateBtn;
    public Button startUpdateBtn;
    public Button toggleUpdateBtn;
    public Button toggleTypeBtn;
    public Button toggleLogBtn;
    public Button enableLocationBtn;
    public Button filePickerBtn;
    public Button shareContentBtn;
    public Image selectedImage;
    public GameObject imagePanel;

    private UpdateMode updateMode = UpdateMode.PLAY_STORE;
    private UpdateType updateType = UpdateType.FLEXIBLE;
    [SerializeField]
    private string m_thirdPartyLink = "";
    private bool toggleLog = false;
    private string[] permissions = { "android.permission.WRITE_EXTERNAL_STORAGE", "android.permission.READ_CONTACTS", "android.permission.READ_CALENDAR" };

    private void OnEnable()
    {
        // Subscribe for events from NativeManager
        MobileNativeManager.OnUpdateAvailable += OnUpdateAvailable;
        MobileNativeManager.OnUpdateVersionCode += OnUpdateVersionCode;
        MobileNativeManager.OnUpdateStalenessDays += OnUpdateStalenessDays;
        MobileNativeManager.OnUpdateInstallState += OnUpdateInstallState;
        MobileNativeManager.OnUpdateDownloading += OnUpdateDownloading;
        MobileNativeManager.OnUpdateError += OnUpdateError;
        MobileNativeManager.OnPermissionGranted += OnPermissionGranted;
        MobileNativeManager.OnPermissionDenied += OnPermissionDenied;
        MobileNativeManager.OnPermissionError += OnPermissionError;
        MobileNativeManager.OnImagePicked += OnImagePicked;
        MobileNativeManager.OnImagePickedError += OnImagePickedError;
        MobileNativeManager.OnClickAction += OnClickAction;
        MobileNativeManager.OnSelectedAction += OnSelectedAction;
    }


    private void OnDisable()
    {
        // Unsubscribe for events from NativeManager
        MobileNativeManager.OnUpdateAvailable -= OnUpdateAvailable;
        MobileNativeManager.OnUpdateVersionCode -= OnUpdateVersionCode;
        MobileNativeManager.OnUpdateStalenessDays -= OnUpdateStalenessDays;
        MobileNativeManager.OnUpdateInstallState -= OnUpdateInstallState;
        MobileNativeManager.OnUpdateDownloading -= OnUpdateDownloading;
        MobileNativeManager.OnUpdateError -= OnUpdateError;
        MobileNativeManager.OnPermissionGranted -= OnPermissionGranted;
        MobileNativeManager.OnPermissionDenied -= OnPermissionDenied;
        MobileNativeManager.OnPermissionError -= OnPermissionError;
        MobileNativeManager.OnImagePicked -= OnImagePicked;
        MobileNativeManager.OnClickAction -= OnClickAction;
        MobileNativeManager.OnSelectedAction -= OnSelectedAction;
    }

    private void Start()
    {
        startUpdateBtn.interactable = false;
        dismissProgressBarBtn.interactable = false;

        showToastBtn.onClick.AddListener(() => {
            MobileNativeManager.Instance.ShowToast("Simple toast message from Unity", 0);
        });
        showAlertBtn.onClick.AddListener(() => {
            MobileNativeManager.Instance.ShowAlertMessage("Mobile Native Plugin", "Simple alert message from Unity");
        });
        showConfirmBtn.onClick.AddListener(() => {
            MobileNativeManager.Instance.ShowConfirmationMessage("Mobile Native Plugin", "Simple confirmation message from Unity");
        });
        showProgressBarBtn.onClick.AddListener(() => {
            MobileNativeManager.Instance.ShowProgressBar("Mobile Native Plugin", "Simple progressbar from Unity");
            dismissProgressBarBtn.interactable = true;
        });
        dismissProgressBarBtn.onClick.AddListener(() => {
            MobileNativeManager.Instance.DismissProgressBar();
            dismissProgressBarBtn.interactable = false;
        });
        showTimePickerBtn.onClick.AddListener(() => {
            MobileNativeManager.Instance.ShowTimePicker();
        });
        showDatePickerBtn.onClick.AddListener(() => {
            MobileNativeManager.Instance.ShowDatePicker();
        });
        RootStatusBtn.onClick.AddListener(() => {
            LoggerUtils.Log("isDeviceRooted::" + MobileNativeManager.Instance.IsDeviceRooted());
        });
        openSettingsBtn.onClick.AddListener(() => {
            MobileNativeManager.Instance.OpenSettingScreen();
        });
        checkPermissionBtn.onClick.AddListener(() => {
            MobileNativeManager.Instance.RequestPermission(permissions);
        });

        checkUpdateBtn.onClick.AddListener(() => {
            MobileNativeManager.Instance.CheckForUpdate(updateMode, updateType, m_thirdPartyLink);
        });
        startUpdateBtn.onClick.AddListener(() => {
            MobileNativeManager.Instance.StartUpdate();

            startUpdateBtn.interactable = false;
            checkUpdateBtn.interactable = false;
        });
        toggleUpdateBtn.onClick.AddListener(() => {
            if(updateMode == UpdateMode.PLAY_STORE)
                updateMode = UpdateMode.THIRD_PARTY;
            else
                updateMode = UpdateMode.PLAY_STORE;

            toggleUpdateBtn.GetComponentInChildren<Text>().text = "Update Mode : " + (updateMode == UpdateMode.PLAY_STORE ? " PLAY STORE" : " THIRD PARTY");
        });
        toggleTypeBtn.onClick.AddListener(() => {
            if(updateType == UpdateType.FLEXIBLE)
                updateType = UpdateType.IMMEDIATE;
            else
                updateType = UpdateType.FLEXIBLE;

            toggleTypeBtn.GetComponentInChildren<Text>().text = "Update Type : "+(updateType == UpdateType.FLEXIBLE ? " Flexiable" : " IMMEDIATE");
        });
        toggleLogBtn.onClick.AddListener(() => {
            toggleLog = !toggleLog;
            MobileNativeManager.Instance.PluginDebug(toggleLog);
            toggleLogBtn.GetComponentInChildren<Text>().text = "Log : " + (toggleLog ? " enabled" : " diabled");
        });

        toggleUpdateBtn.onClick.Invoke();
        toggleTypeBtn.onClick.Invoke();
        toggleLogBtn.onClick.Invoke();

        filePickerBtn.onClick.AddListener(()=> {
            MobileNativeManager.Instance.GetImageFromDevice();
        });

        enableLocationBtn.onClick.AddListener(() => {
            MobileNativeManager.Instance.EnableLocation();
        });

        shareContentBtn.onClick.AddListener(() => {
            MobileNativeManager.Instance.ShareOnWhatsApp("This is test message to share");
        });
    }

    private void OnUpdateError(int code, string error)
    {
        if (code == (int)InstallErrorCode.ERROR_LIBRARY)
            LoggerUtils.Log("Error : " + error);
        else
            LoggerUtils.Log("Error Code : " + (InstallErrorCode)code + " :: " + error);
        startUpdateBtn.interactable = false;
        checkUpdateBtn.interactable = true;

    }

    private void OnUpdateDownloading(long bytesDownloaded, long bytesTotal)
    {
        int downloadProgress = (int)((bytesDownloaded * 100) / bytesTotal);
        infoText.text = "Downloading : " + bytesDownloaded.ToPrettySize(false, 1) + "/" + bytesTotal.ToPrettySize(true, 1) + " ( " + downloadProgress + "% )";
    }

    private void OnUpdateInstallState(InstallStatus state)
    {
        infoText.text = "Install State : " + state.ToString();

        if (state == InstallStatus.DOWNLOADED)
        {
            MobileNativeManager.Instance.CompleteUpdate();
            startUpdateBtn.interactable = false;
            checkUpdateBtn.interactable = true;
        }
    }

    private void OnUpdateStalenessDays(int days)
    {
        LoggerUtils.Log("Staleness Days : " + days);
    }

    private void OnUpdateVersionCode(int versionCode)
    {
        LoggerUtils.Log("Version code : " + versionCode);
    }

    private void OnUpdateAvailable(bool isUpdateAvailable)
    {
        infoText.text = "Is update Available : " + isUpdateAvailable;

        startUpdateBtn.interactable = isUpdateAvailable;
    }

    private void OnPermissionDenied(string[] deniedPermissions)
    {
        LoggerUtils.Log("OnPermissionDenied : We got some denied permissions.");
    }

    private void OnPermissionGranted(string[] grantedPermissions, bool all)
    {
        if (all)
        {
            LoggerUtils.Log("OnPermissionGranted : All permissions are granted. You good to go.");
            if(grantedPermissions[0] == "android.permission.ACCESS_FINE_LOCATION")
            {
                MobileNativeManager.Instance.EnableLocation();
            }
        }
        else
        {
            LoggerUtils.Log("OnPermissionGranted : Some permissions are denied.");
        }
    }

    private void OnPermissionError(string error)
    {
        LoggerUtils.Log("Error : " + error);
    }


    private void OnImagePickedError(int errorCode, string message)
    {

        if (errorCode != -1)
        { 
            LoggerUtils.Log("OnImagePicked::failed::" + message);
        }
    }
    private void OnImagePicked(byte[] bArray, int width, int height)
    {
        LoggerUtils.Log("OnImagePicked::success");

        Texture2D texture2D = new Texture2D(width, height, TextureFormat.BGRA32, false);
        texture2D.LoadImage(bArray);

        Sprite newSprite = Sprite.Create(texture2D as Texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), Vector2.zero);
        selectedImage.sprite = newSprite;
        imagePanel.SetActive(true);
    }

    private void OnClickAction(bool action)
    {
        LoggerUtils.Log("OnClickAction clicked:: " + action);
    }
    
    private void OnSelectedAction(string value)
    {
        LoggerUtils.Log("OnSelectedAction selected:: " + value);
    }
}
