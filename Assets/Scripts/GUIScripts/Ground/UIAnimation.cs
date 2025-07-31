using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimation : Singleton<UIAnimation> 
{
	private GameObject prefabGO  ;
	private GameObject tempGO  ;
	public Button homeBtn, backBtn, muteBtn ;
	public Sprite mute, unmute;
	private Camera renderCamera;

	protected void  Awake ()
	{
		renderCamera = GameObject.Find ("MainCamera").GetComponent<Camera>();
	}

	protected void  Start ()
	{
		muteBtn.GetComponent<Image> ().sprite = mute;
		ShowLevelScreen ();
		//AlignMuteBtn ();
		//GameLogo.Hide (true);
//		yield WaitForSeconds (0.5);
		//MuteBtn.SetValueChangedDelegate (MuteSound);
		//SetGameModeLogo ();
	}

	/*private void  AlignMuteBtn ()
	{
		Vector2 screenPos = new Vector2 (Screen.width, Screen.height);
		Vector3 newPos   = new Vector3 (0,0,0);
		newPos = renderCamera.ScreenToWorldPoint (new Vector3 (screenPos.x, screenPos.y, -0.02f));

		MuteBtn.transform.position =new Vector3 (newPos.x - 10f,MuteBtn.transform.position.y,MuteBtn.transform.position.z);
		if (CONTROLLER.isFreeVersion == false)
		{
			MuteBtn.transform.position =new Vector3 (MuteBtn.transform.position.x,newPos.y - 80f,MuteBtn.transform.position.z);
		}
		else
		{
			MuteBtn.transform.position =new Vector3 (MuteBtn.transform.position.x,newPos.y - 170f,MuteBtn.transform.position.z);
		}
		if (CONTROLLER.isMuted == 0)
		{
			MuteBtn.SetToggleState ("UnMuteNow");
		}
		else
		{
			MuteBtn.SetToggleState ("MuteNow");
		}
		if(CONTROLLER.sndController != null)
		{
			CONTROLLER.sndController.MuteAudio (CONTROLLER.isMuted);
		}
	}*/

	public void  MuteSound ()
	{
		{
			if(muteBtn.GetComponent<Image> ().sprite == unmute)
			{
				CONTROLLER.isMuted = 0;
				if(AudioPlayer.instance != null)
				{
					AudioPlayer.instance.MuteAudio (0);
				}
				muteBtn.GetComponent<Image> ().sprite = mute;
			}
			else if (muteBtn.GetComponent<Image> ().sprite == mute)
			{
				CONTROLLER.isMuted = 1;
				if(AudioPlayer.instance != null)
				{
					AudioPlayer.instance.MuteAudio (1);
				}
				muteBtn.GetComponent<Image> ().sprite = unmute;
			}
			PlayerPrefsManager.SetSettingsList ();
		}
	}
	//April2

	/*private void  SetGameModeLogo ()
	{
		GameLogo.SetToggleState (CONTROLLER.gameMode);
//		HomeBtn.SetValueChangedDelegate (GoToMainMenu);
//		BackBtn.SetValueChangedDelegate (ShowCTMainScreen);
	}*/

	public void  ShowCTMainScreen ()
	{
		if (CTLevelSelectionPage.instance != null)
		{
			CTLevelSelectionPage.instance.HideThis ();
		}
		prefabGO = Resources.Load ("Prefabs/CTMenuScreen")as GameObject;
		tempGO = Instantiate (prefabGO)as GameObject;
		tempGO.name = "CTMenuScreen";
	}

	private void  ShowLevelScreen ()
	{
		if (CONTROLLER.gameMode == "superover" && SOLevelSelectionPage.instance == null)
		{
			HideBackBtn ();
			prefabGO = Resources.Load ("Prefabs/SOLevelSelection")as GameObject;
			tempGO = Instantiate (prefabGO)as GameObject;
			tempGO.name = "SOLevelSelection";
		}
		else if (CONTROLLER.gameMode == "chasetarget" && CTMenuScreen.instance == null)
		{
			//if (CONTROLLER.InningsCompleted == false)
			{
				if (CTLevelSelectionPage.instance != null)
				{
					CTLevelSelectionPage.instance.HideThis ();
				}
				prefabGO = Resources.Load ("Prefabs/CTMenuScreen")as GameObject;
				tempGO = Instantiate (prefabGO)as GameObject;
				tempGO.name = "CTMenuScreen";
			}
			/*else
			{
				if (CONTROLLER.CTSubLevelId != 4)
				{
					prefabGO = Resources.Load ("Prefabs/CTLevelSelection")as GameObject;
					tempGO = Instantiate (prefabGO)as GameObject;
					tempGO.name = "CTLevelSelection";
				}
				else
				{
					prefabGO = Resources.Load ("Prefabs/CTMenuScreen")as GameObject;
					tempGO = Instantiate (prefabGO)as GameObject;
					tempGO.name = "CTMenuScreen";
				}
			}*/
		}
	}

	private void  ShowMenu ()
	{
		ShowLevelScreen ();
	}

	public void  HideMenu ()
	{
		OnAnimationComplete ();
	}

	public void  GoToMainMenu ()
	{
        if (AdIntegrate.instance != null)
        {
            AdIntegrate.instance.HideAd();
        }
        TeamSelection.playerCount = 0;
		TeamSelection.foreignPlayerCount = 0;
		CONTROLLER.canShowMainCamera = true;
		StartCoroutine(GameModel.instance.GameQuitted ());
	}

	private void  OnAnimationComplete ()
	{
		if (CONTROLLER.gameMode == "superover")
		{
			int mod = CONTROLLER.LevelId%2;
			if (mod == 0)
			{
				CONTROLLER.bowlerType = "fast";
			}
			else
			{
				CONTROLLER.bowlerType = "spin";
			}
		}
		FadeView.instance.Hide (false);
		GroundController.instance.ResetAll ();
		GameModel.instance.ResetVariables ();
		Scoreboard.instance.Hide (false);
		PreviewScreen.instance.Hide (false);
		GameModel.instance.ShowIntroAnimation ();
		HideMe ();
	}

	public void  ShowBackBtn ()
	{
		backBtn.gameObject.SetActive(true);
		homeBtn.gameObject.SetActive (false);
	}

	public void  HideBackBtn ()
	{
		homeBtn.gameObject.SetActive (true);
		backBtn.gameObject.SetActive(false);
	}

	public void  HideMe ()
	{
		//Play crowd sound
		Destroy(this.gameObject);
		Resources.UnloadUnusedAssets ();
	}
}
