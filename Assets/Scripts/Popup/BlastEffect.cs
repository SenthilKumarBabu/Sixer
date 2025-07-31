using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastEffect : Singleton<BlastEffect>
{
	public Camera BlastCamera;
	public GameObject BlastParticles;
	ParticleSystem blastPS;
	private void Awake()
	{
		blastPS = BlastParticles.GetComponent<ParticleSystem>();
		BlastParticles.SetActive(false);
		BlastCamera.enabled = false;
	}

	public void playAnimation(Canvas canvas,bool canPlayBlast=true)
	{
		BlastCamera.enabled = true;
		canvas.worldCamera = BlastCamera;
		if (canPlayBlast)
		{
			BlastParticles.SetActive(true);
			AudioPlayer.instance.PlayGameSnd("celebration");
			//Invoke("StopBlastEffect", 10f);
		}
	}

	public void StopAnimation()
	{
		BlastParticles.SetActive(false);
		BlastCamera.enabled = false;
	}
	void StopBlastEffect()
	{
		blastPS.Stop();
	}
}
