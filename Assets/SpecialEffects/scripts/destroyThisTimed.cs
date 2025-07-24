using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyThisTimed : MonoBehaviour {

	float destroyTime  = 5;

	void  Start ()
	{
		Destroy (gameObject, destroyTime);
	}
}
