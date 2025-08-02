using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System ;
using System .Linq ;


public class SuperOverResult : Singleton<SuperOverResult> 
{
	public GameObject passScreen, failScreen, background;

	public GameObject PassscreenNext, Glow_pNext, PassScreenReplay,Glow_pReplay;
	public GameObject PassscreenHome_normal, passscreenHome_RV, passscreenShare_normal, passscreenShare_RV;

	public GameObject Failscreen_Replay, Glow_fReplay;

	private BowlingSpot bowlingSpotScript ;
	public Text levelId;
	public GameObject settings, instructionsPage;

	public Canvas canvas;
	protected void  Awake ()
	{
		bowlingSpotScript = GameObject.Find("BowlingSpot").GetComponent<BowlingSpot>();
		//HideMe ();
		/*if(CONTROLLER.isFreeVersion == false)
		{
			FullVersionBtn.Hide (true);
		}*/

		//if(AdIntegrate.instance != null)
		//{
		//	AdIntegrate.instance.ShowBannerAd ();
		//	AdIntegrate.instance.ShowInterestialAd (); 
		//}
		CONTROLLER.CurrentPage = "soresultpage";
	}

	protected void  Start ()
	{
		if (CONTROLLER.LevelId == 17 ) 
		{
			PassscreenNext.SetActive (false);
			SetButtonAnim(1);
		}
		instructionsPage.SetActive (false);
		settings.SetActive (false);
		passScreen.SetActive (false);
		failScreen.SetActive (false);
		ValidateThisLevel ();
	
		//disable next button in watch video case
		if(PlayerPrefs .HasKey ("SoRVlevID"))
		{
			int tID = PlayerPrefs.GetInt ("SoRVlevID");
			PlayerPrefs.DeleteKey ("SoRVlevID");

			if(CONTROLLER .LevelId ==tID )
			{				
				PassscreenNext.SetActive (false); 
				Failscreen_Replay.SetActive (false);
                PassScreenReplay.SetActive(false);

				passscreenHome_RV.SetActive(true);
				passscreenShare_RV.SetActive(true);
				PassscreenHome_normal.SetActive(false);
				passscreenShare_normal.SetActive(false);
            }
		}


        string tmp = PlayerPrefs.GetString("SuperOverCompletedLevel");
        int[] array = tmp.Split('-').Select(n => Convert.ToInt32(n)).ToArray();

        for (int i=0;i<=CONTROLLER.LevelId;i++)
        {
            if(array[i]==0)
            {
                PassscreenNext.SetActive(false);
				SetButtonAnim(1);
				break;
            }
        }

    }

	private void SetButtonAnim(int idx)//0-continue 1-replay
	{
		if (idx == 1)
		{
			//PassScreenReplay.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(94f, 94f);
			Glow_pReplay.SetActive(true);
		}
		else
		{
			//PassscreenNext.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(94f, 94f);
			Glow_pNext.SetActive(true);
		}
	}

	public void  ShowInAppPage ()
	{
		GameObject prefabGO  ;
		GameObject tempGO ;
		prefabGO = Resources.Load ("Prefabs/InAppPage")as GameObject;
		tempGO = Instantiate (prefabGO)as GameObject;
		tempGO.name = "InAppPage";
		tempGO.transform.localPosition=new Vector3 (0f,0f,0.6f);	
		HideThis ();
	}

	private void  ValidateThisLevel ()
	{
		levelId.text = (CONTROLLER.LevelId+ 1).ToString () ;
		if (CONTROLLER.CurrentLevelCompleted >= CONTROLLER.LevelId && CONTROLLER.LevelFailed == 1 && CONTROLLER.LevelCompletedArray[CONTROLLER./*CurrentLevelCompleted*/LevelId] == 0)
		{
			failScreen.SetActive (true);
			passScreen.SetActive (false);
			AudioPlayer.instance.PlayGameSnd("levelfailed");
		}
		else
		{
			passScreen.SetActive (true);
			failScreen.SetActive (false);
			AudioPlayer.instance.PlayGameSnd("levelcompleted");
		}
		//30march
		if (GameModel.instance.levelCompleted == true)
		{
			passScreen.SetActive (true);
			failScreen.SetActive (false);
			GameModel.instance.levelCompleted = false;

		}
		else
		{
			failScreen.SetActive (true);
			passScreen.SetActive (false);

		}

		//if (CONTROLLER.LevelId == (CONTROLLER.totalLevels - 1) && CONTROLLER.LevelFailed == 0)
		//{
		//	//cleared all levels
		//}

		BlastEffect.instance.playAnimation(canvas,passScreen.activeSelf);
		Scoreboard.instance.Hide(true);
	}

	public void ShowInstertialAdLoadScreen(int index)
	{
		if (CONTROLLER.LevelId % 2 == 0 && (index == 1 || index == 2))
		{
			if (index == 1)
				GoNextLevel();
			else
				ReplayThisLevel();
		}
	}

	public void  GoToHome ()
	{
		CONTROLLER.CurrentPage = "";
        GameModel.instance.ConfirmQuit ();
		HideThis ();
	}

	public void  ReplayThisLevel ()
	{
        //CONTROLLER.sndController.PlayButtonSnd ();
        CONTROLLER.CurrentPage = "";
        AnimationScreen.instance.Hide (true);
		bowlingSpotScript.HideBowlingSpot ();
		CONTROLLER.NewInnings = true;
		GameModel.instance.ReStartGame ();
		HideThis ();
	}

	public void  GoNextLevel ()
	{
		AdIntegrate.instance.SetTimeScale(1f);
        CONTROLLER.CurrentPage = "";
        Invoke("ForDelay", 0.2f);
    }

	void ForDelay()
	{
        BattingScoreCard.instance.ResetPlayerImages();
		//CONTROLLER.sndController.PlayButtonSnd ();
		if (CONTROLLER.LevelId < 17)
			CONTROLLER.LevelId++;
		CONTROLLER.NewInnings = true;
		CONTROLLER.InningsCompleted = false;
		SetBowler();
		PlayerPrefs.SetInt("SOwicketGainUsed", 0);
		HideThis();
	}

	private void  SetBowler ()
	{
		int mod = CONTROLLER.LevelId % 2;
		if (mod == 0)
		{
			CONTROLLER.bowlerType = "fast";
		}
		else
		{
			CONTROLLER.bowlerType = "spin";
		}
		FadeView.instance.Hide (false);
		GroundController.instance.ResetYield ();
		GameModel.instance.ResetVariables ();
		Scoreboard.instance.Hide (false);
		PreviewScreen.instance.Hide (false);
		
		ShowSuperOverLevelInfo ();//29march
		//GameModel.instance.ShowIntroAnimation ();//29march
	}

	private void  ShowSuperOverLevelInfo ()
	{
		GameObject prefabGO ;
		GameObject tempGO ;
		prefabGO = Resources.Load ("Prefabs/SuperOverLevelInfo")as GameObject ;
		tempGO = Instantiate (prefabGO)as GameObject ;
		tempGO.name = "SuperOverLevelInfo";
	}

	/*private void  ShowDisplayMsg (string  errorStr,int  action)
	{
		GameObject prefabGO ;
		GameObject tempGO;

		prefabGO = Resources.Load ("Prefabs/DisplayMsg")as GameObject ;
		tempGO = Instantiate (prefabGO)as GameObject ;
		tempGO.name = "DisplayMsg";
		tempGO.transform.localPosition = new Vector3 (0f, 0f, 0.4f);
		GameObject OkBtn = GameObject.Find ("DisplayMsg/OkButton");
		OkBtn.transform.localPosition=new Vector3 (OkBtn.transform.localPosition.x,OkBtn.transform.localPosition.y,-10f);
		DisplayMsg _displayMsg = tempGO.GetComponent<DisplayMsg>();
		_displayMsg.DisplayError (errorStr, action);
	}*/

	private void  HideMe ()
	{
		this.gameObject.transform.localPosition = CONTROLLER.HIDEPOS;
	}
	private void  ShowMe ()
	{
		this.gameObject.transform.localPosition = CONTROLLER.SHOWPOS;
		this.gameObject.transform.localPosition =new Vector3 (this.gameObject.transform.localPosition.x,this.gameObject.transform.localPosition.y,1f);
	}

	private void  HideThis ()
	{
		background.SetActive(false);
		if (BlastEffect.instance != null)
			BlastEffect.instance.StopAnimation();
		Invoke("destroyGameobj", 2f);
	}

	private void destroyGameobj()
	{
		Destroy(this.gameObject);
		Resources.UnloadUnusedAssets();
	}

	public ModeInstructionLogoAssigner instuctionAssigner;
	public void instructions() 
	{
		background.SetActive (false);
		instructionsPage.SetActive (true);
        CONTROLLER.CurrentPage = "SOinstructions";
		instuctionAssigner.updateInstructionText();
	}
	public void showSettings()
	{
		CONTROLLER.CurrentPage = "SOsettings";
		settings.SetActive(true);
	}
	public void hideInstructions()
	{
		background.SetActive (true);
		instructionsPage.SetActive (false);
	}

}
