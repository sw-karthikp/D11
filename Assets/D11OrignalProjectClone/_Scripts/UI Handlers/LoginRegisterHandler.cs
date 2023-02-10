using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginRegisterHandler : UIHandler
{
	public Button submitBtnEmail;
	public GameObject Register;
	public GameObject login;
	public TMP_InputField emailField, passwordField;
    public TMP_InputField emailFieldRegister, mobileFieldRegister, usernameFieldRegister, passwordFieldRegister, passwordReEnterFieldRegister;


	bool registerUser = false;
	public bool autoLogin = false;
	bool isMobileNoVerified = false;

	public override void ShowMe()
    {
        throw new System.NotImplementedException();
    }

    public override void HideMe()
    {
        throw new System.NotImplementedException();
    }

    public override void OnBack()
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
		usernameFieldRegister.onValidateInput += delegate (string input, int charIndex, char addedChar)
		{

			if (Regex.IsMatch(addedChar.ToString(), @"[a-zA-Z0-9]"))
			{
				return addedChar;
			}
			return SetToUpper('\0');
		};
		emailField.onValidateInput += delegate (string input, int charIndex, char addedChar)
		{

			if (Regex.IsMatch(addedChar.ToString(), @"[a-z0-9.@]"))
			{
				if (addedChar == '@')
					if (input.Contains("@"))
						return SetToUpper('\0');
				return addedChar;
			}
			return SetToUpper('\0');
		};
		emailFieldRegister.onValidateInput += delegate (string input, int charIndex, char addedChar)
		{

			if (Regex.IsMatch(addedChar.ToString(), @"[a-z0-9.@]"))
			{
				if (addedChar == '@')
					if (input.Contains("@"))
						return SetToUpper('\0');
				return addedChar;
			}
			return SetToUpper('\0');
		};
		passwordFieldRegister.onValidateInput += delegate (string input, int charIndex, char addedChar)
		{

			if (Regex.IsMatch(addedChar.ToString(), @"[a-zA-Z0-9!@#$%^&*()_+\-=\[\]{};':\\|,.<>\/?]"))
				return addedChar;
			return SetToUpper('\0');
		};
		passwordReEnterFieldRegister.onValidateInput += delegate (string input, int charIndex, char addedChar)
		{

			if (Regex.IsMatch(addedChar.ToString(), @"[a-zA-Z0-9!@#$%^&*()_+\-=\[\]{};':\\|,.<>\/?]"))
				return addedChar;
			return SetToUpper('\0');
		};
		mobileFieldRegister.onEndEdit.AddListener((value) => { isMobileNoVerified = false; });
	}

	public char SetToUpper(char c)
	{
		return char.ToUpper(c);
	}

    private void OnEnable()
    {
		autoLogin = false;
		registerUser = false;
		//GameController.Instance.onLocationGranted += AutoLogin;
	}

	public void AutoLogin()
    {
		DebugHelper.Log("AutoLogin Called");
    }

	private void OnDisable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSignInButtonClicked()
    {

    }

    
}
