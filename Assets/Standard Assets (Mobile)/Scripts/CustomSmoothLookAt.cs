using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSmoothLookAt : MonoBehaviour 
{

public 	Transform target ;
public 	float  damping = 6.0f;
public 	bool smooth = true;


	void  LateUpdate () {
		if (target) 
		{
			//if(GroundController.ballOnboundaryLine == false)
			if(GroundController.instance .action > 2 && AnimationScreen.instance != null)
			{
				if (smooth)
				{
					// Look at and dampen the rotation
					Quaternion  rotation = Quaternion.LookRotation(target.position - transform.position);
					transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
				}
				else
				{
					transform.LookAt(target);
				}
			}
		}
	}

	void  Start () {
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}
}
