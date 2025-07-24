using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using CodeStage.AntiCheat.ObscuredTypes;
#if UNITY_ANDROID
using Prime31;
#endif

public class GameModeSelector : MonoBehaviour 
{
    public GameObject showSuperCard;
    public static GameModeSelector _instance;
    public GameObject modeSelection, modeInstruction, teamManagement, displayMsg, store, leaderboard, settings,jerseySelectionWindow;
	public ModeInstructionLogoAssigner instuctionAssigner;
	private Vector3 startPos, instructionsResetPos, controlsResetPos;
	public GameObject players, instructions;
	private int gameMode;
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

    public MP_LeaderBoard mpLeaderboardScript;

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
		if(!CONTROLLER.IsUserLoggedIn() )
			SignInPanel.instance.Show();
		else
		{
/*#if UNITY_ANDROID && !UNITY_EDITOR

			if (CONTROLLER.LoginType == 1 && CONTROLLER.bGooglePlayLoginSuccess)
			{
				if (NextwaveMarshmallowPermission.instance.isMarshMallow())
				{
					bool isGranted = EtceteraAndroid.checkSelfPermission("android.permission.GET_ACCOUNTS");
					if (isGranted == false)
					{
						//proceedsign out
						CricMinisWebRequest.instance.ProceedSignout(true);
						return;
					}
				}
			}
#endif*/
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
			StartCoroutine(CricMinisWebRequest.instance.UserSync(true,CONTROLLER.forceSync));
			if(CONTROLLER.canFetchDeeplink && AdIntegrate.instance.checkTheInternet())
			{
				CONTROLLER.canFetchDeeplink = false;
				DeeplinkPopup.instance.checkForDeeplinking();
			}
		}

		signoutToast.SetActive (false); 

        if (AdIntegrate.instance != null && CONTROLLER.launchInternetAdEvent == false)
        {
            StartCoroutine(AdIntegrate.instance.initAdmob());
        }

		LoadingScreen.instance.Hide();
		stopShineAnimation();
		shineCoroutine = StartCoroutine(PlayShineAnimation());

		if (ServerManager.Instance.CanReplay())
			SelectGameMode(4);
	}

	public void ShowLandingPage(bool flag)
	{
		modeSelection.SetActive(flag);
		stopShineAnimation();

		if (flag)
		{
			shineCoroutine = StartCoroutine(PlayShineAnimation());

			AdIntegrate.instance.HideAd();
			if (CONTROLLER.CurrentPage == "splashpage" || CONTROLLER.CurrentPage == "" || ManageScene.CurScene == Scenes.Preloader)
			{
				StartCoroutine(Hide_BannerAd());
			}
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

	IEnumerator Hide_BannerAd()
	{
		yield return new WaitForSecondsRealtime(3.0f);
		if (CONTROLLER.CurrentPage == "splashpage" || CONTROLLER.CurrentPage == "" || ManageScene.CurScene == Scenes.Preloader)
		{
			AdIntegrate.instance.HideAd();
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

		if (MultiplayerPage.instance != null)
			MultiplayerPage.instance.TicketsCount_have.text = /*MultiplayerPage.instance.TicketsCount_Donthave.text =*/ "TICKETS LEFT "+UserProfile.Tickets.ToString();


		if (StorePanel.instance != null && !isUnSyncedPurchasedSyncCalled)
        {
            isUnSyncedPurchasedSyncCalled = true;
			StorePanel.instance.SyncUnSyncedPurchases();
        }
	}



    public void PopupClose()
    {
        AdIntegrate.instance.HideAd();
    }

    public IEnumerator SyncCPpoints()
    {

        if (!AdIntegrate.instance.checkTheInternet()|| !CONTROLLER.IsUserLoggedIn())
        {
            // No Internet
        }
        else
        {
            WWWForm form = new WWWForm();
            WWW download;

            form.AddField("action", "CPUpdate");
            form.AddField("user_id", CONTROLLER.UserID);
            form.AddField("bv", CONTROLLER.CURRENT_VERSION);
            form.AddField("platform", CONTROLLER.TargetPlatform);
            form.AddField("deviceid", CONTROLLER.DeviceID);           
            form.AddField("players", -1);
            form.AddField("rank", -1);
            form.AddField("state", "abort");

            download = new WWW(CONTROLLER.BASE_URL, form);
            yield return download;

            if (!string.IsNullOrEmpty(download.error))
            {
            }
            else
            {
                SimpleJSON.JSONNode node = SimpleJSON.JSONNode.Parse(download.text);
                if ("" + node["CPUpdate"]["status"] == "1")
                {
                    PlayerPrefs.DeleteKey(CONTROLLER.BatMpCpSavedName);
                    CONTROLLER.CricketPoints = node["CPUpdate"]["cp"].AsInt;
                    PlayerPrefsManager.SaveCoins();
                }
            }            
        }
    }
	
	public void SelectGameMode(int index)
    {
        CONTROLLER.GameIsOnFocus = true;
        AudioPlayer.instance.PlayButtonSnd();
		string dataStr;		
		gameMode = index;
		if(index == 1)
		{
			CONTROLLER.gameMode = "superover";
			if(PlayerPrefs.HasKey ("superoverteamlist"))
			{
				dataStr = PlayerPrefs.GetString ("superoverteamlist");
				XMLReader.ParseXML (dataStr);
			}
			else 
			{
				XMLReader.ParseXML (PlayerPrefsManager.instance.xmlAsset.text);
				PlayerPrefsManager.SetTeamList ();
			}
			if (!PlayerPrefs.HasKey ("SuperOverDetail"))
			{
				//HideMe ();
				ShowLandingPage(false);
				modeInstruction.SetActive (true);
				CONTROLLER.CurrentPage = "instructionpage";
				ShowInstructionPage (1);

				AudioPlayer.instance.PlayLandingPageIntoGameSFX(true);
			}
			else
			{
				displayMsg.SetActive(true);
				CONTROLLER.CurrentPage = "dispMsg";
			}
			AdIntegrate.instance.ShowBannerAd();
		}
		else if (index == 2)
		{
			CONTROLLER.gameMode = "slogover";
			if(PlayerPrefs.HasKey ("slogoverteamlist"))
			{
				dataStr = PlayerPrefs.GetString ("slogoverteamlist");
				XMLReader.ParseXML (dataStr);
			}
			else 
			{
				XMLReader.ParseXML (PlayerPrefsManager.instance.xmlAsset.text);
				PlayerPrefsManager.SetTeamList ();
			}
			if (!PlayerPrefs.HasKey ("SlogOverDetail"))
			{
				//HideMe ();
				ShowLandingPage(false);
				modeInstruction.SetActive (true);
				CONTROLLER.CurrentPage = "instructionpage";
				ShowInstructionPage (2);
                AudioPlayer.instance.PlayLandingPageIntoGameSFX(true);

            }
            else
			{
				displayMsg.SetActive(true);
				CONTROLLER.CurrentPage = "dispMsg";
			}
			AdIntegrate.instance.ShowBannerAd();
		}
		else if (index == 3)
		{	
			CONTROLLER.gameMode = "chasetarget";
			if(PlayerPrefs.HasKey ("chasetargetteamlist"))
			{
				dataStr = PlayerPrefs.GetString ("chasetargetteamlist");
				XMLReader.ParseXML (dataStr);
			}
			else 
			{
				XMLReader.ParseXML (PlayerPrefsManager.instance.xmlAsset.text);
				PlayerPrefsManager.SetTeamList ();
			}
			if (!PlayerPrefs.HasKey ("ChaseTargetDetail"))
			{
				//HideMe ();
				ShowLandingPage(false);
				modeInstruction.SetActive (true);
				CONTROLLER.CurrentPage = "instructionpage";
				ShowInstructionPage (3);
                AudioPlayer.instance.PlayLandingPageIntoGameSFX(true);

            }
            else
			{
				displayMsg.SetActive(true);
				CONTROLLER.CurrentPage = "dispMsg";
			}
			AdIntegrate.instance.ShowBannerAd();
		}
		else if(index==4)	//super multiplayer
		{
			StartCoroutine(MultiplayerPage.instance.checkTheStatus());
		}
		else if(index==5)
        {            
            CONTROLLER.gameMode = CONTROLLER.SUPER_Crusade_GameMode;

            if (PlayerPrefs.HasKey(CONTROLLER.SUPER_Crusade_Teamlist))
            {
                dataStr = PlayerPrefs.GetString(CONTROLLER.SUPER_Crusade_Teamlist);
                XMLReader.ParseXML(dataStr);
            }
            else
            {
                XMLReader.ParseXML(PlayerPrefsManager.instance.xmlAsset.text);
				PlayerPrefsManager.SetTeamList();
            }

            if (!PlayerPrefs.HasKey(CONTROLLER.SUPER_Crusade_SavedMatchDetails))
            {
				//HideMe ();
				ShowLandingPage(false);
                modeInstruction.SetActive(true);
                CONTROLLER.CurrentPage = "instructionpage";
                ShowInstructionPage(5);
            }
            else
            {
                displayMsg.SetActive(true);
                CONTROLLER.CurrentPage = "dispMsg";
            }
			AdIntegrate.instance.ShowBannerAd();
			
		}
		else if (index == 6)
		{
            CONTROLLER.gameMode = "supercards";
			//if (PlayerPrefs.HasKey("slogoverteamlist"))
			//{
			//    dataStr = PlayerPrefs.GetString("slogoverteamlist");
			//    XMLReader.ParseXML(dataStr);
			//}
			//else
			//{
			//    XMLReader.ParseXML(LoadPlayerPrefs.instance.xmlAsset.text);
			//    SavePlayerPrefs.SetTeamList();
			//}
			//if (!PlayerPrefs.HasKey("SlogOverDetail"))
			//{
			//HideMe ();
			ShowLandingPage(false);
                modeInstruction.SetActive(true);
                CONTROLLER.CurrentPage = "instructionpage";
                ShowInstructionPage(6);
            //}
            //else
            //{
                //displayMsg.SetActive(true);
                //CONTROLLER.CurrentPage = "dispMsg";
                //logo.gameObject.SetActive(false);
                //gameLogo.gameObject.SetActive(false);
           // }
            AdIntegrate.instance.ShowBannerAd();
            //modeSelection.SetActive(false);
            //SuperCardsUI.instance.ShowMe();
        }
        CONTROLLER.totalWickets = 10;
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
		MultiplayerPage.instance.HideMultiplayerMode ();
        modeInstruction.SetActive(false);
		ShowLandingPage(true);
		UpdateLandingPageTopBars();
		modeInstruction.SetActive(false);
		CONTROLLER.CurrentPage = "splashpage";
        if (AdIntegrate.instance != null)
        {
            AdIntegrate.instance.HideAd();
        }
        resetScroll ();
		 #if UNITY_ANDROID
		if (CONTROLLER.LoginType == 1 && CONTROLLER.bGooglePlayLoginSuccess && PlayerPrefs.HasKey ("Googleplayprofpic")) 
		{
			userProfilePic.sprite = Sprite.Create (ImageSaver.RetriveTexture ("Googleplayprofpic"), new Rect (0, 0, PlayerPrefs.GetInt ("Googleplayprofpic_w"), PlayerPrefs.GetInt ("Googleplayprofpic_h")), new Vector2 (0, 0));
		}
		else
			userProfilePic.sprite = defaultProfilePic;
		  #endif 
		if (AdIntegrate.instance != null)
			AdIntegrate.instance.HideAd ();

		AudioPlayer.instance.PlayLandingPageIntoGameSFX(false);
    }
	public void GoBackTwo() 
	{
		teamManagement.SetActive(false);
		modeInstruction.SetActive(true);
		resetScroll ();
		CONTROLLER.CurrentPage = "instructionpage";
		if (AdIntegrate.instance != null)
			AdIntegrate.instance.HideAd();
	}
	public void Continue() 
	{
		AudioPlayer.instance.PlayButtonSnd();


		if (CONTROLLER.gameMode != "supercards")
        {
			ShowLandingPage(false);
            modeInstruction.SetActive(false);
            AdIntegrate.instance.ShowBannerAd();

			//CricMini-Gopi
			Continues();
		}
        else
        {
            AdIntegrate.instance.HideAd();
            CONTROLLER.CurrentPage = "Cardplaymenu";
            showSuperCard.gameObject.SetActive(true);
            SuperCardsUI.instance.cardsPlayMode.SetActive(true);
        }
	}

	public void Continues()
	{
		if (CONTROLLER.gameMode != "multiplayer")
		{
			PlayerPrefsManager.SetTeamList();
			StartCoroutine(LoadGroundScene());
		}
		else
		{
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
			GameModeSelector.inTeamSelectionPage = false;
			MultiplayerPage.instance.ShowMe(true);
			MultiplayerPage.instance.multiplayerPageNumber = 0;
			MultiplayerPage.instance.CheckPage();
		}
		if (CONTROLLER.gameMode != "superover" && CONTROLLER.gameMode != CONTROLLER.SUPER_Crusade_GameMode && CONTROLLER.gameMode != "slogover" && CONTROLLER.gameMode != "chasetarget")
		{
			AdIntegrate.instance.HideAd();
		}
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
            ShowInstructionPage (gameMode);

        AudioPlayer.instance.PlayLandingPageIntoGameSFX(true);
    }

    IEnumerator show_BannerAd()
    {
        yield return new WaitForSecondsRealtime(5.0f);
        AdIntegrate.instance.ShowBannerAd();
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
					getLeaderboard();
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
			AdIntegrate.instance.ShowBannerAd();
		}
		else if (index == 4)
		{
			if (AdIntegrate.instance.checkTheInternet())
			{
				StorePanel.instance.Show();
				AdIntegrate.instance.ShowBannerAd();
			}
			else
				Popup.instance.ShowNoInternetPopup();
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

		if (index == 1)
		{
			leaderboard.SetActive (false);
			ShowLandingPage(true);
		}
		else if (index == 4) 
		{
			StorePanel.instance.Hide();
			ShowLandingPage(true);
		}
		else if(index==5)
		{
			jerseySelectionWindow.SetActive(false);
		}

		if(AdIntegrate .instance !=null )
		{
			AdIntegrate.instance.HideAd ();
		}

		CONTROLLER.CurrentPage = "splashpage";

        if (CONTROLLER.CurrentPage == "splashpage" || CONTROLLER.CurrentPage == "" || ManageScene.CurScene == Scenes.Preloader)
        {
            StartCoroutine(Hide_BannerAd());
        }

    }
	public void resetScroll() 
	{
		players.transform.position = startPos;
		//instructions.transform.position = instructionsResetPos;
	}

	
	public void MerchandiseStore()
	{
		Application.OpenURL ("https://www.chennaisuperkings.com/CSK_WEB/Merchandise/index.html#/merchandiseLanding");
	}

	public void getLeaderboard()
	{
		StartCoroutine (CricMinisWebRequest.instance.getPointsfromLeaderBoard () ); 
	}

	public void ForceCloseLeaderboard(bool flag)
	{
		LoadingScreen.instance.Hide();
		close(1);
	}

	public void watchVideo()
	{
		AudioPlayer.instance.PlayButtonSnd();

		if (AdIntegrate.instance != null)
		{
			AdIntegrate.instance.GetTicketRewardedVideoHelper();
			//if (AdIntegrate.instance.checkTheInternet())
			//{
			//	if (AdIntegrate.instance.isRewardedReadyToPlay())
			//	{
			//		CONTROLLER.RewardedVideoClickedState = -1;
			//		AdIntegrate.instance.ShowRewardedVideo();
			//	}
			//	else
			//	{
			//		Popup.instance.showGenericPopup("", "No video Available");
			//	}
			//}
			//else
			//	Popup.instance.ShowNoInternetPopup();
		}
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


	public void Hide_BannerAD()
	{
		AdIntegrate.instance.HideAd();
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


	#region MODAPK-HARDCODE
	private void SetModApkHardcode()
	{
#if UNITY_ANDROID
		if (CONTROLLER.isAdminUser)
		{
			if (NextwaveMarshmallowPermission.instance != null)
			{
				NextwaveMarshmallowPermission.instance.printTheAndroidLog("SetModApkHardcode called");
			}
		}
#endif
		ObscuredPrefs.SetInt("impk", 3); //Not called to check this is mod apk or not
#if !UNITY_EDITOR && UNITY_ANDROID
        if(NextwaveMarshmallowPermission.instance != null)
            NextwaveMarshmallowPermission.instance.CheckToShowModApkBlocker();
#endif
#if UNITY_EDITOR
		CheckToShowModApkBlocker();
#endif
	}
	private void CheckToShowModApkBlocker()
	{
		int impkValue = ObscuredPrefs.GetInt("impk", 0); //Yes this is mod apk
#if UNITY_ANDROID && !UNITY_EDITOR
        if (CONTROLLER.isAdminUser)
        {
            if (NextwaveMarshmallowPermission.instance != null)
            {
                NextwaveMarshmallowPermission.instance.printTheAndroidLog("CheckToShowModApkBlocker called:" + impkValue);
            }
        }
#endif
		if (impkValue == 3)
		{
			if (NextwaveMarshmallowPermission.instance != null)
				NextwaveMarshmallowPermission.instance.showTheAlertDialogToExit("INFORMATION", "YOU ARE USING A MOD VERSION OF THE GAME,SO ARE NOT ALLOWED TO PLAY,TRY INSTALLING FROM GOOGLE PLAY STORE", "EXIT");
			else
				Popup.instance.showGenericPopup("INFORMATION", "YOU ARE USING A MOD VERSION OF THE GAME,SO ARE NOT ALLOWED TO PLAY,TRY INSTALLING FROM GOOGLE PLAY STORE", AdIntegrate.instance.GameExit);
		}
	}
	#endregion
}
