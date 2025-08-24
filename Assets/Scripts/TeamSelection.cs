using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamSelection : Singleton<TeamSelection> {

	public Sprite[] selected; //modeLogos, 
    //public string[] modename;
    public Text modetext;
	public Image logo;
	public Text playerCountText;
	public SelectPlayer[] players;
	public Sprite[] playerImage, playerType;
	public GameObject excessForeignPlayers, insufficientPlayers;
	public static int playerCount, foreignPlayerCount;
	private Animator anim;
	public GameObject battingOrder;
	// Use this for initialization

//	void Awake() {
//		foreach (SelectPlayer player in players) {
//			foreach (Sprite image in playerImage) {
//				if (image.name == CONTROLLER.TeamList [0].PlayerList [player.playerIndex].PlayerName) {
//					player.playerImage.sprite = image;
//				}
//			}
//			player.playerName.text = CONTROLLER.TeamList [0].PlayerList [player.playerIndex].ShortName;
//			player.jerseyNo.text = CONTROLLER.TeamList [0].PlayerList [player.playerIndex].JerseyNumber;
//			if (CONTROLLER.TeamList [0].PlayerList [player.playerIndex].PlayerType == "1") {
//				player.isForeignPlayer = true;
//				player.playerType.sprite = playerType [1];
//			} else {
//				player.isForeignPlayer = false;
//				player.playerType.sprite = playerType [0];
//			}
//			player.LoadPreviousSelection ();
//			player.CheckPlayerStatus ();
//		}
//	}

	void Start () {
		battingOrder.SetActive (false);
		playerCount = 0;
		foreignPlayerCount = 0;

		for(int i = 0; i< 25; i++) {
            //Debug.LogError(i);
			if (players[i].GetComponent<Image> ().sprite == players[i].selected) {
				if (players[i].isForeignPlayer)
					foreignPlayerCount++;
				playerCount++;
				//Debug.Log ("===playercount: "+playerCount +" playername: "+CONTROLLER.TeamList[0].PlayerList[player.playerIndex].PlayerName ); 
			}
		}
		if(GameModeSelector ._instance !=null )
			GameModeSelector._instance.resetScroll ();
		anim = GetComponent<Animator> ();
		anim.Play ("rays");
		//playerCount = 0;
		//foreignPlayerCount = 0;
		excessForeignPlayers.SetActive (false);
		insufficientPlayers.SetActive (false);
		//setModeLogo ();
	}
	
	// Update is called once per frame
	void Update () {
		setModeLogo ();
		playerCountText.text = TeamSelection.playerCount + "/11";
		if (GameModeSelector.inTeamSelectionPage == true) {
			SetPlayers();
		}
	}

	public void SetPlayers() {
		for(int i = 0; i< 25; i++) {
			players[i].Reset ();
		//	yield return new WaitForSeconds (0.1f);
			//Debug.Log (player.playerName.text);
		}
		playerCount = 0;
		foreignPlayerCount = 0;

		for(int i = 0; i< 25; i++) {
			if (CONTROLLER.TeamList [0].PlayerList [i].DefaultPlayer == "1") {
//				Debug.Log("Team Player name: " + CONTROLLER.TeamList [0].PlayerList [i].PlayerName + " Team Player dp: " + CONTROLLER.TeamList [0].PlayerList [i].DefaultPlayer);
				players [i].GetComponent<Image> ().sprite = selected [1];
				if (players [i].isForeignPlayer)
					foreignPlayerCount++;
				playerCount++;
				//Debug.Log ("===playercount: "+playerCount +" playername: "+CONTROLLER.TeamList[0].PlayerList[player.playerIndex].PlayerName ); 
			} else {
				players[i].GetComponent<Image> ().sprite = selected[0];
			}
			players [i].CheckPlayerStatus ();	
		}
		GameModeSelector.inTeamSelectionPage = false;
	}

	void setModeLogo() {
		if (CONTROLLER.selectedGameMode == GameMode.SuperOver)
		{
			//logo.sprite = modeLogos [0];
			modetext.text = "SUPER OVER";
			//logo.rectTransform.pivot = new Vector2 (0.25f, 0.75f); 
		}
		else if (CONTROLLER.selectedGameMode == GameMode.OnlyBatting)
		{
			//logo.sprite = modeLogos [1];
			modetext.text = "SUPER SLOG";
			//logo.rectTransform.pivot = new Vector2 (0.15f, 1f); 
		}
		else if (CONTROLLER.selectedGameMode == GameMode.ChaseTarget)
		{
			//logo.sprite = modeLogos [2];
			modetext.text = "SUPER CHASE";
			//	logo.rectTransform.pivot = new Vector2 (0.25f, 1f); 
		}
		else if (CONTROLLER.selectedGameMode == GameMode.BattingMultiplayer)
		{
			//logo.sprite = modeLogos [3];
			modetext.text = "SUPER MULTIPLAYER";
			//logo.rectTransform.pivot = new Vector2 (0.25f, 0.9f); 
		}
		else if (CONTROLLER.selectedGameMode == GameMode.SUPER_Crusade_GameMode)
		{
			//logo.sprite = modeLogos[4];
			modetext.text = "SUPER MATCHES";
			// logo.rectTransform.pivot = new Vector2(0.25f, 0.9f);           
		}
    }

	void ValidatePlayerNames() {
		/*int i = 0;
		int selectedNoOfPlayers = 0;
		int selectedForeignPlayers = 0;
		for(i = 0;i < CONTROLLER.totalPlayers; i++)
		{
			if (NameButtons[i].StateName == "Hover")
			{
				selectedNoOfPlayers++;
				if (CONTROLLER.TeamList[0].PlayerList[i].PlayerType == "1")//Check for foreign players
				{
					selectedForeignPlayers++;
				}
			}
		}
		if (selectedForeignPlayers > 4)
		{
			ShowErrorPopup ("There should be only four foreign players can play this match.",0);
			return false;
		}
		if (selectedNoOfPlayers > CONTROLLER.totalWickets + 1)
		{
			ShowErrorPopup ("You can select only "+(CONTROLLER.totalWickets + 1)+" players to play this match.",0);
			return false;
		}
		if (selectedNoOfPlayers < CONTROLLER.totalWickets + 1)
		{
			ShowErrorPopup ("You should select "+(CONTROLLER.totalWickets + 1)+" players to play this match.",0);
			return false;
		}
		if (selectedNoOfPlayers == CONTROLLER.totalWickets + 1)
		{
			for(i = 0;i < CONTROLLER.totalPlayers; i++)
			{
				if (NameButtons[i].StateName == "Hover")
				{
					CONTROLLER.TeamList[0].PlayerList[i].DefaultPlayer = "1";
				}
				else
				{
					CONTROLLER.TeamList[0].PlayerList[i].DefaultPlayer = "0";
				}
			}
			SavePlayerPrefs.SetTeamList ();
		}
		return true;*/
	}

	public void Continue()
	{
		if (playerCount < 11)
		{
			CONTROLLER.CurrentPage = "teamPopup";
			insufficientPlayers.SetActive(true);
		}
		if (foreignPlayerCount > 4)
		{
			CONTROLLER.CurrentPage = "teamPopup";
			excessForeignPlayers.SetActive(true);
		}
		if (playerCount == 11 && foreignPlayerCount <= 4)
		{
			PlayerPrefsManager.SetTeamList();
			if (CONTROLLER.selectedGameMode != GameMode.BattingMultiplayer)
			{
				StartCoroutine(LoadGroundScene());
			}
			else
			{
				battingOrder.SetActive(true);
				CONTROLLER.PlayingTeam = new ArrayList();
				//Debug.LogError(CONTROLLER.TeamList.Length);
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
				BattingLineup.instance.ResetBattingCard();
				this.gameObject.SetActive(false);

				CONTROLLER.CurrentPage = "battinglineup";
			}
		}
	}

	private IEnumerator  LoadGroundScene() 
	{
		GameModeSelector.isNewGame = true;
		LoadingScreen.instance.Show();
		yield return new WaitForSeconds (1f);
		GameModeSelector.resumeGame = false;
		ManageScene.LoadScene(Scenes.Ground);
	}

	public void CloseMessage(GameObject message) {
		message.SetActive (false);
	}

	public void  showToast() 
	{
		Popup.instance.showGenericPopup("", "Maximum players selected!");
	}

}
