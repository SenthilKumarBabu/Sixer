using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPage : UIPage
{
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private RectTransform popupRectTransform;
    [SerializeField] private GameObject fullNameObj,emailObj,passObj,confirmPassObj;
    [SerializeField] private TMP_InputField fullNameIf, emailIf, passIf, confirmPassIf;
    [SerializeField] private Button signInButton, signUpButton,switchToSignInButton, switchToSignUpButton,forgetPasswordButton,loginAsGuestButton;
    [SerializeField] private TMP_Text errorText;
    
    private LoginPageStatus _status;
    private AuthWr _authWr;
    
    public enum LoginPageStatus
    {
        SignInPage,
        SignUpPage
    }
    
    private void Awake()
    {
        _status = LoginPageStatus.SignInPage;
        _authWr = new AuthWr();
        signInButton.onClick.AddListener(SignInButtonClicked);
        signUpButton.onClick.AddListener(SignUpButtonClicked);
        switchToSignInButton.onClick.AddListener(SwitchToSignInButtonClicked);
        switchToSignUpButton.onClick.AddListener(SwitchToSignUpButtonClicked);
        forgetPasswordButton.onClick.AddListener(ForgetPasswordButtonClicked);
        loginAsGuestButton.onClick.AddListener(LoginAsGuestButtonClicked);
    }

    public override void OnShow(object data = null)
    {
        if (data is LoginPageData pageData)
        {
            _status = pageData.status;
        }
        else
        {
            Debug.LogWarning("LoginPage: Missing or incorrect data!");
        }

        headerText.text = _status == LoginPageStatus.SignInPage ? "SIGN IN" : "SIGN UP";
        fullNameObj.gameObject.SetActive(_status == LoginPageStatus.SignUpPage);
        confirmPassObj.gameObject.SetActive(_status == LoginPageStatus.SignUpPage);
        signInButton.gameObject.SetActive(_status == LoginPageStatus.SignInPage);
        signUpButton.gameObject.SetActive(_status == LoginPageStatus.SignUpPage);
        switchToSignInButton.gameObject.SetActive(_status == LoginPageStatus.SignUpPage);
        switchToSignUpButton.gameObject.SetActive(_status == LoginPageStatus.SignInPage);
        forgetPasswordButton.gameObject.SetActive(_status == LoginPageStatus.SignInPage);
        errorText.gameObject.SetActive(false);
        
        base.OnShow(data);
        LayoutRebuilder.ForceRebuildLayoutImmediate(popupRectTransform);
    }

    private async void SignInButtonClicked()
    {
        var loginData = await _authWr.AuthLogin(emailIf.text,passIf.text);

        if (loginData == null)
        {
            errorText.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Opening Main Page");
        }
    }

    private async void SignUpButtonClicked()
    {
        var registerData =  await _authWr.AuthRegister(emailIf.text,fullNameIf.text,passIf.text);
        
        if (registerData == null)
        {
            errorText.gameObject.SetActive(true);
        }
        else
        {
            UIManager.Instance.OpenPage<OtpPage>();
        }
    }

    private void SwitchToSignInButtonClicked()
    {
        UIManager.Instance.OpenPage<LoginPage>(new LoginPageData(status: LoginPage.LoginPageStatus.SignInPage));
    }

    private void SwitchToSignUpButtonClicked()
    {
        UIManager.Instance.OpenPage<LoginPage>(new LoginPageData(status: LoginPage.LoginPageStatus.SignUpPage));
    }

    private void ForgetPasswordButtonClicked()
    {
        
    }

    private void LoginAsGuestButtonClicked()
    {
        UIManager.Instance.OpenPage<MainMenuPage>();
    }

    [ContextMenu("AutoFillLogin")]
    public void AutoFillLogin()
    {
        emailIf.text = "jbsenthilkumar209@gmail.com";
        passIf.text = "Asdf@1234";
    }
    
    [ContextMenu("AutoFillRegister")]
    public void AutoFillRegister()
    {
        fullNameIf.text = "Chris0";
        emailIf.text = "jbsenthilkumar209+0@gmail.com";
        passIf.text = "Asdf@1234";
        confirmPassIf.text = "Asdf@1234";
    }
}

[Serializable]
public class LoginPageData
{
    public LoginPage.LoginPageStatus status;

    public LoginPageData(LoginPage.LoginPageStatus status)
    {
        this.status = status;
    }
}
