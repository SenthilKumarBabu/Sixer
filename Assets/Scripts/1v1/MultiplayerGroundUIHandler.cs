using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerGroundUIHandler : MonoBehaviour
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
    public Text Overs;
    public Text UserName;
    public Text UserScore;
    public Text[] UserBallInfo;


    public Text OppName;
    public Text OppScore;
    public Text[] OppBallInfo;


    public void resetScoreBoardData()
    {
        ScoreBoards.SetActive(true);

        Overs.text = "0(1)";
        UserName.text = CONTROLLER.UserName;
        UserScore.text = "0/0";
        OppName.text = CONTROLLER.MP_OpponentName;
        OppScore.text = "0/0";

        // Reset user ball data
        CONTROLLER.userBallbyBallData = new string[6];
        for (int i = 0; i < UserBallInfo.Length; i++)
        {
            UserBallInfo[i].text = "";
        }

        // Reset opponent ball data
        CONTROLLER.oppBallbyBallData = new string[6];
        for (int i = 0; i < OppBallInfo.Length; i++)
        {
            OppBallInfo[i].text = "";
        }
    }


    public void UpdateScoreCard()
    {
        UserScore.text = GameModel.ScoreStr;
        Overs.text = GameModel.OversStr;

        for (int i = 0; i < 6; i++)
        {
            UserBallInfo[i].text = CONTROLLER.userBallbyBallData[i];
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
        GameOverPanel.SetActive(true);

        // Parse user score
        ParseScore(UserScore.text, out int userRuns, out int userWickets);

        // Parse opponent score
        ParseScore(OppScore.text, out int oppRuns, out int oppWickets);

        // Determine result
        if (userRuns > oppRuns)
        {
            GO_Title.text = "YOU WON";
            GO_Desc.text = $"You scored {userRuns}/{userWickets} and defended it against {oppRuns}/{oppWickets}.\n Great win!";
        }
        else if (userRuns < oppRuns)
        {
            GO_Title.text = "YOU LOST";
            GO_Desc.text = $"Opponent chased your {userRuns}/{userWickets} with a score of {oppRuns}/{oppWickets}.\n Try again!";
        }
        else
        {
            GO_Title.text = "MATCH TIED";
            GO_Desc.text = $"Both scored {userRuns}/{userWickets}.\n It’s a draw!";
        }
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
