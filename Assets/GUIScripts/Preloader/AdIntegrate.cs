using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using GoogleMobileAds.Api.Mediation;
using GoogleMobileAds.Api.Mediation.UnityAds;
using GoogleMobileAds.Api.Mediation.InMobi;
using GoogleMobileAds.Api.Mediation.IronSource;
using GoogleMobileAds.Api.Mediation.AppLovin;
using GoogleMobileAds.Api.Mediation.AdColony;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;


public class AdIntegrate : MonoBehaviour
{
	public static AdIntegrate instance;
    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewardBasedVideo;
    private bool adshown = true;
    public bool isInterstitialAvailable = false;
    //public bool isRewardedVideoAvailable = false;
	private AndroidJavaObject objNative = null;
	private AndroidJavaObject playerActivityContext = null;
    //private ZaprDataSdkPlugin zaprDataPlugin;

    [HideInInspector]
    public bool isRegisteredContestUser;

    public int CurrentSceneIndex;

    protected void Awake ()
	{
		instance = this;
		#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
		CONTROLLER.TargetPlatform = "standalone";
		#endif

		#if UNITY_WEBPLAYER
		CONTROLLER.TargetPlatform = "web";
		#endif

		#if UNITY_ANDROID
		CONTROLLER.TargetPlatform = "android";
		
		#endif

		#if UNITY_IPHONE
		CONTROLLER.TargetPlatform = "ios";
		#endif

		DontDestroyOnLoad (this);

#if UNITY_ANDROID && !UNITY_EDITOR
		if (objNative == null)
		{
		// First, obtain the current activity context
		AndroidJavaClass actClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");			
		playerActivityContext = actClass.GetStatic<AndroidJavaObject> ("currentActivity");

		// Pass the context to a newly instantiated NativeAndroid object
		AndroidJavaClass pluginClass = new AndroidJavaClass ("com.nextwave.android.NativeAndroid");

		if (pluginClass != null)
		{
		objNative = pluginClass.CallStatic<AndroidJavaObject>("instance");
		objNative.Call ("setContext", playerActivityContext);
		objNative.Call ("setActivity", playerActivityContext);
		}
		}
#endif
	}

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        QuitTheApp();
    }
    void QuitTheApp()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
		if (objNative != null)
		{
            objNative.Call("exitTheGame");
        }
#endif
    }

    public void Start()
	{
        SceneManager.sceneLoaded += OnSceneLoaded;
        AdColonyAppOptions.SetGDPRRequired(true);
        AdColonyAppOptions.SetGDPRConsentString("1");
        UnityAds.SetGDPRConsentMetaData(true);
        Dictionary<string, string> consentObject = new Dictionary<string, string>();
        consentObject.Add("gdpr_consent_available", "true");
        consentObject.Add("gdpr", "1");
        InMobi.UpdateGDPRConsent(consentObject);
        IronSource.SetConsent(true);
        AppLovin.SetHasUserConsent(true);
        StartCoroutine(initAdmob());
	}

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CurrentSceneIndex = scene.buildIndex;
        StartCoroutine(checkAndRemoveEventSystems());
    }
    private IEnumerator checkAndRemoveEventSystems()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        UnityEngine.EventSystems.EventSystem[] eventss = FindObjectsOfType<UnityEngine.EventSystems.EventSystem>();
        if (eventss != null && eventss.Length > 1)
        {
            if (CONTROLLER.EnableHardcodes==1)
            {
                Popup.instance.showGenericPopup("EventSystem!", "EventSystem Count:" + eventss.Length);
            }
            for (int i = 1; i < eventss.Length; i++)
            {
                Destroy(eventss[i].gameObject);
            }
        }
        else if ((eventss != null && eventss.Length == 0) || eventss == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
    }

    public IEnumerator initAdmob()
    {
        if (CurrentSceneIndex != 2)
        {
            yield return new WaitForSeconds(5.0f);
        }
        else
        {
           
            yield return new WaitForSeconds(40.0f);
        }

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (checkTheInternet())
        {
            if(CONTROLLER.launchInternetAdEvent == false)
            {
            RequestConfiguration requestConfiguration =
                        new RequestConfiguration.Builder()
                        .SetSameAppKeyEnabled(true).build();
            MobileAds.SetRequestConfiguration(requestConfiguration);
                MobileAds.Initialize(HandleInitCompleteAction);
            }
            if ((CurrentSceneIndex == 1 || CurrentSceneIndex == 0) && CONTROLLER.launchInternetAdEvent == false )
            {
                CONTROLLER.launchInternetAdEvent = true;
            }
            else if(CurrentSceneIndex == 2 && CONTROLLER.launchInternetAdEvent == false)
            {
                CONTROLLER.launchInternetAdEvent = true;
            }
        }
#endif
    }

    private void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        if (checkTheInternet())
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            //RequestBanner();
            StartCoroutine(requestRewardedVideo());
            //StartCoroutine(RequestInterestialAd());
             CONTROLLER.receivedAdEvent = true;
#endif
        });
        }
    }
    public void RequestBanner()
	{
        //if (CurrentSceneIndex == 1 || (CurrentSceneIndex == 2 && CONTROLLER.gameMode == "multiplayer"))
          //  return; //as per karthik statement fully banner ad is stopped


  #if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (CONTROLLER.isAdRemoved || CONTROLLER.CanShowAdtoNewUser_Banner ==0 )
        {
            return;
        }
        else 
        {
        if (checkTheInternet())
        {
#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-4222943899773726/4913551145";   
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
			string adUnitId = "unexpected_platform";
#endif
            // Clean up banner ad before creating a new one.
            if (this.bannerView != null)
            {
                this.bannerView.Destroy();
            }
            // Create a 320x50 banner at the top of the screen.
            //float widthInPixels = Screen.safeArea.width > 0 ? Screen.safeArea.width : Screen.width;
            //int width = (int)(widthInPixels / MobileAds.Utils.GetDeviceScale());
            //MonoBehaviour.print("requesting width: " + width.ToString());
            //AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(width);
            //this.bannerView = new BannerView(adUnitId, adaptiveSize, AdPosition.Top);
            this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
            // Register for ad events.
            this.bannerView.OnAdLoaded += this.HandleAdLoaded;
            this.bannerView.OnAdFailedToLoad += this.HandleAdFailedToLoad;
            this.bannerView.OnAdOpening += this.HandleAdOpened;
            this.bannerView.OnAdClosed += this.HandleAdClosed;
            AdRequest bannerRequest = new AdRequest.Builder().Build();
            // Load a banner ad.
            this.bannerView.LoadAd(bannerRequest);
                if (objNative != null && CONTROLLER.CanShowAdToast)
                {
                        objNative.Call("showTheToast", "Request Banner Ad");
                }
        }
        }
#endif
    }

    public void HideAd ()
	{
        if (checkTheInternet() && CONTROLLER.receivedAdEvent)
        {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            if (this.bannerView != null)
            {
			    adshown = false;
                this.bannerView.Hide();
            }
            if (CONTROLLER.isAdRemoved)
            {
                RemoveAds();
            }
#endif
        }
    }
	public void ShowBannerAd ()
	{
        if(CurrentSceneIndex== 1 || ( CurrentSceneIndex == 2 && (CONTROLLER.gameMode == "multiplayer" || CONTROLLER.CurrentPage != "ingame") ) )
            return; //as per karthik statement fully banner ad is stopped

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (CONTROLLER.isAdRemoved || CONTROLLER.CanShowAdtoNewUser_Banner == 0)
		{
			HideAd();
			RemoveAds();
			return;
		}
        if (checkTheInternet() && CONTROLLER.receivedAdEvent)
        {
            if (CONTROLLER.CurrentPage != "splashpage" || CONTROLLER.CurrentPage != "")
            {
                if (this.bannerView != null)
                {
                    adshown = true;
                    this.bannerView.Show();
                    FirebaseAnalyticsManager.instance.logEvent("Bannerads_shown");
                }
                else
                {

                }
                if (CONTROLLER.CurrentPage == "splashpage")
                {
                    HideAd();
                }
            }
        }
#endif
    }
    public void RemoveAds ()
	{
    if (checkTheInternet() && CONTROLLER.receivedAdEvent)
    {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (this.bannerView != null)
        {
            this.bannerView.Destroy();
        }
#endif
    }
    }
	public IEnumerator RequestInterestialAd ()
	{        
        yield return new WaitForSecondsRealtime(3.0f);



#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
    if (CONTROLLER.isAdRemoved || CONTROLLER.CanShowAdtoNewUser_Inter == 0)
	{
	    yield break;
	}
    else if (checkTheInternet() && CONTROLLER.launchInternetAdEvent)
    {
#if UNITY_ANDROID
			string adUnitId = "ca-app-pub-4222943899773726/9973224560"; 
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
			string adUnitId = "unexpected_platform";
#endif
        // Clean up interstitial ad before creating a new one.
        if (this.interstitial != null)
        {
            this.interstitial.Destroy();
            if(this.interstitial.IsLoaded())
            {
               yield break;
            }
        }
        // Create an interstitial.
        this.interstitial = new InterstitialAd(adUnitId);

        // Register for ad events.
        this.interstitial.OnAdLoaded += this.HandleInterstitialLoaded;
        this.interstitial.OnAdFailedToLoad += this.HandleInterstitialFailedToLoad;
        this.interstitial.OnAdOpening += this.HandleInterstitialOpened;
        this.interstitial.OnAdClosed += this.HandleInterstitialClosed;
        AdRequest interstitialRequest = new AdRequest.Builder().Build();
        // Load an interstitial ad.
        this.interstitial.LoadAd(interstitialRequest);
         if (objNative != null && CONTROLLER.CanShowAdToast)
         {
              objNative.Call("showTheToast", "Request interstitial Ad");
         }
    }
#endif
    }
    public void ShowInterestialAd ()
	{
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (CONTROLLER.isAdRemoved || this.interstitial == null  )
		{
			return;
		}
        if (checkTheInternet() && CONTROLLER.receivedAdEvent && CONTROLLER.launchInternetAdEvent)
        {
            if (this.interstitial.IsLoaded())
            {
                if (this.interstitial != null && CONTROLLER.CanShowAdToast)
                {
                    DebugLogger.PrintWithColor("################ SHOW INTERTIAL AD ########## GetMediationAdapterClassName interstitial :: " + this.interstitial.GetResponseInfo().GetMediationAdapterClassName() +" GetResponseId interstitial :: " + this.interstitial.GetResponseInfo().GetResponseId());
                }
                FirebaseAnalyticsManager.instance.logEvent("Interstitialads_shown");
                this.interstitial.Show();
            }
            else
            {
                StartCoroutine(RequestInterestialAd());
            }
        }
#endif
    }
    public bool isRewardedReadyToPlay()
    {
#if UNITY_EDITOR
        return true;
#endif
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (checkTheInternet() && CONTROLLER.receivedAdEvent && CONTROLLER.launchInternetAdEvent && this.rewardBasedVideo != null )
        {
          if (this.rewardBasedVideo.IsLoaded())
          {
            return true;
          }
          else
          {
            //requestRewardedVideo();
            return false;
          }
        }
        else if(CONTROLLER.launchInternetAdEvent == false || CONTROLLER.receivedAdEvent == false)
        {
            StartCoroutine(initAdmob());
        }
#endif
        return false;
    }

    public bool isInterstitialReadyToPlay()
    {
#if UNITY_EDITOR
        return true;
#endif
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (checkTheInternet() && CONTROLLER.receivedAdEvent && CONTROLLER.launchInternetAdEvent && this.interstitial != null )
        {
          if (this.interstitial.IsLoaded())
          {
            return true;
          }
          else
          {
            //RequestInterestialAd();
            return false;
          }
        }
        else if(CONTROLLER.launchInternetAdEvent == false || CONTROLLER.receivedAdEvent == false)
        {
            StartCoroutine(initAdmob());
        }
#endif
        return false;
    }
    bool ReadyForNextRequest = true;
    IEnumerator waitForNextRequest()
    {
        ReadyForNextRequest = false;
        yield return new WaitForSecondsRealtime(5.0f);
        ReadyForNextRequest = true;
    }

    public IEnumerator requestRewardedVideo ()
	{
        yield return new WaitForSecondsRealtime(3.0f);
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (ReadyForNextRequest)
        {
            if (checkTheInternet())
            {
#if UNITY_ANDROID
			string adUnitId = "ca-app-pub-4222943899773726/7347061229";  
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
		string adUnitId = "unexpected_platform";
#endif
                rewardBasedVideo = new RewardedAd(adUnitId);
                this.rewardBasedVideo.OnAdLoaded += this.HandleRewardBasedVideoLoaded;
                this.rewardBasedVideo.OnAdFailedToLoad += this.HandleRewardBasedVideoFailedToLoad;
                this.rewardBasedVideo.OnAdOpening += this.HandleRewardBasedVideoOpened;
                this.rewardBasedVideo.OnUserEarnedReward += this.HandleRewardBasedVideoRewarded;
                this.rewardBasedVideo.OnAdClosed += this.HandleRewardBasedVideoClosed;
                AdRequest rewardedVideoRequest = new AdRequest.Builder().Build();
                this.rewardBasedVideo.LoadAd(rewardedVideoRequest);
                StartCoroutine(waitForNextRequest());
                if (objNative != null  && CONTROLLER.CanShowAdToast)
                 {
                  objNative.Call("showTheToast", "Request RewardedVideo Ad");
                 }
            }
        }
#endif
    }
    public void ShowRewardedVideo ()
	{
#if UNITY_EDITOR
        VideoRewarded();
        return;
#endif
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (Achievements .instance !=null)
			Achievements.instance.RewardedVideoEvent(0);
        if (checkTheInternet() && CONTROLLER.receivedAdEvent)
        {
            if (isRewardedReadyToPlay())
            {
                if (rewardBasedVideo != null && CONTROLLER.CanShowAdToast)
                {
                    DebugLogger.PrintWithColor(" SHOW REWARD VIDEO ######## GetMediationAdapterClassName :: " + rewardBasedVideo.GetResponseInfo().GetMediationAdapterClassName() +" GetResponseId :: " + rewardBasedVideo.GetResponseInfo().GetResponseId());
                }
                AdIntegrate.instance.SystemSleepSettings(0);
                this.rewardBasedVideo.Show();
            }
            else
            {
                StartCoroutine(requestRewardedVideo());
                //MonoBehaviour.print("Reward based video ad is not ready yet");
            }
        }
#endif
    }

    IEnumerator call_interstitial_close()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        if (InterstialAdLoadingScript.instance != null)
        {
            InterstialAdLoadingScript.instance.ShowTapToContinue();
        }
    }
    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        CONTROLLER.receivedAdEvent = true;

        if (CurrentSceneIndex == 1 || (CurrentSceneIndex == 2 && CONTROLLER.gameMode == "multiplayer"))
        {
            HideAd();
            return; //as per karthik statement fully banner ad is stopped
        }
        else
        {
            if (CONTROLLER.CurrentPage == "splashpage" || CONTROLLER.CurrentPage == "")
            {
                HideAd();
                return;
            }
        }
        MonoBehaviour.print("Banner AdLoaded event received");
        //if (objNative != null && CONTROLLER.CanShowAdToast)
        //{
        //    objNative.Call("showTheToast", "BannerAd Loaded");
        //}
    }
    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.ToString());
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }
#endregion

#region Interstitial callback handlers
    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialLoaded event received: " + Time.timeScale);
        CONTROLLER.receivedAdEvent = true;
        isInterstitialAvailable = true;
        if (objNative != null && CONTROLLER.CanShowAdToast)
        {
            objNative.Call("showTheToast", "InterstitialAd Loaded");
        }
    }
   public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		isInterstitialAvailable = false;
		MonoBehaviour.print("HandleInterstitialFailedToLoad event received with message: " + args.ToString());
	}
    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        isInterstitialAvailable = false;
        //if (InterstialAdLoadingScript.instance != null)
        //{
        //    InterstialAdLoadingScript.instance.ShowTapToContinue();
        //}
        //  SetTimeScale(1);
        if (CurrentSceneIndex == 2 && CONTROLLER.CurrentPage == "ingame" && CONTROLLER.gameMode!= "multiplayer")
        {
            MonoBehaviour.print("HandleInterstitialOpened isadstartstoplay set TRUE: " + Time.timeScale);

            InterstialAdLoadingScript.instance.isAdStartsToPlay = true;
        }
        MonoBehaviour.print("HandleInterstitialOpened event received: " + Time.timeScale);
    }
    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        if (CurrentSceneIndex == 2 && CONTROLLER.CurrentPage == "ingame" && CONTROLLER.gameMode != "multiplayer")
        {
            MonoBehaviour.print("HandleInterstitialClosed isadstartstoplay set FALSE: " + Time.timeScale);

            InterstialAdLoadingScript.instance.isAdStartsToPlay = false;
        }
        // SetTimeScale(0);
        MonoBehaviour.print("HandleInterstitialClosed event received: " + Time.timeScale);
        isInterstitialAvailable = false;
        StartCoroutine(call_interstitial_close());
        StartCoroutine(RequestInterestialAd());
        
        //if (SuperCardsUI.instance != null && SuperCardsUI.instance.GameOverScreen.gameObject.activeInHierarchy)
        //{
        //    SuperCardsUI.instance.GotoGameOverscreen();
        //}
    }
#endregion
#region RewardBasedVideo callback handlers

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        //isRewardedVideoAvailable = true;
        CONTROLLER.receivedAdEvent = true;
        MonoBehaviour.print("HandleRewardBasedVideoLoaded event received: "+ Time.timeScale);
        if (objNative != null && CONTROLLER.CanShowAdToast)
        {
            objNative.Call("showTheToast", "RewardedVideoAd Loaded");
        }
    }
    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoFailedToLoad event received: " + Time.timeScale);

        //isRewardedVideoAvailable = false;
    }
    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoOpened event received: "+ Time.timeScale);
    }
    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoStarted event received: " + Time.timeScale);
    }
    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received: " + Time.timeScale);
        StartCoroutine(requestRewardedVideo());
    }
    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoRewarded event received: " + Time.timeScale);

        string type = args.Type;
        double amount = args.Amount;
        StartCoroutine(App_VideoRewarded());
	}
    IEnumerator App_VideoRewarded()
    {
        if( CONTROLLER.gameMode == "multiplayer")
            yield return new WaitForSecondsRealtime(0.5f);
        else
            yield return new WaitForSecondsRealtime(0.2f);

        VideoRewarded();
    }
    public void VideoRewarded()
    {
        if (CONTROLLER.RewardedVideoClickedState == 2 || CONTROLLER.RewardedVideoClickedState == 3)
        {
            if (ProgressBar.instance != null)
            {
                ProgressBar.instance.Success();
            }

           // FirebaseAnalyticsManager.instance.logEvent("IAP", new string[] { "IAP_Action", "RV_Wicket" });
        }
        else if (CONTROLLER.RewardedVideoClickedState == 1)
        {
            if (SOLevelSelectionPage.instance != null)
            {
                SOLevelSelectionPage.instance.WatchVideoSuccessEvent();
            }
        }
        else if (CONTROLLER.RewardedVideoClickedState == 4)
        {
            if (ExtraBall.instance != null)
            {
                ExtraBall.instance.Success();
            }
        }
        else if (CONTROLLER.RewardedVideoClickedState == 5)
        {
            if (HeadStart.instance != null)
            {
                HeadStart.instance.Success();
            }
        }
        else if (CONTROLLER.RewardedVideoClickedState == 6)
        {
            if (CTLevelSelectionPage.instance != null)
            {
                CTLevelSelectionPage.instance.WatchVideoSuccessEvent();
            }
        }
        else    // Normal ticket adding scenario
        {
            /* UserProfile.EarnedTickets += 1;
             UserProfile.Tickets += 1;
             PlayerPrefsManager.SaveCoins();
             if (GameModeSelector._instance != null)
             {
                 GameModeSelector._instance.UpdateLandingPageTopBars();
             }
             Popup.instance.showTicketPopup();
             StartCoroutine(CricMinisWebRequest.instance.UserSync());
            */
            if (CONTROLLER.gameMode == "multiplayer")
            {
                
            }

            DBTracking.instance.AddTicket();
        }
        CONTROLLER.RewardedVideoClickedState = -1;


    }
    #endregion
    public void ShowToast(string text)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
        if (objNative != null)
        {
            objNative.Call("showTheToast", text);
        }
#else
        GameObject prefabGO ;
		GameObject tempGO ;
		prefabGO = Resources.Load ("Prefabs/Toast")as GameObject ;
		tempGO = Instantiate (prefabGO)as GameObject ;
		tempGO.name = "Toast";
		tempGO.GetComponent <Toast > ().setMessge (text);
#endif
    }


    public bool NET_STATE = true;
	public bool checkTheInternet()
	{
#if UNITY_EDITOR
        return NET_STATE;
#elif UNITY_ANDROID && !UNITY_EDITOR
		if (objNative != null) {
			bool result = objNative.Call<bool> ("isInternetOn");
			return result; 
		} else {
			return false;
		}
		
#elif UNITY_IOS && !UNITY_EDITOR
		return ShareThisGame.checkTheInternet ();
#endif
	}

    public static readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
    public bool hasPendingPurchase = false;

#region READWRITEFILE FUNCTIONS

    //=================App-specific files======================================================
    public void writeInToFile(string stringData, string folderName, string fileName = "test.dat")
    {
#if UNITY_ANDROID && !UNITY_EDITOR
            objNative.Call("writeIntoFile", folderName, fileName, stringData);
#else
        IsDirectoryExistsElseCreate(folderName);

        File.WriteAllText(Application.persistentDataPath + "/" + folderName + "/" + fileName, stringData);
#endif
    }

    public bool IsFileExits(string folderName, string fileName)
    {
        bool isFilePresent = false;
#if UNITY_ANDROID && !UNITY_EDITOR
            isFilePresent = objNative.Call<bool>("isFileExist", folderName, fileName);
#else
        string filePath = Application.persistentDataPath + "/" + folderName + "/" + fileName;
        if (File.Exists(filePath))
        {
            isFilePresent = true;
        }
#endif
        return isFilePresent;
    }

    public bool IsFolderExits(string folderName)
    {
        bool isFolderPresent = false;
#if UNITY_ANDROID && !UNITY_EDITOR
            isFolderPresent = objNative.Call<bool>("isFolderExist", folderName);
#else
        string filePath = Application.persistentDataPath + "/" + folderName;
        DirectoryInfo dirInfo = new DirectoryInfo(filePath);
        if (dirInfo.Exists)
        {
            isFolderPresent = true;
        }
#endif
        return isFolderPresent;
    }

    public string readFromFile(string folderName, string fileName = "test.dat")
    {
        string stringData = "";
#if UNITY_ANDROID && !UNITY_EDITOR
            stringData = objNative.Call<string>("readFromFile", folderName, fileName);
#else
        bool fileExists = File.Exists(Application.persistentDataPath + "/" + folderName + "/" + fileName);
        if (fileExists)
        {
            stringData = File.ReadAllText(Application.persistentDataPath + "/" + folderName + "/" + fileName);
        }
#endif
        return stringData;
    }

    public void deleteFile(string folderName, string fileName = "test.dat")
    {
#if UNITY_ANDROID && !UNITY_EDITOR
            string isDeleted = objNative.Call<string>("deleteTheFile", folderName, fileName);
#else
        File.Delete(Application.persistentDataPath + "/" + folderName + "/" + fileName);
#endif
    }

    public void deleteFolder(string folderName)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
            string isDeleted = objNative.Call<string>("deleteTheFolder", folderName);
#else
        if (Directory.Exists(Application.persistentDataPath + "/" + folderName))
        {
            string[] filePaths = Directory.GetFiles(Application.persistentDataPath + "/" + folderName);
            foreach (string filePath in filePaths)
            {
                File.Delete(filePath);
            }
            Directory.Delete(Application.persistentDataPath + "/" + folderName);
        }
#endif
    }

    public void deleteTheFileByFullPath(string fullPathFileName)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
            string isDeleted = objNative.Call<string>("deleteTheFileByFullPath", fullPathFileName);
#else
        File.Delete(Application.persistentDataPath + "/" + fullPathFileName);
#endif
    }

    public string GetFilePath()
    {
        string stringData = "";
#if UNITY_ANDROID && !UNITY_EDITOR
            stringData = objNative.Call<string>("getAbsolutePath");
#else
        stringData = Application.persistentDataPath;
#endif
        return stringData;

    }

    private void IsDirectoryExistsElseCreate(string folderName)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
#else
        string filePath = Application.persistentDataPath + "/" + folderName;
        DirectoryInfo dirInfo = new DirectoryInfo(filePath);
        if (!dirInfo.Exists)
        {
            dirInfo.Create();
        }
#endif
    }
    //================================================================================
#endregion


    public void SystemSleepSettings(int idx=1)
    {
        if(idx==0)
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        else
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
    }

    public void SetTimeScale(float value)
    {
        Time.timeScale = value;
    }

    public void GetTicketRewardedVideoHelper()
    {
        if (checkTheInternet())
        {
            if (isRewardedReadyToPlay())
            {
                CONTROLLER.RewardedVideoClickedState = -1;
                ShowRewardedVideo();
            }
            else
            {
                tmpCountCheck = 0;
                StartCoroutine(LoadTicketRV());
            }
        }
        else
            Popup.instance.ShowNoInternetPopup();
    }

    int tmpCountCheck = 0;
    IEnumerator LoadTicketRV()
    {
        LoadingScreen.instance.Show();
        yield return new WaitForSecondsRealtime(1f);
        if(isRewardedReadyToPlay())
        {
            CONTROLLER.RewardedVideoClickedState = -1;
            ShowRewardedVideo();
        }
        else
        {
            tmpCountCheck++;
            if (tmpCountCheck > 29)
            {
                StopCoroutine(LoadTicketRV());
                LoadingScreen.instance.Hide();
                if (isRewardedReadyToPlay())
                {
                    CONTROLLER.RewardedVideoClickedState = -1;
                    ShowRewardedVideo();
                }
                else
                {
                    Popup.instance.showGenericPopup("", "No video Available");
                }
            }
            else
            {
                if (tmpCountCheck == 8)
                {
                    StartCoroutine(requestRewardedVideo());
                }
                StartCoroutine(LoadTicketRV());
            }
        }
    }
}

