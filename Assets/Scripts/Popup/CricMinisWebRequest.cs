using Newtonsoft.Json.Linq;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class CricMinisWebRequest : MonoBehaviour
{

    public static CricMinisWebRequest instance = null;
    Dictionary<string, string> dict = new Dictionary<string, string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    #region LOGIN
    // =================================================== NEW API CALLS ========================================//
    public void CheckForLogin()     
    {
        if (AdIntegrate.instance.checkTheInternet())
        {
            LoadingScreen.instance.Show();
            StartCoroutine(GetAuth());
        }
        else
            Popup.instance.ShowNoInternetPopup();
    }

    private IEnumerator GetAuth()
    {
        dict.Clear();
        dict.Add("did", CONTROLLER.DeviceID);
        dict.Add("bv", "" +CONTROLLER.CURRENT_VERSION);
        dict.Add("os", CONTROLLER.TargetPlatform);
        dict.Add("fcm", CONTROLLER.FCM_Token);
        if (CONTROLLER.LoginType == 1)
        {
            dict.Add("type", "gp");
            dict.Add("uuid", CONTROLLER.userPlatformID);
        }
        else if (CONTROLLER.LoginType == 2)
        {
            dict.Add("type", "gu");
        }
        else if (CONTROLLER.LoginType == 3)
        {
            dict.Add("type", "al");
           // dict.Add("uuid", GooglePlayLogin.Appleuserid);
        }

        UnityWebRequest WebRequest = UnityWebRequest.Post(CONTROLLER.BASE_URL+ "users/auth", dict);
        WebRequest.timeout = 20;
        yield return WebRequest.SendWebRequest();

      //  DebugLogger.PrintWithColor("############## GET AUTH CALL RESPONSE #########: " + WebRequest.downloadHandler.text);

        if (WebRequest.result != UnityWebRequest.Result.ConnectionError && WebRequest.result != UnityWebRequest.Result.DataProcessingError)
        {
            JSONNode data = JSONNode.Parse(WebRequest.downloadHandler.text);
            int status = int.Parse(data["status"]);
            if (status == 200)
            {
                CONTROLLER.TOKEN = data["data"]["token"];
                CONTROLLER.UserID = data["data"]["uid"];
                if (CONTROLLER.UserID == "false")
                {
                    StartCoroutine(RegisterUser());
                }
                else
                {
                    StartCoroutine(LoginUser());
                }
            }
            else
                ProcessError(WebRequest);
        }
        else
        {
            ProcessError(WebRequest);
        }
    }

    private IEnumerator RegisterUser()
    {
        dict.Clear();
        if (CONTROLLER.LoginType == 1)
        {
            dict.Add("type", "gp");
            dict.Add("name", CONTROLLER.UserName);
            dict.Add("uuid", CONTROLLER.userPlatformID);
        }
        else if (CONTROLLER.LoginType == 2)
        {
            dict.Add("type", "gu");
        }
        else if (CONTROLLER.LoginType == 3)
        {
            dict.Add("type", "al");
            dict.Add("name", CONTROLLER.UserName);
            //dict.Add("uuid", GooglePlayLogin.Appleuserid);
        }

        UnityWebRequest WebRequest = UnityWebRequest.Post(CONTROLLER.BASE_URL + "users/register", dict);
        WebRequest.timeout = 20;
        WebRequest.SetRequestHeader("token", CONTROLLER.TOKEN);
        yield return WebRequest.SendWebRequest();

       // DebugLogger.PrintWithColor("############## GET REGISTER CALL RESPONSE #########: " + WebRequest.downloadHandler.text);
        if (WebRequest.result != UnityWebRequest.Result.ConnectionError && WebRequest.result != UnityWebRequest.Result.DataProcessingError)
        {
            JSONNode data = JSONNode.Parse(WebRequest.downloadHandler.text);
            int status = int.Parse(data["status"]);
            if (status == 200)
            {
                CONTROLLER.TOKEN = data["data"]["token"];
                CONTROLLER.UserID = data["data"]["uid"];
                CONTROLLER.M_USERID = data["data"]["mid"].AsInt;
                CONTROLLER.UserName = data["data"]["d_name"];
                CONTROLLER.UserUniqueName = data["data"]["name"];

                if (int.Parse(data["data"]["adfree"]) == 1)
                    CONTROLLER.isAdRemoved = true;
                else
                    CONTROLLER.isAdRemoved = false;

                StartCoroutine(postLoginCalls());
            }
            else if(status == 409)
            {
                StartCoroutine(LoginUser());
            }
            else
            {
                ProcessError(WebRequest);
            }
        }
        else
        {
            ProcessError(WebRequest);
        }
    }

    private IEnumerator LoginUser()
    {
        dict.Clear();
        if (CONTROLLER.LoginType == 1)
        {
            dict.Add("type", "gp");
            dict.Add("name", CONTROLLER.UserName);
            dict.Add("uuid", CONTROLLER.userPlatformID);
        }
        else if (CONTROLLER.LoginType == 2)
        {
            dict.Add("type", "gu");
        }
        else if (CONTROLLER.LoginType == 3)
        {
            dict.Add("type", "al");
            dict.Add("name", CONTROLLER.UserName);
            //dict.Add("uuid", GooglePlayLogin.Appleuserid);
        }

        UnityWebRequest WebRequest = UnityWebRequest.Post(CONTROLLER.BASE_URL + "users/login", dict);
        WebRequest.timeout = 20;
        WebRequest.SetRequestHeader("token", CONTROLLER.TOKEN);
        yield return WebRequest.SendWebRequest();

       // DebugLogger.PrintWithColor("############## GET LOGIN CALL RESPONSE #########: " + WebRequest.downloadHandler.text);
        if (WebRequest.result != UnityWebRequest.Result.ConnectionError && WebRequest.result != UnityWebRequest.Result.DataProcessingError)
        {
            JSONNode data = JSONNode.Parse(WebRequest.downloadHandler.text);
            int status = int.Parse(data["status"]);

            if (status == 200)
            {
                CONTROLLER.TOKEN = data["data"]["token"];
                CONTROLLER.UserID = data["data"]["uid"];
                CONTROLLER.M_USERID = data["data"]["mid"].AsInt;
                CONTROLLER.UserName = data["data"]["d_name"];
                CONTROLLER.UserUniqueName = data["data"]["name"];

                if (int.Parse(data["data"]["adfree"]) == 1)
                    CONTROLLER.isAdRemoved = true;
                else
                    CONTROLLER.isAdRemoved = false;



                StartCoroutine( postLoginCalls() );
            }
            else
                ProcessError(WebRequest);
        }
        else
        {
            ProcessError(WebRequest);
        }
    }

    IEnumerator postLoginCalls()
    {

        yield return StartCoroutine(UserSync(true,true));
        PlayerPrefsManager.SaveUserProfile();
        LoadingScreen.instance.Hide();
        SignInPanel.instance.Hide();
        GameModeSelector._instance.ShowLandingPage(true);

        if (CONTROLLER.canFetchDeeplink && AdIntegrate.instance.checkTheInternet())
        {
            CONTROLLER.canFetchDeeplink = false;
            DeeplinkPopup.instance.checkForDeeplinking();
        }
    }

    // ===================================================END OF NEW API CALLS ========================================//

    public bool CanCallSync()
    {
        if (UserProfile.EarnedTickets > 0 || UserProfile.SpentTickets > 0 || CONTROLLER.gameSyncPoint != 0)
            return true;
        else
            return false;
    }

    public IEnumerator UserSync(bool isBackGroundCall = true, bool ForceCall = false)
    {
        if (AdIntegrate.instance.checkTheInternet() && CONTROLLER.IsUserLoggedIn() && (CanCallSync() || ForceCall))
        {
            // input: userid, etickets, stickets, eticketskey, sticketskey, epoints, epointskey
            int startTime = JWTHandler.GetStartTime();
            int expiryTime = startTime + 30;

            var payload = new Dictionary<string, object>()
                {
                    { "iss", "nextwave" },
                        { "sub", new Dictionary<string, object>(){
                            {"userid",CONTROLLER.UserID},
                            {"etickets",UserProfile.EarnedTickets},
                            {"stickets",UserProfile.SpentTickets },
                            {"epoints",CONTROLLER.gameSyncPoint },
                            {"platform",CONTROLLER.Platform },
                            {"version",CONTROLLER.CURRENT_VERSION }
                            }
                        },
                    { "exp", expiryTime }
                };

            string textToSend = JWTHandler.GetJwtToken(payload);

            // DebugLogger.PrintWithColor("############# USER SYNC JWT #########: " + textToSend);

            UnityWebRequest request = null;
            request = UnityWebRequest.PostWwwForm(CONTROLLER.BASE_URL + "users/sync", textToSend);
            request.SetRequestHeader("Content-Type", "text/plain");
            request.SetRequestHeader("token", CONTROLLER.TOKEN);
            request.timeout = 15;
            yield return request.SendWebRequest();
            // DebugLogger.PrintWithColor("###### RESPONSE FOR UserSync CALL::: ######:  " + request.downloadHandler.text);

            if (request.result != UnityWebRequest.Result.ConnectionError && request.result != UnityWebRequest.Result.DataProcessingError)
            {
                JSONNode data = JSON.Parse(request.downloadHandler.text);
                int status = int.Parse(data["status"]);
                if (status == 200)
                {
                    CONTROLLER.forceSync = false;
                    if (!string.IsNullOrEmpty(request.GetResponseHeader("Tickets")))
                    {
                        UserProfile.Tickets = int.Parse(request.GetResponseHeader("Tickets"));
                    }
                    if (!string.IsNullOrEmpty(request.GetResponseHeader("Tpoints")))
                    {
                        CONTROLLER.gameTotalPoints = int.Parse(request.GetResponseHeader("Tpoints"));
                    }

                    if (data["data"]["nas"] != null)
                    {
                        CONTROLLER.CanShowAdtoNewUser_Inter = int.Parse(data["data"]["nas"]);
                    }

                    if (data["data"]["nasb"] != null)
                    {
                        CONTROLLER.CanShowAdtoNewUser_Banner = int.Parse(data["data"]["nasb"]);
                    }

                    PlayerPrefs.SetInt("nas", CONTROLLER.CanShowAdtoNewUser_Inter);
                    PlayerPrefs.SetInt("nasb", CONTROLLER.CanShowAdtoNewUser_Banner);

                    //DebugLogger.PrintWithColor("###### INSIDE USER SYNC::: CAN SHOW AD TO NEW USER: " + CONTROLLER.CanShowAdtoNewUser_Inter +"  "+ CONTROLLER.CanShowAdtoNewUser_Banner +" CONTROLLER.isUserSyncCalled ::  " + CONTROLLER.isUserSyncCalled);

                    UserProfile.EarnedTickets = 0;
                    UserProfile.SpentTickets = 0;
                    CONTROLLER.gameSyncPoint = 0;

                    PlayerPrefsManager.SaveUserProfile();
                    if (AdIntegrate.instance.CurrentSceneIndex == 1)
                        GameModeSelector._instance.UpdateLandingPageTopBars();
                    //Debug.LogError("CONTROLLER.isUserSyncCalled ::  " +CONTROLLER.isUserSyncCalled);
                    if (CONTROLLER.isUserSyncCalled == false)
                    {
                        CONTROLLER.isUserSyncCalled = true;
                        if (CONTROLLER.CanShowAdtoNewUser_Banner == 1)
                        {
                            AdIntegrate.instance.RequestBanner();
                        }
                        if (CONTROLLER.CanShowAdtoNewUser_Inter == 1)
                        {
                            StartCoroutine(AdIntegrate.instance.RequestInterestialAd());
                        }
                    }
                }
            }
            else
            {
                if (!isBackGroundCall)
                    ProcessError(request);
            }
        }
        else
        {
            GameModeSelector._instance.UpdateLandingPageTopBars();
            yield break;
        }

        CONTROLLER.CanShowAdtoNewUser_Inter = PlayerPrefs.GetInt("nas", 1);
        CONTROLLER.CanShowAdtoNewUser_Banner = PlayerPrefs.GetInt("nasb", 1);
    }
    public void ProceedSignout(bool canshowToast = true)
    {
        ClearControllerVariables();
        PlayerPrefsManager.DeleteUserProfile();
        #region store
        PlayerPrefs.DeleteKey("Googleplayprofpic");
        PlayerPrefs.DeleteKey("TempReceipt");
        Receipts.ClearReceipts();
        Receipts.ClearTempReceipts();
        #endregion store

        GameModeSelector._instance.UpdateLandingPageTopBars();
        PlayerPrefs.DeleteKey(CONTROLLER.SavedName_Cp);
        PlayerPrefs.DeleteKey(CONTROLLER.BatMpCpSavedName);

#if UNITY_ANDROID && !UNITY_EDITOR
		GameModeSelector._instance.userProfilePic.sprite = GameModeSelector._instance.defaultProfilePic;
GoogleManagerScript.instance.Signout (); 
#endif

        PlayerPrefs.DeleteKey("teamlist");

        PlayerPrefs.DeleteKey("tutcount");
        PlayerPrefs.DeleteKey("SoLevFailedDet");
        PlayerPrefs.DeleteKey("SuperOverLevelDetail");
        PlayerPrefs.DeleteKey("superoverteamlist");
        PlayerPrefs.DeleteKey("SuperOverDetail");
        PlayerPrefs.DeleteKey("superoverPlayerDetails");
        PlayerPrefs.DeleteKey("SuperOverCompletedLevel");

        PlayerPrefs.DeleteKey("slogoverteamlist");
        PlayerPrefs.DeleteKey("SlogOverDetail");
        PlayerPrefs.DeleteKey("slogovermatchid");
        PlayerPrefs.DeleteKey("slogoverPlayerDetails");

        PlayerPrefs.DeleteKey("multiplayerteamlist");

        
        PlayerPrefs.DeleteKey("ChaseTargetLevelDetail");
        PlayerPrefs.DeleteKey("chasetargetteamlist");
        PlayerPrefs.DeleteKey("ChaseTargetDetail");
        PlayerPrefs.DeleteKey("chasetargetPlayerDetails");
        PlayerPrefs.DeleteKey("SuperChaseSubLevCompData");
        

        if (canshowToast)
        {
            LoadingScreen.instance.Hide();
            if(Settings.instance!=null)
                Settings.instance.closeSettings();
            SignInPanel.instance.Show();
            PlayerPrefsManager.instance.loadData();
            Popup.instance.showGenericPopup("LOGOUT", "Signed-Out Successfully");
        }
    }

    private void ClearControllerVariables()
    {
        CONTROLLER.TutorialShowCount = 0;
        UserProfile.Tickets = 0;
        UserProfile.SpentTickets = 0;
        UserProfile.EarnedTickets = 0;
        CONTROLLER.gameTotalPoints = 0;
        CONTROLLER.gameSyncPoint = 0;

        CONTROLLER.CricketPoints = 0;
        CONTROLLER.isBatMpContestRunning = false;
        CONTROLLER.isBatMpContestFormShown = false;
        if (AdIntegrate.instance != null)
            AdIntegrate.instance.isRegisteredContestUser = false;
        CONTROLLER.LevelCompletedArray = new int[18];
        CONTROLLER.LevelId = 0;
        CONTROLLER.CurrentLevelCompleted = 0;
        CONTROLLER.totalLevels = 18;
        CONTROLLER.totalFours = 0;
        CONTROLLER.totalSixes = 0;
        CONTROLLER.continousBoundaries = 0;
        CONTROLLER.continousSixes = 0;
        CONTROLLER.LevelFailed = 0;


        CONTROLLER.CTLevelId = 0;//0-5
        CONTROLLER.CTSubLevelId = 0;//0-4
        CONTROLLER.CTLevelCompleted = 0;//0-5
        CONTROLLER.CTSubLevelCompleted = 0;//0-4
        CONTROLLER.CTCurrentPlayingMainLevel = 0;
        CONTROLLER.MainLevelCompletedArray = new int[6];
        CONTROLLER.SubLevelCompletedArray = new int[5];
        CONTROLLER.TargetRun = 0;

        CONTROLLER.currentMatchScores = 0;
        CONTROLLER.currentMatchBalls = 0;
        CONTROLLER.currentMatchWickets = 0;
        CONTROLLER.currentBallNumber = 0;

        CONTROLLER.BGMusicVal = 1;
        CONTROLLER.GameMusicVal = 1;
        CONTROLLER.shotIndicator = 1;
        CONTROLLER.isMuted = 1;
        PlayerPrefsManager.SaveSettings();

        CONTROLLER.BmpMaintenance = -1;
        CONTROLLER.BmpMaintenanceText = string.Empty;
        CONTROLLER.isUserSyncCalled = false;
        CONTROLLER.CanShowAdtoNewUser_Banner = 1;
        CONTROLLER.CanShowAdtoNewUser_Inter = 1;
        CONTROLLER.canFetchDeeplink = true;
        DeeplinkPopup.isFirstTimeShow = false;
    }

#endregion


#region LEADERBOARD
public IEnumerator sendPointstoLeaderboard(int points, bool forceUpate = false)
    {

        if (!forceUpate)
        {
            CONTROLLER.gameSyncPoint += points;
            CONTROLLER.gameTotalPoints += points;
        }

        if (AdIntegrate.instance.checkTheInternet() && CONTROLLER.IsUserLoggedIn() && CONTROLLER.gameSyncPoint!=0 )
        {
            int startTime = JWTHandler.GetStartTime();
            int expiryTime = startTime + 30;

            var payload = new Dictionary<string, object>()
                {
                    { "iss", "nextwave" },
                        { "sub", new Dictionary<string, object>(){
                            {"uid",CONTROLLER.UserID},
                            {"epoint",CONTROLLER.gameSyncPoint},
                            {"platform",CONTROLLER.Platform },
                            {"version",CONTROLLER.CURRENT_VERSION }
                            }
                        },
                    { "exp", expiryTime }
                };

            string textToSend = JWTHandler.GetJwtToken(payload);


            UnityWebRequest request = null;
            request = UnityWebRequest.PostWwwForm(CONTROLLER.BASE_URL + "users/result", textToSend);
            request.SetRequestHeader("Content-Type", "text/plain");
            request.SetRequestHeader("token", CONTROLLER.TOKEN);
            request.timeout = 15;
            yield return request.SendWebRequest();

          //  DebugLogger.PrintWithColor("###### RESPONSE FOR RESULT API CALL::: #####:  " + request.downloadHandler.text);

            if (request.result != UnityWebRequest.Result.ConnectionError && request.result != UnityWebRequest.Result.DataProcessingError)
            {
                JSONNode node = JSON.Parse(request.downloadHandler.text);
                int status = int.Parse(node["status"]);
                if (status == 200)
                {
                    CONTROLLER.gameSyncPoint = 0;
                    CONTROLLER.gameTotalPoints = int.Parse(node["data"]["epoints"]);
                }
            }
            else
            {
                if (!forceUpate)
                    ProcessError(request);
                else
                {
                    canGetLbData = false;
                    ProcessError(request, true, GameModeSelector._instance.ForceCloseLeaderboard);
                }
            }
        }

        PlayerPrefsManager.saveUserPoints();

    }
    bool canGetLbData = true;
    public IEnumerator getPointsfromLeaderBoard()
    {
        //type 0--All time   1--weekly 	2--monthly
        if (!LoadingScreen.instance.holder.activeSelf)
            LoadingScreen.instance.Show("Loading Leaderboard...");
        yield return new WaitForSeconds(0.5f);

        bool isSuccess = false;
        canGetLbData = true;

        if (AdIntegrate.instance.checkTheInternet())
        {
            yield return StartCoroutine(sendPointstoLeaderboard(CONTROLLER.gameSyncPoint, true));

            if (!canGetLbData)
                yield break;

            UnityWebRequest WebRequest = UnityWebRequest.Get(CONTROLLER.BASE_URL + "users/leaderboard-new");
            WebRequest.SetRequestHeader("token", CONTROLLER.TOKEN);
            WebRequest.timeout = 15;
            yield return WebRequest.SendWebRequest();

            //DebugLogger.PrintWithColor("###### RESPONSE FOR Get LeaderBoard CALL::: response: " + WebRequest.downloadHandler.text);
            if (WebRequest.result != UnityWebRequest.Result.ConnectionError && WebRequest.result != UnityWebRequest.Result.DataProcessingError)
            {
                JSONNode node = JSON.Parse(WebRequest.downloadHandler.text);
                int status = int.Parse(node["status"]);
                if (status == 200)
                {
                    int count = int.Parse("" + node["data"]["week"]["list"].Count);
                    CONTROLLER.WeeklyList = new CONTROLLER.LeaderBoard[count];
                    for (int g = 0; g < count; g++)
                    {
                        CONTROLLER.WeeklyList[g] = new CONTROLLER.LeaderBoard("" + node["data"]["week"]["list"][g]["name"], "" + node["data"]["week"]["list"][g]["rank"], "" + node["data"]["week"]["list"][g]["xp"]);
                        CONTROLLER.WeeklyList[g].UserName = "" + node["data"]["week"]["list"][g]["name"];
                        CONTROLLER.WeeklyList[g].UserRank = "" + node["data"]["week"]["list"][g]["rank"];
                        CONTROLLER.WeeklyList[g].UserPoints = "" + node["data"]["week"]["list"][g]["xp"];
                    }
                    CONTROLLER.myRankWeekly = new CONTROLLER.LeaderBoard("" + node["data"]["week"]["myrank"]["name"], "" + node["data"]["week"]["myrank"]["rank"], "" + node["data"]["week"]["myrank"]["xp"]);
                    CONTROLLER.myRankWeekly.UserName = "" + node["data"]["week"]["myrank"]["name"];
                    CONTROLLER.myRankWeekly.UserRank = "" + node["data"]["week"]["myrank"]["rank"];
                    CONTROLLER.myRankWeekly.UserPoints = "" + node["data"]["week"]["myrank"]["xp"];

                    count = int.Parse("" + node["data"]["month"]["list"].Count);
                    CONTROLLER.MonthlyList = new CONTROLLER.LeaderBoard[count];
                    for (int g = 0; g < count; g++)
                    {
                        CONTROLLER.MonthlyList[g] = new CONTROLLER.LeaderBoard("" + node["data"]["month"]["list"][g]["name"], "" + node["data"]["month"]["list"][g]["rank"], "" + node["data"]["month"]["list"][g]["xp"]);
                        CONTROLLER.MonthlyList[g].UserName = "" + node["data"]["month"]["list"][g]["name"];
                        CONTROLLER.MonthlyList[g].UserRank = "" + node["data"]["month"]["list"][g]["rank"];
                        CONTROLLER.MonthlyList[g].UserPoints = "" + node["data"]["month"]["list"][g]["xp"];
                    }
                    CONTROLLER.myRankMonthly = new CONTROLLER.LeaderBoard("" + node["data"]["month"]["myrank"]["name"], "" + node["data"]["month"]["myrank"]["rank"], "" + node["data"]["month"]["myrank"]["xp"]);
                    CONTROLLER.myRankMonthly.UserName = "" + node["data"]["month"]["myrank"]["name"];
                    CONTROLLER.myRankMonthly.UserRank = "" + node["data"]["month"]["myrank"]["rank"];
                    CONTROLLER.myRankMonthly.UserPoints = "" + node["data"]["month"]["myrank"]["xp"];


                    count = int.Parse("" + node["data"]["all"]["list"].Count);
                    CONTROLLER.AllTimeList = new CONTROLLER.LeaderBoard[count];
                    for (int g = 0; g < count; g++)
                    {
                        CONTROLLER.AllTimeList[g] = new CONTROLLER.LeaderBoard("" + node["data"]["all"]["list"][g]["name"], "" + node["data"]["all"]["list"][g]["rank"], "" + node["data"]["all"]["list"][g]["xp"]);
                        CONTROLLER.AllTimeList[g].UserName = "" + node["data"]["all"]["list"][g]["name"];
                        CONTROLLER.AllTimeList[g].UserRank = "" + node["data"]["all"]["list"][g]["rank"];
                        CONTROLLER.AllTimeList[g].UserPoints = "" + node["data"]["all"]["list"][g]["xp"];
                    }
                    CONTROLLER.myRankAllTime = new CONTROLLER.LeaderBoard("" + node["data"]["all"]["myrank"]["name"], "" + node["data"]["all"]["myrank"]["rank"], "" + node["data"]["all"]["myrank"]["xp"]);
                    CONTROLLER.myRankAllTime.UserName = "" + node["data"]["all"]["myrank"]["name"];
                    CONTROLLER.myRankAllTime.UserRank = "" + node["data"]["all"]["myrank"]["rank"];
                    CONTROLLER.myRankAllTime.UserPoints = "" + node["data"]["all"]["myrank"]["xp"];

                    //open LEADERBOARD
                    GameModeSelector._instance.leaderboard.SetActive(true);
                    CONTROLLER.CurrentPage = "leaderboardpage";

                    isSuccess = true;
                }
            }
            else
                ProcessError(WebRequest);
        }
        else
        {
            Popup.instance.ShowNoInternetPopup();
        }



        if (LeaderBoardclass.instance != null)
        {
            if (LoadingScreen.instance.holder.activeSelf)
                LoadingScreen.instance.Hide();
            if (isSuccess)
            {
                LeaderBoardclass.instance.ListUserNames("weekly");
            }

        }

    }

    #endregion




    void ShowToast(string msg)
    {
        GameObject prefabGO;
        GameObject tempGO;
        prefabGO = Resources.Load("Prefabs/Toast") as GameObject;
        tempGO = Instantiate(prefabGO) as GameObject;
        tempGO.name = "Toast";
        tempGO.GetComponent<Toast>().setMessge(msg);
    }

    public void ProcessError(UnityWebRequest response, bool isJWT = false, BoolDelegate OnSuccess = null)
    {
       // DebugLogger.PrintWithColor("###### PROCESS ERROR:::: "+response.error);
        LoadingScreen.instance.Hide();

        if (!string.IsNullOrEmpty(response.downloadHandler.text))
        {
            int statusCode, code = -1;
            JSONNode _node = JSONNode.Parse(response.downloadHandler.text);
            statusCode = _node["status"].AsInt;
            if (_node["code"] != null)
            {
                code = _node["code"].AsInt;
            }
            switch (statusCode)
            {
                case 500:
                    showGenericPopup("UNEXPECTED ERROR", "Unable to establish a connection due to an internal error.\nPlease retry later.");
                    OnSuccess?.Invoke(false);
                    break;
                case 512://Internal Status Code if session code is unable to fetch
                    showGenericPopup("UNEXPECTED ERROR", "Unable to fetch session ID.\nPlease retry later.");
                    OnSuccess?.Invoke(false);
                    break;
                case 503:
                    showGenericPopup("UNEXPECTED ERROR", "Unable to process the request at the moment.\nPlease retry later.");
                    OnSuccess?.Invoke(false);
                    break;
                case 400:
                    if (!isJWT)
                    {
                        showGenericPopup("UNEXPECTED ERROR", "Unable to process the request due to missing parameters.\nPlease retry later.");
                        OnSuccess?.Invoke(false);
                    }
                    else
                    {
                        if (code == 1)
                        {
                            showGenericPopup("UNEXPECTED ERROR", "Unable to process the request due to invalid JWT Token.\nPlease retry later.");
                            OnSuccess?.Invoke(false);
                        }
                        else
                        {
                            ShowSomethingWentWrong();
                            OnSuccess?.Invoke(false);
                        }
                    }
                    break;
                case 403:
                    switch (code)
                    {
                        case 1:
                            //StartCoroutine(GetAuth());
                            break;
                        case 3:
                            //Popup.instance.Show(heading: "UNEXPECTED ERROR", message: "Only one active device can be accessed at a time!\nSign out and Sign in again to make this the active device.", yesString: "SIGN OUT", yesCallBack: OnMultipleLogin);
                            Popup.instance.Show(heading: "", message: "You have already logged in with many devices,\nkindly logout from those devices and try again!", yesString: "SIGN OUT", yesCallBack: OnMultipleLogin);
                            OnSuccess?.Invoke(false);
                            break;
                        case 4:
                            OnSuccess?.Invoke(false);
                            break;
                        default:
                            ShowSomethingWentWrong();
                            OnSuccess?.Invoke(false);
                            break;
                    }
                    break;
                case 426:
                    OnSuccess?.Invoke(false);
                    break;
                default:
                    ShowSomethingWentWrong();
                    OnSuccess?.Invoke(false);
                    break;
            }
        }
        else
        {
            //DebugLogger.PrintWithColor("######### PROCESS ERROR CALLBACK NAME: " + OnSuccess.Method.Name);
            OnSuccess?.Invoke(false);

            if (response.error.Contains("Request timeout"))
                showGenericPopup("TIMED OUT", "Please retry later.");
            else
                ShowSomethingWentWrong();
        }
    }


    public void showGenericPopup(string _title, string _msg, UnityAction callBack = null)
    {
        Popup.instance.showGenericPopup(_title, _msg, callBack);
    }
    public void ShowTimeOutPopup(UnityAction callBack = null)
    {
        Popup.instance.showGenericPopup("TIMED OUT", "Please retry later.", callBack);
    }

    public void ShowSomethingWentWrong(UnityAction callBack = null)
    {
        Popup.instance.showGenericPopup("OOPS!!!", "Something went wrong. \nPlease try again later.", callBack);
    }
    public void ShowNoInternetPopup(UnityAction callBack = null)
    {
        Popup.instance.showGenericPopup("NO INTERNET!", "No Internet Connection detected.Please check Network Settings and Retry", callBack);
    }

    private void OnMultipleLogin()
    {
      //  DebugLogger.PrintWithColor("########### On Multiple Login Found ################");
    }
    
    public IEnumerator SyncData(string secondaryPath,Dictionary<string,string> dict, System.Action<string> onSuccess =null,ServerRequest type=ServerRequest.GET,bool isBackGroundCall=false)  
    {
        string path = CONTROLLER.BASE_URL + secondaryPath;
       // LogDict(dict);

        UnityWebRequest WebRequest;
        if (type == ServerRequest.GET)
            WebRequest = UnityWebRequest.Get(path);
        else
            WebRequest = UnityWebRequest.Post(path, dict);
        WebRequest.SetRequestHeader("token", CONTROLLER.TOKEN);
        WebRequest.SetRequestHeader("skip-jwt", "1");

        WebRequest.timeout = 15;
        yield return WebRequest.SendWebRequest();
       
        //DebugLogger.PrintWithColor("API REQ:::: UID: " + CONTROLLER.UserID + " url: " + path + " Response: " + WebRequest.downloadHandler.text);

        if (WebRequest.result != UnityWebRequest.Result.ConnectionError && WebRequest.result != UnityWebRequest.Result.DataProcessingError)
        {
            if (!string.IsNullOrEmpty(WebRequest.GetResponseHeader("Tickets")))
            {
                UserProfile.Tickets = int.Parse(WebRequest.GetResponseHeader("Tickets"));
            }
            onSuccess?.Invoke(WebRequest.downloadHandler.text);
        }
        else
        {
            if (!isBackGroundCall)
                ProcessError(WebRequest);
        }
    }
    public IEnumerator GetJson(string URL, System.Action<string> onSuccess = null)
    {
        UnityWebRequest WebRequest = UnityWebRequest.Get(URL);
        WebRequest.SetRequestHeader("token", CONTROLLER.TOKEN);
        WebRequest.SetRequestHeader("skip-jwt", "1");

        WebRequest.timeout = 15;
        yield return WebRequest.SendWebRequest();
       // DebugLogger.PrintWithColor("API REQ::::  url: " + URL + " Response: " + WebRequest.downloadHandler.text);

        if (WebRequest.result != UnityWebRequest.Result.ConnectionError && WebRequest.result != UnityWebRequest.Result.DataProcessingError)
        {
            onSuccess?.Invoke(WebRequest.downloadHandler.text);
        }
        else
        {
            if (URL.Equals(CONTROLLER.serverConfig) == true)
            {
                ServerConfigReader.instance.FeedValuesFromBuild();
            }
           //ProcessError(WebRequest);
        }
    }
    public IEnumerator VerifyIAPRecipt( JObject purchaseReceipt,string prodID, System.Action<string> onSuccess)
    {
       // DebugLogger.PrintWithColor("###########VerifyIAPRecipt:: prodID: " + prodID + " ::: SyncDataText" + purchaseReceipt);

        int startTime = JWTHandler.GetStartTime();
        int expiryTime = startTime + 30;

        var payload = new Dictionary<string, object>();
        if (CONTROLLER.IsUserLoggedIn())
        {
            payload = new Dictionary<string, object>()
                {
                    { "iss", "nextwave" },
                        { "sub", new Dictionary<string, object>()
                            {
                                { "pdet",purchaseReceipt},
                                {"pid",prodID },
                                {"platform",CONTROLLER.Platform },
                                {"version",CONTROLLER.CURRENT_VERSION },
                                { "uid",CONTROLLER.UserID }
                            }
                        },
                    { "exp", expiryTime }
                };
        }
        else
        {
            payload = new Dictionary<string, object>()
                {
                    { "iss", "nextwave" },
                    { "aud", "cricminis" },
                        { "sub", new Dictionary<string, object>()
                            {
                                { "pdet",purchaseReceipt},
                                {"pid",prodID },
                                {"platform",CONTROLLER.Platform },
                                {"version",CONTROLLER.CURRENT_VERSION },
                                { "did", CONTROLLER.DeviceID }
                            }
                        },
                    { "exp", expiryTime }
                };
        }


        string textToSend = JWTHandler.GetJwtToken(payload);

        string URL = string.Empty;
#if UNITY_EDITOR
        URL = "users/testinapp";
#elif UNITY_ANDROID || UNITY_IOS
			URL="users/inapp";
#endif
        UnityWebRequest request = null;
        request = UnityWebRequest.PostWwwForm(CONTROLLER.BASE_URL + URL, textToSend);
        request.SetRequestHeader("Content-Type", "text/plain");
        request.SetRequestHeader("token", CONTROLLER.TOKEN);
        request.timeout = 15;
        yield return request.SendWebRequest();
      //  DebugLogger.PrintWithColor("###### RESPONSE FOR IAP validation CALL:::URL:  " + URL +" response: " + request.downloadHandler.text);

        if (request.result != UnityWebRequest.Result.ConnectionError && request.result != UnityWebRequest.Result.DataProcessingError)
        {
            onSuccess?.Invoke(request.downloadHandler.text);
        }
        else
        {
            ProcessError(request);
        }
    }

    void LogDict(Dictionary<string, string> dict)
    {
        if (dict == null)
            return;
        //foreach (var items in dict)
        //    DebugLogger.PrintWithColor("###### KEY: " + items.Key + " value: " + items.Value);
    }

    public IEnumerator SleepTest(System.Action<string> onSuccess = null)
    {
        string path = CONTROLLER.BASE_URL + "users/sleep";
        UnityWebRequest WebRequest = UnityWebRequest.Get(path);
        WebRequest.SetRequestHeader("token", CONTROLLER.TOKEN);
        WebRequest.SetRequestHeader("skip-jwt", "1");

        WebRequest.timeout = 10;
        yield return WebRequest.SendWebRequest();

        if (WebRequest.result != UnityWebRequest.Result.ConnectionError && WebRequest.result != UnityWebRequest.Result.DataProcessingError)
        {
            onSuccess?.Invoke(WebRequest.downloadHandler.text);
        }
        else
        {
            ProcessError(WebRequest);
        }
    }
}
