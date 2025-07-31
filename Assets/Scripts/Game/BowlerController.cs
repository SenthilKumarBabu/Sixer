using UnityEngine;
using System.Collections;

public class BowlerController : MonoBehaviour {

	private GroundController ballScript;

	public void Start ()
	{
		ballScript = GroundController.instance;
	}
	
	public void TriggerCameraZoom ()
	{
		ballScript.ZoomCameraToPitch ();
	}
	
	public void FreezeTheBowlingSpot ()
	{
		ballScript.FreezeTheBowlingSpot ();
	}
	
	public void ReleaseTheBall ()
	{
		ballScript.ReleaseTheBall ();
	}

	public void HideBowler ()
	{
		
		ballScript.ShowFielder10 (true, false);
		ballScript.ShowBowler (false);
	}
}
