using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Database;
using UnityEngine.UI;
using System;

public class WalletManager : UIHandler
{
    public static WalletManager instance;

    public FirebaseFirestore db;

    private string currentUserId;

    [Header("User Details")]
    [SerializeField] private TMP_InputField userName;
    [SerializeField] private TMP_Text amount;
    [SerializeField] private TMP_Text newAddedAmount;
    [SerializeField] private TMP_InputField newAmount;
    [SerializeField] private Button plus;
    [SerializeField] private Button minus;

    // store
    public string userId;
    public int currentAmount;
    public string amount2;



    private void Awake()
    {
        instance = this;

        db = FirebaseFirestore.DefaultInstance;
    }

    public void CreateWalletForNewUser()
    {
        //string userId = FireBaseManager.Instance.user.UserId;
        //Debug.Log(userId);
        DocumentReference docRef = db.Collection("users").Document(FireBaseManager.Instance.user.UserId);

        Dictionary<string, object> userData = new Dictionary<string, object>
        {
            { "Name" , userName.text },
            { "Amount" , 100 },
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

    public void LoadUserDetail()
    {

        DocumentReference colRef = db.Collection("users").Document(FireBaseManager.Instance.user.UserId);
        

        colRef.Listen(task =>
        {
            Dictionary<string, object> valuePairs = task.ToDictionary();

            Debug.Log("User Details loading....");

            foreach (KeyValuePair<string, object> data in valuePairs)
            {
                switch (data.Key)
                {
                    case "Name":
                        {
                            userName.text = data.Value.ToString();
                            break;
                        }

                    case "Amount":
                        {
                            amount.text = data.Value.ToString();
                            amount2 = amount.text;
                            break;
                        }

                    default:
                        {
                            Debug.LogError("Data is not matched...");
                            break;
                        }
                }
            }
        });
    }

    public void UpdataWalletValue(bool value)
    {
        int newAmount2 = int.Parse(amount2);
        newAddedAmount.text = newAmount.text;

        if (value)
        {
            newAmount2 += int.Parse(newAmount.text);
        }
        else
        {
            newAmount2 -= int.Parse(newAmount.text);
        }

        DocumentReference docref = db.Collection("users").Document(FireBaseManager.Instance.auth.CurrentUser.UserId);
        Dictionary<string, object> userData = new Dictionary<string, object>
        {
            { "Amount", newAmount2.ToString()}
        };

        docref.UpdateAsync(userData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("Error: " + task.Exception);
            }

            Debug.Log("Succesfully updated.... ");
        });
    }

    public override void HideMe()
    {
        UIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);
    }
    public override void ShowMe()
    {
        UIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);
    }

    public override void OnBack()
    {

    }
}
