
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ForgotPasswordPage : UIPage
{
    [SerializeField] private TMP_InputField newPasswordIf, confirmPasswordIf;
    [SerializeField] private Button backButton, nextButton;
    [SerializeField] private Toggle toggleCharacterConstraint, toggleSpecialCharacter;

    private void Awake()
    {
        backButton.onClick.AddListener(BackButtonClicked);
        nextButton.onClick.AddListener(NextButtonClicked);
    }

    public override void OnShow(object data = null)
    {
        base.OnShow(data);
    }
    
    public override void OnHide()
    {
        base.OnHide();
    }

    public void BackButtonClicked()
    {
        
    }

    public void NextButtonClicked()
    {
        
    }
}
