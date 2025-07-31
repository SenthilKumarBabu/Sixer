using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatHit : MonoBehaviour {
	private int uvAnimationTileX = 4;
	private int uvAnimationTileY = 4;
	private float framesPerSecond = 24;
	private int totalFrames;
	private bool lastFrameReached = false;
	private float effectStartTime = 0;


	void Start () {
		GetComponent<Renderer>().material.SetTextureScale ("_MainTex", new Vector2(0, 0));
		totalFrames = uvAnimationTileX * uvAnimationTileY;
		lastFrameReached = false;
		effectStartTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		float index = (Time.time - effectStartTime) * framesPerSecond;
		float totalElapsedFrames  = index;
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
