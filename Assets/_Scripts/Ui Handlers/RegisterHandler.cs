using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
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
    public static RegisterHandler Instance;
    public Sprite[] pics;
    public Image register;
    public TMP_Text registerTxt;

    private void Awake()
    {
        Instance= this;
        _userName.onEndEdit.AddListener(delegate { spriteswap(); });
        _emailId.onEndEdit.AddListener(delegate { spriteswap(); });
        _passWord.onEndEdit.AddListener(delegate { spriteswap(); });
        _mobileNumber.onEndEdit.AddListener(delegate { spriteswap(); });


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

            errorTxt.text = "UserName can't be empty";
            loadingtxt.SetActive(true);
            loadingAnim.SetActive(false);
            StartCoroutine(clear());
            return;
        }
        else if((string.IsNullOrWhiteSpace(_emailId.text)))
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
        else if ((string.IsNullOrWhiteSpace(_mobileNumber.text)))
        {
            errorTxt.text = "Mobilenumber can't be empty";
            loadingtxt.SetActive(true);
            loadingAnim.SetActive(false);
            StartCoroutine(clear());
            return;
        }

        StartCoroutine(FireBaseManager.Instance.RegisterLogic(_userName.text, _emailId.text, _passWord.text, _mobileNumber.text,errorTxt, loadingtxt.gameObject, loadingAnim.gameObject));      
        
    }

    public void spriteswap()
    {
        if(string.IsNullOrWhiteSpace(_userName.text) || string.IsNullOrWhiteSpace(_emailId.text) || string.IsNullOrWhiteSpace(_passWord.text) || string.IsNullOrWhiteSpace(_mobileNumber.text))
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
