using UnityEngine;
using System.Collections;

public class BowlingSpot : MonoBehaviour {

	private float initRotationSpeed = 360;//60;
	private float rotationSpeed = 360;
	private float zoomSpeed = 1.5f;
	private float zoomMin = 0.5f;
	private float zoomMax = 1;
	private string animationStatus = "idle";
	private float freezingStartTime;
	private float freezingDuring  = 0.5f; // secs
	private float scaleDuringFreeze = 0;
	
	public void Update ()
	{
		if(animationStatus == "idle")
		{
			return;
		}
		
		if(animationStatus == "spin")
		{
			transform.localEulerAngles += new Vector3(0,rotationSpeed * Time.deltaTime,0);
			float scale = Mathf.PingPong(Time.time * zoomSpeed, (zoomMax - zoomMin)) + zoomMin;
			transform.localScale = Vector3.one * scale;
			//transform.localScale.y *= 2; // to increase the visiblility of ball pitching spot...
			transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y * 2,transform.localScale.z);
		}
		else if(animationStatus == "spinToFreeze")
		{
			rotationSpeed -= (initRotationSpeed / freezingDuring) * Time.deltaTime;
			transform.localEulerAngles += new Vector3(0,rotationSpeed * Time.deltaTime,0);
			transform.localScale -= Vector3.one * ((scaleDuringFreeze - zoomMin)/freezingDuring) * Time.deltaTime;
			if(Time.time > (freezingStartTime + freezingDuring))
			{
				animationStatus = "idle";
			}
		}
	}
	
	public void HideBowlingSpot ()
	{
		GetComponent<Renderer>().enabled = false;
	}
	
	public void ShowBowlingSpot ()
	{
		GetComponent<Renderer>().enabled = true;
		rotationSpeed = initRotationSpeed;
		transform.localScale = Vector3.one;
		transform.localPosition = new Vector3(transform.localPosition.x,-0.02f,transform.localPosition.z);
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,0,transform.localEulerAngles.z);
		animationStatus = "spin";
	}
	
	public void FreezeBowlingSpot ()
	{
		freezingStartTime = Time.time;
		scaleDuringFreeze = transform.localScale.x;
		animationStatus = "spinToFreeze";	
	}
}
