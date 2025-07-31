using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerDetails : MonoBehaviour , IPointerClickHandler {

	public Image Status;
	public Sprite selected;
	public static Sprite previous;
	public Text Name, Runs, Fours, Sixes, Balls;
	public int index;
	// Use this for initialization
	void Start () {
		previous = GetComponent<Image> ().sprite;

		//CricMini-Gopi
		this.GetComponent<Button>().enabled = false;
	}

	//CricMini-Gopi
	/*void Update () {
		if (Application.loadedLevelName == "Ground") {
			if (BattingScoreCard.newGame && BattingScoreCard.canSwap) {
			
				this.GetComponent<Button> ().enabled = true;
			} else {
				this.GetComponent<Button> ().enabled = false;
			}
		}
	}*/

	public virtual void OnPointerClick(PointerEventData point) {
		//		if (BattingScoreCard.newGame && BattingScoreCard.canSwap) {
		//			if (BattingScoreCard.swap < 2) {
		//				BattingScoreCard.swap += 1;
		//				if (BattingScoreCard.swap == 1) {
		//					BattingScoreCard.firstPlayer = index;
		//					GetComponent<Image> ().sprite = selected;
		//				} else if (BattingScoreCard.swap == 2) {
		//					if (BattingScoreCard.firstPlayer == index) {
		//						if(index < 2) 
		//							GetComponent<Image>().sprite = BattingScoreCard.instance.Highlighted;
		//						else
		//							GetComponent<Image>().sprite = BattingScoreCard.instance.Normal;
		//						GetComponent<Image> ().sprite = previous;
		//						BattingScoreCard.firstPlayer = BattingScoreCard.secondPlayer = -1;
		//						BattingScoreCard.swap = 0;
		//					}
		//					else {
		//						BattingScoreCard.secondPlayer = index;
		//						GetComponent<Image> ().sprite = selected;
		//						/*if (this.GetComponent<Animator> () != null) {
		//							this.GetComponent<Animator> ().SetTrigger ("flip");
		//							BattingScoreCard.instance.batsman [BattingScoreCard.firstPlayer].GetComponent<Animator> ().SetTrigger ("flip");
		//						}*/
		//						//BattingScoreCard.instance.ChangeOrder (BattingScoreCard.firstPlayer, BattingScoreCard.secondPlayer);
		//						//BattingScoreCard.swap = 0;
		//					}
		//				}
		//			}
		//			else if (BattingScoreCard.swap == 2) {
		//				if (BattingScoreCard.firstPlayer == index) {
		//					GetComponent<Image> ().sprite = previous;
		//					BattingScoreCard.firstPlayer = BattingScoreCard.secondPlayer;
		//					BattingScoreCard.secondPlayer = -1;
		//					BattingScoreCard.swap -= 1;
		//				}
		//				else if (BattingScoreCard.secondPlayer == index) {
		//					GetComponent<Image> ().sprite = previous;
		//					//BattingScoreCard.firstPlayer = BattingScoreCard.secondPlayer;
		//					BattingScoreCard.secondPlayer = -1;
		//					BattingScoreCard.swap -= 1;
		//				}
		//			}
		//		}


		//CricMini-Gopi
		/*if (Application.loadedLevelName == "Ground") {
			if (BattingScoreCard.newGame && BattingScoreCard.canSwap)
				BattingScoreCard.instance.SetIndex (index);
		} else {
			BattingLineup.instance.SetIndex (index);
		}*/

	}



}
