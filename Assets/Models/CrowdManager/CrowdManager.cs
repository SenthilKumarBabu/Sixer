using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
	public Material[] crowdMaterials;
	public Texture2D[] crowdTextures;
	public Texture2D[] chairTextures;
	private int[] crowdArray = new int[8];

	private float FPS = 12;
	private int cheerAnimationStartingFrame = 1;
	private int cheerAnimationEndingFrame = 8;
	private int currentCheerFrameIndex = 1;
	private int[] currentCheerFrameIndexArray = new int [8];
	private float[] crowdMaterialStartingDelayTime = new float[8];
	private int materialLength = 8; // should be dynamic...
	private float yOffset = 0.0f;
	private float yOffsetIncrement = 0.125f;
	private int i = 0;
	private float frameExecutionTime = 0.0f;
	private float lastframeExecutionTime = 0.0f;

	// Use this for initialization
	void Start ()
	{
		lowEndDevice = IsLowEndDevice();
		FPS = 12.0f;
		frameExecutionTime = 1 / FPS;
		lastframeExecutionTime = Time.time;
		ResetCrowdTextures ();
	}
	private bool lowEndDevice = false;
	public bool IsLowEndDevice()
	{
		bool status = false;
		////CONTROLLER.GameLog("IsLowEndDevice :: SystemInfo.systemMemorySize :: "+SystemInfo.systemMemorySize);
		if (SystemInfo.systemMemorySize <= 1024 || Screen.width <= 850)
		{ // low end device
			status = true;

		}
		return status;
	}
	void Update ()
	{
		//if (lowEndDevice)
		//{
		//	return;
		//}
		//else
		{
			AnimateCheerCrowd();
		}
	}		
		
	public void ResetCrowdTextures ()
	{
		int maxNoOfChairRows = 0;
		int noOfChairRows = 0;

		CONTROLLER.crowdDensity = 75f;
		maxNoOfChairRows = 2;
		currentCheerFrameIndex = cheerAnimationStartingFrame;
		
		int chairTextureIndex = 0;
		int tempSelecteGroundId = 0;
		int.TryParse (CONTROLLER.selectedGround, out tempSelecteGroundId);

		if (tempSelecteGroundId <= 10) {
			chairTextureIndex = 0;
		}  else if (tempSelecteGroundId <= 20) {
			chairTextureIndex = 1;
		}  else {
			chairTextureIndex = 2;
		}

		for (i = 0; i < materialLength; i++) {
			currentCheerFrameIndexArray [i] = cheerAnimationStartingFrame;
			crowdMaterialStartingDelayTime [i] = 0.0f; //Random.Range (0.0f, 0.5f);

			if (Random.Range (0.0f, 100.0f) < CONTROLLER.crowdDensity || noOfChairRows >= maxNoOfChairRows)
			{
				crowdMaterials [i].mainTexture = crowdTextures [i]; // applying respective crowd texture to this material...
				crowdArray [i] = 1;
			}
			else
			{
				crowdMaterials [i].mainTexture = chairTextures[chairTextureIndex]; // placing with empty chair texture...
				crowdArray [i] = 0;
				noOfChairRows++;
			}
		}
	}
	private void AnimateCheerCrowd()
	{
		if (lastframeExecutionTime + frameExecutionTime < Time.time)
		{
			for (i = 0; i < crowdArray.Length; i++)
			{
				if (crowdMaterialStartingDelayTime[i] <= Time.time && crowdArray[i] == 1)
				{
					yOffset = 1 - currentCheerFrameIndexArray[i] * yOffsetIncrement;
					crowdMaterials[i].mainTextureOffset = new Vector2(0.0f, yOffset);
					currentCheerFrameIndexArray[i] += 1;
					if (currentCheerFrameIndexArray[i] > cheerAnimationEndingFrame)
					{
						currentCheerFrameIndexArray[i] = cheerAnimationStartingFrame;
						crowdMaterialStartingDelayTime[i] = Time.time + 0.0f; //Random.Range (0.1f, 0.6f);
					}
				}
			}
			lastframeExecutionTime = Time.time;
		}
	}
	// Texture offset for animation effect => 0.875, 0.75, 0.625, 0.5, 0.375, 0.25  // 0.125, 0.0 // 8 Crowd texture sheets || 6 frames for Cheer || 2 frames for normal...
	//private void AnimateCheerCrowd ()
	//{
	//	if (lastframeExecutionTime + frameExecutionTime < Time.unscaledTime)
	//	{
	//		for (i = 0; i < crowdArray.Length; i++)
	//		{
	//			if (crowdMaterialStartingDelayTime [i] <= Time.unscaledTime && crowdArray [i] == 1)
	//			{
	//				yOffset = 1 - currentCheerFrameIndexArray [i] * yOffsetIncrement;
	//				crowdMaterials [i].mainTextureOffset = new Vector2 (0.0f, yOffset);
	//				currentCheerFrameIndexArray [i] += 1;
	//				if (currentCheerFrameIndexArray [i] > cheerAnimationEndingFrame) {
	//					currentCheerFrameIndexArray [i] = cheerAnimationStartingFrame;
	//					crowdMaterialStartingDelayTime [i] = Time.unscaledTime + 0.0f; //Random.Range (0.1f, 0.6f);
	//				}
	//			}
	//		}
	//		lastframeExecutionTime = Time.unscaledTime;
	//	}
	//}
}


