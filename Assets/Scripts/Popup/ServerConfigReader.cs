using BestHTTP.Extensions;
using Newtonsoft.Json;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;


public class ServerConfigReader : MonoBehaviour
{
    public static ServerConfigReader instance;
    void Start()
    {
        if (instance == null)
            instance = this;
        if (AdIntegrate.instance.checkTheInternet())
        {
            DownloadServerConfig();
        }
        else
        {
            FeedValuesFromBuild();
        }
    }

    public void DownloadServerConfig(Action<bool> OnComplete = null)
    {
        if (AdIntegrate.instance.checkTheInternet())
        {
            StartCoroutine(CricMinisWebRequest.instance.GetJson(CONTROLLER.ServerConfigURL, (response) =>
            {
                if (string.IsNullOrEmpty(response))
                {
                    try
                    {
                        OnComplete?.Invoke(false);
                    }
                    catch (Exception e)
                    {

                    }
                    FeedValuesFromBuild();
                }
                else
                {
                    JSONNode _node = JSON.Parse(response);
                    CONTROLLER.serverConfig.BP = _node["BP"];
                    CONTROLLER.serverConfig.MV = int.Parse(_node["MV"]);
                    CONTROLLER.serverConfig.I =  int.Parse(_node["I"]);
                    CONTROLLER.serverConfig.M =  int.Parse(_node["M"]);
                    CONTROLLER.serverConfig.IM =  int.Parse(_node["IM"]); 
                    CONTROLLER.serverConfig.IBO = int.Parse(_node["IBO"]);
                    CONTROLLER.serverConfig.IGQ = int.Parse(_node["IGQ"]);
                    CONTROLLER.serverConfig.IGR = int.Parse(_node["IGR"]);
                    CONTROLLER.serverConfig.IGN = int.Parse(_node["IGN"]);

                    CONTROLLER.serverConfig.isReady = true;
                    CONTROLLER.isServerConfigSynced = true;

                    try
                    {
                        AdIntegrate.instance.writeInToFile(response, "ServerConfig", "ServerConfig.json");
                    }
                    catch (Exception e)
                    {
                        //Debug.LogError("Error Not saved == REASON ::" + e.ToString());
                    }

                    if (CONTROLLER.CURRENT_VERSION < CONTROLLER.serverConfig.MV)
                    {
                        DownloadTextJSON(CONTROLLER.serverConfig.BP + "MV/1.json", (msg, title) => ShowForcedUpdatePopUp(msg, title));
                    }

                    if (CONTROLLER.serverConfig.M == 1)//Maintenance Pop Up
                    {
                        DownloadTextJSON(CONTROLLER.serverConfig.BP + "M/1.json", (msg, title) => ShowMaintenancePopUp(msg, title));
                    }
                    if (CONTROLLER.CURRENT_VERSION < CONTROLLER.serverConfig.I)//Normal update pop up
                    {
                        string lastShown = PlayerPrefs.GetString("LastUpdatePopUpShown", "0");
                        if (lastShown != "0")
                        {
                            DateTime lS = lastShown.ToDateTime();
                            TimeSpan diff = DateTime.UtcNow - lS;
                            if (diff.TotalHours > 24)
                            {
                                DownloadTextJSON(CONTROLLER.serverConfig.BP + "I/1.json", (msg, title) => ShowUpdatePopUp(msg, title));
                                PlayerPrefs.SetString("LastUpdatePopUpShown", DateTime.UtcNow.ToString());
                            }
                        }
                        else
                        {
                            DownloadTextJSON(CONTROLLER.serverConfig.BP + "I/1.json", (msg, title) => ShowUpdatePopUp(msg, title));
                            PlayerPrefs.SetString("LastUpdatePopUpShown", DateTime.UtcNow.ToString());
                        }

                    }
                    else
                    {
                        if (PlayerPrefs.HasKey("LastUpdatePopUpShown"))
                        {
                            PlayerPrefs.DeleteKey("LastUpdatePopUpShown");
                        }
                    }

                    if (CONTROLLER.serverConfig.IM == 1)//Individual module server maintenanace
                    {
                       DownloadMaintenanceJSON(CONTROLLER.serverConfig.BP + "IM/1.json", OnComplete);
                    }
                    else
                        OnComplete?.Invoke(true);

                }
            }));
        }
        else
        {
            FeedValuesFromBuild();
            try
            {
                OnComplete?.Invoke(false);
            }
            catch (Exception e)
            {

            }
        }
    }

    public void FeedValuesFromBuild()
    {
        bool fileExists = AdIntegrate.instance.IsFileExits("ServerConfig", "ServerConfig.json");
        if (fileExists)
        {
            string jsonString = AdIntegrate.instance.readFromFile("ServerConfig", "ServerConfig.json");
            if (!string.IsNullOrEmpty(jsonString))
            {
                JSONNode _node = JSONNode.Parse(jsonString);

                CONTROLLER.serverConfig.BP = _node["BP"];
                CONTROLLER.serverConfig.MV = int.Parse(_node["MV"]);
                CONTROLLER.serverConfig.I =  int.Parse(_node["I"]);
                CONTROLLER.serverConfig.M =  int.Parse(_node["M"]);
                CONTROLLER.serverConfig.IM = int.Parse(_node["IM"]);
                CONTROLLER.serverConfig.IBO = int.Parse(_node["IBO"]);
                CONTROLLER.serverConfig.IGQ = int.Parse(_node["IGQ"]);
                CONTROLLER.serverConfig.IGR = int.Parse(_node["IGR"]);
                CONTROLLER.serverConfig.IGN = int.Parse(_node["IGN"]);

                if (CONTROLLER.serverConfig.M == 1)//Maintenance Pop Up
                {
                    ShowMaintenancePopUp(string.Empty,string.Empty);
                    AdIntegrate.instance.SetTimeScale(0f);
                }
                if (CONTROLLER.CURRENT_VERSION < CONTROLLER.serverConfig.MV)
                {
                    ShowForcedUpdatePopUp(string.Empty, string.Empty);
                    AdIntegrate.instance.SetTimeScale(0f);
                }

                CONTROLLER.serverConfig.isReady = true;
                CONTROLLER.isServerConfigSynced = false;
            }

        }
        else
        {
            string val = Resources.Load<TextAsset>("ServerConfig").ToString();
            if (!string.IsNullOrEmpty(val))
            {
                JSONNode _node = JSONNode.Parse(val);

                CONTROLLER.serverConfig.BP = _node["BP"];
                CONTROLLER.serverConfig.I = int.Parse(_node["I"]);
                CONTROLLER.serverConfig.M = int.Parse(_node["M"]);
                CONTROLLER.serverConfig.MV = int.Parse(_node["MV"]);
                CONTROLLER.serverConfig.IM = int.Parse(_node["IM"]);
                CONTROLLER.serverConfig.IBO = int.Parse(_node["IBO"]);
                CONTROLLER.serverConfig.IGQ = int.Parse(_node["IGQ"]);
                CONTROLLER.serverConfig.IGR = int.Parse(_node["IGR"]);
                CONTROLLER.serverConfig.IGN = int.Parse(_node["IGN"]);

                CONTROLLER.serverConfig.isReady = true;
                CONTROLLER.isServerConfigSynced = false;
            }
        }
    }
    void ShowForcedUpdatePopUp(string msg, string title)
    {
        if (string.IsNullOrEmpty(msg))
        {
            msg = "Get your Game on with the all-new features;\n Update the latest version now!";
        }
        if (string.IsNullOrEmpty(title))
        {
            title = "UPDATE";
        }
#if UNITY_ANDROID

        Popup.instance.Show(false, title, msg, yesString: "UPDATE",isBackAllowed: false, yesCallBack: RedirectToPlayStore );
#elif UNITY_IOS
        PopUp.Show(PopUpTypes.YES, size: PopUpSize.Medium, hasCloseButton: false,
            heading: title,
            message: msg,
            isHighPriority: true,
            yesString: "UPDATE",
            yesCallBack: () => { RedirectToPlayStore(); });
#endif
        AdIntegrate.instance.SetTimeScale(0f);
    }
    void ShowMaintenancePopUp(string msg, string title)
    {
        if (string.IsNullOrEmpty(msg))
        {
            msg = "Our server are in for a maintenance.\n Please come back later.";
        }
        if (string.IsNullOrEmpty(title))
        {
            title = "SERVER MAINTENANCE";
        }
#if UNITY_ANDROID

        Popup.instance.showGenericPopup(title, msg, AdIntegrate.instance.GameExit);
        AdIntegrate.instance.SetTimeScale(0f);
#elif UNITY_IOS
        Popup.instance.showGenericPopup(title, msg);
        //AdIntegrate.instance.ShowiOSMaintenancePopup(msg, title);
#endif
    }
    void ShowUpdatePopUp(string msg, string title)
    {
        if (string.IsNullOrEmpty(msg))
        {
            msg = "Get your Game on with the all-new features; update the latest version!";
        }
        if (string.IsNullOrEmpty(title))
        {
            title = "UPDATE";
        }

        Popup.instance.Show(true, title, msg, yesString: "UPDATE", noString: "CANCEL", yesCallBack: OpenPlayStore);
    }
    void DownloadMaintenanceJSON(string url, Action<bool> OnComplete = null)
    {
        StartCoroutine(CricMinisWebRequest.instance.GetJson(url,(response) =>
        {
            if (string.IsNullOrEmpty(response))
            {
                OnComplete?.Invoke(false);
            }
            else
            {
                JSONNode _node = JSONNode.Parse(response);

                CONTROLLER.BmpMaintenance =  int.Parse(_node["BMP"]);
                if (CONTROLLER.BmpMaintenance == 1)
                {
                    if (_node["msg"] != null)
                    {
                        CONTROLLER.BmpMaintenanceText = _node["msg"];
                    }
                }
                OnComplete?.Invoke(true);
            }
        }));
       
    }
   
    void DownloadTextJSON(string url, Action<string, string> onMessageReceived)
    {
        string res = string.Empty;
        string title = string.Empty;
        StartCoroutine(CricMinisWebRequest.instance.GetJson(url,(response) =>
        {
            try
            {
                if (string.IsNullOrEmpty(response))
                {
                    res = string.Empty;
                    onMessageReceived?.Invoke(res, title);
                }
                else
                {
                    JSONNode _node = JSONNode.Parse(response);
                    res = _node["T"];
                    title = _node["H"];
                    onMessageReceived?.Invoke(res, title);
                }
            }
            catch (Exception e)
            {

            }
        }));
    }
    void RedirectToPlayStore()
    {
#if UNITY_ANDROID
        Application.OpenURL(CONTROLLER.AppLink);
        AdIntegrate.instance.GameExit();
#elif UNITY_IOS
        EtceteraBinding.openAppStoreReviewPage("1514775424");
#endif
    }
    void OpenPlayStore()
    {
#if UNITY_ANDROID
        Application.OpenURL(CONTROLLER.AppLink);
#elif UNITY_IOS
        EtceteraBinding.openAppStoreReviewPage("1514775424");
#endif
    }

}
