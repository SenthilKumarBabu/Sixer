#define DEBUGLOGGER

using System;
using System.Collections;
using System.IO;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class MultiplayerManager : MonoBehaviourPunCallbacks
{
    public static MultiplayerManager Instance;

    [HideInInspector]
    public MultiplayerGroundUIHandler multiplayerGroundUiHandlerScript;

    [HideInInspector] public string MasterName = string.Empty;
    [HideInInspector] public int sceneIndex = -1;
    private bool connectedToGameRoom = false;
    bool MatchType = false;
    [HideInInspector] public bool bothPlayersInRoom = false;
    [HideInInspector] public bool IsOpponentInOnline = false;

    void Awake()
    {
        SetInstance();
        SceneManager.sceneLoaded += OnSceneLoaded;
        PhotonNetwork.MinimalTimeScaleToDispatchInFixedUpdate = .1f;
    }
    public void SetInstance()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance.GetInstanceID() != GetInstanceID())
        {
            Destroy(gameObject);
            throw new System.Exception("Instance of " + Instance.GetType().FullName + " as " + Instance.gameObject.name + " Gameobject already exists..Removed other Instance= " + gameObject.name + " Gameobject with component added " + GetType().FullName);
        }
    }

    private void OnSceneLoaded(Scene aScene, LoadSceneMode aMode)
    {
        if (aScene.name == Scenes.MainMenu.ToString())
        {
            sceneIndex = 1;
        }
        if (aScene.name == Scenes.Ground.ToString() && (CONTROLLER.selectedGameMode == GameMode.BattingMultiplayer || CONTROLLER.selectedGameMode == GameMode.BatBowlMultiplayer))
        {
            sceneIndex = 2;
            Application.runInBackground = true;
            AdIntegrate.instance.SystemSleepSettings(0);
            BattingScoreCard.instance.HideMe();
            if (multiplayerGroundUiHandlerScript == null)
                multiplayerGroundUiHandlerScript = GameObject.FindFirstObjectByType<MultiplayerGroundUIHandler>();

            multiplayerGroundUiHandlerScript.WaitingPanel.SetActive(true);
            updateStausReCheck();
        }
    }

    private void updateStausReCheck()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.Joined && MultiplayerManager.Instance.IsInternetOn())
        {
            MultiplayerManager.Instance.UpdateMyStatus();
        }
        Invoke("updateStausReCheck", 3f);
    }
    public void CancelUpdateStatusRecheck()
    {
        CancelInvoke("updateStausReCheck");
    }
    public void ResetMyStatus()
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomVariables.masterSyncStatus, 0);
        hash.Add(RoomVariables.clientSyncStatus, 0);
        SetRoomCustomProperties(hash);
    }

    public void ResetMyScorePushStatus()
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomVariables.masterScorePushStatus, 0);
        hash.Add(RoomVariables.clientScorePushStatus, 0);
        SetRoomCustomProperties(hash);
    }


    public void UpdateMyStatus()
    {
        //DebugLogger.PrintWithColor("Update My status called:::: " + isConnectedWithPhoton());
        if (!isConnectedWithPhoton())
        {
            return;
        }
        //if (!CONTROLLER.gameCompleted)
        {
            //DebugLogger.PrintWithColor("Update My status called IsMasterClient () :::: " + IsMasterClient());

            if (IsMasterClient() == true)
            {
                ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
                hash.Add(RoomVariables.masterSyncStatus, 1);
                SetRoomCustomProperties(hash);
            }
            else
            {
                ExitGames.Client.Photon.Hashtable hash2 = new ExitGames.Client.Photon.Hashtable();
                hash2.Add(RoomVariables.clientSyncStatus, 1);
                SetRoomCustomProperties(hash2);
            }
        }
    }

    public void UpdateMyScorePushStatus(int totalScore, string currentRunScored, int currentMatchWicket)
    {
       // DebugLogger.PrintWithColor("Update My Score push status called:::: " + isConnectedWithPhoton() + "::: IsMasterClient()::: " + IsMasterClient() + " currentRunScored: " + currentRunScored);
        if (!isConnectedWithPhoton())
        {
            return;
        }
        SendRPC("ReceiveScoreUpdate", RpcTarget.Others, totalScore, currentRunScored, currentMatchWicket);
       // Debug.Log($"[MASTER] Sent Score: {totalScore}, Runs: {currentRunScored}, Wickets: {currentMatchWicket}");

        if (IsMasterClient() == true)
        {
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash.Add(RoomVariables.masterScorePushStatus, 1);
            SetRoomCustomProperties(hash);
        }
        else
        {
            ExitGames.Client.Photon.Hashtable hash2 = new ExitGames.Client.Photon.Hashtable();
            hash2.Add(RoomVariables.clientScorePushStatus, 1);
            SetRoomCustomProperties(hash2);
        }

        if(botsSpawned && botController!=null && CONTROLLER.selectedGameMode == GameMode.BattingMultiplayer)
        {
            botController.SendScoreForBattingMultiplayer();
        }
    }

    #region PhotonConnection
    public IEnumerator EnablePhotonConnection()
    {
        DebugLogger.PrintWithColor("############ EnablePhotonConnection ############# " + PhotonNetwork.NetworkClientState.ToString());
        PhotonNetwork.NetworkStatisticsEnabled = true;
        if (PhotonNetwork.NetworkClientState == ClientState.Disconnecting
            || PhotonNetwork.NetworkClientState == ClientState.DisconnectingFromGameServer || PhotonNetwork.NetworkClientState == ClientState.DisconnectingFromMasterServer
            || PhotonNetwork.NetworkClientState == ClientState.DisconnectingFromNameServer)
        {
            yield return StartCoroutine(WaitForDisconnectState());
        }
        else
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.SendRate = 12;

            try
            {
                if(CONTROLLER.selectedGameMode== GameMode.BattingMultiplayer)
                    PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = CONTROLLER.CURRENT_MULTIPLAYER_VERSION+"_batonly";
                else
                    PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = CONTROLLER.CURRENT_MULTIPLAYER_VERSION + "_batbowl";

                DebugLogger.PrintWithColor("CONTROLLER.selectedGameMode: " + CONTROLLER.selectedGameMode + " ::: " + PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion);

                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.NickName = CONTROLLER.UserName;
                yield break;
            }
            catch
            {
                StartCoroutine(EnablePhotonConnection());
            }
        }
    }

    IEnumerator WaitForDisconnectState()
    {
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.StopThread();
        yield return new WaitForSeconds(6f);
        StartCoroutine(EnablePhotonConnection());
    }
    public bool IsMasterClient()
    {
        if (MasterName != "CLI")
        {
            return true;
        }
        return false;
    }
    public bool getMasterClient()
    {
        return PhotonNetwork.IsMasterClient;
    }
    public int getPlayerCount()
    {
        return PhotonNetwork.PlayerList.Length;
    }
    public void DisConnectFromPhoton(bool resetSpec = true)
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        DisconnectPhoton();
    }
    public void DisconnectPhoton()
    {
        if (PhotonNetwork.IsConnected == true)
        {
            if (PhotonNetwork.InRoom == true)
            {
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
                PhotonNetwork.NetworkingClient.LoadBalancingPeer.StopThread();
            }
            else
            {
                PhotonNetwork.Disconnect();
                PhotonNetwork.NetworkingClient.LoadBalancingPeer.StopThread();
            }
        }
    }

    public bool IsInternetOn()
    {
       return AdIntegrate.instance.checkTheInternet();
    }
    public bool isConnectedWithPhoton()
    {
        if (IsInternetOn() && PhotonNetwork.NetworkClientState == ClientState.Joined && PhotonNetwork.InRoom)
        {
            return true;
        }
        return false;
    }
    
    #endregion

    #region PUN CALLBACK
    public override void OnConnectedToMaster()
    {
#if DEBUGLOGGER
        DebugLogger.PrintWithColor(" OnConnectedToMaster called ");
#endif
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }
    public override void OnJoinedLobby()
    {
#if DEBUGLOGGER
        DebugLogger.PrintWithColor(" on Joined lobby called sceneIndex: "+ sceneIndex );
#endif
        if (sceneIndex == 1)
        {
            //LoadingScreenPanel.instance.HideGenericLoading();
            MultiplayerManager.Instance.EnterPublicMode();
        }
    }

    public override void OnCreatedRoom()
    {
#if DEBUGLOGGER
            DebugLogger.PrintWithColor("OnCreatedRoom called :: " + IsMasterClient() + " room ID: " + PhotonNetwork.CurrentRoom.Name);
#endif


        LoadingScreen.instance.Hide();
        if (getMasterClient())
        {
            GameModeSelector._instance.StartTimerForMaster();
            GameModeSelector._instance.showMatchmakingScreen();
        }
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
#if DEBUGLOGGER
            DebugLogger.PrintWithColor("OnCreateRoomFailed failed  ." + returnCode + ":::" + message);
#endif
            if (CONTROLLER.MP_RoomType == 0)
            {
                PhotonNetwork.JoinRoom(null);
            }
    }

    public override void OnJoinedRoom()
    {
#if DEBUGLOGGER
        DebugLogger.PrintWithColor("OnPhoton join room called " + IsMasterClient() + "::: PhotonNetwork.IsMasterClient: "+ PhotonNetwork.IsMasterClient + " :: MasterName: "+ MasterName);
#endif

        if (sceneIndex == 1)
        {
            MasterName = string.Empty;
        }
        else if (sceneIndex == 2)
        {
           // AnalyticsController.Instance.logEvent("Other_Events", "Other_Events_Actions", "game_Reconnected");
        }

        if (MasterName == string.Empty)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                MasterName = CONTROLLER.UserName;
            }
            else
            {
                MasterName = "CLI";
            }
        }
        else if (MasterName != "CLI")
        {
            if (CONTROLLER.UserName == MasterName)
            {
                PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
            }
        }

        if (sceneIndex == 1)
        {
            DestroyBot();
            connectedToGameRoom = true;
            PhotonNetwork.NickName = CONTROLLER.UserID;
            LoadingScreen.instance.Hide();
            MatchType = false;
            if (!IsMasterClient())
            {
                GameModeSelector._instance.showMatchmakingScreen();
                GameModeSelector._instance.StartTimerForMaster();
                SendRPC("SendMyDetailsToOpponentLobby", Photon.Pun.RpcTarget.Others, CONTROLLER.UserName, CONTROLLER.UserID);
            }
            else
            {
                GameModeSelector._instance.showMatchmakingScreen();
                ResetMyStatus();

                if(CONTROLLER.selectedGameMode == GameMode.BattingMultiplayer)
                {
                    SetBowlingParameters();
                }

                if(CONTROLLER.isBotNeeded)
                    StartCoroutine(CheckAndSpawnBots());
            }
            if (getPlayerCount() == 2)
            {
                StopAllCoroutines();
            }
        }


    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
#if DEBUGLOGGER
            DebugLogger.PrintWithColor("OnJoinRoomFailed got called." + message);
#endif

    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
#if DEBUGLOGGER
      DebugLogger.PrintWithColor(" Random room join failed called : MP_RoomType:: " + CONTROLLER.MP_RoomType + "::::" + message);
#endif
        if (CONTROLLER.MP_RoomType == 0)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            roomOptions.EmptyRoomTtl = 0;
            roomOptions.PlayerTtl = 30000;  // 0;
            TypedLobby lobbyType = new TypedLobby("myLobby", LobbyType.AsyncRandomLobby);
            roomOptions.IsVisible = true;
            roomOptions.IsOpen = true;
            PhotonNetwork.CreateRoom(null, roomOptions, lobbyType);
        }
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
#if DEBUGLOGGER
        DebugLogger.PrintWithColor("OnPhoton OnPlayerEnteredRoom called ");
#endif

        if (botsSpawned)
        {
            Debug.Log("Bot already present, kicking late player...");
            PhotonNetwork.CloseConnection(other);
            return;
        }

        if (sceneIndex == 1)
        {
            if (getPlayerCount() == 2)
            {
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.CurrentRoom.IsOpen = false;
                StopAllCoroutines();
            }

            if (IsMasterClient())
            {
                bothPlayersInRoom = true;
                SendRPC("SendMyDetailsToOpponentLobby", Photon.Pun.RpcTarget.Others, CONTROLLER.UserName, CONTROLLER.UserID);
            }
            GameModeSelector._instance.StopPublicRoomTimer();
        }
        connectedToGameRoom = true;
    }

    public void OnDisconnected()
    {
#if DEBUGLOGGER
        DebugLogger.PrintWithColor(" OnDisconnectedFromPhoton IS DIS BY US ::  ::::: " + PhotonNetwork.IsConnected);
#endif
    }
    public override void OnPlayerLeftRoom(Player other)
    {
#if DEBUGLOGGER
            DebugLogger.PrintWithColor(" OnPlayer LeftRoom  called::: room Type: "+ CONTROLLER.MP_RoomType + " sceneIndex : "+sceneIndex);
#endif

        if(sceneIndex == 2)
        {
            if (multiplayerGroundUiHandlerScript != null)
                multiplayerGroundUiHandlerScript.OnButtonClickofGameOverScreen(0);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
            switch (cause)
            {
                case DisconnectCause.None:
                    break;
                case DisconnectCause.ExceptionOnConnect:
                    break;
                case DisconnectCause.Exception:
                    break;
                case DisconnectCause.ServerTimeout:
                    //case DisconnectCause.DisconnectByServer:
                    break;
                case DisconnectCause.ClientTimeout:
                    //case DisconnectCause.TimeoutDisconnect:
                    break;
                case DisconnectCause.DisconnectByServerLogic:
                    break;
                case DisconnectCause.DisconnectByServerReasonUnknown:
                    break;
                case DisconnectCause.InvalidAuthentication:
                    break;
                case DisconnectCause.CustomAuthenticationFailed:
                    break;
                case DisconnectCause.AuthenticationTicketExpired:
                    break;
                case DisconnectCause.MaxCcuReached:
                    //case DisconnectCause.DisconnectByServerUserLimit:
                    break;
                case DisconnectCause.InvalidRegion:
                    break;
                case DisconnectCause.OperationNotAllowedInCurrentState:
                    break;
                case DisconnectCause.DisconnectByClientLogic:
                    break;
                default:
                    break;
            }
#if DEBUGLOGGER
            DebugLogger.PrintWithColor("OnDisconnected called....Cause: " + cause);
#endif
    }


    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
/*#if DEBUGLOGGER
        foreach (var key in propertiesThatChanged.Keys)
        {
            DebugLogger.PrintWithColor($"OnRoomPropertiesUpdate Property Changed: {key} = {propertiesThatChanged[key]}");
        }
#endif*/

        if (propertiesThatChanged.ContainsKey(RoomVariables.masterSyncStatus) == true)
        {
            if (multiplayerGroundUiHandlerScript != null)
            {
                multiplayerGroundUiHandlerScript.skipMatchIntro();
            }
        }
        if (propertiesThatChanged.ContainsKey(RoomVariables.clientSyncStatus) == true)
        {
            if (multiplayerGroundUiHandlerScript != null)
            {
                multiplayerGroundUiHandlerScript.skipMatchIntro();
            }
        }

        if (sceneIndex == 2 && GameModel.instance != null)
        {
            if (propertiesThatChanged.ContainsKey(RoomVariables.masterScorePushStatus) == true || propertiesThatChanged.ContainsKey(RoomVariables.clientScorePushStatus) == true)
            {
                GameModel.instance.ScoreSyncedAndMoveToNextBall();
            }
        }
    }
    #endregion

 

    #region UI BUTTON CLICK EVENTS
    public void EnterPublicMode()
    {
        //CONTROLLER.DisconnectStatus = -1;
        //LoadingScreenPanel.instance.ShowGenericLoading("RivalsMatch", "Loading...Please wait...");
        MultiplayerManager.Instance.PublicRoomEvents();
    }
    public void PublicRoomEvents()
    {
        TypedLobby lobbyType = new TypedLobby("myLobby", LobbyType.AsyncRandomLobby);
        PhotonNetwork.JoinRandomRoom(null, 2, MatchmakingMode.RandomMatching, lobbyType, null);
    }
    #endregion

    #region RPC
    public void SendRPC(string methodName, RpcTarget target, params object[] parameters)
    {
        photonView.RPC(methodName, target, parameters);
    }
    public void SendRPC(string methodName, RpcTarget target)
    {
        photonView.RPC(methodName, target);
    }

    public void SetRoomCustomProperties(ExitGames.Client.Photon.Hashtable hash)
    {
        /*foreach (var key in hash.Keys)
        {
            Debug.Log($"SetRoomCustomProperties:: Key: {key}, Value: {hash[key]}");
        }*/

        if (PhotonNetwork.CurrentRoom != null)
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    }
    public static string GetTheRoomProperties(string key)
    {
        return (PhotonNetwork.CurrentRoom.CustomProperties[key]).ToString();
    }
    public static bool GetTheRoomPropertiesBool(string key, bool defVal)
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties[key] != null)
            return bool.Parse((PhotonNetwork.CurrentRoom.CustomProperties[key]).ToString());
        else
            return defVal;
    }
    public static float GetTheRoomPropertiesFloat(string key, float defVal)
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties[key] != null)
            return Globalization.FloatParse((PhotonNetwork.CurrentRoom.CustomProperties[key]).ToString());
        else
            return defVal;
    }
    public static byte GetTheRoomPropertiesByte(string key, byte defVal)
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties[key] != null)
            return byte.Parse((PhotonNetwork.CurrentRoom.CustomProperties[key]).ToString());
        else
            return defVal;
    }
    public static Vector3 GetTheRoomPropertiesVector(string key, Vector3 defVal)
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties[key] != null)
            return (Vector3)PhotonNetwork.CurrentRoom.CustomProperties[key];
        else
            return defVal;
    }
    public static int GetTheRoomPropertiesInt(string key, int defVal)
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties[key] != null)
            return int.Parse((PhotonNetwork.CurrentRoom.CustomProperties[key]).ToString());
        else
            return defVal;
    }
    public static string GetTheRoomPropertiesString(string key, string defVal)
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties[key] != null)
            return (PhotonNetwork.CurrentRoom.CustomProperties[key]).ToString();
        else
            return defVal;
    }

    public byte GetPhotonHashByte(string _vectorKey, byte defaultValue)
    {
        if (isConnectedWithPhoton())
        {
            if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.CustomProperties[_vectorKey] != null)
            {
                try
                {
                    return byte.Parse(PhotonNetwork.CurrentRoom.CustomProperties[_vectorKey].ToString());
                }
                catch (System.InvalidCastException e)
                {
                    return defaultValue;
                }
            }
            else
                return defaultValue;
        }
        else
        {
            return defaultValue;
        }
    }

    public int GetPhotonHashInt(string _vectorKey, int defaultValue)
    {
        if (isConnectedWithPhoton())
        {
            if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.CustomProperties[_vectorKey] != null)
            {
                return (int)PhotonNetwork.CurrentRoom.CustomProperties[_vectorKey];
            }
            else
            {
                return defaultValue;
            }
        }
        else
        {
            return defaultValue;
        }
    }
    public float GetPhotonHashFloat(string _vectorKey, float defaultValue)
    {
        if (isConnectedWithPhoton())
        {
            if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.CustomProperties[_vectorKey] != null)
            {
                return (float)PhotonNetwork.CurrentRoom.CustomProperties[_vectorKey];
            }
            else
                return defaultValue;
        }
        else
        {
            return defaultValue;
        }
    }

    public bool GetPhotonHashBool(string _vectorKey, bool defaultValue)
    {
        if (isConnectedWithPhoton())
        {
            if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.CustomProperties[_vectorKey] != null)
            {
                return (bool)PhotonNetwork.CurrentRoom.CustomProperties[_vectorKey];
            }
            else
                return defaultValue;
        }
        else
        {
            return defaultValue;
        }
    }
    #endregion

    public void CheckIfOpponentOnline()
    {
        SendRPC("CheckIfOpponentOnlineRPC", Photon.Pun.RpcTarget.Others);
    }

    [PunRPC]
    public void CheckIfOpponentOnlineRPC()
    {
        SendRPC("AcknowledgeOnlineStatus", Photon.Pun.RpcTarget.Others);
    }
    [PunRPC]
    public void AcknowledgeOnlineStatus()
    {
        IsOpponentInOnline = true;
        if (sceneIndex == 1)
        {
            if (GameModeSelector._instance!= null && GameModeSelector._instance.MatchMakingPanel.activeSelf)
            {
                GameModeSelector._instance.UserIsOnline();
            }
        }
        else if (sceneIndex == 2)
        {
            //if (PanelManager.LastShownPanelType() == PanelType.ManualField /*&& MP_AfkManager.instance.FieldingTime > 0f*/)
            //{
            //    PanelManager.GetRegisteredPanel<MP_ManualFieldPanel>(PanelType.ManualField).ProceedtoGame();
            //}
            //else
            //{
            //    MP_AfkManager.instance.TimerHitBackFunction();
            //}
        }
    }

    private void SetBowlingParameters()
    {
        if (!isConnectedWithPhoton())
        {
            return;
        }

        byte _bowlerType = 0;
        byte _bowlerSide = 0;
        byte _bowlerHand = 1;

        string _bowlingAngle = "0|0|0|0|0|0";
        string _bowlingSpeed = "3|4|5|6|7|9";

        // Create LHB spot array
       /* object[] lhb = new object[]
        {
            new object[] { "0.899", 0, "5.192" },
            new object[] { "0.134", 0, "7.029" },
            new object[] { "0.327", 0, "4.056" },
            new object[] { "0.574", 0, "5.729" },
            new object[] { "0.274", 0, "8.440" },
            new object[] { "0.615", 0, "6.701" }
        };

        // Create RHB spot array
        object[] rhb = new object[]
        {
            new object[] { -0.899, 0, "5.192" },
            new object[] { -0.134, 0, "7.029" },
            new object[] { -0.327, 0, "4.056" },
            new object[] { -0.574, 0, "5.729" },
            new object[] { -0.274, 0, "8.440" },
            new object[] { -0.615, 0, "6.701" }
        };

        // Wrap both in a hashtable
        ExitGames.Client.Photon.Hashtable bowlingSpotHash = new ExitGames.Client.Photon.Hashtable();
        bowlingSpotHash.Add("LHB", lhb);
        bowlingSpotHash.Add("RHB", rhb);*/

        ExitGames.Client.Photon.Hashtable bowlingSpotHash = GetRandomBowlingSpotsHash();
       // PhotonNetwork.CurrentRoom.SetCustomProperties(hash);


        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomVariables.bowlerType, _bowlerType);
        hash.Add(RoomVariables.bowlerSide, _bowlerSide);
        hash.Add(RoomVariables.bowlerHand, _bowlerHand);
        hash.Add(RoomVariables.bowlingAngle, _bowlingAngle);
        hash.Add(RoomVariables.bowlingSpeed, _bowlingSpeed);
        hash.Add(RoomVariables.bowlingSpot, bowlingSpotHash);  
        SetRoomCustomProperties(hash);

    }

    public static ExitGames.Client.Photon.Hashtable GetRandomBowlingSpotsHash()
    {
        float[] xLHB = { 0.899f, 0.134f, 0.327f, 0.574f, 0.274f, 0.615f };
        float[] xRHB = xLHB.Select(x => -x).ToArray();
        float[] zValues = { 5.192f, 7.029f, 4.056f, 5.729f, 8.440f, 6.701f };

        System.Random rng = new System.Random();
        var shuffledXLHB = xLHB.OrderBy(_ => rng.Next()).ToArray();
        var shuffledXRHB = xRHB.OrderBy(_ => rng.Next()).ToArray();
        var shuffledZ = zValues.OrderBy(_ => rng.Next()).ToArray();

        object[] lhb = new object[6];
        object[] rhb = new object[6];

        for (int i = 0; i < 6; i++)
        {
            lhb[i] = new object[] { shuffledXLHB[i].ToString("F3"), 0, shuffledZ[i].ToString("F3") };
            rhb[i] = new object[] { shuffledXRHB[i].ToString("F3"), 0, shuffledZ[i].ToString("F3") };
        }

        var bowlingSpotHash = new ExitGames.Client.Photon.Hashtable
        {
            { "LHB", lhb },
            { "RHB", rhb }
        };

        return bowlingSpotHash;
    }


    public void AssignBowlingParameters()
    {
        oppBallIndex = 0;userBallIndex = 0;
        // Fetch room properties using your utility methods
        byte _bowlerType = GetTheRoomPropertiesByte(RoomVariables.bowlerType, 0);
        byte _bowlerSide = GetTheRoomPropertiesByte(RoomVariables.bowlerSide, 0);
        byte _bowlerHand = GetTheRoomPropertiesByte(RoomVariables.bowlerHand, 0);

        string _bowlingAngleStr = GetTheRoomPropertiesString(RoomVariables.bowlingAngle, "0|0|0|0|0|0");
        string _bowlingSpeedStr = GetTheRoomPropertiesString(RoomVariables.bowlingSpeed, "3|4|5|6|7|9");

        // Clear existing data
        Multiplayer.oversData.Clear();

        MultiplayerOver newOver = new MultiplayerOver();

        // Assign bowlerType
        switch (_bowlerType)
        {
            case 0: newOver.bowlerType = "fast"; break;
            case 1: newOver.bowlerType = "offspin"; break;
            case 2: newOver.bowlerType = "legspin"; break;
            default: newOver.bowlerType = "fast"; break;
        }

        // Assign bowlerSide
        newOver.bowlerSide = (_bowlerSide == 0) ? "left" : "right";

        // Assign bowlerHand
        newOver.bowlerHand = (_bowlerHand == 0) ? "left" : "right";

        // Parse angle and speed strings into arrays
        string[] angleParts = _bowlingAngleStr.Split('|');
        string[] speedParts = _bowlingSpeedStr.Split('|');

        for (int j = 0; j < 6; j++)
        {
            int angle = (j < angleParts.Length && int.TryParse(angleParts[j], out var a)) ? a : 0;
            int speed = (j < speedParts.Length && int.TryParse(speedParts[j], out var s)) ? s : 0;

            newOver.bowlingAngle[j] = angle;
            newOver.bowlingSpeed[j] = speed;
        }

        // ? Parse bowlingSpot into Vector3[6] arrays
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RoomVariables.bowlingSpot, out object spotObj) &&
            spotObj is ExitGames.Client.Photon.Hashtable spotHash)
        {
            // LHB spots
            if (spotHash.ContainsKey("LHB") && spotHash["LHB"] is object[] lhbArray)
            {
                for (int i = 0; i < Mathf.Min(6, lhbArray.Length); i++)
                {
                    if (lhbArray[i] is object[] pos && pos.Length == 3)
                    {
                        float x = float.Parse(pos[0].ToString());
                        float y = float.Parse(pos[1].ToString());
                        float z = float.Parse(pos[2].ToString());

                        newOver.bowlingSpotL[i] = new Vector3(x, y, z);
                    }
                }
            }

            // RHB spots
            if (spotHash.ContainsKey("RHB") && spotHash["RHB"] is object[] rhbArray)
            {
                for (int i = 0; i < Mathf.Min(6, rhbArray.Length); i++)
                {
                    if (rhbArray[i] is object[] pos && pos.Length == 3)
                    {
                        float x = float.Parse(pos[0].ToString());
                        float y = float.Parse(pos[1].ToString());
                        float z = float.Parse(pos[2].ToString());

                        newOver.bowlingSpotR[i] = new Vector3(x, y, z);
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("bowlingSpot not found or in unexpected format.");
        }



        // Add the new over to the overs list
        Multiplayer.oversData.Add(newOver);

    }

    [HideInInspector] public int oppBallIndex = 0;
    [HideInInspector] public int userBallIndex = 0;
    [PunRPC]
    public void ReceiveScoreUpdate(int totalScore, string currentRunScored, int currentWicket)
    {
       // Debug.Log($"[CLIENT] Received Score Update → Total: {totalScore}, Runs This Ball: {currentRunScored}, Wickets: {currentWicket}");

        multiplayerGroundUiHandlerScript.OppScore.text = totalScore + "/" + currentWicket;
        CONTROLLER.oppBallbyBallData[oppBallIndex % 6] = currentRunScored;

        multiplayerGroundUiHandlerScript.scorePopupAnimationScript.ShowScore(CONTROLLER.oppBallbyBallData[oppBallIndex], false);
        oppBallIndex++;

        // Update UI
        for (int i = 0; i < 6; i++)
        {
            multiplayerGroundUiHandlerScript.OppBallInfo[i].text = CONTROLLER.oppBallbyBallData[i];
        }
    }

    #region BOT LOGIC
    [HideInInspector] public bool botsSpawned = false;
    IEnumerator CheckAndSpawnBots()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(2f,5f)); // wait for others to join

        if (isConnectedWithPhoton() && PhotonNetwork.CurrentRoom.PlayerCount < 2 && PhotonNetwork.IsMasterClient && !botsSpawned)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            
            botsSpawned = true;
            StopAllCoroutines();

            bothPlayersInRoom = true;

            //GameModeSelector._instance.GetOpponentDetails("BotUser", "BOT_001");
            var bot = GetRandomBot();
            GameModeSelector._instance.GetOpponentDetails(bot.botName, bot.botID);
            
            GameModeSelector._instance.StopPublicRoomTimer();
        }
    }

    [HideInInspector]public BotController botController;
    public void SpawnBot()
    {
        if (botController == null)
        {
            Debug.Log("Bot spawned!");
            GameObject botGO = new GameObject("BotPlayer");
            botController = botGO.AddComponent<BotController>();
            botController.InitAsBot();
        }
    }
    public void DestroyBot()
    {
        if (botsSpawned)
        {
            GameObject bot = GameObject.Find("BotPlayer");
            if (bot != null)
            {
                Destroy(bot);
            }
        }
        botsSpawned = false;
        botController = null;
    }
    public (string botName, string botID) GetRandomBot()
    {
        // Predefined bot list (more like real player names)
        string[,] botProfiles = new string[,]
        {
        {"RaviKumar", "BOT001"},
        {"ArjunSingh", "BOT002"},
        {"KaranSharma", "BOT003"},
        {"ManojPatel", "BOT004"},
        {"SandeepReddy", "BOT005"},
        {"VikramJoshi", "BOT006"},
        {"AnilVerma", "BOT007"},
        {"RajeshNair", "BOT008"},
        {"PradeepYadav", "BOT009"},
        {"SureshPillai", "BOT010"}
        };

        int index = UnityEngine.Random.Range(0, botProfiles.GetLength(0));
        return (botProfiles[index, 0], botProfiles[index, 1]);
    }


    #endregion 
}
