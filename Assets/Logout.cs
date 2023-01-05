using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logout : UIHandler
{

    public Button goBackHome;
    public Button logoutNow;

    private void Awake()
    {
        goBackHome.onClick.AddListener(() => { OnclickGoBackHome(); });
        logoutNow.onClick.AddListener(() => { OnclickLogoutButton(); });
    }

    public override void HideMe()
    {
        AdminUIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);
    }

    public override void OnBack()
    {
       
    }

    public override void ShowMe()
    {
        AdminUIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);
    }


    public  void OnclickGoBackHome()
    {
        MainMenuController.Instance.mainMenuToggleGroup.allowSwitchOff = true;
        MainMenuController.Instance.mainMenuTogs[0].isOn= true;
        MainMenuController.Instance.mainMenuToggleGroup.allowSwitchOff= false;
    }

    public void OnclickLogoutButton()
    {
        AdminAuthManager.Instance.SignOutuser();
        AdminUIController.Instance.MainMenuScreen.HideMe();
        AdminUIController.Instance.Loginscreen.ShowMe();
    }

}
