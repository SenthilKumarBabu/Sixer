using TMPro;
using UnityEngine;
using static LoginPage;

public class MatchMakingPage : UIPage
{
    public TMP_Text PlayerA_Name, PlayerB_Name;


    public override void OnShow(object data = null)
    {
        base.OnShow(data);
        if (data is MatchMakingData pageData)
        {
            PlayerA_Name.text = pageData.PlayerName;
        }
        else
        {
            Debug.LogWarning("LoginPage: Missing or incorrect data!");
        }
    }
}

public class MatchMakingData
{
    public string PlayerName,PlayerLevel,PlayerBettingAmount;
    public string OpponentName,OpponentLevel,OpponentBettingAmount;
    public Sprite PlayerProfileImage;
    public Sprite OpponentProfileImage;
    public string WinningAmount;

    public MatchMakingData(string playerName, string playerLevel, string playerBettingAmount, string opponentName=null, string opponentLevel=null, string opponentBettingAmount = null, Sprite playerProfileImage=null, Sprite opponentProfileImage = null, string winningAmount = null)
    {
        PlayerName = playerName;
        PlayerLevel = playerLevel;
        PlayerBettingAmount = playerBettingAmount;
        OpponentName = opponentName;
        OpponentLevel = opponentLevel;
        OpponentBettingAmount = opponentBettingAmount;
        PlayerProfileImage = playerProfileImage;
        OpponentProfileImage = opponentProfileImage;
        WinningAmount = winningAmount;
    }
}
