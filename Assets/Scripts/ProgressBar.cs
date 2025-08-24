using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG .Tweening ;

public class ProgressBar : Singleton<ProgressBar> 
{

	public Text Title,ButtonText;
	public bool showProgress;
	public GameObject holder;
	public GameObject panel;
	public Image LoadingBar;
	private float currentAmount;
	private float FillerStartTime;
	public Text CountDown;
	private float countInt;
	private float TotalCount = 5;
	/*void Update () 
	{
		if (showProgress) 
		{			
			if (currentAmount <=100) 
			{
				currentAmount = Linear (Time.realtimeSinceStartup - FillerStartTime, 0, 100, 5f);       //currentAmount +=speed * Time.unscaledDeltaTime;	
				CountDown.text = ""+ Mathf.CeilToInt(5 - (Time.realtimeSinceStartup - FillerStartTime));
			} 
			else
			{
				closeButEvent ();
			}
			LoadingBar.fillAmount = currentAmount / 100;
		} 
		else 
		{
			currentAmount = 0;
			holder.SetActive (false);
		}
	}


	float Linear(float t, float source, float destination, float duration)
	{
		return (destination  - source ) * t / duration  + source ;
	}
*/

	public void  closeButEvent()
	{
		showProgress = false;
		currentAmount = 0;
		holder.SetActive (false);

		if (glowSeq != null)
			glowSeq.Kill();

		if (CONTROLLER.selectedGameMode == GameMode.SuperOver)
			SO_Cancel_ButtonEvent ();
		else
		{
			GamePauseScreen.instance.playerDetails.SetActive (true);	
			BattingScoreCard.instance.DisplayNextPlayer ();	
			//GamePauseScreen.instance.SetLoftState(true);
		}
			
	}

	public void setProgress() 
	{
		if (glowSeq != null)
			glowSeq.Kill();

		showProgress = true;
		holder.SetActive (true);
		panel.SetActive(true);
		currentAmount = 0;
		FillerStartTime = Time.realtimeSinceStartup ;
		GamePauseScreen.instance.playerDetails.SetActive (false);
		GamePauseScreen.instance.SetLoftState(false); 
		if (CONTROLLER.selectedGameMode == GameMode.SuperOver) 
		{
			Title.text = "You've lost 2 wickets\nin this over";
			ButtonText .text ="FACE THE BALL AGAIN";
			//ButtonText.fontSize = 30;
		} 
		else 
		{
			Title.text = "Face that ball again!";
			ButtonText .text ="WATCH VIDEO";
			//ButtonText.fontSize = 30;

		}
		//StartCountDown();
		 startAnim();
	}

	public Image BlueBG;
	public Transform ButtonGlow;
	Sequence glowSeq;
	public void startAnim()
	{
		//reset
		BlueBG.transform.DOScale(0f, 0);
		BlueBG.DOFade(0f, 0);
		if (glowSeq != null)
			glowSeq.Kill();
		ButtonGlow.localPosition = new Vector2(-350f, 0f);

		float AnimTime = 0.7f;
		Sequence mySeq = DOTween.Sequence();
		mySeq.Insert(0f, BlueBG.transform.DOScale(1f, AnimTime)).SetEase(Ease.OutBack);
		mySeq.Insert(0f, BlueBG.DOFade(1f, AnimTime-0.1f));
		mySeq.SetUpdate(true);

		glowSeq = DOTween.Sequence();
		glowSeq.Insert(0.5f, ButtonGlow.DOLocalMove(new Vector3(300f, 0f, 0f), 3f)).SetEase(Ease.Linear);
		glowSeq.SetLoops(-1,LoopType.Restart);
		glowSeq.SetUpdate(true);


	}


	public void StartCountDown()
	{
		countInt = 0;
		LoadingBar.fillAmount = 1f;

		CountDown.text = TotalCount.ToString();
		StartCoroutine(IncreaseCountText());
	}

	IEnumerator IncreaseCountText()
	{
		if (holder.activeSelf)
		{
			yield return new WaitForSecondsRealtime(1f);
			countInt++;
			float tempCount = TotalCount - countInt;

			float barTempval = tempCount / TotalCount;
			LoadingBar.fillAmount = barTempval;

			CountDown.text = tempCount.ToString();

			if (tempCount < 0)
			{
				closeButEvent();
				tempCount = 0;
			}
			else
			{
				StartCoroutine(IncreaseCountText());
			}
		}
	}

	//0037074
	/*void  OnApplicationPause (bool  focusStatus)
	{
		if (!focusStatus  && ManageScene.CurScene ==Scenes.Ground && CONTROLLER.CurrentPage == "ingame" )
		{
			if(ProgressBar.instance.holder.activeSelf  || showProgress )
			{	
				resetProgress  (); 
			}						
		}
	}*/

	void OnApplicationQuit()
	{
		if(holder .activeSelf )
		{
			closeButEvent (); 
		}
	}

	void resetProgress()
	{
		currentAmount = 0;
		FillerStartTime = Time.realtimeSinceStartup; 
	}

	public void watchVidoButtonClickedEvent()
	{
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

	public  void SO_Cancel_ButtonEvent()
	{
		if (CONTROLLER.CurrentLevelCompleted <= CONTROLLER.LevelId)
		{
			CONTROLLER.LevelFailed = 1;
		}
		GameModel.instance.SaveSoFailedLevelDetails ();
		GameModel .instance .SaveSOLevelDetails ();
		int  swapStriker = CONTROLLER.StrikerIndex;
		CONTROLLER.StrikerIndex = CONTROLLER.NonStrikerIndex;
		CONTROLLER.NonStrikerIndex = swapStriker;
		if (PlayerPrefs.HasKey ("SuperOverDetail") && CONTROLLER.selectedGameMode == GameMode.SuperOver)
		{
			PlayerPrefs.DeleteKey ("SuperOverDetail");
			PlayerPrefs.DeleteKey ("superoverPlayerDetails");
		}
		GameModel .instance .ResetVariables ();
		GameModel .instance .NewOver ();
		GroundController.instance.action = 10;
		GameModel .instance .ShowSuperOverResult ();
		GameModel .instance .ResetCurrentMatchDetails ();
		close (); 
	}

	public  void close()
	{
		//Destroy (this .gameObject); 
		holder .SetActive (false ); 
	}


	public  void Success()
	{
		close (); 
		if(BattingScoreCard .instance!=null )
		{
			BattingScoreCard.instance.RegainWicket ();
		}
	}


}
