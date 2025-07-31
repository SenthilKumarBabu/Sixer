using Photon.Pun;
using UnityEngine;

public partial class MultiplayerManager
{
    [PunRPC]
    public void SendMyDetailsToOpponentLobby(string UserName, string UserID)
    {
        DebugLogger.PrintWithColor("SendMyDetailsToOpponentLobby::: UserName: " + UserName + "::: :UserID::: " + UserID);
        GameModeSelector._instance.GetOpponentDetails(UserName, UserID);
    }


    // #region RPC'S
    // public void ReduceAudioCall(bool _val)
    // {
    //     //photonView.RPC("ReduceAudioRPC", RpcTarget.Others, _val);
    //     SendRPC("ReduceAudioRPC", RpcTarget.Others, _val); //kkk
    // }

    // [PunRPC]
    // public void ReduceAudioRPC(bool _val)
    // {
    //     if(VoiceController.instance != null)
    //         VoiceController.instance.AudioReduce(_val);
    // }
    // public void SetMatchResult(int myteamscr, int oppoteamscr, int mywckts, int oppowckts)
    // {
    //     //photonView.RPC("UpdateMatchResult", RpcTarget.Others, myteamscr, oppoteamscr,mywckts,oppowckts);
    //     SendRPC("UpdateMatchResult", RpcTarget.Others, myteamscr, oppoteamscr, mywckts, oppowckts); //kkk
    // }
    // [PunRPC]
    // public void UpdateMatchResult(int myscr, int oppscr, int mywckts, int oppowckts)
    // {
    //     if (IsSpectator())
    //     {
    //         matchInfo.myscr = myscr;
    //         matchInfo.oppscr = oppscr;
    //         matchInfo.mywckts = mywckts;
    //         matchInfo.oppowckts = oppowckts;
    //     }

    // }
    // public void SwitchToSquadPage()
    // {
    //     //photonView.RPC("SendCommandToSwitchSquadPage", RpcTarget.All);
    //     SendRPC("SendCommandToSwitchSquadPage", RpcTarget.All); //kkk
    // }
    // [PunRPC]
    // public void SendCommandToSwitchSquadPage()
    // {
    //     if (IsMasterClient())
    //     {
    //         SetPitchAndWeather();
    //     }
    //   // Debug.LogError("SendCommandToSwitchSquadPage ");
    //    PremiumLobby.Instance.ProceedToSquadPage();
    // }
    // public void SetEsportsServerMatchID()
    // {
    //     //photonView.RPC("SetMatchIDForPremiumLobby", RpcTarget.Others, EsportsController.EsportsServerSetMatchID);
    //     //photonView.RPC("CanSendBallDataToStreaming", RpcTarget.Others, EsportsController.CanSendBallData);

    //     SendRPC("SetMatchIDForPremiumLobby", RpcTarget.Others, EsportsController.EsportsServerSetMatchID); //kkk
    //     SendRPC("CanSendBallDataToStreaming", RpcTarget.Others, EsportsController.CanSendBallData); //kkk
    // }

    // [PunRPC]//rj inactive
    // public void SetMatchIDForPremiumLobby(string ID)
    // {
    //     PremiumLobby.Instance.GetServerSetMatchID(ID);
    // }

    // [PunRPC]
    // public void CanSendBallDataToStreaming(int canSendBallData)
    // {
    //     EsportsController.CanSendBallData = canSendBallData;
    // }

    // public void SendRankedMatchID(string MatcHID)
    // {
    //     //photonView.RPC("rankedmatchID", RpcTarget.All, MatcHID);
    //     SendRPC("rankedmatchID", RpcTarget.All, MatcHID); //kkk
    // }

    // [PunRPC]
    // public void rankedmatchID(string ID)
    // {
    //     CONTROLLER.MP_MatchID = ID;
    // }

    // public void ClearReceivedBattingInputs()
    // {
    //     if (IsEsporstsMode())
    //         GameModelScript.GroundControllerScript.clearReceivedBattingInputs();
    //     //Debug.LogError("Clear");
    //     //SendRPC("ClearRBInputs", Photon.Pun.RpcTarget.Others,IsEsporstsMode()?rivalsPlayer.Id:0);
    //     SendRPC("ClearRBInputs", Photon.Pun.RpcTarget.Others, IsEsporstsMode() ? rivalsPlayer.Id : 0); //kkk
    // }

    // [PunRPC]
    // public void ClearRBInputs(int id)
    // {
    //     if (IsEsporstsMode())
    //     {
    //         if (IsBattingTeamNotSpec())
    //         {
    //             roomInfo.AddBattingInputCounter(id);
    //         }
    //     }
    //     else
    //     {
    //         GameModelScript.GroundControllerScript.clearReceivedBattingInputs();
    //     }
    // }

    // public void ClearReceivedBallTimingData()
    // {
    //     if(IsEsporstsMode())
    //         GameModelScript.GroundControllerScript.clearReceivedBallTimingData();
    //     //Debug.LogError("ClearReceivedBallTimingData");
    //     SendRPC("ClearRBTimingData", Photon.Pun.RpcTarget.Others,IsEsporstsMode()?rivalsPlayer.Id:0);
    // }

    // [PunRPC]
    // public void ClearRBTimingData(int id)
    // {
    //     if (IsEsporstsMode())
    //     {
    //         if (IsBattingTeamNotSpec())
    //         {
    //             roomInfo.AddBalltimigCounter(id);
    //         }
    //     }
    //     else {
    //         GameModelScript.GroundControllerScript.clearReceivedBallTimingData();
    //     }


    // }

    // //[PunRPC]
    // //public void Receiver(bool isPaused)
    // //{
    // //    Debug.Log("Receiver called");
    // //    if (!CONTROLLER.isLocalMultiplayer)
    // //    {
    // //        if (isPaused == true)
    // //        {
    // //            MP_InterruptsHandler.instance.DisplayInterruption(-1);
    // //        }
    // //        if (isPaused == false)
    // //        {
    // //            MP_InterruptsHandler.instance.HideTab();
    // //        }
    // //    }
    // //}


    // [PunRPC]
    // public void KickScreen(int index)
    // {
    //     kickReason = index;
    //     //MP_InterruptsHandler.instance.DisplayInterruption(2);
    //     MP_ReconnectionHandler.instance.DisplayInterruption(2);
    // }

    // public void SendOpponentExtrasWin()
    // {
    //     SendRPC("ExtrasWin", Photon.Pun.RpcTarget.Others);
    // }

    // [PunRPC]
    // public void ExtrasWin()
    // {
    //     GameModelScript.GameOverScreenScript.ShowResultScreenDueToDisconnect("win", 1);
    // }

    // public void SetUCBEmptyToBowlingTeam()
    // {
    //     if (IsEsporstsMode())
    //     {
    //         roomInfo.UpdateCurrentBallEventState = MultiplayerRoomInfo.UpdateCurrentBallState.Updated;
    //         photonView.RPC("SetUCBToBowlerSpec", RpcTarget.Others, rivalsPlayer.Id);
    //     }
    //     else if (Is2v2Mode())
    //     {
    //         roomInfo.UpdateCurrentBallEventState = MultiplayerRoomInfo.UpdateCurrentBallState.Updated;
    //         photonView.RPC("SetUCBToBowler2v2", RpcTarget.Others, (int)rivalsPlayer.userType);
    //     }
    //     else
    //     {
    //         //photonView.RPC("SetUCBToBowler", RpcTarget.Others);
    //         SendRPC("SetUCBToBowler", RpcTarget.Others);
    //     }
    // }

    //[PunRPC]
    // public void SetUCBToBowlerSpec(int id)
    // {
    //     if (IsBattingTeamNotSpec())
    //     {
    //         roomInfo.playerUpdatedIndex.Add(id);
    //         //Debug.LogError("roomInfo.playerUpdatedIndex.Count=" + roomInfo.playerUpdatedIndex.Count + "currentNoOfPlayers=" + currentNoOfPlayers);
    //         if (roomInfo.playerUpdatedIndex.Count == currentNoOfPlayers && roomInfo.UpdateCurrentBallEventState == MultiplayerRoomInfo.UpdateCurrentBallState.YetToUpdated)
    //         {
    //             GameModelScript.SetUCBEmpty();
    //             roomInfo.UpdateCurrentBallEventState = MultiplayerRoomInfo.UpdateCurrentBallState.Updated;
    //         }
    //     }
    // }
    // [PunRPC]
    // public void SetUCBToBowler()
    // {
    //     GameModelScript.SetUCBEmpty();  
    // }

    // [PunRPC]
    // public void SendMyDetailsToOpponentCustomLobby(string UserName, string UserID, string cp, string picURL, int jer_ID, int favID, int skillRate,bool isAllStar)
    // {
    //     //Debug.Log("SendMyDetailsToOpponentCustomLobby==");
    //     MultiplayerMenus.instance.GetOpponentDetails(UserName, UserID, cp, picURL, jer_ID , favID, skillRate, isAllStar);
    // }

    // [PunRPC]
    // public void SendMyDetailsToOpponentCustomLobby(string UserName, string UserID, string cp, string picURL, int jer_ID, int favID, int skillRate)
    // {
    //     //Debug.Log("SendMyDetailsToOpponentCustomLobby==");

    //     MultiplayerMenus.instance.GetOpponentDetails(UserName, UserID, cp, picURL, jer_ID, favID, skillRate);
    // }

    // [PunRPC]
    // public void ShowSquadPage()
    // {
    //     MultiplayerMenus.instance.ShowSquadPage();
    // }

    // [PunRPC]
    // public void ShowTossResult()
    // {
    //     GameModelScript.ShowTossResultToEveryOne();
    // }

    // [PunRPC]
    // public void SelectBatsmanIndex(int _batsmanIndex)
    // {
    //     ////%%^^("SelectBatsmanIndex : RPC :: " + _batsmanIndex);
    //     //GameModelScript.BatsmanSelectionScript.SelectBatsmanIndexToOthers(_batsmanIndex);
    // }

    // [PunRPC]
    // public void SkipBowlerSelection(int _currentBowler)
    // {
    //     GameModelScript.BowlerSelectionScript.ContinueFromBowlerSelectionScreen(_currentBowler);
    // }

    // [PunRPC]
    // public void SelectBowlerIndex(int _bowlerIndex)
    // {
    //     //GameModelScript.BowlerSelectionScript.SetBowlerIndexToOthers(_bowlerIndex);
    //     ////%%^^("SelectBowlerIndex : RPC :: " + _bowlerIndex);

    // }

    // [PunRPC]
    // public void SelectCurrBowlerIndex(int _bowlerIndex)
    // {
    //     //GameModelScript.BowlerSelectionScript.SetCurrBowlerIndexToOthers(_bowlerIndex);
    //     ////%%^^("SelectBowlerIndex : RPC :: " + _bowlerIndex);

    // }

    // [PunRPC]
    // public void SetBowlerSide(string bowlerSide)
    // {
    //     GameModelScript.GroundControllerScript.ISetBowlerSide(bowlerSide);
    //     //%%^^("SetBowlerSide : RPC :: " + bowlerSide);

    // }

    // //manual field
    // public void ShowPresetFieldForBattingTeam()
    // {
    //     SendRPC("ShowPresetFieldToBattingTeam", Photon.Pun.RpcTarget.Others);


    // }

    // [PunRPC]
    // public void ShowPresetFieldToBattingTeam()
    // {
    //     /*if (MP_ManualField.instance != null)
    //     {
    //         MP_ManualField.instance.ShowPresetFieldForBattingTeam();
    //     }*/

    //     PanelManager.GetRegisteredPanel<MP_ManualFieldPanel>(PanelType.ManualField).ShowPresetField();
    // }

    // public void ShowManualFieldForBattingTeam()
    // {
    //     SendRPC("ShowManualFieldToBattingTeam", Photon.Pun.RpcTarget.Others);
    // }

    // [PunRPC]
    // public void ShowManualFieldToBattingTeam()
    // {
    //     /*if (MP_ManualField.instance != null)
    //     {
    //         MP_ManualField.instance.ShowManualFieldForBattingTeam();
    //     }*/
    //     PanelManager.GetRegisteredPanel<MP_ManualFieldPanel>(PanelType.ManualField).ShowManualField();

    // }
    // //[PunRPC]
    // public void ShowManualField()
    // {
    //     SendRPC("ShowManualFieldToOpponent", Photon.Pun.RpcTarget.All);
    // }

    // [PunRPC]
    // public void ShowManualFieldToOpponent()
    // {
    //     GameModelScript.PreviewScreenScript.ManualFieldPlacementForBattingTeam();
    // }

    // public void HideManualField()
    // {
    //     SendRPC("HideManualFieldForOpponent", Photon.Pun.RpcTarget.All);
    // }

    // [PunRPC]
    // public void HideManualFieldForOpponent()
    // {
    //     /* if (MP_ManualField.instance != null)
    //      {
    //          //MP_ManualField.instance.saveAndCloseManualField("save");
    //          MP_ManualField.instance.timerRanOutForFieldPlacement();
    //      }*/
    //     if (PanelManager.LastShownPanelType() == PanelType.ManualField)
    //         PanelManager.GetRegisteredPanel<MP_ManualFieldPanel>(PanelType.ManualField).timerRanOutForFieldPlacement();
    // }

    // [PunRPC]
    // public void SetOversForCustomMatch(int Overs)
    // {
    //     //%%^^Error("SETTING AUTO SELECT");
    //     MultiplayerMenus.instance.SetOverForClient(Overs);

    // }

    // [PunRPC]
    // public void SetOversForCustomMatchLocal(string data)
    // {
    //     //%%^^Error("SETTING AUTO SELECT");
    //     MultiplayerMenus.instance.SetOverForClientLocal(data);

    // }

    // [PunRPC]
    // public void KickOutOfRoom()
    // {
    //     MultiplayerMenus.instance.GoBackAfterEnteringRoom();
    //     if (CONTROLLER.isLocalMultiplayer)
    //     {
    //         MultiplayerMenus.instance.SelectTeamScreen_Client.SetActive(false);
    //         PopUp.Show(PopUpTypes.YES, hasCloseButton: false, heading: "ROOM EMPTY", message: "Host has left the room.", yesString: "OK", yesCallBack: MultiplayerMenus.instance.GoBackToLandingPage);
    //     }
    //     else
    //     {
    //         PopUp.Show(PopUpTypes.YES, hasCloseButton: false, heading: "ROOM EMPTY", message: "Host has left the room.", yesString: "OK");
    //     }
    // }


    // public void MoveToSquadPage()
    // {
    //     SendRPC("ShowSquadPage", Photon.Pun.RpcTarget.All);
    //     if (IsMasterClient())
    //     {
    //         SetPitchAndWeather();
    //     }
    // }
    // public void SendBowlerSide(string side)
    // {
    //     SendRPC("SetBowlerSide", Photon.Pun.RpcTarget.Others, side);


    //     //SendRPC("SetBow lerSide", Photon.Pun.RpcTarget.All, side);

    // }

    // public void SendBatsmanCanMoveLeftRight()
    // {
    //     SendRPC("SetBatsmanCanMoveLeftRight", Photon.Pun.RpcTarget.All);


    // }

    // [PunRPC]
    // public void SetBatsmanCanMoveLeftRight()
    // {
    //     GameModelScript.GroundControllerScript.setBatsmanCanMoveLeftRight();
    // }

    // #endregion
}
