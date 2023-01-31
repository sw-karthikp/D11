using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Linq;
using System;
using UnityEngine.UI;

public class ConfirmPannel : UIHandler
{
    public TMP_Text balanceAvailable;

    public TMP_Text amountNeeded;

    public TMP_Text bonusAmountAdded;

    public TMP_Text amountToPay, errorMessage;

    float totalEntry, bonusAmountAddedValue, amountToPayValue;

    [SerializeField] private TMP_InputField newAmount;
    [SerializeField] private TMP_Text walletAmount;
    [SerializeField] private Toggle fifty, hundered;

    public RectTransform PanelBg;

    public bool lowBalance;

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
        float bonusToAdd = Mathf.Floor(Mathf.Clamp(totalEntry * 0.1f, 0, 25));
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
        balanceAvailable.text = "<sprite=0> <size=42>" + GameController.Instance.myData.Wallet.amount.ToString();
        amountNeeded.text = "<sprite=0> <size=42>" + totalEntry.ToString();
        bonusAmountAdded.text = "-<sprite=0> <size=42>" + bonusAmountAddedValue.ToString();
        amountToPay.text = "<sprite=0> <size=42>" + amountToPayValue.ToString();
        OnValueChnage("0");
    }
    int valuetoAdd = 0;
    public void OnValueChnage(string value)
    {
        fifty.isOn = value == "50";
        hundered.isOn = value == "100";
        valuetoAdd = 0;
        valuetoAdd = Mathf.Clamp(int.Parse(value), (int)amountToPayValue, 1000000);
        newAmount.text = valuetoAdd.ToString();
        walletAmount.text = $"ADD <sprite=2>{valuetoAdd}";
    }

    public void OnAddAmount()
    {
        GameController.Instance.AddAmount(valuetoAdd,0,() => 
        {
            HideMe();
        },
        () => 
        {
            StopCoroutine("DisableError");
            StartCoroutine("DisableError");
            errorMessage.text = "Error Try Again";
        });
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
        if (lowBalance)
        {
            CalculateAmountToPay();
            errorMessage.text = "";
        }
        UIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);
        
    }

    public override void OnBack()
    {
        PanelBg.DOAnchorPosY(0,0.5f).From().OnComplete( () => gameObject.SetActive(false));
        HideMe();
    }
}
