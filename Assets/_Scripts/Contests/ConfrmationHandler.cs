using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfrmationHandler : UIHandler
{
    public TMP_Text amountNeeded;

    public TMP_Text bonusAmountAdded;

    public TMP_Text amountToPay;


    void CalculateAmountToPay()
    {

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
        HideMe();
    }
}
