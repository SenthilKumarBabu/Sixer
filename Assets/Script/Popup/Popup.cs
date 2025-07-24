using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Popup : MonoBehaviour
{
    public static Popup instance;
    public GameObject holder;
    
    public Text Title;
    public Text Content;

    public GameObject YesButton;
    public Button yesBtn;
    public Text YesButtonText;

    public GameObject NoButton;
    public Button NoBtn;
    public Text NoButtonText;

    [SerializeField]
    RectTransform BGRectTransform;
    [SerializeField]
    RectTransform TitleRectTransform;
    [SerializeField]
    RectTransform ContentRectTransform;
    [SerializeField]
    GameObject NormalPopupBG;
    [SerializeField]
    GameObject TicketPopupBG;
    public Button yesBtn_TicketPopup;

    [HideInInspector]
    private const string NO_INTERNET_TEXT = "There seems to be some problem with your internet connection.\nPlease recheck your network settings.";
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    public bool IsBackAllowed
    {
        get;
        private set;
    }
    public bool isShowing;
    public void Show(bool isDouble = false, string heading = "", string message = "", string yesString = "YES", bool isBackAllowed = true, string noString = "NO", UnityAction yesCallBack = null, UnityAction noCallBack = null, bool canStay = false, bool noButtons = false,int size=0,int popupType=0)
    {
        //reset
        BlueBG.transform.DOScale(0, 0f);
        BlueBG.DOFade(0, 0f);
        //NoButton.transform.DOScale(0, 0f);
        //YesButton.transform.DOScale(0, 0f);

        isShowing = true;
        holder.SetActive(true);
        IsBackAllowed = isBackAllowed;

        if (popupType == 1)
        {
            TicketPopupBG.SetActive(true);
            NormalPopupBG.SetActive(false);
            yesBtn_TicketPopup.onClick.RemoveAllListeners();
            yesBtn_TicketPopup.onClick.AddListener(Close);
            if (yesCallBack != null)
                yesBtn_TicketPopup.onClick.AddListener(yesCallBack);
        }
        else
        {
            TicketPopupBG.SetActive(false);
            NormalPopupBG.SetActive(true);
           // SetPopUpSize(size);

            Title.text = heading;
            
            if (string.IsNullOrEmpty(heading))
                Title.gameObject.SetActive(false);
            else
                Title.gameObject.SetActive(true);

            Content.text = message;
            if (isDouble)
            {
                YesButton.SetActive(true);
                NoButton.SetActive(true);
            }
            else
            {
                YesButton.SetActive(true);
                NoButton.SetActive(false);
            }
            if (noButtons == true)
            {
                YesButton.SetActive(false);
                NoButton.SetActive(false);
            }
            YesButtonText.text = yesString;
            yesBtn.onClick.RemoveAllListeners();
            if (canStay == false)
            {
                yesBtn.onClick.AddListener(Close);
            }

            if (yesCallBack != null)
                yesBtn.onClick.AddListener(yesCallBack);

            NoButtonText.text = noString;
            NoBtn.onClick.RemoveAllListeners();
            NoBtn.onClick.AddListener(Close);
            if (noCallBack != null)
                NoBtn.onClick.AddListener(noCallBack);

            LayoutRebuilder.ForceRebuildLayoutImmediate(BGRectTransform);


            if (yesString.Length <= 3)
            {
                YesButtonText.fontSize = 35;
                YesButtonText.resizeTextForBestFit = false;
            }
            else
            {
                YesButtonText.fontSize = 30;
                YesButtonText.resizeTextForBestFit = true;
                YesButtonText.resizeTextMaxSize = 30;
            }

            if (noString.Length <= 3)
            {
                NoButtonText.fontSize = 35;
                NoButtonText.resizeTextForBestFit = false;
            }
            else
            {
                NoButtonText.fontSize = 30;
                NoButtonText.resizeTextForBestFit = true;
                NoButtonText.resizeTextMaxSize = 30;
            }
        }


        startAnim(popupType);
    }

    public Image BlueBG;
    public void startAnim(int type)
    {
        if (type == 0)
        {
            Sequence mySeq = DOTween.Sequence();
            mySeq.Insert(0f, BlueBG.transform.DOScale(1, 1f));
            mySeq.Insert(0f, BlueBG.DOFade(1, 0.85f));

            //mySeq.Insert(0.7f, NoButton.transform.DOScale(1, 0.4f));
            //mySeq.Insert(0.8f, YesButton.transform.DOScale(1, 0.4f));

            mySeq.SetEase(Ease.OutBack);
            mySeq.SetUpdate(true);
        }
        else
        {
            TicketPopupBG.transform.DOScale(0f, 0f);
            TicketPopupBG.transform.DOScale(1f, 0.5f).SetUpdate(true).SetEase(Ease.OutBack);
        }
    }

  
    //0-Normal 1-Large
    void SetPopUpSize(int popSizeIndex)
    {
        if(popSizeIndex==0)
        {
            BGRectTransform.sizeDelta = new Vector3(BGRectTransform.sizeDelta.x, 433f);
            TitleRectTransform.sizeDelta = new Vector3(TitleRectTransform.sizeDelta.x, 100f);
            ContentRectTransform.sizeDelta = new Vector3(ContentRectTransform.sizeDelta.x, 175f);
        }
        else
        {
            BGRectTransform.sizeDelta = new Vector3(BGRectTransform.sizeDelta.x, 575f);
            TitleRectTransform.sizeDelta = new Vector3(TitleRectTransform.sizeDelta.x, 170f);
            ContentRectTransform.sizeDelta = new Vector3(ContentRectTransform.sizeDelta.x, 230f);
        }
    }
    public void Close()
    {
        isShowing = false;
        AudioPlayer.instance.PlayButtonSnd();
        holder.SetActive(false);
    }

    public void OnBack()
    {
        Button.ButtonClickedEvent callBack = null;

        if (TicketPopupBG.activeSelf)
            callBack = yesBtn_TicketPopup.onClick;
        else
        {
            if (NoBtn.gameObject.activeSelf)
            {
                callBack = NoBtn.onClick;
            }
            else
            {
                callBack = yesBtn.onClick;
            }
        }
        if (callBack != null)
        {
            callBack.Invoke();
        }
    }

    public void ShowNoInternetPopup(UnityAction callback=null)
    {
        LoadingScreen.instance.Hide();
        Show(false, "No Internet.", NO_INTERNET_TEXT, "OK", yesCallBack: callback);
    }

    public void ShowSomethingWentWrong(UnityAction callback = null)
    {
        LoadingScreen.instance.Hide();
        Show(false, "OOPS!!!", "Something went wrong. Please try again later.", "OK", yesCallBack: callback);
    }

    public void showGenericPopup(string _title, string _msg, UnityAction callBack = null)
    {
        LoadingScreen.instance.Hide();
        Show(false, _title, _msg, "OK", yesCallBack: callBack);
    }

    public void showTicketPopup( UnityAction callBack = null)
    {
        LoadingScreen.instance.Hide();
        Show(false, yesCallBack: callBack ,popupType:1);
    }
}
