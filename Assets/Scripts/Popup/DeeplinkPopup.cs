using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DeeplinkPopup : MonoBehaviour
{
    public RawImage DeeplinkImage;
    public Text ButtonText;
    public Text headerText;
    public GameModeSelector LandingPage;
    public static DeeplinkPopup instance;
    public static bool isFirstTimeShow = false;
    public RectTransform ButtonRectTransfrom;

    private class deeplinkData
    {
        public int type;        //1:IAP, 2: Deeplinking, 3: ExternalLink, 
        public string data;     //if type==1->product id       type==2-> Module opening ID   type ==3-> external link
        public string imgURL;
        public string deeplinkVersion;
        public string deeplinkID;
        public string buttonString;
        public string headerString;
        public string ExternalLinkURL;
    }
    deeplinkData _deeplinkdata;

    public void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private string folderName = "Deeplinkpopup";

    private void saveTextureInLocal(Texture _tex)
    {
        byte[] textureBytes = ((Texture2D)_tex).EncodeToPNG();
        ImageSaver.SaveTextureForBanner((Texture2D)_tex, _deeplinkdata.deeplinkID, "png", folderName);
    }

    private void retreiveTextureFromLocal()
    {
        DeeplinkImage.texture = ImageSaver.RetriveTextureForBanner(_deeplinkdata.deeplinkID, "png", folderName);
        DeeplinkImage.SetNativeSize();
    }

    private void doAftertextureDownload()
    {
        ButtonText.text = _deeplinkdata.buttonString;
        headerText.text = _deeplinkdata.headerString;

        if (_deeplinkdata.type == 1)
        {
            if (StorePanel.IsAllPacksInitialised())
            {
                if (string.Equals(_deeplinkdata.data, SKU.blitz100tickets.ToString()) || string.Equals(_deeplinkdata.data, "1"))
                {
                    ButtonText.text = "";//IAPHandler.instance.TICKET.priceValue; 
                }
                else if (string.Equals(_deeplinkdata.data, SKU.blitzadsfree.ToString()) || string.Equals(_deeplinkdata.data, "2"))
                {
                    ButtonText.text = "";//IAPHandler.instance.REMOVE_ADs.priceValue;
                }
            }
        }
        LandingPage.ShowDeeplinkPopup();
        LayoutRebuilder.ForceRebuildLayoutImmediate(ButtonRectTransfrom);
        isFirstTimeShow = true;

        PlayerPrefs.SetString("lastshowDeeplinkID" + _deeplinkdata.deeplinkID, _deeplinkdata.deeplinkID);
        PlayerPrefs.SetString("lastshowDeeplinkVersion" + _deeplinkdata.deeplinkVersion, _deeplinkdata.deeplinkVersion);
    }

    private void downloadTextureFromURL()
    {
      StartCoroutine(downloadTexture(_deeplinkdata.imgURL));
    }

    IEnumerator downloadTexture(string url )
    {
        UnityWebRequest WebRequest = UnityWebRequestTexture.GetTexture(url);
        WebRequest.timeout = 15;
        yield return WebRequest.SendWebRequest();

        if (WebRequest.result == UnityWebRequest.Result.Success)
        {
            DeeplinkImage.texture = ((DownloadHandlerTexture)WebRequest.downloadHandler).texture;
            doAftertextureDownload();
            saveTextureInLocal(((DownloadHandlerTexture)WebRequest.downloadHandler).texture);
        }
    }
   
    public void closeBtn()
    {
       LandingPage.HideDeeplinkPopup();
    }

    public void OnCLickDeeplinkPopup()
    {
       closeBtn();
        switch(_deeplinkdata.type)
        {
            case 1:

                if (StorePanel.IsAllPacksInitialised())
                {
                    if (string.Equals(_deeplinkdata.data, SKU.blitz100tickets.ToString()) || string.Equals(_deeplinkdata.data, "1"))
                    {
                        StorePanel.instance.OnPurchaseButClicked(0);
                    }
                    else if (string.Equals(_deeplinkdata.data, SKU.blitzadsfree.ToString()) || string.Equals(_deeplinkdata.data, "2"))
                    {
                        StorePanel.instance.OnPurchaseButClicked(1);
                    }
                }
                else
                    StorePanel.instance.OpenStore();
                break;

            case 2:
                int moduleIdx = int.Parse(_deeplinkdata.data);
                LandingPage.actionsFromDeeplinkPopup(moduleIdx);
                break;
            case 3:
                Application.OpenURL(_deeplinkdata.ExternalLinkURL);
                break;
        }

    }



}

