using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeInstructionLogoAssigner : MonoBehaviour
{
	public Sprite[] modeLogo;   //SO,SLOG,CHASE,MULTI
	public Image logo;
	//public Text content;
	//public ScrollRect instructionRect;
	public GameObject HelpPanelHolder;

	public GameObject[] ContentHolders;

	private void setHolders(int idx)
	{
		for(int i=0;i<4;i++)
		{
			ContentHolders[i].SetActive(false);
		}
		ContentHolders[idx].SetActive(true);
	}
	public void updateInstructionText()
	{
		string str = string.Empty;

		if (CONTROLLER.selectedGameMode == GameMode.SuperOver)
		{
			setHolders(0);
			logo.sprite = modeLogo[0];
			//content.text = "Play one over for each level without losing two wickets. Each level holds a new challenge to complete. Get ready to finish all 18 challenges now!";
		}
		else if (CONTROLLER.selectedGameMode == GameMode.BattingMultiplayer)
		{
			setHolders(1);
			logo.sprite = modeLogo[3];
			//content.text = "Challenge yourself online against random opponents or create a private room and play with your friends. Compete now!";
		}
		else if (CONTROLLER.selectedGameMode == GameMode.ChaseTarget)
		{
			setHolders(2);
			logo.sprite = modeLogo[2];
			//content.text = "Prove yourself from Rookie to Legend by completing the 5 challenges instore for you at each level & rise to the top of leaderboard.";
		}
		else if (CONTROLLER.selectedGameMode == GameMode.OnlyBatting)
		{
			setHolders(3);
			logo.sprite = modeLogo[1];
			//content.text = "Try & top the leader board with maximum runs in 20 overs. Start Slogging!";
		}

		//content.text = str;
		//LayoutRebuilder.ForceRebuildLayoutImmediate(content.rectTransform);
		//if (instructionRect.content.rect.height >= instructionRect.viewport.rect.height)
		//	instructionRect.vertical = true;
		//else
		//	instructionRect.vertical = false;
	}

	public void HelpButtonClick()
	{
		HelpPanelHolder.SetActive(true);
	}

}
