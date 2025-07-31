using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatsmanInfo : Singleton<BatsmanInfo> 
{
	public GameObject info;
	public Image PlayerThumbnail;
	public Sprite[] images;
	public Text PlayerName;
	private Camera renderCamera  ;

	protected void  Awake ()
	{
		renderCamera = GameObject.Find ("MainCamera").GetComponent<Camera>();
	}

	public void  UpdateStrikerInfo ()
	{
		/*if(CONTROLLER.StrikerIndex < CONTROLLER.PlayingTeam.Count)
		{
			int  playerID =(int ) CONTROLLER.PlayingTeam[CONTROLLER.StrikerIndex];
			string  strikerIndex = CONTROLLER.TeamList[0].PlayerList[playerID].PlayerName;
		
			foreach (Sprite sprite in images) {
				if (sprite.name == strikerIndex) {
					PlayerThumbnail.sprite = sprite;
				}
			}
			PlayerName.text = "" + CONTROLLER.TeamList[0].PlayerList[playerID].ShortName + " " + CONTROLLER.TeamList[0].PlayerList[playerID].RunsScored + " *";
		}*/
	}

	public void  ShowMe ()
	{
		//info.SetActive (true);  

		//CricMini-Gopi
		info.SetActive(false);

	}

	public void  HideMe ()
	{
		info.SetActive (false);
	}

	public void  TweenProfileCard ()
	{
//		this.gameObject.transform.localPosition.x = -CONTROLLER.HIDEPOS.x;
//		iTween.MoveTo(this.gameObject, {"x":CONTROLLER.xOffSet, "time":1, "easetype":"easeInOutSine"});

		this.gameObject.transform.localPosition=new Vector3 (this.gameObject.transform.localPosition.x-CONTROLLER.HIDEPOS.x,this.gameObject.transform.localPosition.y,this.gameObject.transform.localPosition.z);
		iTween.MoveTo (this.gameObject, iTween.Hash ("x", CONTROLLER.xOffSet, "time", 0.2));

	}
}
