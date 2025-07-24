using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattingScoreCard : Singleton<BattingScoreCard> 
{

	public GameObject wicketCamera;
	public static bool newGame, canSwap;
	public static int firstPlayer, secondPlayer;
	public static int swap = 0;
	//public static BattingScoreCard instance  ;
	public GameObject scoreCard, playerName, arrow, moveUp, moveDown, prev, next, instText;
	public GameObject[] tweenPos;
	public Sprite Highlighted, Normal, Selected;
	public Sprite NotOut, Out;
	public Image player1, playerImage;
	public Text player1Text, playerText;
	public GameObject swapButton, changeLineupButton, playerChange;
	public PlayerDetails[] batsman;
	//public GameObject startScreen;
	public Text OversText;
	public Text ScoreText;
	private int tempRank = 0, playerIndex;
	private Camera renderCamera  ;
	private float[] yPos = new float[]{290f,240f,190f,140f,90f};
	public bool  FirstTimeContinueClicked = false;
	public Image[] showPlayers, showPlayerShadow;
	public Text[] showPlayersText;
    public Text CSKUIText;
    public Text VSoppUIText;
    public GameObject scoreCardTitle;
    public string[] oppTeamName = 
    {
        "PUN",  "PUNJAB",

        "MUM",  "MUMBAI",

        "KOL",  "KOLKATA",

        "BAN",  "BANGALORE",

        "DEL",  "DELHI",

        "RAJ",  "RAJASTHAN",

        "DEC",  "DECCAN",

        "KOC",  "KOCHI",

        "PNE",  "PUNE",

        "HYD",  "HYDERABAD"
    };

    protected void  Awake ()
	{
		//instance = this;
		renderCamera = GameObject.Find ("MainCamera").GetComponent<Camera>();
	}

	protected void Start ()
	{
		playerChange.SetActive (false); 

		swapButton.SetActive (false);
		firstPlayer = -1;
		secondPlayer = -1;
		if (GameModel.instance.ContinueMatch == true)
			newGame = false;
		else
			newGame = true;

		//CricMini-Gopi
		changeLineupButton.SetActive(false);

		//ChangeOrder (3, 4);
		//1April


		//1April
//		yield WaitForSeconds (0.01);
	}

	public void setWicketCamera(bool flag)
	{
		if(flag )
		{
			wicketCamera.SetActive (true);
			if(CameraMove.instance!=null)
				CameraMove.instance.CameraEnable ();
		}
		else
		{
			if(CameraMove.instance!=null)
				CameraMove.instance.CameraDisable ();
			wicketCamera.SetActive (false);
		}
	}

	//CricMini-Gopi
	/*private void Update() 
	{
		if (firstPlayer != -1) 
		{
			foreach (Sprite sprite in BatsmanInfo.instance.images) {
				if (sprite.name == batsman[firstPlayer].Name.text) {
					player1Text.gameObject.SetActive (false);
					player1.sprite = sprite;

				}
			} 
			moveUp.SetActive (true);
			moveDown.SetActive (true);
			arrow.SetActive (false);
			playerName.SetActive (true);
			instText.SetActive (true);
            player1.gameObject.SetActive(true);
		}

		if (firstPlayer == -1) {
			player1Text.gameObject.SetActive (true);
			player1.sprite = null;
			playerName.SetActive (false);
			moveUp.SetActive (false);
			moveDown.SetActive (false);
			instText.SetActive (false);
			arrow.SetActive (true);
            player1.gameObject.SetActive(false);
		}

		if (playerChange.activeInHierarchy) {
			if (CONTROLLER.currentMatchWickets == 9) {
				prev.GetComponent<Button> ().interactable = false;	
				next.GetComponent<Button>().interactable = false;	
			} else if (ScrollSnapRect.instance._currentPage == 0) {
				//prev.SetActive (false);
				prev.GetComponent<Button>().interactable = false;	
				next.GetComponent<Button>().interactable = true;	
			} else if (ScrollSnapRect.instance._currentPage == (9 - CONTROLLER.currentMatchWickets)) {
				prev.GetComponent<Button>().interactable = true;	
				next.GetComponent<Button>().interactable = false;	
			} else {
				prev.GetComponent<Button>().interactable = true ;	
				next.GetComponent<Button>().interactable = true ;	
			}
		}
		if (firstPlayer == 0) {
			moveUp.SetActive (false);
			moveDown.SetActive (true);
		} else if (firstPlayer == 10) {
			moveDown.SetActive (false);
			moveUp.SetActive (true);
		} else if (firstPlayer != -1) {
			moveUp.SetActive (true);
			moveDown.SetActive (true);
		}
		

		if (!newGame)
			changeLineupButton.SetActive (false);
	}
	*/
	/*private void  AlignContinueButton ()
	{
		Vector2 screenPos = new Vector2 (Screen.width, Screen.height);
		Vector3 newPos  = new Vector3 (0,0,0);
		newPos = renderCamera.ScreenToWorldPoint (new Vector3 (screenPos.x, screenPos.y, -0.02f));
		ContinueBtn.transform.position=new Vector3 ( newPos.x - 10f,ContinueBtn.transform.position.y,ContinueBtn.transform.position.z) ;
		ContinueBtn.transform.position =new Vector3 (ContinueBtn.transform.position.x, -newPos.y + 10f,ContinueBtn.transform.position.z) ;
		//25march
		BackBtn.transform.position =new Vector3 ( newPos.x - 10,BackBtn.transform.position.y,BackBtn.transform.position.z ) ;
		BackBtn.transform.position =new Vector3 (BackBtn.transform.position.x, -newPos.y + 10 ,BackBtn.transform.position.z ) ;
		if (GameModel.isGamePaused == true)
		{
			ContinueBtn.Hide (true);
			BackBtn.Hide (false);
		}
		else
		{
			ContinueBtn.Hide (false);
			BackBtn.Hide (true);
		}
		//25march
	}*/

	/*public void  addEventListener ()
	{
		ContinueBtn.SetValueChangedDelegate (CloseButtonClicked);
		BackBtn.SetValueChangedDelegate (CloseButtonClicked);//25march
		for (int  i= 0; i < batsman.Length ; i++)
		{
			batsman[i].Highlight.controlIsEnabled = false;
		}
	}*/

	public void  ResetBattingCard ()
	{
		int i;
		for(i = 0; i < batsman.Length; i++)
		{
			//batsman[i].Highlight.SetToggleState ("normal");
			batsman[i].GetComponent<Image>().sprite = Normal;
			batsman [0].Status.enabled = true;
			batsman [0].Status.sprite = NotOut;
			batsman[i].Name.text = "";
            //batsman [i].Name.fontStyle = FontStyle.Normal;
            //batsman[i].Runs.fontStyle = FontStyle.Normal;
            //batsman[i].Balls.fontStyle = FontStyle.Normal;
            //batsman[i].Fours.fontStyle = FontStyle.Normal;
            //batsman[i].Sixes.fontStyle = FontStyle.Normal;

            batsman[i].Name.color = new Color32(31,31,31,255);
            batsman[i].Runs.color = new Color32(31, 31, 31, 255);
            batsman[i].Balls.color = new Color32(31, 31, 31, 255);
            batsman[i].Fours.color = new Color32(31, 31, 31, 255);
            batsman[i].Sixes.color = new Color32(31, 31, 31, 255);

            batsman[i].Status.enabled = false;
			//batsman[i].Status.controlIsEnabled = false;
			batsman[i].Runs.text = "";
			batsman[i].Balls.text = "";
			batsman[i].Fours.text = "";
			batsman[i].Sixes.text = "";
		}
		batsman [0].Status.enabled = true; 
		batsman[1].Status.enabled = true; 
		batsman[0].GetComponent<Image>().sprite = Highlighted;
		batsman[1].GetComponent<Image>().sprite = Highlighted;
        //batsman [0].Name.fontStyle = FontStyle.Bold;
        //batsman[0].Runs.fontStyle = FontStyle.Bold;
        //batsman[0].Balls.fontStyle = FontStyle.Bold;
        //batsman[0].Fours.fontStyle = FontStyle.Bold;
        //batsman[0].Sixes.fontStyle = FontStyle.Bold;
        //batsman [1].Name.fontStyle = FontStyle.Bold;
        //batsman[1].Runs.fontStyle = FontStyle.Bold;
        //batsman[1].Balls.fontStyle = FontStyle.Bold;
        //batsman[1].Fours.fontStyle = FontStyle.Bold;
        //batsman[1].Sixes.fontStyle = FontStyle.Bold;

        batsman[0].Name.color = Color.white;
        batsman[0].Runs.color = Color.white;
        batsman[0].Balls.color = Color.white;
        batsman[0].Fours.color = Color.white;
        batsman[0].Sixes.color = Color.white;
        batsman[1].Name.color = Color.white;
        batsman[1].Runs.color = Color.white;
        batsman[1].Balls.color = Color.white;
        batsman[1].Fours.color = Color.white;
        batsman[1].Sixes.color = Color.white;


        batsman[0].Runs.text = "0";
		batsman[0].Balls.text = "0";
		batsman[0].Fours.text = "0";
		batsman[0].Sixes.text = "0";
		batsman[1].Runs.text = "0";
		batsman[1].Balls.text = "0";
		batsman[1].Fours.text = "0";
		batsman[1].Sixes.text = "0";
		if(firstPlayer != -1) {
			batsman [firstPlayer].GetComponent<Image> ().sprite = Selected;
		}
		PlayingTeamCard ();
		ScoreText.text = GameModel.ScoreStr;
		OversText.text = GameModel.OversStr;
	}

	public void ChangeLineup() {
		canSwap = true;
		scoreCard.GetComponent<Animator> ().SetTrigger ("flip");
	}



	public void RegainWicket() 
	{
		if (CONTROLLER.gameMode == "superover") 
			PlayerPrefs.SetInt("SOwicketGainUsed", 1);	//for do not use the option again

		CONTROLLER.currentMatchWickets--;
		CONTROLLER.totalPoints -= CONTROLLER.wicketPoint;
		CONTROLLER.StrikerIndex = GameModel.instance.batsmanOutIndex;
		CONTROLLER.TeamList [0].PlayerList [CONTROLLER.StrikerIndex].BallsPlayed--;
		GameModel.instance.NewBatsmanIndex--;
		CONTROLLER.currentMatchBalls--;
		CONTROLLER.currentBallNumber--;
		ProgressBar.instance.showProgress = false;
		CONTROLLER.ballUpdate[GameModel.instance.currentBall] = "";
		GameModel.instance.currentBall--;
		GroundController.instance.ChangePlayerLeftRightTextures ();
		GameModel.ScoreStr = CONTROLLER.currentMatchScores + "/" + CONTROLLER.currentMatchWickets;
		GameModel.OversStr = GameModel.instance.GetOverStr () + " (" + CONTROLLER.totalOvers + ")";
		BatsmanInfo.instance.UpdateStrikerInfo ();
		GameModel.instance.DetailsSavedBallbyBall ();
		Scoreboard.instance.UpdateScoreCard ();
		CloseButtonClicked ();
		AdIntegrate.instance.SetTimeScale(1f);
		GamePauseScreen.instance.playerDetails.SetActive (true);  
		GamePauseScreen.instance.SetLoftState(true);
	}

	public void ResetPlayerImages() 
	{
		for (int i = 0; i <9 ; i++) 
		{
			//showPlayers [i].gameObject.SetActive (true);
			//showPlayersText [i].gameObject.SetActive (true);
			//showPlayers [i].transform.parent.gameObject.SetActive (true);
		}
	}

	public void NormalView() {
		firstPlayer = secondPlayer = -1;
		swap = 0;
		canSwap = false;
		ResetBattingCard ();
		scoreCard.GetComponent<Animator> ().SetTrigger ("normal");
	}

	public void SetIndex(int index) {
		playerIndex = index;
		firstPlayer = index;
		int playerID;
		playerID =(int) CONTROLLER.PlayingTeam[index];
		if (batsman [firstPlayer].GetComponent<Image> ().sprite != Selected) {
			batsman [firstPlayer].GetComponent<Image> ().sprite = Selected;
			playerName.GetComponentInChildren<Text>().text = CONTROLLER.TeamList[0].PlayerList[playerID].ShortName;
			ResetBattingCard();
		}
		else {
			if (playerIndex < 2)
				batsman [firstPlayer].GetComponent<Image> ().sprite = Highlighted;
			else
				batsman [firstPlayer].GetComponent<Image> ().sprite = Normal;
			firstPlayer = -1;
		}
	}

	public void MovePlayer(int index) 
	{
		if (firstPlayer != -1) {
			int playerID;
			//playerID =(int) CONTROLLER.PlayingTeam[playerIndex];
			PlayerDetails temp;
			//playerIndex = playerID;
			if (playerIndex == 0 && index == 1) {
				return;
			}
			if (playerIndex == 10 && index == -1) {
				return;
			}
			int nextPlayer = firstPlayer - index;
			CONTROLLER.PlayerInfo tempA;
			playerIndex = (int)CONTROLLER.PlayingTeam [playerIndex];
			nextPlayer = (int)CONTROLLER.PlayingTeam [nextPlayer];
			tempA = CONTROLLER.TeamList [0].PlayerList [playerIndex];
			CONTROLLER.TeamList [0].PlayerList [playerIndex] = CONTROLLER.TeamList [0].PlayerList [nextPlayer];
			CONTROLLER.TeamList [0].PlayerList [nextPlayer] = tempA;
			PlayerPrefsManager.SetTeamList ();
			firstPlayer = firstPlayer - index;
			playerIndex = firstPlayer;
			ResetBattingCard ();
			GroundController.instance.ChangePlayerLeftRightTextures ();
			/*foreach (PlayerDetails player in batsman) {
				player.GetComponent<Image> ().sprite = player.previous;
			}*/
			//AnimateBoard ();
		}
	}

	public void AnimateBoard ()
	{
		tempRank = secondPlayer;
		iTween.MoveTo(tweenPos [firstPlayer], iTween.Hash("position", new Vector3 (tweenPos[firstPlayer].gameObject.transform.localPosition.x, yPos[firstPlayer],tweenPos[firstPlayer].gameObject.transform.localPosition.z), "islocal", true, "time", 1));
		tempRank = firstPlayer;
		iTween.MoveTo(tweenPos [secondPlayer], iTween.Hash("position", new Vector3 (tweenPos[secondPlayer].gameObject.transform.localPosition.x, yPos[secondPlayer],tweenPos[secondPlayer].gameObject.transform.localPosition.z), "islocal", true, "time", 1));
	}

	private void  PlayingTeamCard ()
	{
		int  playerID ;
		int  i ;
		for(i = 0; i < CONTROLLER.PlayingTeam.Count; i++)
		{
			playerID =(int) CONTROLLER.PlayingTeam[i];
			batsman[i].Name.text = CONTROLLER.TeamList[0].PlayerList[playerID].PlayerName;
		}
	}
	public void  UpdateScoreCard ()
	{
		int  playerID ;
		int  _batsmanId ;
        /*if(CONTROLLER.StrikerIndex < CONTROLLER.PlayingTeam.Count)
		{
			playerID = CONTROLLER.StrikerIndex;
			_batsmanId =(int ) CONTROLLER.PlayingTeam[playerID];
			if(playerID >= 0 && playerID < CONTROLLER.TeamList[0].PlayerList.Length)
			{
				batsman[playerID].Status.Hide (false);
				batsman[playerID].Highlight.SetToggleState ("enable");
				batsman[playerID].Status.SetToggleState ("notout");
				batsman[playerID].Runs.Text = "" + CONTROLLER.TeamList[0].PlayerList[_batsmanId].RunsScored;
				batsman[playerID].Balls.Text = "" + CONTROLLER.TeamList[0].PlayerList[_batsmanId].BallsPlayed;
				batsman[playerID].Fours.Text = "" + CONTROLLER.TeamList[0].PlayerList[_batsmanId].Fours;
				batsman[playerID].Sixes.Text = "" + CONTROLLER.TeamList[0].PlayerList[_batsmanId].Sixes;
				CONTROLLER.TeamList[0].PlayerList[_batsmanId].status = "";
			}
		}

		if(CONTROLLER.NonStrikerIndex < CONTROLLER.PlayingTeam.Count)
		{
			playerID = CONTROLLER.NonStrikerIndex;
			_batsmanId =(int ) CONTROLLER.PlayingTeam[playerID];
			if(playerID >= 0 && playerID < CONTROLLER.TeamList[0].PlayerList.Length)
			{
				batsman[playerID].Status.Hide (false);
				batsman[playerID].Highlight.SetToggleState ("enable");
				batsman[playerID].Status.SetToggleState ("notout");
				batsman[playerID].Runs.Text = "" + CONTROLLER.TeamList[0].PlayerList[_batsmanId].RunsScored;
				batsman[playerID].Balls.Text = "" + CONTROLLER.TeamList[0].PlayerList[_batsmanId].BallsPlayed;
				batsman[playerID].Fours.Text = "" + CONTROLLER.TeamList[0].PlayerList[_batsmanId].Fours;
				batsman[playerID].Sixes.Text = "" + CONTROLLER.TeamList[0].PlayerList[_batsmanId].Sixes;
				CONTROLLER.TeamList[0].PlayerList[_batsmanId].status = "";
			}
		}*/
        int i;
        for (i = 0; i < CONTROLLER.PlayingTeam.Count; i++)
        {
            if (i > (CONTROLLER.currentMatchWickets + 1))
            {
                batsman[i].Status.enabled = true;
                batsman[i].GetComponent<Image>().sprite = Normal;
                //batsman[i].Name.fontStyle = FontStyle.Normal;
                //batsman[i].Runs.fontStyle = FontStyle.Normal;
                //batsman[i].Balls.fontStyle = FontStyle.Normal;
                //batsman[i].Fours.fontStyle = FontStyle.Normal;
                //batsman[i].Sixes.fontStyle = FontStyle.Normal;

                batsman[i].Name.color = new Color32(31, 31, 31, 255);
                batsman[i].Runs.color = new Color32(31, 31, 31, 255);
                batsman[i].Balls.color = new Color32(31, 31, 31, 255);
                batsman[i].Fours.color = new Color32(31, 31, 31, 255);
                batsman[i].Sixes.color = new Color32(31, 31, 31, 255);

                batsman[i].Status.sprite = NotOut;
                batsman[i].Runs.text = "";
                batsman[i].Balls.text = "";
                batsman[i].Fours.text = "";
                batsman[i].Sixes.text = "";
                CONTROLLER.TeamList[0].PlayerList[i].status = "";
            }
        }
        if (CONTROLLER.StrikerIndex < CONTROLLER.PlayingTeam.Count)
		{
			playerID = CONTROLLER.StrikerIndex;
			_batsmanId = (int) CONTROLLER.PlayingTeam[playerID];
			if(playerID >= 0 && playerID < CONTROLLER.TeamList[0].PlayerList.Length)
			{
				batsman[playerID].Status.enabled = true;
				batsman[playerID].GetComponent<Image>().sprite = Highlighted;
                //batsman[playerID].Name.fontStyle = FontStyle.Bold;
                //batsman[playerID].Runs.fontStyle = FontStyle.Bold;
                //batsman[playerID].Balls.fontStyle = FontStyle.Bold;
                //batsman[playerID].Fours.fontStyle = FontStyle.Bold;
                //batsman[playerID].Sixes.fontStyle = FontStyle.Bold;

                batsman[playerID].Name.color = Color.white;
                batsman[playerID].Runs.color = Color.white;
                batsman[playerID].Balls.color = Color.white;
                batsman[playerID].Fours.color = Color.white;
                batsman[playerID].Sixes.color = Color.white;

                batsman[playerID].Status.sprite = NotOut;
				batsman[playerID].Runs.text = "" + CONTROLLER.TeamList[0].PlayerList[_batsmanId].RunsScored;
				batsman[playerID].Balls.text = "" + CONTROLLER.TeamList[0].PlayerList[_batsmanId].BallsPlayed;
				batsman[playerID].Fours.text = "" + CONTROLLER.TeamList[0].PlayerList[_batsmanId].Fours;
				batsman[playerID].Sixes.text = "" + CONTROLLER.TeamList[0].PlayerList[_batsmanId].Sixes;
				CONTROLLER.TeamList[0].PlayerList[_batsmanId].status = "";
			}
		}

		if(CONTROLLER.NonStrikerIndex < CONTROLLER.PlayingTeam.Count)
		{
			playerID = CONTROLLER.NonStrikerIndex;
			_batsmanId = (int) CONTROLLER.PlayingTeam[playerID];
			if(playerID >= 0 && playerID < CONTROLLER.TeamList[0].PlayerList.Length)
			{
				batsman [playerID].Status.enabled = true;
				batsman[playerID].GetComponent<Image>().sprite = Highlighted;
                //batsman[playerID].Name.fontStyle = FontStyle.Bold;
                //batsman[playerID].Runs.fontStyle = FontStyle.Bold;
                //batsman[playerID].Balls.fontStyle = FontStyle.Bold;
                //batsman[playerID].Fours.fontStyle = FontStyle.Bold;
                //batsman[playerID].Sixes.fontStyle = FontStyle.Bold;

                batsman[playerID].Name.color = Color.white;
                batsman[playerID].Runs.color = Color.white;
                batsman[playerID].Balls.color = Color.white;
                batsman[playerID].Fours.color = Color.white;
                batsman[playerID].Sixes.color = Color.white;

                batsman[playerID].Status.sprite = NotOut;
				batsman[playerID].Runs.text = "" + CONTROLLER.TeamList[0].PlayerList[_batsmanId].RunsScored;
				batsman[playerID].Balls.text = "" + CONTROLLER.TeamList[0].PlayerList[_batsmanId].BallsPlayed;
				batsman[playerID].Fours.text = "" + CONTROLLER.TeamList[0].PlayerList[_batsmanId].Fours;
				batsman[playerID].Sixes.text = "" + CONTROLLER.TeamList[0].PlayerList[_batsmanId].Sixes;
				CONTROLLER.TeamList[0].PlayerList[_batsmanId].status = "";
			}
		}

		ScoreText.text = GameModel.ScoreStr;
		OversText.text = GameModel.OversStr;
	}

	public void DisplayNextPlayer() 
	{
		/*if (CONTROLLER.currentMatchWickets > 1) 
		{
			for (int i = CONTROLLER.currentMatchWickets; i > 1; i--) {
				showPlayers [10 - i].gameObject.SetActive (false);
				showPlayers [10 - i].transform.parent.gameObject.SetActive (false);
				showPlayersText [10 - i].gameObject.SetActive (false);
				//showPlayerShadow [10 - i].gameObject.SetActive (false);
			}
		}*/
		playerChange.SetActive (false); setWicketCamera (false ); 
		GamePauseScreen.instance.playerDetails.SetActive (false);
		//GamePauseScreen.instance.SetLoftState(false);
		//ScrollSnapRect.instance.Reset ();
		//ScrollSnap2.instance.Reset ();
		//ScrollSnapRect._pageCount = 10 - CONTROLLER.currentMatchWickets;
		//ScrollSnap2._pageCount = 10 - CONTROLLER.currentMatchWickets;
		//ScrollSnapRect.instance.reset ();
		/*for (int i = 0; i < ScrollSnapRect._pageCount; i++) {
			foreach (Sprite sprite in BatsmanInfo.instance.images) {
				if (sprite.name == batsman [CONTROLLER.currentMatchWickets + 1 + i].Name.text) {
					showPlayers [i].sprite = sprite;
					showPlayersText [i].text = sprite.name;
					//playerText.text = sprite.name;
				}
			}
		}*/

		//CricMini-Gopi
		playerChange.SetActive(false);
		ChooseNextPlayer();
	}

	public void ChooseNextPlayer() 
	{
		PlayerDetails temp;
		//playerIndex = playerID;
		/*CONTROLLER.PlayerInfo tempA;
		int chosenPlayer, swapPlayer;
		chosenPlayer = (int)CONTROLLER.PlayingTeam [CONTROLLER.currentMatchWickets + 1 + (10 - CONTROLLER.currentMatchWickets)];
		swapPlayer = (int)CONTROLLER.PlayingTeam [CONTROLLER.currentMatchWickets + 1];
		tempA = CONTROLLER.TeamList [0].PlayerList [chosenPlayer];
		CONTROLLER.TeamList [0].PlayerList [chosenPlayer] = CONTROLLER.TeamList [0].PlayerList [swapPlayer];
		CONTROLLER.TeamList [0].PlayerList [swapPlayer] = tempA;
		SavePlayerPrefs.SetTeamList ();*/
	//	ResetBattingCard ();
		GroundController.instance.ChangePlayerLeftRightTextures ();
		BatsmanInfo.instance.UpdateStrikerInfo ();
		PlayingTeamCard ();
		CloseButtonClicked ();
	}

	public void  UpdateWicket (int  playerID )
	{
		if(playerID < CONTROLLER.PlayingTeam.Count)
		{
			int _batsmanId = (int)CONTROLLER.PlayingTeam[playerID];
			//batsman[playerID].Highlight.SetToggleState ("normal");
			batsman[playerID].Name.text = "" + CONTROLLER.TeamList[0].PlayerList[_batsmanId].PlayerName;
            //batsman[playerID].Name.fontStyle = FontStyle.Normal;
            //batsman[playerID].Runs.fontStyle = FontStyle.Normal;
            //batsman[playerID].Balls.fontStyle = FontStyle.Normal;
            //batsman[playerID].Fours.fontStyle = FontStyle.Normal;
            //batsman[playerID].Sixes.fontStyle = FontStyle.Normal;

            batsman[playerID].Name.color = new Color32(31, 31, 31, 255);
            batsman[playerID].Runs.color = new Color32(31, 31, 31, 255);
            batsman[playerID].Balls.color = new Color32(31, 31, 31, 255);
            batsman[playerID].Fours.color = new Color32(31, 31, 31, 255);
            batsman[playerID].Sixes.color = new Color32(31, 31, 31, 255);

            batsman[playerID].Status.enabled = true;
			batsman[playerID].Status.sprite = Out;
			batsman[playerID].Runs.text = "" + CONTROLLER.TeamList[0].PlayerList[_batsmanId].RunsScored;
			batsman[playerID].Balls.text = "" + CONTROLLER.TeamList[0].PlayerList[_batsmanId].BallsPlayed;
			batsman[playerID].Fours.text = "" + CONTROLLER.TeamList[0].PlayerList[_batsmanId].Fours;
			batsman[playerID].Sixes.text = "" + CONTROLLER.TeamList[0].PlayerList[_batsmanId].Sixes;
			CONTROLLER.TeamList[0].PlayerList[_batsmanId].status = "out";
			batsman[playerID].GetComponent<Image>().sprite = Normal;
		}
	}

	private void  PlaySuperOverLevel ()
	{
		int  mod= CONTROLLER.LevelId%2;
		if (mod == 0)
		{
			CONTROLLER.bowlerType = "fast";
		}
		else
		{
			CONTROLLER.bowlerType = "spin";
		}
		//27march
		GroundController.instance.ResetAll ();
		Scoreboard.instance.HidePause (true);
		PreviewScreen.instance.Hide (true);
		GameModel.instance.ShowIntroAnimation ();
		//27march
	}

	private void  PlaySuperChaseLevel ()
	{
		AdIntegrate.instance.SetTimeScale(1f);
		Scoreboard.instance.Hide (false);
		PreviewScreen.instance.Hide (false);
		CONTROLLER.totalOvers = CONTROLLER.Overs[CONTROLLER.CTLevelId];
		CONTROLLER.GetRandomScoreForOppTeam (CONTROLLER.StartRangeArray[CONTROLLER.CTSubLevelId], CONTROLLER.EndRangeArray[CONTROLLER.CTSubLevelId]);
		Scoreboard.instance.ShowTargetScreen (true);
		GameModel.instance.ShowIntroAnimation ();
	}
    private void PlaySuperCrusadeLevel()
    {
		AdIntegrate.instance.SetTimeScale(1f);
		Scoreboard.instance.Hide(false);
        PreviewScreen.instance.Hide(false);               
        Scoreboard.instance.ShowTargetScreen(true);
        GameModel.instance.ShowIntroAnimation();
    }
    public void StartGame ()
	{
		if (CONTROLLER.gameMode == "superover")
		{
			if(!PlayerPrefs.HasKey ("SuperOverDetail"))
			{
				PlaySuperOverLevel ();
			}
			else
			{
				GameModel.instance.ShowIntroAnimation ();
			}
		}
		else if (CONTROLLER.gameMode == "slogover")
		{
			GameModel.instance.ShowIntroAnimation ();
        }
        else if (CONTROLLER.gameMode == "chasetarget")
		{
			if (!PlayerPrefs.HasKey ("ChaseTargetDetail"))
			{
				PlaySuperChaseLevel ();
			}
			else
			{
				GameModel.instance.ShowIntroAnimation ();
			}
		}
        else if (CONTROLLER.gameMode == CONTROLLER.SUPER_Crusade_GameMode)
        {
            if (!PlayerPrefs.HasKey(CONTROLLER.SUPER_Crusade_SavedMatchDetails))
            {
                PlaySuperCrusadeLevel();
            }
            else
            {
                GameModel.instance.ShowIntroAnimation();
            }
			//Deiva start supercru
		   // Achievements.instance.SuperChaseCompleted(false);
		}
    }

	public void ShowMe ()
	{
		if(AdIntegrate.instance != null)
		{
			AdIntegrate.instance.HideAd ();
		}
		if (CONTROLLER.InningsCompleted && CONTROLLER.gameMode == CONTROLLER.SUPER_Crusade_GameMode)
		{
			CloseButtonClicked();
			return;
		}

		CONTROLLER.CurrentPage = "battingscorecard";
		//AdIntegrate.instance.SetTimeScale(0f);
		GamePauseScreen.instance.Hide (true);
        if(CONTROLLER.gameMode == CONTROLLER.SUPER_Crusade_GameMode)            //arun
        {
            //CSKUIText.enabled = true;
            //VSoppUIText.text = "CSK vs " + oppTeamName[GetOppTeamNameIndex()];
            //VSoppUIText.gameObject.SetActive(true);
            //scoreCardTitle.SetActive(false);
        }
        else
        {
            CSKUIText.enabled = false;
            VSoppUIText.enabled = false;
            scoreCardTitle.SetActive(true);
            VSoppUIText.gameObject.SetActive(false);
        }

		//CricMini-Gopi
		CloseButtonClicked();
	}
	public int GetOppTeamNameIndex()            //getting oppname
    {
        int temp = 0;
        int tempIndex = 0;
        int tempIndex2 = 0;
        for (int i =0; i < 20; i++)
        {
            tempIndex = CONTROLLER.SelectedCrusadeMatchIdx;
            tempIndex2 = CONTROLLER.SelectedCrusadeSeasonIdx;

            if (CONTROLLER.SelectedCrusadeMatchIdx < 0)
            {
                tempIndex = 0;
            }
            if (CONTROLLER.SelectedCrusadeSeasonIdx < 0)
            {
                tempIndex2 = 0;
            }
            if (oppTeamName[i] == SuperCrusadesController.instance.SuperCrusadesData[tempIndex2].MatchDatas[tempIndex].OppTeamName)
            {
                temp = (i-1);
                break;
            }
        }

        return temp;
    }

	public void  HideMe ()
	{
		scoreCard.SetActive(false);
	}

	public void CloseButtonClicked ()
	{ 
		AnimationScreen.instance.isLastBallWicket = false;
		firstPlayer = -1;

		newGame = false;
		if (CONTROLLER.InningsCompleted == true)
		{

            if (CONTROLLER.gameMode == CONTROLLER.SUPER_Crusade_GameMode)
                SuperCrusadesController.instance.ShowGameOver();
            else
			    GameModel.instance.ShowGameOverScreen ();
			
			return;
		}

		if (FirstTimeContinueClicked == false)
		{
			FirstTimeContinueClicked = true;
			StartGame ();
			HideMe ();
			return;
		}

		if (GameModel.isGamePaused == true)
		{
			playerChange.SetActive (false);	setWicketCamera (false );
			GameModel.isGamePaused = false;
			HideMe ();
			return;
		}
		playerChange.SetActive (false); 	setWicketCamera (false);
		
		if (!GamePauseScreen.instance.gamePause.activeInHierarchy && !InterstialAdLoadingScript.instance.holder.activeSelf)
		{
			AdIntegrate.instance.SetTimeScale(1f);
		}

		GameModel.instance.ResetPauseVar ();
		GroundController.instance.StartToBowl ();
		Scoreboard.instance.Hide (false);
		BatsmanInfo.instance.ShowMe ();
		Scoreboard.instance.HidePause (false);
		PreviewScreen.instance.Hide (false);
		GamePauseScreen.instance.SetLoftState(true);
		HideMe();
	}
}

