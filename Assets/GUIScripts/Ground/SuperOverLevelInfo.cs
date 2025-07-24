using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperOverLevelInfo : Singleton<SuperOverLevelInfo> 
{
	//public static SuperOverLevelInfo instance ;
	public Text ChallengeTitle ;
	public Text LevelTitle;
	public Text BowlerType;
	private string [] LevelDescriptionArray = new string[] {
		"Hit 10 runs in", 
		"Hit 3 fours in", 
		"Hit 3 consecutive fours in", 
		"Hit 3 sixes in", 
		"Hit 5 fours in", 
		"Hit 25 runs in",
		"Hit 4 consecutive sixes in", 
		"Hit 6 fours in", 
		"Hit 6 sixes in"};

	protected void  Awake ()
	{
		CONTROLLER.CurrentPage = "";
	}

	protected void  Start ()
	{
		AudioPlayer.instance.PlayGameSnd("nextlevelpopup");
		Scoreboard.instance.Hide (true);
		PreviewScreen.instance.Hide (true);
		//BowlerType.controlIsEnabled = false;
		ValidateThisLevel ();
	}

	void  Update ()
	{
		if (Input.GetMouseButtonDown(0))
		{
			BattingScoreCard.instance.ShowMe ();
			DestroyMe ();
		}
	}

	private void  ValidateThisLevel ()
	{
		int  bowlerType = CONTROLLER.LevelId%2;
		ChallengeTitle.text = "CHALLENGE " + (CONTROLLER.LevelId + 1);
		LevelTitle.text = "" + LevelDescriptionArray[(int )Mathf.Floor(CONTROLLER.LevelId/2)].ToUpper();
		if (bowlerType == 0)
		{
			BowlerType.text = "FAST BOWLING";
		}
		else
		{
			BowlerType.text = "SPIN BOWLING";
		}
/*		if (GoogleAnalyticsBinder.instance != null)
		{
			string  str = ""+ChallengeTitle.text;
			GoogleAnalyticsBinder.instance.PostEvent ("SuperOver_CHALLENGE_" + (CONTROLLER.LevelId + 1)+"_Completed");
		}*/
	}

	private void  DestroyMe ()
	{
		Destroy (this.gameObject);
		Resources.UnloadUnusedAssets ();
	}
}
