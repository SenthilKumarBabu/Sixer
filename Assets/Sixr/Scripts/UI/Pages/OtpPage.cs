using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
        
        for (int i = 0; i < otpInputFieldList.Count; i++)
        {
            int index = i;

            otpInputFieldList[index].onValueChanged.AddListener((text) =>
            {
                if (text.Length > 0 && index < otpInputFieldList.Count - 1)
                {
                    otpInputFieldList[index + 1].Select();
                }
            });

            EventTrigger trigger = otpInputFieldList[index].gameObject.GetComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.UpdateSelected;
            entry.callback.AddListener((eventData) =>
            {
                if (EventSystem.current.currentSelectedGameObject != otpInputFieldList[index].gameObject)
                    return;

                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    if (!string.IsNullOrEmpty(otpInputFieldList[index].text))
                    {
                        otpInputFieldList[index].text = "";
                    }
                    else if (index > 0)
                    {
                        otpInputFieldList[index - 1].Select();
                    }
                }
            });

            trigger.triggers.Add(entry);
        }
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
