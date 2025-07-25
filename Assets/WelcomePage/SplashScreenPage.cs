﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class SplashScreenPage : MonoBehaviour 
{
	public GameObject welcomeScreen;
	public GameObject Tick;

	public GameObject Marshmallow;
    void Awake()
    {
        //Debug.unityLogger.logEnabled = false;
		CONTROLLER.DeviceID = SystemInfo.deviceUniqueIdentifier;

		Marshmallow.SetActive(false);
#if UNITY_ANDROID && !UNITY_EDITOR
		Marshmallow.SetActive(true);
#endif
		startLogoAnim();
		showTheWelcomeScreen();
	}

	#region ANIMATION
	[Header("ANIMATION")]
	public RectTransform logoRect;
	public RectTransform shineRect;
	void startLogoAnim()
	{
		shineRect.DOLocalMoveX(-300f, 0f);
		Sequence seq = DOTween.Sequence();
		seq.Insert(0f,logoRect.DOScale(1.2f,4f).SetEase(Ease.OutBack));
		seq.Insert(0f,shineRect.DOLocalMoveX(300f, 4f));
		seq.SetLoops(-1,LoopType.Yoyo);		
	}
	#endregion

	public void showTheWelcomeScreen()
	{
		Tick.SetActive(false);
		if (PlayerPrefs.HasKey ("Welcome_Screen_Closed") == false) 
		{
			Sequence sequence = DOTween.Sequence ();
			//welcomeScreen.transform.DOLocalMove (new Vector3 (0.0f, -Screen.height, 0.0f), 0.3f).From().SetEase (Ease.OutSine);
			welcomeScreen.SetActive (true);
		} 
		else 
		{
			closeTheWelcomeScreen ();
		}
	}
	public void Accept_OK_Event()
	{
		AudioPlayer.instance.PlayButtonSnd();
		if (!Tick.gameObject.activeSelf)
		{
			Popup.instance.showGenericPopup("", "Accept the Privacy Policy and \nthe Terms and Conditions");
		}
		else
		{
			//FirebaseAnalyticsManager.instance.logEvent("Accept_terms");
			closeTheWelcomeScreen();
		}
	}
	public void closeTheWelcomeScreen()
	{
		Sequence sequence = DOTween.Sequence();
		sequence.Append(welcomeScreen.transform.DOLocalMove(new Vector3(0.0f, -(Screen.height + 100f), 0.0f), 0.0f).SetEase(Ease.Linear));
		sequence.AppendInterval(1.0f);
		sequence.AppendCallback(() =>
	   {
		   welcomeScreen.SetActive(false);
		   PlayerPrefs.SetString("Welcome_Screen_Closed", "1");
		   PlayerPrefs.SetInt("teamlistchanges", 1);
		   PlayerPrefs.SetInt("teamlistchangesv11", 1);
		   LoadMenuScene();
	   });
	}

	public void LoadMenuScene()
	{
		StartCoroutine(LoadAfterDelay());
	}
	private IEnumerator LoadAfterDelay()
	{
		yield return new WaitUntil(() => CONTROLLER.serverConfig.isReady);

		if ((CONTROLLER.serverConfig.M != 1 && CONTROLLER.serverConfig.MV <= CONTROLLER.CURRENT_VERSION) || !AdIntegrate.instance.checkTheInternet())
		{
			yield return new WaitForSeconds(0.02f);
			StartCoroutine(LoadMainMenuFromSplash());
		}
	}


	public Slider splashSlider;
	public Text loadingText;
	AsyncOperation async = null;

	private IEnumerator LoadMainMenuFromSplash()
	{
		yield return new WaitForSecondsRealtime(2.0f);
		splashSlider.gameObject.SetActive(true);
		StartCoroutine(ShowProgress());
		async = ManageScene.LoadSceneAsync(Scenes.MainMenu.ToString());
	}

	IEnumerator ShowProgress()
	{
		yield return null;
		splashSlider.value = async.progress * 100;
		loadingText.text = "LOADING..." + (async.progress * 100).ToString("F0") + "%";
		StartCoroutine(ShowProgress());
	}


	public void privacyPolicy()
	{
		Application.OpenURL (CONTROLLER.PP_Link);
	}
	public void TermsAndCond()
	{
		Application.OpenURL(CONTROLLER.TC_Link);
	}
	public void AcceptTickEvent()
	{
		AudioPlayer.instance.PlayButtonSnd();
		Tick.SetActive(!Tick.activeInHierarchy);
	}


}
