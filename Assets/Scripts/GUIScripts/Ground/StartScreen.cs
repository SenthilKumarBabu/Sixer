using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : Singleton<StartScreen> 
{
	public GameObject Three,Two,One,Go;
	protected void  Awake ()
	{
		Three.SetActive(true);Two.SetActive(true);One.SetActive(true);Go.SetActive(true);
		Three.transform.localScale = Vector3.zero;
        Two.transform.localScale = Vector3.zero;
        One.transform.localScale = Vector3.zero;
        Go.transform.localScale = Vector3.zero;

        CONTROLLER.CurrentPage = "";
		if(GameModel.instance != null)
		{
			GameModel.instance.ShowShotTutorial (false);
			GameModel.instance.ShowBatsmanMoveTutorial (false);//shankar 09April
		}
	}

	protected void  Start ()
	{
		StartCoroutine (StartAnimation ());
		GameModel.instance.bChaseExtraBall = true ;
	}

	private IEnumerator StartAnimation ()
	{
		yield return new WaitForSecondsRealtime(0f);
        if (AudioPlayer.instance != null && ManageScene.CurScene == Scenes.Ground)
        {
            AudioPlayer.instance.PlayGameSnd("coutdown");
        }
        Sequence sq = DOTween.Sequence();
        sq.SetUpdate(true);
        sq.Insert(0f, Three.transform.DOScale(1f, 1f).SetEase(Ease.OutBack));
        sq.AppendCallback(() =>
        {
			Three.GetComponent<Image>().DOFade(0, 0.5f);
        });

        sq.Insert(1f, Two.transform.DOScale(1f, 1f).SetEase(Ease.OutBack));
        sq.AppendCallback(() =>
        {
            Two.GetComponent<Image>().DOFade(0, 0.5f);
        });

        sq.Insert(2f, One.transform.DOScale(1f, 1f).SetEase(Ease.OutBack));
        sq.AppendCallback(() =>
        {
            One.GetComponent<Image>().DOFade(0, 0.5f);
        });

        sq.Insert(3f, Go.transform.DOScale(1f, 1f).SetEase(Ease.OutBack));
        sq.AppendCallback(() =>
        {
            Go.GetComponent<Image>().DOFade(0, 0.5f);
            StopAnimation();
        });		
	}

	public void StopAnimation()
	{
		if (CONTROLLER.gameMode == "multiplayer")
		{
			Multiplayer_Hide_this();
		}
		else
		{
			GameModel.instance.ResetPauseVar();
			GroundController.instance.StartToBowl();
			HideThis();
		}
	}

	public void  HideThis ()
	{	
		GamePauseScreen.instance.playerDetails.SetActive (true);			
		//CONTROLLER.NewInnings = false;
		Scoreboard.instance.Hide (false);//25march
		BatsmanInfo.instance.ShowMe ();
		BatsmanInfo.instance.UpdateStrikerInfo ();
		Scoreboard.instance.HidePause (false);//25march
		Scoreboard.instance.UpdateScoreCard();
		PreviewScreen.instance.Hide (false);//25march
		Destroy(this.gameObject);
		Resources.UnloadUnusedAssets ();
	}


	public void  Multiplayer_Hide_this ()
	{
		BatsmanInfo.instance.UpdateStrikerInfo ();//29march
		CONTROLLER.NewInnings = false;
		Scoreboard.instance.Hide (true);//25march
		BatsmanInfo.instance.ShowMe ();
		Scoreboard.instance.HidePause (false);//25march
		BattingScoreCard.instance .HideMe (); 
		Destroy(this.gameObject);
		Resources.UnloadUnusedAssets ();
        MultiplayerManager.Instance.ResetMyScorePushStatus();
        MultiplayerManager.Instance.AssignBowlingParameters();
       
		PreviewScreen.instance.Hide (false);
		if (MultiplayerManager.Instance.multiplayerGroundUiHandlerScript == null)
            MultiplayerManager.Instance.multiplayerGroundUiHandlerScript = GameObject.FindFirstObjectByType<MultiplayerGroundUIHandler>();

		MultiplayerManager.Instance.multiplayerGroundUiHandlerScript.resetScoreBoardData();


        GameModel.instance.BowlNextBall ();
	}
}
