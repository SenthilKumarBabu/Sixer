using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: Component{

	private static T Instance;
	// Use this for initialization
	public static T instance {
		get {
			if(Instance == null) {
				Instance = (T) FindObjectOfType(typeof(T));
			}
			return Instance;
		}
	}

	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
