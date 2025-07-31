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

public class Multiplayer
{
	public static List<MultiplayerOver> oversData = new List<MultiplayerOver>();
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
