#define IsDebug //UNCOMMENT FOR TESTING WITH DUMMY SCENE
using UnityEngine;
using System.Collections;
using System;

public class EmoticonAnimator : MonoBehaviour
{
	public static EmoticonAnimator Instance { get { return _Instance; } }
	public EmoticonHelper[] playerEmoticon;
	public Emoticons emoticonUI;

	private static EmoticonAnimator _Instance = null;
	private string[] currentAnimation = new string[5];
	private int currentPlayer = -1;
	private int duration = -1;

	public  Texture alphaImage;
	public Texture [] A,B,C,D,E;


	void Awake ()
	{
		if (_Instance == null)
		{
			_Instance = this;
		}
	}

	public void A_ClickEvent()
	{
		ButtonEvent ("A");
	}
	public void B_ClickEvent()
	{
		ButtonEvent ("B");
	}
	public void C_ClickEvent()
	{
		ButtonEvent ("C");
	}
	public void D_ClickEvent()
	{
		ButtonEvent ("D");
	}
	public void E_ClickEvent()
	{
		ButtonEvent ("E");
	}



	public void ButtonEvent (string buttonName)
	{
		emoticonUI.CloseEmoticons ();
//		#if IsDebug
//		currentPlayer = Array.FindIndex (Multiplayer.playerList, t => t.PlayerId == UserProfile.PlayerId);
//#endif

#if IsDebug
        if (CONTROLLER.BattingMultiplayerCountryCode == string.Empty)
            currentPlayer = Array.FindIndex(Multiplayer.playerList, t => t.PlayerId == (CONTROLLER.M_USERID));
        else
            currentPlayer = Array.FindIndex(Multiplayer.playerList, t => t.playerIdwithCountryCode == CONTROLLER.BattingMultiplayerCountryCode + CONTROLLER.M_USERID);
#endif

        switch (buttonName)
		{
			case "A":
			duration = 10;
			break;

			case "B":
			duration = 10;
			break;
	
			case "C":
			duration = 10;
			break;
			
			case "D":
			duration = 10;
			break;
		
			case "E":
			duration = 25;
			break;
		}

		currentAnimation [currentPlayer] = buttonName;
		AnimateEmoticon (CONTROLLER.M_USERID,currentAnimation [currentPlayer],duration, (CONTROLLER.BattingMultiplayerCountryCode + CONTROLLER.M_USERID));
		ServerManager.Instance.SendEmoticon (buttonName);
	}

	public void AnimateEmoticon (int player, string emoticon, int duration, string playeridwithcountrycode)
	{
		switch (emoticon)
		{
			case "A":
			duration = 10;
			break;
			
			case "B":
			duration = 10;
			break;
			
			case "C":
			duration = 10;
			break;
			
			case "D":
			duration = 10;
			break;
			
			case "E":
			duration = 25;
			break;
		}
        if (CONTROLLER.BattingMultiplayerCountryCode == string.Empty)
            player = Array.FindIndex(Multiplayer.playerList, t => t.PlayerId == player);//&& t.playerIdwithCountryCode==playeridwithcountrycode );
        else
            player = Array.FindIndex(Multiplayer.playerList, t => t.PlayerId == player && t.playerIdwithCountryCode == playeridwithcountrycode);

        //player = Array.FindIndex (Multiplayer.playerList, t => t.PlayerId == player);
		playerEmoticon [player].PlayAnimation (emoticon, duration);
	}

	public Texture getTexture(string name,int index)
	{
		Texture temp=alphaImage ;
		switch (name)
		{
		case "A":
			temp = A [index];
			break;

		case "B":
			temp = B[index];
			break;
		case "C":
			temp = C[index];
			break;
		case "D":
			temp = D[index];
			break;
		case "E":
			temp = E[index];
			break;
		}
		return temp;
	}
}
