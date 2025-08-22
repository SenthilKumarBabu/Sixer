using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnimationScreen : Singleton<AnimationScreen> 
{
	public GameObject animObj ;
	private int animType ;
	[HideInInspector]
	public bool isLastBallWicket;
	public Sprite[] sprites;

	protected void  Awake ()
	{
		this.Hide (true);
	}

	public void StartAnimation(int type)
	{
		resetAnimation();

		animType = type;
		numberSprite.sprite = sprites[animType];
		AudioPlayer.instance.PlayGameSnd("runappear");
		Hide(false);
		startAnim();
	}

	public void  Hide (bool boolean)
	{
        if (boolean == true)
		{
			animObj.SetActive (false);
		}
		else
		{
			animObj.SetActive (true);
		}
	}

	public bool IsAnimationPlaying()
	{
		return animObj.activeSelf;
	}


	#region ANIMATION
	public GameObject leftSwipe;
	public GameObject RightSwipe;
	public Image numberSprite;
	private Vector3 startPos = new Vector3(-900f, -600f, 0);
	private Vector3 endPos = new Vector3(850f, 550f, 0);
	public void resetAnimation()
	{
		leftSwipe.transform.DOLocalMove(startPos, 0f).SetUpdate(true);
		RightSwipe.transform.DOLocalMove(endPos, 0f).SetUpdate(true);
		numberSprite.transform.DOScale(0, 0f).SetUpdate(true);
	}
	private void startAnim()
	{
        Sequence seq = DOTween.Sequence();

		seq.Insert(0f, leftSwipe.transform.DOLocalMove(endPos, 2f));
		seq.Insert(0.1f, RightSwipe.transform.DOLocalMove(startPos, 2f));
		seq.Insert(0f, numberSprite.transform.DOScale(1.25f,1.25f));
		seq.SetUpdate(true);
		seq.InsertCallback(1.4f, StopAnim);
	}
	private void StopAnim()
	{
		Hide(true);
		resetAnimation();

        if (animType != 6 && animType!=3 && animType !=4)
		{
			GameModel.instance.AnimationCompleted();
		}
		if (animType == 5 && CONTROLLER.selectedGameMode != GameMode.BattingMultiplayer && CONTROLLER.currentMatchWickets < 10)
		{
			if (CONTROLLER.gameMode == "superover")
			{
				if (CONTROLLER.currentMatchBalls != 0 && CONTROLLER.currentMatchWickets < 2)
				{
					AdIntegrate.instance.SetTimeScale(0f);
					BattingScoreCard.instance.DisplayNextPlayer();
				}
			}
			else if (CONTROLLER.gameMode != "superover" && !GameModel.instance.CheckForInningsComplete())
			{
				AdIntegrate.instance.SetTimeScale(0f);
				if ((CONTROLLER.currentBallNumber == 0 && (CONTROLLER.gameMode == "slogover" || CONTROLLER.gameMode == "chasetarget" || CONTROLLER.gameMode == CONTROLLER.SUPER_Crusade_GameMode)))
				{
					if ((CONTROLLER.currentMatchBalls + 1) < CONTROLLER.totalOvers * 6)
					{
						BattingScoreCard.instance.DisplayNextPlayer();
					}
				}
				else
				{
					//if (AdIntegrate.instance.checkTheInternet() && AdIntegrate.instance.isRewardedReadyToPlay())
					//{
					//	ProgressBar.instance.setProgress();
					//}
					//else
					//{
                        BattingScoreCard.instance.DisplayNextPlayer();
					//}
				}
			}
		}

	}
	#endregion
}
