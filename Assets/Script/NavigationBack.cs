using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationBack : Singleton<NavigationBack> 
{
    protected void  Update ()
    {
        if (CONTROLLER.TargetPlatform == "android")
		{
			if (Input.GetKeyUp(KeyCode.Escape) == true)
			{
				if (LoadingScreen.instance.holder.activeSelf)
					return;
				else if (Popup.instance.holder.activeSelf )
				{
					if( Popup.instance.IsBackAllowed)
						Popup.instance.OnBack();
					
					return;
				}
				else if (CONTROLLER.CurrentPage == "settingspage" && Settings.instance!=null)
				{
					Settings.instance.OnBack();
					return;
				}
				if (ManageScene.CurScene == Scenes.MainMenu)
				{
					if (SignInPanel.instance.Holder.activeSelf)
					{
						//MultiplayerPage.instance.ShowGameExitPopup();
						return;
					}
					else if(CONTROLLER.CurrentPage== "deeplinking")
					{
						GameModeSelector._instance.HideDeeplinkPopup();
					}
                    else if (CONTROLLER.CurrentPage == "instructionpage")
                    {
						if (GameModeSelector._instance.instuctionAssigner.HelpPanelHolder.activeSelf)
						{
							GameModeSelector._instance.instuctionAssigner.HelpPanelHolder.SetActive(false);
						}
						else
						{
							GameModeSelector._instance.GoBackOne();
							CONTROLLER.CurrentPage = "splashpage";
						}
                    }
                    else if (CONTROLLER.CurrentPage == "jerseyselection")
					{
						GameModeSelector._instance.close(5);
						CONTROLLER.CurrentPage = "splashpage";
					}
					else if (CONTROLLER.CurrentPage == "controlpage")
					{
						GameModeSelector._instance.close (2);
						CONTROLLER.CurrentPage = "splashpage";
					}
					else if (CONTROLLER.CurrentPage == "storepage")
					{
						GameModeSelector._instance.close (4);
						CONTROLLER.CurrentPage = "splashpage";
					}

					else if (CONTROLLER.CurrentPage == "leaderboardpage")
					{
						GameModeSelector._instance.close (1);
						CONTROLLER.CurrentPage = "splashpage";
					}
					else if (CONTROLLER.CurrentPage == "teammanagementpage")
					{
						GameModeSelector._instance.GoBackTwo ();
						CONTROLLER.CurrentPage = "instructionpage";
					}
					else if (CONTROLLER.CurrentPage == "teamPopup")
					{
						
						TeamSelection.instance.excessForeignPlayers.SetActive (false);
						TeamSelection.instance.insufficientPlayers.SetActive (false);
						CONTROLLER.CurrentPage = "teammanagementpage";
					}
					else if (CONTROLLER.CurrentPage == "battinglineup")
					{
						BattingLineup.instance.Back (); 
					}
					else if (CONTROLLER.CurrentPage == "dispMsg")
					{
						if (GameModeSelector._instance.displayMsg.activeInHierarchy)
							GameModeSelector._instance.displayMsg.SetActive(false);
						CONTROLLER.CurrentPage = "splashpage";
					}
					else if (CONTROLLER.CurrentPage == "splashpage")
					{
                      // MultiplayerPage.instance.ShowGameExitPopup();
					}
				}
				else if (ManageScene.CurScene == Scenes.Ground && CONTROLLER .gameMode !="multiplayer")
				{
					if (InterstialAdLoadingScript.instance != null && InterstialAdLoadingScript.instance.holder.activeSelf)
					{
						return;
					}
					else if (ProgressBar.instance != null && ProgressBar.instance.holder.activeSelf)
						ProgressBar.instance.closeButEvent();
					else if (HeadStart.instance != null && HeadStart.instance.holder.activeSelf)
						HeadStart.instance.ClosePopup();
					else if (ExtraBall.instance != null && ExtraBall.instance.holder.activeSelf)
						ExtraBall.instance.ClosePopup();
					else if (BattingScoreCard.canSwap)
						BattingScoreCard.instance.NormalView();
					else if (BattingScoreCard.instance.playerChange.activeSelf)
					{
						BattingScoreCard.instance.ChooseNextPlayer();
					}
					else if (CONTROLLER.CurrentPage == "soresultpage" && SuperOverResult.instance != null)
					{
						SuperOverResult.instance.GoToHome();
					}
					else if (GameOverScreen.instance != null)
					{
						GameOverScreen.instance.menuClicked(0);
					}
					else if (CONTROLLER.CurrentPage == "ingame")
					{
						if (BattingScoreCard.instance != null && BattingScoreCard.instance.scoreCard.activeSelf)
							return;
						if (Scoreboard.instance.pauseBtn.gameObject.activeSelf)
						{
							GameModel.instance.GamePaused(true);
							//GamePauseScreen.instance.SetGamePauseScreen(true);
							CONTROLLER.CurrentPage = "gamepausepage";
						}
					}
					else if (CONTROLLER.CurrentPage == "gamepausepage")
					{
						GamePauseScreen.instance.menuClicked(0);
					}
					else if (CONTROLLER.CurrentPage == "superchasesublevelselectionpage")
					{
						ShowCTMainScreen();
					}
					else if (CONTROLLER.CurrentPage == "instructionpage")
					{
						if (GamePauseScreen.instance.instuctionAssigner.HelpPanelHolder.activeSelf)
						{
							GamePauseScreen.instance.instuctionAssigner.HelpPanelHolder.SetActive(false);
						}
						else
						{
							GamePauseScreen.instance.instructionPage.SetActive(false);
							GamePauseScreen.instance.SetGamePauseScreen(true);
							GameModel.instance.GamePaused(true);
							CONTROLLER.CurrentPage = "gamepausepage";
						}
					}
					else if (CONTROLLER.CurrentPage == "SOsettings")
					{
						SuperOverResult.instance.settings.SetActive(false);
						//GamePauseScreen.instance.gamePause.SetActive (true);
						//GameModel.instance.GamePaused (true);
						CONTROLLER.CurrentPage = "";
					}
					else if (CONTROLLER.CurrentPage == "SOinstructions")
					{
						SuperOverResult.instance.hideInstructions();
						//GameModel.instance.GamePaused (true);
						CONTROLLER.CurrentPage = "";
					}
					else if (CONTROLLER.CurrentPage == "GOsettings")
					{
						GameOverScreen.instance.settings.SetActive(false);
						//GamePauseScreen.instance.gamePause.SetActive (true);
						//GameModel.instance.GamePaused (true);
						CONTROLLER.CurrentPage = "";
					}
					else if (CONTROLLER.CurrentPage == "GOinstructions")
					{
						GameOverScreen.instance.hideInstructions();
						//GameModel.instance.GamePaused (true);
						CONTROLLER.CurrentPage = "";
					}
					/*else if (CONTROLLER.CurrentPage == "battingscorecard" && GameModel.isGamePaused == true)
					{
						BattingScoreCard.instance.HideMe ();
						GameModel.instance.GamePaused (true);
						GamePauseScreen.instance.gamePause.SetActive (true);
						CONTROLLER.CurrentPage = "gamepausepage";
					}*/
					else if (CONTROLLER.CurrentPage == "dispMsg")
					{
						GamePauseScreen.instance.quitPopup.SetActive(false);
						GamePauseScreen.instance.SetGamePauseScreen(true);
						CONTROLLER.CurrentPage = "gamepausepage";
					}
					else if (CONTROLLER.CurrentPage == "levelselection")
					{
						UIAnimation.instance.GoToMainMenu();
						CONTROLLER.CurrentPage = "splashpage";
					}
				}
				
			}
		}
	}


	private void  ShowCTMainScreen ()
	{
		if (CTLevelSelectionPage.instance != null)
		{
			CTLevelSelectionPage.instance.HideThis ();
		}
		GameObject prefabGO;
		GameObject tempGO ;
		prefabGO = Resources.Load ("Prefabs/CTMenuScreen")as GameObject;
		tempGO = Instantiate (prefabGO)as GameObject;
		tempGO.name = "CTMenuScreen";
	}
	/*In GUIModel.js*/
	/*private var NavigationBack : GameObject;
	protected function Awake ()
	{
		instance = this;
		NavigationBack = new GameObject ();
		NavigationBack.name = "NavigationBack";
		if (CONTROLLER.TargetPlatform == "android")
		{
			NavigationBack.AddComponent ("NavigationBack");
		}
	}
	*/
	/*private function AlignContinueButton ()
	{
		var screenPos : Vector2 = new Vector2 (Screen.width, Screen.height);
		var newPos : Vector3 = new Vector3 (0,0,0);
		newPos = renderCamera.ScreenToWorldPoint (new Vector3 (screenPos.x, screenPos.y, -0.02));
		ContinueBtn.transform.position.x = newPos.x - 10;
		ContinueBtn.transform.position.y = -newPos.y + 10;
	}//In BattingScoreCard
	*/
}

//this.gameObject.transform.localPosition.z = -0.05; in FadeView.js
/*1. BattingScoreCard out and not out status. (controlisEnabled = false)
2. Bg sound playing in game scene.
3. Show preloader in leaderboard. (06_Loader.x = 0)
4. In batting scorecard sometimes the continue button is not displaying
5. Change LoginPage as dynamic
*/