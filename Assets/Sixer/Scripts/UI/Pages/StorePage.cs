using System;
using UnityEngine;
using UnityEngine.UI;

public class StorePage : UIPage
{
    [SerializeField] private Button backButton;

    private void Awake()
    {
        backButton.onClick.AddListener(BackButtonClicked);
    }

    private void BackButtonClicked()
    {
        UIManager.Instance.CloseCurrentPage();
    }

    public override void OnShow(object data = null)
    {
        base.OnShow(data);
    }
    
    public override void OnHide()
    {
        base.OnHide();
    }
}
