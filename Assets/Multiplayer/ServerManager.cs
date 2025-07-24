#define SMDEBUG // UNCOMMENT THIS TO ENABLE DEBUG LOG
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP.SocketIO;

public class ServerManager : MonoBehaviour
{
	public static ServerManager Instance;
	public bool isOnlineNodeServer = true ;
//	public UILabel uiText;

	#region SM_Variables
	private SocketManager _SocketManager;

	private string SocketUri
	{
		get
		{
			if (isOnlineNodeServer)
			{
				//return "ws://boc.batattackcricket.com:8086/socket.io/"; 	// BOC2 live url
				return "ws://boc.batattackcricket.com:8080/socket.io/";     //live url
			}
			else
			{
                return "ws://192.168.1.40:8086/socket.io/"; // Local URL WCC2
            }
        }
	}

	public ScoreBoardMultiPlayer scoreBoardScript;
	//	public CountDown  countdownScript;
	public static MultiplayerReplayData ReplayData;
    #endregion

    #region Unity_Function_Overloads
    void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad (this.gameObject);
		}
		SetupSocketManager();
		SetReplayDataNull();
		//AssignDataForReplay(false);	//hardcode

		//DebugLogger.PrintWithColor("############# AWAKE CALLED ################# _SocketManager.State:: " + _SocketManager.State);
        if (_SocketManager.State != SocketManager.States.Open)
        {
            _SocketManager.Open();
        }
    }

    #region REPLAY 
    public void SetReplayDataNull()
	{
        ReplayData = null;
    }
	public void AssignDataForReplay(bool original=true)
	{
        ReplayData = new MultiplayerReplayData();
		if (original)
		{
			ReplayData.overs = Multiplayer.overs;
			ReplayData.roomType = Multiplayer.roomType;
			ReplayData.isHost = Multiplayer.isHost;
		}
		else
		{
            ReplayData.overs = 2;
            ReplayData.roomType = 0;
            ReplayData.isHost = 0;
        }
    }
	public bool CanReplay()
	{
        if (ReplayData != null)
			return true;
		else
			return false;
	}
    #endregion 

    void OnLevelWasLoaded (int level)
	{
		if (level == 1)
		{
			Application.runInBackground = false;
			AdIntegrate.instance.SystemSleepSettings(1);
		}

		if ((level == 2 || level == 3) && CONTROLLER.gameMode == "multiplayer")
		{
			Application.runInBackground = true;
			AdIntegrate.instance.SystemSleepSettings(0);
			scoreBoardScript = GameObject.Find ("ScoreBoard_MultiPlayer").GetComponent <ScoreBoardMultiPlayer> ();
//			countdownScript = GameObject.Find ("CountDown").GetComponent <CountDown> ();
			scoreBoardScript.SortAndRankScores ();
			StartGameAfterCountdown ();
		}

		if (level == 2)
		{
//			countdownScript = GameObject.Find ("CountDown").GetComponent <CountDown> ();
		}
	}

	public void  StartGameAfterCountdown ()
	{		
		BattingScoreCard.instance.HideMe ();
		GameModel.instance.ShowIntroAnimation ();
	}
	#endregion
	
	#region Socket Manager
	private void SetupSocketManager()
	{
		// Change an option to show how it should be done
		SocketOptions options = new SocketOptions();
		options.AutoConnect = false;
		options.Reconnection = true;
		options.Timeout = TimeSpan.FromSeconds (10.0f);

		// Create the Socket.IO manager
		_SocketManager = new SocketManager(new Uri(SocketUri),options); 
		SetupSocketManagerEvents();		 
	}
	
	private void SetupSocketManagerEvents()
	{
		// Set up connected and disconnected events
		_SocketManager.Socket.On("player connected", Connected);
		_SocketManager.Socket.On("disconnect", Disconnected);
		_SocketManager.Socket.On("room info", RoomInfo);
		_SocketManager.Socket.On("player duplicate", DuplicatePlayer);
		_SocketManager.Socket.On("player entered room", PlayerEnteredRoom);
		_SocketManager.Socket.On("player exit room", PlayerExitedRoom);
		_SocketManager.Socket.On("countdown", Countdown);
		_SocketManager.Socket.On("game start", GameStart);
		_SocketManager.Socket.On("game update", GameUpdate);
		_SocketManager.Socket.On("game next ball", GameNextBall);
		_SocketManager.Socket.On("game finish", GameFinish);
		_SocketManager.Socket.On("game cancel", GameCancel);
		_SocketManager.Socket.On("game abandoned", GameAbandoned);
		_SocketManager.Socket.On("room found", RoomFound);
		_SocketManager.Socket.On("room not found", RoomNotFound);
		_SocketManager.Socket.On("countdown rematch", CountdownRematch);
		_SocketManager.Socket.On("emoticon recieve", RecieveEmoticon);

		// The argument will be an Error object.
		//_SocketManager.Socket.On(SocketIOEventTypes.Error, (socket, packet, args) => Debug.Log(string.Format("Error: {0}", args[0].ToString())));
		_SocketManager.Socket.On(SocketIOEventTypes.Error, OnError);
	}


	//Added by Stanley on Jul 21,2016
	void OnError(Socket socket, Packet packet, params object[] args)
	{
		Error error = args[0] as Error;
//		uiText.text = "SOCKET ERROR "+error.ToString ();
		switch (error.Code)
		{
		case SocketIOErrors.User:
			//Debug.Log("Exception in an event handler! "+error.ToString ());
			break;
		case SocketIOErrors.Internal:
				if (gameObject.activeSelf)
					StartCoroutine(CheckNet ());
			break;
		default:
			//Debug.Log("Server error!");
			break;
		}

		string[] err = error.ToString().Split ('"');

		if (err[1] == "ConnectionTimedOut")
		{
			MultiplayerPage.instance.HideMultiplayerMode (); 
		}
	}
	private bool fireOnce = false;
	private IEnumerator CheckNet ()
	{
        yield return 0;// StartCoroutine (NetworkManager.Instance.CheckInternetConnection ());
		if(CONTROLLER.CurrentPage == "multiplayerpage" || UserProfile.isInMultiplayer)
		{
			if(!AdIntegrate.instance.checkTheInternet() && !fireOnce)
			{
				//Debug.Log ("INTERNET DISCONNECTED!! SHOW ERROR  Client ");
				//_SocketManager.Close ();
				StartCoroutine (HandleDisconnect ());
				UserProfile.isInMultiplayer = false;
				fireOnce = true;

			}
		}
	}
	//End by Stanley on Jul 21,2016

	private void Disconnected(Socket socket, Packet packet, object[] args)
	{
		#if SMDEBUG
		Debug.Log ("Disconnected from Server. Call from Server ");
		#endif

		if (UserProfile.isInMultiplayer)
		{
			ExitRoom();

			StartCoroutine(HandleDisconnect ());
			UserProfile.isInMultiplayer = false;
		}
	}

	IEnumerator HandleDisconnect ()
	{	
        yield return 0;// StartCoroutine (NetworkManager.Instance.CheckInternetConnection ());

		if (AdIntegrate.instance.checkTheInternet())
		{
			if (ManageScene.CurScene == Scenes.MainMenu)
			{
				if(CONTROLLER.gameMode == "multiplayer")
					Popup.instance.showGenericPopup("", "You have been disconnected from the game. Please make sure your Internet connection is stable.", MultiplayerPage.instance.HideMultiplayerMode);
			}
			else if (ManageScene.CurScene == Scenes.Ground)
			{				
				GroundScriptHandler.Instance.ShowServerDisconnectedPopup ();
			}
		} 
		else
		{
			if (ManageScene.CurScene == Scenes.MainMenu)
			{
				Popup.instance.ShowNoInternetPopup(MultiplayerPage.instance.HideMultiplayerMode);
			}
			else if (ManageScene.CurScene ==Scenes.Ground)
			{				
				Popup.instance.ShowNoInternetPopup(	GroundScriptHandler.Instance.LoadMainMenuScene);
			}
		}


		/*	if(Achievements .instance !=null)
			Achievements.instance.SuperMultiplayer (100,true );*/

	}
	#endregion

	#region Server to Game Responses
	private void Connected(Socket socket, Packet packet, object[] args)
	{
		#if SMDEBUG
		Debug.Log ("Connected to Server._SocketManager.State:: " + _SocketManager.State);
		#endif

		string jsonData = packet.ToString ();

		SimpleJSON.JSONNode node = SimpleJSON.JSONNode.Parse (jsonData);

		Multiplayer.entryTickets =int.Parse ("" + node [1] ["entryCoins"]);
		int[] winningCoins = new int[5];
		if("" + node [1] ["winCoins2"]!=null )
		{
			for (int i = 0; i < 5; i++)
			{
				winningCoins [i] = int.Parse ("" + node [1] ["winCoins2"] [i]);
			}
			Multiplayer.winningCoins2 = winningCoins;
		}
		else 
		{
			Multiplayer.winningCoins2=new int[5]{500,400,300,200,100};
		}
		winningCoins = new int[5];
		if("" + node [1] ["winCoins5"]!=null )
		{
			for (int i = 0; i < 5; i++)
			{
				winningCoins [i] = int.Parse ("" + node [1] ["winCoins5"] [i]);
			}
			Multiplayer.winningCoins5 = winningCoins;
		}
		else 
		{
			Multiplayer.winningCoins5=new int[5]{1000,800,600,400,200};
		}



		if (_SocketManager.State != SocketManager.States.Open)
		{
		} 
		else
		{
			MultiplayerPage.instance.OnBattingMultiPlayerConnected();
		}
	}

	private void RoomInfo(Socket socket, Packet packet, object[] args)
	{
		#if SMDEBUG
		Debug.Log ("Room Info"+packet.ToString ());
		#endif

		if (Multiplayer.roomType == 2)
		{
			MultiplayerPage.instance.HideWaitingForOtherPlayers ();
		}
		UserProfile.isInMultiplayer = true;
		Multiplayer.oversData.Clear ();
		Array.Clear (Multiplayer.playerList,0,Multiplayer.playerList.Length);

		string jsonData = packet.ToString ();

		SimpleJSON.JSONNode node = SimpleJSON.JSONNode.Parse (jsonData);

		Multiplayer.overs = int.Parse ("" + node [1]["overs"]);
		Multiplayer.roomID = "" + node [1]["roomID"];
		MultiplayerPage.instance.SetRoomIDValue (Multiplayer.roomID);

		#region PlayerList
		string[] playerNames = ("" + node [1]["playerList"]).Split ('|');

		int playerCount = playerNames.Length/3;
		Multiplayer.playerCount = playerCount;
		int k=0;
		for (int i=0; i<5; i++,k+=3)
		{
			Multiplayer.playerList [i] = new PlayerList ();
			if (i < playerCount)
			{
				Multiplayer.playerList [i].PlayerId = GetUserId(playerNames[k]); //int.Parse (playerNames [k]);
				Multiplayer.playerList [i].PlayerName = playerNames [k+1];
                Multiplayer.playerList[i].Cp = playerNames[k + 2];
                Multiplayer.playerList[i].playerIdwithCountryCode = playerNames[k];
            }
            else
			{
				Multiplayer.playerList [i].PlayerId = -1;
				Multiplayer.playerList [i].PlayerName = "waiting for player...";
                Multiplayer.playerList[i].Cp = "0";
                Multiplayer.playerList[i].playerIdwithCountryCode = string.Empty;

            }
        }
		#endregion

		#region Ball Parameters
		for (int i=0; i<Multiplayer.overs; i++)
		{
			MultiplayerOver newOver = new MultiplayerOver ();

			int bowlerHand = int.Parse ("" + node [1]["ballParameters"][i]["bowlerHand"]);
			if (bowlerHand == 0)
			{
				newOver.bowlerHand = "left";
			}
			else
			{
				newOver.bowlerHand = "right";
			}

			int bowlerSide = int.Parse ("" + node [1]["ballParameters"][i]["bowlerSide"]);
			if (bowlerSide == 0)
			{
				newOver.bowlerSide = "left";
			}
			else
			{
				newOver.bowlerSide = "right";
			}

			int bowlerType = int.Parse ("" + node [1]["ballParameters"][i]["bowlerType"]);
			if (bowlerType == 0)
			{
				newOver.bowlerType = "fast";
			}
			else if (bowlerType == 1)
			{
				newOver.bowlerType = "offspin";
			}
			else
			{
				newOver.bowlerType = "legspin";
			}

			for (int j=0; j<6; j++)
			{
				newOver.bowlingAngle [j] = int.Parse ("" + node [1]["ballParameters"][i]["bowlingAngle"][j]);
				newOver.bowlingSpeed [j] = int.Parse ("" + node [1]["ballParameters"][i]["bowlingSpeed"][j]);

				float xL = Globalization.FloatParse("" + node [1]["ballParameters"][i]["bowlingSpot"]["LHB"][j][0]);
				float yL = Globalization.FloatParse("" + node [1]["ballParameters"][i]["bowlingSpot"]["LHB"][j][1]);
				float zL = Globalization.FloatParse("" + node [1]["ballParameters"][i]["bowlingSpot"]["LHB"][j][2]);

				Vector3 tempVectorL = new Vector3 (xL,yL,zL);
				newOver.bowlingSpotL [j] = tempVectorL;

				float xR = Globalization.FloatParse("" + node [1]["ballParameters"][i]["bowlingSpot"]["RHB"][j][0]);
				float yR = Globalization.FloatParse("" + node [1]["ballParameters"][i]["bowlingSpot"]["RHB"][j][1]);
				float zR = Globalization.FloatParse("" + node [1]["ballParameters"][i]["bowlingSpot"]["RHB"][j][2]);

				Vector3 tempVectorR = new Vector3 (xR,yR,zR);
				newOver.bowlingSpotR [j] = tempVectorR;
			}

			Multiplayer.oversData.Add (newOver);
		}
		#endregion

		MultiplayerPage.instance.UpdatePlayerList ();
	}

	private void DuplicatePlayer(Socket socket, Packet packet, object[] args)
	{
#if SMDEBUG
		Debug.Log ("Duplicate Player.");
#endif
		LoadingScreen.instance.Hide();
		Popup.instance.showGenericPopup("", "Looks like your account is already in use on another device. Try another?", MultiplayerPage.instance.HideMultiplayerMode);
	}

	private void PlayerEnteredRoom(Socket socket, Packet packet, object[] args)
	{
		#if SMDEBUG
		Debug.Log ("Player Entered Room."+packet.ToString ());
		#endif
		string jsonData = packet.ToString ();

		SimpleJSON.JSONNode node = SimpleJSON.JSONNode.Parse (jsonData);

		Multiplayer.playerList [Multiplayer.playerCount].PlayerName = "" + node [1]["playerName"];
		Multiplayer.playerList [Multiplayer.playerCount].PlayerId = GetUserId("" + node[1]["playerID"]); // int.Parse ("" + node [1]["playerID"]);
        Multiplayer.playerList[Multiplayer.playerCount].playerIdwithCountryCode = "" + node[1]["playerID"];

        if (node[1]["cp"] != null)
            Multiplayer.playerList[Multiplayer.playerCount].Cp = "" + node[1]["cp"];
        else
            Multiplayer.playerList[Multiplayer.playerCount].Cp = "0";
        Multiplayer.playerCount++;
		MultiplayerPage.instance.UpdatePlayerList ();
	}

	private void PlayerExitedRoom(Socket socket, Packet packet, object[] args)
	{
		#if SMDEBUG
		Debug.Log ("Player Exited Room."+packet.ToString ());
		#endif
		string jsonData = packet.ToString ();

		SimpleJSON.JSONNode node = SimpleJSON.JSONNode.Parse (jsonData);


        int id = GetUserId("" + node[1]["playerID"]);       // int.Parse ("" + node [1] ["playerID"]);
        string pidwithcode = "" + node[1]["playerID"];

        // to show the player name 
        if (pidwithcode != CONTROLLER.BattingMultiplayerCountryCode + CONTROLLER.M_USERID && ManageScene.CurScene ==Scenes.Ground && !ScoreBoardMultiPlayer .instance .MultiplayerGameOverPage.activeSelf )
		{
//#if UNITY_ANDROID && !UNITY_EDITOR
			//NextwaveMarshmallowPermission .instance .ShowToast ("" + node [1] ["playerName"]+" has left"); 
//#else
			GameObject prefabGO ;
			GameObject tempGO ;
			prefabGO = Resources.Load ("Prefabs/Toast")as GameObject ;
			tempGO = Instantiate (prefabGO)as GameObject ;
			tempGO.name = "Toast";
			tempGO.GetComponent <Toast > ().setMessge ("" + node [1] ["playerName"]+" has left");
			//#endif
		}

		//PlayerList temp = Array.Find (Multiplayer.playerList, t => t.PlayerId == id);
		PlayerList temp = Array.Find (Multiplayer.playerList, t => t.PlayerId == id && t.playerIdwithCountryCode==pidwithcode);

		temp.PlayerId = -1;
		temp.PlayerName = "waiting for player...";
        temp.Cp = "0";
        temp.playerIdwithCountryCode = string.Empty;

        List<PlayerList> tempList = new List<PlayerList> ();

		for (int i = 0; i < 5; i++)
		{
			if (Multiplayer.playerList [i] != null)
			{
				if (Multiplayer.playerList [i].PlayerId != -1)
				{
					PlayerList tem = Multiplayer.playerList [i];
					tempList.Add (tem);
				}
			}
		}

		for (int j = 0; j < 5; j++)
		{
			if (j < tempList.Count)
			{
				Multiplayer.playerList [j] = tempList [j];
			} 
			else
			{
				PlayerList tempScore = new PlayerList ();
				tempScore.PlayerId = -1;
				tempScore.PlayerName = "waiting for player...";
                tempScore.Cp = "0";
                tempScore.playerIdwithCountryCode = string.Empty;
                Multiplayer.playerList [j] = tempScore;
			}
		}

		Multiplayer.playerCount --;
		if (ManageScene.CurScene == Scenes.MainMenu)
		{
			MultiplayerPage.instance.UpdatePlayerList ();
		} 
		else
		{
            MultiplayerScore tempScoreUser = Array.Find(Multiplayer.playerScores, t => t.PlayerId == id && t.playerIdwithCountryCode == pidwithcode);

            int pos = Array.FindIndex(Multiplayer.playerScores, t => t.PlayerId == id && t.playerIdwithCountryCode == pidwithcode);

            if (pos != -1)
                StartCoroutine(EmoticonAnimator.Instance.playerEmoticon[pos].StopAnimation());

            tempScoreUser.PlayerId = -1;
			tempScoreUser.Username = "waiting for player...";
            tempScoreUser.playerIdwithCountryCode = string.Empty;

            List<MultiplayerScore> tempScoreList = new List<MultiplayerScore> ();

			for (int k = 0; k < Multiplayer.playerCount+1; k++)
			{
				if (Multiplayer.playerScores [k] != null)
				{
					if (Multiplayer.playerScores [k].PlayerId != -1)
					{
						tempScoreList.Add (Multiplayer.playerScores [k]);
					}
				}
			}

			for (int j = 0; j < 5; j++)
			{
				if (j < tempScoreList.Count)
				{
					Multiplayer.playerScores [j] = tempScoreList [j];
				} 
				/*else
				{
					MultiplayerScore tempScores = new MultiplayerScore ();
					tempScores.PlayerId = -1;
					tempScores.Username = "waiting for player...";

					Multiplayer.playerScores [j] = tempScores;
				}*/
			}
			if(scoreBoardScript!=null)
				scoreBoardScript.UpdateMultiplayerScores ();
		}	
	}

	private void Countdown(Socket socket, Packet packet, object[] args)
	{
		#if SMDEBUG
		//Debug.Log ("Countdown.");
		#endif
		if (Multiplayer.roomType == 2)
		{
			MultiplayerPage.instance.HideWaitingForOtherPlayers ();
		}
		MultiplayerPage.instance.UpdateCountdown (int.Parse (args[0].ToString ()));
	}

	private void GameStart (Socket socket, Packet packet, params object[] args)
	{
		#if SMDEBUG
		Debug.Log ("====Game Start.");
		#endif

		if (ManageScene.CurScene == Scenes.MainMenu && Popup.instance.isShowing )
		{
			return;
		}			

		

		Multiplayer.playerScores = new MultiplayerScore[Multiplayer.playerCount];
		for (int i = 0; i < Multiplayer.playerCount; i++)
		{
			MultiplayerScore tempScore = new MultiplayerScore ();
			tempScore.Username = Multiplayer.playerList [i].PlayerName;
			tempScore.PlayerId = Multiplayer.playerList [i].PlayerId;
            tempScore.Cp = Multiplayer.playerList[i].Cp;
            tempScore.playerIdwithCountryCode = Multiplayer.playerList[i].playerIdwithCountryCode;
            tempScore.Rank = 0;
			tempScore.Score = "0";
			tempScore.Wickets = 0;
			tempScore.LastBallScore = 0;
			Multiplayer.playerScores [i] = tempScore;
		}
		CONTROLLER.totalOvers = Multiplayer.overs;
		CONTROLLER.currentMatchWickets = 0;
		CONTROLLER.totalWickets = 10;
		
		/*UserProfile.Tickets -= Multiplayer.entryTickets;
		UserProfile.SpentTickets += Multiplayer.entryTickets;
		UserProfile.spentTotTickets += 1;

		PlayerPrefsManager.SaveUserProfile ();
        if(UserProfile.Tickets <=0)
        {
            UserProfile.Tickets = 0;
        }
		GameModeSelector._instance.userTickets.text = UserProfile.Tickets.ToString ();  
		MultiplayerPage.instance.TicketsCount_have.text = "TICKETS LEFT " + UserProfile.Tickets.ToString ();
		*/
        DBTracking.instance.isTicketUsed = true;
		DBTracking.instance.CurrentMatchID=string.Empty;


        StartCoroutine(MultiplayerPage.instance.LoadGroundScene ());
			
	}

	private void GameUpdate(Socket socket, Packet packet, object[] args)
	{
		#if SMDEBUG
		Debug.Log ("Game Update.");
		#endif

		string jsonData = packet.ToString ();

		SimpleJSON.JSONNode node= SimpleJSON.JSONNode.Parse (jsonData);
        int playerId = GetUserId("" + node[1]["playerID"]);     // int.Parse ("" + node [1] ["playerID"]);
        string pidwithcountrycode = "" + node[1]["playerID"];
        string score = "" + node [1] ["score"];
		int wickets = int.Parse ("" + node [1] ["wickets"]);
		int lastBallScore = int.Parse ("" + node [1] ["lastBallScore"]);

        int index = Array.FindIndex(Multiplayer.playerScores, t => t.PlayerId == playerId && t.playerIdwithCountryCode == pidwithcountrycode);

		if (index != -1)
		{
			MultiplayerScore tempPlayerScore = Multiplayer.playerScores[index];
			tempPlayerScore.Score = score;
			tempPlayerScore.Wickets = wickets;
			tempPlayerScore.LastBallScore = lastBallScore;
			tempPlayerScore.playerIdwithCountryCode = pidwithcountrycode;

			Multiplayer.playerScores[index] = tempPlayerScore;

			if (scoreBoardScript != null)
				scoreBoardScript.SortAndRankScores();
		}
	}

	private void GameNextBall(Socket socket, Packet packet, object[] args)
	{
		#if SMDEBUG
		Debug.Log ("Game Next Ball.");
		#endif

		if (CONTROLLER.currentMatchWickets < 10)
		{
			GameModel.instance.BowlNextBall ();
			scoreBoardScript.HideWait ();
			scoreBoardScript.AnimateBoard ();
		}
		else
		{
			scoreBoardScript.HideWait ();
			scoreBoardScript.ShowWicketsWait ();
			SendMatchScore (CONTROLLER.currentMatchScores.ToString (), CONTROLLER.currentMatchWickets, 0);
		}
		CheckManualInternetInterruption(false);

	}

	private void GameFinish(Socket socket, Packet packet, object[] args)
	{
		#if SMDEBUG
		Debug.Log ("Game Finish."+packet.ToString ());
		#endif
		int bonusPoints = 0, bonusCP = 0;
		string jsonData = packet.ToString ();

		SimpleJSON.JSONNode node= SimpleJSON.JSONNode.Parse (jsonData);

		int playerCount = int.Parse ("" + node [1].Count);

		for (int i = 0; i < playerCount; i++)
		{
			MultiplayerScore tempScore = new MultiplayerScore ();

            tempScore.PlayerId = GetUserId("" + node[1][i]["id"]);        // int.Parse ("" + node [1] [i] ["id"]);
            tempScore.playerIdwithCountryCode = "" + node[1][i]["id"];
            tempScore.Score = "" + node [1] [i] ["runs"];
			tempScore.Rank = int.Parse ("" + node [1] [i] ["rank"]);
			tempScore.Wickets = int.Parse ("" + node [1] [i] ["wickets"]);
            tempScore.Username = "" + node[1][i]["name"];

            if (node[1][i]["cp"] != null)
                tempScore.Cp = "" + node[1][i]["cp"];
            else
                tempScore.Cp = "0";

            Multiplayer.playerScores [i] = tempScore;
            if (tempScore.playerIdwithCountryCode == CONTROLLER.BattingMultiplayerCountryCode + CONTROLLER.M_USERID)
            {
                int index = /*Multiplayer.GetWinningCostIndex */	(tempScore.Rank - 1);


                if (Multiplayer.overs == 2)
                    bonusPoints = Multiplayer.winningCoins2[index];
                else
                    bonusPoints = Multiplayer.winningCoins5[index];

                int index2 = Multiplayer.GetWinningCostIndex(PlayerPrefs.GetInt(CONTROLLER.BatMpCpSavedName, Multiplayer.playerCount), tempScore.Rank - 1);
                bonusCP = CONTROLLER.BatMPRewards_CP[index2];
            }
		}

		scoreBoardScript.HideWait ();
		scoreBoardScript.HideWicketsWait ();

/*		if(Multiplayer.roomType == 0 || Multiplayer.roomType == -1)
		{
			//GOPI		RandomDrop_UI.instance.coinsToGive = userCoins;
			//Gopi	StartCoroutine (RandomDrop_UI.instance.OpenRandomDrop ());
		}
		else*/
		{
			scoreBoardScript.SetMultiplayerGameOverTexts ();
			scoreBoardScript.ShowMultiPlayerGameOver (bonusPoints, bonusCP);
		}
		ExitRoom ();

		
	}

	private void GameCancel(Socket socket, Packet packet, object[] args)
	{
		#if SMDEBUG
		Debug.Log ("Game Cancel. "+CONTROLLER.CurrentPage);
		#endif
		//ID - 0010588 - FIXED ON 09-02-2016 - KARTHIK - 0.25
		if (ManageScene.CurScene == Scenes.MainMenu)
		{
			MultiplayerPage.instance.HideWaitingForOtherPlayers ();
			Popup.instance.showGenericPopup("", " Looks like there's nobody to play against at the moment! Come back later?", MultiplayerPage.instance.HideMultiplayerMode);
		}
	}

	private void GameAbandoned(Socket socket, Packet packet, object[] args)
	{
		#if SMDEBUG
		Debug.Log ("Game Abandoned.");
		#endif

		string hostName = args[0].ToString ();
		if(AdIntegrate.instance.CurrentSceneIndex==1)
		{
			Popup.instance.showGenericPopup("", "The host " + hostName + " has abandoned the private match.", MultiplayerPage.instance.HideMultiplayerMode);
		}
	}

	private void RoomFound(Socket socket, Packet packet, object[] args)
	{
#if SMDEBUG
		Debug.Log ("Room Found.");
#endif
		MultiplayerPage.instance.ShowLobbyForPrivateJoin ();
	}

	private void RoomNotFound(Socket socket, Packet packet, object[] args)
	{
		#if SMDEBUG
		Debug.Log ("Room Not Found.");
		#endif
		Popup.instance.showGenericPopup("","Room ID you're trying to join is either expired or does not exist");
	}

	private void CountdownRematch(Socket socket, Packet packet, object[] args)
	{
		#if SMDEBUG
		Debug.Log ("Countdown Rematch.");
		#endif
		//ID - 0010588 - FIXED ON 09-02-2016 - KARTHIK - 0.25
		if (ManageScene.CurScene == Scenes.Ground)
		{
			scoreBoardScript.ReplayCountdown (int.Parse (args[0].ToString ()));
		}
		else if (ManageScene.CurScene ==Scenes.MainMenu)
		{
			if (CONTROLLER.CurrentPage != "multiplayerpage")
			{
				Multiplayer.roomType = -1;
			}
		}
	}

	private void RecieveEmoticon(Socket socket, Packet packet, object[] args)
	{
		#if SMDEBUG
		Debug.Log ("Recieve Emoticon.");
		#endif
		string jsonData = packet.ToString ();
		
		SimpleJSON.JSONNode node = SimpleJSON.JSONNode.Parse (jsonData);

        int playerID = GetUserId("" + node[1]["playerID"]);     // int.Parse ("" + node [1] ["playerID"]);
        string pidwithCode = "" + node[1]["playerID"];
        string emoticon = "" + node[1]["emoticonindex"];
        if (EmoticonAnimator.Instance != null)
            EmoticonAnimator.Instance.AnimateEmoticon(playerID, emoticon, -1, pidwithCode);
    }
	#endregion
	
	#region Connect and Disconnect Socket
	public void Connect()
	{
#if SMDEBUG
        Debug.Log("Connect called::::: SocketManager State: "+ _SocketManager.State);
#endif
        if (_SocketManager.State != SocketManager.States.Open) 
		{
			_SocketManager.Open ();
		}
		PlayerConnect ();
	}
	
	public void Disconnect()
	{
		#if SMDEBUG
		Debug.Log ("Multiplayer Connection will not be Disconnected.");
		#endif
		UserProfile.isInMultiplayer = false;
	}
	#endregion

	#region Game to Server Requests
	public void PlayerConnect ()
	{
#if SMDEBUG
		Debug.Log ("Player Connect. profile id: "+CONTROLLER.M_USERID);
#endif

		JSONObject tempJson = new JSONObject ();
        tempJson.AddField("playerID", CONTROLLER.BattingMultiplayerCountryCode + CONTROLLER.M_USERID);
        _SocketManager.Socket.Emit ("player connect",tempJson.ToDictionary ());
	}

	public void FindRoom ()
	{
		#if SMDEBUG
		Debug.Log ("Find Room: overs:::: " + Multiplayer.overs);
		#endif
		JSONObject tempJson = new JSONObject ();
        tempJson.AddField("playerID", CONTROLLER.BattingMultiplayerCountryCode + CONTROLLER.M_USERID);        
		tempJson.AddField ("playerName", CONTROLLER.UserName);
		tempJson.AddField ("roomOvers",Multiplayer.overs);
        tempJson.AddField("cp", CONTROLLER.CricketPoints);
        _SocketManager.Socket.Emit ("find room",tempJson.ToDictionary ());
	}

	public void CreateRoom ()
	{
		#if SMDEBUG
		Debug.Log ("Create Room.");
		#endif
		JSONObject tempJson = new JSONObject ();
        tempJson.AddField("playerID", CONTROLLER.BattingMultiplayerCountryCode + CONTROLLER.M_USERID);
		tempJson.AddField ("playerName", CONTROLLER.UserName);
		tempJson.AddField ("roomOvers",Multiplayer.overs);
        tempJson.AddField("cp", CONTROLLER.CricketPoints);
        _SocketManager.Socket.Emit ("create room",tempJson.ToDictionary ());
	}

	public void JoinRoom ()
	{
		#if SMDEBUG
		Debug.Log ("Join Room.");
		#endif
		JSONObject tempJson = new JSONObject ();
        tempJson.AddField("playerID", CONTROLLER.BattingMultiplayerCountryCode + CONTROLLER.M_USERID);
        tempJson.AddField ("playerName", CONTROLLER.UserName);
		tempJson.AddField ("roomID",Multiplayer.roomID);
        tempJson.AddField("cp", CONTROLLER.CricketPoints);
        _SocketManager.Socket.Emit ("join room",tempJson.ToDictionary ());
	}

	public void ExitRoom ()
	{
		#if SMDEBUG
		Debug.Log ("Exit Room "+Multiplayer.roomID);
		#endif
		Multiplayer.roomID = string.Empty;

		if (Multiplayer.roomType == 2)
		{
			Multiplayer.roomType = -1;
		}

		JSONObject tempJson = new JSONObject ();		
        tempJson.AddField("playerID", CONTROLLER.BattingMultiplayerCountryCode + CONTROLLER.M_USERID);
        _SocketManager.Socket.Emit ("exit room",tempJson.ToDictionary ());
		
		CheckManualInternetInterruption(true);

	}

	public void StartPrivateMatch ()
	{
		#if SMDEBUG
		Debug.Log ("Match Start");
		#endif
		_SocketManager.Socket.Emit ("match start");
	}

	public void SendMatchScore (string matchScore, int wickets, int lastBallScore)
	{
		#if SMDEBUG
		Debug.Log ("Match Score");
		#endif
		if (UserProfile.isInMultiplayer)
		{
			JSONObject tempJson = new JSONObject ();
            tempJson.AddField("playerID", CONTROLLER.BattingMultiplayerCountryCode + CONTROLLER.M_USERID);
			tempJson.AddField ("playerName", CONTROLLER.UserName);
			tempJson.AddField ("score", matchScore);
			tempJson.AddField ("wickets", wickets);
			tempJson.AddField ("lastBallScore", lastBallScore);

			_SocketManager.Socket.Emit ("match score", tempJson.ToDictionary ());
			if (CONTROLLER.currentMatchWickets < 10)
			{
				scoreBoardScript.ShowWait ();
			}
		}
	}

	public void RematchRoom ()
	{
		#if SMDEBUG
		Debug.Log ("Rematch Room");
		#endif
		_SocketManager.Socket.Emit ("rematch room");
		Multiplayer.roomType = 2;
		ManageScene.LoadScene(Scenes.MainMenu);
	}

	public void SendEmoticon (string _emoticon)
	{
		JSONObject tempjson = new JSONObject ();
        tempjson.AddField("playerID", CONTROLLER.BattingMultiplayerCountryCode + CONTROLLER.M_USERID);
		tempjson.AddField ("emoticonindex", _emoticon);

		_SocketManager.Socket.Emit ("emoticon send", tempjson.ToDictionary ());
	}
    #endregion

    private int GetUserId(string str)
    {
        // return int.Parse(str);
        int tID = -1;
        string[] datas = str.Split('-');
        if (datas.Length == 2)
            tID = int.Parse(datas[1]);
        else
            tID = int.Parse(datas[0]);

        // Debug.Log("*****  Given str:************************* " + str + " datas.len: " + datas.Length + " tid: " + tID);
        return tID;

    }


	private void CheckInternetResponse(bool flag)
	{
		if (!flag && CONTROLLER.gameMode == "multiplayer")
		{
			if (ManageScene.CurScene == Scenes.MainMenu)
			{
				Popup.instance.ShowNoInternetPopup(MultiplayerPage.instance.HideMultiplayerMode);
			}
			else if (ManageScene.CurScene == Scenes.Ground)
			{
				Popup.instance.ShowNoInternetPopup(GroundScriptHandler.Instance.LoadMainMenuScene);
			}
		}
		else if (flag && CONTROLLER.gameMode == "multiplayer")
		{
			CheckManualInternetInterruption(false);
		}
	}

	//for Avoiding manual internet interruption

	public void CheckManualInternetInterruption(bool Cancel)        //true--stop  false-start
	{
		if (Cancel)
		{
			CancelInvoke("ForInvoke");
		}
		else
		{
			CancelInvoke("ForInvoke");
			Invoke("ForInvoke", 5f);       //10 sec time out
		}
	}

	private void ForInvoke()
	{
		if (!AdIntegrate.instance.checkTheInternet())
		{
			CheckInternetResponse(false);
		}
		else
		{
			CheckInternetResponse(true);
		}
	}
}