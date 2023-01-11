using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoginHandler : UIHandler
{
    public TMP_InputField _emailId;
    public TMP_InputField _passWord;
    public TMP_Text errorTxt;
    public RectTransform BG;
    public GameObject loadingtxt;
    public GameObject loadingAnim;


    public override void HideMe()
    {       
        UIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);
    }  
    public override void ShowMe()
    {
      
       this.gameObject.SetActive(true);
        UIController.Instance.AddToOpenPages(this);
        BG.DOAnchorPosX(0, 0.5f);
    }  
    public override void OnBack()
    {
        BG.DOAnchorPosX(1450, 0.5f).OnComplete(() => gameObject.SetActive(false));
        UIController.Instance.RemoveFromOpenPages(this);
    }
    public void OnClickLoginButton()
    {
        loadingtxt.SetActive(false);
        loadingAnim.SetActive(true);
        if ((string.IsNullOrWhiteSpace(_emailId.text)))
        {
            errorTxt.text = "Email ID can't be  Empty";
            loadingtxt.SetActive(true);
            loadingAnim.SetActive(false);
            return;
        }
        else if ((string.IsNullOrWhiteSpace(_passWord.text)))
        {
            errorTxt.text = "passWord can't be  Empty";
            loadingtxt.SetActive(true);
            loadingAnim.SetActive(false);
            return;
        }


        StartCoroutine(FireBaseManager.Instance.LoginLogic(_emailId.text, _passWord.text, errorTxt,loadingtxt.gameObject,loadingAnim.gameObject));

    
    }
}
