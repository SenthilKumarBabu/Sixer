using UnityEngine;
using System.Collections;
using UnityEngine .UI ;

public class MultiplayerPage : Singleton <MultiplayerPage > 
{

	public GameObject Holder;

	#region Variable Definitions and Declarations
	public GameObject oversPage;
	public GameObject ticketsPage,roomPage;
	public GameObject createPage;
	public GameObject joinPage;
	public GameObject lobbyPage;
	public GameObject roomIDGO;

	public GameObject  backButton;
	public Image backBtn;
	public GameObject continueButton;

	public Toggle [] roomToggle;
	public Toggle[] createToggle;
	public Toggle[] oversToggle;

	public InputField  roomIDInput;
	public Text  roomIDVal;

	public Text  timeToWait;
	public GameObject statusTxtGO;
	public Text  statusTxt;

	public Text[] playerNameLabel;
	public Image[] playerNameSprite;
    public GameObject[] PlayerCpPoints;

    public int multiplayerPageNumber = 0;			//-1 = MainMenu, 0 = RoomPage, 1 = CreatePage, 2 = JoinIDPage, 3 = Overs Page, 4 = Lobby Page

	public GameObject waitingForOtherPlayers;
	public Sprite   BackButtonEnable, BackButtonDiable,PlayerNameSprite_User,PlayerNameSprite_other;

	public Button watchVideoButton;
	public Sprite[] btnTexture;

    public bool isContestWarningPopupShown = false;

	public GameObject instructionHolder;
	//public Text instrunctionText;
	public GameObject InstuctionContent;
	public GameObject HelpHolder;

	public GameObject TicketsHave, TicketsDontHave;
	public Text TicketsCount_have, TicketsCount_Donthave;

	#endregion

	private void Awake()
	{
		Holder.SetActive(false);
		LoadingScreen.instance.Hide();
		AdIntegrate.instance.SystemSleepSettings(1);
	}

	#region ButtonEvents

	public void ShareRoom ()
	{
		AudioPlayer.instance.PlayButtonSnd();
		string subject = CONTROLLER.AppName;
		string body;
		
		body = CONTROLLER.UserName + " wants compete against you in Multiplayer. Use Room ID:" + roomIDVal.text + " in Super Multiplayer. Will you answer the challenge?\nDownload Now " + CONTROLLER.AppLink;

		//execute the below lines if being run on a Android device
#if UNITY_ANDROID && !UNITY_EDITOR
		//Refernece of AndroidJavaClass class for intent
		AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");
		//Refernece of AndroidJavaObject class for intent
		AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");
		//call setAction method of the Intent object created
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		//set the type of sharing that is happening
		intentObject.Call<AndroidJavaObject>("setType", "text/plain");
		//intentObject.Call<AndroidJavaObject>("setType", "message/rfc822");
		//add data to be passed to the other activity i.e., the data to be sent
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body);
		//get the current activity
		AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		//start the activity by sending the intent data
		currentActivity.Call ("startActivity", intentObject);
#endif
#if UNITY_IOS
        /*body = UserProfile.PlayerName+" wants you to joins his Multiplayer Match. Room ID:"+roomIDVal.text+". If you dont have Bat Attack Cricket, download from: http://batattackcricket.com/";*/
			ShareThisGame.sendTextWithOutPath (body);
#endif

	}

	public void ShowContinueButton ()
	{
		continueButton.SetActive (true);
	}

	public void HideContinueButton ()
	{
		continueButton.SetActive (false);
	}

	public void ShowLobbyForPrivateJoin ()
	{
		multiplayerPageNumber += 2;
		ClickedContinue ();
	}

	public void ClickedContinue ()
	{
		AudioPlayer.instance.PlayButtonSnd();
		if (AdIntegrate.instance.checkTheInternet())// Application .internetReachability !=NetworkReachability.NotReachable)  
		{
			CheckToggles ();
			CheckPage ();
		}
		else 
		{
			Popup.instance.ShowNoInternetPopup();
		}
	}

	public void ClickedBack ()
	{
		AudioPlayer.instance.PlayButtonSnd();
		if (AdIntegrate.instance != null)
		{
			AdIntegrate.instance.HideAd();
		}
		if(CONTROLLER.canPressBackBtn == false)
			return;
		if(HelpHolder.activeSelf)
		{
			HelpHolder.SetActive(false);
			return;
		}
        ServerManager.Instance.SetReplayDataNull();
        if (Multiplayer.roomType == 2)
		{
			multiplayerPageNumber = -1;
			ServerManager.Instance.ExitRoom ();
			Multiplayer.roomType = -1;
		}
		else if (multiplayerPageNumber == 5) 
		{
			multiplayerPageNumber = 0;
			ServerManager.Instance.ExitRoom ();
			roomIDInput.text  = string.Empty;
		}
		else
		{
			if (Multiplayer.roomType == 0 && multiplayerPageNumber == 3)
			{
				multiplayerPageNumber -= 3;
			} 
			else if (Multiplayer.isHost == 1 && multiplayerPageNumber == 3)
			{
				multiplayerPageNumber -= 2;
			}
			else if (multiplayerPageNumber == 50)
			{
				multiplayerPageNumber = -1;
			}
			else if (multiplayerPageNumber == 0)
			{
                isContestWarningPopupShown = false;
				//multiplayerPageNumber = -1;
				multiplayerPageNumber = 51;
			}
			else
			{
				if (multiplayerPageNumber == 2)
				{
					roomIDInput.text  = string.Empty;
					CONTROLLER.CurrentPage = "multiplayerpage";
				}
				multiplayerPageNumber--;
			}
		}

		CheckPage ();
	}

	public void watchVideo()
	{
		if (AdIntegrate.instance != null)
		{
			AdIntegrate.instance.GetTicketRewardedVideoHelper();
			/*
			if (AdIntegrate.instance.checkTheInternet())
			{
				if (AdIntegrate.instance.isRewardedReadyToPlay())
				{
					//watchVideoButton.GetComponent<Image>().sprite = btnTexture[0];
					CONTROLLER.RewardedVideoClickedState = -1;
					AdIntegrate.instance.ShowRewardedVideo();
				}
				else
				{
					Popup.instance.showGenericPopup("", "No video Available");
				}
			}
			else
				Popup.instance.ShowNoInternetPopup();*/
		}
	}

	

	#endregion

	#region public access methods
	public void OpenLobbyForRematch ()
	{
		multiplayerPageNumber = 4;
		Multiplayer.isHost = 0;
		//Multiplayer.roomType = 2;
		statusTxt.text = "WAITING FOR REMATCH.";
		CheckPage ();
		ShowWaitingForOtherPlayers ();
	}

	public void ShowWaitingForOtherPlayers ()
	{
		waitingForOtherPlayers.SetActive (true);
	}

	public void HideWaitingForOtherPlayers ()
	{
		waitingForOtherPlayers.SetActive (false);
	}

	public void SetRoomIDValue (string roomVal)
	{
		roomIDVal.text = roomVal;
	}

	public void ShowMe (bool isTrue)
	{
		isContestWarningPopupShown = false;
        Holder.SetActive(isTrue);
		if (isTrue) 
		{
			multiplayerPageNumber = 50;
			TicketsCount_have.text=/* TicketsCount_Donthave.text =*/ "TICKETS LEFT " + UserProfile.Tickets.ToString (); 
			CheckPage ();
			AdIntegrate.instance.SystemSleepSettings(0);
		}
	}

	public void UpdatePlayerList ()
	{
		for (int i = 0; i < 5; i++)
		{
			playerNameLabel [i].text = Multiplayer.playerList [i].PlayerName;

            //if (PlayerCpPoints != null)
            //{
            //    PlayerCpPoints[i].SetActive(true);

            //    int val = -1;
            //    bool isInt = int.TryParse(Multiplayer.playerList[i].Cp, out val);
            //    if (isInt)
            //        PlayerCpPoints[i].GetComponent<Text>().text = Multiplayer.playerList[i].Cp;
            //    else
            //    {
            //        string[] tmpArr = { "100", "150", "200", "250", "300", "350", "400", "450", "500", "550", "600" };
            //        PlayerCpPoints[i].GetComponent<Text>().text = tmpArr[Random.Range(0, tmpArr.Length)];
            //    }
            //}

            if (Multiplayer.playerList[i].playerIdwithCountryCode == CONTROLLER.BattingMultiplayerCountryCode + CONTROLLER.M_USERID) //(Multiplayer.playerList[i].PlayerName == CONTROLLER.ud_UserName)
            //if (Multiplayer.playerList [i].PlayerName == UserProfile.PlayerName)
			{
				playerNameSprite [i].sprite  = PlayerNameSprite_User;// "white-rounded-rectangle";
			}
            else if (Multiplayer.playerList[i].PlayerName == "waiting for player...")
            {
                //playerNameSprite[i].color = PlayerNameSpriteCOLOR_Empty;
                PlayerCpPoints[i].SetActive(false);
            }
            else
			{
				playerNameSprite [i].sprite = PlayerNameSprite_other;//"yellow-rounded-rectangle";
			}
		}

		if (multiplayerPageNumber == 4 || multiplayerPageNumber == 5)
		{
			if (Multiplayer.playerCount > 1 && Multiplayer.isHost == 1 && Multiplayer.roomType == 1)
			{
				ShowContinueButton ();
			} else
			{
				HideContinueButton ();
			}
		}

		statusTxt.text = "" + Multiplayer.overs + " OVERS MATCH";// WAITING FOR PLAYERS";
        if(Multiplayer.playerCount==5)
            BlockBackBtn();
	}

	public void UpdateCountdown (int time)
	{
		timeToWait.text = "" + time + "";
		if(time == 2)
			BlockBackBtn ();
	}
	#endregion

	#region page methods
	private void BlockBackBtn ()
	{
		//Block Back Btn
		//backBtn.sprite=BackButtonDiable ;	// "gray-button - Copy";
		backButton.SetActive (false);
		CONTROLLER.canPressBackBtn = false;
	}
		
	public  void CheckPage ()
	{
		if(multiplayerPageNumber ==50)
		{
			CONTROLLER.CurrentPage = "multiplayerpage";
			ShowContinueButton();
			ticketsPage.SetActive (true); 
			roomPage.SetActive (false );
			createPage.SetActive (false);
			joinPage.SetActive (false);
			oversPage.SetActive (false);
			lobbyPage.SetActive (false);
			instructionHolder.SetActive(false);
			HelpHolder.SetActive(false);
			CONTROLLER.canPressBackBtn = true;
			//backBtn.sprite = BackButtonEnable;	// "oranges-01";
			backButton.SetActive (true);

			updateTicketsPage();

		}
		else if (multiplayerPageNumber == 51)
		{
			ShowContinueButton();
			ticketsPage.SetActive(false);
			roomPage.SetActive(false);
			createPage.SetActive(false);
			joinPage.SetActive(false);
			oversPage.SetActive(false);
			lobbyPage.SetActive(false);
			CONTROLLER.canPressBackBtn = true;
			backButton.SetActive(true);
			TicketsCount_have.text = /*TicketsCount_Donthave.text =*/ "TICKETS LEFT " + UserProfile.Tickets.ToString();
			ShowInstruction();

			if (ServerManager.Instance.CanReplay())
			{
				if (ServerManager.ReplayData.roomType == 0)
					roomToggle[0].isOn = true;
				else
					roomToggle[1].isOn = true;

				ClickedContinue();
			}
        }
		else if(multiplayerPageNumber == 0)
		{
			ShowContinueButton ();
			ticketsPage.SetActive (false ); 
			roomPage.SetActive (true);
			createPage.SetActive (false);
			joinPage.SetActive (false);
			oversPage.SetActive (false);
			lobbyPage.SetActive (false);
			CONTROLLER.canPressBackBtn = true;
			backButton.SetActive (true);
			instructionHolder.SetActive(false);
			CONTROLLER.CurrentPage = "multiplayerpage";		
			AdIntegrate.instance.ShowBannerAd();

            if (ServerManager.Instance.CanReplay())
			{
                if (ServerManager.ReplayData.overs == 2)
				{
                    oversToggle[0].isOn = true;
                    oversToggle[1].isOn = false;
				}
                else
				{
                    oversToggle[1].isOn = true;
                    oversToggle[0].isOn = false;
				}

                ClickedContinue();
			}
        }
		else if(multiplayerPageNumber == 1)
		{
			ShowContinueButton ();
			ticketsPage.SetActive (false ); 
			roomPage.SetActive (false);
			createPage.SetActive (true);
			joinPage.SetActive (false);
			oversPage.SetActive (false);
			lobbyPage.SetActive (false);
			instructionHolder.SetActive(false);
			CONTROLLER.CurrentPage = "multiplayerpage";		
			AdIntegrate .instance .ShowBannerAd (); 
		}
		else if(multiplayerPageNumber == 2)
		{
			ShowContinueButton ();
			ticketsPage.SetActive (false ); 
			roomPage.SetActive (false);
			createPage.SetActive (false);
			joinPage.SetActive (true);
			oversPage.SetActive (false);
			lobbyPage.SetActive (false);
			instructionHolder.SetActive(false);
			CONTROLLER.CurrentPage = "multiplayerpage"; 		
			AdIntegrate .instance .ShowBannerAd (); 
		}
		else if(multiplayerPageNumber == 3)
		{
			ShowContinueButton ();
			ticketsPage.SetActive (false ); 
			roomPage.SetActive (false);
			createPage.SetActive (false);
			joinPage.SetActive (false);
			oversPage.SetActive (true);
			lobbyPage.SetActive (false);
			instructionHolder.SetActive(false);
			CONTROLLER.CurrentPage = "multiplayerpage";		
			AdIntegrate .instance .ShowBannerAd ();

            if (ServerManager.Instance.CanReplay())
            {
                ClickedContinue();
            }
        }
		else if(multiplayerPageNumber == 4)
		{
			CONTROLLER.CurrentPage = "multiplayerpage";		

			roomPage.SetActive (false);
			ticketsPage.SetActive (false ); 
			createPage.SetActive (false);
			joinPage.SetActive (false);
			oversPage.SetActive (false);
			lobbyPage.SetActive (true);
			instructionHolder.SetActive(false);
			multiplayerPageNumber++;

            for (int x = 0; x < PlayerCpPoints.Length; x++)
                PlayerCpPoints[x].SetActive(false);


            if (Multiplayer.isHost == 0)
			{
				HideContinueButton ();
				roomIDGO.SetActive (false);
				statusTxtGO.SetActive (true);
			}
			else
			{ 
				ServerManager.Instance.CreateRoom ();
				roomIDGO.SetActive (true);
				statusTxtGO.SetActive (true);
			}

			if (Multiplayer.roomType == 0)
			{ 
				ServerManager.Instance.FindRoom ();
				HideContinueButton ();
				
				timeToWait.text = "30";
			}
			else if (Multiplayer.roomType == 1)
			{
				timeToWait.text = "180";
			}
			else if (Multiplayer.roomType == 2)
			{
				HideContinueButton ();
			}
			AdIntegrate .instance .ShowBannerAd ();
		}
		else if(multiplayerPageNumber == -1)
		{
            AudioPlayer.instance.PlayLandingPageIntoGameSFX(false);
            HideMultiplayerMode();
		}
	}


	public void updateTicketsPage()
	{
		TicketsCount_have.text = /*TicketsCount_Donthave.text =*/ "TICKETS LEFT " + UserProfile.Tickets.ToString();

		if (UserProfile.Tickets > 0)
		{
			TicketsHave.SetActive(true);
			TicketsDontHave.SetActive(false);
		}
		else
		{
			TicketsDontHave.SetActive(true);
			TicketsHave.SetActive(false);
		}
	}

    private void CheckToggles()
    {
        if (multiplayerPageNumber == 50)
        {
			if ( UserProfile.Tickets >= Multiplayer.entryTickets)   
            {
				ShowInstruction();
            }
            else
            {
				ServerManager.Instance.SetReplayDataNull();
				Popup.instance.Show(true, "", "You don't have enough tickets to play multiplayer. You can either buy tickets or earn a ticket by watching a video.", "WATCH", true, "CANCEL", watchVideo);
            }
        }
		else if(multiplayerPageNumber==51)
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
			multiplayerPageNumber = 0;
		}
        else if (multiplayerPageNumber == 0)
        {
            for (int i = 0; i < roomToggle.Length; i++)
            {
                if (roomToggle[i].isOn == true)
                {
                    Multiplayer.roomType = i;
                    if (i == 0)
                    {       //User Clicked Public Room
                        Multiplayer.isHost = 0;
                        multiplayerPageNumber += 3;
                    }
                    else
                    {       //User Clicked Private Room
                        multiplayerPageNumber++;
                    }
                }
            }
        }
        else if (multiplayerPageNumber == 1)
        {
            for (int i = 0; i < createToggle.Length; i++)
            {
                if (createToggle[i].isOn == true)
                {
                    if (i == 0)
                    {   //User Clicked Create Private Match
                        Multiplayer.isHost = 1;
                        multiplayerPageNumber += 2;
                    }
                    else
                    {       //User Clicked Join Private Match
                        Multiplayer.isHost = 0;
                        multiplayerPageNumber++;
                    }
                }
            }
        }
        else if (multiplayerPageNumber == 2)
        {
            VerifyID();
            //multiplayerPageNumber += 2;
        }
        else if (multiplayerPageNumber == 3)
        {
            for (int i = 0; i < oversToggle.Length; i++)
            {
                if (oversToggle[i].isOn == true)
                {
                    if (i == 0)
                    {
                        Multiplayer.overs = 2;
                    }
                    else
                    {
                        Multiplayer.overs = 5;
                    }
                }
            }
            multiplayerPageNumber++;
        }
        else if (multiplayerPageNumber == 4)
        {

        }
        else if (multiplayerPageNumber == 5)
        {
            if (Multiplayer.isHost == 1)
            {
				LoadingScreen.instance.Show();
				ServerManager.Instance.StartPrivateMatch();
            }
        }
    }

	private void VerifyID ()
	{
		if (roomIDInput.text== string.Empty || roomIDInput.text== "Enter room ID.")
		{
			Popup.instance.showGenericPopup("", "Room ID can't be empty.");
		}
		else
		{
			Multiplayer.roomID = roomIDInput.text;
			ServerManager.Instance.JoinRoom ();
			roomIDInput.text = string.Empty;
		}
	}



	private void ShowInstruction()
	{
		HelpHolder.SetActive(false);
		multiplayerPageNumber = 51;
		instructionHolder.SetActive(true);
		InstuctionContent.SetActive(true);
		CONTROLLER.CurrentPage = "multiplayerpage";



        //		instrunctionText.text = "You can play multiplayer mode with 2 to 5 online players. You can choose from 2 different modes - Public or Private and also compete in 2-over or 5-over matches\n\n" +
        //"<b>PUBLIC</b>\n" +
        //"- Public mode allows you to compete against random players online. The match starts as soon as enough players are on board.\n\n" +
        //"<b>PRIVATE</b>\n" +
        //"- Private mode lets you create a private room where you can invite the people you know and want to compete with. You can choose to play either 2 or 5 overs match with your friends.\n\n" +
        //"<b>NOTE</b>\n" +
        //"- To create a room, choose the number of overs for the match and click continue.\n" +
        //"- This will generate a Room ID which you can share with your friends for them to join.\n" +
        //"- You can also join the room created by your friends as long as you know the room ID.\n" +
        //"- Remember that you need at least one other player to start a game.\n" +
        //"\n\nHit only fours and sixes in this mode for better points.\n" + "Points earned in this mode will be added in the leaderboard, make sure to login and be on top.\n\n" + "Here are the points:\n\n" + "1. For every four : " + CONTROLLER.boundaryPoint + " points.\n" + "2. For every six : " + CONTROLLER.sixPoint + " points.\n" + "3. For each single and double, \n\t" + CONTROLLER.singlePoint + " and " + CONTROLLER.doublePoint + " points respectively.";

        //instrunctionText.text = "Challenge yourself online against random opponents or create a private room and play with your friends. Compete now!";
    }

	#endregion

	#region FUNTIONS FROM INTERFACE HANDLER CLASS
	public IEnumerator checkTheStatus()
	{
		if (AdIntegrate.instance.checkTheInternet() && (!CONTROLLER.serverConfig.isReady || !CONTROLLER.isServerConfigSynced || (CONTROLLER.serverConfig.IM == 1 && CONTROLLER.BmpMaintenance == -1)))
		{
			yield return null;
			ShowLoadingScreen();
			ServerConfigReader.instance.DownloadServerConfig(OnContinuePostChecking);
		}
		else
			OnContinuePostChecking();
	}


	private void OnContinuePostChecking(bool dummy=false)
	{
		HideLoadingScreen();

		if (AdIntegrate.instance != null && AdIntegrate.instance.checkTheInternet())
		{
			if (CONTROLLER.BmpMaintenance == 1)
			{
				Popup.instance.showGenericPopup("SERVER MAINTENANCE", CONTROLLER.BmpMaintenanceText);
				return;
			}

			ContinueToBatMP();

			AdIntegrate.instance.ShowBannerAd();
		}
		else
		{
			Popup.instance.ShowNoInternetPopup();
		}
	}
	public void ContinueToBatMP()
	{
		CONTROLLER.gameMode = "multiplayer";
		StartCoroutine(CheckForBatMultiplayerConnection());
	}

	/*IEnumerator ConnectToMultiplayer()
	{
		ShowLoadingScreen(true);

		yield return 0;

		if (AdIntegrate.instance.checkTheInternet())
		{
			if (!CONTROLLER.IsUserLoggedIn())
			{
				CONTROLLER.tempCurrentPage = CONTROLLER.CurrentPage;
				SignInPanel.instance.Show();
			}
			else
			{
				StorePanel.instance.DisplayValuesInProducts();
				ServerManager.Instance.Connect();
			}
		}
		else
		{
			HideLoadingScreen();
			Popup.instance.ShowNoInternetPopup();
		}
	}*/

	private IEnumerator CheckForBatMultiplayerConnection()
	{
		ShowLoadingScreen(true);
		yield return new WaitForSeconds(0.5f);
		if (AdIntegrate.instance.checkTheInternet())
		{
			Invoke("ShowConnectionError", 28f);
			ServerManager.Instance.Connect();
		}
		else
		{
			HideLoadingScreen();
			Popup.instance.ShowNoInternetPopup();
		}
	}


	public void OnBattingMultiPlayerConnected()
	{
		CancelInvoke("ShowConnectionError");
		HideLoadingScreen();
		StorePanel.instance.DisplayValuesInProducts();
		CONTROLLER.canPressBackBtn = true;
		CONTROLLER.CurrentPage = "multiplayerpage";
		ShowMe(true);

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
        AudioPlayer.instance.PlayLandingPageIntoGameSFX(true);

        if (ServerManager.Instance.CanReplay())
            ClickedContinue();
    }

    private void ShowConnectionError()
	{
		HideLoadingScreen();
		Popup.instance.showGenericPopup("OOPS!!!", "Something went wrong. \n Please try again later.");
	}
	public IEnumerator LoadGroundScene()
	{
		if (Multiplayer.roomType == 2)
		{
			Multiplayer.roomType = -1;
		}

		PlayerPrefs.SetInt(CONTROLLER.BatMpCpSavedName, Multiplayer.playerCount);
		PlayerPrefs.Save();
		ServerManager.Instance.CheckManualInternetInterruption(false);

		yield return new WaitForSeconds(1);
		if (AdIntegrate.instance.checkTheInternet())
		{
			ManageScene.LoadScene(Scenes.Ground);
		}
		else
		{
			Popup.instance.ShowNoInternetPopup(HideMultiplayerMode);
		}
	}

	public void HideMultiplayerMode()
	{
		if (AdIntegrate.instance != null)
		{
			AdIntegrate.instance.HideAd();
		}
		CONTROLLER.canPressBackBtn = true;
		CONTROLLER.CurrentPage = "splashpage";
		ShowMe(false);
		GameModeSelector._instance.ShowLandingPage(true);
		if (ServerManager.Instance != null)
		{
			ServerManager.Instance.Disconnect();
			ServerManager.Instance.ExitRoom();
		}
		ServerManager.Instance.CheckManualInternetInterruption(true);

		AdIntegrate.instance.SystemSleepSettings(1);
        ServerManager.Instance.SetReplayDataNull();
    }

    public void ShowLoadingScreen(bool bCanShowCountdown = false)
	{
		CONTROLLER.tempCanPressBackBtn = CONTROLLER.canPressBackBtn;
		CONTROLLER.canPressBackBtn = false;
		LoadingScreen.instance.Show("Please ensure that you use a fast and stable internet to have a fluid experience");
		LoadingScreen.instance.fLoadingCountDownStrtTime = Time.time;
		if (bCanShowCountdown)
		{
			LoadingScreen.instance.LoadingScreenCountdown.gameObject.SetActive(true);
			LoadingScreen.instance.LoadingScreenCountdown.text = "";
		}
		else
			LoadingScreen.instance.LoadingScreenCountdown.gameObject.SetActive(false);
	}

	public void HideLoadingScreen()
	{
		CONTROLLER.canPressBackBtn = CONTROLLER.tempCanPressBackBtn;
		LoadingScreen.instance.Hide();

	}


	public void ShowGameExitPopup()
	{
		Popup.instance.Show(true, "", "Are you sure you want to \nexit the game ? ", yesString: "EXIT", yesCallBack: AdIntegrate.instance.GameExit, noString: "CANCEL");
	}

	#endregion
}
