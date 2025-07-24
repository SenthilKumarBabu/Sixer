using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewScreen : Singleton<PreviewScreen> {
	public GameObject GroundBG  ;
	public GameObject PreviewBG ;
	public GameObject BallIcon ;
	public GameObject StrikerIcon ;
	public GameObject RunnerIcon ;
	public GameObject[] FielderIcon ;
	private Camera renderCamera ;

	public RectTransform SafeArea;

	protected void Start ()
	{
		renderCamera = GameObject.Find ("MainCamera").GetComponent<Camera>();
        SafeAreaManager.Initialize();
        SafeAreaManager.ApplySafeArea(SafeArea);
    }

	private void  AlignMe ()
	{
//		var screenPos : Vector2 = new Vector2 (Screen.width, Screen.height);
//		var newPos : Vector3 = new Vector3 (0,0,0);
		//newPos = renderCamera.ScreenToWorldPoint (new Vector3 (screenPos.x, screenPos.y, -0.02));
		//PreviewBG.transform.localPosition.x = 0;//-newPos.x;
	}

	public void  UpdatePreviewScreen (Dictionary<string ,object >  hashtable)
	{
		Vector3 posIn3D  ;
		float ratio   = 1;

		if(hashtable.ContainsKey("Ball"))
		{
			BallIcon.SetActive (true);
			posIn3D =(Vector3 ) hashtable["Ball"];
			posIn3D = posIn3D * ratio;
			posIn3D.x = posIn3D.x - GroundBG.transform.localPosition.x;
			posIn3D.z = posIn3D.z - GroundBG.transform.localPosition.y;
			BallIcon.transform.localPosition = new Vector3(-posIn3D.x, -(posIn3D.z), 0);
		}
		else
		{
			BallIcon.SetActive (false);
		}
		if(hashtable.ContainsKey ("Striker"))
		{
			StrikerIcon.SetActive (true);
			posIn3D = (Vector3 )hashtable["Striker"];
			posIn3D = posIn3D * ratio;
			posIn3D.x = posIn3D.x - GroundBG.transform.localPosition.x;
			posIn3D.z = posIn3D.z - GroundBG.transform.localPosition.y;
			StrikerIcon.transform.localPosition = new Vector3(-posIn3D.x, -(posIn3D.z), 0);
		}
		else
		{
			StrikerIcon.SetActive (false);
		}
		if(hashtable.ContainsKey("NonStriker"))
		{
			RunnerIcon.SetActive (true);
			posIn3D =(Vector3 ) hashtable["NonStriker"];
			posIn3D = posIn3D * ratio;
			posIn3D.x = posIn3D.x - GroundBG.transform.localPosition.x;
			posIn3D.z = posIn3D.z - GroundBG.transform.localPosition.y;
			RunnerIcon.transform.localPosition = new Vector3(-posIn3D.x, -(posIn3D.z), 0);
		}
		else
		{
			RunnerIcon.SetActive (false);
		}
		if(hashtable.ContainsKey("field_01"))
		{
			posIn3D = (Vector3 )hashtable["field_01"];
			posIn3D = posIn3D * ratio;
			posIn3D.x = posIn3D.x - GroundBG.transform.localPosition.x;
			posIn3D.z = posIn3D.z - GroundBG.transform.localPosition.y;
			FielderIcon[0].transform.localPosition = new Vector3(-posIn3D.x, -(posIn3D.z), 0);
		}
		if(hashtable.ContainsKey("field_02"))
		{
			posIn3D =(Vector3 ) hashtable["field_02"];
			posIn3D = posIn3D * ratio;
			posIn3D.x = posIn3D.x - GroundBG.transform.localPosition.x;
			posIn3D.z = posIn3D.z - GroundBG.transform.localPosition.y;
			FielderIcon[1].transform.localPosition = new Vector3(-posIn3D.x, -(posIn3D.z), 0);
		}
		if(hashtable.ContainsKey("field_03"))
		{
			posIn3D = (Vector3 )hashtable["field_03"];
			posIn3D = posIn3D * ratio;
			posIn3D.x = posIn3D.x - GroundBG.transform.localPosition.x;
			posIn3D.z = posIn3D.z - GroundBG.transform.localPosition.y;
			FielderIcon[2].transform.localPosition = new Vector3(-posIn3D.x, -(posIn3D.z), 0);
		}
		if(hashtable.ContainsKey("field_04"))
		{
			posIn3D =(Vector3 ) hashtable["field_04"];
			posIn3D = posIn3D * ratio;
			posIn3D.x = posIn3D.x - GroundBG.transform.localPosition.x;
			posIn3D.z = posIn3D.z - GroundBG.transform.localPosition.y;
			FielderIcon[3].transform.localPosition = new Vector3(-posIn3D.x, -(posIn3D.z), 0);
		}
		if(hashtable.ContainsKey("field_05"))
		{
			posIn3D = (Vector3 )hashtable["field_05"];
			posIn3D = posIn3D * ratio;
			posIn3D.x = posIn3D.x - GroundBG.transform.localPosition.x;
			posIn3D.z = posIn3D.z - GroundBG.transform.localPosition.y;
			FielderIcon[4].transform.localPosition = new Vector3(-posIn3D.x, -(posIn3D.z), 0);
		}
		if(hashtable.ContainsKey("field_06"))
		{
			posIn3D = (Vector3 )hashtable["field_06"];
			posIn3D = posIn3D * ratio;
			posIn3D.x = posIn3D.x - GroundBG.transform.localPosition.x;
			posIn3D.z = posIn3D.z - GroundBG.transform.localPosition.y;
			FielderIcon[5].transform.localPosition = new Vector3(-posIn3D.x, -(posIn3D.z), 0);
		}
		if(hashtable.ContainsKey("field_07"))
		{
			posIn3D =(Vector3 ) hashtable["field_07"];
			posIn3D = posIn3D * ratio;
			posIn3D.x = posIn3D.x - GroundBG.transform.localPosition.x;
			posIn3D.z = posIn3D.z - GroundBG.transform.localPosition.y;
			FielderIcon[6].transform.localPosition = new Vector3(-posIn3D.x, -(posIn3D.z), 0);
		}
		if(hashtable.ContainsKey("field_08"))
		{
			posIn3D = (Vector3 )hashtable["field_08"];
			posIn3D = posIn3D * ratio;
			posIn3D.x = posIn3D.x - GroundBG.transform.localPosition.x;
			posIn3D.z = posIn3D.z - GroundBG.transform.localPosition.y;
			FielderIcon[7].transform.localPosition = new Vector3(-posIn3D.x, -(posIn3D.z), 0);
		}
		if(hashtable.ContainsKey("field_09"))
		{
			posIn3D = (Vector3 )hashtable["field_09"];
			posIn3D = posIn3D * ratio;
			posIn3D.x = posIn3D.x - GroundBG.transform.localPosition.x;
			posIn3D.z = posIn3D.z - GroundBG.transform.localPosition.y;
			FielderIcon[8].transform.localPosition = new Vector3(-posIn3D.x, -(posIn3D.z), 0);
		}
		if(hashtable.ContainsKey("field_10"))
		{
			posIn3D = (Vector3 )hashtable["field_10"];
			posIn3D = posIn3D * ratio;
			posIn3D.x = posIn3D.x - GroundBG.transform.localPosition.x;
			posIn3D.z = posIn3D.z - GroundBG.transform.localPosition.y;
			FielderIcon[9].transform.localPosition = new Vector3(-posIn3D.x, -(posIn3D.z), 0);
		}
		if(hashtable.ContainsKey("field_11"))
		{
			posIn3D =(Vector3 ) hashtable["field_11"];
			posIn3D = posIn3D * ratio;
			posIn3D.x = posIn3D.x - GroundBG.transform.localPosition.x;
			posIn3D.z = posIn3D.z - GroundBG.transform.localPosition.y;
			FielderIcon[10].transform.localPosition = new Vector3(-posIn3D.x, -(posIn3D.z), 0);
		}
	}
	public void  Hide (bool  boolean)
	{
		float   showPos = CONTROLLER.xOffSet;
		if(boolean == true)
		{
			PreviewBG.SetActive (false);
		}
		else
		{
			PreviewBG.SetActive (true);
		}
	}
}
