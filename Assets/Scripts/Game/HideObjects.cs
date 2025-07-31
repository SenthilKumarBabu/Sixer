using UnityEngine;
using System.Collections;

public class HideObjects : MonoBehaviour {

	public void Start ()
	{
		foreach (Transform trans1 in transform)
		{
			foreach (Transform trans2 in trans1)
			{
				Renderer cube = trans2.GetComponent <Renderer> ();
				cube.GetComponent<Renderer>().enabled = false;
			}
		}
	}
}
