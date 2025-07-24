using UnityEngine;
using System.Collections;

public class HideCameraFlashRef : MonoBehaviour {

	// Use this for initialization
	void Start () {
		foreach (Transform trans1 in transform)
			{
				Renderer cube = trans1.GetComponent <Renderer> ();
				cube.GetComponent<Renderer>().enabled = false;
			}
	}

}
