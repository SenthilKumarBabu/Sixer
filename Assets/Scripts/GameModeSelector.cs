using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using CodeStage.AntiCheat.ObscuredTypes;
using Photon.Pun;
using TMPro;

#if UNITY_ANDROID
using Prime31;
#endif

public class GameModeSelector : MonoBehaviour 
{
    public static GameModeSelector _instance;
    public GameObject modeSelection, modeInstruction, teamManagement, displayMsg, store, leaderboard, settings,jerseySelectionWindow;
	public ModeInstructionLogoAssigner instuctionAssigner;
	private Vector3 startPos, instructionsResetPos, controlsResetPos;
	public GameObject players, instructions;
	public static bool inTeamSelectionPage = false;
	public static bool isNewGame;
	public static bool resumeGame;
	public Text userTickets,userName,userpoints;

	public Image  userProfilePic;
	public Sprite defaultProfilePic;	
	public GameObject signoutToast;
	public Text SignoutBut;
	public GameObject DeleteAccountButton;

    public Image splashScreen;

	[Header("Match Making Panel")]
	public GameObject MatchMakingPanel;
    public TMP_Text PlayerA_Name, PlayerB_Name;



    void Awake() 
	{
		_instance = this;
        float ratio;
        ratio = (float)Screen.width / (float)Screen.height;
        if ((ratio - 1.33f) < 0.01f)
            splashScreen.rectTransform.localScale = new Vector3(1.4f, 1f, 1f);

		if (CONTROLLER.EnableHardcodes == 1)
		{
			hardcodeText.text = "HC-TRUE";
		}
		else
		{
			hardcodeText.text = "HC-FALSE";
		}
	}

    bool isUnSyncedPurchasedSyncCalled = false;
	void Start () 
	{
		AdIntegrate.instance.SystemSleepSettings(1);
		inTeamSelectionPage = false;
		startPos = players.transform.position;
		CONTROLLER.CurrentPage = "splashpage";
		ShowLandingPage(true);
		modeInstruction.SetActive (false);
		teamManagement.SetActive (false);
		displayMsg.SetActive (false);
		/*if(!CONTROLLER.IsUserLoggedIn() ) 
			SignInPanel.instance.Show();
		else
		{
#if UNITY_ANDROID
			if (CONTROLLER.LoginType == 1 && CONTROLLER.bGooglePlayLoginSuccess && PlayerPrefs.HasKey("Googleplayprofpic"))
			{
				userProfilePic.sprite = Sprite.Create(ImageSaver.RetriveTexture("Googleplayprofpic"), new Rect(0, 0, PlayerPrefs.GetInt("Googleplayprofpic_w"), PlayerPrefs.GetInt("Googleplayprofpic_h")), new Vector2(0, 0));
			}
			else
				userProfilePic.sprite = defaultProfilePic;
#endif

#if UNITY_IOS
        userProfilePic.enabled = false;
#endif
            isUnSyncedPurchasedSyncCalled = false;
            UpdateLandingPageTopBars();
			//StartCoroutine(CricMinisWebRequest.instance.UserSync(true,CONTROLLER.forceSync));
			//if(CONTROLLER.canFetchDeeplink && AdIntegrate.instance.checkTheInternet())
			//{
			//	CONTROLLER.canFetchDeeplink = false;
			//	DeeplinkPopup.instance.checkForDeeplinking();
			//}
		}
		*/
		signoutToast.SetActive (false); 

		LoadingScreen.instance.Hide();
		stopShineAnimation();
		shineCoroutine = StartCoroutine(PlayShineAnimation());

	}

	void resetVariables()
	{
		DebugLogger.PrintWithColor("Reset Variables Called:::: ");
        CONTROLLER.selectedGameMode = GameMode.None;
        CONTROLLER.gameMode = "";

        if (MultiplayerManager.Instance != null)
			MultiplayerManager.Instance.DestroyBot();
    }

	public void ShowLandingPage(bool flag)
    {
		if(flag)
		{
			resetVariables();
        }

        modeSelection.SetActive(flag);
		stopShineAnimation();

		if (flag)
		{
			shineCoroutine = StartCoroutine(PlayShineAnimation());
		}
    }

    public Image[] ModeImages;
	public Material Shine_Material;
	private Coroutine shineCoroutine;
	IEnumerator PlayShineAnimation()
	{
		yield return new WaitForSecondsRealtime(2f);
		ModeImages[0].material = Shine_Material;
		yield return new WaitForSecondsRealtime(5.5f);
		ModeImages[0].material = null;
		ModeImages[1].material = Shine_Material;
		yield return new WaitForSecondsRealtime(5.5f);
		ModeImages[1].material = null;
		ModeImages[2].material = Shine_Material;
		yield return new WaitForSecondsRealtime(5.5f);
		ModeImages[2].material = null;
		ModeImages[3].material = Shine_Material;
		yield return new WaitForSecondsRealtime(5.5f);
		ModeImages[3].material = null;

		stopShineAnimation();
		shineCoroutine = StartCoroutine(PlayShineAnimation());
	}

	void stopShineAnimation()
	{
		if (shineCoroutine != null)
		{
			StopCoroutine(shineCoroutine);
			shineCoroutine = null;

			ModeImages[0].material = null;
			ModeImages[1].material = null;
			ModeImages[2].material = null;
			ModeImages[3].material = null;

		}
	}

	public void UpdateLandingPageTopBars()
	{
		if (CONTROLLER.gameTotalPoints <= 0)
		{
			CONTROLLER.gameTotalPoints = 0;
		}
		if (UserProfile.Tickets <= 0)
		{
			UserProfile.Tickets = 0;
		}

		userTickets.text = UserProfile.Tickets.ToString();
		userName.text = CONTROLLER.UserName;
		userpoints.text = CONTROLLER.gameTotalPoints.ToString();
	}

	public void SelectGameMode(int index)
    {
        CONTROLLER.GameIsOnFocus = true;
        AudioPlayer.instance.PlayButtonSnd();
		string dataStr;		
		CONTROLLER.selectedGameMode = (GameMode)index;
        CONTROLLER.meFirstBatting = 1;
        CONTROLLER.totalWickets = 10;
        CONTROLLER.totalOvers = 1;

        if (index == 0)
		{
			CONTROLLER.gameMode = "slogover";
			if (PlayerPrefs.HasKey("slogoverteamlist"))
			{
				dataStr = PlayerPrefs.GetString("slogoverteamlist");
				XMLReader.ParseXML(dataStr);
			}
			else
			{
				XMLReader.ParseXML(PlayerPrefsManager.instance.xmlAsset.text);
				PlayerPrefsManager.SetTeamList();
			}
			newGame();
			/*if (!PlayerPrefs.HasKey("SlogOverDetail"))
			{
				ShowLandingPage(false);
				modeInstruction.SetActive(true);
				CONTROLLER.CurrentPage = "instructionpage";
				Continue();
				resetScroll();

			}
			else
			{
				displayMsg.SetActive(true);
				CONTROLLER.CurrentPage = "dispMsg";
			}*/
		}
		else if(index ==1 ) // Batting bowling mode with AI
		{
            CONTROLLER.meFirstBatting = (UnityEngine.Random.Range(0, 100) < 50) ? 0 : 1;
            XMLReader.ParseXML(PlayerPrefsManager.instance.xmlAsset.text);
            PlayerPrefsManager.SetTeamList();
            ShowLandingPage(false);
            StartCoroutine(LoadGroundScene());
        }
		else if (index == 2 || index == 3 )    //2 - Batting multiplayer   3 - Bat bowl multiplayer
		{
			//StartCoroutine(MultiplayerPage.instance.checkTheStatus());
			MultiplayerManager.Instance.MasterName = string.Empty;
			LoadingScreen.instance.Show();
			StartCoroutine(MultiplayerManager.Instance.EnablePhotonConnection());
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
		}
		
	}
	public void showMatchmakingScreen()
	{
        LoadingScreen.instance.Hide();
        ShowLandingPage(false);
		MatchMakingPanel.SetActive(true);
		Timer.enabled = false;
    }
	public void ShowInstructionPage (int index) 
	{
		CONTROLLER.CurrentPage = "instructionpage";
		instuctionAssigner.updateInstructionText();
	}

	public void GoBackOne()
	{
		AdIntegrate.instance.SystemSleepSettings(1);
		AudioPlayer.instance.PlayButtonSnd();
        modeInstruction.SetActive(false);
		MatchMakingPanel.SetActive(false);
		ShowLandingPage(true);
		UpdateLandingPageTopBars();
		modeInstruction.SetActive(false);
		CONTROLLER.CurrentPage = "splashpage";
        resetScroll ();
		 #if UNITY_ANDROID
		if (CONTROLLER.LoginType == 1 && CONTROLLER.bGooglePlayLoginSuccess && PlayerPrefs.HasKey ("Googleplayprofpic")) 
		{
			userProfilePic.sprite = Sprite.Create (ImageSaver.RetriveTexture ("Googleplayprofpic"), new Rect (0, 0, PlayerPrefs.GetInt ("Googleplayprofpic_w"), PlayerPrefs.GetInt ("Googleplayprofpic_h")), new Vector2 (0, 0));
		}
		else
			userProfilePic.sprite = defaultProfilePic;
		  #endif 

		AudioPlayer.instance.PlayLandingPageIntoGameSFX(false);
    }
	public void GoBackTwo() 
	{
		teamManagement.SetActive(false);
		modeInstruction.SetActive(true);
		resetScroll ();
		CONTROLLER.CurrentPage = "instructionpage";
	}
	public void Continue()
	{
		AudioPlayer.instance.PlayButtonSnd();

		ShowLandingPage(false);
		modeInstruction.SetActive(false);

		//CricMini-Gopi
		Continues();
	}

	public void Continues()
	{
		PlayerPrefsManager.SetTeamList();
		StartCoroutine(LoadGroundScene());
	}

	private IEnumerator LoadGroundScene()
	{
		GameModeSelector.isNewGame = true;
		LoadingScreen.instance.Show();
		yield return new WaitForSeconds(1f);
		GameModeSelector.resumeGame = false;
		ManageScene.LoadScene(Scenes.Ground);
	}

	public void ResumeSavedGame() 
	{
        AudioPlayer.instance.PlayLandingPageIntoGameSFX(true);

        isNewGame = false;
		LoadingScreen.instance.Show();
		ManageScene.LoadScene (Scenes.Ground);
		AdIntegrate.instance.SetTimeScale(0f);
		resumeGame = true;
		Destroy (this.gameObject);
		Resources.UnloadUnusedAssets ();
	}


	public void newGame() 
	{
		isNewGame = true;
		if (CONTROLLER.gameMode == "superover")
		{
			PlayerPrefs.DeleteKey ("SuperOverDetail");
			PlayerPrefs.DeleteKey ("superoverPlayerDetails");
		}
		else if (CONTROLLER.gameMode == "slogover")
		{
			PlayerPrefs.DeleteKey ("SlogOverDetail");
			PlayerPrefs.DeleteKey ("slogoverPlayerDetails");
            PlayerPrefs.DeleteKey("slogovermatchid");
        }
		else if (CONTROLLER.gameMode == "chasetarget")
		{
			PlayerPrefs.DeleteKey ("ChaseTargetDetail");
			PlayerPrefs.DeleteKey ("chasetargetPlayerDetails");
		}
        else if (CONTROLLER.gameMode == CONTROLLER.SUPER_Crusade_GameMode)
        {
            PlayerPrefs.DeleteKey(CONTROLLER.SUPER_Crusade_SavedMatchDetails);
            PlayerPrefs.DeleteKey(CONTROLLER.SUPER_Crusade_PlayerDetails);
        }

		resumeGame = false;
		displayMsg.SetActive(false);
		ShowLandingPage(false);
		modeInstruction.SetActive (true);
        //Scoreboard.instance.NewOver ();

        if (CONTROLLER.gameMode == CONTROLLER.SUPER_Crusade_GameMode)
            ShowInstructionPage (5);
        else
		{
            //ShowInstructionPage (gameMode);
            Continue();
            resetScroll();
        }

        AudioPlayer.instance.PlayLandingPageIntoGameSFX(true);
    }

	public void showPage(int index)
	{
		stopShineAnimation();
		AudioPlayer.instance.PlayButtonSnd();

        if (index == 1)
		{
			if (AdIntegrate.instance.checkTheInternet())
			{
				if (CONTROLLER.IsUserLoggedIn())
				{
					//getLeaderboard();
				}
				else
				{
					SignInPanel.instance.Show();
				}
			}
			else
				Popup.instance.ShowNoInternetPopup();
		}
		else if (index == 3)
		{
			CONTROLLER.tempCurrentPage = CONTROLLER.CurrentPage;
			CONTROLLER.CurrentPage = "settingspage";
			CONTROLLER.gameMode = "";

			settings.SetActive(true);
			ShowLandingPage(false);
			if (CONTROLLER.IsUserLoggedIn())
			{
				DeleteAccountButton.SetActive(true);
				SignoutBut.text = "Sign out";
			}
			else
			{
				DeleteAccountButton.SetActive(false);
				SignoutBut.text = "Sign In";
			}
		}
		else if (index == 6)
		{
			jerseySelectionWindow.SetActive(true);

			//StartCoroutine(CricMinisWebRequest.instance.SleepTest());
		}

	}


	public void close(int index) 
	{
		AudioPlayer.instance.PlayButtonSnd();

        if (index == 0)
        {
            if (Photon.Pun.PhotonNetwork.IsConnected)
            {
                MultiplayerManager.Instance.DisconnectPhoton();
            }
            MatchMakingPanel.SetActive(false);
            ShowLandingPage(true);
        }
        if (index == 1)
		{
			leaderboard.SetActive (false);
			ShowLandingPage(true);
		}
		else if(index==5)
		{
			jerseySelectionWindow.SetActive(false);
		}

		CONTROLLER.CurrentPage = "splashpage";
    }
	public void resetScroll() 
	{
		players.transform.position = startPos;
		//instructions.transform.position = instructionsResetPos;
	}

	public void ForceCloseLeaderboard(bool flag)
	{
		LoadingScreen.instance.Hide();
		close(1);
	}

	public void watchVideo()
	{
		AudioPlayer.instance.PlayButtonSnd();
	}

	void ShowToast()
	{
		GameObject prefabGO ;
		GameObject tempGO ;
		prefabGO = Resources.Load ("Prefabs/Toast")as GameObject ;
		tempGO = Instantiate (prefabGO)as GameObject ;
		tempGO.name = "Toast";
		tempGO.GetComponent <Toast > ().setMessge ("No video Available");
	}


	public Text hardcodeText;
	public void Hardcode()
	{
        if (CONTROLLER.EnableHardcodes==0)
		{
			CONTROLLER.EnableHardcodes = 1;
			hardcodeText.text = "HC-TRUE";
			PlayerPrefsManager.instance.HardcodesuperChaseTargets();
		}
		else
		{
			CONTROLLER.EnableHardcodes = 0;
			hardcodeText.text = "HC-FALSE";
			PlayerPrefsManager.instance.HardcodesuperChaseTargets();
		}

		//SetModApkHardcode();
	}

	public GameObject DeeplinkingHolder;
	public void ShowDeeplinkPopup()
	{
		CONTROLLER.tempCurrentPage = CONTROLLER.CurrentPage;
		CONTROLLER.CurrentPage = "deeplinking";
		DeeplinkingHolder.SetActive(true);
	}
	public void HideDeeplinkPopup()
	{
		DeeplinkingHolder.SetActive(false);
		CONTROLLER.CurrentPage = CONTROLLER.tempCurrentPage;
	}

	public void actionsFromDeeplinkPopup(int idx)
	{
		switch(idx)
		{
			case 1:	//super over
			case 2: //super slog
			case 3:	//super chase
			case 4:	//super multiplayer
				SelectGameMode(idx);
				break;
			case 5: //leaderboard
				showPage(1);
				break;
			case 6:	//store
				showPage(4);
				break;
		}
	}

    public Text publicRoomTimeText;
    float currCountdownValue = 30;
    Coroutine timerCo;

    public void StartTimerForMaster()
    {
        int time;
        if (CONTROLLER.MP_RoomType == 0)
        {
            time = 30;
        }
        else
        {
            time = 120;
        }
        publicRoomTimeText.text = time.ToString();
        currCountdownValue = time;
        if (timerCo == null)
        {
            timerCo = StartCoroutine(PublicRoomTimer());
        }
    }

    IEnumerator PublicRoomTimer()
    {
        //NWDebugLogger.PrintWithColor("############### PublicRoomTimer ######## currCountdownValue; " + currCountdownValue + " playercount: " + MultiplayerManager.instance.getPlayerCount());
        while (currCountdownValue > 0)
        {
            publicRoomTimeText.text = currCountdownValue.ToString();
            yield return new WaitForSecondsRealtime(1.0f);
            currCountdownValue--;
        }
        if (MultiplayerManager.Instance.getPlayerCount() == 2 || MultiplayerManager.Instance.botsSpawned)
        {
            // //NWDebugLogger.PrintWithColor("############### 3333  PublicRoomTimer ######## currCountdownValue; " + currCountdownValue + " playercount: " + MultiplayerManager.instance.getPlayerCount());

            yield break;
        }
        if (currCountdownValue <= 0 && MultiplayerManager.Instance.getPlayerCount() < 2 && !MultiplayerManager.Instance.botsSpawned)
        {
            GoBackAfterEnteringRoom();
            //PopUp.Show(PopUpTypes.YES, hasCloseButton: false, message: "Sorry, no opponent found. Please try again.", yesString: "OK");
        }
    }
	public void GoBackAfterEnteringRoom()
	{
		MultiplayerManager.Instance.bothPlayersInRoom = false;
		if (PhotonNetwork.InRoom && CONTROLLER.MP_RoomType == 0)
		{
			PhotonNetwork.CurrentRoom.IsVisible = false;
			PhotonNetwork.CurrentRoom.IsOpen = false;
		}

		if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom != null)
			PhotonNetwork.LeaveRoom();

		MultiplayerManager.Instance.DisConnectFromPhoton();
		Screen.sleepTimeout = SleepTimeout.SystemSetting;

		MatchMakingPanel.SetActive(false);

		StopPublicRoomTimer();
		close(0);
		CONTROLLER.CurrentPage = "splashpage";
	}

    public void StopPublicRoomTimer()
    {
        if (timerCo != null)
        {
            StopCoroutine(timerCo);
            timerCo = null;
        }
		publicRoomTimeText.text = string.Empty;
        Timer.enabled = true;
    }

    public void GetOpponentDetails(string UserName, string UserID)
    {
        CONTROLLER.MP_Opponent_ud_uID = UserID;
        CONTROLLER.MP_OpponentName = UserName;
        PlayerB_Name.text = UserName.ToUpper();
		PlayerA_Name.text = CONTROLLER.UserName.ToUpper();
        if (!MultiplayerManager.Instance.getMasterClient())
        {
            MultiplayerManager.Instance.bothPlayersInRoom = true;
        }
        
		showMatchmakingScreen();
        //MP_PhpManager.MpPhpManagerScript.LoadOppTeamDetails(isAllStar);
       
		if (timerCo != null)
        {
            StopCoroutine(timerCo);
        }

		if (CONTROLLER.selectedGameMode == GameMode.BattingMultiplayer)
		{
			CONTROLLER.gameMode = "multiplayer";
			if (PlayerPrefs.HasKey("multiplayerteamlist"))
			{
				string dataStr;
				dataStr = PlayerPrefs.GetString("multiplayerteamlist");
				XMLReader.ParseXML(dataStr);
			}
			else
			{
				XMLReader.ParseXML(PlayerPrefsManager.instance.xmlAsset.text);
				PlayerPrefsManager.SetTeamList();
			}

			CONTROLLER.PlayingTeam = new ArrayList();
			for (int i = 0; i < CONTROLLER.TeamList.Length; i++)
			{
				for (int j = 0; j < CONTROLLER.TeamList[i].PlayerList.Length; j++)
				{
					if (CONTROLLER.TeamList[i].PlayerList[j].DefaultPlayer == "1")
					{
						CONTROLLER.PlayingTeam.Add(j);
					}
				}
			}
			PlayerPrefsManager.SetTeamList();

			CONTROLLER.totalOvers = 1;
			CONTROLLER.currentMatchWickets = 0;
			CONTROLLER.totalWickets = 10;

			Invoke("LoadSceneForMultiplayerMode", 2f);
		}
		else
		{
			XMLReader.ParseXML(PlayerPrefsManager.instance.xmlAsset.text);
			PlayerPrefsManager.SetTeamList();
            CONTROLLER.meFirstBatting = MultiplayerManager.Instance.IsMasterClient() ? 0 : 1; //master bowling hardcode
            Invoke("LoadSceneForMultiplayerMode", 2f);
        }
    }

    public bool IsUserOnline = false;
    public Tween timerTween;
    public Image Timer;

    void LoadSceneForMultiplayerMode()
	{
        TimeToSwitchToGround(LoadSceneCheck);

    }
    public void TimeToSwitchToGround(TweenCallback LoadSceneAction)
    {
        Sequence sq = DOTween.Sequence();
        Timer.enabled = true;
        Timer.fillAmount = 1f;
        publicRoomTimeText.text = string.Empty;
        timerTween = Timer.DOFillAmount(0f, 10f).OnComplete(LoadSceneAction);
    }

    public Coroutine userOnlineCheckCoroutine = null;

    public void LoadSceneCheck()
    {
      //  if (!PopUp.IsPopUpShowing() && MultiplayerMenus.instance.playingEleven.activeSelf)
        {
			LoadingScreen.instance.Show();
			if (MultiplayerManager.Instance.botsSpawned)
			{
                IsUserOnline = true;
                LoadScene();
            }
            else
			{
				MultiplayerManager.Instance.IsOpponentInOnline = false;
				userOnlineCheckCoroutine = StartCoroutine(WaitTillUserIsOnline());
			}
        }
    }

    private IEnumerator WaitTillUserIsOnline()
    {
        CheckIfUserIsOnline();
        yield return new WaitForSecondsRealtime(0.5f);
        if (IsUserOnline)
        {
            StopCoroutine(userOnlineCheckCoroutine);
            LoadScene();
            yield break;
        }
        else
        {
            userOnlineCheckCoroutine = StartCoroutine(WaitTillUserIsOnline());
        }
    }

    private void CheckIfUserIsOnline()
    {
        MultiplayerManager.Instance.CheckIfOpponentOnline();
    }

    public void UserIsOnline()
    {
        IsUserOnline = true;
    }

	public void LoadScene()
	{
		if (MatchMakingPanel.activeSelf && (MultiplayerManager.Instance.getPlayerCount() == 2 || MultiplayerManager.Instance.botsSpawned ))
		{
			LoadingScreen.instance.Show();

			ManageScene.SetCurrentScene(Scenes.Ground);
			if (MultiplayerManager.Instance.IsMasterClient() && IsUserOnline)
			{
				Invoke("LoadSceneWithDelay", 0.5f);
			}
			else if (MultiplayerManager.Instance.IsMasterClient())
			{
				LoadingScreen.instance.Hide();
			}
		}
	}

    private void LoadSceneWithDelay()
    {
        if (IsUserOnline)
        {
            PhotonNetwork.LoadLevel(2); 
        }
    }
}
