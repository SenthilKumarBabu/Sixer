using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlashScript : MonoBehaviour {

	private int uvAnimationTileX = 4;
	private int uvAnimationTileY = 1;
	private float framesPerSecond = 24.0f; //24.0
	private int totalFrames;
	private bool lastFrameReached = false;
	private float effectStartTime = 0.0f;

	public void Start ()
	{
		GetComponent<Renderer>().material.SetTextureScale ("_MainTex", new Vector2(0, 0));
		totalFrames = uvAnimationTileX * uvAnimationTileY;
		lastFrameReached = false;
		effectStartTime = Time.time;
	}

	public void Update () 
	{
		int index = (int)((Time.time - effectStartTime) * framesPerSecond);
		int totalElapsedFrames = index;
		index = index % totalFrames;

		if(lastFrameReached == true)
		{
			Destroy(gameObject);
		}

		Vector2 size = new Vector2 (1.0f / uvAnimationTileX, 1.0f / uvAnimationTileY);

		// split into horizontal and vertical index
		float uIndex = index % uvAnimationTileX;
		float vIndex = index / uvAnimationTileX;


		// build offset // v coordinate is the bottom of the image in opengl so we need to invert.
		Vector2 offset = new Vector2 (uIndex * size.x, 1.0f - size.y - vIndex * size.y);

		GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", offset);
		GetComponent<Renderer>().material.SetTextureScale ("_MainTex", size);
		if(totalElapsedFrames >= (totalFrames - 1))
		{
			lastFrameReached = true;
		}
	}
}
