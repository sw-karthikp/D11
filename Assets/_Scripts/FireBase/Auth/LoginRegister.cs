

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginRegister : MonoBehaviour
{
    Firebase.Auth.FirebaseAuth auth;

    // Start is called before the first frame update
    void Start()
    {
        InitializeFirebase();
        // CreateUser();
        SignInUser("hello@gmail.com","123456");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }


    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        DebugHelper.Log("Changed");
        //if (auth.CurrentUser != user)
        //{
        //    bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
        //    if (!signedIn && user != null)
        //    {
        //        DebugLog("Signed out " + user.UserId);
        //    }
        //    user = auth.CurrentUser;
        //    if (signedIn)
        //    {
        //        DebugLog("Signed in " + user.UserId);
        //        displayName = user.DisplayName ?? "";
        //        emailAddress = user.Email ?? "";
        //        photoUrl = user.PhotoUrl ?? "";
        //    }
        //}
    }

    void CreateUser(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith
            (task => 
            {
            if (task.IsCanceled)
            {
                DebugHelper.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                DebugHelper.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception.Message);
                return;
            }
            Firebase.Auth.FirebaseUser newUser = task.Result;
                DebugHelper.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }


    void SignInUser(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => 
        {
            if (task.IsCanceled)
            {
                DebugHelper.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                DebugHelper.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);

                return;
            }
            Firebase.Auth.FirebaseUser newUser = task.Result;
            DebugHelper.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

}
