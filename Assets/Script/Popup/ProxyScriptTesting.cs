using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Beebyte.Obfuscator;
using BestHTTP;
using UnityEngine.UI;
using System.IO;
using SimpleJSON;
using Newtonsoft.Json;

public class ProxyScriptTesting : MonoBehaviour
{
    private void Awake()
    {
        //if (PlayerPrefs.HasKey("BaseURL"))
        //{
        //    inputField2.text = PlayerPrefs.GetString("BaseURL");
        //}
        //else
        //{
        //    inputField2.text = CONTROLLER.BaseURLNew;
        //}
        //if (PlayerPrefs.HasKey("ProxyURL"))
        //{
        //    inputField1.text = PlayerPrefs.GetString("ProxyURL");
        //}
        CreateAndReadFileForBackEnd();
    }
    public void CreateAndReadFileForBackEnd()
    {

        string path = Application.persistentDataPath + "/Proxy.json";
        string proxy = "http://192.168.1.151:8888";
        Dictionary<string, string> buildURL = new Dictionary<string, string>
        {
            { "baseUrlIndex", CONTROLLER.BaseUrlIndex.ToString()},
            { "proxy", "" },
            { "url", "" },
            { "clean", "1" }
        };
        FileInfo TxtFile = new FileInfo(path);
        if (!TxtFile.Exists)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(buildURL));
        }
        else
        {
            string Final = File.ReadAllText(path);
            JSONNode _node = JSONNode.Parse(Final);
            int bp = _node["baseUrlIndex"].AsInt;
            int clean = _node["clean"].AsInt;
            //if(clean == 1)
            //{
            //    CONTROLLER.enableHardCodes = false;
            //}
            //else
            //{
            //    CONTROLLER.enableHardCodes = false;
            //}

            if (bp > 0)
            {
                if (PlayerPrefs.GetInt("bpInd", -1) != bp)
                {
                    PlayerPrefs.DeleteAll();
                }
                PlayerPrefs.SetInt("bpInd", bp);

                if (bp == 1)
                {
                    CONTROLLER.BASE_URL = CONTROLLER.BaseURLDev;
                    CONTROLLER.ServerConfigURL = CONTROLLER.ServerConfigURLDev;
                }
                else if (bp == 2)
                {
                    CONTROLLER.BASE_URL = CONTROLLER.BaseURLStag;
                    CONTROLLER.ServerConfigURL = CONTROLLER.ServerConfigURLStag;
                }
                else if (bp == 3)
                {
                    CONTROLLER.BASE_URL = CONTROLLER.BaseURLProd;
                    CONTROLLER.ServerConfigURL = CONTROLLER.ServerConfigURLPrd;
                }
            }
            string proxyVal = _node["proxy"];
            if (proxyVal != null && proxyVal.Trim() != "")
            {
                ProxyButtonClicked(proxyVal);
            }
            string urlVal = _node["url"];
            if (urlVal != null && urlVal.Trim() != "")
            {
                CONTROLLER.BASE_URL = urlVal;
            }
        }
    }

    private InputField inputField1, inputField2;
    //[SkipRename]
    public void ProxyButtonClicked(string TEXT)
    {
        string URL = TEXT;
        Debug.LogError(URL);
        HTTPManager.Proxy = new HTTPProxy(new System.Uri(URL),null,true);
    }

    //[SkipRename]
    public void BaseUrlChange()
    {
        string URL = inputField2.text;
        CONTROLLER.BASE_URL = URL;
        PlayerPrefs.SetString("BaseURL", URL);
    }

    //[SkipRename]
    public void ChangeScene()
    {
        //LoadingScreenPanel.instance.ShowSceneLoading("General", "Loading ground... Please wait...", "Preloader");
        //ManageScene.LoadScene("Preloader");
    }

    //[SkipRename]
    public void LoadDefaultUrlValues()
    {
        if (PlayerPrefs.HasKey("ProxyURL"))
        {
            inputField1.text = PlayerPrefs.GetString("ProxyURL");
        }
        else
        {
          //  Debug.LogError("No Proxy Preference available");
        }
        if (PlayerPrefs.HasKey("BaseURL"))
        {
            inputField2.text = PlayerPrefs.GetString("BaseURL");
        }
        else
        {
            //Debug.LogError("No Base URL Preference available");
        }
    }
}
