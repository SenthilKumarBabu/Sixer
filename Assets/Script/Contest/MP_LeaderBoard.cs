using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public class MP_LeaderBoard : MonoBehaviour
{
    public GameObject LeaderBoardPanel;

    public Text SeasonResetText;
    public GameObject[] Bar;
    public GameObject[] UserBG;
    public GameObject[] OtherBG;

    public Text[] Rank;
    public Text[] Name;
    public Text[] CpValue;

    [Header("USER'S BAR")]
    public GameObject UsersBar;
    public Text UsersRank;
    public Text UsersName;
    public Text UsersCpValue;


    public List<CpLeaderBoardData> CPLeaderBoardData;


    [HideInInspector]
    public string LeaderboardIsFrom = string.Empty;

    public Font userfont, othersfont;

    [Header("CONTEST RELATED")]
    public GameObject SeasonalLBHolder;
    public GameObject Btn1, Btn2;
    public Text TitleText;
    public Text Btn1Name, Btn2Name;
    private string Btn1URL, Btn2URL;

    public void ShowMe()
    {
        LeaderBoardPanel.SetActive(true);
        StartCoroutine(GetLeaderBoardData());        
    }
    public void HideMe()
    {
        StopCoroutine("RemainingTime");
        LeaderBoardPanel.SetActive(false);
    }
       
    public IEnumerator GetLeaderBoardData()
    {
        if (ManageScene.CurScene == Scenes.MainMenu)
            MultiplayerPage.instance.ShowLoadingScreen();

        WWWForm form = new WWWForm();
        WWW download;

        form.AddField("action", "CPLeaderboad");
        form.AddField("bv", CONTROLLER.CURRENT_VERSION);
        form.AddField("platform", CONTROLLER.TargetPlatform);
        form.AddField("user_id", CONTROLLER.UserID);

        download = new WWW(CONTROLLER.BASE_URL, form);
        yield return download;
                
        if (!string.IsNullOrEmpty(download.error))
        {
            if (ManageScene.CurScene == Scenes.MainMenu)
                MultiplayerPage.instance.HideLoadingScreen();            
        }
        else
        {
            SimpleJSON.JSONNode node = SimpleJSON.JSONNode.Parse(download.text);
            ShowLeaderBoard(node);            
        }
    }    
    
       
    public void ShowLeaderBoard(SimpleJSON.JSONNode node)
    {
        LeaderBoardPanel.SetActive(true);
        SeasonalLBHolder.SetActive(false);

        if (ManageScene.CurScene ==Scenes.MainMenu)
        {
            MultiplayerPage.instance.HideLoadingScreen();
        }
  

        CPLeaderBoardData = null;

        if (""+node["CPLeaderboad"]["status"]=="1")
        {
            StartCoroutine(RemainingTime(Globalization.FloatParse(""+ node["CPLeaderboad"]["end_time"])));
            int count=node["CPLeaderboad"]["list"].Count;
            CPLeaderBoardData = new List<CpLeaderBoardData>(count);

            string N, R, C;
            bool me;
            for(int i=0;i< count; i++)
            {
                N = node["CPLeaderboad"]["list"][i]["username"];
                R = node["CPLeaderboad"]["list"][i]["rank"];
                C = node["CPLeaderboad"]["list"][i]["cp"];

                if (node["CPLeaderboad"]["list"][i]["myrank"] != null)
                    me = true;
                else
                    me = false;

                CPLeaderBoardData.Add(new CpLeaderBoardData(N,R,C,me));
            }           
            FeedDataintoUI();

            //CONTEST RELATED
            if (CONTROLLER.isBatMpContestRunning)
            {
                TitleText.text = "LEADERBOARD-DAILY";

                if (node["CPLeaderboad"]["contest"] != null)
                {
                    SeasonalLBHolder.SetActive(true);
                    Btn1.SetActive(false);
                    Btn2.SetActive(false);
                    if (node["CPLeaderboad"]["contest"]["btn1"] != null)
                    {
                        Btn1.SetActive(true);
                        Btn1Name.text = "" + node["CPLeaderboad"]["contest"]["btn1"];
                        Btn1URL = "" + node["CPLeaderboad"]["contest"]["btn1Link"];
                    }

                    if (node["CPLeaderboad"]["contest"]["btn2"] != null)
                    {
                        Btn2.SetActive(true);
                        Btn2Name.text = "" + node["CPLeaderboad"]["contest"]["btn2"];
                        Btn2URL = "" + node["CPLeaderboad"]["contest"]["btn2Link"];
                    }
                }
            }
            else
            {
                TitleText.text = "LEADERBOARD";
            }
        }


    }

    private void SortLeaderboard()
    {
        try
        {     
            CpLeaderBoardData temp;
             for (int i = 0; i < CPLeaderBoardData.Count; i++)
              {
                  for (int j = i + 1; j < CPLeaderBoardData.Count; j++)
                  {
                      if (int.Parse(CPLeaderBoardData[i].CpValue) < int.Parse(CPLeaderBoardData[j].CpValue))
                      {
                        temp = CPLeaderBoardData[i];
                        CPLeaderBoardData[i] = CPLeaderBoardData[j];
                        CPLeaderBoardData[j] = temp;
                      }
                  }
              }

            for (int i = 0; i <CPLeaderBoardData.Count; i++)
            {
                if (CPLeaderBoardData[i].Rank.Length == 1)
                    CPLeaderBoardData[i].Rank =""+(i+1);
            }
        }
        catch
        {
        }
    }
        
	

    private void FeedDataintoUI()
    {
        if (CPLeaderBoardData == null)
            return;

        foreach (GameObject go in Bar)
            go.SetActive(false);

        UsersBar.SetActive(false);
        SortLeaderboard();


        for (int i = 0; i < 3 && (i<CPLeaderBoardData.Count); i++)
        {
            Bar[i].SetActive(true);

            Name[i].text = CPLeaderBoardData[i].Name;
            CpValue[i].text = CPLeaderBoardData[i].CpValue;

            Rank[i].text = "" + (i + 1);
            if (i == 9)
                Rank[i].text = CPLeaderBoardData[i].Rank;


            //BG selection
            if (CPLeaderBoardData[i].IsMine)
            {
                UserBG[i].SetActive(true);
                OtherBG[i].SetActive(false);

                CpValue[i].color = Color.black;
                Name[i].color = Color.black;
                Rank[i].color = Color.black;

                CpValue[i].font = userfont;
                Name[i].font = userfont;
                Rank[i].font = userfont;

                
                int tmp = 0;
                if (int.TryParse(CPLeaderBoardData[i].CpValue,out tmp))
                {
                    CONTROLLER.CricketPoints = tmp;
                    PlayerPrefsManager.SaveCoins();
                }

                UsersBar.SetActive(true);
                UsersRank.text = Rank[i].text; 
                UsersName.text= CPLeaderBoardData[i].Name;
                UsersCpValue.text= CPLeaderBoardData[i].CpValue;
            }
            else
            {
                UserBG[i].SetActive(false);
                OtherBG[i].SetActive(true);

                CpValue[i].color = Color.white;
                Name[i].color = Color.white;
                Rank[i].color = Color.white;

                CpValue[i].font = othersfont;
                Name[i].font = othersfont;
                Rank[i].font = othersfont; 
            }



        }

    }

    public IEnumerator RemainingTime(float remTime)
    {
        if (remTime >= 86400)
        {
            SeasonResetText.text = TimeConversionDays("" + remTime);
        }
        else if (remTime >= 7200)
        {
            SeasonResetText.text = TimeConversionHours("" + remTime);
        }
        else
        {
            SeasonResetText.text = TimeConversion("" + remTime);
        }

        if (SeasonResetText.text == "00:00:00")
        {
            SeasonResetText.text = "";
            yield break;
        }
        else
        {
            yield return new WaitForSeconds(1f);
            remTime--;
            StartCoroutine("RemainingTime", remTime);
        }
    }
    private string TimeConversionHours(string _time)
    {
        TimeSpan _t = TimeSpan.FromSeconds(double.Parse(_time));
        return string.Format("{0:D2} HOURS", _t.Hours);
    }
    private string TimeConversionDays(string _time)
    {
        TimeSpan _t = TimeSpan.FromSeconds(double.Parse(_time));        
        return string.Format("{0:00} DAYS, {1:00} HOURS", _t.Days, _t.Hours);
    }
    private string TimeConversion(string _time)
    {
        TimeSpan _t = TimeSpan.FromSeconds(double.Parse(_time));
        return string.Format("{0:00}:{1:00}:{2:00}", _t.Hours, _t.Minutes, _t.Seconds);
    }


    public void ButtonClickEvents(int index)
    {
        if (index == 0)
        {
            Application.OpenURL(Btn1URL);
        }
        else if (index == 1)
        {
            Application.OpenURL(Btn2URL);
        }
        else if(index==2)//close button
        {
            HideMe();
        }
    }
}

public class CpLeaderBoardData
{
    public string Name { get; set; }
    public string Rank { get; set; }
    public string CpValue { get; set; }
    public bool IsMine { get; set; }

    public CpLeaderBoardData(string name,string rank,string cp,bool ismine)
    {
        this.Name = name;
        this.Rank = rank;
        this.CpValue = cp;
        this.IsMine = ismine;
    }
}
