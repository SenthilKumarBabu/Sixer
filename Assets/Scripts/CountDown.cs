using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour {

	public static CountDown instance  ;
	public GameObject[] NumberGo ;

	protected void  Awake ()
	{
		instance = this;

		//setActive (false ); 
	}

	private void setActive(bool flag)
	{
		foreach (GameObject go in NumberGo )
		{
			go.SetActive (flag ); 
		}
	}



	public  IEnumerator StartCountdown ()
	{
		/*setActive (true ); 
		int  i ;
		for(i=0;i<3;i++)
		{
			iTween.ScaleTo (NumberGo[i], iTween .Hash ("scale",new  Vector3(0.2f,0.2f,0.2f), "time", 0, "delay", 0));
			iTween.FadeTo (NumberGo[i], iTween .Hash ("alpha", 0, "time", 0, "delay", 0));
		}
		iTween.ScaleTo (NumberGo[2], iTween .Hash("scale",new  Vector3(1f,1f,1f), "time", 0.5, "delay", 0.5));
		iTween.FadeTo (NumberGo[2], iTween .Hash("alpha", 1, "time", 0.5, "delay", 0.5));*/

		yield return new  WaitForSeconds (0.7f);
		StartCoroutine (AnimateTwo () );
	}
	private IEnumerator  AnimateTwo ()
	{
		yield return new  WaitForSeconds (0.2f);
		/*iTween.ScaleTo (NumberGo[2], iTween.Hash ("scale",new  Vector3(1.5f,1.5f,1.5f), "time", 0.4, "delay", 0.4));
		iTween.FadeTo (NumberGo[2], iTween .Hash ("alpha", 0, "time", 0.4, "delay", 0.4));

		iTween.ScaleTo (NumberGo[1], iTween .Hash ("scale",new  Vector3(1,1,1), "time", 0.5, "delay", 0.5));
		iTween.FadeTo (NumberGo[1], iTween .Hash ("alpha", 1, "time", 0.5, "delay", 0.5));*/
		yield return new  WaitForSeconds (0.7f);
		StartCoroutine (AnimateThree ());
	}

	private IEnumerator  AnimateThree ()
	{
		yield return new  WaitForSeconds (0.2f);
		/*iTween.ScaleTo (NumberGo[1], iTween.Hash ("scale",new  Vector3(1.5f,1.5f,1.5f), "time", 0.4, "delay", 0.4));
		iTween.FadeTo (NumberGo[1], iTween .Hash ("alpha", 0, "time", 0.4, "delay", 0.4));

		iTween.ScaleTo (NumberGo[0], iTween .Hash ("scale",new  Vector3(1,1,1), "time", 0.5, "delay", 0.5));
		iTween.FadeTo (NumberGo[0], iTween .Hash ("alpha", 1, "time", 0.5, "delay", 0.5));*/
		yield return new  WaitForSeconds (0.7f);
		StartCoroutine (AnimateGO());
	}

	private IEnumerator  AnimateGO()
	{
		yield return new  WaitForSeconds (0.2f);
		/*iTween.ScaleTo (NumberGo[0],iTween .Hash ("scale",new  Vector3(1.5f,1.5f,1.5f), "time", 0.4, "delay", 0.4));
		iTween.FadeTo (NumberGo[0], iTween .Hash ("alpha", 0, "time", 0.4, "delay", 0.4));

		iTween.ScaleTo (NumberGo[3], iTween .Hash ("scale",new  Vector3(1f,1f,1f), "time", 0.5, "delay", 0.5, "oncomplete","FadeThree", "oncompletetarget",this.gameObject));
		iTween.FadeTo (NumberGo[3], iTween.Hash ("alpha", 1, "time", 0.5, "delay", 0.5));*/

		StopAnimation ();
	}

	public void  FadeThree ()
	{
		/*iTween.ScaleTo (NumberGo[3], iTween .Hash ("scale",new  Vector3(1.5f,1.5f,1.5f), "time", 0.4, "delay", 0.4, "oncomplete","StopAnimation", "oncompletetarget",this.gameObject));
		iTween.FadeTo (NumberGo[3], iTween .Hash ("alpha", 0, "time", 0.4, "delay", 0.4));*/
	}


	 


	public void  StopAnimation ()
	{
		HideThis ();
	}

	public void  HideThis ()
	{
		BatsmanInfo.instance.UpdateStrikerInfo ();//29march
		CONTROLLER.NewInnings = false;
		Scoreboard.instance.Hide (true);//25march
		BatsmanInfo.instance.ShowMe ();
		Scoreboard.instance.HidePause (false);//25march
		PreviewScreen.instance.Hide (false);//25march
		BattingScoreCard.instance .HideMe (); 
	}
}
