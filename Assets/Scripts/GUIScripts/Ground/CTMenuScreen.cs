using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System .Linq ;
using System ;
using DG.Tweening;
using UnityEngine.UIElements;

public class CTMenuScreen : Singleton<CTMenuScreen> 
{
	private Camera mainCamera  ;

	[Header("NEW UI")]
	public CTChallenges[] challenges;
	public Sprite LockButtonwithOutline;
	public Sprite LockButtonSubLevel;
	public Sprite playButton,failedButton, CompletedButton, watchVideo,playButtonBGOnly;
	public Sprite Star, StarOutline;
	public Sprite PlayedBG, CurrentBG, NotYetPlayedBG;

	public CTLevelSelectionPage[] SubChallenges;

	protected void  Awake ()
	{
		CONTROLLER.CurrentPage = "levelselection";
		CONTROLLER.canShowMainCamera = false;
		mainCamera = GameObject.Find ("Main Camera").GetComponent<Camera>();
	}

	protected void  Start ()
	{
		ValidateLockedLevels ();
		mainCamera.enabled = false;
	}
	public void HomeButtonClick()
	{
		AudioPlayer.instance.PlayButtonSnd();
		UIAnimation.instance.GoToMainMenu();
		CONTROLLER.CurrentPage = "splashpage";
	}
	public void  ValidateLockedLevels ()
	{
		PlayerPrefsManager.instance.GetChaseTargetLevelDetails ();
		int  i ;
		for ( i=0;i<challenges.Length;i++)
		{
			challenges[i].RightSideInfo.enabled = true;
			challenges[i].RightSideInfo.sprite = LockButtonwithOutline;
			challenges[i].RightSideInfo.SetNativeSize();
		}
		for(i=0;i<=CONTROLLER.CTLevelCompleted;i++)
		{
			challenges[i].RightSideInfo.enabled= false;
		}
		if (CONTROLLER.CTLevelCompleted <= 5)
		{
			challenges[CONTROLLER.CTLevelCompleted].RightSideInfo.sprite = playButton;
			challenges[CONTROLLER.CTLevelCompleted].RightSideInfo.enabled = true;
			challenges[CONTROLLER.CTLevelCompleted].RightSideInfo.SetNativeSize();
			
			//to open the sub levels of current playing level at start
			LevelSelected(CONTROLLER.CTLevelCompleted);
		}

		if (UIAnimation.instance != null)
		{
			UIAnimation.instance.HideBackBtn ();
		}

		startAnimaiton();
    }
	[Space]
	[Header("ANIMATION COMPONENTS")]
	public Transform ContentTransform;
	void startAnimaiton()
	{
		ContentTransform.DOLocalMove(new Vector3(0f, 1500f, 0), 0f).SetUpdate(true);
        float AnimTime = 2f;
        Sequence mySeq = DOTween.Sequence();
		float topos =  (120+10) * CONTROLLER.CTLevelCompleted;
		if(topos>=480)topos=480;
        mySeq.Insert(0f, ContentTransform.DOLocalMove(new Vector3(0f, topos, 0), AnimTime)).SetEase(Ease.Linear);
        mySeq.SetUpdate(true);
    }

    public void  LevelSelected (int index)
	{
		AudioPlayer.instance.PlayButtonSnd();
		if (challenges [index].RightSideInfo.sprite.name != LockButtonwithOutline.name || !challenges[index].RightSideInfo.enabled) 
		{
			CONTROLLER.CTLevelId = index;
			CONTROLLER.CTLevelCompleted = index;
			CONTROLLER.CTCurrentPlayingMainLevel = index;

			if(!SubChallenges[index].gameObject.activeInHierarchy)
			{
				SubChallenges[index].gameObject.SetActive(true);
				challenges[index].RightSideInfo.enabled = false;
			}
			else
			{
				SubChallenges[index].gameObject.SetActive(false);

				if (CONTROLLER.MainLevelCompletedArray[CONTROLLER.CTLevelId] != 1)
				{
					challenges[index].RightSideInfo.enabled = true;
				}

				//chase target mode completed
				if (CONTROLLER.CTLevelCompleted == 5 && PlayerPrefs.GetString("SuperChaseSubLevCompData") == "1-1-1-1-1")
					challenges[index].RightSideInfo.enabled = false;
			}
		}
	}

	public void  HideThis ()
	{
		Destroy(this.gameObject);
		Resources.UnloadUnusedAssets ();
	}
}
