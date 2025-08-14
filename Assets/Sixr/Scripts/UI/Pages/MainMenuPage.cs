using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPage : UIPage
{
    [SerializeField] private Button profileButton;
    [SerializeField] private TMP_Text coinsText, gemsText;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button missionsButton;
    [SerializeField] private Button storeButton;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Button playButton;
    
    private void Awake()
    {
        profileButton.onClick.AddListener(ProfileButtonClicked);
        settingsButton.onClick.AddListener(SettingsButtonClicked);
        missionsButton.onClick.AddListener(MissionsButtonClicked);
        storeButton.onClick.AddListener(StoreButtonClicked);
        inventoryButton.onClick.AddListener(InventoryButtonClicked);
        playButton.onClick.AddListener(PlayButtonClicked);
    }

    public override void OnShow(object data = null)
    {
        base.OnShow(data);
    }
    
    public override void OnHide()
    {
        base.OnHide();
    }

    private void ProfileButtonClicked()
    {
        UIManager.Instance.OpenPopup<GenericPopup>(new GenericPopupData("NOTICE ",
            "New Version of app detected.\nWill you update now?\nNew Version of app detected.\nWill you update now?",
            UIManager.Instance.CloseTopPopup, UIManager.Instance.CloseTopPopup));
    }

    private void SettingsButtonClicked()
    {
        UIManager.Instance.OpenPopup<SettingsPopup>();
    }

    private void MissionsButtonClicked()
    {
        UIManager.Instance.OpenPopup<DailyRewardsPopup>();
    }
    
    private void StoreButtonClicked()
    {
        UIManager.Instance.OpenPage<StorePage>();
    }
    
    private void InventoryButtonClicked()
    {
        
    }

    private void PlayButtonClicked()
    {
        
    }
}
