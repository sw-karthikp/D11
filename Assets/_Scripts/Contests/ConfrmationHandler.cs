using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfrmationHandler : UIHandler
{
    public TMP_Text amountNeeded;

    public TMP_Text bonusAmountAdded;

    public TMP_Text amountToPay;

    float totalEntry,  bonusAmountAddedValue,amountToPayValue;


    void CalculateAmountToPay()
    {
        totalEntry = GameController.Instance.currentPools.Entry;
        float  bonusToAdd = totalEntry * 0.1f;
        bonusAmountAddedValue = (Mathf.Floor(Mathf.Clamp(bonusToAdd, 0, bonusToAdd)));
        amountToPayValue = totalEntry - bonusAmountAddedValue;

        amountNeeded.text = "<sprite=0> <size=42>"+totalEntry.ToString();
        bonusAmountAdded.text = "-<sprite=0> <size=42>" + bonusAmountAddedValue.ToString();
        amountToPay.text = "<sprite=0> <size=42>" + amountToPayValue.ToString();

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
