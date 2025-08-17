using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GenericPopup : UIPopup
{
    [SerializeField] private RectTransform popupRectTransform;
    [SerializeField] private Image headerLogo;
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_Text contentText;
    [SerializeField] private Button okButton, cancelButton;

    private GenericPopupData _popupData;
    
    protected override void Awake()
    {
        base.Awake();
        closeButton.onClick.AddListener(CloseButtonClicked);
    }
    
    public override void OnShow(object data = null)
    {
        if (data is GenericPopupData popupData)
        {
            _popupData = popupData;
        }
        else
        {
            Debug.LogWarning("LoginPopup: Missing or incorrect data!");
        }

        headerText.text = _popupData.header;
        contentText.text = _popupData.content;
        if (_popupData.OkCallback != null)
        {
            okButton.onClick.AddListener(_popupData.OkCallback);
        }
        okButton.gameObject.SetActive(_popupData.OkCallback != null);
        
        if (_popupData.CancelCallback != null)
        {
            cancelButton.onClick.AddListener(_popupData.CancelCallback);
        }
        cancelButton.gameObject.SetActive(_popupData.CancelCallback != null);
        
        base.OnShow(data);
        LayoutRebuilder.ForceRebuildLayoutImmediate(popupRectTransform);
    }

    private void CloseButtonClicked()
    {
        UIManager.Instance.CloseTopPopup();
    }
}

[Serializable]
public class GenericPopupData
{
    public string header;
    public string content;
    public UnityAction OkCallback;
    public UnityAction CancelCallback;

    public GenericPopupData(string header,string content,UnityAction okCallback, UnityAction cancelCallback)
    {
        this.header = header;
        this.content = content;
        this.OkCallback = okCallback;
        this.CancelCallback = cancelCallback;
    }
}