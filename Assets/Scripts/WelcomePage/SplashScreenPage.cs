using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public partial class SplashScreenPage : MonoBehaviour 
{
	public GameObject welcomeScreen;
	public GameObject Tick;

    void Awake()
    {
        //Debug.unityLogger.logEnabled = false;
		CONTROLLER.DeviceID = SystemInfo.deviceUniqueIdentifier;
		startLogoAnim();
		closeTheWelcomeScreen();
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
        welcomeScreen.SetActive(false);
        Sequence sequence = DOTween.Sequence();
		sequence.Append(welcomeScreen.transform.DOLocalMove(new Vector3(0.0f, -(Screen.height + 100f), 0.0f), 0.0f).SetEase(Ease.Linear));
		sequence.AppendInterval(1.0f);
		sequence.AppendCallback(() =>
	   {
		   welcomeScreen.SetActive(false);
		   PlayerPrefs.SetString("Welcome_Screen_Closed", "1");
		   LoadMenuScene();
	   });
	}

	public void LoadMenuScene()
	{
        StartCoroutine(LoadMainMenuFromSplash());
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


	
	public void AcceptTickEvent()
	{
		AudioPlayer.instance.PlayButtonSnd();
		Tick.SetActive(!Tick.activeInHierarchy);
	}


}
