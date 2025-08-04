using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPopup : UIPopup
{
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private Button closeButton;
    [SerializeField] private RectTransform popupRectTransform;
    [SerializeField] private TMP_InputField emailIdInputField, userNameInputField, passwordInputField;
    [SerializeField] private Button loginButton, registerButton;
    [SerializeField] private Button switchToLoginButton, switchToRegisterButton, forgetPasswordButton;

    private LoginPopupStatus _status;
    
    public enum LoginPopupStatus
    {
        LoginPopup,
        RegisterPopup
    }

    private void Awake()
    {
        _status = LoginPopupStatus.LoginPopup;
        closeButton.onClick.AddListener(OnHide);
        loginButton.onClick.AddListener(LoginButtonClicked);
        registerButton.onClick.AddListener(RegisterButtonClicked);
        switchToLoginButton.onClick.AddListener(SwitchToLoginButtonClicked);
        switchToRegisterButton.onClick.AddListener(SwitchToRegisterButtonClicked);
        forgetPasswordButton.onClick.AddListener(ForgetPasswordButtonClicked);
    }

    public override void OnShow(object data = null)
    {
        if (data is LoginPopupData popupData)
        {
            _status = popupData.status;
        }
        else
        {
            Debug.LogWarning("LoginPopup: Missing or incorrect data!");
        }

        headerText.text = _status == LoginPopupStatus.LoginPopup ? "LOGIN" : "SIGN UP";
        userNameInputField.gameObject.SetActive(_status == LoginPopupStatus.RegisterPopup);
        loginButton.gameObject.SetActive(_status == LoginPopupStatus.LoginPopup);
        registerButton.gameObject.SetActive(_status == LoginPopupStatus.RegisterPopup);
        switchToLoginButton.gameObject.SetActive(_status == LoginPopupStatus.RegisterPopup);
        switchToRegisterButton.gameObject.SetActive(_status == LoginPopupStatus.LoginPopup);
        forgetPasswordButton.gameObject.SetActive(_status == LoginPopupStatus.LoginPopup);
        
        base.OnShow(data);
        LayoutRebuilder.ForceRebuildLayoutImmediate(popupRectTransform);
    }
    
    public override void OnHide()
    {
        base.OnHide();
    }

    private void LoginButtonClicked()
    {
        
    }

    private void RegisterButtonClicked()
    {
        
    }
    
    private void SwitchToLoginButtonClicked()
    {
        UIManager.Instance.OpenPopup<LoginPopup>(new LoginPopupData(LoginPopup.LoginPopupStatus.LoginPopup));
    }
    
    private void SwitchToRegisterButtonClicked()
    {
        UIManager.Instance.OpenPopup<LoginPopup>(new LoginPopupData(LoginPopup.LoginPopupStatus.RegisterPopup));
    }
    
    private void ForgetPasswordButtonClicked()
    {
        
    }
}

[Serializable]
public class LoginPopupData
{
    public LoginPopup.LoginPopupStatus status;

    public LoginPopupData(LoginPopup.LoginPopupStatus status)
    {
        this.status = status;
    }
}