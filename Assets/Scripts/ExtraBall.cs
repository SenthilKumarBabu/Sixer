using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ExtraBall : Singleton<ExtraBall> {

	public GameObject holder;
	void Start () 
	{
		HideMe ();
	}

	public void WatchVideo() 
	{
		AudioPlayer.instance.PlayButtonSnd();
		if (AdIntegrate.instance != null)
		{
			if (AdIntegrate.instance.checkTheInternet ())
			{
					ShowToast("No video Available"); 
			}
			else
			{
				ShowToast("you're not connected to the Internet"); 
			}

		}
	}

	void ShowToast(string text)
	{
		GameObject prefabGO ;
		GameObject tempGO ;
		prefabGO = Resources.Load ("Prefabs/Toast")as GameObject ;
		tempGO = Instantiate (prefabGO)as GameObject ;
		tempGO.name = "Toast";
		tempGO.GetComponent <Toast > ().setMessge (text);
	}

	public void Success()
	{
		for (int i = 0; i < 6; i++) 
		{
			Scoreboard.instance.BallInfo [i].transform.parent.gameObject.SetActive (false);
		}
		Scoreboard.instance.extraBall.SetActive (true);
		GameModel.OversStr = GameModel.instance.GetOverStr () + " (" + CONTROLLER.totalOvers + ")";
		GameModel.instance.DetailsSavedBallbyBall ();
		BattingScoreCard.instance.CloseButtonClicked ();
		HideMe ();
		//GamePauseScreen.instance.playerDetails.SetActive (true);	
		CONTROLLER.CurrentPage = "ingame";
		GameModel.instance.GamePaused (false);
	}
	public void ClosePopup() 
	{
		if (glowSeq != null)
			glowSeq.Kill();
		AudioPlayer.instance.PlayButtonSnd();
		CONTROLLER.currentMatchBalls += 2;
		HideMe ();
		AdIntegrate.instance.SetTimeScale(1f);
		GameModel.instance.CheckForOverComplete ();
	}

	public void ShowMe() 
	{
		if (glowSeq != null)
			glowSeq.Kill();
		GamePauseScreen.instance.playerDetails.SetActive (false);
		GamePauseScreen.instance.SetLoftState(false);
		GameModel.isGamePaused = true;
		ReSetAnimation();
		holder.SetActive (true);
		startAnim();
		AdIntegrate.instance.SetTimeScale(0f);
	}

	public void HideMe() 
	{
		holder.SetActive (false);
	}

	//void OnApplicationQuit()
	//{
	//	if(holder .activeSelf )
	//	{
	//		ClosePopup (); 
	//	}
	//}

	#region ANIMATION
	public Image BlueBG;
	public Text TitleText;
	public Button playButton, closeButton;

	public Transform ButtonGlow;
	Sequence glowSeq;

	void ReSetAnimation()
	{
		//reset
		BlueBG.transform.DOScale(0f, 0);
		BlueBG.DOFade(0f, 0);
		TitleText.transform.DOScale(0.5f, 0);
		TitleText.DOFade(0f, 0);
		playButton.transform.DOScale(0f, 0);
		closeButton.transform.DOScale(0f, 0);

		if (glowSeq != null)
			glowSeq.Kill();
		ButtonGlow.localPosition = new Vector2(-350f, 0f);

	}
	void startAnim()
	{


		float AnimTime = 1f;
		Sequence mySeq = DOTween.Sequence();
		mySeq.Insert(0f, BlueBG.transform.DOScale(1f, AnimTime)).SetEase(Ease.OutBack);
		mySeq.Insert(0f, BlueBG.DOFade(1f, AnimTime));

		mySeq.Insert(AnimTime / 2f, TitleText.DOFade(1f, 0));
		mySeq.Insert(AnimTime / 2f, TitleText.transform.DOScale(1f, AnimTime/3f));

		mySeq.Insert(AnimTime / 2f, playButton.transform.DOScale(1f, AnimTime/2f));
		mySeq.Insert(AnimTime / 2f, closeButton.transform.DOScale(1f, AnimTime/2f));

		mySeq.SetUpdate(true);

		glowSeq = DOTween.Sequence();
		glowSeq.Insert(0.5f, ButtonGlow.DOLocalMove(new Vector3(350f, 0f, 0f), 3f)).SetEase(Ease.Linear);
		glowSeq.SetLoops(-1, LoopType.Restart);
		glowSeq.SetUpdate(true);

	}
    #endregion
}
