using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine .UI ;
using DG.Tweening;
public class HeadStart : Singleton<HeadStart> 
{
	public GameObject holder;
	float [] percentageArray;
	public Text Content, Title;
	int extraRuns;
	
	protected void Awake()
	{
		HideMe();
		percentageArray = new float[] {0.1f,0.06f,0.04f};
		extraRuns = 0;
	}

	public void WatchVideo() 
	{
		AudioPlayer.instance.PlayButtonSnd();
		if (AdIntegrate.instance != null)
		{
			if (AdIntegrate.instance.checkTheInternet () )
			{
				if (AdIntegrate.instance.isRewardedReadyToPlay () )
				{
					if (glowSeq != null)
						glowSeq.Kill();
					CONTROLLER.RewardedVideoClickedState = 5;	// SuperChase Head Start ball
					AdIntegrate.instance.ShowRewardedVideo ();
				}					
				else
				{
					Popup.instance.showGenericPopup("","No video Available"); 
				}
			}
			else
			{
				Popup.instance.ShowNoInternetPopup();
			}

		}
	}

	private void TextAnim() 
	{
		string titleText="", desc="";

		if(GameModel .instance .nChaseHeadStrtCount==0)
		{
			extraRuns =Mathf .CeilToInt( CONTROLLER.TargetToChase*percentageArray[0] );
			titleText="Easy head start ?";
			//desc = "Watch a video to get a " + extraRuns.ToString () + " runs head start.";
			desc = "GET " + extraRuns.ToString () + " RUNS";
		}
		else if(GameModel .instance .nChaseHeadStrtCount==1)
		{
			extraRuns =Mathf .CeilToInt( CONTROLLER.TargetToChase*percentageArray[0] ) +  Mathf .CeilToInt( CONTROLLER.TargetToChase*percentageArray[1] );
			titleText="Great head start ?";
			desc = "GET " + extraRuns.ToString () + " RUNS";
		}
		else if(GameModel .instance .nChaseHeadStrtCount==2)
		{
			extraRuns =Mathf .CeilToInt( CONTROLLER.TargetToChase*percentageArray[0] ) +  Mathf .CeilToInt( CONTROLLER.TargetToChase*percentageArray[1] ) +  Mathf .CeilToInt( CONTROLLER.TargetToChase*percentageArray[2] );
			titleText="Huge head start ?";
			desc = "GET " + extraRuns.ToString () + " RUNS";
		}

		Title.text = titleText;
		Content.text = desc;
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
		GameModel .instance.nChaseHeadStrtCount++;
		CONTROLLER.currentMatchScores = extraRuns;
		Scoreboard.instance.TargetToWin ();
		GameModel.ScoreStr = CONTROLLER.currentMatchScores + "/" + CONTROLLER.currentMatchWickets;
		Scoreboard.instance.UpdateScoreCard (); 
		if(GameModel .instance.nChaseHeadStrtCount>=3 || !AdIntegrate .instance .isRewardedReadyToPlay () )
		{
			GameObject prefabGO;
			GameObject tempGO;
			prefabGO = Resources.Load ("Prefabs/StartScreen")as GameObject;
			tempGO = Instantiate (prefabGO);
			tempGO.name = "StartScreen";
			HideMe ();
		}
		else
		{
			ShowMe (); 
		}

	}
	public void ClosePopup() 
	{
		if (glowSeq != null)
			glowSeq.Kill();
		AudioPlayer.instance.PlayButtonSnd();
		GameObject prefabGO;
		GameObject tempGO;
		prefabGO = Resources.Load ("Prefabs/StartScreen")as GameObject;
		tempGO = Instantiate (prefabGO);
		tempGO.name = "StartScreen";
		HideMe ();
	}

	public void ShowMe() 
	{
		if (glowSeq != null)
			glowSeq.Kill();

		GamePauseScreen.instance.playerDetails.SetActive (false);
		GamePauseScreen.instance.SetLoftState(false);
		holder.SetActive (true);
		ReSetAnimation();
		if (BattingScoreCard.instance != null)
			BattingScoreCard.instance.setWicketCamera (true); 
		Title.text = "";
		Content.text = "";
		TextAnim ();
		startAnim();
	}

	public void HideMe() 
	{
		holder.SetActive (false);
		if (glowSeq != null)
			glowSeq.Kill();
		if (BattingScoreCard.instance != null)
			BattingScoreCard.instance.wicketCamera .SetActive(false );

		if(CameraMove.instance!=null)
			CameraMove.instance.reset();
			
	}

	#region ANIMATION
	public Image BlueBG;
	public Text TitleText;
	public Button playButton, closeButton;
	public Transform ButtonGlow;
	Sequence glowSeq;
	void ReSetAnimation()
	{
		BlueBG.transform.DOScale(0f, 0);
		BlueBG.DOFade(0f, 0);
		TitleText.transform.DOScale(0.5f, 0);
		TitleText.DOFade(0f, 0);
		playButton.transform.DOScale(0f, 0);
		closeButton.transform.DOScale(0f, 0);

		if (glowSeq != null)
			glowSeq.Kill();
		ButtonGlow.localPosition = new Vector2(-350f, 0f);  //DOLocalMove(new Vector3(-300f, 0f, 0f), 0f);
	}
	void startAnim()
	{
	
		float AnimTime = 1f;
		Sequence mySeq = DOTween.Sequence();
		mySeq.Insert(0f, BlueBG.transform.DOScale(1f, AnimTime)).SetEase(Ease.OutBack);
		mySeq.Insert(0f, BlueBG.DOFade(1f, AnimTime));


		mySeq.Insert(AnimTime / 2f, TitleText.DOFade(1f, 0));
		mySeq.Insert(AnimTime / 2f, TitleText.transform.DOScale(1f, AnimTime / 3f));

		mySeq.Insert(AnimTime / 2f, playButton.transform.DOScale(1f, AnimTime / 2f));
		mySeq.Insert(AnimTime / 2f, closeButton.transform.DOScale(1f, AnimTime / 2f));

		mySeq.SetUpdate(true);

		glowSeq = DOTween.Sequence();
		glowSeq.Insert(0.5f, ButtonGlow.DOLocalMove(new Vector3(350f, 0f, 0f), 3f)).SetEase(Ease.Linear);
		glowSeq.SetLoops(-1, LoopType.Restart);
		glowSeq.SetUpdate(true);

	}
	#endregion
}
