using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

private float rotationSpeed = 5;

public void Update ()
{
	transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);
}
}
