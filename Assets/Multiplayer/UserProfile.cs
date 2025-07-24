//#define UPDEBUG
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using System;

public class SettingsPrefs
{
	public static bool sfxState = false;
	public static bool musicState = false;
	public static bool selectedGender = false; // false=male or true=female
}

public class UserProfile
{
	public static int Tickets;
	public static int SpentTickets;
	public static int EarnedTickets;
	public static int spentTotTickets;
	public static bool isInMultiplayer = false;
}

public class LeaderboardScore
{
	public int PlayerId;
	public int Rank;
	public string Username;
	public int Score;

	public LeaderboardScore (int _PlayerId, int _Rank, string _Username, int _Score)
	{
		PlayerId = _PlayerId;
		Rank = _Rank;
		Username = _Username;
		Score = _Score;
	}
}

public class MultiplayerReplayData
{
    public int roomType = 0;
    public int overs = 0; // 2 or 5
    public int isHost = 0;
}

public class Multiplayer
{
	public static int roomType = 0;
	//public static int oversCount = 0;		//0 = 2 overs, 1 = 5 overs
	public static int overs = 0; // 2 or 5
	public static int playerCount = 0;
	public static int isHost = 0;
	public static string roomID =  string.Empty;
	public static PlayerList[] playerList = new PlayerList[5];
	public static List<MultiplayerOver> oversData = new List<MultiplayerOver>();
	public static MultiplayerScore[] playerScores;

	public static int entryTickets = 1;
	public static int[] winningCoins2 = new int[5] {500,400,300,200,100};
	public static int[] winningCoins5 = new int[5] {1000,800,600,400,200};

	public static int GetWinningCostIndex (int i)
	{
		int index = 0;
		if (Multiplayer.playerCount == 2)
		{
			if (i == 0)
			{
				index = 0;
			} else if (i == 1)
			{
				index = 4;
			}
		} 
		else if (Multiplayer.playerCount == 3)
		{
			if (i == 0)
			{
				index = 0;
			}
			else if (i == 1)
			{
				index = 3;
			}
			else if (i == 2)
			{
				index = 4;
			}
		}
		else if (Multiplayer.playerCount == 4)
		{
			if (i == 0)
			{
				index = 0;
			} else if (i == 1)
			{
				index = 2;
			} else if (i == 2)
			{
				index = 3;
			} else if (i == 3)
			{
				index = 4;
			}
		}
		else if (Multiplayer.playerCount == 5)
		{
			index = i;
		}

		return index;
	}

    public static int GetWinningCostIndex(int playerCount, int rank)
    {
        int index = 0;
        if (playerCount == 2)
        {
            if (rank == 0)
            {
                index = 0;
            }
            else if (rank == 1)
            {
                index = 4;
            }
        }
        else if (playerCount == 3)
        {
            if (rank == 0)
            {
                index = 0;
            }
            else if (rank == 1)
            {
                index = 3;
            }
            else if (rank == 2)
            {
                index = 4;
            }
        }
        else if (playerCount == 4)
        {
            if (rank == 0)
            {
                index = 0;
            }
            else if (rank == 1)
            {
                index = 2;
            }
            else if (rank == 2)
            {
                index = 3;
            }
            else if (rank == 3)
            {
                index = 4;
            }
        }
        else if (playerCount == 5)
        {
            index = rank;
        }

        return index;
    }
}

public class MultiplayerOver
{
	public string bowlerType; // fast or spin
	public string bowlerSide; //around or over
	public string bowlerHand; // left or right
	public int[] bowlingAngle = new int[6];
	public int[] bowlingSpeed = new int[6];
	public Vector3[] bowlingSpotL = new Vector3[6];
	public Vector3[] bowlingSpotR = new Vector3[6];
}

public class PlayerList
{
	public int PlayerId;
	public string PlayerName;
    public string Cp;
    public string playerIdwithCountryCode;

}

public class MultiplayerScore
{
	public int Rank;
	public int PlayerId;
	public string Username;
	public string Score;
	public int Wickets;
	public int LastBallScore;
	public int RankPos;
    public string Cp;
    public string playerIdwithCountryCode;


}
