using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System .Linq ;
using System;
using DG.Tweening;

public class CTLevelSelectionPage : Singleton<CTLevelSelectionPage> 
{
	[SerializeField] private int MAIN_LEVEL_IDX;
	//private string [] NamesToDisplayInGoogle = new string[] { "Rookie", "SemiPro", "Professional", "Veteran", "Champion", "Legend" };
	private Camera mainCamera;
	private int watchVideoClickedLevel = -1;

	[Header("NEW UI")]
	public Image[] BGimage;
	public Image[] RightSideImage;
	public GameObject[] RightSideBat;
	public Text[] TargetRangeText;
	public Image[] Stars_1;
	public Image[] Stars_2;
	public Image[] Stars_3;
	public Image[] Stars_4;
	public Image[] Stars_5;

	[Header("Animation")]
	public GameObject[] maskHolder;
	public Image[] TopShine;
	public Image[] BottomShine;

	protected void  Awake ()
	{
		mainCamera = GameObject.Find ("Main Camera").GetComponent<Camera>();
		mainCamera.enabled = false;
	}
	protected void  OnEnable ()
	{
		ValidateLockedLevels ();
	}
	private void ValidateLockedLevels()
	{
		PlayerPrefsManager.instance.GetChaseTargetLevelDetails();
		CONTROLLER.CTLevelId = CONTROLLER.CTCurrentPlayingMainLevel;
		CONTROLLER.CTLevelCompleted = CONTROLLER.CTCurrentPlayingMainLevel;
		SetOverRange();
		int i;

		if (CONTROLLER.MainLevelCompletedArray[CONTROLLER.CTLevelId] == 1)
		{
			for (int x = 0; x < 5; x++)
			{
				BGimage[x].sprite = CTMenuScreen.instance.PlayedBG;
				RightSideImage[x].sprite = CTMenuScreen.instance.CompletedButton;
				RightSideImage[x].SetNativeSize();

				RightSideBat[x].SetActive(false);
				maskHolder[x].SetActive(false);
			}
			SetStarsState(0, true);
			SetStarsState(1, true);
			SetStarsState(2, true);
			SetStarsState(3, true);
			SetStarsState(4, true);
		}
		else
		{
			string tmp = PlayerPrefs.GetString("SuperChaseSubLevCompData");
			CONTROLLER.SubLevelCompletedArray = tmp.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
			bool bflag = false, bflag2 = false;
			for (int j = 0; j < CONTROLLER.SubLevelCompletedArray.Length; j++)
			{
				if (CONTROLLER.SubLevelCompletedArray[j] == 0)
				{
					//locked
					BGimage[j].sprite = CTMenuScreen.instance.NotYetPlayedBG;
					RightSideImage[j].sprite = CTMenuScreen.instance.LockButtonSubLevel;
					RightSideImage[j].SetNativeSize();
					SetStarsState(j, false);

					RightSideBat[j].SetActive(false);
					maskHolder[j].SetActive(false);

					if (!bflag)
					{
						bflag = true; bflag2 = true;
						BGimage[j].sprite = CTMenuScreen.instance.CurrentBG;
						RightSideImage[j].sprite = CTMenuScreen.instance.playButtonBGOnly;
						RightSideImage[j].SetNativeSize();

						RightSideBat[j].SetActive(true);
						maskHolder[j].SetActive(true);
					}
					//else if (bflag2 && AdIntegrate.instance.checkTheInternet() && AdIntegrate.instance.isRewardedReadyToPlay())
					//{
					//	BGimage[j].sprite = CTMenuScreen.instance.NotYetPlayedBG;
					//	RightSideImage[j].sprite = CTMenuScreen.instance.watchVideo;
					//	RightSideImage[j].SetNativeSize();
					//	bflag2 = false;

					//	RightSideBat[j].SetActive(false);
					//	maskHolder[j].SetActive(false);
					//}

				}
				else
				{
					BGimage[j].sprite = CTMenuScreen.instance.PlayedBG;
					RightSideImage[j].sprite = CTMenuScreen.instance.CompletedButton;
					RightSideImage[j].SetNativeSize();
					SetStarsState(j, true);

					RightSideBat[j].SetActive(false);
					maskHolder[j].SetActive(false);
				}
			}
		}

		if (UIAnimation.instance != null)
		{
			UIAnimation.instance.ShowBackBtn();
		}

		DoAnimation();

	}

	void DoAnimation()
	{
		//resetAnimation();
		for (int idx = 0; idx < 5; idx++)
		{
			if (BGimage[idx].sprite == CTMenuScreen.instance.CurrentBG)
			{
				maskHolder[idx].SetActive(true);
				TopShine[idx].transform.DOLocalMoveX(-400f, 0f);
				BottomShine[idx].transform.DOLocalMoveX(400f, 0f);
				RightSideImage[idx].transform.DOScale(0.75f, 0f);
				RightSideBat[idx].transform.DOLocalRotate(Vector3.zero, 0f);

				Sequence shineSeq = DOTween.Sequence();
				shineSeq.Insert(0f, TopShine[idx].transform.DOLocalMoveX(400f, 2f));
				shineSeq.Insert(0f, BottomShine[idx].transform.DOLocalMoveX(-400f, 2f));
				shineSeq.SetEase(Ease.Linear);
				shineSeq.SetLoops(-1, LoopType.Restart).SetUpdate(true);
				shineSeq.AppendInterval(1f);

				Sequence rightSideAnim = DOTween.Sequence();
				rightSideAnim.Insert(0f, RightSideImage[idx].transform.DOScale(1f, 2f));
				rightSideAnim.AppendInterval(1f);
				rightSideAnim.SetEase(Ease.Linear);
				rightSideAnim.SetLoops(-1, LoopType.Yoyo).SetUpdate(true);

				//Sequence batSeq = DOTween.Sequence();
				//batSeq.Insert(0f, RightSideBat[idx].transform.DORotate(new Vector3(0f, 0f, -359.99f), 4f, RotateMode.FastBeyond360));
				//batSeq.SetEase(Ease.Linear);
				//batSeq.SetLoops(-1, LoopType.Incremental).SetUpdate(true);

				break;
			}
		}
	}

	private void SetStarsState(int idx, bool completed)
	{
		for (int i = 0; i < 3; i++)
		{
			if (completed)
			{
				if(idx==0)
					Stars_1[i].sprite = CTMenuScreen.instance.Star;
				else if(idx ==1)
					Stars_2[i].sprite = CTMenuScreen.instance.Star;
				else if(idx ==2)
					Stars_3[i].sprite = CTMenuScreen.instance.Star;
				else if(idx ==3)
					Stars_4[i].sprite = CTMenuScreen.instance.Star;
				else if(idx ==4)
					Stars_5[i].sprite = CTMenuScreen.instance.Star;
			}
			else
			{
				if (idx == 0)
					Stars_1[i].sprite = CTMenuScreen.instance.StarOutline;
				else if (idx == 1)
					Stars_2[i].sprite = CTMenuScreen.instance.StarOutline;
				else if (idx == 2)
					Stars_3[i].sprite = CTMenuScreen.instance.StarOutline;
				else if (idx == 3)
					Stars_4[i].sprite = CTMenuScreen.instance.StarOutline;
				else if (idx == 4)
					Stars_5[i].sprite = CTMenuScreen.instance.StarOutline;
			}
		}
	}

	public void LevelSelected(int index)
	{
		AudioPlayer.instance.PlayButtonSnd();
		CONTROLLER.CTLevelId = MAIN_LEVEL_IDX;
		CONTROLLER.CTLevelCompleted = MAIN_LEVEL_IDX;
		CONTROLLER.CTCurrentPlayingMainLevel = MAIN_LEVEL_IDX;

		if (RightSideImage[index].sprite.name== CTMenuScreen.instance.watchVideo.name)	// watchVideoBtn[index].gameObject.activeSelf)
		{
			watchVideoClickedLevel = index;
			watchVidoButtonClickedEvent();
		}
		else if (RightSideImage[index].sprite.name != CTMenuScreen.instance.LockButtonSubLevel.name)
		{

			AudioPlayer.instance.PlayButtonSnd();
			CONTROLLER.CTSubLevelId = index;
			CONTROLLER.CTSubLevelCompleted = index;
			CONTROLLER.totalOvers = CONTROLLER.Overs[CONTROLLER.CTLevelId];
			CONTROLLER.GetRandomScoreForOppTeam(CONTROLLER.StartRangeArray[CONTROLLER.CTSubLevelId], CONTROLLER.EndRangeArray[CONTROLLER.CTSubLevelId]);
			CONTROLLER.InningsCompleted = false;
			CONTROLLER.canShowMainCamera = true;
			mainCamera.enabled = true;
			HideThis();
			UIAnimation.instance.HideMe();
			GameModel.instance.NewInnings();
			//CricMini-Gopi
			BattingScoreCard.instance.StartGame();// ShowMe();
			BattingScoreCard.instance.FirstTimeContinueClicked = true;  //false
			GroundController.instance.ChangePlayerLeftRightTextures();

			//Analytics
			//string str = "SuperChase_" + NamesToDisplayInGoogle[CONTROLLER.CTLevelId] + "_" + (CONTROLLER.CTSubLevelId + 1) + "_Entered";
			//GameModel.instance.PostSuperChaseGoogleAnalyticsEvents(str);

			PlayerPrefs.DeleteKey("CTRVlevID");


		}
	}
	private void SetOverRange()
	{
		int i;
		string overRangeStr = CONTROLLER.TargetRangeArray[CONTROLLER.CTCurrentPlayingMainLevel];
		string[] overRangeArray = overRangeStr.Split("$"[0]);
		string[] StartRangeStr;
		string[] EndRangeStr;
		for (i = 0; i < TargetRangeText.Length; i++)
		{
			TargetRangeText[i].text = overRangeArray[i]+" RUNS";
			StartRangeStr = overRangeArray[i].Split("-"[0]);
			EndRangeStr = overRangeArray[i].Split("-"[0]);
			CONTROLLER.StartRangeArray[i] = int.Parse(StartRangeStr[0] as string);
			CONTROLLER.EndRangeArray[i] = int.Parse(EndRangeStr[1] as string);
		}
	}

	public void ClearLevel() 
	{
		CONTROLLER.CTLevelId = CONTROLLER.CTCurrentPlayingMainLevel;
		CONTROLLER.CTLevelCompleted = CONTROLLER.CTCurrentPlayingMainLevel;
		if (CONTROLLER.MainLevelCompletedArray [CONTROLLER.CTLevelId] == 0 && CONTROLLER.CTLevelId < 5) 
		{
			CONTROLLER.CTSubLevelCompleted++;
			CONTROLLER.CTLevelId++;
			if(CONTROLLER.CTLevelId == 4) 
			{
				CONTROLLER.MainLevelCompletedArray [CONTROLLER.CTLevelId] = 1;
				CTMenuScreen.instance.ValidateLockedLevels ();
			}
			ValidateLockedLevels ();
		}

	}

	public void  HideThis ()
	{
		CTMenuScreen.instance.HideThis();
	}

	public void watchVidoButtonClickedEvent()
	{
		if (AdIntegrate.instance != null)
		{
			if (AdIntegrate.instance.checkTheInternet())
			{
				Popup.instance.showGenericPopup("", "No video Available");
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
			CONTROLLER.CTSubLevelId = watchVideoClickedLevel;
			CONTROLLER.CTSubLevelCompleted = watchVideoClickedLevel;
			CONTROLLER.totalOvers = CONTROLLER.Overs [CONTROLLER.CTLevelId];
			CONTROLLER.GetRandomScoreForOppTeam (CONTROLLER.StartRangeArray [CONTROLLER.CTSubLevelId], CONTROLLER.EndRangeArray [CONTROLLER.CTSubLevelId]);
			CONTROLLER.InningsCompleted = false;
			CONTROLLER.canShowMainCamera = true;
			mainCamera.enabled = true;
			HideThis ();
			//LoadPlayerPrefs.instance.SetArrayForChaseTarget ();		//original 
			UIAnimation.instance.HideMe ();
			BattingScoreCard.instance.ShowMe ();
			GameModel.instance.NewInnings ();
			BattingScoreCard.instance.FirstTimeContinueClicked = true;	//false
			GroundController.instance.ChangePlayerLeftRightTextures ();

			//Analytics
			//string str = "SuperChase_" + NamesToDisplayInGoogle [CONTROLLER.CTLevelId] + "_" + (CONTROLLER.CTSubLevelId + 1) + "_Entered";
			//GameModel.instance.PostSuperChaseGoogleAnalyticsEvents (str);

			PlayerPrefs.SetInt ("CTRVlevID", watchVideoClickedLevel);

			watchVideoClickedLevel = -1;
		}
	}
}
