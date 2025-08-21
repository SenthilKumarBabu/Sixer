using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BotController : MonoBehaviour
{
    [HideInInspector]public bool isBot = false;
    public void InitAsBot()
    {
        isBot = true;
        totalMatchScore = 0;
        currentMatchWickets = 0;
    }

    enum DifficultyLevel
    {
        Easy,   //bot will score more runs
        Medium,  
        Hard    // runs will be lesser
    }

    DifficultyLevel botDifficulty = DifficultyLevel.Hard;

    private int totalMatchScore;
    private int currentMatchWickets;
    public void SendScoreForBattingMultiplayer()
    {
        string currentRunScored = GetBotScoringOutcome(botDifficulty);

        if (currentRunScored == "W")
        {
            currentMatchWickets++;
        }
        else
        {
            int runValue = int.Parse(currentRunScored);
            totalMatchScore += runValue;
        }

        if (MultiplayerManager.Instance.userBallIndex == 5 && totalMatchScore == CONTROLLER.currentMatchScores)
        {
            if (totalMatchScore > 0)
            {
                totalMatchScore -= 1;
                currentRunScored = (currentRunScored != "W") ? Math.Max(0, int.Parse(currentRunScored) - 1).ToString() : "0";
            }
            else
            {
                totalMatchScore += 1;
                currentRunScored = "1";
            }
        }


        MultiplayerManager.Instance.ReceiveScoreUpdate(totalMatchScore, currentRunScored, currentMatchWickets);
        GameModel.instance.ScoreSyncedAndMoveToNextBall();
    }

    /// <summary>
    /// Returns a weighted scoring outcome depending on difficulty.
    /// </summary>
    private string GetBotScoringOutcome(DifficultyLevel difficulty)
    {
        // Define probability distributions for outcomes (sum should equal 100)
        Dictionary<string, int> weights = new Dictionary<string, int>();

        switch (difficulty)
        {
            case DifficultyLevel.Easy:
                // More chances for 4s and 6s
                weights = new Dictionary<string, int>
            {
                {"0", 5}, {"1", 15}, {"2", 10}, {"3", 5}, {"4", 25}, {"6", 30}, {"W", 10}
            };
                break;

            case DifficultyLevel.Medium:
                // Balanced distribution
                weights = new Dictionary<string, int>
            {
                {"0", 10}, {"1", 20}, {"2", 15}, {"3", 10}, {"4", 20}, {"6", 15}, {"W", 10}
            };
                break;

            case DifficultyLevel.Hard:
                // More dot balls & wickets, fewer boundaries
                weights = new Dictionary<string, int>
            {
                {"0", 20}, {"1", 25}, {"2", 15}, {"3", 5}, {"4", 15}, {"6", 5}, {"W", 15}
            };
                break;
        }

        // Weighted random selection
        int totalWeight = 0;
        foreach (var kvp in weights)
            totalWeight += kvp.Value;

        int randomValue = UnityEngine.Random.Range(0, totalWeight);
        int cumulative = 0;

        foreach (var kvp in weights)
        {
            cumulative += kvp.Value;
            if (randomValue < cumulative)
                return kvp.Key;
        }

        return "0"; // fallback
    }

}
