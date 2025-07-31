using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBTracking : Singleton<DBTracking>
{
    Dictionary<string, string> dict = new Dictionary<string, string>();

    #region TICKETS
    [HideInInspector]
    public bool isTicketUsed;
    [HideInInspector]
    public string CurrentMatchID;
    public void ConsumeTicket()
    {
        if (!AdIntegrate.instance.checkTheInternet())
        {
            return;
        }
        dict.Clear();
        dict.Add("roomid", "0");
        dict.Add("mtype", "0");
        StartCoroutine(CricMinisWebRequest.instance.SyncData("match/start", dict, (_response) =>
        {
            if (!string.IsNullOrEmpty(_response))
            {
                JSONNode _node = JSON.Parse(_response);
                int status = int.Parse(_node["status"]);
                if (status == 200)
                {
                    if (_node["data"]["match_id"] != null)
                    {
                        CurrentMatchID = _node["data"]["match_id"];
                    }
                    isTicketUsed = false;
                    PlayerPrefsManager.SaveUserProfile();
                    if (AdIntegrate.instance.CurrentSceneIndex == 1)
                        GameModeSelector._instance.UpdateLandingPageTopBars();
                }
            }
        }, ServerRequest.POST, true));
    }

    public void AddTicket( )
    {
        if (!AdIntegrate.instance.checkTheInternet())
        {
            Popup.instance.ShowNoInternetPopup();
            return;
        }
        LoadingScreen.instance.Show();

        dict.Clear();
        if (AdIntegrate.instance.CurrentSceneIndex == 1)
        {
            if (CONTROLLER.CurrentPage == "splashpage")
                dict.Add("rvtype", "0");
            else
                dict.Add("rvtype", "1");
        }
        else
            dict.Add("rvtype", "2");


        StartCoroutine(CricMinisWebRequest.instance.SyncData("users/addrvpoints", dict, (_response) =>
        {
            if (!string.IsNullOrEmpty(_response))
            {
                JSONNode _node = JSON.Parse(_response);
                int status = int.Parse(_node["status"]);
                if (status == 200)
                {
                    PlayerPrefsManager.SaveUserProfile();
                    Popup.instance.showTicketPopup(onTicketOkClicked);
                }
            }
            LoadingScreen.instance.Hide();
        }, ServerRequest.POST, false));
    }

    void onTicketOkClicked()
    {
        if (AdIntegrate.instance.CurrentSceneIndex == 1)
        {
            GameModeSelector._instance.UpdateLandingPageTopBars();
        }
    }
    #endregion

    #region SUPER OVER
    //levelcompleted=0-fail 1-completed
    public void SuperOverLevelCompletion(int levelcompleted)
    {
        if (!AdIntegrate.instance.checkTheInternet())
        {
            Popup.instance.ShowNoInternetPopup();
            return;
        }
        int RV = 0;
        if (PlayerPrefs.HasKey("SoRVlevID"))
            RV = 1;

        if (CONTROLLER.LevelCompletedArray[CONTROLLER.LevelId] == 0)
        {
            dict.Clear();
            dict.Add("ctype", (CONTROLLER.LevelId+1).ToString());
            dict.Add("comp", levelcompleted.ToString());
            dict.Add("rv", RV.ToString());

            StartCoroutine(CricMinisWebRequest.instance.SyncData("match/superover", dict, (_response) =>
            {
                //if (!string.IsNullOrEmpty(_response))
                //{
                //    JSONNode _node = JSON.Parse(_response);
                //    int status = int.Parse(_node["status"]);
                //    if (status == 200)
                //    {
                //    }
                //}
            }, ServerRequest.POST, true));
        }
    }
    #endregion

    #region SUPER CHASE
    //levelcompleted=0-fail 1-completed
    public void SuperChaseLevelCompletion(int levelcompleted)
    {
        if (!AdIntegrate.instance.checkTheInternet())
        {
            Popup.instance.ShowNoInternetPopup();
            return;
        }
        int RV = 0;
        if (PlayerPrefs.HasKey("CTRVlevID"))
            RV = 1;

        if (CONTROLLER.SubLevelCompletedArray[CONTROLLER.CTSubLevelCompleted] == 0 && CONTROLLER.MainLevelCompletedArray[CONTROLLER.CTLevelCompleted] == 0)
        {
            dict.Clear();
            dict.Add("ltype", (CONTROLLER.CTLevelCompleted + 1).ToString());
            dict.Add("mtype", (CONTROLLER.CTSubLevelCompleted + 1).ToString());
            dict.Add("comp", levelcompleted.ToString());
            dict.Add("rv", RV.ToString());

            StartCoroutine(CricMinisWebRequest.instance.SyncData("match/superchase", dict, (_response) =>
            {
                //if (!string.IsNullOrEmpty(_response))
                //{
                //    JSONNode _node = JSON.Parse(_response);
                //    int status = int.Parse(_node["status"]);
                //    if (status == 200)
                //    {
                //    }
                //}
            }, ServerRequest.POST, true));
        }
    }
    #endregion

    #region SUPER SLOG

    public void GetSlogModeMatchID()
    {
        if (!AdIntegrate.instance.checkTheInternet())
        {
            return;
        }
        dict.Clear();
        dict.Add("mtype", "1");
        StartCoroutine(CricMinisWebRequest.instance.SyncData("match/start", dict, (_response) =>
        {
            if (!string.IsNullOrEmpty(_response))
            {
                JSONNode _node = JSON.Parse(_response);
                int status = int.Parse(_node["status"]);
                if (status == 200)
                {
                    if (_node["data"]["match_id"] != null)
                    {
                        PlayerPrefs.SetString("slogovermatchid", (_node["data"]["match_id"]));
                    }
                }
            }
        }, ServerRequest.POST, true));
    }

    public void SuperSlogLevelCompletion(string score, string wicket, string overs)
    {
        if (!AdIntegrate.instance.checkTheInternet() || !PlayerPrefs.HasKey("slogovermatchid"))
        {
            return;
        }

        dict.Clear();
        dict.Add("match_id", PlayerPrefs.GetString("slogovermatchid"));
        dict.Add("ov", overs);
        dict.Add("wc", wicket);
        dict.Add("score", score);

        StartCoroutine(CricMinisWebRequest.instance.SyncData("match/end", dict, (_response) =>
        {
            //if (!string.IsNullOrEmpty(_response))
            //{
            //    JSONNode _node = JSON.Parse(_response);
            //    int status = int.Parse(_node["status"]);
            //    if (status == 200)
            //    {
            //    }
            //}
        }, ServerRequest.POST, true));
    }
    #endregion
}
