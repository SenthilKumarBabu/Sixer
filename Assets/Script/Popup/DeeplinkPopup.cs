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

    public void checkForDeeplinking()
    {
        if (isFirstTimeShow == true)//|| OnNewIntent.instance.startGameWithIntent == true)
        {
            return;
        }
        StartCoroutine(CricMinisWebRequest.instance.SyncData("deeplink/popup", null, (_response) =>
        {
            if (!string.IsNullOrEmpty(_response))
            {
                JSONNode _node = JSON.Parse(_response);
                _deeplinkdata = new deeplinkData();

                if (_node["data"] != null)//&& OnNewIntent.instance.startGameWithIntent == false)
                {
                    _deeplinkdata.type = _node["data"]["type"].AsInt;
                    _deeplinkdata.data = _node["data"]["data"];
                    _deeplinkdata.imgURL = _node["data"]["imageurl"];
                    _deeplinkdata.deeplinkVersion = _node["data"]["dversion"];
                    _deeplinkdata.deeplinkID = _node["data"]["id"];
                    _deeplinkdata.buttonString = _node["data"]["bLabel"];
                    _deeplinkdata.headerString = _node["data"]["text"];
                    _deeplinkdata.ExternalLinkURL = _node["data"]["elink"];

                    if (PlayerPrefs.GetString("lastshowDeeplinkID" + _deeplinkdata.deeplinkID, "0") == _deeplinkdata.deeplinkID)
                    {
                        if (int.Parse(PlayerPrefs.GetString("lastshowDeeplinkVersion" + _deeplinkdata.deeplinkVersion, "0")) == int.Parse(_deeplinkdata.deeplinkVersion))
                        {
                                retreiveTextureFromLocal();
                                doAftertextureDownload();
                        }
                        else
                        {
                            downloadTextureFromURL();
                        }
                    }
                    else
                    {
                        downloadTextureFromURL();
                    }
                }
            }
        }, ServerRequest.GET, true));

    }


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

