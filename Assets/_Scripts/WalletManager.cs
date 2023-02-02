using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Database;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
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
    [SerializeField] private TMP_Text winningAmount;
    [SerializeField] private TMP_Text bonusAdded;
    [SerializeField] private TMP_InputField newAmount;
    [SerializeField] private Button plus;
    [SerializeField] private Button minus;
    [SerializeField] private Button walletButton;
    [SerializeField] private TMP_Text walletAmount;
    [SerializeField] private Toggle fifty, hundered;

    [SerializeField] private Tween Slidetween;
    [SerializeField] private float XSlidePos = 1000;

    // store
    public string userId;
    public int currentAmount;
    public string amount2;

    public RectTransform Popup;
    public Button GobackBtn;

    private void Awake()
    {
        instance = this;
        db = FirebaseFirestore.DefaultInstance;
    }


    private void OnEnable()
    {
        GameController.Instance.OnUserDataUpdated += LoadData;
    }

    private void OnDisable()
    {
        GameController.Instance.OnUserDataUpdated -= LoadData;
    }

    public void LoadData()
    {
        Wallet wallet = GameController.Instance.myData.Wallet;
        amount.text = wallet.amount.ToString();
        bonusAdded.text = wallet.bonusAmount.ToString();
        newAddedAmount.text = wallet.addedAmount.ToString();
        winningAmount.text = wallet.winningAmount.ToString();
    }

    public void OnValueChnage(string value)
    {
        fifty.isOn = value == "50";
        hundered.isOn = value == "100";
        int valuetoAdd = Mathf.Clamp(int.Parse(value), 0, 1000000);
        newAmount.text = valuetoAdd.ToString();
        walletAmount.text = $"ADD <sprite=2>{valuetoAdd}";
    }

    public void OnvalueChange50(string value)
    {
        walletAmount.text = $"ADD <sprite=2>{value}";
    }
    public void OnvalueChange100(string value)
    {
        walletAmount.text = $"ADD <sprite=2>{value}";
    }


    public void OnAddAmount()
    {
        GameController.Instance.AddAmount(int.Parse(newAmount.text));
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
                            //   userName.text = data.Value.ToString();
                            break;
                        }

                    case "Amount":
                        {
                            amount.text = data.Value.ToString();
                            amount2 = amount.text;
                            break;
                        }
                    case "Bonus":
                        {

                            bonusAdded.text = data.Value.ToString();
                            break;
                        }

                    case "AmountAdded":
                        {
                            newAddedAmount.text = data.Value.ToString();
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
            { "AmountAdded", newAmount2.ToString()  },
            { "Amount" , newAmount2.ToString()}
        };

        docref.UpdateAsync(userData).ContinueWithOnMainThread(task =>
        {

            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("Error: " + task.Exception);
            }

            Debug.Log("Succesfully updated.... ");


            newAddedAmount.text = newAmount2.ToString();
            amount.text = newAmount2.ToString();
            amount2 = newAmount2.ToString();
        });
    }

    public override void HideMe()
    {
        UIController.Instance.RemoveFromOpenPages(this);
        if (Slidetween != null)
        {
            Slidetween.Kill();
        }

        Slidetween = Popup.DOScaleY(0, 0.5f).OnComplete(() =>
        {
            walletButton.interactable = true;
            Slidetween = null;
            gameObject.SetActive(false);

        });
    }
    public override void ShowMe()
    {
        this.gameObject.SetActive(true);
        UIController.Instance.AddToOpenPages(this);
        if (Slidetween != null)
            Slidetween.Kill();

        Slidetween = Popup.DOScaleY(1f, 0.5f).OnComplete(() => Slidetween = null);
        walletButton.interactable= false;
        //  LoadUserDetail();
        LoadData();
    }



    public override void OnBack()
    {
        HideMe();
    }
}
