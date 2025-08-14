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
    [SerializeField] private Button signInButton, signUpButton,switchToSignInButton, switchToSignUpButton,forgetPasswordButton;
    
    private LoginPageStatus _status;
    
    public enum LoginPageStatus
    {
        SignInPage,
        SignUpPage
    }
    
    private void Awake()
    {
        _status = LoginPageStatus.SignInPage;
        signInButton.onClick.AddListener(SignInButtonClicked);
        signUpButton.onClick.AddListener(SignUpButtonClicked);
        switchToSignInButton.onClick.AddListener(SwitchToSignInButtonClicked);
        switchToSignUpButton.onClick.AddListener(SwitchToSignUpButtonClicked);
        forgetPasswordButton.onClick.AddListener(ForgetPasswordButtonClicked);
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
        
        base.OnShow(data);
        LayoutRebuilder.ForceRebuildLayoutImmediate(popupRectTransform);
    }

    private void SignInButtonClicked()
    {
        UIManager.Instance.OpenPage<OtpPage>();
    }

    private void SignUpButtonClicked()
    {
        UIManager.Instance.OpenPage<OtpPage>();
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
