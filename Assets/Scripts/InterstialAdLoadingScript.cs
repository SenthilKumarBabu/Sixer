//#define GDEBUG // UNCOMMENT THIS TO ENABLE DEBUG LOG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class InterstialAdLoadingScript : Singleton<InterstialAdLoadingScript>
{
    public Sprite[] modeLogoSprites;
    public Image[] modeLogoImage;
    public Image fillImage,FillImageScorecard;
    public Text countText,CountTextScorecard;
    public GameObject holder;
    public GameObject SimpleLoadingHolder;
    public GameObject ScoreCardLoadingHolder;
    public GameObject tapToContinuePanel;
    private float countInt;
    private float TotalCount = 3;
    public int buttonIndex = 0;
    
    public GameObject Number3,Number2,Number1,NumberGO;
    public Image HolderAlpha;

    public Text ScoreText, OversText;

    public bool isAdStartsToPlay = false;

    private void Start()
    {
        holder.SetActive(false);
    }
   
    public void ShowMe(int index)
    {
        buttonIndex = index;
        if ( AdIntegrate.instance.checkTheInternet() && CONTROLLER.launchInternetAdEvent && AdIntegrate.instance.isInterstitialReadyToPlay() == true && !CONTROLLER.isAdRemoved )
        {
            countText.text = "3";
            CountTextScorecard.text = "3";
            holder.SetActive(true);
            if (buttonIndex == 6)
            {
                HolderAlpha.color = new Color(0f, 0f, 0f, 0.7f);
                ScoreCardLoadingHolder.SetActive(true);
                ScoreText.text = ("SCORE : " + GameModel.ScoreStr).ToUpper();
                OversText.text = ("OVERS : " + GameModel.OversStr).ToUpper();
            }
            else
            {
                HolderAlpha.color = new Color(0f, 0f, 0f, 0.5f);
                SimpleLoadingHolder.SetActive(true);
            }
            tapToContinuePanel.SetActive(false);
            StartCountDown();
            AdIntegrate.instance.SetTimeScale(0f);
        }
        else if (buttonIndex == 6  )
        {
#if GDEBUG
            DebugLogger.PrintWithColor("222222222222222222222222222222");
#endif
            StartCoroutine(AdIntegrate.instance.RequestInterestialAd());
            return;
        }
        else
        {
#if GDEBUG
            DebugLogger.PrintWithColor("333333333333333333333333");
#endif

            holder.SetActive(false);
            OnCompletedInterstialAd();
            StartCoroutine(AdIntegrate.instance.RequestInterestialAd());
        }
    }

    public void StartCountDown()
    {
#if GDEBUG
        DebugLogger.PrintWithColor("############## START COUNTDOWN ##########::: TIME.time::  " + Time.timeScale);
#endif
        isAdStartsToPlay = false;

        countInt = 0;

        fillImage.fillAmount = 1f;
        FillImageScorecard.fillAmount = 1f;
        countText.text = "3";
        CountTextScorecard.text = "3";
        StartCoroutine(IncreaseCountText());
    }

    IEnumerator IncreaseCountText()
    {
#if GDEBUG
        DebugLogger.PrintWithColor("########### InCREASE COUNT TEXT: tempCount::   TIME: " + Time.timeScale);
#endif
        yield return new WaitForSecondsRealtime(1f);
        countInt++;
        float tempCount = TotalCount - countInt;

        float barTempval = tempCount / TotalCount;
        fillImage.fillAmount = barTempval;
        FillImageScorecard.fillAmount = barTempval;
        countText.text = tempCount.ToString();
        CountTextScorecard.text = tempCount.ToString();

        if (tempCount < 0)
        {
            HideMe();
            tempCount = 0;
        }
        else
        {
            if (holder.activeSelf)
                StartCoroutine(IncreaseCountText());
        }
    }

    public void HideMe()
    {
        SimpleLoadingHolder.SetActive(false);
        ScoreCardLoadingHolder.SetActive(false);
#if GDEBUG
        DebugLogger.PrintWithColor("######### HIDE MEEEEEEEEEE##########  AdIntegrate.instance.isInterstitialAvailable" + AdIntegrate.instance.isInterstitialReadyToPlay() + " TIME: " + Time.timeScale + " CanShowAdtoNewUser: " + CONTROLLER.CanShowAdtoNewUser_Inter + " IGQ: " + CONTROLLER.serverConfig.IGQ);
#endif
            ShowTapToContinue();
    }

    public GameObject Three, Two, One, Go;

    public void ShowTapToContinue()
    {
#if GDEBUG
        DebugLogger.PrintWithColor("########## SHOW TAP TO CONTINUE ###############   TIME: " + Time.timeScale +" holder activeself: "+ holder.activeSelf);
#endif
        if (holder.activeSelf)
        {
            Number3.SetActive(false);
            Number2.SetActive(false);
            Number1.SetActive(false);
            NumberGO.SetActive(false);

            //image animation
            Three.SetActive(true); Two.SetActive(true); One.SetActive(true); Go.SetActive(true);
            Three.transform.localScale = Vector3.zero;
            Two.transform.localScale = Vector3.zero;
            One.transform.localScale = Vector3.zero;
            Go.transform.localScale = Vector3.zero;

            if (CanShowResumeCountdown())
            {
                HolderAlpha.color = new Color(0f, 0f, 0f, 0f);
                tapToContinuePanel.SetActive(true);
              //  StartCoroutine(ShowCountAndContinue());
                StartCoroutine(StartAnimation());

            }
            else
            {
                Continue();
            }
        }
    }

    IEnumerator ShowCountAndContinue()
    {
#if GDEBUG
        DebugLogger.PrintWithColor("########### SHOW COUNTDOWN AND CONTINUE TIME: " + Time.timeScale);
#endif
        yield return new WaitForSecondsRealtime(1f);
        Number3.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        Number3.SetActive(false);
        Number2.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        Number2.SetActive(false);
        Number1.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        Number1.SetActive(false);
        NumberGO.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        Continue();
    }

    private IEnumerator StartAnimation()
    {
#if GDEBUG
        DebugLogger.PrintWithColor("########### SHOW COUNTDOWN AND CONTINUE TIME: " + Time.timeScale +" time: "+Time.time);
#endif
        yield return new WaitForSecondsRealtime(0f);
        if (AudioPlayer.instance != null && ManageScene.CurScene == Scenes.Ground)
        {
            AudioPlayer.instance.PlayGameSnd("coutdown");
        }
        Sequence sq = DOTween.Sequence();
        sq.SetUpdate(true);
        sq.Insert(0f, Three.GetComponent<Image>().DOFade(1, 0f));
        sq.Insert(0f, Two.GetComponent<Image>().DOFade(1, 0f));
        sq.Insert(0f, One.GetComponent<Image>().DOFade(1, 0f));
        sq.Insert(0f, Go.GetComponent<Image>().DOFade(1, 0f));

        sq.Insert(0f, Three.transform.DOScale(1f, 1f).SetEase(Ease.OutBack));
        sq.AppendCallback(() =>
        {
            Three.GetComponent<Image>().DOFade(0, 0.2f).SetUpdate(true);
            //Three.SetActive(false);
        });

        sq.Insert(1f, Two.transform.DOScale(1f, 1f).SetEase(Ease.OutBack));
        sq.AppendCallback(() =>
        {
            Two.GetComponent<Image>().DOFade(0, 0.2f).SetUpdate(true);
            //Two.SetActive(false);
        });

        sq.Insert(2f, One.transform.DOScale(1f, 1f).SetEase(Ease.OutBack));
        sq.AppendCallback(() =>
        {
            One.GetComponent<Image>().DOFade(0, 0.2f).SetUpdate(true);
            //One.SetActive(false);
        });

        sq.Insert(3f, Go.transform.DOScale(1f, 1f).SetEase(Ease.OutBack));
        sq.AppendCallback(() =>
        {
            Go.GetComponent<Image>().DOFade(0, 0.2f).SetUpdate(true);
            //Go.SetActive(false);
            Continue();
        });
    }


    public void Continue()
    {
#if GDEBUG
        DebugLogger.PrintWithColor("########## CONTINUE BUTTON CLICKED ###############   TIME: " + Time.timeScale);
#endif
        isAdStartsToPlay = false;
        AdIntegrate.instance.HideAd();
        holder.SetActive(false);
        AdIntegrate.instance.SetTimeScale(1f);
        OnCompletedInterstialAd();
    }

    bool CanShowResumeCountdown()
    {
        if (CONTROLLER.gameMode == "superover")
            return false;
        else if (GameOverScreen.instance != null)
            return false;
        else
            return true;
    }

    public void OnCompletedInterstialAd()
    {
#if GDEBUG
        DebugLogger.PrintWithColor("==========OnCompletedInterstialAd CALLED=========== "+ CONTROLLER.gameMode + " TIME: " + Time.timeScale);
#endif

        switch (CONTROLLER.gameMode)
        {
            case "superover": CallSuperOverFunction();
                break;
            case "slogover":CallSuperSlogFunction();
                break;
            case "chasetarget": CallSuperChaseFunction();
                break;               
        }        
    }

    private void CallSuperOverFunction()
    {
#if GDEBUG
        DebugLogger.PrintWithColor("################ CALL SUPER OVER FUNCTIONS###########: buttonIndex: " + buttonIndex + " TIME: " + Time.timeScale);
#endif
        switch (buttonIndex)
        {
            case 0:
                SuperOverResult.instance.GoToHome();
                break;
            case 1:
                SuperOverResult.instance.GoNextLevel();
                break;
            case 2:
                SuperOverResult.instance.ReplayThisLevel();
                break;
        }
    }

    private void CallSuperChaseFunction()
    {
#if GDEBUG
        DebugLogger.PrintWithColor("################ CALL SUPER CHASES FUNCTIONS###########: TIME: " + Time.timeScale);
#endif

        if (GameOverScreen.instance != null)
        {
            GameOverScreen.instance.menuClicked(buttonIndex);
        }
        else
        {
#if GDEBUG
            DebugLogger.PrintWithColor("######### ELSE SUPER CHASE CALLED ######### ");
#endif
            ShowTapToContinue();
        }
    }

    private void CallSuperSlogFunction()
    {
#if GDEBUG
        DebugLogger.PrintWithColor("################ CALL SUPER SLOG FUNCTIONS###########: TIME: " + Time.timeScale);
#endif

        if (GameOverScreen.instance != null)
		{
			GameOverScreen.instance.menuClicked(buttonIndex);
		}
        else
        {
#if GDEBUG
            DebugLogger.PrintWithColor("######### ELSE SUPER SLOG CALLED ######### ");
#endif
            ShowTapToContinue();
        }
    }
}
