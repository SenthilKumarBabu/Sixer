using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameScorecardPage : UIPage
{
    [SerializeField] private UserScorecardUI userScorecard, opponentScorecard;
    [SerializeField] private TMP_Text oversText;
}

[System.Serializable]
public class UserScorecardUI
{
    public TMP_Text scoreText, userNameText;
    public List<TMP_Text> runsEachBallList;
    public Image userImage;
}
