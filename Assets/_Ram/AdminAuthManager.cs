using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Functions;
using Firebase.Database;
using Firebase.Extensions;

public class AdminAuthManager : MonoBehaviour
{

    public static AdminAuthManager Instance;


    [Header("FireBase")]
    public FirebaseAuth auth;
    public FirebaseUser user;


    private void Awake()
    {
        Instance = this;

    }


    private void Start()
    {

        StartCoroutine(CheckAndFixDependancies());
    }

    private IEnumerator CheckAndFixDependancies()
    {
        var CheckAndFixDependanciesTask = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(predicate: () => CheckAndFixDependanciesTask.IsCompleted);
        var dependancyStatus = CheckAndFixDependanciesTask.Result;

        if (dependancyStatus == DependencyStatus.Available)
        {

            InitializeFirebase();
        }
        else
        {
            Debug.LogError($"Could not resolve all FireBase dependencies: {dependancyStatus}");
        }

    }

    public void InitializeFirebase()
    {

        auth = FirebaseAuth.DefaultInstance;


        //if (!ClonesManager.IsClone())
        //{
        //    StartCoroutine(CheckAutoLogin());
        //}



        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }


    private IEnumerator CheckAutoLogin()
    {
        yield return new WaitForEndOfFrame();

        if (user != null)
        {
            var reloadUserTask = user.ReloadAsync();
            yield return new WaitUntil(predicate: () => reloadUserTask.IsCompleted);
            //UIController.Instance.LoadingScreen.ShowMe();
           /// AutoLogin();

        }
    }
    private void AutoLogin()
    {
        if (user != null)
        {
            Debug.Log("AutoLogin Success");

            if (user.IsEmailVerified)
            {
                ///Temp
                ///
                //UIController.Instance.LoadingScreen.HideMe();
                UIController.Instance.MainMenuScreen.ShowMe();
                UIController.Instance.Loginscreen.HideMe();
                UIController.Instance.RegisterScreen.HideMe();
            }
            else
            {
                ///Temp
                ///
                //UIController.Instance.LoadingScreen.HideMe();
                UIController.Instance.MainMenuScreen.ShowMe();
                UIController.Instance.Loginscreen.HideMe();
                UIController.Instance.RegisterScreen.HideMe();

            }


        }
        else
        {
            //UIController.Instance.LoadingScreen.HideMe();
            UIController.Instance.Loginscreen.ShowMe();
        }
    }
    bool signedIn;
    public void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        signedIn = PlayerPrefs.GetInt("signedIn") == 0 ? false : true;
        if (auth.CurrentUser != user)
        {


            user = auth.CurrentUser;

            if(signedIn)
            {
                Debug.Log(user.UserId);

                AdminUIController.Instance.MainMenuScreen.ShowMe();
                AdminUIController.Instance.Loginscreen.HideMe();
            }

            //if (signedIn)
            //{
            //    Debug.Log($"Signed In : {user.DisplayName}");
            //    UIController.Instance.MainMenuScreen.ShowMe();
            //    UIController.Instance.Loginscreen.HideMe();
            //    UIController.Instance.RegisterScreen.HideMe();
            //}
            //else
            //{
            //    Debug.Log("Signed Out");
            //    UIController.Instance.MainMenuScreen.HideMe();
            //    UIController.Instance.Loginscreen.HideMe();
            //    UIController.Instance.RegisterScreen.HideMe();
            //}
        }


    }


    #region LoginLogic
    public IEnumerator AdminLogin(string email, string password)//, string errormsg)
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
            string Error = "Unknown Error  Validate the Error in Switch";
            switch (error)
            {
                case AuthError.MissingEmail:
                    Error = "please Enter your Email";
                    break;
                case AuthError.MissingPassword:
                    Error = "please Enter your Password";
                    break;
                case AuthError.InvalidEmail:
                    Error = "Invalid Email";
                    break;
                case AuthError.WrongPassword:
                    Error = "Incorrect  Password";
                    break;
                case AuthError.UserNotFound:
                    Error = "Acccount Does  not Exsit ";
                    break;

            }

            Debug.Log("Error Message:" + " " + Error);
            //errormsg = Error;
        }
        else
        {

            if (user.IsEmailVerified)
            {

                UIController.Instance.MainMenuScreen.ShowMe();
                //UIController.Instance.Loginscreen.HideMe();
                //UIController.Instance.RegisterScreen.HideMe();

            }
            else
            {

                Debug.Log("Sign-In");

                PlayerPrefs.SetInt("signedIn", 1);
                AdminUIController.Instance.MainMenuScreen.ShowMe();
                AdminUIController.Instance.Loginscreen.HideMe();
                //UIController.Instance.Loginscreen.HideMe();
                //UIController.Instance.RegisterScreen.HideMe();

            }
        }
    }

    #endregion

    #region SignOutUser

    public void SignOutuser()
    {
        auth.SignOut();
        PlayerPrefs.SetInt("signedIn", 0);

        //UIController.Instance.LoadingScreen.HideMe();
    }
    #endregion



}


