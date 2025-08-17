using System;
using UnityEngine;
using UnityEngine.UI;

public class QuickPlayModesPage : UIPage
{
    [SerializeField] private Button playerVsPlayerButton, playerVsFriendsButton, playerVsAiButton, backButton;

    private void Awake()
    {
        playerVsPlayerButton.onClick.AddListener(PlayerVsPlayerButtonClicked);
        playerVsFriendsButton.onClick.AddListener(PlayerVsFriendsButtonClicked);
        playerVsAiButton.onClick.AddListener(PlayerVsAiButtonClicked);
        backButton.onClick.AddListener(BackButtonClicked);
    }

    public override void OnShow(object data = null)
    {
        base.OnShow(data);
    }
    
    public override void OnHide()
    {
        base.OnHide();
    }

    public void PlayerVsPlayerButtonClicked()
    {
        
    }

    public void PlayerVsFriendsButtonClicked()
    {
        
    }

    public void PlayerVsAiButtonClicked()
    {
        
    }

    public void BackButtonClicked()
    {
        
    }
}
