using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine .UI;

public class SixDistanceDisplayer : MonoBehaviour 
{
	public static SixDistanceDisplayer instance  ;
	public Image stripBg  ;
	public Text  stripText  ;

	public Camera mainCamera  ;
	public Camera rightSideCamera  ;
	public Camera leftSideCamera ;
	public Camera straightCamera ;
	public Camera RenderCamera ;

	protected void  Awake ()
	{
		instance = this;
		this.gameObject.layer = LayerMask.NameToLayer("SixDistance");
		show (false ); 
	}

	public  void show(bool flag)
	{
		stripBg.enabled = flag;
		stripText.enabled = flag;
	}

	public void  PositionSixDistanceProfile (GameObject ballGO,string  battingHand)
	{
		Vector3 ScreenPos=RenderCamera.WorldToScreenPoint(mainCamera.WorldToScreenPoint (ballGO.transform.position));
		Vector3 DisplayPos;
		if (mainCamera.enabled == true)
		{
			ScreenPos = RenderCamera.WorldToScreenPoint(mainCamera.WorldToScreenPoint (ballGO.transform.position));
		}
		else if (leftSideCamera.enabled == true)
		{
			ScreenPos = RenderCamera.WorldToScreenPoint(leftSideCamera.WorldToScreenPoint (ballGO.transform.position));
		}
		else if (rightSideCamera.enabled == true)
		{
			ScreenPos = RenderCamera.WorldToScreenPoint(rightSideCamera.WorldToScreenPoint (ballGO.transform.position));
		}
		else if (straightCamera.enabled == true)
		{
			ScreenPos = RenderCamera.WorldToScreenPoint(straightCamera.WorldToScreenPoint (ballGO.transform.position));
		}
		DisplayPos = RenderCamera.ScreenToWorldPoint(ScreenPos);

		/*if (rightSideCamera.enabled == false)
		{
			if (battingHand == "right")
			{
				stripBg.transform.localPosition.x = DisplayPos.x - (Screen.width / 2) + 85;
				stripText.transform.localPosition.x = DisplayPos.x - (Screen.width / 2) + 85;
			}
			else
			{
				stripBg.transform.localPosition.x = DisplayPos.x - (Screen.width / 2) + 85;
				stripText.transform.localPosition.x = DisplayPos.x - (Screen.width / 2) + 85;
			}
		}
		else
		{
			if (battingHand == "right")
			{
				stripBg.transform.localPosition.x = DisplayPos.x - (Screen.width / 2) + 85;
				stripText.transform.localPosition.x = DisplayPos.x - (Screen.width / 2) + 85;
			}
			else
			{
				stripBg.transform.localPosition.x = DisplayPos.x - (Screen.width / 2) + 85;
				stripText.transform.localPosition.x = DisplayPos.x - (Screen.width / 2) + 85;
			}
		}*/


		stripBg.transform.localPosition =new Vector3 (DisplayPos.x - (Screen.width / 2),DisplayPos.y - (Screen.height / 2) - 25,-2f);
		stripText.transform.localPosition =new Vector3 ( DisplayPos.x - (Screen.width / 2),DisplayPos.y - (Screen.height / 2) - 25,-2.01f);



	}

}
