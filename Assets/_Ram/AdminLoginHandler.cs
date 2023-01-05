using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdminLoginHandler : UIHandler
{
    public TMP_InputField _emailId;
    public TMP_InputField _passWord;
    //public TMP_Text errorTxt;

    public override void HideMe()
    {
        AdminUIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);
    }
    public override void ShowMe()
    {
        AdminUIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);
    }

    public override void OnBack()
    {

    }

    public void OnClickLoginButton()
    {

        if ((string.IsNullOrWhiteSpace(_emailId.text)))
        {
            //errorTxt.text = "Email ID can't be  Empty";
            return;
        }
        else if ((string.IsNullOrWhiteSpace(_passWord.text)))
        {
            //errorTxt.text = "passWord can't be  Empty";
            return;
        }


        StartCoroutine(AdminAuthManager.Instance.AdminLogin(_emailId.text, _passWord.text));// errorTxt.text);//);
    }
}
