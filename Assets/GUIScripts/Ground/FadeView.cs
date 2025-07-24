using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeView : Singleton<FadeView>
{

	public GameObject fade ;

	protected void  Awake ()
	{
		this.Hide (false);
	}

	public void  FadeOut (float  sec)
	{
		Hashtable fadeHash = new Hashtable ();
		fadeHash.Add ("alpha", 0);
		fadeHash.Add ("time", sec);
		fadeHash.Add ("oncomplete", "FadeOutCompleted");
		fadeHash.Add ("oncompletetarget", this.gameObject);
		iTween.FadeTo (fade, fadeHash);
	}

	public void  FadeOutCompleted ()
	{
		this.Hide (true);
	}

	public void  FadeIn (float  sec )
	{
		Hashtable fadeHash  = new Hashtable ();
		fadeHash.Add ("alpha", 1);
		fadeHash.Add ("time", sec);
		iTween.FadeTo (fade.gameObject, fadeHash);
	}
	public void  Hide(bool  boolean)
	{
		if(boolean == true)
		{
			FadeIn (1);
			fade.SetActive (false);
		}
		else
		{
			FadeOut (1);
			fade.SetActive (true);
		}
	}
}
