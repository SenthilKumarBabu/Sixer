using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattingLineup : Singleton<BattingLineup> {
	public static bool newGame, canSwap;
	public static int firstPlayer, secondPlayer;
	public static int swap = 0;
	//public static BattingScoreCard instance  ;
	public GameObject scoreCard, playerName, arrow, moveUp, moveDown, instText;
	public Sprite Highlighted, Normal, Selected;
	public Image player1;
	public Text player1Text, playerNameText;
	public GameObject changeLineupButton;
	public PlayerDetails[] batsman;
	private int tempRank = 0, playerIndex;
	private Camera renderCamera  ;
	public bool  FirstTimeContinueClicked = false;

	protected void  Awake ()
	{
		//instance = this;
		renderCamera = GameObject.Find ("MainCamera").GetComponent<Camera>();
	}

	protected void Start ()
	{
		firstPlayer = -1;
		secondPlayer = -1;
		//ResetBattingCard ();
	}

	/*private void Update() {
		if (firstPlayer != -1) {
			
			foreach (Sprite sprite in Batsmen.instance.images) {
				if (sprite.name == batsman[firstPlayer].Name.text) {
					player1Text.gameObject.SetActive (false);
					playerNameText.text = sprite.name;
					player1.sprite = sprite;

				}
			} 
			moveUp.SetActive (true);
			moveDown.SetActive (true);
			instText.SetActive (true);
			arrow.SetActive (false);
			playerName.SetActive (true);
            player1.gameObject.SetActive(true);
        }

		if (firstPlayer == -1) {
			player1Text.gameObject.SetActive (true);
			player1.sprite = null;
            player1.gameObject.SetActive(false);
			playerName.SetActive (false);
			moveUp.SetActive (false);
			moveDown.SetActive (false);
			instText.SetActive (false);
			arrow.SetActive (true);
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
			
	}*/

	public void Back() {
		CONTROLLER.CurrentPage = "teammanagementpage";
		for(int i = 0; i < CONTROLLER.TeamList.Length; i++)
		{
			for(int j = 0; j < CONTROLLER.TeamList[i].PlayerList.Length; j++)
			{
				if(CONTROLLER.TeamList[i].PlayerList[j].DefaultPlayer == "1")
				{
					CONTROLLER.PlayingTeam.Add(j);
				}
			}
		}
		GameModeSelector.inTeamSelectionPage = true;
		this.gameObject.SetActive (false);
		TeamSelection.instance.gameObject.SetActive (true);
		GameModeSelector._instance.resetScroll ();
	}

	public void Continue() 
	{
		PlayerPrefsManager.SetTeamList ();
		//InterfaceHandler._instance.GameModeSelGO.SetActive (false); 
		GameModeSelector.inTeamSelectionPage = false;
		this.gameObject.SetActive (false);
		AdIntegrate.instance.ShowBannerAd();
	}

	public void  ResetBattingCard ()
	{
        int i;
		for(i = 0; i < batsman.Length; i++)
		{
			
			batsman[i].Name.text = "";
			//batsman[i].Highlight.SetToggleState ("normal");
			batsman[i].GetComponent<Image>().sprite = Normal;
			batsman [i].Name.fontStyle = FontStyle.Normal;
			batsman [i].Name.color = Color.black;
		}

		if(firstPlayer != -1) {
			batsman [firstPlayer].GetComponent<Image> ().sprite = Selected;
			batsman [firstPlayer].Name.color = Color.black;
			//batsman [firstPlayer].Name.fontStyle = FontStyle.Bold;
		}
		PlayingTeamCard ();
	}

	public void ChangeLineup() {
        canSwap = true;
		//scoreCard.GetComponent<Animator> ().SetTrigger ("flip");
	}

	public void NormalView() {
		firstPlayer = secondPlayer = -1;
		swap = 0;
		canSwap = false;
		ResetBattingCard ();
		//scoreCard.GetComponent<Animator> ().SetTrigger ("normal");
	}

	public void SetIndex(int index) {
        playerIndex = index;
		firstPlayer = index;
		int playerID;
		playerID =(int) CONTROLLER.PlayingTeam[index];
		if (batsman [firstPlayer].GetComponent<Image> ().sprite != Selected) {
			batsman [firstPlayer].GetComponent<Image> ().sprite = Selected;
			//playerName.GetComponentInChildren<Text>().text = CONTROLLER.TeamList[0].PlayerList[playerID].ShortName;
			ResetBattingCard();
		}
		else {
			if (playerIndex < 2)
				batsman [firstPlayer].GetComponent<Image> ().sprite = Highlighted;
			else
				batsman [firstPlayer].GetComponent<Image> ().sprite = Normal;
			batsman [firstPlayer].Name.fontStyle = FontStyle.Normal;
			batsman [firstPlayer].Name.color = Color.black;
			firstPlayer = -1;
		}
	}

	public void MovePlayer(int index) {
        if (firstPlayer != -1) {
			int playerID;
			//playerID =(int) CONTROLLER.PlayingTeam[playerIndex];
			PlayerDetails temp;
			//playerIndex = playerID;
			if (playerIndex == 0 && index == 1)
				return;
			if (playerIndex == 10 && index == -1)
				return;
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
		}
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
		

	public void CloseButtonClicked ()
	{ 

	}
}
