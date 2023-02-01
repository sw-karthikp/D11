using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Logout : UIHandler
{

    public Button goBackHome;
    public Button logoutNow;
    public GameObject move;
    public Ease ease;

    private void Awake()
    {
        goBackHome.onClick.AddListener(() => { OnclickLogoutButton(); });
        logoutNow.onClick.AddListener(() => {  OnclickGoBackHome(); });
    }

    public override void HideMe()
    {
        UIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);
        move.GetComponent<RectTransform>().DOAnchorPos(new Vector3(0f, -706, 0f), 0.2f).SetEase(ease);
    }

    public override void OnBack()
    {
       
    }

    public override void ShowMe()
    {
        UIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);
        move.GetComponent<RectTransform>().DOAnchorPos(new Vector3(0f, 439.5f, 0f), 0.2f).SetEase(ease);
    }


    public  void OnclickGoBackHome()
    {
        HideMe();
    }

    public void OnclickLogoutButton()
    {
        FireBaseManager.Instance.SignOutuser();
        UIController.Instance.MainMenuScreen.HideMe();
        UIController.Instance.Loginscreen.ShowMe();
    }

}
