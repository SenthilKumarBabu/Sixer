using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modeselectorpage : MonoBehaviour 
{
	void Start()
	{
		CONTROLLER.CurrentPage = "splashpage";
	}
	void OnEnable () 
	{
		CONTROLLER.tempCurrentPage = "splashpage";
		CONTROLLER.CurrentPage = "splashpage";
	}

}
