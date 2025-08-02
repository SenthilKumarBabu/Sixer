using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GamePauseScreen : Singleton<GamePauseScreen> 
{
	public GameObject gamePause, instructionPage, settingsPage, playerDetails, loftButton;

	public GameObject GameModeLogo ;
	private bool shotPresent, positionPresent;
	public Sprite[] gameModes;
	public GameObject quitPopup, shotHolder, positionHolder;
	private BowlingSpot bowlingSpotScript ;

	[Header("PAUSE SCREEN")]
	public GameObject pauseBG;
	public GameObject quitBG;

	protected void  Awake ()
	{
		bowlingSpotScript = GameObject.Find("BowlingSpot").GetComponent<BowlingSpot>();
		this.Hide (true);
	}

	protected void  Start ()
	{
		
		quitPopup.SetActive (false);
		instructionPage.SetActive (false);
		settingsPage.SetActive (false);
	}

	public void SetLoftState(bool flag)
	{
		loftButton.SetActive(flag);
	}
	public void menuClicked(int index)
	{
		AudioPlayer.instance.PlayButtonSnd();

        if (index == 0)
		{
            if (shotPresent)
			{
				shotHolder.SetActive(true);
				shotPresent = false;
			}
			if (positionPresent)
			{
				positionHolder.SetActive(true);
				positionPresent = false;
			}
			playerDetails.SetActive(true);
			SetLoftState(true);
			CONTROLLER.CurrentPage = "ingame";
			Hide(true);
			GameModel.instance.GamePaused(false);
			if (ProgressBar.instance != null && ProgressBar.instance.holder.activeSelf)
				ProgressBar.instance.close();

            if (!CONTROLLER.GameIsOnFocus)
                GroundController.instance.SetFieldersAnimSpeed(false);
            CONTROLLER.GameIsOnFocus = true;
        }
        else if (index == 1)
		{
            if (!CONTROLLER.GameIsOnFocus)
                GroundController.instance.SetFieldersAnimSpeed(false);
            CONTROLLER.GameIsOnFocus = true;

            Hide(true);
			AnimationScreen.instance.Hide(true);
			bowlingSpotScript.HideBowlingSpot();
			CONTROLLER.NewInnings = true;
			SetLoftState(true);
			playerDetails.SetActive(true);
			GameModel.instance.ReStartGame();
			CONTROLLER.CurrentPage = "ingame";
			//GroundController.instance.InitCamera ();
		}
		else if (index == 2)
		{
			//ShowInGameBg ();//25march
			settingsPage.SetActive(true);
			CONTROLLER.tempCurrentPage = CONTROLLER.CurrentPage;
			CONTROLLER.CurrentPage = "settingspage";
		}
		else if (index == 3)
		{
			instructions();
			CONTROLLER.CurrentPage = "instructionpage";
		}
		else if (index == 4)
		{
            if (!CONTROLLER.GameIsOnFocus)
                GroundController.instance.SetFieldersAnimSpeed(false);
            CONTROLLER.GameIsOnFocus = true;
            Hide(true);
			BattingScoreCard.instance.ShowMe();
		}
		else if (index == 5)
		{
            playerDetails.SetActive(true);
			SetLoftState(true);
			quitPopup.SetActive(true);
			DoTweenController.DOscale(quitBG, 0.5f, 0f, 1f, 0f, ease: true, easeType: Ease.OutBack, setUpdate: true);
			CONTROLLER.CurrentPage = "dispMsg";
		}
    }

	//Quit popup button actions
    public void MessageResponse(int i)
    {
        if (i == 1)
        {
            LoadingScreen.instance.Show();
            GameModel.instance.ConfirmQuit();
            GroundController.instance.stopAllAnimations();
            CONTROLLER.GameIsOnFocus = true;
        }
        else if (i == 0)
        {
            SetGamePauseScreen(true);
            quitPopup.SetActive(false);
            if (GameModel.isGamePaused == true)
            {
                CONTROLLER.CurrentPage = "gamepausepage";
            }
        }
        else if (i == 2)
        {
            SetGamePauseScreen(true);
            instructionPage.SetActive(false);
            if (GameModel.isGamePaused == true)
            {
                CONTROLLER.CurrentPage = "gamepausepage";
            }
        }
    }

    public void SetGamePauseScreen(bool flag)
	{
		gamePause.SetActive(flag);

		if (flag)
		{
			pauseBG.transform.DOScale(0f, 0f).SetUpdate(true);
			pauseBG.transform.DOScale(1f, 0.5f).SetUpdate(true).SetEase(Ease.OutBack);
			//DoTweenController.DOscale(pauseBG, 0.5f, 0f, 1f, 0f, ease: true, easeType: Ease.OutBack, setUpdate: true);
		}


		if (flag)
			AdIntegrate.instance.SystemSleepSettings(1);
		else
			AdIntegrate.instance.SystemSleepSettings(0);
	}

	private void  ShowInGameBg ()//25march
	{
		GameObject prefabGO ;
		GameObject tempGO ;

		prefabGO = Resources.Load ("Prefabs/InGameBg")as GameObject ;
		tempGO = Instantiate (prefabGO)as GameObject ;
		tempGO.name = "InGameBg";
		tempGO.transform.localPosition = new Vector3 (0f, 0f, 1.01f);
	}

	


	public ModeInstructionLogoAssigner instuctionAssigner;

	private void instructions() 
	{
		//SetGamePauseScreen(false);
		instructionPage.SetActive (true);
		instuctionAssigner.updateInstructionText();
	}

	public void  Hide (bool  boolean)
	{
		if(boolean == true)
		{
			SetGamePauseScreen(false);		
		}
		else
		{
			if (GameModel.instance.positionHolder.activeSelf) 
			{
				positionHolder.SetActive (false);
				positionPresent = true;
			}
			if (GameModel.instance.shotHolder.activeSelf) 
			{
				shotHolder.SetActive (false);
				SetLoftState(false);
				shotPresent = true;
			}
			playerDetails.SetActive (false);
			CONTROLLER.CurrentPage = "gamepausepage";//shankar 08April
			SetGamePauseScreen(true);
			BattingScoreCard.instance.HideMe ();
		}
	}
}
