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

    [Header("FireBase")]
    public FirebaseAuth auth;
    public FirebaseUser user;
    public FirebaseFirestore db;

    private void Awake()
    {
 
        Instance = this;
        db = FirebaseFirestore.DefaultInstance;
        isFirstTime = true;
        auth = FirebaseAuth.DefaultInstance;
        StartCoroutine(CheckAutoLogin());
    }

    private IEnumerator CheckAutoLogin()
    {
        user = auth.CurrentUser;
        yield return new WaitForEndOfFrame();

        if (user != null)
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
    bool  signedIn =false;
    public void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            user = auth.CurrentUser;
        }
    }

    #region LoginLogic
    public IEnumerator LoginLogic(string email, string password,TMP_Text errormsg,GameObject loadingtxt,GameObject loadinganim)
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

    #region RegisterLogic
    public IEnumerator RegisterLogic(string username, string email, string password, string mobile,TMP_Text errormsg, GameObject loadingtxt, GameObject loadinganim)
    {
        Debug.Log("called");
        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: () => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            FirebaseException firebaseException = (FirebaseException)registerTask.Exception.GetBaseException();
            AuthError error = (AuthError)firebaseException.ErrorCode;
            string Error = "Unknown Error  Validate the Error in Switch";
            switch (error)
            {
                case AuthError.InvalidEmail:
                    Error = "Invalid email";
                    break;
                case AuthError.EmailAlreadyInUse:
                    Error = "Email already in use";
                    break;
                case AuthError.WeakPassword:
                    Error = "Weakpassword";
                    break;
                case AuthError.MissingEmail:
                    Error = "Please enter your email";
                    break;
                case AuthError.MissingPassword:
                    Error = "Please enter your password";
                    break;
                case AuthError.Failure:
                    Error = "Failure";
                    break;

            }

            Debug.Log("Error Message:" + " " + Error);
            errormsg.text = Error;
            loadingtxt.SetActive(true);
            loadinganim.SetActive(false);
            StartCoroutine(RegisterHandler.Instance.clear());
            //UIController.Instance.LoadingScreen.HideMe();

        }
        else
        {
            user = auth.CurrentUser;
            UserProfile profile = new UserProfile
            {
                DisplayName = username,
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
                errormsg.text = Error;
                StartCoroutine(RegisterHandler.Instance.clear());
                //UIController.Instance.LoadingScreen.HideMe();
                loadingtxt.SetActive(true);
                loadinganim.SetActive(false);



            }
            else
            {
                Debug.Log($"FireBase user Created Succesfully : {user.DisplayName} {user.UserId}");

                UIController.Instance.loading.SetActive(true);
                PlayerPrefs.SetInt("signedIn", 1);
                GameController.Instance.myUserID = user.UserId;
                GameController.Instance.myData.Name = user.DisplayName;
        
                //UIController.Instance.LoadingScreen.HideMe();
                UIController.Instance.MainMenuScreen.ShowMe();
                UIController.Instance.Loginscreen.HideMe();
                UIController.Instance.RegisterScreen.HideMe();
                CreateNewUserInFireStore();



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

    // 
    public void CreateNewUserInFireStore()
    {
        //string userId = FireBaseManager.Instance.user.UserId;
        //Debug.Log(userId);
        DocumentReference docRef = db.Collection("users").Document(FireBaseManager.Instance.user.UserId);

        UserDetails userdetail = new UserDetails();
        userdetail.Name = FireBaseManager.Instance.user.DisplayName;
        userdetail.Wallet = new Wallet()
        {
            bonusAmount = 100,
            amount = 100
        };


        //Dictionary<string, object> userData = new Dictionary<string, object>
        //{
        //    { "Name" , FireBaseManager.Instance.user.DisplayName },
        //    { "Amount" , 100},
        //    { "Bonus", 100 }
        //};

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



