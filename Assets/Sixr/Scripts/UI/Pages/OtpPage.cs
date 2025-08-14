using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OtpPage : UIPage
{
    [SerializeField] private List<TMP_InputField> otpInputFieldList;
    [SerializeField] private Button resendButton, nextButton,backButton;

    private void Awake()
    {
        resendButton.onClick.AddListener(ResendButtonClicked);
        nextButton.onClick.AddListener(NextButtonClicked);
        backButton.onClick.AddListener(BackButtonClicked);
    }
    
    private void ResendButtonClicked()
    {
        
    }
    
    private void NextButtonClicked()
    {
        
    } 
    
    private void BackButtonClicked()
    {
        UIManager.Instance.CloseCurrentPage();
    }
}
