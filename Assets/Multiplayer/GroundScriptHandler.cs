using UnityEngine;
using System.Collections;

public class GroundScriptHandler : MonoBehaviour
{
	private static GroundScriptHandler _instance = null;

	public GameObject GroundUI, loadingScreen;

	public static GroundScriptHandler Instance
	{
		get
		{
			return _instance;
		}
	}

	void Start ()
	{
		if (_instance == null)
		{
			_instance = this;
		}
	}

	public void ShowLoadingScreen ()
	{
		GroundUI.SetActive (true ); 
		loadingScreen.SetActive (true);
	}

	public void HideLoadingScreen ()
	{
		GroundUI.SetActive (false);
		loadingScreen.SetActive (false);
	}

	public void ShowServerDisconnectedPopup ()
	{
		if (CONTROLLER.gameMode == "multiplayer" && AdIntegrate.instance.CurrentSceneIndex ==2 )
			Popup.instance.showGenericPopup("DISCONNECTED", "Seems like you have been disconnected from the server due to unkown network issue.\nPlease make sure you have stable internet.", LoadMainMenuScene);
	}

	public void LoadMainMenuScene ()
	{
		ServerManager.Instance.CheckManualInternetInterruption(true);
		GameModel.instance .ResetCurrentMatchDetails ();
		GameModel .instance .ResetVariables ();
		GameModel .instance .ResetAllLocalVariables ();
		ShowLoadingScreen ();
		ManageScene.LoadScene(Scenes.MainMenu);
	}

}
