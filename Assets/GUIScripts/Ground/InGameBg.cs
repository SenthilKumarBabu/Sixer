using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameBg : MonoBehaviour {

	public static InGameBg instance ;

	protected void  Awake ()
	{
		instance = this;
	}

	public void  HideMe ()
	{
		Destroy(this.gameObject);
		Resources.UnloadUnusedAssets ();
	}
}
