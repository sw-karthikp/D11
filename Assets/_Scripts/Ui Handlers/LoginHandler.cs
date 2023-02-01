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
    public Sprite[] pics;
    public Image register;
    public TMP_Text registerTxt;

    public static LoginHandler Instance;

    private void Awake()
    {
        _emailId.onEndEdit.AddListener(delegate { spriteswap(); });
        _passWord.onEndEdit.AddListener(delegate { spriteswap(); });
    }
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
            errorTxt.text = "Email ID can't be empty";
            loadingtxt.SetActive(true);
            loadingAnim.SetActive(false);
            StartCoroutine(clear());
            return;
        }
        else if ((string.IsNullOrWhiteSpace(_passWord.text)))
        {
            errorTxt.text = "Password can't be empty";
            loadingtxt.SetActive(true);
            loadingAnim.SetActive(false);
            StartCoroutine(clear());
            return;
        }


        StartCoroutine(FireBaseManager.Instance.LoginLogic(_emailId.text, _passWord.text, errorTxt,loadingtxt.gameObject,loadingAnim.gameObject));

    
    }
    public void spriteswap()
    {
        if ( string.IsNullOrWhiteSpace(_emailId.text) || string.IsNullOrWhiteSpace(_passWord.text) )
        {
            register.sprite = pics[0];
            registerTxt.color = Color.grey;
        }
        else
        {
            register.sprite = pics[1];
            registerTxt.color = Color.white;
        }
    }
    public IEnumerator clear()
    {
        yield return new WaitForSeconds(0.8f);
        errorTxt.text = "";
    }
}
