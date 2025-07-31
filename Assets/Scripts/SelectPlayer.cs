using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectPlayer : Singleton<SelectPlayer>, IPointerClickHandler  {
	public bool isForeignPlayer;
	public static bool dp;
	public Image FogImage;
	public Color32 WhiteColor = new Color32(255, 255, 255, 255);
	public Color32 OrangeColor = new Color32(255, 207, 30, 255);
	private Sprite sprite;
	public Image PlayerBG;
	public Image playerImage;
	public Text playerName, jerseyNo;
	public bool teamcountIncreasable;
	public Sprite selected, notSelected;
	public int playerIndex;
	public Text playerCount;
	public GameObject rays;
	public Image playerType;
	Animator anim;
	// Use this for initialization
	void Awake () {
		//sprite = this.gameObject.GetComponent<Image> ();
//		this.gameObject.GetComponent<Image> ().sprite = notSelected;
//		sprite = this.gameObject.GetComponent<Image> ().sprite;
//		foreach (Sprite image in TeamSelection.instance.playerImage) {
//			if (image.name == CONTROLLER.TeamList [0].PlayerList [playerIndex].PlayerName) {
//				playerImage.sprite = image;
//			}
//		}
//		playerName.text = CONTROLLER.TeamList [0].PlayerList [playerIndex].ShortName.ToUpper();
//		jerseyNo.text = CONTROLLER.TeamList [0].PlayerList [playerIndex].JerseyNumber;
//		if(int.Parse(CONTROLLER.TeamList [0].PlayerList [playerIndex].JerseyNumber) < 10)
//			jerseyNo.text = "0" + CONTROLLER.TeamList [0].PlayerList [playerIndex].JerseyNumber;
//		if (CONTROLLER.TeamList [0].PlayerList [playerIndex].PlayerType == "1") {
//			isForeignPlayer = true;
//			playerType.sprite = TeamSelection.instance.playerType [1];
//		} else {
//			isForeignPlayer = false;
//			playerType.sprite = TeamSelection.instance.playerType [0];
//		}
//		LoadPreviousSelection ();
////		Debug.Log ("====player name: " + CONTROLLER.TeamList[0].PlayerList[playerIndex].PlayerName + "====sprite " + this.gameObject.GetComponent<Image> ().sprite.name);
		anim = this.transform.parent.gameObject.GetComponent<Animator> ();
		//rays.SetActive (true);
		//RayAnimator.instance.CheckPlayerStatus (playerIndex);
	}
	
	// Update is called once per frame
	void Update () {
		



	}

	public void Reset() {
		this.gameObject.GetComponent<Image> ().sprite = notSelected;
		sprite = this.gameObject.GetComponent<Image> ().sprite;
		foreach (Sprite image in TeamSelection.instance.playerImage) {
			if (image.name == CONTROLLER.TeamList [0].PlayerList [playerIndex].PlayerName) {
				playerImage.sprite = image;
			}
		}
		playerName.text = CONTROLLER.TeamList [0].PlayerList [playerIndex].ShortName.ToUpper();
		jerseyNo.text = CONTROLLER.TeamList [0].PlayerList [playerIndex].JerseyNumber;
		if(int.Parse(CONTROLLER.TeamList [0].PlayerList [playerIndex].JerseyNumber) < 10)
			jerseyNo.text = "0" + CONTROLLER.TeamList [0].PlayerList [playerIndex].JerseyNumber;
		if (CONTROLLER.TeamList [0].PlayerList [playerIndex].PlayerType == "1") {
			isForeignPlayer = true;
			playerType.sprite = TeamSelection.instance.playerType [1];
		} else {
			isForeignPlayer = false;
			playerType.sprite = TeamSelection.instance.playerType [0];
		}
		PlayerBG.SetNativeSize();
	}

	public void LoadPreviousSelection() {
		if (CONTROLLER.TeamList [0].PlayerList [playerIndex].DefaultPlayer == "1") {
			this.gameObject.GetComponent<Image> ().sprite = selected;

		}
		else if (CONTROLLER.TeamList [0].PlayerList [playerIndex].DefaultPlayer == "0") {
			this.gameObject.GetComponent<Image> ().sprite = notSelected;
		}
		CheckPlayerStatus ();
	}
		

	public virtual void OnPointerClick(PointerEventData point) {
		if (TeamSelection.playerCount < 12) {
			if (this.gameObject.GetComponent<Image> ().sprite == selected) {
				//Debug.Log ("Selected");
				//CONTROLLER.TeamList[0].PlayerList[TeamSelection.playerCount].DefaultPlayer = "0";
				//Debug.Log (CONTROLLER.TeamList [0].PlayerList[TeamSelection.playerCount].DefaultPlayer);
				sprite = notSelected;
				FogImage.color = OrangeColor;
				this.gameObject.GetComponent<Image> ().sprite = notSelected;
                playerName.GetComponent<Text>().color = new Color32(255, 255, 255, 255);
				CONTROLLER.TeamList [0].PlayerList [playerIndex].DefaultPlayer = "0";
				CheckPlayerStatus ();
				if (isForeignPlayer)
					TeamSelection.foreignPlayerCount -= 1;
				TeamSelection.playerCount -= 1;
				//playerCount.text = TeamSelection.playerCount;
			} else if (this.gameObject.GetComponent<Image> ().sprite == notSelected && TeamSelection.playerCount < 11) {
				//Debug.Log ("Not");
				//CONTROLLER.TeamList[0].PlayerList[TeamSelection.playerCount].DefaultPlayer = "1";

				sprite = selected;
				FogImage.color = WhiteColor;
				this.gameObject.GetComponent<Image> ().sprite = selected;
                playerName.GetComponent<Text>().color = new Color32(31, 31, 31, 255);
                CONTROLLER.TeamList [0].PlayerList [playerIndex].DefaultPlayer = "1";
				CheckPlayerStatus ();
				if (isForeignPlayer)
					TeamSelection.foreignPlayerCount += 1;
				TeamSelection.playerCount += 1;
				//Debug.Log (CONTROLLER.TeamList [0].PlayerList[TeamSelection.playerCount].DefaultPlayer);
				//playerCount.text = TeamSelection.playerCount;
			} else {
				TeamSelection.instance.showToast ();
				//TeamSelection.instance.toast.SetActive (false);
			}
		}
		anim.Play ("pressAnimation");
		//anim.SetTrigger("pressed");
		//drag = false;
		PlayerBG.SetNativeSize();
	}
	public void CheckPlayerStatus() {
		if (this.gameObject.GetComponent<Image> ().sprite == selected) {
			//anim.Play ("rays");
			rays.SetActive (true);
            FogImage.color = WhiteColor;
            playerName.GetComponent<Text>().color = new Color32(31, 31, 31, 255);
            this.gameObject.GetComponent<Image>().sprite = selected;
        } else if (this.gameObject.GetComponent<Image> ().sprite == notSelected) {
			rays.SetActive (false);
            FogImage.color = OrangeColor;
            playerName.GetComponent<Text>().color = new Color32(255, 255, 255, 255);
            this.gameObject.GetComponent<Image>().sprite = notSelected;
        }
	}


		
}
