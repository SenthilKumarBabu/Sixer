using UnityEngine;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Linq;
using System;

public class PlayerPrefsManager : MonoBehaviour
{
	public TextAsset xmlAsset;
	public static PlayerPrefsManager instance;

	void Awake ()
	{
		instance = this;
		Encryption.Initialize ();
		DontDestroyOnLoad (this.gameObject);

#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
		CONTROLLER.TargetPlatform = "standalone";
#endif

#if UNITY_ANDROID
		CONTROLLER.TargetPlatform = "android";
#endif

#if UNITY_IPHONE
		CONTROLLER.TargetPlatform = "ios";
#endif

#if UNITY_WEBPLAYER
		CONTROLLER.TargetPlatform = "web";
#endif

		XMLReader.Loader = this.gameObject;

		CONTROLLER.Platform = CONTROLLER.TargetPlatform;


		LoadUserProfile ();
//		Invoke ("LogUserID", 5.0f);
	}

	public static void LogUserID ()
	{
//		if (UserProfile.PlayerName != "")
//		{
//			GoogleUniversalAnalytics.Instance.setUserID (UserProfile.PlayerName.ToString());
//			GoogleUniversalAnalytics.Instance.setApplicationVersion (AppInfo.Version);
//		}
	}

	//From LoadPlayerPref.cs
	protected void Start()
	{
		if (CONTROLLER.TeamList == null)
		{
			InitializeGame();
			GetTeamList();
			GetSuperOverLevelDetails();
			GetChaseTargetLevelDetails();
			GetSettingsList();
		}
	}

	public void loadData()
	{
		GetSuperOverLevelDetails();
		GetChaseTargetLevelDetails();
		GetSettingsList();
	}

	IEnumerator DelayedPause()
    {
        yield return new WaitForSecondsRealtime(0.5f);
		GameModel.instance.GamePaused(true,true);
	}

    private void InitializeGame()
	{
		HardcodesuperChaseTargets();

		float DefaultRatio = CONTROLLER.DefaultWidth / CONTROLLER.DefaultHeight;
		float SreenWidth = Screen.width;
		float SreenHeight = Screen.height;
		float ScreenRatio = SreenWidth / SreenHeight;
		CONTROLLER.xOffSet = ((DefaultRatio - ScreenRatio) * (CONTROLLER.DefaultHeight / 2));
		CONTROLLER.ScreenWidth = Screen.width;
		CONTROLLER.ScreenHeight = Screen.height * ScreenRatio;
		XMLReader.ParseXML(xmlAsset.text);
	}

	public void HardcodesuperChaseTargets()
	{
		if (CONTROLLER.EnableHardcodes == 0)
		{
			CONTROLLER.Overs[0] = 5;
			CONTROLLER.Overs[1] = 10;
			CONTROLLER.Overs[2] = 15;
			CONTROLLER.Overs[3] = 20;
			CONTROLLER.Overs[4] = 25;
			CONTROLLER.Overs[5] = 30;
			CONTROLLER.TargetRangeArray[0] = "40-50$60-70$80-90$100-120$120-150";
			CONTROLLER.TargetRangeArray[1] = "70-90$100-120$125-140$150-170$180-200";
			CONTROLLER.TargetRangeArray[2] = "90-110$115-130$135-150$160-180$185-220";
			CONTROLLER.TargetRangeArray[3] = "120-140$145-160$165-190$195-220$225-260";
			CONTROLLER.TargetRangeArray[4] = "150-180$185-210$215-240$245-260$265-280";
			CONTROLLER.TargetRangeArray[5] = "180-200$210-230$235-260$265-280$285-320";
		}
		else
		{
			//hardcode-gopi
			CONTROLLER.Overs[0] = 1;
			CONTROLLER.Overs[1] = 1;
			CONTROLLER.Overs[2] = 1;
			CONTROLLER.Overs[3] = 1;
			CONTROLLER.Overs[4] = 1;
			CONTROLLER.Overs[5] = 1;

#if UNITY_EDITOR
			CONTROLLER.TargetRangeArray[0] = "1-2$1-2$1-2$1-2$1-2";
			CONTROLLER.TargetRangeArray[1] = "1-2$1-2$1-2$1-2$1-2";
			CONTROLLER.TargetRangeArray[2] = "1-2$1-2$1-2$1-2$1-2";
			CONTROLLER.TargetRangeArray[3] = "1-2$1-2$1-2$1-2$1-2";
			CONTROLLER.TargetRangeArray[4] = "1-2$1-2$1-2$1-2$1-2";
			CONTROLLER.TargetRangeArray[5] = "1-2$1-2$1-2$1-2$1-2";
#else
            CONTROLLER.TargetRangeArray[0] = "7-8$8-8$9-9$10-10$11-11";
            CONTROLLER.TargetRangeArray[1] = "7-8$8-8$9-9$10-10$11-11";
            CONTROLLER.TargetRangeArray[2] = "7-8$8-8$9-9$10-10$11-11";
            CONTROLLER.TargetRangeArray[3] = "7-8$8-8$9-9$10-10$11-11";
            CONTROLLER.TargetRangeArray[4] = "7-8$8-8$9-9$10-10$11-11";
            CONTROLLER.TargetRangeArray[5] = "7-8$8-8$9-9$10-10$11-11";
#endif
		}
    }

	public void GetTeamList()
	{
		string dataStr;
		XMLReader.Loader = this.gameObject;
		//teamlist is deleted. And always it will load the xml.
		if (PlayerPrefs.HasKey("CurrentSeason"))
		{
			string _currSeason = PlayerPrefs.GetString("CurrentSeason");
			if (_currSeason == CONTROLLER.current_Season)
			{
				CONTROLLER.isupdatePopup = false;
				if (PlayerPrefs.HasKey("teamlist"))
				{
					dataStr = PlayerPrefs.GetString("teamlist");
					XMLReader.ParseXML(dataStr);
				}
				else
				{
					XMLReader.ParseXML(xmlAsset.text);
					PlayerPrefsManager.SetTeamList();
				}
			}
			else
			{
				//add the popup with the reason why you are deleting the previous datas
				ResetTeamForNewSeason();
			}
			// added by gopi for distinguish between old and fresh users
			if (!PlayerPrefs.HasKey("InstalledBuildNumber"))
				PlayerPrefs.SetInt("InstalledBuildNumber", CONTROLLER.CURRENT_VERSION);

		}
		else
		{
			//fresh install
			MakeAsFreshInstall();

			ResetTeamForNewSeason();
		}

	}

	void MakeAsFreshInstall()
	{
		//Debug.Log ("fresh install "+AppInfoController .BuildNumber );
		PlayerPrefs.SetInt("InstalledBuildNumber", CONTROLLER.CURRENT_VERSION);     //Installed Build Number

#if UNITY_IOS
		PlayerPrefs .SetInt ("teamlistchangesIOSv11",1);
#endif
	}

	private void ResetTeamForNewSeason()
	{
		PlayerPrefs.SetString("CurrentSeason", CONTROLLER.current_Season);
		if (PlayerPrefs.HasKey("superoverteamlist") || PlayerPrefs.HasKey("slogoverteamlist") || PlayerPrefs.HasKey("chasetargetteamlist") || PlayerPrefs.HasKey("multiplayerteamlist") || PlayerPrefs.HasKey(CONTROLLER.SUPER_Crusade_Teamlist))
		{
			CONTROLLER.isupdatePopup = true;
			PlayerPrefs.DeleteKey("teamlist");
			PlayerPrefs.DeleteKey("superoverteamlist");
			PlayerPrefs.DeleteKey("slogoverteamlist");
			PlayerPrefs.DeleteKey("multiplayerteamlist");
			PlayerPrefs.DeleteKey("chasetargetteamlist");
			PlayerPrefs.DeleteKey("SuperOverDetail");
			PlayerPrefs.DeleteKey("SlogOverDetail");
            PlayerPrefs.DeleteKey("slogovermatchid");
            PlayerPrefs.DeleteKey("ChaseTargetDetail");

			PlayerPrefs.DeleteKey(CONTROLLER.SUPER_Crusade_Teamlist);
			PlayerPrefs.DeleteKey(CONTROLLER.SUPER_Crusade_SavedMatchDetails);

			XMLReader.ParseXML(xmlAsset.text);
			PlayerPrefsManager.SetTeamList();
		}
		else
		{
			XMLReader.ParseXML(xmlAsset.text);
			PlayerPrefsManager.SetTeamList();
		}
	}

	

	public void GetSuperOverLevelDetails()
	{
		if (PlayerPrefs.HasKey("SuperOverLevelDetail"))
		{
			string levelDetailsStr = PlayerPrefs.GetString("SuperOverLevelDetail");
			string[] LevelDetailArray = levelDetailsStr.Split("|"[0]);
			CONTROLLER.CurrentLevelCompleted = int.Parse(LevelDetailArray[0] as string);
			CONTROLLER.LevelFailed = int.Parse(LevelDetailArray[1] as string);
			CONTROLLER.LevelId = int.Parse(LevelDetailArray[2] as string);
		}

		// ========== gopi  v1.1.2
		if (!PlayerPrefs.HasKey("SuperOverCompletedLevel"))
		{
			int[] tmArr = new int[CONTROLLER.totalLevels];
			for (int g = 0; g < CONTROLLER.totalLevels; g++)
				tmArr[g] = 0;
			for (int g = 0; g < CONTROLLER.CurrentLevelCompleted; g++)
				tmArr[g] = 1;

			string tmp2 = string.Empty;
			for (int i = 0; i < tmArr.GetLength(0); i++)
			{
				if (i + 1 < tmArr.GetLength(0))
					tmp2 += tmArr[i] + "-";
				else
					tmp2 += tmArr[i];
			}

			PlayerPrefs.SetString("SuperOverCompletedLevel", tmp2);
		}

		if (!PlayerPrefs.HasKey("SoLevFailedDet"))
		{
			//Debug.Log ("currentlevel completed  "+CONTROLLER .CurrentLevelCompleted +" level failed id: "+CONTROLLER .LevelFailed); 

			int[] tmArr = new int[CONTROLLER.totalLevels];
			for (int g = 0; g < CONTROLLER.totalLevels; g++)
			{
				tmArr[g] = 0;
				if (g == CONTROLLER.CurrentLevelCompleted && CONTROLLER.LevelFailed == 1)
					tmArr[g] = 1;
			}

			string tmp2 = string.Empty;
			for (int i = 0; i < tmArr.GetLength(0); i++)
			{
				if (i + 1 < tmArr.GetLength(0))
					tmp2 += tmArr[i] + "-";
				else
					tmp2 += tmArr[i];
			}

			PlayerPrefs.SetString("SoLevFailedDet", tmp2);
		}

		//==========end


		SetSuperOverLevels();
	}

	private void SetSuperOverLevels()
	{
		int i;
		for (i = 0; i < CONTROLLER.totalLevels; i++)
		{
			CONTROLLER.LevelCompletedArray[i] = 0;
		}

		if (PlayerPrefs.HasKey("SuperOverCompletedLevel"))
		{
			string tmp = PlayerPrefs.GetString("SuperOverCompletedLevel");
			CONTROLLER.LevelCompletedArray = tmp.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
		}
		else
		{
			for (i = 0; i < CONTROLLER.CurrentLevelCompleted; i++)
			{
				CONTROLLER.LevelCompletedArray[i] = 1;
			}
		}
	}

	public void GetChaseTargetLevelDetails()
	{
		if (PlayerPrefs.HasKey("ChaseTargetLevelDetail"))
		{
			string levelDetailsStr = PlayerPrefs.GetString("ChaseTargetLevelDetail");
			string[] LevelDetailArray = levelDetailsStr.Split("|"[0]);
			CONTROLLER.CTLevelCompleted = int.Parse(LevelDetailArray[0] as string);
			CONTROLLER.CTSubLevelCompleted = int.Parse(LevelDetailArray[1] as string);
		}


		if (!PlayerPrefs.HasKey("SuperChaseSubLevCompData"))
		{
			int[] tmArr = new int[] { 0, 0, 0, 0, 0 };

			for (int g = 0; g < CONTROLLER.CTSubLevelCompleted; g++)
				tmArr[g] = 1;

			string tmp2 = string.Empty;
			for (int i = 0; i < 5; i++)
			{
				if (i + 1 < tmArr.GetLength(0))
					tmp2 += tmArr[i] + "-";
				else
					tmp2 += tmArr[i];
			}
			PlayerPrefs.SetString("SuperChaseSubLevCompData", tmp2);
		}

		SetChaseTargetLevels();
	}

	public void SetChaseTargetLevels()
	{
		int i; bool flag = false;
		for (i = 0; i < CONTROLLER.TargetRangeArray.Length; i++)
		{
			CONTROLLER.MainLevelCompletedArray[i] = 0;
			if (i < CONTROLLER.CTLevelCompleted || (PlayerPrefs.GetString("SuperChaseSubLevCompData").Equals("1-1-1-1-1") && !flag))
			{
				CONTROLLER.MainLevelCompletedArray[i] = 1;
				flag = true;
			}
		}

		if (PlayerPrefs.HasKey("SuperChaseSubLevCompData"))
		{
			string tmp = PlayerPrefs.GetString("SuperChaseSubLevCompData");
			CONTROLLER.SubLevelCompletedArray = tmp.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
		}
		else
		{
			for (i = 0; i < 5; i++)
			{
				CONTROLLER.SubLevelCompletedArray[i] = 0;
				if (i < CONTROLLER.CTSubLevelCompleted)
				{
					CONTROLLER.SubLevelCompletedArray[i] = 1;
				}
			}
		}

	}

	public void SetArrayForChaseTarget()
	{
		//Debug.Log ("================Set Array For ChaseTarget called "); 
		if (PlayerPrefs.HasKey("ChaseTargetLevelDetail"))
		{
			string levelDetailsStr = PlayerPrefs.GetString("ChaseTargetLevelDetail");
			string[] LevelDetailArray = levelDetailsStr.Split("|"[0]);
			int _completedMainLevel = int.Parse(LevelDetailArray[0] as string);
			int _completedSubLevel = int.Parse(LevelDetailArray[1] as string);
			int i;

			if (CONTROLLER.CTCurrentPlayingMainLevel <= _completedMainLevel)
			{
				if (CONTROLLER.MainLevelCompletedArray[CONTROLLER.CTCurrentPlayingMainLevel] == 1)
				{
					for (i = 0; i < 5; i++)
					{
						CONTROLLER.SubLevelCompletedArray[i] = 1;
					}

					PlayerPrefs.SetString("SuperChaseSubLevCompData", "1-1-1-1-1"); //gopi
					return;
				}
			}


			if (PlayerPrefs.HasKey("SuperChaseSubLevCompData"))
			{
				string tmp = PlayerPrefs.GetString("SuperChaseSubLevCompData");
				CONTROLLER.SubLevelCompletedArray = tmp.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
			}
			else
			{
				for (i = 0; i < 5; i++)
				{
					CONTROLLER.SubLevelCompletedArray[i] = 0;
					if (i < _completedSubLevel)
					{
						CONTROLLER.SubLevelCompletedArray[i] = 1;
					}
				}
			}

		}
	}
	//End of LoadPlayerPref.cs

	//From save SavePlayerPrefs.cs
	public static void SetTeamList()
	{
		if (CONTROLLER.STORE == "facebook")
		{
			return;
		}
		string teamlist = "";
		teamlist += "<cricket>";

		for (int i = 0; i < CONTROLLER.TeamList.Length; i++)
		{
			teamlist += "<team name=\"" + CONTROLLER.TeamList[i].teamName + "\" abbrevation=\"" + CONTROLLER.TeamList[i].abbrevation + "\">";
			teamlist += "<PlayerDetails>";
			for (int k = 0; k < CONTROLLER.TeamList[i].PlayerList.Length; k++)
			{
				teamlist += "<player";
				teamlist += " name=\"" + CONTROLLER.TeamList[i].PlayerList[k].PlayerName + "\"";
				teamlist += " sname=\"" + CONTROLLER.TeamList[i].PlayerList[k].ShortName + "\"";
				teamlist += " num=\"" + CONTROLLER.TeamList[i].PlayerList[k].JerseyNumber + "\"";
				teamlist += " batHand=\"" + CONTROLLER.TeamList[i].PlayerList[k].BattingHand + "\"";
				teamlist += " batStyle=\"" + CONTROLLER.TeamList[i].PlayerList[k].Style + "\"";
				teamlist += " lp=\"" + CONTROLLER.TeamList[i].PlayerList[k].PlayerType + "\"";
				teamlist += " dp=\"" + CONTROLLER.TeamList[i].PlayerList[k].DefaultPlayer + "\"";
				teamlist += "/>";
			}
			teamlist += "</PlayerDetails>";
			teamlist += "</team>";
		}
		teamlist += "</cricket>";
		if (CONTROLLER.selectedGameMode == GameMode.SuperOver)
		{
			PlayerPrefs.SetString("superoverteamlist", teamlist);
		}
		else if (CONTROLLER.selectedGameMode == GameMode.OnlyBatting)
		{
			PlayerPrefs.SetString("slogoverteamlist", teamlist);
		}
		else if (CONTROLLER.selectedGameMode == GameMode.ChaseTarget)
		{
			PlayerPrefs.SetString("chasetargetteamlist", teamlist);
		}
		else if (CONTROLLER.selectedGameMode == GameMode.BattingMultiplayer)
		{
			PlayerPrefs.SetString("multiplayerteamlist", teamlist);
		}
		else if (CONTROLLER.selectedGameMode == GameMode.SUPER_Crusade_GameMode)
		{
			PlayerPrefs.SetString(CONTROLLER.SUPER_Crusade_Teamlist, teamlist);
		}
		PlayerPrefs.SetString("teamlist", teamlist);
	}

	public void GetSettingsList()
	{
		if (ObscuredPrefs.HasKey("SettingDetails"))
		{
			string settingsStr = ObscuredPrefs.GetString("SettingDetails");
			string[] settingsArray = settingsStr.Split("|"[0]);

			CONTROLLER.BGMusicVal = int.Parse(settingsArray[0] as string);
			CONTROLLER.GameMusicVal = int.Parse(settingsArray[1] as string);
			CONTROLLER.isMuted = int.Parse(settingsArray[2] as string);
			CONTROLLER.shotIndicator = int.Parse(settingsArray[3] as string);
		}
		else
		{
			CONTROLLER.BGMusicVal = 1;
			CONTROLLER.GameMusicVal = 1;
			CONTROLLER.isMuted = 1;
			CONTROLLER.shotIndicator = 1;
			SetSettingsList();
		}

		if (AudioPlayer.instance != null)
		{
			AudioPlayer.instance.MuteAudio(CONTROLLER.isMuted);
			AudioPlayer.instance.PlayOrStop_BGM(true);
		}
	}
	public static void SetSettingsList()
	{
		string settingsStr = "";
		settingsStr += CONTROLLER.BGMusicVal + "|";
		settingsStr += CONTROLLER.GameMusicVal + "|";
		settingsStr += CONTROLLER.isMuted + "|";
		settingsStr += CONTROLLER.shotIndicator;
		ObscuredPrefs.SetString("SettingDetails", settingsStr);
		ObscuredPrefs.Save();
	}
	//End of SavePlayerPrefs.cs

#region Save and Load Cloud Details
	public static void SaveProfile ()
	{
		ObscuredPrefs.SetString("token", CONTROLLER.TOKEN);
		ObscuredPrefs.SetInt("M_USERID", CONTROLLER.M_USERID);
		ObscuredPrefs.SetString("userid", CONTROLLER.UserID);
		ObscuredPrefs.SetString ("playername", CONTROLLER.UserName);
		ObscuredPrefs.SetString ("playeruniquename", CONTROLLER.UserUniqueName);
        ObscuredPrefs.SetString ("playeremailid",CONTROLLER.UserEmailID );
		ObscuredPrefs.SetString("userPlatformID", CONTROLLER.userPlatformID);
		ObscuredPrefs.SetString("profileplayeremailid", CONTROLLER.UserEmailID);
		ObscuredPrefs.SetString("profileplayerPicURL", CONTROLLER.profilePicURL);
		ObscuredPrefs.SetBool("profileloginstatus", CONTROLLER.bGooglePlayLoginSuccess);
		ObscuredPrefs.SetInt("LoginType", CONTROLLER.LoginType);
	}

	public static void LoadProfile ()
	{
		if (ObscuredPrefs.HasKey ("token")) 
		{
			CONTROLLER.TOKEN = ObscuredPrefs.GetString("token");
			CONTROLLER.M_USERID = ObscuredPrefs.GetInt("M_USERID");
			CONTROLLER.UserID = ObscuredPrefs.GetString("userid");
			CONTROLLER.UserName = ObscuredPrefs.GetString("playername");
            CONTROLLER.UserUniqueName = ObscuredPrefs.GetString("playeruniquename");

            CONTROLLER.UserEmailID = ObscuredPrefs.GetString("playeremailid");
			CONTROLLER.userPlatformID = ObscuredPrefs.GetString("userPlatformID");
			CONTROLLER.UserEmailID = ObscuredPrefs.GetString("profileplayeremailid");
			CONTROLLER.profilePicURL = ObscuredPrefs.GetString("profileplayerPicURL");
			CONTROLLER.bGooglePlayLoginSuccess = ObscuredPrefs.GetBool("profileloginstatus");
			CONTROLLER.LoginType = ObscuredPrefs.GetInt("LoginType");
		}

		if (PlayerPrefs.HasKey("jerseyidx"))
			CONTROLLER.JerseyIDX = PlayerPrefs.GetInt("jerseyidx");

		if (!PlayerPrefs.HasKey("tutcount"))
		{
			CONTROLLER.TutorialShowCount = 0;
			PlayerPrefs.SetInt("tutcount", 0);
        }
		else
			CONTROLLER.TutorialShowCount = PlayerPrefs.GetInt("tutcount");
	}


	public static void DeleteUserProfile()
	{
		ObscuredPrefs.DeleteKey("token");
		ObscuredPrefs.DeleteKey("M_USERID");
		ObscuredPrefs.DeleteKey("userid");
		ObscuredPrefs.DeleteKey("playername");
		ObscuredPrefs.DeleteKey("playeruniquename");
        ObscuredPrefs.DeleteKey("playeremailid");
		ObscuredPrefs.DeleteKey("userPlatformID");
		ObscuredPrefs.DeleteKey("profileplayeremailid");
		ObscuredPrefs.DeleteKey("profileplayerPicURL");
		ObscuredPrefs.DeleteKey("profileloginstatus");
		ObscuredPrefs.DeleteKey("LoginType");
		ObscuredPrefs.DeleteKey("iAP_removeAds");

		CONTROLLER.TOKEN = string.Empty;
		CONTROLLER.M_USERID = -1;
		CONTROLLER.UserID = string.Empty;
		CONTROLLER.UserName = "Guest";
		CONTROLLER.UserEmailID = string.Empty;
		CONTROLLER.userPlatformID = string.Empty;
		CONTROLLER.profilePicURL = string.Empty;
		CONTROLLER.bGooglePlayLoginSuccess = false;
		CONTROLLER.LoginType = -1;
		CONTROLLER.isAdRemoved = false;
	}
#endregion

#region Save and Load Coins
	public static void SaveCoins ()
	{
		ObscuredPrefs.SetInt ("TC", UserProfile.Tickets);
		ObscuredPrefs.SetInt ("EC", UserProfile.EarnedTickets);
		ObscuredPrefs.SetInt ("SC", UserProfile.SpentTickets);
		ObscuredPrefs.SetInt ("ST", UserProfile.spentTotTickets);
        ObscuredPrefs.SetInt(CONTROLLER.SavedName_Cp, CONTROLLER.CricketPoints);
    }

    public static void LoadCoins ()
	{
		if (ObscuredPrefs.HasKey ("TC"))
        {
			UserProfile.Tickets = ObscuredPrefs.GetInt ("TC");
			UserProfile.EarnedTickets = ObscuredPrefs.GetInt ("EC");
			UserProfile.SpentTickets = ObscuredPrefs.GetInt ("SC");
			UserProfile.spentTotTickets = ObscuredPrefs.GetInt ("ST");
            CONTROLLER.CricketPoints=ObscuredPrefs.GetInt(CONTROLLER.SavedName_Cp,0);
        }
        else
        {
			UserProfile.Tickets = 0;
			UserProfile.SpentTickets = 0;
			UserProfile.EarnedTickets = 0;
			UserProfile.spentTotTickets = 0;
            CONTROLLER.CricketPoints = 0;
            SaveCoins ();
		}
	}
#endregion

#region userPoints

	public static  void saveUserPoints()
	{
		ObscuredPrefs.SetInt ("gameusertotalpoints",CONTROLLER .gameTotalPoints );
		ObscuredPrefs.SetInt ("gameusersyncpoints", CONTROLLER .gameSyncPoint );
	}
	public static  void loadUserPoints()
	{
		if (ObscuredPrefs.HasKey ("gameusertotalpoints")) 
		{
			CONTROLLER .gameTotalPoints= ObscuredPrefs.GetInt ("gameusertotalpoints");
			CONTROLLER .gameSyncPoint = ObscuredPrefs.GetInt ("gameusersyncpoints");
		}
		else 
		{
			CONTROLLER .gameTotalPoints = 0;
			CONTROLLER .gameSyncPoint = 0;
			saveUserPoints ();
		}
	}
#endregion

#region googleplayprofile
	public static void saveuserGooglePlayStatus()
	{

	}

	public static void LoadGoogleplayStatus()
	{
		if (ObscuredPrefs.HasKey ("profileplayerid"))
		{
			CONTROLLER .userPlatformID=ObscuredPrefs.GetString("profileplayerid");
			CONTROLLER .UserName=ObscuredPrefs.GetString ("profileplayername");
			CONTROLLER .UserEmailID=ObscuredPrefs.GetString ("profileplayeremailid");
			CONTROLLER .profilePicURL = ObscuredPrefs.GetString ("profileplayerPicURL");
			CONTROLLER .bGooglePlayLoginSuccess =ObscuredPrefs .GetBool("profileloginstatus"); 
		}
	}
#endregion

#region Save and Load User Profile
	public static void SaveUserProfile ()
	{
		SaveCoins ();
		SaveProfile ();
		SaveSettings ();
		Save_iAP ();
		saveUserPoints (); 
		ObscuredPrefs.Save ();
	}

	public static void LoadUserProfile ()
	{
		LoadCoins ();
		LoadProfile ();
		LoadSettings ();
		Load_iAP ();
		loadUserPoints (); 
	}
#endregion

#region Save and Load Settings
	public static void SaveSettings ()
	{
		ObscuredPrefs.SetBool ("sfxState", SettingsPrefs.sfxState);
		ObscuredPrefs.SetBool ("musicState", SettingsPrefs.musicState);
		ObscuredPrefs.SetBool ("selectedGender", SettingsPrefs.selectedGender);
	}
	
	public static void LoadSettings ()
	{
		if (ObscuredPrefs.HasKey ("sfxState"))
		{
			SettingsPrefs.sfxState = ObscuredPrefs.GetBool ("sfxState");
			SettingsPrefs.musicState = ObscuredPrefs.GetBool ("musicState");
		}

		if (ObscuredPrefs.HasKey ("selectedGender"))
		{
			SettingsPrefs.selectedGender = ObscuredPrefs.GetBool ("selectedGender");
		}
	}
#endregion

#region Save and Load iAP
	public static void Save_iAP ()
	{
		ObscuredPrefs.SetBool ("iAP_removeAds", CONTROLLER.isAdRemoved);
	}
	
	public static void Load_iAP ()
	{
		if (ObscuredPrefs.HasKey ("iAP_removeAds"))
		{
			CONTROLLER.isAdRemoved = ObscuredPrefs.GetBool ("iAP_removeAds");
		}
	}
#endregion
}
