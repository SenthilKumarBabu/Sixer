using UnityEngine;
using UnityEngine.UI;

public class GameModePage : UIPage
{
    [SerializeField] private Button backButton,challengeModeButton, sixrModeButton;
    
    private void Awake()
    {
        backButton.onClick.AddListener(BackButtonClicked);
        challengeModeButton.onClick.AddListener(ChallengeModeButtonClicked);
        sixrModeButton.onClick.AddListener(SixrModeButtonClicked);
    }
    
    public override void OnShow(object data = null)
    {
        base.OnShow(data);
    }
    
    public override void OnHide()
    {
        base.OnHide();
    }
    
    private void BackButtonClicked()
    {
        
    }
    
    private void ChallengeModeButtonClicked()
    {
        
    }
    
    private void SixrModeButtonClicked()
    {
        
    }
}
