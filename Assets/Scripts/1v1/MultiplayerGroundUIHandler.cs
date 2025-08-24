using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerGroundUIHandler : UIPage
{
    public ScorePopupAnimation scorePopupAnimationScript;
    public GameObject WaitingPanel;
    public void Awake()
    {
        WaitingPanel.SetActive(false);
        ScoreBoards.SetActive(false);
        GameOverPanel.SetActive(false);
    }

    #region WaitingPanel
    public void skipMatchIntro()
    {
        int _mss = MultiplayerManager.Instance.GetPhotonHashInt(RoomVariables.masterSyncStatus, -1);
        int _css = MultiplayerManager.Instance.GetPhotonHashInt(RoomVariables.clientSyncStatus, -1);

        if (_mss == 1 && _css == 1)
        {
            Invoke("ContinueToStartGame", 1);
        }
        else
        {
            WaitingPanel.SetActive(true);
        }

        if (MultiplayerManager.Instance.botsSpawned)
        {
            MultiplayerManager.Instance.SpawnBot();
            Invoke("ContinueToStartGame", UnityEngine.Random.Range(1f, 2.5f));
        }
    }

    void ContinueToStartGame()
    {
        CancelInvoke("ContinueToStartGame");
        WaitingPanel.SetActive(false);
        MultiplayerManager.Instance.CancelUpdateStatusRecheck();
        GameModel.instance.ShowIntroAnimation();
    }
    #endregion

    #region SCOREBOARD

    public GameObject ScoreBoards;
    public UserScorecardUI userScorecard, opponentScorecard;
    [SerializeField] private TMP_Text oversText;

    public void resetScoreBoardData()
    {
        ScoreBoards.SetActive(true);

        oversText.text = "0(1)";
        userScorecard.userNameText.text = CONTROLLER.UserName;
        userScorecard.scoreText.text = "0/0";
        opponentScorecard.userNameText.text = CONTROLLER.MP_OpponentName;
        opponentScorecard.scoreText.text = "0/0";

        // Reset user ball data
        CONTROLLER.userBallbyBallData = new string[6];
        for (int i = 0; i < userScorecard.runsEachBallList.Count; i++)
        {
            userScorecard.runsEachBallList[i].text = "";
            userScorecard.BGEachBallList[i].SetActive(false);
        }

        // Reset opponent ball data
        CONTROLLER.oppBallbyBallData = new string[6];
        for (int i = 0; i < opponentScorecard.runsEachBallList.Count; i++)
        {
            opponentScorecard.runsEachBallList[i].text = "";
            opponentScorecard.BGEachBallList[i].SetActive(false);
        }
    }


    public void UpdateScoreCard()
    {
        userScorecard.scoreText.text = GameModel.ScoreStr;
        oversText.text = GameModel.OversStr;

        for (int i = 0; i < 6; i++)
        {
            userScorecard.runsEachBallList[i].text = CONTROLLER.userBallbyBallData[i];
            if (i <= MultiplayerManager.Instance.userBallIndex)
                userScorecard.BGEachBallList[i].SetActive(true);
        }
        scorePopupAnimationScript.ShowScore(CONTROLLER.userBallbyBallData[MultiplayerManager.Instance.userBallIndex], true);
    }
    #endregion

    #region GAMEOVER

    public GameObject GameOverPanel;
    public Text GO_Title;
    public Text GO_Desc;
    public void OnButtonClickofGameOverScreen(int idx)
    {
        if(idx == 1) //Replay
        {
            GameModel.instance.ReStartGame();
            PhotonNetwork.LoadLevel(2);
        }
        else //Home
        {
            if (MultiplayerManager.Instance != null && PhotonNetwork.NetworkClientState != ClientState.PeerCreated)
            {
                MultiplayerManager.Instance.DisConnectFromPhoton();
            }
            StartCoroutine(GameModel.instance.GameQuitted());
        }
    }

    public void UpdateGameOverScreen()
    {
        //GameOverPanel.SetActive(true);
        

        // Parse user score
        ParseScore(userScorecard.scoreText.text, out int userRuns, out int userWickets);

        // Parse opponent score
        ParseScore(opponentScorecard.scoreText.text, out int oppRuns, out int oppWickets);

        string headertext, desc;
        // Determine result
        if (userRuns > oppRuns)
        {
            headertext = "YOU WON";
            desc = $"You scored {userRuns}/{userWickets} and defended it against {oppRuns}/{oppWickets}.\n Great win!";
        }
        else if (userRuns < oppRuns)
        {
            headertext = "YOU LOST";
            desc = $"Opponent chased your {userRuns}/{userWickets} with a score of {oppRuns}/{oppWickets}.\n Try again!";
        }
        else
        {
            headertext = "MATCH TIED";
            desc = $"Both scored {userRuns}/{userWickets}.\n It’s a draw!";
        }
        UIManager.Instance.OpenPopup<GenericPopup>(new GenericPopupData( headertext, desc,()=>OnButtonClickofGameOverScreen(1),()=>OnButtonClickofGameOverScreen(0) ));

    }

    // Utility method to parse score string in format "runs/wickets"
    private void ParseScore(string scoreText, out int runs, out int wickets)
    {
        runs = 0;
        wickets = 0;

        string[] parts = scoreText.Split('/');
        if (parts.Length == 2)
        {
            int.TryParse(parts[0], out runs);
            int.TryParse(parts[1], out wickets);
        }
    }

    #endregion
}

[System.Serializable]
public class UserScorecardUI
{
    public TMP_Text scoreText, userNameText;
    public List<TMP_Text> runsEachBallList;
    public List<GameObject> BGEachBallList;
    public Image userImage;
}
