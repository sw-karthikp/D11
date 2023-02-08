using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ConfrmationHandler : UIHandler
{
    public TMP_Text amountNeeded;

    public TMP_Text bonusAmountAdded;

    public TMP_Text amountToPay,errorMessage;

    float totalEntry,  bonusAmountAddedValue,amountToPayValue;

    public ConfirmPannel lowBalancePanal;

    private void OnEnable()
    {
        GameController.Instance.OnUserDataUpdated += CalculateAmountToPay;
        errorMessage.text = "";
    }

    private void OnDisable()
    {
        GameController.Instance.OnUserDataUpdated -= CalculateAmountToPay;
    }


    void CalculateAmountToPay()
    {
        Pools value;
        try
        {
            value = GameController.Instance.matchpool.First(X => X.Value.MatchID == GameController.Instance.CurrentMatchID).Value.Pools.Values.First(x => x.PoolID == GameController.Instance.CurrentPoolID);
        }
        catch (Exception e)
        {
            return;
        }
        GameController.Instance.currentPools = value;
        totalEntry = GameController.Instance.currentPools.Entry;
        float  bonusToAdd = Mathf.Floor(Mathf.Clamp(totalEntry * 0.1f, 0, 25));
        bonusAmountAddedValue = 0;
        if (GameController.Instance.myData.Wallet.bonusAmount >= bonusToAdd)
        {
            bonusAmountAddedValue = bonusToAdd;
        }
        else
        {
            bonusAmountAddedValue = GameController.Instance.myData.Wallet.bonusAmount;
        }

        
        amountToPayValue = totalEntry - bonusAmountAddedValue;

        amountNeeded.text = "<sprite=0> <size=42>"+totalEntry.ToString();
        bonusAmountAdded.text = "-<sprite=0> <size=42>" + bonusAmountAddedValue.ToString();
        amountToPay.text = "<sprite=0> <size=42>" + amountToPayValue.ToString();

    }

    public void OnJoinClicked()
    {
        UIController.Instance.loading.SetActive(true);
        if (GameController.Instance.myData.Wallet.addedAmount >= amountToPayValue)
        {
            GameController.Instance.SubtractAmount((int)amountToPayValue,(int)bonusAmountAddedValue,(() =>
            {
                if (ContestHandler.Instance.isContest)
                {
                    FirebaseDatabase mDatabase = FirebaseDatabase.DefaultInstance;
                    string TeamId = "Team" + (GameController.Instance.selectedMatches.Count > 0 ? GameController.Instance.selectedMatches.ContainsKey(GameController.Instance.CurrentMatchID.ToString()) ? GameController.Instance.selectedMatches[GameController.Instance.CurrentMatchID.ToString()].SelectedTeam.Count : 1 : 1);
                    SelectedPoolID selectedPool = new SelectedPoolID() { PoolID = GameController.Instance.CurrentPoolID, TeamID = TeamId };
                    string selectedPoolKey = mDatabase.RootReference.Child("PlayerMatches").Child($"{GameController.Instance.myUserID}").Child($"{GameController.Instance.CurrentMatchID}").Child("SelectedPools").Push().Key;
                    mDatabase.RootReference.Child("PlayerMatches").Child($"{GameController.Instance.myUserID}").Child($"{GameController.Instance.CurrentMatchID}").Child("SelectedPools").Child($"{selectedPoolKey}").SetRawJsonValueAsync(JsonUtility.ToJson(selectedPool));

                    HideMe();
                    UIController.Instance.loading.SetActive(false);
                    ContestHandler.Instance.isContest = false;
                } 
                else if(MyTeam.Instance != null && MyTeam.Instance.isMySelectedTeam)
                {

                    FirebaseDatabase mDatabase = FirebaseDatabase.DefaultInstance;
                    string TeamId = MyTeam.Instance.teamNameSelected;
                    SelectedPoolID selectedPool = new SelectedPoolID() { PoolID = GameController.Instance.CurrentPoolID, TeamID = TeamId };
                    string selectedPoolKey = mDatabase.RootReference.Child("PlayerMatches").Child($"{GameController.Instance.myUserID}").Child($"{GameController.Instance.CurrentMatchID}").Child("SelectedPools").Push().Key;
                    mDatabase.RootReference.Child("PlayerMatches").Child($"{GameController.Instance.myUserID}").Child($"{GameController.Instance.CurrentMatchID}").Child("SelectedPools").Child($"{selectedPoolKey}").SetRawJsonValueAsync(JsonUtility.ToJson(selectedPool));

                    HideMe();
                    UIController.Instance.loading.SetActive(false);
                    MyTeam.Instance.isMySelectedTeam = false;
                    MyTeam.Instance.gameObject.SetActive(false);
                }
                else
                {
                    captainSelection.Instance.SaveData();
                    captainSelection.Instance.HideMe();
                    HideMe();
                    UIController.Instance.loading.SetActive(false);
                }


            }),
            () =>
            {
                UIController.Instance.loading.SetActive(false);
                StopCoroutine("DisableError");
                StartCoroutine("DisableError");
                errorMessage.text = "Error Try Again";

            });
            
        }
        else
        {
            UIController.Instance.loading.SetActive(false);
            lowBalancePanal.ShowMe();
          
        }
    }


    IEnumerator DisableError()
    {
        yield return new WaitForSeconds(2f);
        errorMessage.text = "";
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
        CalculateAmountToPay();
    }

    public override void OnBack()
    {
        HideMe();
    }
}
