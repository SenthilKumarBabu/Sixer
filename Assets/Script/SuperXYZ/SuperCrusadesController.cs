using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class SuperCrusadesController : MonoBehaviour
{
    public static SuperCrusadesController instance;
    [HideInInspector]
    //public List<SuperCrusadesDetails> SuperCrusadesData1;
    public List<SuperCrusadesDetails> SuperCrusadesData;
    public TextAsset JsonData;
    [Header("LevelSelection")]
    public Text modename;
    public GameObject Canvas_InGame;
    public SuperCrusadesUIinfo ChildObj_Season;
    public Transform seasonUIParents;
    public GameObject BGPanel;
    //public Sprite[] Bg;
    public GameObject SeasonGO;
    public GameObject MatchGO;
    //public GameObject Gamelogo;
    public Text matchUISeasonNO;
    public Text matchUISeasonYear;
    public Sprite PlayImage, LockImage,WatchVideoImage,replayImage;
    public SuperCrusadesUIinfo ChildObj_Match;
    public GameObject HomeBtn, BackBtn;
    public Text seasonTitleText;
    public Text ticketCount;
	public GameObject watchVideoPopup;
   

    [Header("GameOver")]
    public GameObject Canvas_GameOver;
    public Text[] ObjectiveTexts;
    public GameObject[] ObjectiveStars;
    public GameObject NextButton;
    public GameObject ShareButton;
    public GameObject RetryButton;
    public Text ticketCountText;
    public Text gameOver_SeasonText;
    public Text gameOver_MatchText;
    public Text gameOver_YearText;
    public GameObject Gameover_WinPanel;
    public GameObject Gameover_LosePanel;
    public GameObject Lose_RetryButton;
    public bool paid = false;
    public Text instruction;
    public GameObject instructionHolder;
    public GameObject blocker;
	public GameObject paymentPopup;
	public GameObject ticketPanel;
	public GameObject watchVideoPanel;
	public Text availabelTicketText;
	private bool ticketPanelActive = false;
	private int superMatchesButtonIndex;

    [Header("LevelInfo")]
    public GameObject Canvas_LevelInfo;
    public Text primaryObjText, SecondaryObjText, TertiaryObjText;

    public string[] oppTeamNameColor =
    {
        "PUNJAB",

        "MUMBAI",

        "KOLKATA",

        "DELHI",

        "RAJASTHAN",

        "DECCAN",

        "KOCHI",

        "PUNE",

        "HYDERABAD",

        "BANGALORE",
    };

    private void Awake()
    {
        instance = this;
        Canvas_InGame.SetActive(false);
        SeasonGO.SetActive(false);
        MatchGO.SetActive(false);
		//ParseCrusadesJson(JsonData.text);
		GetSeasonValueOnline();
	}

	public void GetSeasonValueOnline()
	{
		if(CONTROLLER.SeasonValue == "")
		{
			ParseCrusadesJson(JsonData.text);
		}
		else
		{
			ParseCrusadesJson(CONTROLLER.SeasonValue);
		}
	}

		public void ParseCrusadesJson(string text)
    {        
        SimpleJSON.JSONNode node = SimpleJSON.JSONNode.Parse(text);
        int TotalSeasonCount = node["SuperXYZ"].Count;    
        SuperCrusadesData = new List<SuperCrusadesDetails>(TotalSeasonCount);                
        int x = 0,y=0;
        for (x=0;x<TotalSeasonCount;x++)
        {
            SuperCrusadesDetails tmpObj = new SuperCrusadesDetails();
            tmpObj.SeasonName =node["SuperXYZ"][x]["name"];
            tmpObj.IsOpen=false;
            int MatchCount = node["SuperXYZ"][x]["matches"].Count;           
            tmpObj.MatchDatas = new List<SuperCrusadesMatchData>(MatchCount);
            for (y=0;y<MatchCount;y++)
            {                
                string PrimaryObj = node["SuperXYZ"][x]["matches"][y]["primary"]["objective"];
                string SecObj = node["SuperXYZ"][x]["matches"][y]["secondary"]["objective"];
                string ThirdObj = node["SuperXYZ"][x]["matches"][y]["tertiary"]["objective"];

                int PrimaryReward =node["SuperXYZ"][x]["matches"][y]["primary"]["reward"].AsInt;
                int SecReward =node["SuperXYZ"][x]["matches"][y]["secondary"]["reward"].AsInt;
                int ThirdReward =node["SuperXYZ"][x]["matches"][y]["tertiary"]["reward"].AsInt;

                SuperCrusadesMatchData _Matchdata = new SuperCrusadesMatchData();
                SuperCrusadesObjectives _objectives1 = new SuperCrusadesObjectives();
                SuperCrusadesObjectives _objectives2 = new SuperCrusadesObjectives();
                SuperCrusadesObjectives _objectives3 = new SuperCrusadesObjectives();

                //Primary Objective
                string[] splitArr = PrimaryObj.Split('|');
                _objectives1.ObjectiveType = SuperCrusadesObjectiveTypes.Score;
				_objectives1.ObjValue = int.Parse(splitArr[1]);  //hardcode
                _objectives1.RewardAmount = PrimaryReward;
                _Matchdata.PrimaryObjective = _objectives1;

                //Secondary Objective
                splitArr = SecObj.Split('|');
                if (splitArr[0] == "wicket")                
                    _objectives2.ObjectiveType = SuperCrusadesObjectiveTypes.Wicket;
                else if (splitArr[0] == "overs")
                    _objectives2.ObjectiveType = SuperCrusadesObjectiveTypes.Overs;
                else if (splitArr[0] == "four")
                    _objectives2.ObjectiveType = SuperCrusadesObjectiveTypes.Fours;
                else if (splitArr[0] == "six")
                    _objectives2.ObjectiveType = SuperCrusadesObjectiveTypes.Six;
                else if (splitArr[0] == "sixFour")
                    _objectives2.ObjectiveType = SuperCrusadesObjectiveTypes.SixFour;
                else if (splitArr[0] == "halfcentury")
                    _objectives2.ObjectiveType = SuperCrusadesObjectiveTypes.HalfCentury;
                else if (splitArr[0] == "century")
                    _objectives2.ObjectiveType = SuperCrusadesObjectiveTypes.Century;
                else if (splitArr[0] == "150runs")
                    _objectives2.ObjectiveType = SuperCrusadesObjectiveTypes.OneAndHalfCentury;
                else if (splitArr[0] == "doublecentury")
                    _objectives2.ObjectiveType = SuperCrusadesObjectiveTypes.DoubleCentury;
                else if (splitArr[0] == "boundary")
                    _objectives2.ObjectiveType = SuperCrusadesObjectiveTypes.Boundaries;

                if (splitArr[0] == "sixFour")
                {
                    string[] tmpVal = splitArr[1].Split('-');
                    _objectives2.ObjValue = int.Parse(tmpVal[0]);
                    _objectives2.ObjValue2 = int.Parse(tmpVal[1]);
                }
                else
                 _objectives2.ObjValue = int.Parse(splitArr[1]);

                _objectives2.RewardAmount = SecReward;
                _Matchdata.SecondaryObjective = _objectives2;

                //Tertiary Objective
                splitArr = ThirdObj.Split('|');
                if (splitArr[0] == "wicket")
                    _objectives3.ObjectiveType = SuperCrusadesObjectiveTypes.Wicket;
                else if (splitArr[0] == "overs")
                    _objectives3.ObjectiveType = SuperCrusadesObjectiveTypes.Overs;
                else if (splitArr[0] == "four")
                    _objectives3.ObjectiveType = SuperCrusadesObjectiveTypes.Fours;
                else if (splitArr[0] == "six")
                    _objectives3.ObjectiveType = SuperCrusadesObjectiveTypes.Six;
                else if (splitArr[0] == "sixFour")
                    _objectives3.ObjectiveType = SuperCrusadesObjectiveTypes.SixFour;
                else if (splitArr[0] == "halfcentury")
                    _objectives3.ObjectiveType = SuperCrusadesObjectiveTypes.HalfCentury;
                else if (splitArr[0] == "century")
                    _objectives3.ObjectiveType = SuperCrusadesObjectiveTypes.Century;
                else if (splitArr[0] == "150runs")
                    _objectives3.ObjectiveType = SuperCrusadesObjectiveTypes.OneAndHalfCentury;
                else if (splitArr[0] == "doublecentury")
                    _objectives3.ObjectiveType = SuperCrusadesObjectiveTypes.DoubleCentury;
                else if (splitArr[0] == "boundary")
                    _objectives3.ObjectiveType = SuperCrusadesObjectiveTypes.Boundaries;


                if (splitArr[0] == "sixFour")
                {
                    string[] tmpVal = splitArr[1].Split('-');
                    _objectives3.ObjValue = int.Parse(tmpVal[0]);
                    _objectives3.ObjValue2 = int.Parse(tmpVal[1]);
                }
                else
                    _objectives3.ObjValue = int.Parse(splitArr[1]);

                _objectives3.RewardAmount = ThirdReward;
                _Matchdata.TertiaryObjective = _objectives3;
                                
                if (node["SuperXYZ"][x]["matches"][y]["opp"] == null )                                    
                    _Matchdata.OppTeamName = "XYZ";                
                else                
                    _Matchdata.OppTeamName = node["SuperXYZ"][x]["matches"][y]["opp"];  


                _Matchdata.SetSavedLevelData(false, false, false, false);
                tmpObj.MatchDatas.Add(_Matchdata);
            }

            SuperCrusadesData.Add(tmpObj);

        }

        //for Opening the first match of the first season
        SuperCrusadesData[0].IsOpen = true;
        SuperCrusadesData[0].MatchDatas[0].IsOpen = true;
        ReadSavedCrusadesData();
        LoadSeasonUI();

        /* //for Testing
        for (int i = 0; i < SuperCrusadesData.Count; i++)
        {
            //DebugLogger.PrintWithColor(" seson name: " + SuperCrusadesData[i].SeasonName + " match count: " + SuperCrusadesData[i].MatchDatas.Count +" is Open: "+ SuperCrusadesData[i].IsOpen);
           for (int j = 0; j < SuperCrusadesData[i].MatchDatas.Count; j++)
            {
                //DebugLogger.PrintWithColor("Primary:::  Type:  " + SuperCrusadesData[i].MatchDatas[j].PrimaryObjective.ObjectiveType.ToString() + "  Value:  " + SuperCrusadesData[i].MatchDatas[j].PrimaryObjective.ObjValue + " reward:  " + SuperCrusadesData[i].MatchDatas[j].PrimaryObjective.RewardAmount + "   Secondary:::Type:  " + SuperCrusadesData[i].MatchDatas[j].SecondaryObjective.ObjectiveType.ToString() + "  Value:  " + SuperCrusadesData[i].MatchDatas[j].SecondaryObjective.ObjValue + "  reward:  " + SuperCrusadesData[i].MatchDatas[j].SecondaryObjective.RewardAmount + "  objVal 2: " + SuperCrusadesData[i].MatchDatas[j].SecondaryObjective.ObjValue2 + "   Tertiary :::Type: " + SuperCrusadesData[i].MatchDatas[j].TertiaryObjective.ObjectiveType.ToString() + "  Value:  " + SuperCrusadesData[i].MatchDatas[j].TertiaryObjective.ObjValue + " reward: " + SuperCrusadesData[i].MatchDatas[j].TertiaryObjective.RewardAmount + "  objVal 2: " + SuperCrusadesData[i].MatchDatas[j].TertiaryObjective.ObjValue2 + "  :::::::: isOpen : " + SuperCrusadesData[i].MatchDatas[j].IsOpen + " 1stDone: " + SuperCrusadesData[i].MatchDatas[j].IsPrimaryObjAchieved + " 2nd: " + SuperCrusadesData[i].MatchDatas[j].IsSecondaryObjAchieved + " 3rd: " + SuperCrusadesData[i].MatchDatas[j].IsTertiaryObjAchieved);
            }
        }*/
    }   
    private void StartGame()
    {
        GameModel.instance.ResetVariables();
        GameModel.instance.ResetCurrentMatchDetails();
        GameModel.instance.ResetAllLocalVariables();
        BattingScoreCard.instance.ResetPlayerImages();
        BattingScoreCard.instance.ResetBattingCard();
        Canvas_InGame.SetActive(false);
        Canvas_GameOver.SetActive(false);
        MatchGO.SetActive(false);

        AudioPlayer.instance.PlayButtonSnd();        
        CONTROLLER.TargetToChase = SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx ].PrimaryObjective.ObjValue;
        CONTROLLER.InningsCompleted = false;
        CONTROLLER.canShowMainCamera = true;
        GameModel.instance.NewInnings();
        BattingScoreCard.instance.FirstTimeContinueClicked = false;

        GroundController.instance.ChangePlayerLeftRightTextures();
        ShowLevelInfo();
    }
    private void ShowLevelInfo()
    {
        Canvas_LevelInfo.SetActive(true);
        primaryObjText.text = "" + GetTextBasedOnObjectivs(SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].PrimaryObjective.ObjectiveType, SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].PrimaryObjective.ObjValue, SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].PrimaryObjective.ObjValue2);
        SecondaryObjText.text = "" + GetTextBasedOnObjectivs(SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].SecondaryObjective.ObjectiveType, SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].SecondaryObjective.ObjValue, SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].SecondaryObjective.ObjValue2);
        TertiaryObjText.text = "" + GetTextBasedOnObjectivs(SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].TertiaryObjective.ObjectiveType, SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].TertiaryObjective.ObjValue, SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].TertiaryObjective.ObjValue2);        
    }

    #region ButtonClicks
    public void HomeButtonClickEvent()
    {
        if (MatchGO.activeSelf)
        {
            ShowSeasonUI();
        }
        else
        {
            TeamSelection.playerCount = 0;
            TeamSelection.foreignPlayerCount = 0;
            CONTROLLER.canShowMainCamera = true;
            CONTROLLER.SelectedCrusadeSeasonIdx = -1;
            CONTROLLER.SelectedCrusadeMatchIdx = -1;
            StartCoroutine(GameModel.instance.GameQuitted());
        }
		AdIntegrate.instance.HideAd();
    }
    public void SeasonButtonClickedEvent(GameObject _Go)
    {
		if (int.Parse(_Go.name) <= CONTROLLER.seasonUnlock - 1)
        {
            string toastMessage = "";
            CONTROLLER.SelectedCrusadeSeasonIdx = int.Parse(_Go.name);

            if (SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].IsOpen) 
				ShowMatchesUI(); 
			else
            {
                if (CONTROLLER.SelectedCrusadeSeasonIdx <= CONTROLLER.seasonUnlock - 1)
                {
                    toastMessage = "Complete previous season to unlock";
                }
                else
                {
                    toastMessage = "This season will be unlocked soon";
                }
//#if UNITY_ANDROID && !UNITY_EDITOR
//					NextwaveMarshmallowPermission .instance .ShowToast (toastMessage); 
//#else
				ShowToast(toastMessage);
//#endif
            }
        }
        else
        {
//#if UNITY_ANDROID && !UNITY_EDITOR
//					NextwaveMarshmallowPermission .instance .ShowToast ("This season will be unlocked soon"); 
//#else
			ShowToast("This season will be unlocked soon");
//#endif
        }

    }
    public void MatchButtonClickedEvent(GameObject _Go)
    {
        CONTROLLER.SelectedCrusadeMatchIdx = int.Parse(_Go.name);
       
        if ((SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].IsOpen || CONTROLLER.SelectedCrusadeMatchIdx==0))
        {
			DeductTicket();
			if (paid)
			{
				CallStartGame();
				//StartGame();
				//PlayerPrefs.DeleteKey(CONTROLLER.SUPER_Crusade_WatchVideoLevelID);
				//GroundController.instance.SetFielderUniformColor();
			}
			else
			{

				if (AdIntegrate.instance.checkTheInternet() && AdIntegrate.instance.isRewardedReadyToPlay())
				{
					CONTROLLER.RewardedVideoClickedState = 9;
					watchVideoPopup.SetActive(true);
				}
				else
				{
					ShowToast("Insufficient tickets! \nBuy tickets from the store to continue.");
				}				
			}
        }            
        else
        {
            if (_Go.GetComponent<SuperCrusadesUIinfo>().WatchVideo.activeSelf)
            {
                WatchVidoButtonClickedEvent(7);
            }
            else
            {
                string toastMessage = "";
                if (!SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].IsOpen)
                {
                    toastMessage = " Complete previous levels to unlock";
                }
                else
                {
                    toastMessage = " Insufficient tickets! \nBuy tickets from the store to continue.";
                }
                DebugLogger.PrintWithSize("**************************");
//#if UNITY_ANDROID && !UNITY_EDITOR
//					NextwaveMarshmallowPermission .instance .ShowToast (toastMessage); 
//#else
				ShowToast(toastMessage);
//#endif
            }
        }
		AdIntegrate.instance.HideAd();
        
    }

	public void CallStartGame()
	{
		
		StartGame();
		PlayerPrefs.DeleteKey(CONTROLLER.SUPER_Crusade_WatchVideoLevelID);
		GroundController.instance.SetFielderUniformColor();
	}

	public void CallShowRewardedVideo()
	{
		AdIntegrate.instance.ShowRewardedVideo();
	}

	public void OpenStore()
	{
		CONTROLLER.openStore = 1;
		MatchGO.SetActive(false);
		GameOverButtonClickEvents(0);
	}

    public void LevelInfoContinueButtonClickEvent()
    {
        Canvas_LevelInfo.SetActive(false);
        BattingScoreCard.instance.ShowMe();
    }
    #endregion

    #region UI
    [SerializeField]
    private List<SuperCrusadesUIinfo> SeasonUI;
    private List<SuperCrusadesUIinfo> MatchUI;
    private void LoadSeasonUI()
    {
        //season ui Instantiaiton
        Transform tempParent = null;
        SeasonUI= new List<SuperCrusadesUIinfo>();
        for (int k =0; k < SuperCrusadesData.Count-1; k++)
        {
            tempParent = seasonUIParents;//[((k+1) % 3)];           
            SuperCrusadesUIinfo Go = Instantiate(ChildObj_Season, tempParent);
            SeasonUI.Add(Go);
        }

        //Match ui Instantiaiton
        MatchUI = new List<SuperCrusadesUIinfo>();
        for (int k = 0; k < 25; k++)
        {
            SuperCrusadesUIinfo Go = Instantiate(ChildObj_Match, ChildObj_Match.transform.parent);
            MatchUI.Add(Go);
        }
    }
    public void ShowSeasonUI()
    {
       CONTROLLER.CurrentPage = "cruseadeSeasonUI";
        Canvas_InGame.SetActive(true);
        Canvas_GameOver.SetActive(false);
        //BGPanel.GetComponent<Image>().sprite = Bg[0];
        modename.gameObject.SetActive(true);
        SeasonGO.SetActive(true);
        MatchGO.SetActive(false);
        //Gamelogo.SetActive(true);
        CONTROLLER.SelectedCrusadeSeasonIdx = -1;
        CONTROLLER.SelectedCrusadeMatchIdx = -1;


        HomeBtn.SetActive(true);
        BackBtn.SetActive(false);

        ChildObj_Season.TitleText.text = "SEASON 01";  //SuperCrusadesData[0].SeasonName;
        ChildObj_Season.seasonYear.text = GetSeasonYear(SuperCrusadesData[0].SeasonName);
        ChildObj_Season.isOpen = true;
        ChildObj_Season.LockIcon.SetActive(false);
     //   ChildObj_Season.PlayIcon.sprite = PlayImage;

       

        for (int i =0; i <SeasonUI.Count; i++)
        {
            //Debug.LogError(SeasonUI.Count);
            SuperCrusadesUIinfo Go = SeasonUI[i];
            Go.TitleText.text = "SEASON 0" + (i + 2).ToString();//SuperCrusadesData[i+1].SeasonName;
            //Debug.LogError(SeasonUI.Count);
            if (i >= SeasonUI.Count - 2)
            {
                //Debug.LogError(i+2);
                Go.TitleText.text = "SEASON " + (i + 2).ToString();//SuperCrusadesData[i+1].SeasonName;
            }
            Go.seasonYear.text = GetSeasonYear( SuperCrusadesData[i + 1].SeasonName);
            if (i > CONTROLLER.seasonUnlock -2)                      //to lock seasons more then 2 arun
            {
                Go.LockIcon.SetActive(true);
            }
            else
            {
                Go.LockIcon.SetActive(false);
            }
            if (SuperCrusadesData[i+1].IsOpen)       //if season is open
            {
                Go.isOpen = true;
                //  Go.PlayIcon.sprite = PlayImage;
                //Go.LockIcon.SetActive(false);
            }
            else
            {
                Go.isOpen = false;
                //   Go.PlayIcon.sprite = LockImage;
               // Go.LockIcon.SetActive(true);
            }
            Go.name = (i+1).ToString();
        }
    }
    public void ShowMatchesUI()
    {
        CONTROLLER.CurrentPage = "cruseadeMatchUI";
        Canvas_GameOver.SetActive(false);
        Canvas_InGame.SetActive(true);
        SeasonGO.SetActive(false);
        //BGPanel.GetComponent<Image>().sprite = Bg[1];
        modename.gameObject.SetActive(false);
        MatchGO.SetActive(true);
        HomeBtn.SetActive(false);
        //Gamelogo.SetActive(false);
        BackBtn.SetActive(true);
        seasonTitleText.text = SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].SeasonName;
        matchUISeasonNO.text = "SEASON 0" + (CONTROLLER.SelectedCrusadeSeasonIdx + 1).ToString();
        if (CONTROLLER.SelectedCrusadeSeasonIdx >= 9)
        {
            matchUISeasonNO.text = "SEASON " + (CONTROLLER.SelectedCrusadeSeasonIdx + 1).ToString();
        }
        matchUISeasonYear.text = GetSeasonYear(SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].SeasonName);
        ticketCountText.text = "" + UserProfile.Tickets;

        for (int m=0;m<MatchUI.Count;m++)
        MatchUI[m].transform.gameObject.SetActive(false);

        ChildObj_Match.TitleText.text = "1"; //"CSK vs "+SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[0].OppTeamName;// "Match 1";
      //  ChildObj_Match.TargetScoreText.text = "Target " +SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[0].PrimaryObjective.ObjValue;
        ChildObj_Match.isOpen = true;
        LockMatch(0,false);
      //  ChildObj_Match.PlayIcon.sprite = PlayImage;
        
        ChildObj_Match.Star1.enabled = SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[0].IsPrimaryObjAchieved;
        ChildObj_Match.Star2.enabled = SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[0].IsSecondaryObjAchieved;
        ChildObj_Match.Star3.enabled = SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[0].IsTertiaryObjAchieved;


        for (int i = 1; i < SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas.Count; i++)
        {
            SuperCrusadesUIinfo Go = MatchUI[i];   
            Go.transform.gameObject.SetActive(true);
            Go.TitleText.text = (i + 1).ToString();//"CSK vs " + SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[i].OppTeamName;       // "Match " + (i + 1);
          //  Go.TargetScoreText.text = "Target " + SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[i].PrimaryObjective.ObjValue;
            
            if (SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[i].IsOpen)      
            {
                Go.isOpen = true;
                LockMatch(i, false);
                //   Go.PlayIcon.sprite = PlayImage;
            }
            else
            {
                Go.isOpen = false;
                LockMatch(i,true);
            //    Go.PlayIcon.sprite = LockImage;
            }

            Go.Star1.enabled = SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[i].IsPrimaryObjAchieved;
            Go.Star2.enabled = SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[i].IsSecondaryObjAchieved;
            Go.Star3.enabled = SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[i].IsTertiaryObjAchieved;

            Go.name = i.ToString();
        }

		//placing Watch video icon  // Video hard code
		/*for (int i = 1; i < SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas.Count; i++)
		{
			SuperCrusadesUIinfo Go = MatchUI[i];
			if (!SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[i].IsOpen)
			{
				if (AdIntegrate.instance.isRewardedReadyToPlay()  )  
				{
					Go.WatchVideo.SetActive(true);
					Go.LockIcon.SetActive(false);
				}
				break;
			}
		}*/
	}

    public void LockMatch(int index , bool state)
    {
        MatchUI[index].TitleText.gameObject.SetActive(!state);
        MatchUI[index].starHolder.SetActive(!state);
        MatchUI[index].LockIcon.SetActive(state);
        MatchUI[index].WatchVideo.SetActive(false);
    }
     
    public string GetSeasonYear(string name)
    {
        string[] splitedName = new string[2];
        splitedName = name.Split('-');
        return splitedName[1];
    }

    #endregion

    #region WatchVideo
    private void WatchVidoButtonClickedEvent(int num)
    {
        if (AdIntegrate.instance != null)
        {
            if (AdIntegrate.instance.checkTheInternet())
            {
                if (AdIntegrate.instance.isRewardedReadyToPlay())
                {
                    CONTROLLER.RewardedVideoClickedState = num;   //7- Super Crusade level open     8-Reply
                    AdIntegrate.instance.ShowRewardedVideo();
                }
                else
                {
//#if UNITY_ANDROID && !UNITY_EDITOR
//					NextwaveMarshmallowPermission .instance .ShowToast ("No video Available"); 
//#else
					ShowToast("No video Available");
//#endif
                }
            }

            else
            {
//#if UNITY_ANDROID && !UNITY_EDITOR
//				NextwaveMarshmallowPermission .instance .ShowToast ("You're not connected to the internet"); 
//#else
				ShowToast("You're not connected to the internet");
//#endif
            }

        }
    }
    void ShowToast(string text)
    {
        GameObject prefabGO;
        GameObject tempGO;
        prefabGO = Resources.Load("Prefabs/Toast") as GameObject;
        tempGO = Instantiate(prefabGO) as GameObject;
        tempGO.name = "Toast";
        tempGO.GetComponent<Toast>().setMessge(text);
    }
    public void WatchVideoSuccessEvent()
    {
        StartGame();
        PlayerPrefs.SetInt(CONTROLLER.SUPER_Crusade_WatchVideoLevelID, CONTROLLER.SelectedCrusadeMatchIdx);
        CONTROLLER.RewardedVideoClickedState = -1;
    }
    public void WatchVideoSuccessEvent_Replay()
    {
        Canvas_GameOver.SetActive(false);
        CONTROLLER.TargetToChase = SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].PrimaryObjective.ObjValue;
        CONTROLLER.NewInnings = true;
        GameModel.instance.ReStartGame();
        CONTROLLER.RewardedVideoClickedState = -1;
    }
    #endregion


    #region GameOver
    public void ShowGameOver()
    {
        Canvas_GameOver.SetActive(true);
        Gameover_WinPanel.SetActive(false);
        Gameover_LosePanel.SetActive(false);
		BattingScoreCard.instance.HideMe();

		//arun bug fix
		PlayerPrefs.DeleteKey(CONTROLLER.SUPER_Crusade_SavedMatchDetails);
		PlayerPrefs.DeleteKey(CONTROLLER.SUPER_Crusade_PlayerDetails);

		ObjectiveTexts[0].text = ObjectiveTexts[1].text = ObjectiveTexts[2].text = string.Empty;
        ObjectiveStars[0].SetActive(false); ObjectiveStars[1].SetActive(false); ObjectiveStars[2].SetActive(false);

        if (CONTROLLER.currentMatchScores >= CONTROLLER.TargetToChase-1)
        {
            Gameover_WinPanel.SetActive(true);
            gameOver_SeasonText.text = "SEASON " + (CONTROLLER.SelectedCrusadeSeasonIdx+1).ToString();
            gameOver_MatchText.text = "MATCH "+ (CONTROLLER.SelectedCrusadeMatchIdx+1).ToString();
            gameOver_YearText.text = GetSeasonYear(SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].SeasonName);
            NextButton.SetActive(false);
            ShareButton.SetActive(true);

            ObjectiveTexts[0].text = "1. " + GetTextBasedOnObjectivs(SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].PrimaryObjective.ObjectiveType, SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].PrimaryObjective.ObjValue, SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].PrimaryObjective.ObjValue2);
            ObjectiveTexts[1].text = "2. " + GetTextBasedOnObjectivs(SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].SecondaryObjective.ObjectiveType, SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].SecondaryObjective.ObjValue, SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].SecondaryObjective.ObjValue2);
            ObjectiveTexts[2].text = "3. " + GetTextBasedOnObjectivs(SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].TertiaryObjective.ObjectiveType, SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].TertiaryObjective.ObjValue, SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].TertiaryObjective.ObjValue2);
            

            //star Validation
            //DebugLogger.PrintWithColor(" fous: " + CONTROLLER.totalFours + " six: " + CONTROLLER.totalSixes + " wicket: " + CONTROLLER.currentMatchWickets + " overs: " + (CONTROLLER.currentMatchBalls / 6) + " haflCentury Count: " + GetCenturyAndHalfCenturyCount(0) + " centuryCount: " + GetCenturyAndHalfCenturyCount(1));
            ObjectiveStars[0].SetActive(true);
            ObjectiveStars[1].SetActive(GetValidationofObjectivs(SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].SecondaryObjective.ObjectiveType, SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].SecondaryObjective.ObjValue, SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].SecondaryObjective.ObjValue2));
            ObjectiveStars[2].SetActive(GetValidationofObjectivs(SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].TertiaryObjective.ObjectiveType, SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].TertiaryObjective.ObjValue, SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].TertiaryObjective.ObjValue2));

            //ticket Adding
            int ticketToAdd = 0;
            if (!SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].IsPrimaryObjAchieved)
                ticketToAdd = 1;            
            if (!SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].IsSecondaryObjAchieved && ObjectiveStars[1].activeSelf)
                ticketToAdd += 1;
            if (!SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].IsTertiaryObjAchieved && ObjectiveStars[2].activeSelf)
                ticketToAdd += 2;
            TicketToAdd(ticketToAdd);

            ticketCount.text =""+ticketToAdd;

            //data Saving
            SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].IsOpen = true;
            SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].IsOpen = true;
            SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].IsPrimaryObjAchieved = true;
            SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].IsSecondaryObjAchieved = ObjectiveStars[1].activeSelf;
            SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].IsTertiaryObjAchieved = ObjectiveStars[2].activeSelf;

            
            //Next level open
            if ((CONTROLLER.SelectedCrusadeMatchIdx + 1) < SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas.Count && !PlayerPrefs.HasKey(CONTROLLER.SUPER_Crusade_WatchVideoLevelID))
            {
                //SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx + 1].IsOpen = true;

                bool previousLevelsNotAchieved = false;
                for (int i=0;i<CONTROLLER.SelectedCrusadeMatchIdx;i++)
                {
                    if(!SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[i].IsPrimaryObjAchieved)
                    {
                        previousLevelsNotAchieved = true;
                    }
                }

                if (!previousLevelsNotAchieved)
                {
                    for (int i = CONTROLLER.SelectedCrusadeMatchIdx + 1; i < SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas.Count; i++)
                    {						
						if (!SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[i].IsPrimaryObjAchieved && !SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[i].IsOpen)
						{
							bool previousLevelsNotAchieved2 = false;
							for (int x = 0; x < i; x++)
							{
								if (!SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[x].IsPrimaryObjAchieved)
								{
									previousLevelsNotAchieved2 = true;
								}
							}
							if (!previousLevelsNotAchieved2)
							{
								SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[i].IsOpen = true;
								break;
							}
						}						
					}
				}
            }
			//
			
            //Next Season open
            if ((CONTROLLER.SelectedCrusadeSeasonIdx + 1) < SuperCrusadesData.Count )
            {

				if (!PlayerPrefs.HasKey(CONTROLLER.SUPER_Crusade_WatchVideoLevelID))
                {
                    bool bflag = true;
                    for (int i = 0; i < SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas.Count; i++)
                    {
                        if (!SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[i].IsPrimaryObjAchieved)
                        {
                            bflag = false;
                            break;
                        }
                    }
                    if (bflag)
                    {
                        SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx + 1].IsOpen = true;
                        SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx + 1].MatchDatas[0].IsOpen = true;
                    }
                }
            }
            //

            //Next button Checking
            bool canopenNextbtn =true;
            for (int i = 0; i < CONTROLLER.SelectedCrusadeMatchIdx; i++)
            {
                if (!SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[i].IsPrimaryObjAchieved)
                {
                    canopenNextbtn = false;
                    break;
                }
            }
            if (PlayerPrefs.HasKey(CONTROLLER.SUPER_Crusade_WatchVideoLevelID) || ( (CONTROLLER.SelectedCrusadeSeasonIdx + 1) >= SuperCrusadesData.Count && CONTROLLER.SelectedCrusadeMatchIdx+1 == SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas.Count && (CONTROLLER.SelectedCrusadeSeasonIdx + 1) < CONTROLLER.seasonUnlock))
                canopenNextbtn = false;

            NextButton.SetActive(canopenNextbtn);
            //

            WriteCrusadesData();


			if((CONTROLLER.SelectedCrusadeSeasonIdx + 1) >= CONTROLLER.seasonUnlock && CONTROLLER.SelectedCrusadeMatchIdx + 1 == SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas.Count )
				 NextButton.SetActive(false);

		}
		else
        {
            Gameover_LosePanel.SetActive(true);           

            if (SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].IsPrimaryObjAchieved)
            {
                //Normal retry image
                Lose_RetryButton.SetActive(true);
                Lose_RetryButton.transform.GetChild(0).GetComponent<Image>().sprite = replayImage;
            }
            else if (!SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].IsPrimaryObjAchieved && AdIntegrate.instance.checkTheInternet() && AdIntegrate.instance.isRewardedReadyToPlay())
            {
                //retry with watch video image
                Lose_RetryButton.SetActive(true);
                Lose_RetryButton.transform.GetChild(0).GetComponent<Image>().sprite = WatchVideoImage;
            }
            else
                Lose_RetryButton.SetActive(false);
        }

        SetReplayButDisabled();
		//FirebaseAnalyticsManager.instance.SCS_Finish();
		//Deiva supercru completed
    }
   private void SetReplayButDisabled()
    {
        //disable next button in watch video case
        if (PlayerPrefs.HasKey(CONTROLLER.SUPER_Crusade_WatchVideoLevelID))
        {
            RetryButton.SetActive(false);
            NextButton.SetActive(false);
            PlayerPrefs.DeleteKey(CONTROLLER.SUPER_Crusade_WatchVideoLevelID);
        }
    }

    private bool GetValidationofObjectivs(SuperCrusadesObjectiveTypes objType, int val1, int val2)
    {
        bool flag = false;
        switch (objType)
        {
            case SuperCrusadesObjectiveTypes.Score:
                if (CONTROLLER.currentMatchScores >= val1)
                    flag =true;
                break;
            case SuperCrusadesObjectiveTypes.Wicket:
                if (CONTROLLER.currentMatchWickets <= val1)
                    flag = true;                        
                break;
            case SuperCrusadesObjectiveTypes.Overs:
                if ((CONTROLLER.currentMatchBalls / 6) < val1)
                    flag = true;
                break;
            case SuperCrusadesObjectiveTypes.Fours:
                if (CONTROLLER.totalFours >= val1)
                    flag = true;
                break;
            case SuperCrusadesObjectiveTypes.Six:
                if (CONTROLLER.totalSixes>= val1)
                    flag = true;
                break;
            case SuperCrusadesObjectiveTypes.SixFour:
                if (CONTROLLER.totalSixes >= val1 && CONTROLLER.totalFours >= val2)
                    flag = true;
                break;
            case SuperCrusadesObjectiveTypes.HalfCentury:
                if (GetCenturyAndHalfCenturyCount(0) >= val1)
                    flag = true;
                break;
            case SuperCrusadesObjectiveTypes.Century:
                if (GetCenturyAndHalfCenturyCount(1) >= val1)
                    flag = true;
                break;
            case SuperCrusadesObjectiveTypes.OneAndHalfCentury:
                if (GetCenturyAndHalfCenturyCount(2) >= val1)
                    flag = true;
                break;
            case SuperCrusadesObjectiveTypes.DoubleCentury:
                if (GetCenturyAndHalfCenturyCount(3) >= val1)
                    flag = true;
                break;
            case SuperCrusadesObjectiveTypes.Boundaries:                
                if ( (CONTROLLER.totalSixes + CONTROLLER.totalFours) >= val1)
                    flag = true;
                break;
        }

        return flag;
    }


    private int GetCenturyAndHalfCenturyCount(int type)
    {
        //where type=0 for HalfCentury and type=1 for Century
        int flag = 0,Cap=100;

        if (type == 0)
            Cap = 50;
        else if (type == 1)
            Cap = 100;
        else if (type == 2)
            Cap = 150;
        else if (type == 3)
            Cap = 200;

        for (int i=0;i<11;i++)
        {
            if (CONTROLLER.TeamList[0].PlayerList[i].RunsScored > Cap)
                flag++;
        }

        return flag;
    }
    private string GetTextBasedOnObjectivs(SuperCrusadesObjectiveTypes objType,int val1,int val2)
    {
        string flag = string.Empty;
        switch (objType)
        {
            case SuperCrusadesObjectiveTypes.Score: flag ="Score "+val1+" runs.";  break;
            case SuperCrusadesObjectiveTypes.Wicket:
                if(val1==0)
                    flag = "Don't lose any wickets.";
                else if(val1==1)
                    flag = "Don't lose more than "+val1+" wicket.";
                else
                    flag = "Don't lose more than " + val1 + " wickets.";
                break;
            case SuperCrusadesObjectiveTypes.Overs: flag = "Score within "+val1+" overs.";  break; 
            case SuperCrusadesObjectiveTypes.Fours: flag = "Hit "+val1+" fours.";  break;
            case SuperCrusadesObjectiveTypes.Six: flag = "Hit "+val1+" sixes.";  break;
            case SuperCrusadesObjectiveTypes.SixFour: flag = "Hit "+val1+ " sixes and "+val2+ " fours.";  break;
            case SuperCrusadesObjectiveTypes.HalfCentury:
                if(val1==1)
                    flag = "Hit " + val1 + " half-century.";
                else
                    flag = "Hit "+val1 +" half-centuries.";
                break;
            case SuperCrusadesObjectiveTypes.Century:
                if(val1==1)
                    flag = "Hit " + val1 + " century."; 
                else
                    flag = "Hit "+val1 +" centuries.";
                break;
            case SuperCrusadesObjectiveTypes.OneAndHalfCentury:                
                    flag = "Hit 150 runs with 1 batsman.";               
                break;

            case SuperCrusadesObjectiveTypes.DoubleCentury:
                if (val1 == 1)
                    flag = "Hit " + val1 + " double century.";
                else
                    flag = "Hit " + val1 + " double centuries.";
                break;

            case SuperCrusadesObjectiveTypes.Boundaries: flag = "Hit " + val1  +" boundaries."; break;

        }

        return flag;
    }

	public void ShowPaymentPopup(int index)
	{
		int tempselectedMatchIdx = 0;
		int tempselectedSeasonIdx = 0;
		ticketPanelActive = false;
		superMatchesButtonIndex = index;
		if(index == 2)
		{
			tempselectedMatchIdx = CONTROLLER.SelectedCrusadeMatchIdx;
			tempselectedSeasonIdx = CONTROLLER.SelectedCrusadeSeasonIdx;
		}
		else
		{
			tempselectedMatchIdx = CONTROLLER.SelectedCrusadeMatchIdx + 1;
			tempselectedSeasonIdx = CONTROLLER.SelectedCrusadeSeasonIdx;
		}
		//Debug.LogError("(SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas.Count-1)"+ (SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas.Count - 1));
		if ((CONTROLLER.SelectedCrusadeMatchIdx >= (SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas.Count - 1) && index == 3))
		{			
			CallGameOverButtonClick();
			return;
		}
	    if ( SuperCrusadesData[tempselectedSeasonIdx].MatchDatas[tempselectedMatchIdx].IsPrimaryObjAchieved && SuperCrusadesData[tempselectedSeasonIdx].MatchDatas[tempselectedMatchIdx].IsSecondaryObjAchieved && SuperCrusadesData[tempselectedSeasonIdx].MatchDatas[tempselectedMatchIdx].IsTertiaryObjAchieved)
		{
			ticketPanelActive = true;
			CallGameOverButtonClick();
			return;
		}
		else
		{
			if (AdIntegrate.instance.checkTheInternet() && AdIntegrate.instance.isRewardedReadyToPlay() && index != 3)
			{
				watchVideoPanel.SetActive(true);
				ticketPanel.SetActive(false);
				ticketPanelActive = false;
			}
			else
			{
				watchVideoPanel.SetActive(false);
				ticketPanel.SetActive(true);
				ticketPanelActive = true;
				availabelTicketText.text = "You need 1 ticket to play the match.\n\nAVAILABLE TICKETS: " + UserProfile.Tickets;
			}
			paymentPopup.SetActive(true);
		}
		
	}

	public void CallGameOverButtonClick()
	{
	//	paymentPopup.SetActive(false);
		GameOverButtonClickEvents(superMatchesButtonIndex);
	}

    public void GameOverButtonClickEvents(int id)
    {
        switch(id)
        {
            case 0: HomeButtonClickEvent(); break;  
            case 1: ShareChallenge();    break;
            case 2://restart
				if (ticketPanelActive == false)//AdIntegrate.instance.checkTheInternet() && AdIntegrate.instance.isRewardedReadyToPlay())
				{
					WatchVidoButtonClickedEvent(8);
				}
				else
				{
					DeductTicket();
					if (paid)//SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].IsPrimaryObjAchieved)
					{
						WatchVideoSuccessEvent_Replay();
					}
					else
					{
						ShowToast("Insufficient tickets! \nBuy tickets from the store to continue.");
					}
				}                
				    //Supercrusader
                break;    
            case 3://next
				//if (CONTROLLER.SelectedCrusadeSeasonIdx + 1 > 1 && CONTROLLER.SelectedCrusadeMatchIdx + 1 == 16)
				//{
				//	// Debug.LogError("CONTROLLER.SelectedCrusade");
				//	break;
				//}
				//DebugLogger.PrintWithColor("next button clicked::: paid::: " + paid);
               
                
                    if (((CONTROLLER.SelectedCrusadeMatchIdx + 1) >= SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas.Count) && ((CONTROLLER.SelectedCrusadeSeasonIdx + 1) < SuperCrusadesData.Count))
                    {
                        CONTROLLER.SelectedCrusadeSeasonIdx++;
					//FirebaseAnalyticsManager.instance.SCS_Start_res();
					//FirebaseAnalyticsManager.instance.SCS_fin_res();
                        ShowMatchesUI();
                    }
                    else
                    {
                        CONTROLLER.SelectedCrusadeMatchIdx++;
                        DeductTicket();
                        if (paid)
                        {
                            CONTROLLER.NewInnings = true;
                            CONTROLLER.InningsCompleted = false;
                            GroundController.instance.ResetYield();
                            GameModel.instance.ResetVariables();

                            GameModel.instance.ResetCurrentMatchDetails();
                            GameModel.instance.ResetAllLocalVariables();
                            BattingScoreCard.instance.ResetPlayerImages();
                            BattingScoreCard.instance.ResetBattingCard();
                            StartGame();
                        }
                        else
                        {
                            CONTROLLER.SelectedCrusadeMatchIdx--;
                            ShowToast("Insufficient tickets! \nBuy tickets from the store to continue.");
                        }

                    }                
                
                break;     

            case 4: instructionHolder.SetActive(true);blocker.SetActive(true); instruction.text = CONTROLLER.SUPER_Crusade_Instructions; break;

        }
		paymentPopup.SetActive(false);
	}

    public void ShareChallenge()
    {
        string subject = CONTROLLER.AppName;
        string body = "";        

        body = CONTROLLER.UserName + " completed Match " + (CONTROLLER.SelectedCrusadeMatchIdx  + 1) + " in Season "+(CONTROLLER.SelectedCrusadeSeasonIdx+1)+ " of SuperMatches mode in " + CONTROLLER.AppName + " - " + " You can download from " + CONTROLLER.AppLink;

        //execute the below lines if being run on a Android device
#if UNITY_ANDROID &&!UNITY_EDITOR
        //Refernece of AndroidJavaClass class for intent
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        //Refernece of AndroidJavaObject class for intent
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        //call setAction method of the Intent object created
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        //set the type of sharing that is happening
        intentObject.Call<AndroidJavaObject>("setType", "text/plain");
        //intentObject.Call<AndroidJavaObject>("setType", "message/rfc822");
        //add data to be passed to the other activity i.e., the data to be sent
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body);
        //get the current activity
        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        //start the activity by sending the intent data
        currentActivity.Call("startActivity", intentObject);
#endif
#if UNITY_IOS
		ShareThisGame.sendTextWithOutPath (body);
#endif
    }


    private void TicketToAdd(int count)
    {        
        UserProfile.EarnedTickets += count;
        UserProfile.Tickets += count;
        PlayerPrefsManager.SaveCoins();                
       // StartCoroutine(CricMinisWebRequest.instance.UserSync());
    }
    #endregion

    #region SaveRetrive 
    private void WriteCrusadesData()
    {
        int i, j;
        int totalSeasonCount = SuperCrusadesData.Count;
        string[] tmpSeasonStr = new string[totalSeasonCount];
        string[] tmpMatchStr = new string[totalSeasonCount];
        for (i = 0; i < totalSeasonCount; i++)
        {
            //season data
            tmpSeasonStr[i] = JsonUtility.ToJson(SuperCrusadesData[i]); 
            
            //Match data
            int totalMatchCount = SuperCrusadesData[i].MatchDatas.Count;
            string[] tmpMatchDatas = new string[totalMatchCount];
            for (j = 0; j < totalMatchCount; j++)
            {
                tmpMatchDatas[j] = JsonUtility.ToJson(SuperCrusadesData[i].MatchDatas[j]);
            }
            tmpMatchStr[i] = string.Join("|", tmpMatchDatas);
        }
        string seasonStr = string.Join("-",tmpSeasonStr);
        string matchStr = string.Join("~", tmpMatchStr);
        string MajorStr = seasonStr + "$" + matchStr;
        ObscuredPrefs.SetString(CONTROLLER.SUPER_Crusade_SavedName,MajorStr);
    }

    private void ReadSavedCrusadesData()
    {        
        string MainString = ObscuredPrefs.GetString(CONTROLLER.SUPER_Crusade_SavedName,string.Empty);
        if (MainString == string.Empty)
        {
            WriteCrusadesData();
            return;
        }            
        int i, j;
        string[] SplittedStr = MainString.Split('$');
        string[] SeasonStr = SplittedStr[0].Split('-');
        string[] MatchStr = SplittedStr[1].Split('~');
        for (i = 0; i < MatchStr.Length; i++)
        {
            //SeasonData
            SuperCrusadesDetails tmSeasonData = JsonUtility.FromJson<SuperCrusadesDetails>(SeasonStr[i]);
            SuperCrusadesData[i].IsOpen = tmSeasonData.IsOpen;

            //MatchData
            string[] matchString = MatchStr[i].Split('|');
            for (j = 0; j < matchString.Length; j++)
            {
                SuperCrusadesMatchData tmMatchData = JsonUtility.FromJson<SuperCrusadesMatchData>(matchString[j]);
                SuperCrusadesData[i].MatchDatas[j].SetSavedLevelData(tmMatchData.IsOpen, tmMatchData.IsPrimaryObjAchieved, tmMatchData.IsSecondaryObjAchieved, tmMatchData.IsTertiaryObjAchieved);
            }
        }
    }
    #endregion


    private void DeductTicket()
    {

       paid = false;
        if (SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].IsPrimaryObjAchieved && SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].IsSecondaryObjAchieved && SuperCrusadesData[CONTROLLER.SelectedCrusadeSeasonIdx].MatchDatas[CONTROLLER.SelectedCrusadeMatchIdx].IsTertiaryObjAchieved)
        {
            paid = true;
            
            return;

        }
        if (UserProfile.Tickets >= 1)
        {
            UserProfile.Tickets -= 1;
            UserProfile.SpentTickets += 1;
            UserProfile.spentTotTickets += 1;

            if (CONTROLLER.IsUserLoggedIn())
            {
                PlayerPrefsManager.SaveUserProfile();
            }
            else
            {
                PlayerPrefsManager.SaveCoins();
            }
            paid = true;
        }
        else
        {
            paid = false;
        }
    }
}


public enum SuperCrusadesObjectiveTypes
{
    Score=0,
    Wicket=1,
    Overs=2,
    Fours=3,
    Six=4,
    SixFour=5,
    HalfCentury=6,
    Century=7,
    OneAndHalfCentury=8,
    DoubleCentury=9,
    Boundaries=10

}


public class SuperCrusadesObjectives
{
    public SuperCrusadesObjectiveTypes ObjectiveType { get; set; }
    public int ObjValue { get;  set;}
    public int ObjValue2 { get;  set; }
    public int RewardAmount { get; set; }
}
public class SuperCrusadesMatchData
{
    public SuperCrusadesObjectives PrimaryObjective { get; set; }
    public SuperCrusadesObjectives SecondaryObjective { get; set; }
    public SuperCrusadesObjectives TertiaryObjective { get; set; }

    public bool IsOpen;
    public bool IsPrimaryObjAchieved;
    public bool IsSecondaryObjAchieved;
    public bool IsTertiaryObjAchieved;

    public string OppTeamName;

    public void SetSavedLevelData(bool islock, bool primary, bool second, bool third)
    {
        IsOpen = islock;
        IsPrimaryObjAchieved = primary;
        IsSecondaryObjAchieved = second;
        IsTertiaryObjAchieved = third;
    }
}
public class SuperCrusadesDetails
{
    public string SeasonName { get; set; }
    public List<SuperCrusadesMatchData> MatchDatas { get; set; }
    public bool IsOpen;   
}
