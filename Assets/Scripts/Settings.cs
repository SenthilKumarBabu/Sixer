using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : Singleton<Settings> 
{
    public Sprite[] commonimage;
	public Image[] BGSound, gameSound, shotIndcator;
	public GameObject BottomPanel;

    public Text UserName;
	void OnEnable () 
	{
		SetSettingsPage ();
	}


    public void  SetSettingsPage ()
	{
		if (CONTROLLER.BGMusicVal == 1)
		{
			BGSound [0].enabled = true;
            BGSound [1].enabled = false;
           // BGSound[0].gameObject.transform.parent.GetComponent<Image>().sprite = commonimage[0];
        }
		else
		{
			BGSound [0].enabled = false;
            BGSound [1].enabled = true;
           // BGSound[0].gameObject.transform.parent.GetComponent<Image>().sprite = commonimage[1];
        }

		if(CONTROLLER.GameMusicVal == 1)
		{
			gameSound [0].enabled = true;
			gameSound [1].enabled = false;
         //   gameSound[0].gameObject.transform.parent.GetComponent<Image>().sprite = commonimage[0];
        }
		else
		{
			gameSound [0].enabled = false;
			gameSound [1].enabled = true;
         //   gameSound[0].gameObject.transform.parent.GetComponent<Image>().sprite = commonimage[1];
        }

		if(CONTROLLER.shotIndicator == 1)
		{
			shotIndcator [0].enabled = true;
			shotIndcator [1].enabled = false;
            //shotIndcator[0].gameObject.transform.parent.GetComponent<Image>().sprite = commonimage[0];
        }
		else
		{
			shotIndcator [0].enabled = false;
			shotIndcator [1].enabled = true;
           // shotIndcator[0].gameObject.transform.parent.GetComponent<Image>().sprite = commonimage[1];
        }

        if (AdIntegrate.instance.CurrentSceneIndex == 2)
            BottomPanel.SetActive(false);
        else
            BottomPanel.SetActive(true);

        UserName.text = CONTROLLER.UserUniqueName;
    }

	public void  SettingsSelected (int index)
	{
         AudioPlayer.instance.PlayButtonSnd ();
        if (index == 0)     //BGMusicBtn
        {
            if (BGSound[1].GetComponent<Image>().enabled == true)
            {
                BGSound[0].enabled = true;
                BGSound[1].enabled = false;
                //BGSound_text[0].GetComponent<Text>().color = new Color32(3, 112, 223, 255);
                BGSound[0].gameObject.transform.parent.GetComponent<Image>().sprite = commonimage[0];
                CONTROLLER.BGMusicVal = 1;
                AudioPlayer.instance.BGMFadeInOut(0);
                CONTROLLER.isMuted = 1;
                AudioPlayer.instance.MuteAudio(1);
            }
            else
            {
                BGSound[0].enabled = false;
                BGSound[1].enabled = true;
                //BGSound_text[0].GetComponent<Text>().color = new Color32(190, 190, 190, 255);
                BGSound[0].gameObject.transform.parent.GetComponent<Image>().sprite = commonimage[1];
                CONTROLLER.BGMusicVal = 0;
                AudioPlayer.instance.BGMFadeInOut(1);
            }
        }
        else if (index == 1)    //GameMusicBtn
        {
            if (gameSound[1].enabled == true)
            {
                gameSound[0].enabled = true;
                gameSound[1].enabled = false;
                //gameSound_text[0].GetComponent<Text>().color = new Color32(3, 112, 223, 255);
                gameSound[0].gameObject.transform.parent.GetComponent<Image>().sprite = commonimage[0];
                CONTROLLER.GameMusicVal = 1;
                CONTROLLER.isMuted = 1;
                AudioPlayer.instance.MuteAudio(1);
                AudioPlayer.instance.PlayButtonSnd();
            }
            else
            {
                gameSound[0].enabled = false;
                gameSound[1].enabled = true;
                //gameSound_text[0].GetComponent<Text>().color = new Color32(190, 190, 190, 255);
                gameSound[0].gameObject.transform.parent.GetComponent<Image>().sprite = commonimage[1];
                CONTROLLER.GameMusicVal = 0;
            }
           // AudioPlayer.instance.ToggleInGameSounds(CONTROLLER.GameMusicVal == 1 ? true:false) ;
        }
        else if (index == 2)
        {//ShotIndicatorBtn
            if (shotIndcator[1].GetComponent<Image>().enabled == true)
            {
                shotIndcator[0].enabled = true;
                shotIndcator[1].enabled = false;
               // shotIndcator_text[0].GetComponent<Text>().color = new Color32(3, 112, 223, 255);
                shotIndcator[0].gameObject.transform.parent.GetComponent<Image>().sprite = commonimage[0];
                CONTROLLER.shotIndicator = 1;
            }
            else
            {
                shotIndcator[0].enabled = false;
                shotIndcator[1].enabled = true;
               // shotIndcator_text[0].GetComponent<Text>().color = new Color32(190, 190, 190, 255);
                shotIndcator[0].gameObject.transform.parent.GetComponent<Image>().sprite = commonimage[1];
                CONTROLLER.shotIndicator = 0;
            }
        }

		/*if (AudioPlayer.instance != null)
        {
            AudioPlayer.instance.MuteAudio (CONTROLLER.isMuted);

            //0036990
            if (AdIntegrate.instance.CurrentSceneIndex==2)
            {
                if (CONTROLLER.GameMusicVal == 1 || CONTROLLER.BGMusicVal ==1)
                {
                    CONTROLLER.isMuted = 1;
                    AudioPlayer.instance.MuteAudio(1);
                    Scoreboard.instance.muteBtn.sprite = Scoreboard.instance.mute;
                }
                else
                {
                    Scoreboard.instance.muteBtn.sprite = Scoreboard.instance.unmute;
                }
            }
		}*/
        PlayerPrefsManager.SetSettingsList ();

	}
	public void closeSettings() 
    {
        PlayerPrefsManager.SetSettingsList();
        this.gameObject.SetActive (false);
	}

    public void DeleteAccount()
    {
        AudioPlayer.instance.PlayButtonSnd();

        if (AdIntegrate.instance.checkTheInternet())
        {
            /*if (IAPHandler.instance.deferredProductsList.Count > 0 || IAPHandler.instance.HasUnSyncedPurchases())
            {
                Popup.instance.Show(heading: "PURCHASE IN PROCESS", message: "Your recent purchase is still in process. Please try signing out after a while. You can still safely exit the app though.", yesString: "OK", size: 1);
            }
            else
                Popup.instance.Show(true, "DELETE ACCOUNT", "Deleting your game account is permanent.\nYour progress and leaderboard ranking will all be removed forever.\nAre you sure you want to delete your account? ", "YES", true, "NO", OnDeleteCall,size:0);*/
        }
        else
            Popup.instance.ShowNoInternetPopup();
    }


    public GameObject ControlsHolder, AboutHolder,HelpHolder;
    public void OnButtonClicked(int idx)
    {

        switch (idx)
        {
            case 0:
                AudioPlayer.instance.PlayButtonSnd();
                ControlsHolder.SetActive(true);
                break;
            case 1:
                AudioPlayer.instance.PlayButtonSnd();
                AboutHolder.SetActive(true);
                break;
            case 2: //help
                AudioPlayer.instance.PlayButtonSnd();
                HelpHolder.SetActive(true);
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                TextEditor te = new TextEditor();
                te.text = CONTROLLER.UserUniqueName;
                te.SelectAll();
                te.Copy();
                AdIntegrate.instance.ShowToast("User id is Copied");
                break;
        }
    }
    public void OnBack()
    {
        AudioPlayer.instance.PlayButtonSnd();

        if (AboutHolder.activeSelf)
            AboutHolder.SetActive(false);
        else if (ControlsHolder.activeSelf)
            ControlsHolder.SetActive(false);
        else if (HelpHolder.activeSelf)
            HelpHolder.SetActive(false);
        else
        {
            CONTROLLER.CurrentPage = CONTROLLER.tempCurrentPage;
            if (AdIntegrate.instance.CurrentSceneIndex == 2)
            {
                GamePauseScreen.instance.SetGamePauseScreen(true);
                GameModel.instance.GamePaused(true);
            }
            closeSettings();
            if(AdIntegrate.instance.CurrentSceneIndex==1)
                GameModeSelector._instance.ShowLandingPage(true);
        }
    }
}
