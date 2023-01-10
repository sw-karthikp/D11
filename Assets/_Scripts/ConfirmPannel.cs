using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ConfirmPannel : UIHandler
{
    [SerializeField] private TMP_InputField amountDisplay;
    [SerializeField] private TMP_Text CurrentBalance;

    public RectTransform PanelBg;
    public void AddAmountTo50()
    {
        amountDisplay.text = "50";
        Debug.Log("Add 50 in amount input");
    }

    public void AddAmountTo100()
    {
        amountDisplay.text = "100";
        Debug.Log("Add 100 in amount input");
    }

    public override void HideMe()
    {
        UIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);
    }
    public override void ShowMe()
    {
        PanelBg.DOAnchorPosY(0,0.5f).From();
        UIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);
    }

    public override void OnBack()
    {
        PanelBg.DOAnchorPosY(0,0.5f).From().OnComplete( () => gameObject.SetActive(false));
        HideMe();
    }
}
