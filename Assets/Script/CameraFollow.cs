using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	public float damping = 6f;
	float fov;
	Camera cam;
	public bool canRotate = false;
	public float fovOffset = 5f;
	public float smoothSpeed = 0.125f, zoomSpeed = 3f;
	public Vector3 offset;
	public Vector3 start, end;

	void  Start () {
		cam = this.GetComponent<Camera> ();
		fov = this.GetComponent<Camera> ().fieldOfView;
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}

	void LateUpdate () {
		if (target) {
			if (GroundController.instance.action > 2 && AnimationScreen.instance != null) {
				Vector3 desiredPosition = target.position + offset;
				Vector3 smoothPosition = Vector3.Lerp (transform.position, desiredPosition, smoothSpeed);
				//this.GetComponent<Camera> ().fieldOfView = Mathf.Lerp (fov, fovOffset, Time.deltaTime);
			
				//offset = Vector3.Slerp (start, end, Time.deltaTime * zoomSpeed);
				transform.position = smoothPosition;

				Quaternion rotation = Quaternion.LookRotation (target.position - transform.position);
				transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * damping);
				if(canRotate)
					this.GetComponent<Camera> ().transform.eulerAngles = new Vector3 (this.GetComponent<Camera> ().transform.eulerAngles.x - 0.1f, this.GetComponent<Camera> ().transform.eulerAngles.y, this.GetComponent<Camera> ().transform.eulerAngles.z); 
			}
		}
	}
}
