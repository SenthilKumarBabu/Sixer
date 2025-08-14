using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : UIPopup
{
    [SerializeField] private Button closeButton;
    
    protected override void Awake()
    {
        base.Awake();
        closeButton.onClick.AddListener(CloseButtonClicked);
    }
    
    public override void OnShow(object data = null)
    {
        base.OnShow(data);
    }
    
    public override void OnHide()
    {
        base.OnHide();
    }
    
    private void CloseButtonClicked()
    {
        UIManager.Instance.CloseTopPopup();
    }
}
