using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CameraMove : Singleton<CameraMove> {
	private float rotateTime;
	private Vector3 startPos, startRot;

	void Awake() {
		rotateTime = 75f;
		startPos = transform.position;
		startRot = transform.eulerAngles;
	}
	public void CameraEnable() 
	{
		Scoreboard.instance.scoreBoard.SetActive (false);
		PreviewScreen.instance.PreviewBG.SetActive (false);
		GamePauseScreen.instance.playerDetails.SetActive (false);
		GamePauseScreen.instance.SetLoftState(false);

		GameModel.instance.shotHolder.SetActive (false);
		GameModel.instance.positionHolder.SetActive (false); 

		Sequence sq = DOTween.Sequence ();
		sq.Insert (0, this.transform.DOLocalMove (new Vector3 (10f, this.transform.position.y, this.transform.position.z), 75f));
		sq.Insert (0, this.transform.DORotate(new Vector3 (this.transform.eulerAngles.x, -15f, this.transform.eulerAngles.z), 75f));
		sq.SetLoops (-1, LoopType.Yoyo).SetUpdate (true);
	}

	public void CameraDisable() 
	{
		Scoreboard.instance.scoreBoard.SetActive (true);		
		PreviewScreen.instance.PreviewBG.SetActive (true);
		GamePauseScreen.instance.playerDetails.SetActive (true); 
		GamePauseScreen.instance.SetLoftState(true);

		DOTween.Clear ();
		transform.position = startPos;
		transform.eulerAngles = startRot;
	}

	public void reset()
	{
		DOTween.Clear ();
		transform.position = startPos;
		transform.eulerAngles = startRot;
	}



//	private void ResetAnim() {
//		Sequence sq = DOTween.Sequence ();
//		sq.Insert (0, this.transform.DOLocalMove (new Vector3 (25f, this.transform.position.y, this.transform.position.z), rotateTime));
//		sq.Insert (0, this.transform.DORotate(new Vector3 (this.transform.eulerAngles.x, -30f, this.transform.eulerAngles.z), rotateTime));
//		sq.SetLoops (-1, LoopType.Yoyo).SetUpdate (true);
//	}
//
//	public void timeAlter(int index) {
//		if (rotateTime + index > 0) {
//			rotateTime += index;
//			timer.text = rotateTime.ToString();
//			DOTween.Clear ();
//			transform.position = startPos;
//			transform.eulerAngles = startRot;
//			ResetAnim ();
//		}
//	}

}
