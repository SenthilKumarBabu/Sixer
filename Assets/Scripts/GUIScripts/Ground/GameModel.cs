using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System ;
using System .Linq ;
using UnityEditor.Hardware;
using Photon.Pun;
using Photon.Realtime;

public class GameModel : Singleton<GameModel>
{
	//public static GameModel instance;
	public static string ScoreStr;
	public static string ExtraStr;
	public static string OversStr;
	public static  bool isGamePaused = false;
	public bool CanShowBattingScoreCard = false;
	public bool levelCompleted = false;
	public int BatsmanEntryTime = 3;
	public float fadeTime = 0.2f;
	public GameObject positionHolder;
	public GameObject shotHolder;
	//public UIStateToggleBtn batsman;

	private bool CanPauseGame = false;
	private bool IntroOpeners ;
	public int NewBatsmanIndex ;
	private int PowerPlayOver ;
	public  int currentBall  = -1;
	private int runsScoredInOver ;
	public bool  IsWicketBall = false;
	public int  batsmanOutIndex ;
	private bool  gameQuit = false;
	private bool  overCompleted = false;
	private bool  inningsCompleted = false;
	private int action ; // 0 -- battingscorecard 1 -- bowlingscorecard 2 -- batsmanrecord 3 -- batsmaninfo 4 -- bowlingcontrol 5 -- shotbtn 6 -- run btn
	private int bowlingControl  = -1;  // 0 -- Speed, 1 -- spin, 2 -- selected
	private int userAction  = -1;

	private Touch[] touchPhase;
	private bool NewTouch = false;
	private Vector2 TouchInitPos;
	private Vector2 TouchEndPos;
	private int selectedAngle;
	protected bool isTouchEnded = true;
	protected bool shotCompleted = false;
	protected float touchInitTime;
	protected float shotTimeLimit = 0.3f;

	protected int introAction = -1;
	protected int SlogOverValue;
	protected int WicketsInOver;

	private bool  maxRunsReached  = false;
	private bool  maxWidesReached  = false;
	private Vector2  prevMousePos ;
	//private SpriteText SkipText;
	public bool  ContinueMatch = false;
	private string [] NamesToDisplayInGoogle= new string[] { "Rookie", "SemiPro", "Professional", "Veteran", "Champion", "Legend" };//April2
	private int  CurrentSubLevel;

	public bool bChaseExtraBall;
	public int nChaseHeadStrtCount;

	protected void Awake ()
	{
		//instance = this;
/*		if (GoogleAnalyticsBinder.instance != null)
		{
			GoogleAnalyticsBinder.instance.PostPage ();
		}*/
		GameObject SkipObj = GameObject.Find ("GUIContainer/14_skiptext");
		/*if(SkipObj != null)
		{
			SkipObj.transform.localPosition = CONTROLLER.SHOWPOS;
			SkipText = SkipObj.GetComponent<SpriteText> ();
			SkipText.Text = "";
		}*/
		GetPlayingTeam ();
		isGameSaved ();
	}

	protected IEnumerator Start ()
	{
        AdIntegrate.instance.SystemSleepSettings(0);

		StartGame();
		yield return new  WaitForSeconds (0.001f);//28march

		if(CONTROLLER .gameMode !="multiplayer")
		{
			BattingScoreCard.instance.ResetBattingCard ();
			if(ContinueMatch == true)
			{
				MatchStartingFromSaved ();
			}
			else
			{
				SaveinGoogleAnalytics (ContinueMatch);
			}
		}
	}

	private bool IsPointerOverUIObject() {

		if (CONTROLLER.gameMode == "multiplayer")
			return false;

		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		foreach(RaycastResult result in results) {
			if (result.gameObject.name == "PauseButton" || result.gameObject.name == "MuteButton" || result.gameObject.name == "LoftButton")
				return false;
		}
		return results.Count > 0;
	}

	private void  isGameSaved ()
	{
		NewBatsmanIndex = 1;
		CONTROLLER.StrikerIndex = 0;
		CONTROLLER.NonStrikerIndex = 1;
		string  str = GetOverStr ();
		if (CONTROLLER.gameMode == "superover")
		{
			if(PlayerPrefs.HasKey ("SuperOverDetail"))
			{
				string superOverStr = PlayerPrefs.GetString ("SuperOverDetail");
				string[]  superOverArray= superOverStr.Split ("|" [0]);
               
				CONTROLLER.bowlerType = superOverArray[0] as string;
				CONTROLLER.currentMatchScores = int.Parse(superOverArray[1] as string);
				CONTROLLER.currentMatchWickets = int.Parse(superOverArray[2] as string);
				CONTROLLER.currentMatchBalls = int.Parse(superOverArray[3] as string);
				currentBall = int.Parse(superOverArray[4] as string);
				CONTROLLER.ballUpdate[0] = superOverArray[5] as string;
				CONTROLLER.ballUpdate[1] = superOverArray[6] as string;
				CONTROLLER.ballUpdate[2] = superOverArray[7] as string;
				CONTROLLER.ballUpdate[3] = superOverArray[8] as string;
				CONTROLLER.ballUpdate[4] = superOverArray[9] as string;
				CONTROLLER.ballUpdate[5] = superOverArray[10] as string;
				NewBatsmanIndex = int.Parse(superOverArray[11] as string);
				CONTROLLER.StrikerIndex = int.Parse(superOverArray[12] as string);
				CONTROLLER.NonStrikerIndex = int.Parse(superOverArray[13] as string);

				CONTROLLER.totalFours = int.Parse(superOverArray[14] as string);
				CONTROLLER.totalSixes = int.Parse(superOverArray[15] as string);
				CONTROLLER.continousBoundaries = int.Parse(superOverArray[16] as string);
				CONTROLLER.continousSixes = int.Parse(superOverArray[17] as string);
                CONTROLLER.currentAIUniformColor = int.Parse(superOverArray[19] as string);
				if (superOverArray.Length == 20)
				{	
					CONTROLLER.LevelId = 0;
				}
				else
				{
					CONTROLLER.LevelId = int.Parse(superOverArray[18] as string);
				}
				ContinueMatch = true;
			}
			str = GetOverStr ();
		}
		else if (CONTROLLER.gameMode == "slogover")
		{
			if(PlayerPrefs.HasKey ("SlogOverDetail"))
			{
				string slogOverStr  = PlayerPrefs.GetString ("SlogOverDetail");
				string[] slogOverArray = slogOverStr.Split ("|" [0]);
				CONTROLLER.bowlerType = slogOverArray[0] as string;
				CONTROLLER.totalOvers = int.Parse(slogOverArray[1] as string);
				CONTROLLER.totalWickets = 10;
				CONTROLLER.totalPoints = int.Parse(slogOverArray[3] as string);
				CONTROLLER.currentMatchScores = int.Parse(slogOverArray[4] as string);
				CONTROLLER.currentMatchWickets = int.Parse(slogOverArray[5] as string);
				CONTROLLER.currentMatchBalls = int.Parse(slogOverArray[6] as string);
				currentBall = int.Parse(slogOverArray[7] as string);
				CONTROLLER.ballUpdate[0] = slogOverArray[8] as string;
				CONTROLLER.ballUpdate[1] = slogOverArray[9] as string;
				CONTROLLER.ballUpdate[2] = slogOverArray[10] as string;
				CONTROLLER.ballUpdate[3] = slogOverArray[11] as string;
				CONTROLLER.ballUpdate[4] = slogOverArray[12] as string;
				CONTROLLER.ballUpdate[5] = slogOverArray[13] as string;
				NewBatsmanIndex = int.Parse(slogOverArray[14] as string);
				CONTROLLER.StrikerIndex = int.Parse(slogOverArray[15] as string);
				CONTROLLER.NonStrikerIndex = int.Parse(slogOverArray[16] as string);
                CONTROLLER.currentAIUniformColor = int.Parse(slogOverArray[18] as string);
                CONTROLLER.BowlingEnd = slogOverArray[17] as string;
				ContinueMatch = true;
			}
			str = GetOverStr ();
		}
		else if (CONTROLLER.gameMode == "chasetarget")
		{
			if(PlayerPrefs.HasKey ("ChaseTargetDetail"))
			{
				string chaseTargetStr  = PlayerPrefs.GetString ("ChaseTargetDetail");
				string[] chaseTargetArray = chaseTargetStr.Split ("|" [0]);
				CONTROLLER.bowlerType = chaseTargetArray[0] as string;
				CONTROLLER.totalPoints = int.Parse(chaseTargetArray[1] as string);
				CONTROLLER.currentMatchScores = int.Parse(chaseTargetArray[2] as string);
				CONTROLLER.currentMatchWickets = int.Parse(chaseTargetArray[3] as string);
				CONTROLLER.currentMatchBalls = int.Parse(chaseTargetArray[4] as string);
				currentBall = int.Parse(chaseTargetArray[5] as string);
				CONTROLLER.TargetToChase = int.Parse(chaseTargetArray[6] as string);
				CONTROLLER.totalOvers = int.Parse(chaseTargetArray[7] as string);
				CONTROLLER.ballUpdate[0] = chaseTargetArray[8] as string;
				CONTROLLER.ballUpdate[1] = chaseTargetArray[9] as string;
				CONTROLLER.ballUpdate[2] = chaseTargetArray[10] as string;
				CONTROLLER.ballUpdate[3] = chaseTargetArray[11] as string;
				CONTROLLER.ballUpdate[4] = chaseTargetArray[12] as string;
				CONTROLLER.ballUpdate[5] = chaseTargetArray[13] as string;
				NewBatsmanIndex = int.Parse(chaseTargetArray[14] as string);
				CONTROLLER.StrikerIndex = int.Parse(chaseTargetArray[15] as string);
				CONTROLLER.NonStrikerIndex = int.Parse(chaseTargetArray[16] as string);
				CONTROLLER.BowlingEnd = chaseTargetArray[17] as string;
				CONTROLLER.CTLevelId = int.Parse(chaseTargetArray[18] as string);
				CONTROLLER.CTSubLevelCompleted = int.Parse(chaseTargetArray[19] as string);
				CONTROLLER.CTCurrentPlayingMainLevel = CONTROLLER.CTLevelId;
                CONTROLLER.currentAIUniformColor = int.Parse(chaseTargetArray[20] as string);
                ContinueMatch = true;

				CONTROLLER.CTSubLevelId = CONTROLLER.CTSubLevelCompleted;	//v1.1.1 	

			}
			str = GetOverStr ();
		}
        else if (CONTROLLER.gameMode == CONTROLLER.SUPER_Crusade_GameMode)
        {
            if (PlayerPrefs.HasKey(CONTROLLER.SUPER_Crusade_SavedMatchDetails))
            {
                string chaseTargetStr = PlayerPrefs.GetString(CONTROLLER.SUPER_Crusade_SavedMatchDetails);
                string[] chaseTargetArray = chaseTargetStr.Split("|"[0]);
                CONTROLLER.bowlerType = chaseTargetArray[0] as string;
                CONTROLLER.totalPoints = int.Parse(chaseTargetArray[1] as string);
                CONTROLLER.currentMatchScores = int.Parse(chaseTargetArray[2] as string);
                CONTROLLER.currentMatchWickets = int.Parse(chaseTargetArray[3] as string);
                CONTROLLER.currentMatchBalls = int.Parse(chaseTargetArray[4] as string);
                currentBall = int.Parse(chaseTargetArray[5] as string);
                CONTROLLER.TargetToChase = int.Parse(chaseTargetArray[6] as string);
                CONTROLLER.totalOvers = int.Parse(chaseTargetArray[7] as string);
                CONTROLLER.ballUpdate[0] = chaseTargetArray[8] as string;
                CONTROLLER.ballUpdate[1] = chaseTargetArray[9] as string;
                CONTROLLER.ballUpdate[2] = chaseTargetArray[10] as string;
                CONTROLLER.ballUpdate[3] = chaseTargetArray[11] as string;
                CONTROLLER.ballUpdate[4] = chaseTargetArray[12] as string;
                CONTROLLER.ballUpdate[5] = chaseTargetArray[13] as string;
                NewBatsmanIndex = int.Parse(chaseTargetArray[14] as string);
                CONTROLLER.StrikerIndex = int.Parse(chaseTargetArray[15] as string);
                CONTROLLER.NonStrikerIndex = int.Parse(chaseTargetArray[16] as string);
                CONTROLLER.BowlingEnd = chaseTargetArray[17] as string;
                CONTROLLER.SelectedCrusadeSeasonIdx= int.Parse(chaseTargetArray[18] as string);
                CONTROLLER.SelectedCrusadeMatchIdx = int.Parse(chaseTargetArray[19] as string);

                CONTROLLER.totalFours = int.Parse(chaseTargetArray[20] as string);
                CONTROLLER.totalSixes = int.Parse(chaseTargetArray[21] as string);
                CONTROLLER.continousBoundaries = int.Parse(chaseTargetArray[22] as string);
                CONTROLLER.continousSixes = int.Parse(chaseTargetArray[23] as string);
                CONTROLLER.currentAIUniformColor = int.Parse(chaseTargetArray[24] as string);
                ContinueMatch = true;
            }
            str = GetOverStr();
        }


        if (currentBall >= 5)
		{
			currentBall = -1; 
			for(int  i= 0; i < 6; i++)
			{
				CONTROLLER.ballUpdate[i] = "";
			}
		}
	}

	private void  MatchStartingFromSaved ()
	{
		string  playerDetailStr = "";
		int i ;
		if (CONTROLLER.gameMode == "superover")
		{
			if(PlayerPrefs.HasKey ("superoverPlayerDetails"))
			{
				playerDetailStr = PlayerPrefs.GetString ("superoverPlayerDetails");
			}
		}
		else if (CONTROLLER.gameMode == "slogover")
		{
			if(PlayerPrefs.HasKey ("slogoverPlayerDetails"))
			{
				playerDetailStr = PlayerPrefs.GetString ("slogoverPlayerDetails");
			}
		}
		else if (CONTROLLER.gameMode == "chasetarget")
		{
			if(PlayerPrefs.HasKey ("chasetargetPlayerDetails"))
			{
				playerDetailStr = PlayerPrefs.GetString ("chasetargetPlayerDetails");
			}
		}
        else if (CONTROLLER.gameMode ==CONTROLLER.SUPER_Crusade_GameMode)
        {
            if (PlayerPrefs.HasKey(CONTROLLER.SUPER_Crusade_PlayerDetails))
            {
                playerDetailStr = PlayerPrefs.GetString(CONTROLLER.SUPER_Crusade_PlayerDetails);
            }
        }

        if (playerDetailStr != "" && playerDetailStr != null)
		{
			string [] PlayerScoreStr = playerDetailStr.Split ("|" [0]);
			for(i = 0; i < PlayerScoreStr.Length - 1; i++)
			{
				int _batsmanIndex =(int) CONTROLLER.PlayingTeam[i];
				string [] scoreStr= PlayerScoreStr[i].Split ("$" [0]);
				CONTROLLER.TeamList[0].PlayerList[_batsmanIndex].RunsScored = int.Parse (scoreStr[0]);
				CONTROLLER.TeamList[0].PlayerList[_batsmanIndex].BallsPlayed = int.Parse (scoreStr[1]);
				CONTROLLER.TeamList[0].PlayerList[_batsmanIndex].Fours = int.Parse (scoreStr[2]);
				CONTROLLER.TeamList[0].PlayerList[_batsmanIndex].Sixes = int.Parse (scoreStr[3]);
				CONTROLLER.TeamList[0].PlayerList[_batsmanIndex].status = scoreStr[4];
				if(CONTROLLER.TeamList[0].PlayerList[_batsmanIndex].status == "out")
				{
					BattingScoreCard.instance.UpdateWicket (i);
				}
				else
				{
					BattingScoreCard.instance.UpdateScoreCard ();
				}
			}
		}
		Scoreboard.instance.UpdateScoreCard ();
		BatsmanInfo.instance.UpdateStrikerInfo ();//28march
		GroundController.instance.ChangePlayerLeftRightTextures ();//April1
        GroundController.instance.SetFielderUniformColor();
	}

	public void  DetailsSavedBallbyBall ()
	{
		string  MatchDetailStr = "";
		if (CONTROLLER.gameMode == "superover")
		{
			MatchDetailStr += CONTROLLER.bowlerType + "|";		
			MatchDetailStr += CONTROLLER.currentMatchScores + "|";
			MatchDetailStr += CONTROLLER.currentMatchWickets + "|";
			MatchDetailStr += CONTROLLER.currentMatchBalls + "|";
			MatchDetailStr += currentBall + "|";
			MatchDetailStr += CONTROLLER.ballUpdate[0] + "|";
			MatchDetailStr += CONTROLLER.ballUpdate[1] + "|";
			MatchDetailStr += CONTROLLER.ballUpdate[2] + "|";
			MatchDetailStr += CONTROLLER.ballUpdate[3] + "|";
			MatchDetailStr += CONTROLLER.ballUpdate[4] + "|";
			MatchDetailStr += CONTROLLER.ballUpdate[5] + "|";
			MatchDetailStr += NewBatsmanIndex + "|";
			MatchDetailStr += CONTROLLER.StrikerIndex + "|";
			MatchDetailStr += CONTROLLER.NonStrikerIndex + "|";

			MatchDetailStr += CONTROLLER.totalFours + "|";
			MatchDetailStr += CONTROLLER.totalSixes + "|";
			MatchDetailStr += CONTROLLER.continousBoundaries + "|";
			MatchDetailStr += CONTROLLER.continousSixes + "|";//shankar 08April



            MatchDetailStr += CONTROLLER.LevelId + "|";//shankar 08April
            MatchDetailStr += CONTROLLER.currentAIUniformColor + "|"; //arun

            PlayerPrefs.SetString ("SuperOverDetail", MatchDetailStr);
		}
		else if (CONTROLLER.gameMode == "slogover")
		{
			MatchDetailStr += CONTROLLER.bowlerType + "|";
			MatchDetailStr += CONTROLLER.totalOvers + "|";
			MatchDetailStr += CONTROLLER.totalWickets + "|";
			MatchDetailStr += CONTROLLER.totalPoints + "|";
			MatchDetailStr += CONTROLLER.currentMatchScores + "|";
			MatchDetailStr += CONTROLLER.currentMatchWickets + "|";
			MatchDetailStr += CONTROLLER.currentMatchBalls + "|";
			MatchDetailStr += currentBall + "|";
			MatchDetailStr += CONTROLLER.ballUpdate[0] + "|";
			MatchDetailStr += CONTROLLER.ballUpdate[1] + "|";
			MatchDetailStr += CONTROLLER.ballUpdate[2] + "|";
			MatchDetailStr += CONTROLLER.ballUpdate[3] + "|";
			MatchDetailStr += CONTROLLER.ballUpdate[4] + "|";
			MatchDetailStr += CONTROLLER.ballUpdate[5] + "|";
			MatchDetailStr += NewBatsmanIndex + "|";
			MatchDetailStr += CONTROLLER.StrikerIndex + "|";
			MatchDetailStr += CONTROLLER.NonStrikerIndex + "|";
          
            MatchDetailStr += CONTROLLER.BowlingEnd + "|";
            MatchDetailStr += CONTROLLER.currentAIUniformColor + "|"; //arun
            PlayerPrefs.SetString ("SlogOverDetail", MatchDetailStr);
		}
		else if (CONTROLLER.gameMode == "chasetarget")
		{
			MatchDetailStr += CONTROLLER.bowlerType + "|";
			MatchDetailStr += CONTROLLER.totalPoints + "|";
			MatchDetailStr += CONTROLLER.currentMatchScores + "|";
			MatchDetailStr += CONTROLLER.currentMatchWickets + "|";
			MatchDetailStr += CONTROLLER.currentMatchBalls + "|";
			MatchDetailStr += currentBall + "|";
			MatchDetailStr += CONTROLLER.TargetToChase + "|";
			MatchDetailStr += CONTROLLER.totalOvers + "|";
			MatchDetailStr += CONTROLLER.ballUpdate[0] + "|";
			MatchDetailStr += CONTROLLER.ballUpdate[1] + "|";
			MatchDetailStr += CONTROLLER.ballUpdate[2] + "|";
			MatchDetailStr += CONTROLLER.ballUpdate[3] + "|";
			MatchDetailStr += CONTROLLER.ballUpdate[4] + "|";
			MatchDetailStr += CONTROLLER.ballUpdate[5] + "|";
			MatchDetailStr += NewBatsmanIndex + "|";
			MatchDetailStr += CONTROLLER.StrikerIndex + "|";
			MatchDetailStr += CONTROLLER.NonStrikerIndex + "|";
			MatchDetailStr += CONTROLLER.BowlingEnd + "|";
			MatchDetailStr += CONTROLLER.CTLevelId + "|";          
            MatchDetailStr += CONTROLLER.CTSubLevelCompleted + "|";
            MatchDetailStr += CONTROLLER.currentAIUniformColor + "|"; //arun
            PlayerPrefs.SetString ("ChaseTargetDetail", MatchDetailStr);
		}
        else if (CONTROLLER.gameMode == CONTROLLER.SUPER_Crusade_GameMode)
        {
            MatchDetailStr += CONTROLLER.bowlerType + "|";
            MatchDetailStr += CONTROLLER.totalPoints + "|";
            MatchDetailStr += CONTROLLER.currentMatchScores + "|";
            MatchDetailStr += CONTROLLER.currentMatchWickets + "|";
            MatchDetailStr += CONTROLLER.currentMatchBalls + "|";
            MatchDetailStr += currentBall + "|";
            MatchDetailStr += CONTROLLER.TargetToChase + "|";
            MatchDetailStr += CONTROLLER.totalOvers + "|";
            MatchDetailStr += CONTROLLER.ballUpdate[0] + "|";
            MatchDetailStr += CONTROLLER.ballUpdate[1] + "|";
            MatchDetailStr += CONTROLLER.ballUpdate[2] + "|";
            MatchDetailStr += CONTROLLER.ballUpdate[3] + "|";
            MatchDetailStr += CONTROLLER.ballUpdate[4] + "|";
            MatchDetailStr += CONTROLLER.ballUpdate[5] + "|";
            MatchDetailStr += NewBatsmanIndex + "|";
            MatchDetailStr += CONTROLLER.StrikerIndex + "|";
            MatchDetailStr += CONTROLLER.NonStrikerIndex + "|";
            MatchDetailStr += CONTROLLER.BowlingEnd + "|";
            MatchDetailStr += CONTROLLER.SelectedCrusadeSeasonIdx + "|";
            MatchDetailStr += CONTROLLER.SelectedCrusadeMatchIdx   +"|";

            MatchDetailStr += CONTROLLER.totalFours + "|";
            MatchDetailStr += CONTROLLER.totalSixes + "|";
            MatchDetailStr += CONTROLLER.continousBoundaries + "|";
            MatchDetailStr += CONTROLLER.continousSixes + "|";
            MatchDetailStr += CONTROLLER.currentAIUniformColor + "|"; //arun

            PlayerPrefs.SetString(CONTROLLER.SUPER_Crusade_SavedMatchDetails, MatchDetailStr);
        }

        string  playerDetailStr = "";
		for(var i = 0; i <= (CONTROLLER.currentMatchWickets + 1); i++)
		{
			if (i<CONTROLLER.PlayingTeam.Count)
			{
				int _batsmanIndex =(int ) CONTROLLER.PlayingTeam[i];
				playerDetailStr += CONTROLLER.TeamList[0].PlayerList[_batsmanIndex].RunsScored + "$";
				playerDetailStr += CONTROLLER.TeamList[0].PlayerList[_batsmanIndex].BallsPlayed + "$";
				playerDetailStr += CONTROLLER.TeamList[0].PlayerList[_batsmanIndex].Fours + "$";
				playerDetailStr += CONTROLLER.TeamList[0].PlayerList[_batsmanIndex].Sixes + "$";
				playerDetailStr += CONTROLLER.TeamList[0].PlayerList[_batsmanIndex].status + "|";
			}
		}
		if (CONTROLLER.gameMode == "superover")
		{
			PlayerPrefs.SetString ("superoverPlayerDetails", playerDetailStr);
		}
		else if (CONTROLLER.gameMode == "slogover")
		{
			PlayerPrefs.SetString ("slogoverPlayerDetails", playerDetailStr);
		}
		else if (CONTROLLER.gameMode == "chasetarget")
		{
			PlayerPrefs.SetString ("chasetargetPlayerDetails", playerDetailStr);
		}
        else if (CONTROLLER.gameMode ==CONTROLLER.SUPER_Crusade_GameMode)
        {
            PlayerPrefs.SetString(CONTROLLER.SUPER_Crusade_PlayerDetails, playerDetailStr);
        }
    }

	public void  StartGame ()
	{
		CONTROLLER.NewInnings = true;
		if (CONTROLLER.gameMode == "superover")
		{
			CONTROLLER.totalOvers = 1;
			CONTROLLER.totalWickets = 2;
			if(!PlayerPrefs.HasKey ("SuperOverDetail"))
			{
				BattingScoreCard.instance.HideMe ();
				ShowUIAnimation ();
			}
			else
			{
				BattingScoreCard.instance.ShowMe ();
			}
		}
		else if (CONTROLLER.gameMode == "slogover" )
		{
            CONTROLLER.totalOvers = CONTROLLER.totalOversPracticeMode;
			//if (CONTROLLER.EnableHardcodes == 1) //hardcode-gopi  
			//{
			//	CONTROLLER.totalOvers = 1;
			//}
			BattingScoreCard.instance.ShowMe ();
		}
		else if (CONTROLLER.gameMode == "chasetarget")
		{
            
            if (!PlayerPrefs.HasKey ("ChaseTargetDetail"))
			{
				BattingScoreCard.instance.HideMe ();
				ShowUIAnimation ();
			}
			else
			{
				BattingScoreCard.instance.ShowMe ();
			}
		}

        Scoreboard.instance.Hide (true);
		BatsmanInfo.instance.HideMe ();
		//Scoreboard.instance.HidePause (true);
		//PreviewScreen.instance.Hide (true);
		CONTROLLER.currentInnings = 0;
		ResetVariables ();
		JerseyHandler.instance.UpdateBatsmanMaterials();
		GroundController.instance.ChangePlayerLeftRightTextures ();//April1
	}

    public void  ShowUIAnimation ()
	{
		if (UIAnimation.instance == null)
		{
			GameObject  prefabGO ;
			GameObject tempGO;
			prefabGO = Resources.Load ("Prefabs/UIAnimation")as GameObject;
			tempGO = Instantiate (prefabGO)as GameObject;
			tempGO.name = "UIAnimation";
			Scoreboard.instance.Hide (true);
			BatsmanInfo.instance.HideMe ();
			PreviewScreen.instance.Hide (true);
		}
	}

	public void ShowIntroAnimation ()
	{
		if (StartScreen.instance != null)
		{
			StartScreen.instance.HideThis ();
		}

		CanPauseGame = false;
		nChaseHeadStrtCount = 0;
		if (CONTROLLER.gameMode != "chasetarget" || ContinueMatch  || !AdIntegrate .instance .isRewardedReadyToPlay ()) 
		{
			GameObject prefabGO;
			GameObject tempGO;
			prefabGO = Resources.Load ("Prefabs/StartScreen")as GameObject ;
			tempGO = Instantiate (prefabGO);
			tempGO.name = "StartScreen";

			if(ContinueMatch )
				ContinueMatch = false;	
		} 
		else 
		{
			HeadStart.instance.ShowMe ();
		}
	}

	public void  ResetCurrentMatchDetails ()
	{
		currentBall = -1; 
		CONTROLLER.currentMatchBalls = 0;
		CONTROLLER.currentMatchScores = 0;
		CONTROLLER.currentMatchWickets = 0;
		CONTROLLER.currentBallNumber = 0;
		CONTROLLER.totalPoints = 0;
		NewBatsmanIndex = 1;
		CONTROLLER.StrikerIndex = 0;
		CONTROLLER.NonStrikerIndex = 1;
		CanShowBattingScoreCard = false;
		//30march
		CONTROLLER.totalSixes = 0;
		CONTROLLER.totalFours = 0;
		CONTROLLER.continousBoundaries = 0;
		CONTROLLER.continousSixes = 0;
		//30march
		ClearLevelDetails ();
		for (int i = 0; i < CONTROLLER.TeamList.Length; i++)
		{
			for (int j= 0; j < CONTROLLER.TeamList[0].PlayerList.Length; j++)
			{
				CONTROLLER.TeamList[i].PlayerList[j].RunsScored = 0;
				CONTROLLER.TeamList[i].PlayerList[j].BallsPlayed = 0;
				CONTROLLER.TeamList[i].PlayerList[j].Fours = 0;
				CONTROLLER.TeamList[i].PlayerList[j].Sixes = 0;
				CONTROLLER.TeamList[i].PlayerList[j].status = "";
			}
		}
		BatsmanInfo.instance.UpdateStrikerInfo ();
		Scoreboard.instance.UpdateScoreCard ();
		if(CONTROLLER .gameMode !="multiplayer")
			BattingScoreCard.instance.ResetBattingCard ();
	}

	public void ResetVariables ()
	{
		CanPauseGame = false;
		IntroOpeners = false;
		runsScoredInOver = 0;
		IsWicketBall = false;
		//batsmanOutIndex = -1;
		inningsCompleted = false;
		levelCompleted = false;
		action = -1;
		WicketsInOver = 0;

		maxWidesReached = false;
		CONTROLLER.isReplayGame = false;
		NewInnings ();
	}

	public void NewInnings ()
	{
		if (CONTROLLER.gameMode == "slogover" || CONTROLLER.gameMode == "chasetarget" || CONTROLLER.gameMode == CONTROLLER.SUPER_Crusade_GameMode )
		{
			Scoreboard.instance.ShowTargetScreen (true);
		}
		else
		{
			Scoreboard.instance.ShowTargetScreen (false);
		}
		CONTROLLER.BattingTeamIndex = CONTROLLER.myTeamIndex;
		CONTROLLER.BowlingTeamIndex = CONTROLLER.opponentTeamIndex;

		ScoreStr = CONTROLLER.currentMatchScores + "/" + CONTROLLER.currentMatchWickets;
		OversStr = GetOverStr () + " (" + CONTROLLER.totalOvers + ")";
        CONTROLLER.SixDistance = 0;
		Scoreboard.instance.UpdateScoreCard ();
		BattingScoreCard.instance.UpdateScoreCard ();
		GroundController.instance.NewInnings ();
		GroundController.instance.ResetFielders ();
		GroundController.instance.moveBannerTextureScript.Reset();
	}
    
    public bool CanShowTutorial()
	{
		if (CONTROLLER.gameMode != "multiplayer" && CONTROLLER.currentMatchBalls < 3 && CONTROLLER.TutorialShowCount < 3 )
		{
			return true;
		}
		else
			return false;
	}
	public void ShowShotTutorial(bool canShow)
	{
		if (canShow)
		{
			shotHolder.SetActive(true);
			positionHolder.SetActive(false);
		}
		else
		{
			shotHolder.SetActive(false);
			positionHolder.SetActive(false);
		}
	}

	public void ShowBatsmanMoveTutorial(bool canShow)
	{
		if (canShow)
		{
			positionHolder.SetActive(true);
			shotHolder.SetActive(false);
		}
		else
		{
			positionHolder.SetActive(false);
			shotHolder.SetActive(false);
		}
	}
	//shankar 09April
	public void  IntroOver ()
	{
		GroundController.instance.IntroCompleted ();
	}
	public void HideFadeView ()
	{
		FadeView.instance.Hide(true);
	}
	private bool isGameInterrupted;
	void OnApplicationPause(bool isInterrupted)
	{
		if(CONTROLLER.CurrentPage == "" || CONTROLLER.CurrentPage != "ingame")
		{
			return;
		}

		if( !isGameInterrupted && isInterrupted )
		{   
			isGameInterrupted = true;
		}  

		if( isGameInterrupted /*&& !GameOverScreen.instance.isAdShownOnCoinsSpend*/ )
		{    
			// Call your pause screen here //  
			/*GamePauseScreen.instance.ShowPauseMenu ();
			if (CONTROLLER.CurrentPage == "ingame" && CONTROLLER.CurrentPage != "ultraslowmotion")
			{
				GameModel.instance.GamePaused(true);
			}*/
			isGameInterrupted = false;
		}
	}

	protected void Update ()
	{
		if(gameQuit == true || !CONTROLLER.GameIsOnFocus)
		{
			return;
		}

		Vector3  batsmanPoint = GroundController.instance.UpdateCameraPosition ();

		//GetKeyBoardInput ();
		if(Application.isEditor == false && CONTROLLER.TargetPlatform != "standalone" && CONTROLLER.TargetPlatform != "web")
		{
			switch (userAction)
			{
				case 10:
					// Can move batsman left or right
#if UNITY_ANDROID || UNITY_IPHONE
					if (CONTROLLER.BattingTeamIndex == CONTROLLER.myTeamIndex)
					{
						DetectBatsmanMove();
					}
#endif
					break;
				case 11:
					// Can batsman make shot
#if UNITY_ANDROID || UNITY_IPHONE
					if (CONTROLLER.BattingTeamIndex == CONTROLLER.myTeamIndex)
					{
						DetectBatsmanShot();
					}
#endif
					break;
			}

			if (CONTROLLER.InningsCompleted == true)
			{
				if (CanPauseGame == true)
				{
					if (!Scoreboard.instance.muteBtn.transform.parent.gameObject.activeSelf)
						Scoreboard.instance.HidePause(false);
				}
				else
				{
					if (Scoreboard.instance.muteBtn.transform.parent.gameObject.activeSelf)
						Scoreboard.instance.HidePause(true);
				}

				if (CONTROLLER.gameMode == "multiplayer" && Scoreboard.instance.muteBtn.transform.parent.gameObject.activeSelf)
					Scoreboard.instance.HidePause(true);
			}
		}
		else
		{
			switch (userAction)
			{
			case 10 :
				if(CONTROLLER.BattingTeamIndex == CONTROLLER.myTeamIndex)
				{
					DetectBatsmanMoveMouse ();
				}
				break;
			case 11 :
				if(CONTROLLER.BattingTeamIndex == CONTROLLER.myTeamIndex)
				{
					DetectBatsmanShotMouse ();
				}
				break;
			}


		}
		//if(action == 2 || action == 3 || introAction == 1 || introAction == 2)
		//{
		//	#if UNITY_IPHONE || UNITY_ANDROID
			//SkipText.Text = "Tap to continue";
		//	#else
			//SkipText.Text = "Press escape to continue";
		//	#endif
		//}
		/*else
		{
			if(SkipText != null)
			{
				SkipText.Text = "";
			}
		}*/
	}

	public void  ShowBattingScoreCard ()
	{
		if(isGamePaused == false)
		{
			action = 0;
		}
	}
	public void ResetPauseVar ()
	{
		overCompleted = false;
		CanPauseGame = true;
	}

	public void BowlNextBall()
	{
		NewBall();
		CONTROLLER.NewInnings = false;//April1
		BatsmanInfo.instance.UpdateStrikerInfo();//29march
		BatsmanInfo.instance.ShowMe();//29march

		Scoreboard.instance.HidePause(false);//29march
		Scoreboard.instance.Hide(false);//29march	

		GroundController.instance.BowlNextBall("user", "computer");
	}
	public void  NewBall ()
	{
		NewTouch = false;
		isTouchEnded = true; 
		TouchInitPos = new Vector2 ();
		TouchEndPos = new Vector2 ();
		touchInitTime = 0.0f;
		selectedAngle = 0;

		//batsmanOutIndex = -1;
		shotCompleted = false;
		userAction = -1;
		prevMousePos = Vector2.zero;
		CanPauseGame = true;
	}

	private void  ChangeBowlerType ()
	{
		if(CONTROLLER.gameMode == "slogover" || CONTROLLER.gameMode == "chasetarget" )//|| CONTROLLER.gameMode == CONTROLLER.SUPER_Crusade_GameMode)
		{
			if (UnityEngine .Random.Range (0, 2) == 0)
			{
				CONTROLLER.bowlerType = "fast";
			}
			else
			{
				CONTROLLER.bowlerType = "spin";
			}
		}
	}
	// Called from GroundController
	public void ShowBowlingInterface (bool boolean)
	{
		if(gameQuit == true)
		{
			return;
		}
		if(boolean == true)
		{
			//BowlingControls.instance.ShowBlocker ();
			if(CONTROLLER.BowlingTeamIndex == CONTROLLER.myTeamIndex)
			{
				bowlingControl = 0;
			}
			action = 4;
			Scoreboard.instance.HideStrip (false);
			Scoreboard.instance.ShowChallengeTitle ();
		}
		else
		{
			action = -1;
			if(CONTROLLER.currentInnings == 1)
			{
				Scoreboard.instance.HideStrip (false);
				Scoreboard.instance.TargetToWin ();
			}
			else
			{
				Scoreboard.instance.HideStrip (true);
			}
		}
	}
	public void UpdateCurrentBall (int validBall, int canCountBall, int runsScored, int extraRun, int batsmanID, int isWicket, int wicketType, int bowlerID, int catcherID, int batsmanOut, bool isBoundary)
	{
		if(gameQuit == true)
		{
			return;
		}
        if (CONTROLLER.gameMode == "slogover" && !PlayerPrefs.HasKey("slogovermatchid"))
        {
            DBTracking.instance.GetSlogModeMatchID();
        }

        if (CanShowTutorial())
		{
            CONTROLLER.TutorialShowCount++;
            PlayerPrefs.SetInt("tutcount", CONTROLLER.TutorialShowCount);
        }
        int batsmanIndex = (int)CONTROLLER.PlayingTeam[batsmanID];

		if(validBall == 1)
		{
			currentBall += 1;
			CONTROLLER.currentBallNumber++;
			CONTROLLER.currentMatchBalls++;
		}

		if(isWicket ==1 && currentBall == 5 )
		{
			AnimationScreen .instance.isLastBallWicket = true;
		}

        CONTROLLER.currentMatchScores += runsScored;
		ScoreStr = CONTROLLER.currentMatchScores + "/" + CONTROLLER.currentMatchWickets;
		OversStr = GetOverStr () + " (" + CONTROLLER.totalOvers + ")";
		// 26march
		if (currentBall < CONTROLLER.totalBallInOver)
		{
			CONTROLLER.ballUpdate[currentBall] = "" + runsScored;
            CONTROLLER.userBallbyBallData[currentBall] = "" + runsScored;
        }
		if(isWicket == 1)
		{
			CONTROLLER.TeamList[0].PlayerList[batsmanIndex].RunsScored += runsScored;
			if(canCountBall == 1)
			{
				CONTROLLER.TeamList[0].PlayerList[batsmanIndex].BallsPlayed++;
			}
			CONTROLLER.continousBoundaries = 0;
			CONTROLLER.continousSixes = 0;
			CONTROLLER.currentMatchWickets++;
			CONTROLLER.ballUpdate[currentBall] = "W";
            CONTROLLER.userBallbyBallData[currentBall] = "W";
            WicketBall(validBall, batsmanID, wicketType, bowlerID, catcherID, batsmanOut);
			if(CONTROLLER.gameMode !="multiplayer")
				CONTROLLER.totalPoints += CONTROLLER.wicketPoint;
			ScoreStr = CONTROLLER.currentMatchScores + "/" + CONTROLLER.currentMatchWickets;
            AudioPlayer.instance.StopBallTravelSound();
            InitAnimation(5);
			BattingScoreCard.instance.UpdateScoreCard ();

		}
		else if (isBoundary == true)
		{
			if(runsScored == 4)
			{
				CONTROLLER.continousBoundaries++;
				CONTROLLER.totalFours++;
				CONTROLLER.continousSixes = 0;
				CONTROLLER.totalPoints += CONTROLLER.boundaryPoint;
				CONTROLLER.TeamList[0].PlayerList[batsmanIndex].Fours++;
                //InitAnimation (3);
                AnimationCompleted();
            }
            else if(runsScored == 6)
			{
				CONTROLLER.continousBoundaries = 0;
				CONTROLLER.continousSixes++;
				CONTROLLER.totalSixes++;
				CONTROLLER.totalPoints += CONTROLLER.sixPoint;
				CONTROLLER.TeamList[0].PlayerList[batsmanIndex].Sixes++;
                //InitAnimation (4);
                AnimationCompleted();
                SixDistanceDisplayer.instance.show (false );		//gopi--for hiding six distance meter
			}
			CONTROLLER.TeamList[0].PlayerList[batsmanIndex].RunsScored += runsScored;
			if(canCountBall == 1)
			{
				CONTROLLER.TeamList[0].PlayerList[batsmanIndex].BallsPlayed++;
			}
			BattingScoreCard.instance.UpdateScoreCard ();
		}
		else
		{
			CONTROLLER.continousBoundaries = 0;
			CONTROLLER.continousSixes = 0;
			CONTROLLER.TeamList[0].PlayerList[batsmanIndex].RunsScored += runsScored;
			if(canCountBall == 1)
			{
				CONTROLLER.TeamList[0].PlayerList[batsmanIndex].BallsPlayed++;
			}
			BattingScoreCard.instance.UpdateScoreCard ();
			
            AudioPlayer.instance.StopBallTravelSound(0.5f);
			
			if(runsScored == 1)
			{
                CONTROLLER.totalPoints += CONTROLLER.singlePoint;
				InitAnimation (0);
			}
			else if (runsScored == 2)
			{
                CONTROLLER.totalPoints += CONTROLLER.doublePoint;
				InitAnimation (1);
			}
			else if (runsScored == 3)
			{
                InitAnimation(2);
			}
			else
			{
				CONTROLLER.continousBoundaries = 0;
				CONTROLLER.continousSixes = 0;
				if(CONTROLLER .gameMode !="multiplayer")
					CONTROLLER.totalPoints += CONTROLLER.dotBallPoint;
				CheckForOverComplete ();
			}
		}
		Scoreboard.instance.UpdateScoreCard ();
		if(runsScored == 1 || runsScored == 3)
		{
			int  temp = CONTROLLER.StrikerIndex;
			CONTROLLER.StrikerIndex = CONTROLLER.NonStrikerIndex;
			CONTROLLER.NonStrikerIndex = temp;
		}

        if (CONTROLLER.gameMode == "multiplayer")
        {
			MultiplayerManager.Instance.multiplayerGroundUiHandlerScript.UpdateScoreCard();
            SendScoreToServer(CONTROLLER.currentMatchScores, CONTROLLER.userBallbyBallData[MultiplayerManager.Instance.userBallIndex], CONTROLLER.currentMatchWickets);
            MultiplayerManager.Instance.userBallIndex++;
        }
    }


	private void SendScoreToServer(int totalscore,string currentrunscored,int currentmatchwicket)
	{
        if (PhotonNetwork.NetworkClientState == ClientState.Joined && MultiplayerManager.Instance.IsInternetOn())
        {
            MultiplayerManager.Instance.UpdateMyScorePushStatus(totalscore, currentrunscored, currentmatchwicket);
        }
    }

	public void ScoreSyncedAndMoveToNextBall()
	{
		if (CONTROLLER.gameMode == "multiplayer" && MultiplayerManager.Instance.isConnectedWithPhoton())
		{
			int _msi = MultiplayerManager.Instance.GetPhotonHashInt(RoomVariables.masterScorePushStatus, -1);
			int _csi = MultiplayerManager.Instance.GetPhotonHashInt(RoomVariables.clientScorePushStatus, -1);

			//DebugLogger.PrintWithColor("ScoreSynced and moved to next ball:::: " + _msi + " :: _csi::: " + _csi);
			if (_msi == 1 && _csi == 1)
			{
				MultiplayerManager.Instance.ResetMyScorePushStatus();
                if ( !CheckForInningsComplete())
				{
					BowlNextBall();
				}
				else
				{
					MultiplayerManager.Instance.multiplayerGroundUiHandlerScript.UpdateGameOverScreen();
                }
			}
		}
	}


    public void PlayCommentarySound (string  SoundType )
	{
		if(gameQuit == true)
		{
			return;
		}
	}

	public void PlayGameSound (string  SoundType )
	{
		if(gameQuit == true)
		{
			return;
		}
		AudioPlayer.instance.PlayGameSnd(SoundType);
	}
	
	
	private void WicketBall (int validBall, int batsmanID, int wicketType, int bowlerID, int catcherID, int batsmanOut)
	{
		CanPauseGame = false;
		if (Scoreboard.instance.muteBtn.transform.parent.gameObject.activeSelf)
			Scoreboard.instance.HidePause (true);

		NewBatsmanIndex++;
		if(batsmanOut == CONTROLLER.StrikerIndex)
		{
			CONTROLLER.StrikerIndex = NewBatsmanIndex;
		}
		else
		{
			CONTROLLER.NonStrikerIndex = NewBatsmanIndex;
		}
		batsmanOutIndex = batsmanOut;
		IsWicketBall = true;
		if(validBall == 1)
		{
			CONTROLLER.ballUpdate[currentBall] = "W";
            CONTROLLER.userBallbyBallData[currentBall] = "W";
        }
        BattingScoreCard.instance.UpdateWicket (batsmanOutIndex);
	}
	private bool SuccessfulChase = false;

	

	public void CheckForOverComplete ()
	{
		UpdateBallCompleteResult();
		if (CONTROLLER.gameMode != "superover")
		{
			inningsCompleted = CheckForInningsComplete ();
			if(inningsCompleted == true)
			{
				if (CONTROLLER.gameMode == "slogover")
                {
                    DBTracking.instance.SuperSlogLevelCompletion(CONTROLLER.currentMatchScores.ToString(), CONTROLLER.currentMatchWickets.ToString(),GetOverStr());
                }

                userAction = 0;
				CONTROLLER.CurrentPage = "";
				GroundController.instance.fieldRestriction = true;
				CONTROLLER.fielderChangeIndex = 1;
				CONTROLLER.computerFielderChangeIndex = 0;
				Scoreboard.instance.Hide (true);
				BatsmanInfo.instance.HideMe ();
				PreviewScreen.instance.Hide (true);
				if(CONTROLLER.gameMode != "multiplayer")
				{
					CONTROLLER.InningsCompleted = true;
					CanPauseGame = false;
				}
				CONTROLLER.NewInnings = true;
				if (CONTROLLER.gameMode != "chasetarget")
					NewOver();
				GroundController.instance.ResetAll ();	

               UpdateChaseTargetLevel ();
            }
            else if(currentBall == CONTROLLER.totalBallInOver-1)
			{
				overCompleted = true;
				int  temp = CONTROLLER.StrikerIndex;
				CONTROLLER.StrikerIndex = CONTROLLER.NonStrikerIndex;
				CONTROLLER.NonStrikerIndex = temp;
				ChangeBowlerType ();
				if (CONTROLLER.gameMode !="multiplayer")
				{
					BowlNextBall ();
				}					
				NewOver ();
				DetailsSavedBallbyBall ();
			}
			else
			{ 
				if (CONTROLLER.gameMode !="multiplayer")
					BowlNextBall ();
			}
		}
		else
		{
			levelCompleted = CheckCurrentLevelCompleted ();
			if (levelCompleted == true)
			{
				userAction = 0;
				CONTROLLER.CurrentPage = "";
				GroundController.instance.fieldRestriction = true;
				CONTROLLER.fielderChangeIndex = 1;
				CONTROLLER.computerFielderChangeIndex = 0;
				Scoreboard.instance.Hide (true);
				BatsmanInfo.instance.HideMe ();
				PreviewScreen.instance.Hide (true);
				CONTROLLER.InningsCompleted = true;
				CanPauseGame = false;
				CONTROLLER.NewInnings = true;
				GroundController.instance.ResetAll ();	//GroundController.instance.ResetAll ();
				UpdateLevelId ();
				ResetCurrentMatchDetails ();
				//DisplayFullScreenAd ();
			}
			else if(currentBall == (CONTROLLER.totalBallInOver - 1) || CONTROLLER.currentMatchWickets >= CONTROLLER.totalWickets)
			{
				if (CONTROLLER.currentMatchWickets == CONTROLLER.totalWickets && PlayerPrefs .GetInt ("SOwicketGainUsed")==0 && AdIntegrate.instance.checkTheInternet() && AdIntegrate .instance .isRewardedReadyToPlay ())	
				{					
					GameModel.isGamePaused = true;
					AdIntegrate.instance.SetTimeScale(0f);
					BowlNextBall();
					GroundController.instance.ChangePlayerLeftRightTextures ();
                    if (PlayerPrefs.HasKey("SuperOverDetail") && CONTROLLER.gameMode == "superover")
                    {
                        PlayerPrefs.DeleteKey("SuperOverDetail");
                        PlayerPrefs.DeleteKey("superoverPlayerDetails");
                    }

                    ProgressBar.instance.setProgress (); 

				}
				else
				{
					if (CONTROLLER.CurrentLevelCompleted <= CONTROLLER.LevelId)
					{
						CONTROLLER.LevelFailed = 1; 
					}
					SaveSOLevelDetails ();
					SaveSoFailedLevelDetails ();
					int  swapStriker = CONTROLLER.StrikerIndex;
					CONTROLLER.StrikerIndex = CONTROLLER.NonStrikerIndex;
					CONTROLLER.NonStrikerIndex = swapStriker;
					if (PlayerPrefs.HasKey ("SuperOverDetail") && CONTROLLER.gameMode == "superover")
					{
						PlayerPrefs.DeleteKey ("SuperOverDetail");
						PlayerPrefs.DeleteKey ("superoverPlayerDetails");
					}
					ResetVariables ();
					NewOver ();
					GroundController.instance.action = 10;
					ShowSuperOverResult ();
					ResetCurrentMatchDetails ();
				}
			}
			else
			{
				if (CONTROLLER.gameMode !="multiplayer")
					BowlNextBall ();
			}
		}
		GroundController.instance.ChangePlayerLeftRightTextures ();//25march
	}



	public  void ShowSuperOverResult ()
	{
		GameObject prefabGO;
		GameObject tempGO;
		prefabGO = Resources.Load ("Prefabs/SuperOverResult")as GameObject;
		tempGO = Instantiate (prefabGO) as GameObject;
		tempGO.name = "SuperOverResult";
		Scoreboard.instance.Hide (true);
		BatsmanInfo.instance.HideMe ();
		PreviewScreen.instance.Hide (true);
        ShowShotTutorial(false);
    }

    private void  UpdateLevelId ()
	{
		if(CONTROLLER .LevelId <18)
		{
			DBTracking.instance.SuperOverLevelCompletion(1);
			if (CONTROLLER.LevelCompletedArray[CONTROLLER.LevelId] == 0)
			{
                WriteSuperOverEachLevelCompData ();
				//CONTROLLER.LevelCompletedArray[CONTROLLER.CurrentLevelCompleted] = 1;
				CONTROLLER.CurrentLevelCompleted++;
				CONTROLLER.LevelFailed = 0;
				SaveSOLevelDetails ();
			}
			if (PlayerPrefs.HasKey ("SuperOverDetail") && CONTROLLER.gameMode == "superover")
			{
				PlayerPrefs.DeleteKey ("SuperOverDetail");
				PlayerPrefs.DeleteKey ("superoverPlayerDetails");
			}
			for (int i = 0; i < CONTROLLER.TeamList.Length; i++)
			{
				for (int j = 0; j < CONTROLLER.TeamList[0].PlayerList.Length; j++)
				{
					CONTROLLER.TeamList[i].PlayerList[j].RunsScored = 0;
					CONTROLLER.TeamList[i].PlayerList[j].BallsPlayed = 0;
					CONTROLLER.TeamList[i].PlayerList[j].Fours = 0;
					CONTROLLER.TeamList[i].PlayerList[j].Sixes = 0;
				}
			}
			BattingScoreCard.instance.ResetBattingCard ();
			NewOver ();
			ShowSuperOverResult ();
		}
	}

	public void WriteSuperOverEachLevelCompData()
	{
		string tmp = PlayerPrefs.GetString ("SuperOverCompletedLevel");
		int[] array = tmp.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
		array [CONTROLLER.LevelId] = 1;
		CONTROLLER.LevelCompletedArray = array;		// [CONTROLLER.CurrentLevelCompleted] = 1;
		string tmp2=string .Empty ;
		for(int i=0;i<array .GetLength (0);i++)
		{
			if(i+1<array .GetLength (0))
				tmp2+=array[i]+"-";
			else
				tmp2+=array[i];
		}

		PlayerPrefs.SetString ("SuperOverCompletedLevel", tmp2); 

	}
	public  void SaveSOLevelDetails ()
	{
		string levelDetailsStr = "";
		levelDetailsStr += CONTROLLER.CurrentLevelCompleted + "|";
		levelDetailsStr += CONTROLLER.LevelFailed + "|";
		levelDetailsStr += CONTROLLER.LevelId + "|";
		PlayerPrefs.SetString ("SuperOverLevelDetail", levelDetailsStr);
	}

	public void SaveSoFailedLevelDetails()
	{
        //gopi  v1.1.2 for saving level failed array 
        DBTracking.instance.SuperOverLevelCompletion(0);

        if (!PlayerPrefs .HasKey ("SoRVlevID"))
		{
			string tmp = PlayerPrefs.GetString ("SoLevFailedDet");
			int[] array = tmp.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
			array [CONTROLLER.LevelId] = 1;			
			string tmp2=string .Empty ;
			for(int i=0;i<array .GetLength (0);i++)
			{
				if(i+1<array .GetLength (0))
					tmp2+=array[i]+"-";
				else
					tmp2+=array[i];
			}
			PlayerPrefs.SetString ("SoLevFailedDet", tmp2);		
		}
	}

	public void ShowGameOverScreen ()
	{
		GameObject prefabGO;
		GameObject tempGO;
		prefabGO = Resources.Load ("Prefabs/GameOver")as GameObject ;
		tempGO = Instantiate (prefabGO);
		tempGO.name = "GameOver";
        ShowShotTutorial(false);
    }

	public void UpdateChaseTargetLevel()
	{
		if (CONTROLLER.gameMode == "chasetarget")
		{
			if (CONTROLLER.CTSubLevelCompleted < CONTROLLER.SubLevelCompletedArray.Length && CONTROLLER.CTLevelCompleted < CONTROLLER.MainLevelCompletedArray.Length)
			{
				CurrentSubLevel = CONTROLLER.CTSubLevelCompleted;

				DBTracking.instance.SuperChaseLevelCompletion(SuccessfulChase == true ? 1 : 0);
				if (SuccessfulChase == true && CONTROLLER.SubLevelCompletedArray[CONTROLLER.CTSubLevelCompleted] == 0 && CONTROLLER.MainLevelCompletedArray[CONTROLLER.CTLevelCompleted] == 0)
				{
					SuccessfulChase = false;
					WriteSuperChaseEachSubLevelCompData();
					//CONTROLLER.SubLevelCompletedArray[CONTROLLER.CTSubLevelCompleted] = 1;
					CONTROLLER.CTSubLevelCompleted++;


					if (PlayerPrefs.GetString("SuperChaseSubLevCompData").Equals("1-1-1-1-1"))          //(CONTROLLER.CTSubLevelCompleted == 5)
					{
						CONTROLLER.MainLevelCompletedArray[CONTROLLER.CTLevelCompleted] = 1;
						if (CONTROLLER.CTLevelCompleted < (NamesToDisplayInGoogle.Length - 1))
						{
							CONTROLLER.CTLevelCompleted++;
							CONTROLLER.CTSubLevelCompleted = 0;
							PlayerPrefs.SetString("SuperChaseSubLevCompData", "0-0-0-0-0");
						}
						else
						{
							CONTROLLER.CTSubLevelCompleted = 5;
							PlayerPrefs.SetString("SuperChaseSubLevCompData", "1-1-1-1-1");
						}
					}
					string levelDetailsStr = "";
					levelDetailsStr += CONTROLLER.CTLevelCompleted + "|";
					levelDetailsStr += CONTROLLER.CTSubLevelCompleted + "|";
					PlayerPrefs.SetString("ChaseTargetLevelDetail", levelDetailsStr);
				}
			}
			NewOver();
		}
        ClearLevelDetails();
	}


	public void WriteSuperChaseEachSubLevelCompData()
	{
		string tmp = PlayerPrefs.GetString ("SuperChaseSubLevCompData");
		int[] array = tmp.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
		array [CONTROLLER.CTSubLevelId] = 1;
		CONTROLLER.SubLevelCompletedArray = array;		
		string tmp2=string .Empty ;
		for(int i=0;i<array .GetLength (0);i++)
		{
			if(i+1<array .GetLength (0))
				tmp2+=array[i]+"-";
			else
				tmp2+=array[i];
		}
		PlayerPrefs.SetString ("SuperChaseSubLevCompData", tmp2); 
	}

	public int  GetCurrentSubLevel ()
	{
		return CurrentSubLevel;
	}
	public void SendPointsToLeaderBoard ()
	{
		if (CONTROLLER.gameMode == "slogover")
		{
//			UnityPHPConnector.instance.SendPointsToPHP (CONTROLLER.totalPoints);
		}
		else if (CONTROLLER.gameMode == "chasetarget")
		{
			//CONTROLLER.totalPoints = CalculatePointsForMatch ();
//			UnityPHPConnector.instance.SendPointsToPHP (CONTROLLER.totalPoints);
		}
	}

	//This is only for ChaseTheTarget
	private int CalculatePointsForMatch ()
	{
		int  pointValue = 0;
		pointValue = (CONTROLLER.currentMatchScores / CONTROLLER.currentMatchBalls) * 100 * CONTROLLER.currentMatchBalls;
		return pointValue;
	}

	public void ClearLevelDetails ()
	{
		CONTROLLER.TempPoint = CONTROLLER.totalPoints;
		if(PlayerPrefs.HasKey ("SlogOverDetail") && CONTROLLER.gameMode == "slogover")
		{
			PlayerPrefs.DeleteKey ("SlogOverDetail");
			PlayerPrefs.DeleteKey ("slogoverPlayerDetails");
            PlayerPrefs.DeleteKey("slogovermatchid");

        }
        if (PlayerPrefs.HasKey ("ChaseTargetDetail") && CONTROLLER.gameMode == "chasetarget")
		{
			PlayerPrefs.DeleteKey ("ChaseTargetDetail");
			PlayerPrefs.DeleteKey ("chasetargetPlayerDetails");
		}

        if (PlayerPrefs.HasKey(CONTROLLER.SUPER_Crusade_SavedMatchDetails) && CONTROLLER.gameMode == CONTROLLER.SUPER_Crusade_GameMode)
        {
            PlayerPrefs.DeleteKey(CONTROLLER.SUPER_Crusade_SavedMatchDetails);
            PlayerPrefs.DeleteKey(CONTROLLER.SUPER_Crusade_PlayerDetails);
        }

        CONTROLLER.BowlingEnd = "madrasclub";
	}

	public  bool  CheckForInningsComplete () // sc, ss, scr
	{
		//if (CONTROLLER.EnableHardcodes == 1)    //hardcode-gopi
		//{
		//	CONTROLLER.currentMatchScores = CONTROLLER.TargetToChase;
		//	return true;
		//}
		int ballsBowledInOver = CONTROLLER.currentMatchBalls % 6;
		int  battingTeamScore = CONTROLLER.currentMatchScores;
        if (battingTeamScore >= CONTROLLER.TargetToChase && (CONTROLLER.gameMode == "chasetarget" || CONTROLLER.gameMode==CONTROLLER.SUPER_Crusade_GameMode) )
		{
			SuccessfulChase = true;
			return true;
		}
		else 
		{
			SuccessfulChase = false;
		}

		if(CONTROLLER.currentMatchWickets >= CONTROLLER.totalWickets)
		{
            return true;
		}

		if( (CONTROLLER.gameMode == "chasetarget" || CONTROLLER.gameMode==CONTROLLER.SUPER_Crusade_GameMode) && CONTROLLER.currentMatchBalls == CONTROLLER.totalOvers*6 && CONTROLLER.TargetToChase - 6 <= battingTeamScore && bChaseExtraBall && AdIntegrate.instance.checkTheInternet() && AdIntegrate .instance .isRewardedReadyToPlay ())
		{
			CONTROLLER.currentMatchBalls--;
			ExtraBall.instance.ShowMe ();
			bChaseExtraBall=false;
			return false;
		}
		if(CONTROLLER.currentMatchBalls >= CONTROLLER.totalOvers * 6)
		{
            return true;
		}
		return false;
	}

	private bool  CheckCurrentLevelCompleted () //so
	{
		if (CONTROLLER.EnableHardcodes == 1)	//hardcode-gopi
		{
			return true; 
		}
		if (CONTROLLER.LevelId == 0 || CONTROLLER.LevelId == 1)
		{
			if (CONTROLLER.currentMatchScores >= 10)
			{
				return true;
			}
		}
		else if (CONTROLLER.LevelId == 2 || CONTROLLER.LevelId == 3)
		{
			if (CONTROLLER.totalFours == 3)
			{
				return true;
			}
		}
		else if (CONTROLLER.LevelId == 4 || CONTROLLER.LevelId == 5)
		{
			if (CONTROLLER.continousBoundaries == 3)
			{
				return true;
			}
		}
		else if (CONTROLLER.LevelId == 6 || CONTROLLER.LevelId == 7)
		{
			if (CONTROLLER.totalSixes == 3)
			{
				return true;
			}
		}
		else if (CONTROLLER.LevelId == 8 || CONTROLLER.LevelId == 9)
		{
			if (CONTROLLER.totalFours == 5)
			{
				return true;
			}
		}
		else if (CONTROLLER.LevelId == 10 || CONTROLLER.LevelId == 11)
		{
			if (CONTROLLER.currentMatchScores >= 25)
			{
				return true;
			}
		}
		else if (CONTROLLER.LevelId == 12 || CONTROLLER.LevelId == 13)
		{
			if (CONTROLLER.continousSixes == 4)
			{
				return true;
			}
		}
		else if (CONTROLLER.LevelId == 14 || CONTROLLER.LevelId == 15)
		{
			if (CONTROLLER.continousBoundaries == 6)
			{
				return true;
			}
		}
		else if (CONTROLLER.LevelId == 16 || CONTROLLER.LevelId == 17)
		{
			if (CONTROLLER.continousSixes == 6)
			{
				return true;
			}
		}
		return false;
	}
	//CONTROLLER.continousBoundaries
	//CONTROLLER.continousSixes
	//CONTROLLER.totalFours
	private void  UpdateBallCompleteResult ()
	{
		int  AchievedID = -1;

		// Hatrick Fours while batting
		if(CONTROLLER.continousBoundaries == 3)
		{
			AchievedID = 1;
		}

		// Hatrick Sixes while batting
		if(CONTROLLER.continousSixes == 3)
		{
			AchievedID = 8;
		}

		// Six Fours while batting
		if(CONTROLLER.continousBoundaries == 6)
		{
			AchievedID = 13;
		}

		// Six Sixes while batting
		if(CONTROLLER.continousSixes == 6)
		{
			AchievedID = 14;
		}

		// 25 or more runs in a over while bowling
		//if(runsScoredInOver >= 2)
		if (maxRunsReached == false)
		{
			if (CONTROLLER.currentMatchScores >= 2)
			{
				maxRunsReached = true;
				AchievedID = 20;
			}
		}
		DetailsSavedBallbyBall ();
		GroundController.instance.DisableTrail ();
	}

	public  void NewOver ()
	{
        maxRunsReached = false;
		Scoreboard.instance.NewOver ();
		Scoreboard.instance.Hide (true);
		BatsmanInfo.instance.HideMe ();
		CONTROLLER.ballUpdate[0] = "";
		CONTROLLER.ballUpdate[1] = "";
		CONTROLLER.ballUpdate[2] = "";
		CONTROLLER.ballUpdate[3] = "";
		CONTROLLER.ballUpdate[4] = "";
		CONTROLLER.ballUpdate[5] = "";

		if (CONTROLLER.BowlingEnd == "madrasclub")
		{
			CONTROLLER.BowlingEnd = "pattabigate";
		}
		else
		{
			CONTROLLER.BowlingEnd = "madrasclub";
		}
		GroundController.instance.NewOver ();
		currentBall = -1; 
		CONTROLLER.currentBallNumber = 0;
		runsScoredInOver = 0;
		if (CONTROLLER.gameMode != "superover" && CONTROLLER.gameMode != "multiplayer")
		{ 
			if(!AnimationScreen .instance .isLastBallWicket && !ExtraBall.instance.holder.activeSelf)
				BattingScoreCard.instance.ShowMe ();
			else if(AnimationScreen .instance .isLastBallWicket && CheckForInningsComplete() && !ExtraBall.instance.holder.activeSelf)
				BattingScoreCard.instance.ShowMe ();
			GroundController.instance.ChangePlayerLeftRightTextures ();//31march
		}

		if (CONTROLLER.gameMode == "multiplayer")
		{
			BatsmanInfo.instance.UpdateStrikerInfo ();//29march
			BatsmanInfo.instance.ShowMe ();//29march
		}

	}

	public void UpdatePreview(Dictionary<string,object> hash)
	{
		PreviewScreen.instance.UpdatePreviewScreen (hash);
	}
	public void SendBowlingDatasToGame ()
	{
		bowlingControl = 2;
		GroundController.instance.StartBowling ();
	}
	public void InitAnimation (int  type )
	{
		CanPauseGame = false;
		Scoreboard.instance.HidePause (true);//28march
		AnimationScreen.instance.StartAnimation (type);
	}
	public void AnimationCompleted ()
	{
		CheckForOverComplete ();
		AnimationScreen.instance.resetAnimation();
	}
	// Game Pause View
	public void GamePaused(bool boolean,bool FromAppPause=false)
	{
		if (CONTROLLER.gameMode == "multiplayer")
			return;
		if(boolean == true)
		{
			PreviewScreen.instance.Hide (true);
			Scoreboard.instance.HidePause (true);
			GamePauseScreen.instance.Hide (false);
			FadeView.instance.Hide (true);
			CanPauseGame = false;
			if (AdIntegrate.instance != null && !FromAppPause)
			{
				AdIntegrate.instance.ShowBannerAd();
			}
		}
		else
		{
			PreviewScreen.instance.Hide(false);
			Scoreboard.instance.HidePause(false);
			GamePauseScreen.instance.Hide(true);
			FadeView.instance.Hide(false);
			CanPauseGame = true;
			if (AdIntegrate.instance != null)
			{
				AdIntegrate.instance.HideAd();
			}
		}
		isGamePaused = boolean;
		GroundController.instance.GameIsPaused (boolean, FromAppPause);
		if(boolean == false && gameQuit == false)
		{
			CanPauseGame = true;
			Scoreboard.instance.Hide (false);
			BatsmanInfo.instance.ShowMe ();
			CONTROLLER.CurrentPage = "ingame";
			if(AdIntegrate.instance != null )	
			{
				AdIntegrate.instance.HideAd ();
			}
		}
	}

	

/*private function CheckForAllOut ()
{
    var allout : boolean = false;

    if(NewBatsmanIndex > CONTROLLER.totalWickets)
    {
        allout = true;
    }
    return allout;
}*/

// UTILITY FUNCTIONS
/*private function GetKeyBoardInput ()
{
    if(Input.GetKeyDown(KeyCode.Escape) == true)
    {
        if(CanPauseGame == true && inningsCompleted == false && SOLevelSelectionPage.instance == null && overCompleted == false)
        {
            GamePaused (true);
        }
    }
    // Swing...
    else if(Input.GetKeyDown(KeyCode.A) == true && isGamePaused == false)
    {
        if(action == 4)
        {
            if(bowlingControl == 0)
            {
                //BowlingControls.instance.ChangeSwingParameter ();
            }
        }
    }
    // Swing...
    else if(Input.GetKeyDown(KeyCode.S) == true && isGamePaused == false)
    {
        if(action == 0)
        {
            //BattingScoreCard.instance.Hide (true);
            if(isGamePaused == false)
            {
                //ShowBowlingScoreCard ();
            }
            else
            {
                GamePaused (false);
            }
        }
        else if(action == 1)
        {
            //BowlingScoreCard.instance.Hide (true);
            if(isGamePaused == false)
            {
                //ShowScoreCard ();
            }
            else
            {
                GamePaused (false);
            }
        }
        else if(action == 4)
        {
            if(bowlingControl == 0)
            {
                bowlingControl = 1;
                //BowlingControls.instance.LockSpeed ();
            }
            else if(bowlingControl == 1)
            {
                bowlingControl = 2;
                //BowlingControls.instance.LockAngle ();
            }
        }
    }
}*/



public void enableBlocker ()
	{
		if(bowlingControl == 0)
		{
			bowlingControl = 1;
			//BowlingControls.instance.LockSpeed ();
		}
		else if(bowlingControl == 1)
		{
			bowlingControl = 0;
			//BowlingControls.instance.LockAngle ();
		}
	}

    public string GetOverStr() => $"{CONTROLLER.currentMatchBalls / 6}.{CONTROLLER.currentMatchBalls % 6}";

	// UTILITY FUNCTIONS

	public void EnableMovement (bool boolean)
	{
		if(boolean == true)
		{
			EnableShot (true);
			userAction = 10;
		}
	}

	public void DetectBatsmanMoveMouse()
	{
		if (Input.GetMouseButton(0) && isGamePaused == false && !IsPointerOverUIObject())
		{
			if (prevMousePos.x == 0 && prevMousePos.y == 0)
			{
				prevMousePos = Input.mousePosition;
			}
			Vector2 touchDeltaPosition = new Vector2((Input.mousePosition.x - prevMousePos.x), (Input.mousePosition.y - prevMousePos.y));
			prevMousePos = Input.mousePosition;
			if (touchDeltaPosition.x < 0)
			{
				MoveLeft(true);
			}
			else
			{
				MoveLeft(false);
			}
			if (touchDeltaPosition.x > 0)
			{
				MoveRight(true);
			}
			else
			{
				MoveRight(false);
			}
		}

		if (Input.GetMouseButtonUp(0) && !IsPointerOverUIObject())
		{
			MoveLeft(false);
			MoveRight(false);
		}
	}

	public void DetectBatsmanMove ()
	{
		int count = Input.touchCount;
		touchPhase = Input.touches;
		if (count > 0)
		{
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			if (touchDeltaPosition.x < 0)
			{
				MoveLeft(true);
			}
			else
			{
				MoveLeft(false);
			}
			if (touchDeltaPosition.x > 0)
			{
				MoveRight(true);
			}
			else
			{
				MoveRight(false);
			}
		}
		else
		{
			MoveLeft(false);
			MoveRight(false);
		}
	}
	public void MoveLeft (bool boolean)
	{
		GroundController.instance.MoveLeftSide (boolean);
	}
	public void MoveRight (bool boolean)
	{
		GroundController.instance.MoveRightSide (boolean);
	}

	public void EnableShot (bool boolean)
	{
		//#if UNITY_ANDROID || UNITY_IPHONE
		if(CONTROLLER.BattingTeamIndex == CONTROLLER.myTeamIndex)
		{
			if(boolean == true)
			{
				action = 5;
			}
			else
			{
				action = -1;
			}
		}
		//#endif
	}
	public void EnableShotSelection (bool boolean)
	{
		if(boolean == true)
		{
			userAction = 11;
		}
	}

	public void DetectBatsmanShotMouse ()
	{
//		if(Input.GetMouseButtonDown (0) && !IsPointerOverUIObject ())
//		{
//			Vector2 _mousePos = Input.mousePosition;
//			float  PauseExcludedArea = Screen.width - 85;
//			if(CONTROLLER.CurrentPage == "ingame" && ((_mousePos.x > PauseExcludedArea && _mousePos.y < 90) || isGamePaused == true))
//			{
//				GamePaused (true);
//				if (NewTouch == false)
//				{
//					return;
//				}
//			}
//			else
//			{
//
//			}
//		}
		if(Input.GetMouseButtonDown(0) && NewTouch == false && !IsPointerOverUIObject ())
		{
			NewTouch = true;
			isTouchEnded = false;

			TouchInitPos = Input.mousePosition;
			touchInitTime = Time.time;
		}

		if(Input.GetMouseButtonUp(0) && NewTouch == true && !IsPointerOverUIObject ())
		{
			TouchEndPos = Input.mousePosition;
			GetTheShot ();
		}
	}

	public void DetectBatsmanShot ()
	{
		int  count = Input.touchCount;
		touchPhase = Input.touches;
		if(count > 0)
		{
			Vector2 _touchPos = touchPhase[0].position;
			float PauseExcludedArea = Screen.width - 85;
			if(CONTROLLER.CurrentPage == "ingame" && ((_touchPos.x > PauseExcludedArea && _touchPos.y < 90) || isGamePaused == true))
			{
				GamePaused (true);
				if (NewTouch == false)
				{
					return;
				}
			}
			else
			{

			}
		}
		if(count > 0 && shotCompleted == false)
		{
			if((touchPhase[0].phase == TouchPhase.Began || touchPhase[0].phase == TouchPhase.Stationary) && NewTouch == false && !IsPointerOverUIObject ())
			{
				NewTouch = true;
				isTouchEnded = false;

				TouchInitPos = touchPhase[0].position;
				touchInitTime = Time.time;
			}
			if(touchPhase[0].phase == TouchPhase.Ended && isTouchEnded == false && !IsPointerOverUIObject ())
			{
				TouchEndPos = touchPhase[0].position;
				GetTheShot ();
			}
		}
	}

	public void GetTheShot ()
	{
		if (NewTouch == true && isTouchEnded == false)
		{
			NewTouch = false;
			isTouchEnded = true;

			touchInitTime = 0.0f;
			shotCompleted = true;

			float xDiff = TouchEndPos.x - TouchInitPos.x;
			float yDiff = TouchEndPos.y - TouchInitPos.y;
			if (xDiff == 0.0f && yDiff == 0.0f)
			{
				return;
			}

			NewTouch = false;
			isTouchEnded = true;

			touchInitTime = 0.0f;
			shotCompleted = true;

			int cameraType = 1; //

			float angleVar;

			angleVar = Mathf.Atan2(yDiff, xDiff) * Mathf.Rad2Deg;
			angleVar = (angleVar + 360) % 360;
			if (cameraType == 0)
			{
				if (angleVar < 36)
				{
					selectedAngle = 8;
				}
				else if (angleVar < 72)
				{
					selectedAngle = 7;
				}
				else if (angleVar < 108)
				{
					selectedAngle = 6;
				}
				else if (angleVar < 144)
				{
					selectedAngle = 5;
				}
				else if (angleVar < 180)
				{
					selectedAngle = 4;
				}
				else if (angleVar < 216)
				{
					selectedAngle = 3;
				}
				else if (angleVar < 252)
				{
					selectedAngle = 2;
				}
				else if (angleVar < 288)
				{
					selectedAngle = 1;
				}
				else if (angleVar < 324)
				{
					selectedAngle = 10;
				}
				else
				{
					selectedAngle = 9;
				}
			}
			else
			{
				if (angleVar < 36)
				{
					selectedAngle = 3;
				}
				else if (angleVar < 72)
				{
					selectedAngle = 2;
				}
				else if (angleVar < 108)
				{
					selectedAngle = 1;
				}
				else if (angleVar < 144)
				{
					selectedAngle = 10;
				}
				else if (angleVar < 180)
				{
					selectedAngle = 9;
				}
				else if (angleVar < 216)
				{
					selectedAngle = 8;
				}
				else if (angleVar < 252)
				{
					selectedAngle = 7;
				}
				else if (angleVar < 288)
				{
					selectedAngle = 6;
				}
				else if (angleVar < 324)
				{
					selectedAngle = 5;
				}
				else
				{
					selectedAngle = 4;
				}
				angleVar += 180;
			}


			ShotSelected(selectedAngle, angleVar);
		}
	}
	//30march
	/*public function GetShotSelected ()
	{
		var count : int = Input.touchCount;
		if(count > 0 && shotCompleted == false)
		{
			touchPhase = Input.touches;
			if((touchPhase[0].phase == TouchPhase.Stationary || touchPhase[0].phase == TouchPhase.Moved) && isTouchEnded == false)
			{
				TouchEndPos = touchPhase[0].position;
				GetTheShot ();
			}
		}
	}*/

	public void GetShotSelected ()
	{
		if (NewTouch == true && shotCompleted == false && inningsCompleted == false)
		{
			if ((CONTROLLER.TargetPlatform == "ios" || CONTROLLER.TargetPlatform == "android") && Application.isEditor == false && touchPhase.Length > 0)
			{
				touchPhase = Input.touches;
				if (isTouchEnded == false)
				{
					if (touchPhase.Length > 1)
						TouchEndPos = touchPhase[1].position;
					//	GetTheShot ();
				}
			}
			else
			{
				if (NewTouch == true && isTouchEnded == false)
				{
					TouchEndPos = Input.mousePosition;
					//	GetTheShot ();
				}
			}
		}
	}


	//30march
	public void ShotSelected (int SelectedAngle, float swipeAngle)
	{
		userAction = -1;
		GroundController.instance.ShotSelected( SelectedAngle, swipeAngle);
	}
	public void EnableRun (bool boolean)
	{
		//#if UNITY_ANDROID || UNITY_IPHONE
		if(CONTROLLER.BattingTeamIndex == CONTROLLER.myTeamIndex)
		{
			if(boolean == true)
			{
				action = 6;
			}
			else
			{
				action = -1;
			}
		}
		//#endif
	}

	public IEnumerator InitRun (bool boolean)
	{
		GroundController.instance.InitRun (boolean);

		yield return new  WaitForSeconds (1);
		GroundController.instance.InitRun (false);
	}
	public void  ConfirmQuit ()
	{
		AdIntegrate.instance.HideAd();
		StartCoroutine (GameQuitted ());
	}
	public IEnumerator GameQuitted ()
	{
		TeamSelection.playerCount = 0;
		TeamSelection.foreignPlayerCount = 0;
		gameQuit = true;
		GamePaused (false);
		CONTROLLER.InningsCompleted = false;
		GroundController.instance.fieldRestriction = true;
		CONTROLLER.fielderChangeIndex = 1;
		CONTROLLER.computerFielderChangeIndex = 0;
		ResetAllLocalVariables ();//29march
		if(AudioPlayer.instance != null)
		{
			AudioPlayer.instance.ToggleInGameSounds(false);
            AudioPlayer.instance.PlayOrStop_BGM(true);
			PlayerPrefsManager.SetSettingsList ();
		}
		LoadingScreen.instance.Show();
		yield  return new  WaitForSeconds (1);
		ManageScene.LoadScene (Scenes.MainMenu);
	}

	private void  SetOverRange ()
	{
		if (CONTROLLER.gameMode != "chasetarget")
		{
			return;
		}
		int i;
		string  overRangeStr = CONTROLLER.TargetRangeArray[CONTROLLER.CTCurrentPlayingMainLevel];
		string [] overRangeArray = overRangeStr.Split ("$" [0]);
		string [] StartRangeStr ;
		string [] EndRangeStr ;
		for (i=0; i < 5; i++)
		{
			StartRangeStr = overRangeArray[i].Split ("-" [0]);
			EndRangeStr = overRangeArray[i].Split ("-" [0]);
			CONTROLLER.StartRangeArray[i] = int.Parse(StartRangeStr[0] as string );
			CONTROLLER.EndRangeArray[i] = int.Parse(EndRangeStr[1] as string );
		}
	}

	public void ReStartGame ()
	{
		if (PlayerPrefs.HasKey("loft"))
		{
			if (PlayerPrefs.GetInt("loft") == 1)
			{
				GroundController.loft = true;
			}
			else
			{
				GroundController.loft = false;
			}
		}
		else
		{
			GroundController.loft = true;
		}
        
		//By default loft is off as per Jaysree
		GroundController.loft = false;

        GroundController.instance.SetLoftImage (); 		 
		BattingScoreCard.instance.ResetPlayerImages ();
		SetOverRange ();
		AdIntegrate.instance.SetTimeScale(1f);
		if (CONTROLLER.gameMode== "chasetarget")
		    CONTROLLER.GetRandomScoreForOppTeam (CONTROLLER.StartRangeArray[CONTROLLER.CTSubLevelId], CONTROLLER.EndRangeArray[CONTROLLER.CTSubLevelId]);
		CONTROLLER.InningsCompleted = false;//30march
		GroundController.instance.ResetAll ();
		ResetCurrentMatchDetails ();
		ResetVariables ();
		ResetAllLocalVariables ();//30march
		Scoreboard.instance.NewOver ();
		CONTROLLER.isReplayGame = true;
		GamePaused (false);
		CONTROLLER.NewInnings = true;
		BatsmanInfo.instance.UpdateStrikerInfo();//30march
		GroundController.instance.ChangePlayerLeftRightTextures ();//30march
		ShowIntroAnimation ();
		GroundController.instance.loftBtn.gameObject.SetActive (false); 

		//gopi - for removing data while user clicks replay
		if (CONTROLLER.gameMode == "superover")
		{ 
			PlayerPrefs.DeleteKey ("SuperOverDetail");
			PlayerPrefs.DeleteKey ("superoverPlayerDetails");
			PlayerPrefs.SetInt("SOwicketGainUsed", 0);
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
        else if (CONTROLLER.gameMode ==CONTROLLER.SUPER_Crusade_GameMode)
        {
            PlayerPrefs.DeleteKey(CONTROLLER.SUPER_Crusade_SavedMatchDetails);
            PlayerPrefs.DeleteKey(CONTROLLER.SUPER_Crusade_PlayerDetails);
        }
        else if (CONTROLLER.gameMode == "multiplayer")
		{
			MultiplayerManager.Instance.ResetMyStatus ();
		}
            GroundController.instance.moveBannerTextureScript.Reset();
	}

	public int getInstalledBuildNumber()
	{
		return PlayerPrefs .GetInt ("InstalledBuildNumber");
	}

	public void ShowSettingsPage ()
	{
		GameObject prefabGO;
		GameObject tempGO;
		prefabGO = Resources.Load ("Prefabs/Settings") as GameObject;
		tempGO = Instantiate (prefabGO) as GameObject;
		tempGO.name = "Settings";
		tempGO.transform.localPosition = new Vector3(0,0,1);
	}

	public void ShowInstructionPage ()
	{
		GameObject prefabGO;
		GameObject tempGO;
		prefabGO = Resources.Load ("Prefabs/ModeInstruction") as GameObject;
		tempGO = Instantiate (prefabGO) as GameObject;
		tempGO.name = "ModeInstruction";
		tempGO.transform.localPosition = new Vector3(0,0,1);
	}

	private void GetPlayingTeam ()
	{
		if (CONTROLLER.gameMode != "multiplayer") {
		CONTROLLER.PlayingTeam = new ArrayList ();
			for (int i = 0; i < CONTROLLER.TeamList.Length; i++) {
				for (int j = 0; j < CONTROLLER.TeamList [i].PlayerList.Length; j++) {
					if (CONTROLLER.TeamList [i].PlayerList [j].DefaultPlayer == "1") {
					CONTROLLER.PlayingTeam.Add(j);
					}
				}
			}
		}
	}

	public  void ResetAllLocalVariables ()//29march
	{
		CanPauseGame = false;
		IntroOpeners = false;
		runsScoredInOver = 0;
		IsWicketBall = false;
		//batsmanOutIndex = -1;
		inningsCompleted = false;
		levelCompleted = false;
		action = -1;
		WicketsInOver = 0;
		CanShowBattingScoreCard = false;

		CONTROLLER.totalSixes = 0;
		CONTROLLER.totalFours = 0;
		CONTROLLER.continousBoundaries = 0;
		CONTROLLER.continousSixes = 0;
		CONTROLLER.totalFours = 0;
		CONTROLLER.totalSixes = 0;
		CONTROLLER.continousBoundaries = 0;
		CONTROLLER.continousSixes = 0;
		CONTROLLER.SixDistance = 0;

		maxWidesReached = false;
		CONTROLLER.isReplayGame = false;
		////////////
		currentBall = -1;  
		CONTROLLER.currentMatchBalls = 0;
        CONTROLLER.currentMatchScores = 0;
		CONTROLLER.currentMatchWickets = 0;
		CONTROLLER.currentBallNumber = 0;
		CONTROLLER.totalPoints = 0;
		NewBatsmanIndex = 1;
		CONTROLLER.StrikerIndex = 0;
		CONTROLLER.NonStrikerIndex = 1;
		for (int i = 0; i < CONTROLLER.TeamList.Length; i++)
		{
			for (int j = 0; j < CONTROLLER.TeamList[0].PlayerList.Length; j++)
			{
				CONTROLLER.TeamList[i].PlayerList[j].RunsScored = 0;
				CONTROLLER.TeamList[i].PlayerList[j].BallsPlayed = 0;
				CONTROLLER.TeamList[i].PlayerList[j].Fours = 0;
				CONTROLLER.TeamList[i].PlayerList[j].Sixes = 0;
				CONTROLLER.TeamList[i].PlayerList[j].status = "";
			}
		}
		if(CONTROLLER .gameMode !="multiplayer")
			BattingScoreCard.instance.ResetBattingCard ();
		//BatsmanInfo.instance.UpdateStrikerInfo ();
	}

	private void  SaveinGoogleAnalytics ( bool continuing  )
	{
		if(continuing == false)
		{
			for(var i = 0; i < CONTROLLER.PlayingTeam.Count; i++)
			{
				int _batsmanIndex =(int ) CONTROLLER.PlayingTeam[i];
				string  playerName = "Player_"+CONTROLLER.TeamList[0].PlayerList[_batsmanIndex].ShortName;
				/*if(GoogleAnalyticsBinder.instance != null)
				{
					GoogleAnalyticsBinder.instance.PostEvent (playerName);
				}*/
			}
			/*if (GoogleAnalyticsBinder.instance != null)
			{
				GoogleAnalyticsBinder.instance.PostEvent ("CSK_Anthem_"+CONTROLLER.BGMLanguage);
			}*/
			if (CONTROLLER.gameMode == "slogover")
			{
				/*if (GoogleAnalyticsBinder.instance != null)
				{
					GoogleAnalyticsBinder.instance.PostEvent ("SuperSlog_Entered");
				}*/
			}
		}
	}

	//April2 - Called from CTLevelSelectionPage and GameModel
	public void  PostSuperChaseGoogleAnalyticsEvents (string  str)
	{
		/*if (GoogleAnalyticsBinder.instance != null)
		{
			GoogleAnalyticsBinder.instance.PostEvent (str);
		}*/
	}

   
}
