using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : Singleton<Scoreboard> 
{

	public GameObject scoreBoard, extraBall;
	public Image muteBtn;
	public Button pauseBtn;
	public Sprite mute, unmute;
	public Text ScoreTxt,OverTxt;

	public GameObject targetBG ;
	public Text TargetTxt ;

	public GameObject stripBG,stripChaseBG ;
	public Text StripTxt  ;
	public Text StripTxt_CT;

	public Text[] BallInfo  ;

	public GameObject ProfileDatas;

	private string[]  LevelDescriptionArray = new string[]  {
		"Hit 10 runs", 
		"Hit 3 fours", 
		"Hit 3 consecutive fours", 
		"Hit 3 sixes", 
		"Hit 5 fours", 
		"Hit 25 runs",
		"Hit 4 consecutive sixes", 
		"Hit 6 fours", 
		"Hit 6 sixes"};

	private Camera renderCam ;
	public RectTransform SafeArea;


	protected void  Awake ()
	{
        isRTUpdated = false;
        extraBall.SetActive (false);
		this.Hide (true);
		HideStrip (true);
		renderCam = GameObject.Find ("MainCamera").GetComponent<Camera>();
    }

	protected void Start()
	{
		if (GameModeSelector.isNewGame)
		{
			NewOver();
		}

		//gopi v1.1.2
		if (muteBtn.sprite.name == unmute.name)
		{
			CONTROLLER.isMuted = 1;
			if (AudioPlayer.instance != null)
				AudioPlayer.instance.MuteAudio(1);
		}

		if (CONTROLLER.GameMusicVal == 0)
			muteBtn.sprite = unmute;
		else
			muteBtn.sprite = mute;

		SafeAreaManager.Initialize();
		SafeAreaManager.ApplySafeArea(SafeArea);
	}

	private bool isRTUpdated=false;
    public RectTransform RT_viewInfo, RT_viewScore;
    public RectTransform RT_ProfileData;
	public RectTransform RT_previewMap;
	void UpdateInGameTopUIpos()
	{
		if (AdIntegrate.instance.CurrentSceneIndex == 2 && !isRTUpdated )
		{
            isRTUpdated = true;
            if (CONTROLLER.selectedGameMode != GameMode.BattingMultiplayer)
			{
				if (CONTROLLER.isAdRemoved )
				{
					RT_ProfileData.anchoredPosition = new Vector2(RT_ProfileData.anchoredPosition.x, RT_ProfileData.anchoredPosition.y + 107);
					RT_viewScore.anchoredPosition = new Vector2(RT_viewScore.anchoredPosition.x, RT_viewScore.anchoredPosition.y + 107);
					RT_viewInfo.anchoredPosition = new Vector2(RT_viewInfo.anchoredPosition.x, RT_viewInfo.anchoredPosition.y + 107);
					RT_previewMap.anchoredPosition = new Vector2(RT_previewMap.anchoredPosition.x, RT_previewMap.anchoredPosition.y + 107);
				}
			}
			else
			{
				RT_previewMap.anchoredPosition = new Vector2(RT_previewMap.anchoredPosition.x, RT_previewMap.anchoredPosition.y + 107);
			}
		}
	}

    public void MuteSound()
	{
		AudioPlayer.instance.PlayButtonSnd();
		if (muteBtn.sprite == mute)
		{
			CONTROLLER.isMuted = 0;
			AudioPlayer.instance.MuteAudio(0);
			muteBtn.sprite = unmute;
			CONTROLLER.GameMusicVal = 0;
			CONTROLLER.BGMusicVal = 0; //0036881
		}
		else if (muteBtn.sprite == unmute)
		{
			CONTROLLER.isMuted = 1;
            muteBtn.sprite = mute;
            CONTROLLER.GameMusicVal = 1;
            CONTROLLER.BGMusicVal = 1;
			AudioPlayer.instance.MuteAudio(1);
		}
		PlayerPrefsManager.SetSettingsList();
	}

	public void pauseGame ()
	{
		AudioPlayer.instance.PlayButtonSnd();
		GameModel.instance.GamePaused(true);
    }

    public void  UpdateScoreCard ()
	{
		for (int i = 0; i < 6; i++) {
			BallInfo [i].transform.parent.gameObject.SetActive (true);
		}
		extraBall.SetActive (false);
		string  str ;
		string playerName;
		int  runs ;
		int  balls ;
		ScoreTxt.text = GameModel.ScoreStr;
		OverTxt.text = GameModel.OversStr;
		if (CONTROLLER.selectedGameMode == GameMode.OnlyBatting)
		{
			TargetTxt.text = "POINTS : "+CONTROLLER.totalPoints;
		}
		BallInfo[0].text = CONTROLLER.ballUpdate[0];
		BallInfo[1].text = CONTROLLER.ballUpdate[1];
		BallInfo[2].text = CONTROLLER.ballUpdate[2];
		BallInfo[3].text = CONTROLLER.ballUpdate[3];
		BallInfo[4].text = CONTROLLER.ballUpdate[4];
		BallInfo[5].text = CONTROLLER.ballUpdate[5];
    }

	public void  ShowTargetScreen (bool  boolean)
	{
		if (CONTROLLER.selectedGameMode == GameMode.SuperOver || CONTROLLER.selectedGameMode == GameMode.ChaseTarget || CONTROLLER.selectedGameMode == GameMode.SUPER_Crusade_GameMode || ( CONTROLLER.isBatBowlMode() && CONTROLLER.currentInnings ==1 ))
		{
			if(boolean == true)
			{
				targetBG.SetActive (true);
				TargetTxt.text = "TARGET (" + CONTROLLER.TargetToChase+")";
			}
			else
			{
				targetBG.SetActive (false);
				TargetTxt.text = " ";
			}
		}

        UpdateInGameTopUIpos();
    }

    public void  UpdateStripText (string  str )
	{
		StripTxt.text= StripTxt_CT.text = str;

		if (str != "")
		{
			if (CONTROLLER.selectedGameMode == GameMode.ChaseTarget)
				stripChaseBG.SetActive(true);
			else
				stripBG.SetActive (true);
		}
		else
		{
			stripBG.SetActive (false);
			stripChaseBG.SetActive(false);
		}
	}
	public void  HideStrip (bool  boolean)
	{
		UpdateStripText ("");
		if (CONTROLLER.selectedGameMode == GameMode.SuperOver || CONTROLLER.selectedGameMode == GameMode.ChaseTarget || CONTROLLER.selectedGameMode == GameMode.SUPER_Crusade_GameMode)
		{
			if(boolean == true)
			{
				stripBG.SetActive (false);
				stripChaseBG.SetActive(false);
			}
			else
			{
				if (CONTROLLER.selectedGameMode == GameMode.ChaseTarget)
					stripChaseBG.SetActive(true);
				else
					stripBG.SetActive (true);
			}
		}
	}

	public void  ShowChallengeTitle ()
	{
		if (CONTROLLER.selectedGameMode == GameMode.SuperOver)
		{
			UpdateStripText (LevelDescriptionArray[(int )Mathf.Floor(CONTROLLER.LevelId/2)]);
		}
	}

	public void  TargetToWin ()
	{
		int  toWin = CONTROLLER.TargetToChase - CONTROLLER.currentMatchScores;
		int  fromBalls = (CONTROLLER.totalOvers * 6) - CONTROLLER.currentMatchBalls;
		int  wkts  = CONTROLLER.totalWickets - CONTROLLER.currentMatchWickets;

		string  str = "Required " + toWin + " runs from " + fromBalls +" balls.";
		if (CONTROLLER.selectedGameMode == GameMode.ChaseTarget || CONTROLLER.selectedGameMode == GameMode.SUPER_Crusade_GameMode)
		{
			UpdateStripText (str);
		}
	}

	public void  NewOver ()
	{
//		yield WaitForSeconds (0.01);
		CONTROLLER.ballUpdate[0] = "";
		CONTROLLER.ballUpdate[1] = "";
		CONTROLLER.ballUpdate[2] = "";
		CONTROLLER.ballUpdate[3] = "";
		CONTROLLER.ballUpdate[4] = "";
		CONTROLLER.ballUpdate[5] = "";

		BallInfo[0].text = "";
		BallInfo[1].text = "";
		BallInfo[2].text = "";
		BallInfo[3].text = "";
		BallInfo[4].text = "";
		BallInfo[5].text = "";
	}

	public void  Hide (bool  boolean)
	{
		//DebugLogger.PrintWithColor("Scoreboard Hide called::::: "+boolean);
		if(CONTROLLER.selectedGameMode == GameMode.BattingMultiplayer && !boolean)
		{
            scoreBoard.SetActive(false);
            ProfileDatas.SetActive(false);
            HidePause(true);
        }
        else if (boolean == true)
		{
			scoreBoard.SetActive (false);
			HidePause (true);
		}
		else
		{
			scoreBoard.SetActive (true);
			ProfileDatas.SetActive(true);
		}
	}

	public void HideButtonsWhenShotMade()
	{
		pauseBtn.gameObject.SetActive(false);
		muteBtn.transform.gameObject.SetActive(false);
	}

	public void  HidePause (bool  boolean)
	{
		if (boolean == true) 
		{
			pauseBtn.gameObject.SetActive (false);
			stripBG.SetActive (false); stripChaseBG.SetActive(false);
			muteBtn.transform.parent.gameObject.SetActive (false);
			ProfileDatas.SetActive(false);
			PreviewScreen.instance.Hide(true);			
		}
		else 
		{
			muteBtn.transform.parent.gameObject.SetActive (true);
			
			if (CONTROLLER.GameIsOnFocus)
			{
				muteBtn.transform.gameObject.SetActive(true);
				pauseBtn.gameObject.SetActive(true);
			}
			ProfileDatas.SetActive(true);
			PreviewScreen.instance.Hide(false);
			if (StripTxt.text != "") 
			{
				if (CONTROLLER.selectedGameMode == GameMode.ChaseTarget)
					stripChaseBG.SetActive(true);
				else
					stripBG.SetActive (true);
			}
		}
	}
}
