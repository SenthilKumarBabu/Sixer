using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System .Linq ;
using System ;
using DG.Tweening;


public class SOLevelSelectionPage : Singleton<SOLevelSelectionPage>
{
	public ChallengeManager[] Challenges;
	public Sprite playButton, lockButton,failedButton,CompletedButton,watchVideo;
	public Sprite PlayedBG, CurrentBG, NotYetPlayedBG;
	public Sprite ChallengeNumBG_Play, ChallengeNumBG_others;
	private Camera mainCamera  ;

	private int watchVideoClickedLevel = -1;

    private string[] LevelDescriptionArray = new string[] 
	{
        "hit 10 runs",
        "hit 3 fours",
        "hit 3 consecutive fours",
        "hit 3 sixes",
        "hit 5 fours",
        "hit 25 runs",
        "hit 4 consecutive sixes",
        "hit 6 fours",
        "hit 6 sixes",
        "defend 27 runs",
        "defend 24 runs",
        "defend 21 runs",
        "defend 18 runs",
        "defend 15 runs",
        "defend 12 runs",
        "defend 9 runs",
        "defend 6 runs",
        "defend 3 runs"
    };

    protected void  Awake ()
	{
		CONTROLLER.CurrentPage = "levelselection";
		CONTROLLER.canShowMainCamera = false;
		mainCamera = GameObject.Find ("Main Camera").GetComponent<Camera>();
	}
	public void HomeButtonClick()
	{
		AudioPlayer.instance.PlayButtonSnd();
		UIAnimation.instance.GoToMainMenu();
		CONTROLLER.CurrentPage = "splashpage";
	}
	protected void Start ()
	{
		ValidateLockedLevels ();
		mainCamera.enabled = false;
	}


	private void  ValidateLockedLevels ()
	{
		int  i ;
        int bowlerType =0;

        for (i=0; i < CONTROLLER.totalLevels; i++)
		{
            bowlerType = i % 2;
			Challenges[i].ChallengeNumber.text=(i + 1).ToString();
			Challenges[i].LeftChallengeBG.sprite=ChallengeNumBG_others;
			Challenges[i].LevelHeading.text = LevelDescriptionArray[(int)Mathf.Floor(i / 2)].ToString().ToUpper();
			if (bowlerType == 0)
			{
				Challenges[i].BowlingType.text = "FAST BOWLING";
			}
			else
			{
				Challenges[i].BowlingType.text = "SPIN BOWLING";
			}

			Challenges[i].RightSideBat.SetActive(false);
			Challenges[i].maskHolder.SetActive(false);

			if (i <= CONTROLLER.CurrentLevelCompleted)
			{
				Challenges[i].RightSideImage.sprite = playButton;
				Challenges[i].RightSideBat.SetActive(true);
				Challenges[i].RightSideImage.SetNativeSize();
				Challenges[i].BGimage.sprite = CurrentBG;
				Challenges[i].LeftChallengeBG.sprite = ChallengeNumBG_Play;

			}
			else
			{
				Challenges[i].RightSideImage.sprite = lockButton;
				Challenges[i].RightSideBat.SetActive(false);
				Challenges[i].RightSideImage.SetNativeSize();
				Challenges[i].BGimage.sprite = NotYetPlayedBG;
				Challenges[i].LeftChallengeBG.sprite = ChallengeNumBG_others;
			}
		}

		if(PlayerPrefs .HasKey ("SuperOverCompletedLevel"))
		{
			string tmp = PlayerPrefs.GetString ("SuperOverCompletedLevel");
			int[] array = tmp.Split('-').Select(n => Convert.ToInt32(n)).ToArray();

			//level failed details array

			tmp = PlayerPrefs.GetString ("SoLevFailedDet");
			int[] failArray = tmp.Split('-').Select(n => Convert.ToInt32(n)).ToArray();

			for (int j=0;j<array .GetLength (0);j++)
			{
				if(array [j]==0)
				{
					//Challenges [j].pass.SetActive (false );
					//Challenges [j].playOrLock.SetActive (true);
					//to be added

					//
					if(failArray [j]==1)
					{
						//Challenges [j].fail.SetActive (true);

						Challenges[j].RightSideImage.sprite = failedButton;
						Challenges[j].RightSideBat.SetActive(false);
						Challenges[j].RightSideImage.SetNativeSize();
						Challenges[j].LeftChallengeBG.sprite = ChallengeNumBG_Play;
						Challenges[j].BGimage.sprite = CurrentBG;
					}
				}
				else
				{
					//Challenges [j].pass.SetActive (true);
     //               Challenges [j].playOrLock.SetActive (false);

					Challenges[j].RightSideImage.sprite = CompletedButton;
					Challenges[j].RightSideBat.SetActive(false);
					Challenges[j].RightSideImage.SetNativeSize();
					Challenges[j].LeftChallengeBG.sprite = ChallengeNumBG_others;
					Challenges[j].BGimage.sprite = PlayedBG;
				}
			}
		}
		else 
		{
			for(i=0; i<CONTROLLER.CurrentLevelCompleted; i++)
			{
				Challenges [i].RightSideImage.sprite= CompletedButton;
				Challenges[i].RightSideBat.SetActive(false);
				Challenges[i].RightSideImage.SetNativeSize();
				Challenges[i].LeftChallengeBG.sprite= ChallengeNumBG_others;
				Challenges [i].BGimage.sprite= PlayedBG;
			}
		}

		i = CONTROLLER.CurrentLevelCompleted;

		//for video button
		if((CONTROLLER .CurrentLevelCompleted+1) <18 && AdIntegrate.instance.checkTheInternet() && AdIntegrate .instance .isRewardedReadyToPlay ())	
		{
			Challenges[CONTROLLER .CurrentLevelCompleted+1].RightSideImage.sprite = watchVideo;
			Challenges[CONTROLLER.CurrentLevelCompleted + 1].RightSideBat.SetActive(false);
			Challenges[CONTROLLER.CurrentLevelCompleted + 1].RightSideImage.SetNativeSize();
		}


		/*if (CONTROLLER.LevelId < 17) 
		{
			if (i<18 && Challenges [i].playOrLock.activeSelf && Challenges [i].playOrLock.transform.GetComponent<Image> ().sprite == playButton && i > 2) 
			{
				if (i > 15)
					i = 15;
				SnapTo (Challenges [i].GetComponent<RectTransform> ());
			}
		}*/
		if (CONTROLLER.CurrentLevelCompleted >= CONTROLLER.LevelId && CONTROLLER.LevelFailed == 1 && CONTROLLER.LevelCompletedArray[CONTROLLER.CurrentLevelCompleted] == 0)
		{
			Challenges[i].BGimage.sprite = CurrentBG;
			Challenges[i].RightSideImage.sprite = failedButton;
			Challenges[i].RightSideBat.SetActive(false);
			Challenges[i].RightSideImage.SetNativeSize();
			Challenges[i].LeftChallengeBG.sprite = ChallengeNumBG_Play;
		}
		if (UIAnimation.instance != null)
		{
			UIAnimation.instance.HideBackBtn ();
		}

        startAnimaiton();
        DoAnimation();
	}

    [Space]
    [Header("ANIMATION COMPONENTS")]
    public Transform ContentTransform;
    void startAnimaiton()
    {
		ContentTransform.DOLocalMove(new Vector3(0f, 2500f, 0), 0f).SetUpdate(true);
        float AnimTime = 2f;
        Sequence mySeq = DOTween.Sequence();
        float topos = (100) * CONTROLLER.CurrentLevelCompleted;
        if (topos <= 200) topos = 0;
        if (topos >= 1340) topos = 1340;
		mySeq.Insert(0f, ContentTransform.DOLocalMove(new Vector3(0f, topos, 0), AnimTime)).SetEase(Ease.Linear);
		mySeq.SetUpdate(true);
	}
    //Sequence shineSeq;
    //Sequence rightSideAnim;
    //void resetAnimation()
    //{
    //	if (shineSeq != null)
    //		shineSeq.Kill();
    //	if (rightSideAnim != null)
    //		rightSideAnim.Kill();
    //}

    void DoAnimation()
	{
		//resetAnimation();
		for (int idx = 0; idx < CONTROLLER.totalLevels; idx++)
		{
			if (Challenges[idx].BGimage.sprite == CurrentBG)
			{
				Challenges[idx].maskHolder.SetActive(true);
				Challenges[idx].TopShine.transform.DOLocalMoveX(-400f, 0f);
				Challenges[idx].BottomShine.transform.DOLocalMoveX(400f, 0f);
				Challenges[idx].RightSideImage.transform.DOScale(0.75f, 0f);
				Challenges[idx].RightSideBat.transform.DOLocalRotate(Vector3.zero, 0f);

				Sequence shineSeq = DOTween.Sequence();
				shineSeq.Insert(0f, Challenges[idx].TopShine.transform.DOLocalMoveX(400f, 2f));
				shineSeq.Insert(0f, Challenges[idx].BottomShine.transform.DOLocalMoveX(-400f, 2f));
				shineSeq.SetEase(Ease.Linear);
				shineSeq.SetLoops(-1, LoopType.Restart).SetUpdate(true);
				shineSeq.AppendInterval(1f);

				Sequence leftseq = DOTween.Sequence();
				leftseq.Insert(0f, Challenges[idx].LeftChallengeBG.transform.DOScale(1.1f, 0.5f));
				leftseq.SetLoops(-1, LoopType.Yoyo).SetUpdate(true);

				Sequence rightSideAnim = DOTween.Sequence();
				rightSideAnim.Insert(0f, Challenges[idx].RightSideImage.transform.DOScale(1f, 2f));
				rightSideAnim.AppendInterval(1f);
				rightSideAnim.SetEase(Ease.Linear);
				rightSideAnim.SetLoops(-1, LoopType.Yoyo).SetUpdate(true);

				//Sequence batSeq = DOTween.Sequence();
				//batSeq.Insert(0f, Challenges[idx].RightSideBat.transform.DORotate(new Vector3(0f, 0f, -359.99f), 4f, RotateMode.FastBeyond360));
				//batSeq.SetEase(Ease.Linear);
				//batSeq.SetLoops(-1, LoopType.Incremental).SetUpdate(true);

				break;
			}
		}
	}



	public void ClearLevel() 
	{
		if(CONTROLLER.LevelId < 17) {
		GameModel .instance.WriteSuperOverEachLevelCompData ();
			CONTROLLER.CurrentLevelCompleted++;
			CONTROLLER.LevelId++;
			ValidateLockedLevels ();
		}

	}



	public void SnapTo(RectTransform target)
	{
		Canvas.ForceUpdateCanvases();

		//content.GetComponent<RectTransform>().anchoredPosition =
		//	(Vector2)scrollView.GetComponent<ScrollRect>().transform.InverseTransformPoint(content.GetComponent<RectTransform>().position)
		//	- (Vector2)scrollView.GetComponent<ScrollRect>().transform.InverseTransformPoint(target.position);
	}

	public void  LevelSelected (int index)
	{
		AudioPlayer.instance.PlayButtonSnd();
		
		if (Challenges[index].RightSideImage.sprite.name == watchVideo.name)
		{
			watchVideoClickedLevel = index;
			watchVidoButtonClickedEvent();
		}
		else if (Challenges [index].RightSideImage.sprite.name!= lockButton.name) 
		{
			AudioPlayer.instance.PlayButtonSnd ();
			CONTROLLER.LevelId = index;
			CONTROLLER.NewInnings = true;
			CONTROLLER.InningsCompleted = false;
			CONTROLLER.canShowMainCamera = true;
			mainCamera.enabled = true;
			HideThis ();
			SetBowler ();//27march
			if (GameModel.instance.CanShowBattingScoreCard == false) 
			{
				GameModel.instance.CanShowBattingScoreCard = true;
				UIAnimation.instance.HideMe ();
				BattingScoreCard.instance.ShowMe ();
			} 
			else 
			{
				UIAnimation.instance.HideMenu ();
			}

			PlayerPrefs.SetInt("SOwicketGainUsed", 0);
			PlayerPrefs.DeleteKey ("SoRVlevID");
		}

	}

	private void  SetBowler ()
	{
		int mod = CONTROLLER.LevelId%2;
		if (mod == 0)
		{
			CONTROLLER.bowlerType = "fast";
		}
		else
		{
			CONTROLLER.bowlerType = "spin";
		}	
	}

	private void  HideThis ()
	{
		Destroy (this.gameObject);
		Resources.UnloadUnusedAssets ();
	}


	public void watchVidoButtonClickedEvent()
	{
		if (AdIntegrate.instance != null)
		{
			if (AdIntegrate.instance.checkTheInternet ())
			{
				if (AdIntegrate.instance.isRewardedReadyToPlay ())
				{
					CONTROLLER.RewardedVideoClickedState = 1;	// Superover Wicket loss
					AdIntegrate.instance.ShowRewardedVideo ();
				}					
				else
				{
					ShowToast("No video Available");
					StartCoroutine(AdIntegrate.instance.requestRewardedVideo ());
                }
            }
			else
			{
				Popup.instance.ShowNoInternetPopup();
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
	public void WatchVideoSuccessEvent()
	{
		if(watchVideoClickedLevel!=-1)
		{
			AudioPlayer.instance.PlayButtonSnd ();
			CONTROLLER.LevelId = watchVideoClickedLevel ;
			CONTROLLER.NewInnings = true;
			CONTROLLER.InningsCompleted = false;
			CONTROLLER.canShowMainCamera = true;
			mainCamera.enabled = true;
			HideThis ();
			SetBowler ();//27march
			if (GameModel.instance.CanShowBattingScoreCard == false)
			{
				GameModel.instance.CanShowBattingScoreCard = true;
				UIAnimation.instance.HideMe ();
				BattingScoreCard.instance.ShowMe ();
			}
			else 
			{
				UIAnimation.instance.HideMenu ();
			}

			PlayerPrefs.SetInt ("SoRVlevID", watchVideoClickedLevel);

			watchVideoClickedLevel = -1;

			PlayerPrefs.SetInt("SOwicketGainUsed", 0);
			 
		}
	}
}
