using UnityEngine;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class CONTROLLER
{
    public static int totalOversPracticeMode=5;
    public static string CURRENT_MULTIPLAYER_VERSION = "0.1";
    public static int MP_RoomType = 0;       //0-public  1-private
    public static string MP_OpponentName = string.Empty; // Unique Username
    public static string MP_Opponent_ud_uID;
	public static string[] userBallbyBallData = new string[6];
	public static string[] oppBallbyBallData = new string[6];

    public static bool isUserSyncCalled = false;
	public static bool isAdminUser = false;

	public static int BmpMaintenance = -1;
	public static string BmpMaintenanceText = string.Empty;

	public static string TOKEN = string.Empty;
	public static string UserID=string.Empty;
	public static string UserName = "Guest";
	public static string UserUniqueName = "Guest";
    public static string UserEmailID =string.Empty;
	public static string userPlatformID = string.Empty;
	public static string profilePicURL = string.Empty;
	public static int LoginType = -1;    //1-google 2- guest 3-apple 
	public static bool bGooglePlayLoginSuccess=false;
	public static int JerseyIDX = 0;
	public static int M_USERID = -1;
	public static string DeviceID = string.Empty;
	public static bool isAdRemoved = false; // 0- ads 1- removed 

	public static int TutorialShowCount = 0;
	public static bool forceSync = true;
#if UNITY_ANDROID
    public static string VersionString = "1.0";
	public static int CURRENT_VERSION = 2;
#elif UNITY_IOS
    public static string Version = "1.0";
    	public static int CURRENT_VERSION = 1;
#else
	public static int CURRENT_VERSION=1;
#endif
	public static string STORE = "googleplay";      //mac, intel, ios, googleplay, kindle, samsung, chrome, facebook
	public static string TargetPlatform = "";   //standalone, android, ios, web
	public static string Platform = TargetPlatform;

	public static string AppName = "SixR";
	public static bool IsUserLoggedIn()
	{
		if (string.IsNullOrEmpty(UserID))
			return false;
		else
			return true;
	}

    public static int EnableHardcodes = 0;
	public static bool CanShowAdToast = false;

	public static bool canFetchDeeplink=true;
	//public static int zaprEnabled = 0;
	public static bool isUserInitiatedThePurchase = false;
    public static bool CanShowFBPersonalizedAds;
    public static bool  Squadupdatednotify = false;
	public static string current_Season = "IPL14";
	public const float DefaultWidth = 1024.0f;
	public const float DefaultHeight = 768.0f;
	public const int totalBallInOver = 6;
	public static float crowdDensity;
	public static string selectedGround;

    public static bool GameIsOnFocus = true;

    public static string gameMode = "slogover";		//superover, slogover, chasetarget,supercards
	public static bool isPlayingChallenge = true;
	public static string CurrentPage = "";
	public static bool canShowMainCamera = true;
	public static Vector3 HIDEPOS = new Vector3((Screen.width+9500), -(Screen.height+9500), -10);
	public static Vector3 SHOWPOS = new Vector3(0, 0, 0);
	
	public static TeamInfo [] TeamList;
	public static string bowlerType = "fast";//fast,spin
	public static string[] ballUpdate = new string[6];
	public static string difficultyMode = "medium"; // easy, medium, hard
	public static int [] Overs = new int[6];
	public static int totalWickets = 10;
	public static int totalOvers;
	public static int currentMatchScores;
	public static int currentMatchBalls;
	public static int currentMatchWickets;
	public static int currentBallNumber;
	public static int fielderChangeIndex = 1;
	public static int computerFielderChangeIndex = 0;
	public static int closefieldIndex = 1;
	public static int openfieldIndex = 1;
	public static int totalPlayers = 25;
    public static int currentAIUniformColor = 0;
	// new
	public static int InterfaceCameraHeight = 384;//Target height is 768. So (768/2)
	public static int myTeamIndex = 0;
	public static int opponentTeamIndex = 1;
	public static int oversSelectedIndex;
	
	public static int BattingTeamIndex;
	public static int BowlingTeamIndex;
	public static string BattingTeamName;
	public static string BowlingTeamName;
	public static bool HattrickBall;
	
	public static int [] FieldersArray;
	
	public static int StrikerIndex = 0;
	public static int NonStrikerIndex = 1;
	public static string StrikerHand;
	public static string NonStrikerHand;
	
	public static int CurrentBowlerIndex;
	public static int BowlerType;
	public static string BowlerHand;
	
	public static int wickerKeeperIndex;
	
	public static int isBowlingControl;		//	0 --> BowlingInterface, 1 --> Not in Bowling Interface
	
	public static bool GoodMatch;
	public static int currentInnings;
	public static int TargetToChase;
	
	public static int RunsInAOver;
	public static int WideInAOver;
	public static int DotInAOver;
	public static bool NewOver;
	public static bool InBetweenOvers;
	
	public static bool PowerPlay = false;
	public static float RunRate;
	public static float ReqRunRate;
	public static int BowlingSpeed;		// 1-10;
	public static float BowlingAngle;		// 1-10;
	public static float BowlingSwing; 	// 0-4;
	public static int TournamentSixes;
	public static int SixDistance;
	
	public static bool postToFB = false;
	public static string FBUserName;
	
	public static ArrayList PlayingTeam;		// default "1" in xml.. Playing XI for example
	
	// Confidence Level
	public static bool isConfidenceLevel = false;
	public static bool SlogOvers = false;
	// Confidence Level
	
	// Settings
	public static int BGMusicVal = 1;
	public static int GameMusicVal = 1;
	public static int shotIndicator = 1; // 1 || 0
	public static string BatsmanHand = "right";
	public static int isMuted = 1;
	// Settings
	
	public static bool NewInnings = true;
	public static float xOffSet;
	public static float ScreenWidth;
	public static float ScreenHeight;
	public static string adType;
	public static bool receivedAdEvent = false;
    public static bool launchInternetAdEvent = false;
    public static bool CanShowAd = true;
	
	public static int totalPoints;
	public static int TempPoint;
	public static int boundaryPoint = 40;
	public static int sixPoint = 60;
	public static int singlePoint = 10;
	public static int doublePoint = 20;
	public static int wicketPoint = -100;
	public static int dotBallPoint = -10;
	public static string BowlingEnd = "madrasclub";//"madrasclub","pattabigate"
	//public static bool LandedInGround = false;
	/******SuperOver******/
	public static int[] LevelCompletedArray = new int[18];
	public static int LevelId = 0;
	public static int CurrentLevelCompleted = 0;
	public static int totalLevels = 18;
	public static int totalFours;
	public static int totalSixes;
	public static int continousBoundaries;
	public static int continousSixes;
	public static bool InningsCompleted = false;
	public static int LevelFailed = 0;
	/******SuperOver******/
	
	/******ChaseThe Target******/
	public static string[] TargetRangeArray = new string[6];
	public static int CTLevelId = 0;//0-5
	public static int CTSubLevelId = 0;//0-4
	public static int CTLevelCompleted = 0;//0-5
	public static int CTSubLevelCompleted = 0;//0-4
	public static int CTCurrentPlayingMainLevel;
	public static int[] MainLevelCompletedArray = new int[6];
	public static int[] SubLevelCompletedArray = new int[5];
	public static int[] StartRangeArray = new int[5];
	public static int[] EndRangeArray = new int[5];
	public static bool isReplayGame = false;
	public static int TargetRun = 0;


	/******ChaseThe Target******/

	public static LeaderBoard [] WeeklyList;
	public static LeaderBoard [] MonthlyList;
	public static LeaderBoard [] AllTimeList;

	public static LeaderBoard myRankWeekly;
	public static LeaderBoard myRankAllTime;
	public static LeaderBoard myRankMonthly;

	public static bool billingSupported;
	public static int AdHeight;
	
	public static bool isupdatePopup = false;
	
	// For Custom ad
	public static string InGameFullAdLinkURL;
	public static int adInterval = 6;
	public static int superOverAdInterval = 1;
	public static int ServerAdType=0;
	public static string InGameFullAdBannerURL;
	public static bool adImageDownloaded;
	public static bool isShowingFullScreenAd;

	public static bool canPressBackBtn = false;
	public static bool tempCanPressBackBtn = false;
	public static string tempCurrentPage = "";

	public static int gameTotalPoints=0, gameSyncPoint=0;


	public static int RewardedVideoClickedState = -1;

    #region SUPER_XYZ_GAMEMODE
    public static string SUPER_Crusade_GameMode = "superMatchesGamemode";
    public static string SUPER_Crusade_Teamlist = "superXyzTeamList";
    public static string SUPER_Crusade_SavedMatchDetails = "superXyzSavedMatchDetails";
    public static string SUPER_Crusade_PlayerDetails = "superXyzPlayerDetails";
    public static string SUPER_Crusade_Instructions = "Super matches allows you to retake the journey of CSK with the new squad through the IPL seasons.\n\nEvery match is played for 20 overs length and will require the gamer to chase a pre-set score which is based on the score set by the team during the IPL seasons. The primary objective is based on the match score set or chased by CSK and must be completed to progress further. The secondary and the tertiary objectives are loosely based on that match stats. You must complete the primary objective in order to claim the other objectives.\n\nYou get the following rewards for the completion of each match:\n\nPrimary objective: 1 Ticket\n\nSecondary Objective: 1 Ticket\n\nTertiary objective: 2 Tickets";
    public static string SUPER_Crusade_SavedName= "superXyzSeasonDetails";
    public static string SUPER_Crusade_WatchVideoLevelID= "crusadewatchvideolevelids";
    public static int SelectedCrusadeSeasonIdx=-1, SelectedCrusadeMatchIdx =-1, seasonUnlock=11;
	public static string SeasonValue="";
	public static int openStore = 0;
    #endregion

    //Gopi
    #region CONTEST
    public static bool isBatMpContestRunning = false;
    public static bool isBatMpContestFormShown = false;
    public static ObscuredInt CricketPoints = 0;
    public static string SavedName_Cp = "lk35jf45f21ds";
    public static string BatMpCpSavedName = "iuw25hrefbjr";
    public static int[] BatMPRewards_CP = new int[] { 30, 15, 5, -10, -20 };
    public static string BattingMultiplayerCountryCode = "W-";
    #endregion


    public static void GetRandomScoreForOppTeam (int StartRange, int EndRange)
	{
		TargetToChase = UnityEngine.Random.Range(StartRange, EndRange);
	}

	public static bool isThisDayOne()
	{
		if(!PlayerPrefs.HasKey("dayone"))
		{
			PlayerPrefs.SetString("dayone", System.DateTime.Now.ToBinary().ToString());
			return true;
		}
		else
		{
			long temp = Convert.ToInt64(PlayerPrefs.GetString("dayone"));
			DateTime oldDate = DateTime.FromBinary(temp);
			TimeSpan difference = DateTime.Now.Subtract(oldDate);
			if(difference.TotalHours < 24)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}

	public static bool isThisFirstPush(int type)
	{
		//0-login	//1-start	2-complete

		string key = string.Empty;
		if (type == 0)
			key = "Fologin";
		else if (type == 1)
			key = "Fostart";
		else
			key = "Focomplete";

		if (!PlayerPrefs.HasKey(key))
		{
			PlayerPrefs.SetInt(key, 1);
			return true;
		}
		else
			return false;
	}

	public static string NumberSystem (int number)
	{
		string str = number.ToString ();
		string finalStr = str;
		if(str.Length > 3)
		{
			string last3Digits = str.Substring(str.Length - 3, 3);
			string remainingDigits = str.Substring(0, str.Length-3);
			finalStr = ","+last3Digits;

			string last2Digits;
			while (remainingDigits.Length > 2)
			{
				last2Digits = remainingDigits.Substring(remainingDigits.Length - 2, 2);
				remainingDigits = remainingDigits.Substring(0, remainingDigits.Length-2);
				finalStr = ","+last2Digits + finalStr;
			}
			finalStr = remainingDigits + finalStr;
		}
		return finalStr;
	}
	public static string UppercaseFirst(string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return string.Empty;
		}
		char[] a = s.ToCharArray();
		a[0] = char.ToUpper(a[0]);
		return new string(a);
	}
	public class TeamInfo
	{
		public string teamName;
		public string abbrevation;
		public PlayerInfo[] PlayerList;
		//public int currentMatchScores;
		//public int currentMatchBalls;
	}
	
	public class PlayerInfo
	{
		public string PlayerName;
		public string ShortName;
		public string JerseyNumber;
		public string BattingHand;
		public string Style;
		public string PlayerType;
		public string DefaultPlayer;
		
		public int RunsScored;
		public int BallsPlayed;
		public int Fours;
		public int Sixes;
		public int StrikeRate;
		public int FallOfWicket;
		public string status;
	}
	
	public class LeaderBoard
	{
		public string UserName;
		public string UserRank;
		public string UserPoints;

		public LeaderBoard (string name,string rank,string point)
		{
			UserName = name;
			UserRank =rank;
			UserPoints =point;
		}

	}
}

public enum ShotStatus : byte
{
	None = 0,
	PERFECT = 4,
	GOOD = 5,
	EARLY_NICETRY = 3,
	EARLY = 2,
	LATE = 6,
	TOO_EARLY = 1,
	TOO_LATE = 7
}