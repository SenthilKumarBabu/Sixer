using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayAnimation : MonoBehaviour {

	private Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		anim.Play("ray");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
