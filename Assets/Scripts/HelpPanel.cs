using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpPanel : MonoBehaviour
{
	/*public Text content;
	public ScrollRect instructionRect;

	public void OnEnable()
	{
		if (CONTROLLER.gameMode == "multiplayer")
			content.text = "Here are the points:\n\n" + "1. For every four : " + CONTROLLER.boundaryPoint + " points.\n\n" + "2. For every six : " + CONTROLLER.sixPoint + " points.\n\n" + "3. For each single and double, " + CONTROLLER.singlePoint + " and " + CONTROLLER.doublePoint + " points respectively."
				+ ("\n\n Bonus Coins \n In 2 Overs : 1st  - 500, 2nd - 400 , 3rd - 300, 4th - 200 &  5th - 100" + "\n For 5 Overs : 1st  - 1000,  2nd - 800, 3rd - 600, 4th - 400 & 5th -200 \n *only in Public Mode").set;
		else
			content.text = "Here are the points:\n\n" + "1. For every four : " + CONTROLLER.boundaryPoint + " points.\n\n" + "2. For every six : " + CONTROLLER.sixPoint + " points.\n\n" + "3. For losing a wicket : " + CONTROLLER.wicketPoint + " points.\n\n" + "4. For every dot ball : " + CONTROLLER.dotBallPoint + " points.\n\n" + "5. For each single and double," + CONTROLLER.singlePoint + " and " + CONTROLLER.doublePoint + " points respectively.";
				

		LayoutRebuilder.ForceRebuildLayoutImmediate(content.rectTransform);
		if (instructionRect.content.rect.height >= instructionRect.viewport.rect.height)
			instructionRect.vertical = true;
		else
			instructionRect.vertical = false;
	}*/

	public Text Title;
	public GameObject Dot, Wicket;
	public GameObject MultiplayerBonusHolder;
	public void OnEnable()
	{

		if (CONTROLLER.gameMode == string.Empty)
		{
			bottomButtons.SetActive(true);
			OnButtonClick(0);
		}
		else
		{
			bottomButtons.SetActive(false);

			if (CONTROLLER.gameMode == "multiplayer")
			{
				Dot.SetActive(false);
				Wicket.SetActive(false);
				MultiplayerBonusHolder.SetActive(true);
				Title.text = "SUPER MULTIPLAYER";
			}
			else
			{
				Dot.SetActive(true);
				Wicket.SetActive(true);
				MultiplayerBonusHolder.SetActive(false);
				if (CONTROLLER.gameMode == "slogover")
					Title.text = "SUPER SLOG";
				else if (CONTROLLER.gameMode == "superover")
					Title.text = "SUPER CHASE & SUPER SLOG";
				else
					Title.text = "SUPER CHASE";
			}
		}
	}

	public GameObject bottomButtons;
	public GameObject page1on, page2on;
	public Button page1Button, page2Button;
	public void OnButtonClick(int idx)
	{
		switch(idx)
		{
			case 0: //page 1
				Title.text = "SUPER CHASE & SUPER SLOG";
				page1on.SetActive(true);
				page2on.SetActive(false);
				page1Button.gameObject.SetActive(false);
				page2Button.gameObject.SetActive(true);
				Dot.SetActive(true);
				Wicket.SetActive(true);
				MultiplayerBonusHolder.SetActive(false);
				break;
			case 1:     //page 2
				Title.text = "SUPER MULTIPLAYER";

				page1on.SetActive(false);
				page2on.SetActive(true);
				page1Button.gameObject.SetActive(true);
				page2Button.gameObject.SetActive(false);
				Dot.SetActive(false);
				Wicket.SetActive(false);
				MultiplayerBonusHolder.SetActive(true);
				break;
		}
	}
	public void Hide()
	{
        AudioPlayer.instance.PlayButtonSnd();
        this.gameObject.SetActive(false);
	}
}
