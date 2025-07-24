using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : Singleton <ScoreCounter > {
	public float duration = 5f;
	int score = 0, bonus = 0, total = 0;
	public Text points, bonusPoints, totalPoints;
	//private AnimationController anim;

	int nEarnedPoints,nBonusPoints,nTotPoints;
	public void SetValues(int _earned,int _bonus,int _total)
	{
		nEarnedPoints = _earned;
		nBonusPoints = _bonus;
		nTotPoints = _total;
		ScoreCountTo (nEarnedPoints ); 
	}


	void Update() 
	{
		points.text = score.ToString(); 
		bonusPoints.text = bonus.ToString ();
		totalPoints.text = total.ToString ();
	}

	public void ScoreCountTo (int target) {
		
			StopCoroutine ("CountTo");
			StartCoroutine ("CountTo", target);
	}

	public void BonusCountTo (int target) {

		StopCoroutine ("BonusCount");
		StartCoroutine ("BonusCount", target);
	}

	public void TotalCountTo (int target) {

		StopCoroutine ("TotalCount");
		StartCoroutine ("TotalCount", target);
	}

	IEnumerator CountTo (int target) {
		int start = score;
		for (float timer = 0; timer < duration; timer += Time.deltaTime) {
			float progress = timer / duration;
			score = (int)Mathf.Lerp (start, target, progress);
			yield return null;
		}
		score = target;
		yield return new WaitForSecondsRealtime (1.3f);
		BonusCountTo (nBonusPoints );

	}

	IEnumerator BonusCount (int target) {
		int start = bonus;
		for (float timer = 0; timer < duration; timer += Time.deltaTime) {
			float progress = timer / duration;
			bonus = (int)Mathf.Lerp (start, target, progress);
			yield return null;
		}
		bonus = target;
		yield return new WaitForSecondsRealtime (1.3f);
		TotalCountTo (nTotPoints );
	}

	IEnumerator TotalCount (int target) {
		int start = bonus;
		for (float timer = 0; timer < duration; timer += Time.deltaTime) {
			float progress = timer / duration;
			total = (int)Mathf.Lerp (start, target, progress);
			yield return null;
		}
		total = target;

	}
}
