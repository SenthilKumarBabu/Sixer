//#define SUPERCARDS_MULTIPLAYER
using System.Collections;
using UnityEngine;
#if SUPERCARDS_MULTIPLAYER
using Photon.Realtime;
using Photon.Pun;
#endif

#if SUPERCARDS_MULTIPLAYER
public class MultiplayerManager : MonoBehaviourPunCallbacks
#else
public class MultiplayerManager : MonoBehaviour
#endif
{
    public static MultiplayerManager instance;

    string[] customRoomPropertiesForLobby = { RoomProperty.CURRENT_VERSION, RoomProperty.KICK_PLAYER_OUT_INSTANTLY };

    private void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;
    }

    public void EstablishConnection()
    {
        resetValues();
#if SUPERCARDS_MULTIPLAYER
        PhotonNetwork.ConnectUsingSettings();
#endif
    }

#if SUPERCARDS_MULTIPLAYER
    public override void OnConnectedToMaster()
    {
        //Debug.Log("OnConnectedToMaster");
        if (isReConnection == false)
        {
            //CreateRoom();
            PhotonNetwork.JoinRandomRoom(expectedRoomProperties(), 2);
        }
    }
# endif

#if SUPERCARDS_MULTIPLAYER
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom();
    }
#endif

    public void disconnectPhoton()
    {
#if SUPERCARDS_MULTIPLAYER
        PhotonNetwork.Disconnect();
#endif
    }

#if SUPERCARDS_MULTIPLAYER
    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;


        roomOptions.CustomRoomPropertiesForLobby = customRoomPropertiesForLobby;
        roomOptions.CustomRoomProperties = expectedRoomProperties();
        roomOptions.PlayerTtl = 0;
        roomOptions.EmptyRoomTtl = 0;
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    private ExitGames.Client.Photon.Hashtable expectedRoomProperties()
    {
        return new ExitGames.Client.Photon.Hashtable { { customRoomPropertiesForLobby[0], 1 }, { customRoomPropertiesForLobby[1], KICK_PLAYER_OUT_IF_MINIMIZE } };
    }

    public override void OnJoinedRoom()
    {
        if (isReConnection == false)

        {
            SuperCardsUI.instance.updateMyName("" + PhotonNetwork.LocalPlayer.ActorNumber);
            //Invoke("test", 10f);  
            if (PhotonNetwork.CurrentRoom.PlayerCount > 1) // This will be called to client only
            {
                sendMyDetailsToOpponent();
            }
            roomName = PhotonNetwork.CurrentRoom.Name;
            isJoinedInRoom = true;
        }
        else
        {
            //connectionText.text = "Re Joined...";
            if (isGameStarted)
            {
                reconnecting_Multiplayer(true);
                sendMyFocusStatus(true);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) // This will be called to master only

    {
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            //SuperCardsUI.instance.ShowMe();
            //SuperCardsUI.instance.TurnOnMasterButton(); 
            sendMyDetailsToOpponent();
        }
    }


    public override void OnPlayerLeftRoom(Player otherPlayer) 
    {
        if (isGameStarted)
        {
            //if (GameController.instance.isGameCompleted() == false)
            //{
                if (KICK_PLAYER_OUT_IF_MINIMIZE)
                {
                    SuperCardsUI.instance.ShowGameOverScreen(true, "OPPONENT LEFT");
                    stopCoroutines();
                }
                else
                {
                    //wait till reconnection time and display result
                    opponentReconnectionDelayCoroutine = StartCoroutine(waitForOpponentToReconnect(PhotonNetwork.CurrentRoom.PlayerTtl / 1000f));
                    reconnecting_Multiplayer(false, true);
                }
            //}
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //Debug.Log("cause == " + cause);
        if (isJoinedInRoom == false)
        {
            //Popup.instance.Show(headerMsg: "", msg: "Connection lost!", okButtonMsg: "TRY AGAIN", okButtonAction: onClick_TryAgain_MultiPlayer, cancelButtonMsg: "CANCEL", cancelButtonAction: onClick_Cancel_MultiPlayer);
            stopCoroutines();
        }

        switch (cause)
        {
            case DisconnectCause.None:
                break;
            case DisconnectCause.ExceptionOnConnect:
                break;
            case DisconnectCause.Exception:
                break;
            case DisconnectCause.ServerTimeout:
                break;
            case DisconnectCause.ClientTimeout:
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
    }
#endif
    private void sendMyDetailsToOpponent() // This will be called to both master and client

    {
#if SUPERCARDS_MULTIPLAYER
        if (KICK_PLAYER_OUT_IF_MINIMIZE == false)
        {
            PhotonNetwork.CurrentRoom.PlayerTtl = 10000;
        }

        photonView.RPC("SendPlayerDetails", RpcTarget.Others, "" + PhotonNetwork.LocalPlayer.ActorNumber);
        if(PhotonNetwork.IsMasterClient)
        {
            SuperCardsUI.instance.ShuffleAndParseDetails();
        }
#endif
    }

#if SUPERCARDS_MULTIPLAYER
    [PunRPC]
    public void SendPlayerDetails(string name)
    {
        SuperCardsUI.instance.updateOpponentName(name);
    }
#endif

    public void sendShuffledCardsList(ref byte[] Player1ShuffledCards, ref byte[] Player2ShuffledCards)
    {
#if SUPERCARDS_MULTIPLAYER
        photonView.RPC("onReceiveShuffledCards", RpcTarget.Others, Player1ShuffledCards, Player2ShuffledCards); //RpcTarget.Others -> Except me others 
        PhotonNetwork.SendAllOutgoingCommands();
#endif
    }

#if SUPERCARDS_MULTIPLAYER
    [PunRPC]
    public void onReceiveShuffledCards(byte[] p1c, byte[] p2c) // Called for only client
    {
        SuperCardsUI.instance.CardNumberUser = p2c;
        SuperCardsUI.instance.CardNumberBot = p1c;

        SuperCardsUI.instance.onReceiveShuffledCards_Multiplayer(); // Called for only client

        var props = new ExitGames.Client.Photon.Hashtable();
        props.Add(RoomProperty.START_GAME, 1);//start game
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }
#endif
    //Dictionary<string, NormalSuperCards> dummy = new Dictionary<string, NormalSuperCards>();
    //private void test()
    //{
    //    dummy.Add("round1", new NormalSuperCards());
    //    dummy.Add("round2", new NormalSuperCards());
    //    var data = dummy["round1"] 

    //}
#if SUPERCARDS_MULTIPLAYER
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged) // for both master and client
    {
        object tempValue;
        if (propertiesThatChanged.TryGetValue(RoomProperty.START_GAME, out tempValue) && tempValue != null)
        {
            isGameStarted = true;
            SuperCardsUI.instance.StartGame_MultiplayerAfterSharingShuffledCards();
        }
    }
#endif

    public void sendMySelectionToOpponent(byte type, int listIndex, int buttonIndex, SuperCard_PowerUpsList superCardSelected) //listIndex - Round number  // type  0-> All Rounder screen selection, 1 -> After chose the cards, 2-> Submit screen ., buttonIndex -> if type 1,2 -> 5 stats selection., if type 0 -> All Rounder bat , bowl card selection

    {
#if SUPERCARDS_MULTIPLAYER
        photonView.RPC("onReceiveSelectionFromOpponent", RpcTarget.Others, type, listIndex, buttonIndex, superCardSelected);
        PhotonNetwork.SendAllOutgoingCommands();
#endif
    }

#if SUPERCARDS_MULTIPLAYER
    [PunRPC]
    public void onReceiveSelectionFromOpponent(byte type, int listIndex, int buttonIndex, SuperCard_PowerUpsList superCardSelected)
    {
        //Debug.Log("onReceiveCardDetailsFromOpponent type == " + type + " listIndex" + listIndex + " buttonIndex == " + buttonIndex + " superCardSelected == " + superCardSelected);

        if (type == 0)
        {
            SuperCardsUI.instance.onClickAllRounderChoice(buttonIndex);
        }
        else if (type == 1)
        {
            SuperCardsUI.instance.NormalSuperCardsBot[listIndex].OnReceiveCardSelectionFromOpponent_Multiplayer(buttonIndex, type);
        }
    }
#endif
    public void sendMySubmissionToOpponent(byte playerType, int listIndex, int buttonIndex, SuperCard_PowerUpsList superCardSelected)
    {
#if SUPERCARDS_MULTIPLAYER
        photonView.RPC("onReceiveSubmissionFromOpponent", RpcTarget.Others, playerType, listIndex, buttonIndex, superCardSelected);
        PhotonNetwork.SendAllOutgoingCommands();
#endif
    }

#if SUPERCARDS_MULTIPLAYER
    [PunRPC]
    public void onReceiveSubmissionFromOpponent(byte playerType, int listIndex, int buttonIndex, SuperCard_PowerUpsList superCardSelected)
    {
        if(SuperCardsUI.instance.isCurrentTurnByUser || SuperCardsUI.instance.isValidatingScores)
        {
            return;
        }
        //Debug.Log("onReceiveCardDetailsFromOpponent type == " + type + " listIndex" + listIndex + " buttonIndex == " + buttonIndex + " superCardSelected == " + superCardSelected);
        if (SuperCardsUI.instance.NormalSuperCardsBot[listIndex].isAllRounder)
        {
            SuperCardsUI.instance.onClickAllRounderChoice(playerType);
        }

        SuperCardsUI.instance.applyBotSuperCard(superCardSelected);

        SuperCardsUI.instance.NormalSuperCardsBot[listIndex].OnReceiveCardSelectionFromOpponent_Multiplayer(buttonIndex, 2);

        sendReceivedAcknowledgement(2);
    }
#endif

    private void sendReceivedAcknowledgement(byte type)
    {
#if SUPERCARDS_MULTIPLAYER
        photonView.RPC("onReceiveAcknowledgement", RpcTarget.Others, type);
        PhotonNetwork.SendAllOutgoingCommands();
#endif
    }

#if SUPERCARDS_MULTIPLAYER
    [PunRPC]
    public void onReceiveAcknowledgement(byte type)
    {
        SuperCardsUI.instance.stopWaitACKCoroutine();
        if (type == 2)
        {
            SuperCardsUI.instance.checkForRoundCompletionOrTurnChange();
        }
        else if(type == 0)
        {
            SuperBallUI.instance.checkForNextTurn();
        }
    }
#endif

    public void resetValues()
    {
        isJoinedInRoom = false;
        isGameStarted = false;
        isReConnection = false;
        isOutOfFocus = false;
        opponentIsOutOfFocus = false;
    }

    public void sendMySelectionInSuperBallToOpponent(byte arrayIndex, byte Value)
    {
#if SUPERCARDS_MULTIPLAYER
        photonView.RPC("onReceiveSuperBallOpponentSelection", RpcTarget.Others, arrayIndex, Value);
#endif
    }

#if SUPERCARDS_MULTIPLAYER
    [PunRPC]
    public void onReceiveSuperBallOpponentSelection(byte arrayIndex, byte Value)
    {
        if (SuperCardsUI.instance.SuperBallUI.turnBYUser || SuperCardsUI.instance.isValidatingScores)
        {
            return;
        }

        SuperBallUI.instance.onReceiveOpponentSelection_Multiplayer(arrayIndex, Value);
        sendReceivedAcknowledgement(0);
    }
#endif

    private bool isOutOfFocus;
    [HideInInspector]
    public bool opponentIsOutOfFocus;
    private bool isGameStarted;
    private bool isReConnection;
    private string roomName;
    private bool isJoinedInRoom;
    private Coroutine opponentReconnectionDelayCoroutine;

    private const bool KICK_PLAYER_OUT_IF_MINIMIZE = false;

    private bool isPhotonActive()
    {
#if SUPERCARDS_MULTIPLAYER
        return (PhotonNetwork.IsConnected && PhotonNetwork.CurrentRoom != null);
#else
        return false;
#endif
    }

//#if !UNITY_EDITOR
//    private void OnApplicationFocus(bool focus)
//    {
//        onApplicationPauseAndFocus(focus);
//    }
//#endif

//    private void OnApplicationPause(bool pause)
//    {
//        onApplicationPauseAndFocus(!pause);
//    }

    private float timeAtGoingToBackGround;
    private void onApplicationPauseAndFocus(bool focus)
    {
        if (isOutOfFocus == focus)
        {
            isOutOfFocus = !focus;
            if (CONTROLLER.PLAYMULTIPLAYER)
            {
                if (focus == false)
                {
                    if (isGameStarted /*&& GameController.instance.isGameCompleted() == false*/)
                    {
                        if (KICK_PLAYER_OUT_IF_MINIMIZE)
                        {
                            disconnectPhoton();
                            stopCoroutines();
                        }
                        else
                        {
                            if (isPhotonActive())
                            {
                                sendMyFocusStatus(focus);
                            }
                        }
                    }

#if SUPERCARDS_MULTIPLAYER
                    if (PhotonNetwork.IsConnected)
                    {
                        PhotonNetwork.SendAllOutgoingCommands();
                    }
#endif
                }
                else
                {
                    //focus
                    StartCoroutine(addDelayForOnFocus(focus));
                }
            }
        }
    }

    private void sendMyFocusStatus(bool focus)
    {
#if SUPERCARDS_MULTIPLAYER
        this.photonView.RPC("onReceiveOpponentFocusStatus", RpcTarget.Others, focus);
#endif
    }

#if SUPERCARDS_MULTIPLAYER
    [PunRPC]
    public void onReceiveOpponentFocusStatus(bool focus, PhotonMessageInfo info)
    {
        opponentIsOutOfFocus = !focus;
        //Debug.Log("opponentIsOutOfFocus == " + opponentIsOutOfFocus);
        if (opponentIsOutOfFocus)
        {
            if (SuperCardsUI.instance.isOpponentsTurnToPlayThisRound)//opponents turn
            {
                reconnecting_Multiplayer(false, true);
            }
        }
        else
        {
            if(opponentReconnectionDelayCoroutine != null)
            {
                StopCoroutine(opponentReconnectionDelayCoroutine);
            }
            reconnecting_Multiplayer(true, true);
            SuperCardsUI.instance.checkAndStartNewRoundMultiplayer();
        }
    }
#endif
    private void stopCoroutines()
    {
        //if (getNetworkStatusCoroutine != null)
        //{
        //    StopCoroutine(getNetworkStatusCoroutine);
        //}
    }

    IEnumerator addDelayForOnFocus(bool focus)
    {
        yield return null;

        if (isGameStarted /*&& GameController.instance.isGameCompleted() == false*/)
        {
            if (KICK_PLAYER_OUT_IF_MINIMIZE)
            {
                //Popup.instance.Show(headerMsg: "", msg: "You were disconnected for going to background", okButtonMsg: "OK", okButtonAction: onClick_Cancel_MultiPlayer);
            }
            else
            {
                if (isPhotonActive())
                {
                    //after current running shot complete - will send focus status
                    sendMyFocusStatus(true);
                }
                else
                {
                    reConnectPhoton();
                    reconnecting_Multiplayer(false);
                }
            }
        }
    }

    public void reConnectPhoton()
    {
#if SUPERCARDS_MULTIPLAYER
        isReConnection = true;
        if (PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.ReconnectAndRejoin();
        }
        else
        {
            PhotonNetwork.RejoinRoom(roomName);
        }
#endif
    }

    public void reconnecting_Multiplayer(bool isDone, bool isReconnectingByOpponent = false)
    {
        if (isDone)
        {
            SuperCardsUI.instance.updateReconnectionLoadingScreen_Multiplayer("");
            //pauseGame(false);
        }
        else
        {
            if (isReconnectingByOpponent)
            {
                SuperCardsUI.instance.updateReconnectionLoadingScreen_Multiplayer("Opponent Reconnecting...");
            }
            else
            {
                SuperCardsUI.instance.updateReconnectionLoadingScreen_Multiplayer("Reconnecting...");
            }
            //pauseGame(true);
        }
    }

    IEnumerator waitForOpponentToReconnect(float delay)
    {
        yield return new WaitForSecondsRealtime(delay + 2f);
#if SUPERCARDS_MULTIPLAYER
        if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount <= 1)
        {
            SuperCardsUI.instance.ShowGameOverScreen(true, "OPPONENT DISCONNECTED");
            stopCoroutines();
        }
#endif
    }

    //private Coroutine getNetworkStatusCoroutine;

    //IEnumerator checkInternetConnectionInLoop()
    //{
    //    yield return new WaitForSecondsRealtime(0.5f);
    //    bool focus;
    //    if (NetworkManager.instance.isConnectedToInternet())
    //    {//connected to internet
    //        focus = true;
    //    }
    //    else
    //    {
    //        focus = false;
    //    }

    //    if (PanelManager.currentActivePanel == PanelTypes.Multiplayer_Connection || PanelManager.currentActivePanel == PanelTypes.InGame)
    //    {
    //        onApplicationPauseAndFocus(focus);

    //        getNetworkStatusCoroutine = StartCoroutine(checkInternetConnectionInLoop());
    //    }
    //}
}
