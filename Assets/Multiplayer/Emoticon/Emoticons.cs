using UnityEngine;
using System.Collections;

public class Emoticons : MonoBehaviour {

	public GameObject smileyBtn;
	public GameObject closeBtn;

	public GameObject posi;
	private bool isTransition;
	private float transitionTime,transitionTotTime,Moving_Src,Moving_Dest;

	void Awake()
	{	posi.SetActive (false);
		smileyBtn.SetActive (true);
		closeBtn.SetActive (false);
		isTransition = false;
		transitionTotTime = 0.5f;
	}
	public void OpenEmoticons ()
	{
		if (!isTransition) {
			posi.SetActive (true);
			transitionTime = 0f;
			isTransition = true;
		}
	}

	public void CloseEmoticons ()
	{
		if (!isTransition) {
			posi.GetComponent<Animator> ().SetTrigger ("close");
			isTransition = true;
			transitionTime = 0f;
			StartCoroutine (StopAnimation ());
		}

	}

	IEnumerator StopAnimation() {
		yield return new WaitForSeconds (0.5f);
		posi.SetActive (false);
	}


	public void Update()
	{

		if(isTransition)
		{
			float tmpX=Linear (transitionTime, Moving_Src , Moving_Dest, transitionTotTime); 
			transitionTime += Time.deltaTime;

			if(transitionTime >transitionTotTime )
			{	
				isTransition = false;

				if(smileyBtn .activeSelf)
				{
					smileyBtn.SetActive (false );
					closeBtn.SetActive (true );	
				}
				else
				{
					smileyBtn.SetActive (true );
					closeBtn.SetActive (false );
				}
			}
		}
	}

	float Linear(float t, float source, float destination, float duration)
	{
		return (destination  - source ) * t / duration  + source ;
	}

}
