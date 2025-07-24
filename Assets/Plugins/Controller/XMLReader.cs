using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

public class XMLReader : CONTROLLER
{
	public static GameObject Loader;
	public static GameObject UnityPHPConnector;
	
	public static void ParseXML (string datas)
	{
		//Debug.LogError("############################### parse xml ########################### ");
		XMLParser parser = new XMLParser ();
		XMLNode node = parser.Parse (datas);
		int teamLength, i, j, PlayerCount;
		
		ArrayList cricketList = (ArrayList)node["cricket"];
		XMLNode cricketNode = (XMLNode)cricketList[0];
		
		ArrayList TeamDetails = (ArrayList)cricketNode["team"];
		teamLength = TeamDetails.Count;
		TeamList = new TeamInfo[teamLength];
		
		for(i = 0; i < teamLength; i++)
		{
			XMLNode teamNode = (XMLNode)TeamDetails[i];
			
			TeamList[i] = new TeamInfo();
			TeamList[i].teamName = teamNode["@name"] as string;
			
			TeamList[i].abbrevation = teamNode["@abbrevation"] as string;
			ArrayList PlayerDetailsList = (ArrayList)teamNode["PlayerDetails"];
			XMLNode PlayerDetailsNode = (XMLNode)PlayerDetailsList[0];
			
			ArrayList PlayerInfoList = (ArrayList)PlayerDetailsNode["player"];
			PlayerCount = PlayerInfoList.Count;
            //Debug.LogError(PlayerCount);
			TeamList[i].PlayerList = new PlayerInfo[PlayerCount];
			
			for(j = 0; j < PlayerCount; j++)
			{
				XMLNode PlayerNode = (XMLNode)PlayerInfoList[j];
				TeamList[i].PlayerList[j] = new PlayerInfo ();
				
				TeamList[i].PlayerList[j].PlayerName = PlayerNode["@name"] as string;
                //Debug.LogError(PlayerNode["@name"] as string);
				TeamList[i].PlayerList[j].ShortName = PlayerNode["@sname"] as string;//28march
				TeamList[i].PlayerList[j].JerseyNumber = PlayerNode["@num"] as string;
				TeamList[i].PlayerList[j].BattingHand = PlayerNode["@batHand"] as string;
				TeamList[i].PlayerList[j].Style = PlayerNode["@batStyle"] as string;
				TeamList[i].PlayerList[j].PlayerType = PlayerNode["@lp"] as string;
				TeamList[i].PlayerList[j].DefaultPlayer = PlayerNode["@dp"] as string;
			}
		}
		//if(Loader != null)
		//	Loader.SendMessage("XMLLoaded");
		//Debug.Log (TeamList[0].PlayerList[26].PlayerName+" :: "+TeamList[0].PlayerList.Length);
	}
	
}