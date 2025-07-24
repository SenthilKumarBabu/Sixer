using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgScreen : MonoBehaviour {

	public static BgScreen instance  ;

	protected void  Awake ()
	{
		instance = this;
	}

	protected void  Start ()
	{
//		yield WaitForSeconds (0.5);
		//GameLogo.SetToggleState (CONTROLLER.gameMode);
	}	
}
