using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Functions;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Firestore;
using TMPro;
using D11;


public class FireBaseManager : MonoBehaviour
{

    public static FireBaseManager Instance;
    public bool isFirstTime;
    private bool IsFreshInstall = true;

    [Header("FireBase")]
    public FirebaseAuth auth;
    public FirebaseUser user;
    public FirebaseFirestore db;

    public TMP_InputField OTP;
    public TMP_InputField Email;

    private void Awake()
    {

        Instance = this;

        if (PlayerPrefs.HasKey("FreshInstall"))
        {
            IsFreshInstall = PlayerPrefs.GetInt("FreshInstall", 0) == 0;
        }
        else
        {
            IsFreshInstall = true;
        }

        db = FirebaseFirestore.DefaultInstance;
        isFirstTime = true;
        auth = FirebaseAuth.DefaultInstance;
        StartCoroutine(CheckAutoLogin());
    }



    private IEnumerator CheckAutoLogin()
    {
        user = auth.CurrentUser;
        yield return new WaitForEndOfFrame();


        if (user != null && !IsFreshInstall)
        {
            var reloadUserTask = user.ReloadAsync();
            yield return new WaitUntil(predicate: () => reloadUserTask.IsCompleted);
            AutoLogin();

        }
    }
    private void AutoLogin()
    {
        if (user != null)
        {
            Debug.Log("AutoLogin Success");


            UIController.Instance.MainMenuScreen.ShowMe();
            UIController.Instance.Loginscreen.HideMe();
            UIController.Instance.RegisterScreen.HideMe();
            UIController.Instance.loading.SetActive(true);
            GameController.Instance.myUserID = user.UserId;

            GameController.Instance.GetUserDetails();
        }
        else
        {
            UIController.Instance.loading.SetActive(false);
            UIController.Instance.Loginscreen.ShowMe();
        }
    }
    bool signedIn = false;
    public void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            user = auth.CurrentUser;
        }
    }

    #region LoginLogic
    public IEnumerator LoginLogic(string email, string password, TMP_Text errormsg, GameObject loadingtxt, GameObject loadinganim)
    {
        Debug.Log("called");
        Credential credential = EmailAuthProvider.GetCredential(email, password);

        var loginTask = auth.SignInWithCredentialAsync(credential);


        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);


        if (loginTask.Exception != null)
        {
            //UIController.Instance.LoadingScreen.HideMe();
            FirebaseException firebaseException = (FirebaseException)loginTask.Exception.GetBaseException();
            AuthError error = (AuthError)firebaseException.ErrorCode;
            string Error = "Unknown Error";
            switch (error)
            {
                case AuthError.MissingEmail:
                    Error = "Please enter your email";
                    break;
                case AuthError.MissingPassword:
                    Error = "Please enter your password";
                    break;
                case AuthError.InvalidEmail:
                    Error = "Invalid email";
                    break;
                case AuthError.WrongPassword:
                    Error = "Incorrect password";
                    break;
                case AuthError.UserNotFound:
                    Error = "Account does not exist ";
                    break;

            }

            Debug.Log("Error Message:" + " " + Error);
            errormsg.text = Error;
            loadingtxt.SetActive(true);
            loadinganim.SetActive(false);
            StartCoroutine(LoginHandler.Instance.clear());

        }
        else
        {
            user = auth.CurrentUser;


            GameController.Instance.myUserID = user.UserId;
            GameController.Instance.myData.Name = user.DisplayName;
            UIController.Instance.loading.SetActive(true);
            PlayerPrefs.SetInt("signedIn", 1);
            UIController.Instance.MainMenuScreen.ShowMe();
            UIController.Instance.Loginscreen.HideMe();
            UIController.Instance.RegisterScreen.HideMe();
            GameController.Instance.GetUserDetails();
        }
    }

    #endregion

    public void Forgotpasswordlogic()
    {
        user = auth.CurrentUser;

            auth.SendPasswordResetEmailAsync(Email.text).ContinueWithOnMainThread(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SendPasswordResetEmailAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("Password reset email sent successfully.");
            });
        
        
        
    }


    #region RegisterLogic
    string _email;
    string _userName;
    string _passWord;
    public IEnumerator RegisterLogic(string username, string email, string password, string mobile, TMP_Text errormsg, GameObject loadingtxt, GameObject loadinganim)
    {
        Debug.Log("called");
        uint phoneAuthTimeoutMs = 10000;
        PhoneAuthProvider provider = PhoneAuthProvider.GetInstance(auth);
        provider.VerifyPhoneNumber($"+91{mobile}",phoneAuthTimeoutMs, null,
        verificationCompleted: (credential) =>
        {

            Debug.Log("Successs");

        },
        verificationFailed: (error) =>
        {
            Debug.Log(error + " *************************");
            // The verification code was not sent.
            // `error` contains a human readable explanation of the problem.
        },
        codeSent: (id, token) =>
        {
            Debug.Log(id + " *************************" + " " + token);
            string ID = id;
            PlayerPrefs.SetString("UniqueAuthID", ID);
            _email = email;
            _userName = username;
            _passWord = password;
            // Verification code was successfully sent via SMS.
            // `id` contains the verification id that will need to passed in with
            // the code from the user when calling GetCredential().
            // `token` can be used if the user requests the code be sent again, to
            // tie the two requests together.
        },
        codeAutoRetrievalTimeOut: (id) =>
        {
            Debug.Log(id + " *************************" + " " );
            // Called when the auto-sms-retrieval has timed out, based on the given
            // timeout parameter.
            // `id` contains the verification id of the request that timed out.
        });

        yield return null;

        //var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        //yield return new WaitUntil(predicate: () => registerTask.IsCompleted);

        //if (registerTask.Exception != null)
        //{
        //    FirebaseException firebaseException = (FirebaseException)registerTask.Exception.GetBaseException();
        //    AuthError error = (AuthError)firebaseException.ErrorCode;
        //    string Error = "Unknown Error  Validate the Error in Switch";
        //    switch (error)
        //    {
        //        case AuthError.InvalidEmail:
        //            Error = "Invalid email";
        //            break;
        //        case AuthError.EmailAlreadyInUse:
        //            Error = "Email already in use";
        //            break;
        //        case AuthError.WeakPassword:
        //            Error = "Weakpassword";
        //            break;
        //        case AuthError.MissingEmail:
        //            Error = "Please enter your email";
        //            break;
        //        case AuthError.MissingPassword:
        //            Error = "Please enter your password";
        //            break;
        //        case AuthError.Failure:
        //            Error = "Failure";
        //            break;

        //    }

        //    Debug.Log("Error Message:" + " " + Error);
        //    errormsg.text = Error;
        //    loadingtxt.SetActive(true);
        //    loadinganim.SetActive(false);
        //    StartCoroutine(RegisterHandler.Instance.clear());
        //    //UIController.Instance.LoadingScreen.HideMe();

        //}
        //else
        //{
        //    user = auth.CurrentUser;
        //    UserProfile profile = new UserProfile
        //    {
        //        DisplayName = username,
        //    };
        //    var defaultUserTask = user.UpdateUserProfileAsync(profile);
        //    yield return new WaitUntil(predicate: () => defaultUserTask.IsCompleted);
        //    if (defaultUserTask.Exception != null)
        //    {
        //        user.DeleteAsync();
        //        FirebaseException firebaseException = (FirebaseException)defaultUserTask.Exception.GetBaseException();
        //        AuthError error = (AuthError)firebaseException.ErrorCode;
        //        string Error = "Unknown Error  Validate the Error in Switch";
        //        switch (error)
        //        {
        //            case AuthError.Cancelled:
        //                Error = "Update User Cancelled";
        //                break;
        //            case AuthError.SessionExpired:
        //                Error = "SessionExpiered";
        //                break;


        //        }

        //        Debug.Log("Error Message:" + " " + Error);
        //        errormsg.text = Error;
        //        StartCoroutine(RegisterHandler.Instance.clear());
        //        //UIController.Instance.LoadingScreen.HideMe();
        //        loadingtxt.SetActive(true);
        //        loadinganim.SetActive(false);



        //    }
        //    else
        //    {
        //        Debug.Log($"FireBase user Created Succesfully : {user.DisplayName} {user.UserId}");

        //        UIController.Instance.loading.SetActive(true);
        //        PlayerPrefs.SetInt("signedIn", 1);
        //        GameController.Instance.myUserID = user.UserId;
        //        GameController.Instance.myData.Name = user.DisplayName;

        //        //UIController.Instance.LoadingScreen.HideMe();
        //        UIController.Instance.MainMenuScreen.ShowMe();
        //        UIController.Instance.Loginscreen.HideMe();
        //        UIController.Instance.RegisterScreen.HideMe();
        //        CreateNewUserInFireStore();



        //    }
        //}


    }
    public void VerifyCode()
    {
        PhoneAuthProvider provider = PhoneAuthProvider.GetInstance(auth);
       string ID =   PlayerPrefs.GetString("UniqueAuthID");
        Credential credential = provider.GetCredential(ID, OTP.text);

        auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " +task.Exception);
                return;
            }
            user = task.Result;
            StartCoroutine(nameof(AddEmailAndDisplayName));
        });
    }

    public IEnumerator AddEmailAndDisplayName()
    {
        user.UpdateEmailAsync(_email);
        user.UpdatePasswordAsync(_passWord);
        UserProfile profile = new UserProfile
        {
            DisplayName = _userName,
        };
        var defaultUserTask = user.UpdateUserProfileAsync(profile);
        yield return new WaitUntil(predicate: () => defaultUserTask.IsCompleted);
        if (defaultUserTask.Exception != null)
        {
            user.DeleteAsync();
            FirebaseException firebaseException = (FirebaseException)defaultUserTask.Exception.GetBaseException();
            AuthError error = (AuthError)firebaseException.ErrorCode;
            string Error = "Unknown Error  Validate the Error in Switch";
            switch (error)
            {
                case AuthError.Cancelled:
                    Error = "Update User Cancelled";
                    break;
                case AuthError.SessionExpired:
                    Error = "SessionExpiered";
                    break;


            }

            Debug.Log("Error Message:" + " " + Error);
        }
        else
        {
            Debug.Log("User signed in successfully");
            Debug.Log("Phone number: " + user.PhoneNumber);
            Debug.Log("Email ID: " + user.Email);
            Debug.Log("User Name :" + user.DisplayName);
            Debug.Log($"FireBase user Created Succesfully : {user.DisplayName} {user.UserId}");
            UIController.Instance.loading.SetActive(true);
            PlayerPrefs.SetInt("signedIn", 1);
            GameController.Instance.myUserID = user.UserId;
            UIController.Instance.MainMenuScreen.ShowMe();
            UIController.Instance.Loginscreen.HideMe();
            UIController.Instance.RegisterScreen.HideMe();
            CreateNewUserInFireStore();
        }
    }
    #endregion

    #region SignOutUser

    public void SignOutuser()
    {
        auth.SignOut();
    }
    #endregion


    public void CreateNewUserInFireStore()
    {
        DocumentReference docRef = db.Collection("users").Document(FireBaseManager.Instance.user.UserId);
        UserDetails userdetail = new UserDetails();
        userdetail.Name = FireBaseManager.Instance.user.DisplayName;
        userdetail.Wallet = new Wallet()
        {
            bonusAmount = 0, ///Added for Temporay
            amount = 0 ///Added for Temporay
        };


        docRef.SetAsync(userdetail.ToDictionary()).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("new user data creation is failed....");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.Log("Something wrong.... " + task.Exception);
                return;
            }
            GameController.Instance.GetUserDetails();
            Debug.Log("Successfully wallet created....");
        });
    }

    #region UserDetails

    #endregion
}



