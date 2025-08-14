using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardsPopup : UIPopup
{
    [SerializeField] private Button closeButton;
    [SerializeField] private List<RewardsData> rewardsDataList;
    
    [Serializable]
    public struct RewardsData
    {
        public Image icon;
        public Image completedIcon;
    }

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
