using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIPopup : UIBase
{
    [Tooltip("If true, clicking outside will close this popup.")]
    public bool closeOnBackgroundClick = true;
    [Tooltip("If true, allows stacking with other popups.")]
    public bool isOverlay = false;
    public Button backgroundButton;

    protected virtual void Awake()
    {
        if (backgroundButton != null)
            backgroundButton.onClick.AddListener(OnBackgroundClicked);
    }

    private void OnBackgroundClicked()
    {
        if (closeOnBackgroundClick)
            UIManager.Instance.CloseTopPopup();
    }
}
