using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBaseInit : MonoBehaviour
{
    Firebase.FirebaseApp app;
    
    void Start()
    {
        FireBaseCheck();
    }

    void FireBaseCheck()
    {   
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;               
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format
                    (
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }
}