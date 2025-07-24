using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameOverScreen : Singleton<GameOverScreen>
{
	public Canvas canvas;

	public GameObject GameOver, settings, instructionsPage;
	public GameObject continueButton,Glow_continueBtn, replayButton,Glow_replay;
	public GameObject HomeRV,HomeNormal;
	private string [] levelNames = new string[] { "ROOKIE", "SEMI PRO", "PROFESSIONAL", "VETERAN","CHAMPION", "LEGEND" };
	public Text ResultText  ;
	public Text MenuTitle;
	public Text MyPoints;
	public bool  pointsSentFromGOScreen= false;
	public GameObject share;
	private bool  lastChaseCompleted = false;

	public Image starOutline;
	protected void  Awake ()
	{
		CONTROLLER.CurrentPage = "";
		HideThis ();
	}
	protected void  Start ()
	{	
		settings.SetActive (false);
		instructionsPage.SetActive (false);
		BattingScoreCard.instance.HideMe ();
		ShowMe ();
		ValidateText ();
		StartCoroutine (CricMinisWebRequest.instance.sendPointstoLeaderboard (CONTROLLER.TempPoint) );

	}
	public void ShowInstertialAdLoadScreen(int index)
	{
		AudioPlayer.instance.PlayButtonSnd();

        if ( (CONTROLLER.serverConfig.IGQ == 0 && index == 0) || (CONTROLLER.serverConfig.IGR == 0 && index == 3) || (CONTROLLER.serverConfig.IGN == 0 && index == 4))
			menuClicked(index);
		else
		{
			InterstialAdLoadingScript.instance.ShowMe(index);
		}
	}

    public void menuClicked (int index)
	{
		CONTROLLER.NewInnings = true;
		//if (CONTROLLER.sndController != null) 
		//{
		//	CONTROLLER.sndController.PlayButtonSnd ();
		//}
		if (index == 0)	//home
		{
			AdIntegrate.instance.HideAd();
			StartCoroutine(GameModel.instance.GameQuitted());
		}
		else if (index == 3)	//restart
		{
			if (InGameBg.instance != null)
			{
				InGameBg.instance.HideMe();
			}
            AudioPlayer.instance.PlayTheIntroSound();
            CONTROLLER.CTLevelId = CONTROLLER.CTCurrentPlayingMainLevel;
			CONTROLLER.CTLevelCompleted = CONTROLLER.CTCurrentPlayingMainLevel;
			CONTROLLER.CTSubLevelCompleted = GameModel.instance.GetCurrentSubLevel();
			CONTROLLER.NewInnings = true;
			GameModel.instance.ReStartGame();
            HideMe();
		}
		else if (index == 4)
		{
			AudioPlayer.instance.PlayTheIntroSound();
			if (InGameBg.instance != null)
			{
				InGameBg.instance.HideMe();
			}
			GameModel.instance.ShowUIAnimation();
			GameModel.instance.ResetCurrentMatchDetails();
			BattingScoreCard.instance.ResetPlayerImages();
			HideMe();
		}

	}

	private void setReplayButDisabled()
	{
		//disable next button in watch video case
		if(PlayerPrefs .HasKey ("CTRVlevID"))
		{
			if(CONTROLLER .CTSubLevelId ==PlayerPrefs.GetInt ("CTRVlevID") )
			{
				replayButton.SetActive (false);
				HomeRV.SetActive(true);
				HomeNormal.SetActive(false);
			}
			PlayerPrefs.DeleteKey ("CTRVlevID");
		}
	}

	private void  ValidateText ()
	{
		bool canShowBlasteffect = true;
		int  randNumber ;
		if (CONTROLLER.gameMode == "chasetarget")
		{
			if (CONTROLLER.currentMatchScores >= CONTROLLER.TargetToChase)
			{
				canShowBlasteffect = true;
				AudioPlayer.instance.PlayGameSnd("levelcompleted");
				MenuTitle.text = "LEVEL COMPLETED";
				lastChaseCompleted = true;
				randNumber = Random.Range (0,6);
				if (randNumber == 0)
				{
					ResultText.text = "That was a Successful chase.";
				}
				else if (randNumber == 1)
				{
					ResultText.text = "You have successfully chased the target.";
				}
				else if (randNumber == 2)
				{
					ResultText.text = "Congratulations! You have \nsuccessfully crossed your target!";
				}
				else if (randNumber == 3)
				{
					ResultText.text = "That chase was a Winner!";
				}
				else if (randNumber == 4)
				{
					ResultText.text = "Cool! What a Chase!";
				}
				else if (randNumber == 5)
				{
					ResultText.text = "That chase was stunning!";
				}
				share.SetActive (true);
				continueButton.SetActive (true);
				SetButtonAnim(0);
				//won

			}
			else if (CONTROLLER.currentMatchScores == CONTROLLER.TargetToChase - 1)
			{
				canShowBlasteffect = false;
				AudioPlayer.instance.PlayGameSnd("levelfailed");
				MenuTitle.text = "LEVEL FAILED";
				randNumber = Random.Range (0,3);
				if (randNumber == 0)
				{
					ResultText.text = "Its a Tie. Need to go a step further \nto achieve glory. Try again.";
				}
				else if (randNumber == 1)
				{
					ResultText.text = "Pretty close, But not yet there.\n Try again.";
				}
				else if (randNumber == 2)
				{
					ResultText.text = "Evenly matched.\n Try again to beat em.";
				}
				share.SetActive (false);
				continueButton.SetActive (false);
				SetButtonAnim(1);
				setReplayButDisabled();
				//tie
			}
			else
			{
				canShowBlasteffect = false;
				AudioPlayer.instance.PlayGameSnd("levelfailed");
				MenuTitle.text = "LEVEL FAILED";
				//LionStatus.SetToggleState ("fail");
				randNumber = Random.Range (0,3);
				if (randNumber == 0)
				{
					ResultText.text = "Oops... Try again?";
				}
				else if (randNumber == 1)
				{
					ResultText.text = "That was an unsuccessful chase.\n Try again?";
				}
				else if (randNumber == 2)
				{
					ResultText.text = "Tough luck! Try again?";
				}
				share.SetActive (false);
				continueButton.SetActive (false);
				SetButtonAnim(1);
				setReplayButDisabled();
			}
		}
		else
		{
			canShowBlasteffect = true;
			AudioPlayer.instance.PlayGameSnd("levelcompleted");
			share.SetActive (true);
			MenuTitle.text = "GAME OVER";
			int  RunRate  = (CONTROLLER.currentMatchScores / CONTROLLER.currentMatchBalls) * 6;
			if (RunRate <= 4)
			{
				randNumber = Random.Range (0,3);
				if (randNumber == 0)
				{
					ResultText.text = "The ball still looks new!";
				}
				else if (randNumber == 1)
				{
					ResultText.text = "Get your game face on and \nsmack that ball!";
				}
				else if (randNumber == 2)
				{
					ResultText.text = "Next time make sure they \nrun out of balls.";
				}
			}
			else if (RunRate > 4 && RunRate <=5)
			{
				randNumber = Random.Range (0,4);
				if (randNumber == 0)
				{
					ResultText.text = "This ain't a test match, \nhit like you mean it!";
				}
				else if (randNumber == 1)
				{
					ResultText.text = "Sixes + Fours = Better Run Rate!";
				}
				else if (randNumber == 2)
				{
					ResultText.text = "Hit the ball hard.\nObserve what happens next.";
				}
				else if (randNumber == 3)
				{
					ResultText.text = "Relax and just whack it!";
				}
			}
			else if (RunRate > 5 && RunRate < 6)
			{
				randNumber = Random.Range (0,5);
				if (randNumber == 0)
				{
					ResultText.text = "Every war has losses,\n just be cool & keep fighting.";
				}
				else if (randNumber == 1)
				{
					ResultText.text = "Every ball is your last,\nso kiss it goodbye.";
				}
				else if (randNumber == 2)
				{
					ResultText.text = "Never hesitate, never think again, \njust play on.";
				}
				else if (randNumber == 3)
				{
					ResultText.text = "Great score\n but you could have got better.";
				}
				else if (randNumber == 4)
				{
					ResultText.text = "Learning is one thing, \nexperience is a different ball game.";
				}
			}
			else if (RunRate >= 6)
			{
				randNumber = Random.Range (0,3);
				if (randNumber == 0)
				{
					ResultText.text = "Great! Keep it up and you'll \ndefinitely head the leaderboard!";
				}
				else if (randNumber == 1)
				{
					ResultText.text = "Wow! You have the makings of a king!";
				}
				else if (randNumber == 2)
				{
					ResultText.text = "A few more sixes and fours \nand you will be on your way to the top!";
				}
			}
		}
		
		if (CONTROLLER.gameMode == "slogover")
		{
			continueButton.SetActive (false);
			SetButtonAnim(1);
		}

		if (CONTROLLER.gameMode == "chasetarget")
		{
			int  _currentLevel  = GameModel.instance.GetCurrentSubLevel ();

			if((CONTROLLER.CTSubLevelCompleted == 5 && CONTROLLER.CTLevelCompleted == 5)) //((CONTROLLER.CTSubLevelCompleted == 5) || (CONTROLLER.CTSubLevelCompleted == 5 && CONTROLLER.CTLevelCompleted == 5) || (CONTROLLER.CTSubLevelCompleted == 4 && CONTROLLER.CTLevelCompleted == 5 && CONTROLLER.MainLevelCompletedArray[CONTROLLER.CTLevelCompleted] == 1))
			{
				MenuTitle.text = "CONGRATULATIONS!";// "CONGRATULATIONS!";
				ResultText.text = "You have cleared all the chases.\n You are truly the King!";
				continueButton.SetActive (false);
				SetButtonAnim(1);
				if (lastChaseCompleted == false)
				{
					MenuTitle.text = "LEVEL FAILED";
					//LionStatus.SetToggleState ("fail");
					ShowResult ();
				}
			}
		}
		
		MyPoints.text = ""+CONTROLLER.TempPoint;
		BlastEffect.instance.playAnimation(canvas,canShowBlasteffect);
	}

	private void  ShowResult ()
	{
		int  randNumber ;
		if (CONTROLLER.currentMatchScores == CONTROLLER.TargetToChase - 1)
		{
			randNumber = Random.Range (0,3);
			if (randNumber == 0)
			{
				ResultText.text = "Its a Tie. Need to go a step further\n to achieve glory. Try again.";
			}
			else if (randNumber == 1)
			{
				ResultText.text = "Pretty close,\n But not yet there. Try again.";
			}
			else if (randNumber == 2)
			{
				ResultText.text = "Evenly matched. \nTry again to beat.";
			}
			//tie
		}
		else
		{
			randNumber = Random.Range (0,3);
			if (randNumber == 0)
			{
				ResultText.text = "Oops... Try again?";
			}
			else if (randNumber == 1)
			{
				ResultText.text = "That was an unsuccessful chase.\n Try again?";
			}
			else if (randNumber == 2)
			{
				ResultText.text = "Tough luck. \nTry again?";
			}
			//loss
		}
	}

	private void  ShowDisplayMsg (string  errorStr , int action )
	{
		GameObject prefabGO ;
		GameObject tempGO ;

		prefabGO = Resources.Load ("Prefabs/DisplayMsg")as GameObject ;
		tempGO = Instantiate (prefabGO)as GameObject ;
		tempGO.name = "DisplayMsg";
		tempGO.transform.localPosition = new Vector3 (0f,0f,0.4f);

		GameObject  OkBtn = GameObject.Find ("DisplayMsg/OkButton");
		OkBtn.transform.localPosition=new Vector3 (OkBtn.transform.localPosition.x,OkBtn.transform.localPosition.y,-10);
	}


	public void  HideButtons ()
	{
		//LoginHolderGO.transform.localPosition =new Vector3 (LoginHolderGO.transform.localPosition.x,LoginHolderGO.transform.localPosition.y,-10f);
		//RetryButtonHolderGO.transform.localPosition=new Vector3 (RetryButtonHolderGO.transform.localPosition.x,RetryButtonHolderGO.transform.localPosition.y,-10f);
		//AppLogo.transform.position=new Vector3 (0f,AppLogo.transform.position.y,AppLogo.transform.position.z) ;
	}

	public void  ShowMe ()
	{
		GameOver.SetActive (true);
		starOutline.fillAmount = 0;
		Invoke("starAnimation", 0.5f);
	}

	void starAnimation()
	{
		//StartCoroutine(DoTweenController.barFillerValue(0, CONTROLLER.TempPoint, 2f, 0, MyPoints));
		starOutline.DOFillAmount(1, 2f).SetLoops(-1, LoopType.Restart).SetUpdate(true);
	}

	private void  HideMe ()
	{
		if (BlastEffect.instance != null)
			BlastEffect.instance.StopAnimation();

		Destroy (this.gameObject);
		Resources.UnloadUnusedAssets ();
	}
	private void  HideThis ()
	{
		GameOver.SetActive (false);
	}
	public ModeInstructionLogoAssigner instuctionAssigner;
	public void instructions() 
	{
		AudioPlayer.instance.PlayButtonSnd();
		GameOver.SetActive (false);
		instructionsPage.SetActive (true);
		CONTROLLER.CurrentPage = "GOinstructions";
		instuctionAssigner.updateInstructionText();
	}
	public void showSettings() {
		CONTROLLER.CurrentPage = "GOsettings";
		settings.SetActive (true);
	}
	public void hideInstructions() {
		GameOver.SetActive (true);
		instructionsPage.SetActive (false);
	}


	public void ShareChallenge ()
	{
		AudioPlayer.instance.PlayButtonSnd();
		string subject = CONTROLLER.AppName;
		string body ="";

		if(CONTROLLER.gameMode =="slogover")
			body = CONTROLLER.UserName + " completed Super Slog  with " + (CONTROLLER.totalPoints) + " points in "+CONTROLLER.AppName +" - " + " Can you do better?\nDownload Now! " + CONTROLLER.AppLink; 
		else			
            body = CONTROLLER.UserName + " completed level " + (CONTROLLER.CTSubLevelId + 1) + " in " + levelNames[CONTROLLER.CTCurrentPlayingMainLevel] + " of Super Chase in "+CONTROLLER.AppName+" - " + ". Can you beat it?\nDownload Now " + CONTROLLER.AppLink;

		//execute the below lines if being run on a Android device
		#if UNITY_ANDROID
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
		ShareThisGame.sendTextWithOutPath (body);
		#endif
	}

	private void SetButtonAnim(int idx)//0-continue 1-replay
	{
		if(idx==1)
		{
			//replayButton.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(100f, 100f);
			//replayButton.GetComponentInChildren<Text>().fontSize =27;
			//continueButton.GetComponentInChildren<Text>().fontSize =22;
			Glow_replay.SetActive(true);
		}
		else
		{
			//continueButton.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(100f, 100f);
			Glow_continueBtn.SetActive(true);
			//replayButton.GetComponentInChildren<Text>().fontSize = 22;
			//continueButton.GetComponentInChildren<Text>().fontSize = 27;
		}
	}
}
