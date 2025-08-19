using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPage : UIPage
{
    [SerializeField] private GameModeSelector gameModeSelector;
    [SerializeField] private Button profileButton;
    [SerializeField] private TMP_Text coinsText, gemsText;
    [SerializeField] private Button quickPlayButton, chooseModeButton;
    
    private void Awake()
    {
        profileButton.onClick.AddListener(ProfileButtonClicked);
        quickPlayButton.onClick.AddListener(QuickPlayButtonClicked);
        chooseModeButton.onClick.AddListener(ChooseModeButtonClicked);
    }
    
    private void ProfileButtonClicked()
    {
        
    }
    
    private void QuickPlayButtonClicked()
    {
        gameModeSelector.SelectGameMode(2);
    }
    
    private void ChooseModeButtonClicked()
    {
        gameModeSelector.SelectGameMode(4);
    }
}
