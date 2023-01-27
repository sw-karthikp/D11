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
            AutoLogin();

        }
    }
    private void AutoLogin()
    {
        if (user != null)
        {
            Debug.Log("AutoLogin Success");

          
                //UIController.Instance.MainMenuScreen.ShowMe();
                //UIController.Instance.Loginscreen.HideMe();
                //UIController.Instance.RegisterScreen.HideMe();
                // UIController.Instance.loading.SetActive(true);



        }
        else
        {
            //UIController.Instance.LoadingScreen.HideMe();
          //  UIController.Instance.Loginscreen.ShowMe();
        }
    }
    bool signedIn =false;
    public void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        signedIn = PlayerPrefs.GetInt("signedIn") == 0 ? false :true ;
        if (auth.CurrentUser != user)
        {
 
  
            user = auth.CurrentUser;

            if (signedIn)
            {
                DebugHelper.Log($"Signed In : {user.UserId}");
                PlayerPrefs.SetString("userId", user.UserId);
                UIController.Instance.MainMenuScreen.ShowMe();
                UIController.Instance.Loginscreen.HideMe();
                UIController.Instance.RegisterScreen.HideMe();
                UIController.Instance.loading.SetActive(true);
            }
            else
            {
                DebugHelper.Log("Signed Out");
                UIController.Instance.MainMenuScreen.HideMe();
                UIController.Instance.Loginscreen.HideMe();
                UIController.Instance.RegisterScreen.HideMe();
       
            }
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

         
            if (user.IsEmailVerified)
            {
                UIController.Instance.loading.SetActive(true);
                UIController.Instance.MainMenuScreen.ShowMe();
                UIController.Instance.Loginscreen.HideMe();
                UIController.Instance.RegisterScreen.HideMe();
                GameController.Instance.myUserID = user.UserId;
                GameController.Instance.myName = user.DisplayName;

            }
            else
            {

                GameController.Instance.myUserID = user.UserId;
                GameController.Instance.myName = user.DisplayName;
                UIController.Instance.loading.SetActive(true);
                PlayerPrefs.SetInt("signedIn", 1);
                UIController.Instance.MainMenuScreen.ShowMe();
                UIController.Instance.Loginscreen.HideMe();
                UIController.Instance.RegisterScreen.HideMe();
            
            }
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
                GameController.Instance.myName = user.DisplayName;
        
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

        Dictionary<string, object> userData = new Dictionary<string, object>
        {
            { "Name" , FireBaseManager.Instance.user.DisplayName },
            { "Amount" , 100},
            { "Bonus", 100 }
        };

        docRef.SetAsync(userData).ContinueWithOnMainThread(task =>
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

            Debug.Log("Successfully wallet created....");
        });
    }

}



