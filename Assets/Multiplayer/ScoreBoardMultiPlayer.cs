using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class ScoreBoardMultiPlayer : MonoBehaviour 
{
	public static ScoreBoardMultiPlayer instance;

	public GameObject InGameHolder;
	public GameObject _ScoreHandler_Multiplayer;

	public Canvas GameoverCanvas;

	public Text CurrentScore;	 	//0 = Endless, 1 = Multiplayer, 2 = Missions
	public Text BallsFaced;		//0 = Endless, 1 = Multiplayer, 2 = Missions
	public Text positionTxt;
	public Text numberTxt;
	public GameObject postionGO;

	public Text MultiplayerTotalOversLabel;

	public Image[] multiplayerBG;
	public Text[] multiplayerRank;
	public Text[] multiplayerUsername;
	public Text[] multiplayerScore;
	public Text[] multiplayerLastScore;

	public Sprite userBG, otherBG;

	public GameObject MultiplayerGameOverPage,EmoticonPanel;
	public Text multiplayerPositionTxt,multiplayerPositionTxt2;
	string  multiplayerPosSuperTxt;
	public Text multiplayerEarningsPoints,multiplayerBonusPoints;
	public Image [] multiplayerGameOverBG;
	public GameObject [] multiplayerGameOverShine;
	public Text[] multiplayerGameOverRank;
	public Text[] multiplayerGameOverUsername;
	public Text[] multiplayerGameOverScore;


	public GameObject WaitForOthersPanel, WaitForOthersWicketsOverPanel;

	public GameObject[] MultiplayerPlayerScoreBG;

	public GameObject multiplayerFinalBallCountdown;
	public Text ballsLeftInMultiplayer;

	string contentToShare = "";


	Text replayCountdown;

	private float[] yPos = new float[] { 290f,258f,226f,194f,162f};//{290f,240f,190f,140f,90f};		//{ 0,-75f,-150f,-225f,-300f};	
	private int playerCount = 0;

	private int tempRank = 0;

	public GameObject[] tweenPos;

	public GameObject ReplayButton;
	public RectTransform RightsideContents;
	public bool ReturnWaitState ()
	{
		return WaitForOthersPanel.activeSelf;
	}

	public void ReplayCountdown (int time)
	{
		/*if (UserProfile.Tickets < Multiplayer.entryTickets)
		{
			replayCountdown.text = "NOT ENOUGH COINS FOR REPLAY";
			return;
		}
		if (time > 1)
		{
			replayCountdown.text = "REPLAY IN " + time;
		}
		else
		{
			replayCountdown.text = "REPLAY DISABLED";
		}*/
	}

	public void ReplayMatch ()
	{
		ServerManager.Instance.RematchRoom ();
		//GroundScriptHandler.Instance.ShowLoadingScreen ();
	}

	public void UpdateMultiplayerBallsLeft ()
	{
		int ballsLeft = -1;
		if (Multiplayer.overs == 2)
		{
			if (CONTROLLER.currentMatchBalls > 8)
				ballsLeft = 12 - CONTROLLER.currentMatchBalls;
		} 
		else if (Multiplayer.overs == 5)
		{
			if (CONTROLLER.currentMatchBalls > 23)
				ballsLeft = 30 - CONTROLLER.currentMatchBalls;
		}

		if (ballsLeft != -1 && !MultiplayerGameOverPage.activeSelf)
        {
			multiplayerFinalBallCountdown.SetActive (true);
			if (ballsLeft == 1)
				ballsLeftInMultiplayer.text = ballsLeft + " BALL LEFT";
			else
				ballsLeftInMultiplayer.text = ballsLeft + " BALLS LEFT";
		}
		else
		{
			multiplayerFinalBallCountdown.SetActive (false );
		}
	}

	public void ShowWait ()
	{
		//ID - 0010572 - FIXED ON 09-02-2016 - KARTHIK - 0.25
//		SixDistanceScript.Instance.HideSixDistance ();
		WaitForOthersPanel.SetActive (true);
		if(!AnimationScreen.instance.IsAnimationPlaying())
			GroundController.instance.ResetAll_BatMP();
	}

	public void HideWait ()
	{
		WaitForOthersPanel.SetActive (false);
	}

	public void ShowWicketsWait ()
	{
		WaitForOthersWicketsOverPanel.SetActive (true);
		//GroundController.instance.ResetAll_BatMP();
	}

	public void HideWicketsWait ()
	{
		WaitForOthersWicketsOverPanel.SetActive (false);
	}

	private void SetAnimatorVal ()
	{
		/*if(!SettingsPrefs.selectedGender)
			_UICharAnimator = _UICharacter[0].GetComponent<Animation> () as Animation;
		else
			_UICharAnimator = _UICharacter[1].GetComponent<Animation> () as Animation;*/
	}

	private int plyrCount = 0;
	private int[] rnk;
	private string[] uname;
	private string[] scr;
    private string[] cp;
    private string[] pidwithCcode;


    public void SetMultiplayerGameOverTexts ()
	{
		plyrCount = Multiplayer.playerCount;
		rnk = new int[plyrCount];
		uname = new string[plyrCount];
		scr = new string[plyrCount];
        cp = new string[plyrCount];
        pidwithCcode = new string[plyrCount];

        for (int i = 0; i < plyrCount; i++) 
		{
			rnk[i] = Multiplayer.playerScores [i].Rank;
			uname[i] = CONTROLLER.UppercaseFirst( Multiplayer.playerScores [i].Username);            
			scr[i] = Multiplayer.playerScores [i].Score;
            cp[i] = Multiplayer.playerScores[i].Cp;
            pidwithCcode[i] = Multiplayer.playerScores[i].playerIdwithCountryCode;

        }
    }

	public void ShowMultiPlayerGameOver (int bonusEarningsPoints,int rewardedCpValue)
	{
		PreviewScreen.instance.Hide(true);
		BlastEffect.instance.playAnimation(GameoverCanvas, true);

		CONTROLLER.CurrentPage = "";
		InGameHolder.SetActive(false);
        _ScoreHandler_Multiplayer.SetActive (false);
		EmoticonPanel.SetActive (false);
		WaitForOthersPanel.SetActive (false);
		WaitForOthersWicketsOverPanel.SetActive (false);
		multiplayerFinalBallCountdown.SetActive (false);
		postionGO.SetActive (false); 
		BatsmanInfo.instance.HideMe (); 
		MultiplayerGameOverPage.SetActive (true);

		if (Multiplayer.roomType == 0)
			ReplayButton.SetActive(true);
		else
			ReplayButton.SetActive(false);
        
		int tempFlag = 0;
        int myrank = 1;

        for (int i = 0; i < 5; i++) 
		{
			if (i < plyrCount) 
			{
				multiplayerGameOverRank[i].text = rnk[i].ToString ();
				multiplayerGameOverUsername[i].text = uname[i];
				multiplayerGameOverScore[i].text = scr[i];

				if (pidwithCcode[i] == CONTROLLER.BattingMultiplayerCountryCode + CONTROLLER.M_USERID)      //if (uname[i] == CONTROLLER.ud_UserName)
                {
					multiplayerGameOverBG[i].enabled = true;
					multiplayerGameOverShine[i].SetActive(true);
					//multiplayerGameOverBG [i].sprite = multiplayerGameOverUserBG[0];
					myrank = rnk[i];
                    if (rnk [i] == 1) 
					{
						multiplayerPosSuperTxt = "ST";
					}
					else 
					{
						if (Multiplayer.playerScores [i].Rank == 2)
							multiplayerPosSuperTxt = "ND";
						else if (Multiplayer.playerScores [i].Rank == 3)
							multiplayerPosSuperTxt = "RD";
						else
							multiplayerPosSuperTxt = "TH";
					}

					multiplayerPositionTxt.text = (rnk [i]).ToString () ;
					multiplayerPositionTxt2.text = multiplayerPosSuperTxt;

					if (plyrCount == 1) 
					{
						if (tempFlag == 0)
							contentToShare = CONTROLLER.UserName + " came " + rnk [i] + multiplayerPosSuperTxt + " in " + CONTROLLER.AppName +" - " + (plyrCount + 1) + " player Multiplayer match.";
						tempFlag = 1;
					}
					else 
					{
						if (tempFlag == 0)
							contentToShare = CONTROLLER.UserName + " came " + rnk [i] + multiplayerPosSuperTxt + " in "+CONTROLLER.AppName +" - " + plyrCount + " player Multiplayer match.";
						tempFlag = 1;
					}

					//multiplayerGameOverRank [i].color = Color.white;
					//multiplayerGameOverUsername [i].color = Color.white;
					//multiplayerGameOverScore [i].color = Color.white;

				}
                else
                {
                    multiplayerGameOverBG[i].enabled = false;
                    multiplayerGameOverShine[i].SetActive(false);
					//multiplayerGameOverBG [i].sprite = multiplayerGameOverUserBG[1];
					//multiplayerGameOverRank [i].color = Color.black;
					//multiplayerGameOverUsername [i].color = Color.black;
					//multiplayerGameOverScore [i].color = Color.black;
				}

                int tVal = CONTROLLER.BatMPRewards_CP[Multiplayer.GetWinningCostIndex(PlayerPrefs.GetInt(CONTROLLER.BatMpCpSavedName, Multiplayer.playerCount), (rnk[i] - 1))];
                if (tVal >= 0)
                {
                    //GameoverCPText[i].text = "+" + tVal;
                }
                else
                {                 
                    //GameoverCPText[i].text = "" + tVal;
                }
            } 
			else
			{
				multiplayerGameOverBG [i].gameObject.SetActive (false); 
				multiplayerGameOverShine [i].SetActive (false);
				multiplayerGameOverRank[i].text = "";
				multiplayerGameOverUsername[i].text = "";
				multiplayerGameOverScore[i].text = "";
			}
		}

        RightsideContents.anchoredPosition = new Vector2(RightsideContents.anchoredPosition.x, -60f * myrank);
        ScoreCounter.instance.SetValues(CONTROLLER.totalPoints, bonusEarningsPoints, CONTROLLER.totalPoints + bonusEarningsPoints);


        multiplayerEarningsPoints.text =""+CONTROLLER.totalPoints ; 	//match points
		multiplayerBonusPoints.text=""+bonusEarningsPoints;			//bonus points


		StartCoroutine (CricMinisWebRequest.instance.sendPointstoLeaderboard (CONTROLLER.totalPoints + bonusEarningsPoints) );

        if (!CONTROLLER.isBatMpContestRunning || (Multiplayer.roomType == 0 && CONTROLLER.isBatMpContestRunning && AdIntegrate.instance.isRegisteredContestUser))
        {
            CONTROLLER.CricketPoints += rewardedCpValue;
            if (CONTROLLER.CricketPoints < 0)
                CONTROLLER.CricketPoints = 0;
        }
        PlayerPrefsManager.SaveCoins();


        if (!CONTROLLER.isBatMpContestRunning || (Multiplayer.roomType == 0 && CONTROLLER.isBatMpContestRunning && AdIntegrate.instance.isRegisteredContestUser))
        {
        }
        else
        {
            PlayerPrefs.DeleteKey(CONTROLLER.BatMpCpSavedName);
        }
        ServerManager.Instance.Disconnect ();


		if (AdIntegrate.instance != null)
			AdIntegrate.instance.ShowBannerAd ();

		AudioPlayer.instance.PlayGameSnd("levelcompleted");




    }

    public void ShareMultiplayer ()
	{
		AudioPlayer.instance.PlayButtonSnd();
		string subject = CONTROLLER.AppName;
		string body;
		body = contentToShare + "\n Can you do better ?\n Download Now "+CONTROLLER .AppLink;
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
        /*body = "Hey, let's play this fun and exciting multiplayer cricket game! Try it out: http://batattackcricket.com/";*/
		ShareThisGame.sendTextWithOutPath (body);
#endif
	}

	public void GoToMainMenu (int idx)	//0=home 1=replay
	{
		AudioPlayer.instance.PlayButtonSnd();

		if (AdIntegrate.instance != null && CONTROLLER.serverConfig.IGQ == 1 &&  CONTROLLER.CanShowAdtoNewUser_Inter == 1)
		{
            AdIntegrate.instance.HideAd();
            AdIntegrate.instance.ShowInterestialAd();
		}
		
		if(idx == 0)
			ServerManager.Instance.SetReplayDataNull();
        else    //saving data for the replay
            ServerManager.Instance.AssignDataForReplay();
        
		Multiplayer.roomType = -1;

        ServerManager.Instance.ExitRoom ();
		GameModel .instance .ResetCurrentMatchDetails ();
		GameModel .instance .ResetVariables ();
		GameModel .instance .ResetAllLocalVariables ();
        if(GroundScriptHandler.Instance!=null)
            GroundScriptHandler.Instance.ShowLoadingScreen ();
		if (BlastEffect.instance != null)
			BlastEffect.instance.StopAnimation();
        if (AudioPlayer.instance != null)
        {
            AudioPlayer.instance.ToggleInGameSounds(false);
            AudioPlayer.instance.PlayOrStop_BGM(true);
        }
        ManageScene.LoadScene (Scenes.MainMenu);
	}

	public void ReplayButtonClick()
	{
        AudioPlayer.instance.PlayButtonSnd();

        if (AdIntegrate.instance != null && CONTROLLER.serverConfig.IGR == 1 && CONTROLLER.CanShowAdtoNewUser_Inter == 1)
        {
            AdIntegrate.instance.HideAd();
            AdIntegrate.instance.ShowInterestialAd();
        }

	}

	public void SortAndRankScores ()
	{
		MultiplayerScore[] tempScoreArray = new MultiplayerScore[Multiplayer.playerCount];
		MultiplayerScore temp = tempScoreArray [0];

		for (int i = 0; i < Multiplayer.playerCount; i++)
		{
			tempScoreArray [i] = Multiplayer.playerScores [i];
		}

		for (int i = 0; i < tempScoreArray.Length; i++)
		{
			for (int j = i + 1; j < tempScoreArray.Length; j++)
			{
				if (int.Parse (tempScoreArray [i].Score) < int.Parse (tempScoreArray [j].Score))
				{
					temp = tempScoreArray [i];
					tempScoreArray [i] = tempScoreArray [j];
					tempScoreArray [j] = temp;
				}
			}
		}

		for (int i = 0; i < tempScoreArray.Length; i++)
		{
			for (int j = i + 1; j < tempScoreArray.Length; j++)
			{
				if (int.Parse (tempScoreArray [i].Score) == int.Parse (tempScoreArray [j].Score))
				{
					if (tempScoreArray [i].Wickets > tempScoreArray [j].Wickets)
					{
						temp = tempScoreArray [i];
						tempScoreArray [i] = tempScoreArray [j];
						tempScoreArray [j] = temp;
					}
				}
			}
		}

		//		tempScoreArrayForAnimation = new int[Multiplayer.playerCount];
		//		//tempScoreArrayForAnimation = tempScoreArray;
		//		for (int i=0;i<Multiplayer.playerCount;i++)
		//		{
		//			tempScoreArrayForAnimation[i] = tempScoreArray [i].Rank;
		//		}

		int rank = 1;
		int rankPos = 1;

		for (int i = 0; i < tempScoreArray.Length; i++)
		{
			if (i < tempScoreArray.Length - 1)
			{
				if (tempScoreArray [i].Score == tempScoreArray [i + 1].Score && tempScoreArray [i].Wickets == tempScoreArray [i + 1].Wickets)
				{
					tempScoreArray [i].Rank = rank;
					//rank++; //added to check

					tempScoreArray [i].RankPos = rankPos;
					rankPos++;
				} else
				{
					tempScoreArray [i].Rank = rank;
					rank++;

					tempScoreArray [i].RankPos = rankPos;
					rankPos++;
				}
			} 
			else
			{
				if (i != 0 && tempScoreArray [i-1].Score == tempScoreArray [i].Score && tempScoreArray [i-1].Wickets == tempScoreArray [i].Wickets)
				{
					tempScoreArray [i].Rank = rank;
					//rank++; //added to check

					tempScoreArray [i].RankPos = rankPos;
					rankPos++;
				}
				else
				{
					tempScoreArray [i].Rank = rank;
					rank++;

					tempScoreArray [i].RankPos = rankPos;
					rankPos++;
				}
			}
		}
		/*//Old Code for Score and Wickets to be same.
		int rank = 1;

		foreach (MultiplayerScore temp1Score in tempScoreArray)
		{
			foreach (MultiplayerScore temp2Score in Multiplayer.playerScores)
			{
				if (temp1Score.PlayerId == temp2Score.PlayerId)
				{
					temp2Score.Rank = rank;
					rank++;
				}
			}
		}*/

		UpdateMultiplayerScores ();
	}

	public void UpdateMultiplayerScores ()
	{
		for (int i = 0; i < 5; i++)
		{
			if (i < Multiplayer.playerCount) {
				multiplayerRank [i].text = Multiplayer.playerScores [i].Rank.ToString ();
				multiplayerUsername [i].text = CONTROLLER.UppercaseFirst(Multiplayer.playerScores [i].Username);
				multiplayerScore [i].text = Multiplayer.playerScores [i].Score.ToString () + "/" + Multiplayer.playerScores [i].Wickets;
				multiplayerLastScore [i].text = Multiplayer.playerScores [i].LastBallScore.ToString ();
				MultiplayerPlayerScoreBG [i].SetActive (true);

                if (Multiplayer.playerScores[i].playerIdwithCountryCode == CONTROLLER.BattingMultiplayerCountryCode + CONTROLLER.M_USERID)          //(Multiplayer.playerScores[i].PlayerId == int.Parse(CONTROLLER.ud_uID))
                //if (Multiplayer.playerScores[i].PlayerId == CONTROLLER.M_USERID)
				{
					if(Multiplayer.playerScores[i].Rank == 1)
					{
						numberTxt.text =/*Multiplayer.playerScores[i].Rank*/1.ToString ();
						positionTxt.text = "ST PLACE";
					}
					else if(Multiplayer.playerScores[i].Rank == 2)
					{
						numberTxt.text = /*Multiplayer.playerScores[i].Rank*/2.ToString ();
						positionTxt.text = "ND PLACE";
					}
					else if(Multiplayer.playerScores[i].Rank == 3)
					{
						numberTxt.text = /*Multiplayer.playerScores[i].Rank*/3.ToString ();
						positionTxt.text = "RD PLACE";
					}
					else
					{
						numberTxt.text = Multiplayer.playerScores[i].Rank.ToString ();
						positionTxt.text = "TH PLACE";
					}

					//multiplayerUsername [i].color = Color.red;// multiplayerRank [i].color;
					//					multiplayerUsername [i].effectStyle = UILabel.Effect.Outline;
					multiplayerBG[i].sprite = userBG;
				}
				else
				{
					multiplayerBG[i].sprite = otherBG;

					//multiplayerUsername [i].color =  Color.white;
					//					multiplayerUsername [i].effectStyle = UILabel.Effect.None;
				}

			} else {
				multiplayerRank [i].text = "";
				multiplayerUsername [i].text = "";
				multiplayerScore [i].text = "";
				multiplayerLastScore [i].text = "";
				MultiplayerPlayerScoreBG [i].SetActive (false);
			}
		}
		if(playerCount != Multiplayer.playerCount)
		{
			//MultiplayerTable.repositionNow = true;
			playerCount = Multiplayer.playerCount;
		}
	}


	public void AnimateBoard ()
	{
		for (int i = 0; i < Multiplayer.playerCount; i++)
		{
			tempRank = Multiplayer.playerScores[i].RankPos;
			iTween.MoveTo(tweenPos [i], iTween.Hash("position", new Vector3 (tweenPos[i].gameObject.transform.localPosition.x, yPos[tempRank - 1],tweenPos[i].gameObject.transform.localPosition.z), "islocal", true, "time", 1));
		}
	}

	public void OnCompleteAnimateBoard (GameObject go)
	{
		/*		
		int i = int.Parse(go.name.Split ("_"[0])[2]);
		i--;
		tweenPos[i].delay = 1.0f;
		tweenPos[i].SetStartToCurrentValue ();
		tweenPos[i].ResetToBeginning ();
		//		for (int i = 0; i < Multiplayer.playerCount; i++)
		//		{
		////			tweenPos[i].delay = 2.0f;
		//			tweenPos[i].SetStartToCurrentValue ();
		//			tweenPos[i].ResetToBeginning ();
		//		}*/
	}

	protected void Awake ()
	{
		instance = this;
		playerCount = Multiplayer.playerCount;
		this.Hide (true);
		/*	CONTROLLER.ballUpdate[0] = "";
		CONTROLLER.ballUpdate[1] = "";
		CONTROLLER.ballUpdate[2] = "";
		CONTROLLER.ballUpdate[3] = "";
		CONTROLLER.ballUpdate[4] = "";
		CONTROLLER.ballUpdate[5] = "";*/
	}

	public void pauseGame () //IUIObject Control
	{
		//GameModel.instance.GamePaused(true);
	}

	public void UpdateScoreCard ()
	{
		CurrentScore.text = ("" +GameModel.ScoreStr).ToUpper ();
		BallsFaced.text = ("" + GameModel.OversStr).ToUpper ();
			
		MultiplayerTotalOversLabel.text = "TOTAL OVERS CHOSEN: "+Multiplayer.overs;
	}

	public IEnumerator NewOver ()
	{
		yield return new WaitForSeconds (0.01f);
		CONTROLLER.ballUpdate[0] = "";
		CONTROLLER.ballUpdate[1] = "";
		CONTROLLER.ballUpdate[2] = "";
		CONTROLLER.ballUpdate[3] = "";
		CONTROLLER.ballUpdate[4] = "";
		CONTROLLER.ballUpdate[5] = "";
	}

	public void Hide (bool boolean)
	{
		float DefaultRatio = CONTROLLER.DefaultWidth / CONTROLLER.DefaultHeight;
		float SreenWidth = Screen.width;
		float SreenHeight = Screen.height;
		float ScreenRatio = SreenWidth / SreenHeight;
		CONTROLLER.xOffSet = ((DefaultRatio - ScreenRatio) * (CONTROLLER.DefaultHeight/2));
		PreviewScreen.instance.Hide (boolean);

		if(boolean == true)
		{
			_ScoreHandler_Multiplayer.SetActive (false);
			WaitForOthersPanel.SetActive (false);
			WaitForOthersWicketsOverPanel.SetActive (false);
			postionGO.SetActive (false);  
			MultiplayerGameOverPage.SetActive (false);
			EmoticonPanel.SetActive (false);
			multiplayerFinalBallCountdown.SetActive (false);
			Scoreboard.instance.ProfileDatas.SetActive(false);
		}
		else
		{
			_ScoreHandler_Multiplayer.SetActive (true);
			postionGO.SetActive (true); 
			EmoticonPanel.SetActive (true);
			//Scoreboard.instance.ProfileDatas.SetActive(true);
		}
	}

	public void FadeThisObject (bool state)
	{
		/*if (state)
		{
			FadeObject.PlayForward ();
		}
		else
		{
			FadeObject.PlayReverse ();
		}*/
	}



	public void watchVideo()
	{
		AudioPlayer.instance.PlayButtonSnd();
		if (AdIntegrate.instance != null)
		{
			AdIntegrate.instance.GetTicketRewardedVideoHelper();
			/*
			if (AdIntegrate.instance.checkTheInternet())
			{
				if (AdIntegrate.instance.isRewardedReadyToPlay())
				{
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

    public IEnumerator SyncCPpoints(int totalplayers, int rank)
    {

        if (!AdIntegrate.instance.checkTheInternet() || !CONTROLLER.IsUserLoggedIn())
        {            
            // No Internet
        }
        else
        {
            WWWForm form = new WWWForm();
            WWW download;
            
            form.AddField("action", "CPUpdate");
            form.AddField("user_id", CONTROLLER.M_USERID);
            form.AddField("players", totalplayers);
            form.AddField("rank", rank);
            form.AddField("state", "win");
                        
            form.AddField("bv", CONTROLLER.CURRENT_VERSION);
            form.AddField("platform", CONTROLLER.TargetPlatform);
            form.AddField("deviceid", CONTROLLER.DeviceID);


            download = new WWW(CONTROLLER.BASE_URL, form);
            yield return download;

			if (!string.IsNullOrEmpty(download.error))
            {
            }
            else
            {
                SimpleJSON.JSONNode node = SimpleJSON.JSONNode.Parse(download.text);

                //if ("" + node["CPUpdate"]["status"] == "1")
                //{
                //    PlayerPrefs.DeleteKey(CONTROLLER.BatMpCpSavedName);
                //    CONTROLLER.CricketPoints = node["CPUpdate"]["cp"].AsInt;
                //    // MyCpPoints.text = CONTROLLER.CricketPoints.ToString();
                //    PlayerPrefsManager.SaveCoins();
                //}
            }
        }
    }

}
