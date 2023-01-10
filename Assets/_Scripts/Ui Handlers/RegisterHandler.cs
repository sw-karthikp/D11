using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class RegisterHandler : UIHandler
{

    public TMP_InputField _userName;
    public TMP_InputField _emailId;
    public TMP_InputField _passWord;
    public TMP_InputField _mobileNumber;
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
        UIController.Instance.AddToOpenPages(this);
       this.gameObject.SetActive(true);
        BG.DOAnchorPosX(0, 0.5f);
    }
  
    public override void OnBack()
    {
        BG.DOAnchorPosX(1450, 0.5f).OnComplete(() => gameObject.SetActive(false));
        UIController.Instance.RemoveFromOpenPages(this);
    }


    public void OnClickRegisterButton()
    {

        loadingtxt.SetActive(false);
        loadingAnim.SetActive(true);
        if (string.IsNullOrWhiteSpace(_userName.text))
        {

            errorTxt.text = "UserName can't be  Empty";
            loadingtxt.SetActive(true);
            loadingAnim.SetActive(false);
            return;
        }
        else if((string.IsNullOrWhiteSpace(_emailId.text)))
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
        else if ((string.IsNullOrWhiteSpace(_mobileNumber.text)))
        {
            errorTxt.text = "mobileNumber can't be  Empty";
            loadingtxt.SetActive(true);
            loadingAnim.SetActive(false);
            return;
        }

        StartCoroutine(FireBaseManager.Instance.RegisterLogic(_userName.text, _emailId.text, _passWord.text, _mobileNumber.text,errorTxt, loadingtxt.gameObject, loadingAnim.gameObject));        
    }
}
