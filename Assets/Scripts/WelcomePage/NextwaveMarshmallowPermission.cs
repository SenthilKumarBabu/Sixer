using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Text;



/* Please add this activity as MainActivity in to your AndroidManifest xml file.
 <activity android:icon="@drawable/app_icon" android:name="com.nextwave.unityandroidpermission.OverrideUnityPlayerActivity" android:label="@string/app_name" android:screenOrientation="sensorLandscape" android:launchMode="singleTask" android:configChanges="mcc|mnc|locale|touchscreen|keyboard|keyboardHidden|navigation|orientation|screenLayout|uiMode|screenSize|smallestScreenSize|fontScale|layoutDirection" android:theme="@android:style/Theme.NoTitleBar.Fullscreen">
 <intent-filter>
 <action android:name="android.intent.action.MAIN" />
 <category android:name="android.intent.category.LAUNCHER" />
 <category android:name="android.intent.category.LEANBACK_LAUNCHER" />
 </intent-filter>
 <meta-data android:name="unityplayer.SkipPermissionsDialog" android:value="true" />
 <meta-data android:name="unityplayer.UnityActivity" android:value="true" />
 <meta-data android:name="unityplayer.ForwardNativeEventsToDalvik" android:value="true" />
 </activity>
 */

/*
 showTheAlertDialog() arguments
 1) UnityGameobjectName - (Must)
 2) UnityreceiverMethodName - (Must)
 3) TitleName - (Optional i.e "None")
 4) Content msg - (Must)
 5) Positivebuttontext - (Must)
 6) Negativebuttontext - (Optional i.e "None")
 7) isCancelable (boolean) - (Must)
 8) IconName - (Optional i.e "None")
*/

public class NextwaveMarshmallowPermission : MonoBehaviour
{
	public static NextwaveMarshmallowPermission instance;
	private SplashScreenPage PreloadPageScript;
	private int INITIAL_PERMISSIONS_REQUEST_CODE = 100;
	private int SINGLE_PERMISSIONS_REQUEST_CODE = 200;
	public AndroidJavaObject objNextwavePermission = null;

	public AndroidJavaObject objNative = null;
	public AndroidJavaObject playerActivityContext = null;
	public AndroidJavaObject objNextwaveActivity = null;


	private string positiveButton = "Ok";
	private string negativeButton = "Cancel";
	private bool isInitialPermissionRequested = false;


	void Awake ()
	{
		DontDestroyOnLoad (this.gameObject);
		//PreloadPageScript = GameObject.Find("SplashScreenPage").GetComponent<SplashScreenPage>();
		instance = this;      
	}

	public void Start ()
	{
		initTheNativeAndroid ();
		SceneManager.sceneLoaded += OnSceneLoaded;
		if (ObscuredPrefs.GetInt("impk", 0) != 3) //Not Already detected
		{
			ObscuredPrefs.SetInt("impk", 0); //Not called to check this is mod apk or not
		}
		else if(ObscuredPrefs.GetInt("impk", 0) == 3)
		{
			CheckToShowModApkBlocker();
		}
		checkThisIsAdminUser();
		//Invoke("isDeviceRooted", 3f);

	}
	public bool isRootedDevice = false;
	private void isDeviceRooted()
	{
		if (objNative != null)
		{
			if (objNative.Call<bool>("isDeviceRooted"))
			{
				isRootedDevice = true;
				//FirebaseAnalyticsManager.instance.logEvent("isDeviceRooted");
			}
		}
	}
	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
	public void initTheNativeAndroid ()
	{
#if UNITY_ANDROID && !UNITY_EDITOR


		if(objNextwavePermission == null){
			// find the plugin instance
			AndroidJavaClass pluginClass = new AndroidJavaClass ("com.nextwave.unityandroidpermission.PermissionManager");
			objNextwavePermission = pluginClass.CallStatic<AndroidJavaObject> ("instance");
		}

		if (objNextwaveActivity == null)
        {
            AndroidJavaClass actClass = new AndroidJavaClass("com.nextwave.unityandroidpermission.OverrideUnityPlayerActivity");
            objNextwaveActivity = actClass.CallStatic<AndroidJavaObject>("instance");
        }

		if (ObscuredPrefs.GetInt("impk", 0) != 3) //Not Already detected
        {
           Invoke("abc123xyz456pqr00pqr",3f);//validateTheAppSignature
        }

		if (objNative == null) {
			// First, obtain the current activity context
			AndroidJavaClass actClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			playerActivityContext = actClass.GetStatic<AndroidJavaObject> ("currentActivity");

			// Pass the context to a newly instantiated NativeAndroid object
			AndroidJavaClass pluginClass = new AndroidJavaClass ("com.nextwave.android.NativeAndroid");
			if (pluginClass != null) {
				objNative = pluginClass.CallStatic<AndroidJavaObject> ("instance");
				objNative.Call ("setContext", playerActivityContext);
				objNative.Call ("setActivity", playerActivityContext);
				//objNative.Call ("showTheToast", "welcome to all");
			}
		}

		if (isMarshMallow () == false) 
		{
			moveToNextScene();
			return;
		}
		//objNative.Call ("showTheAlertDialog", this.gameObject.name, " ", "kaalirajan", "welcome eoheoh oejhio ", this.positiveButton, this.negativeButton, false, "None"); 
		if (isMarshMallow () == false) 
        {
            moveToNextScene();
            return;
        }
        else 
        {
			if (getAndroidSDKVersion() < 29)
			{
              requestTheInitialPermissions ();
            }else
            {
                moveToNextScene();
                return;
            }
        }
#else
		moveToNextScene();
		#endif
	}

	public int getAndroidSDKVersion()
	{
#if UNITY_EDITOR || UNITY_IOS
		return 30;
#endif
#if !UNITY_EDITOR
                using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
                {
                    return version.GetStatic<int>("SDK_INT");
                }
#endif
	}
	public bool isMarshMallow () 
	{
		using (var version = new AndroidJavaClass ("android.os.Build$VERSION")) {
			int sdkVersion = version.GetStatic<int> ("SDK_INT");
			if (sdkVersion >= 23) {
				return true;
			}
			return false;
		}
	}

	List<string> myDangerousPermissions = new List<string> ();
	List<string> myExplanationToPermissions = new List<string> ();
	List<string> mySettingMessages = new List<string> ();

	List<string> neededExplanationPermissions = new List<string> ();
	List<string> neededSettingsScreenPermissions = new List<string> ();
	List<string> normalRequestPermissions = new List<string> ();

	int neededExplanationIndexe = -1;
	int neededSettingScreenIndexe = -1;

	public void requestTheInitialPermissions ()
	{
		//objNative.Call ("showTheToast", "requestTheInitialPermissions called");
			isInitialPermissionRequested = true;
			//myDangerousPermissions.Add ("android.permission.READ_PHONE_STATE");
			//myDangerousPermissions.Add ("android.permission.WRITE_EXTERNAL_STORAGE");
			//myDangerousPermissions.Add ("android.permission.ACCESS_COARSE_LOCATION");
			//myDangerousPermissions.Add("android.permission.RECORD_AUDIO");

		//myExplanationToPermissions.Add ("Phone state enables us to send and accept challenges and also allows you to receive push notifications on various updates & offers.");
		//myExplanationToPermissions.Add ("Read/Write permissions is needed to offers and caching ads.");
		//myExplanationToPermissions.Add ("Coarse location permission is needed to serve you location specific ads & offers.");
		//myExplanationToPermissions.Add("The app needs microphone access to analyze offline TV viewing habits, to serve fewer but more relevant advertising.");

		//mySettingMessages.Add ("You have denied READ_PHONE_STATE permission, please enable it from settings.");
		//mySettingMessages.Add ("You have denied STORAGE permission, please enable it from settings.");
		//mySettingMessages.Add ("You have denied ACCESS COARSE LOCATION permission, please enable it from settings.");
		//mySettingMessages.Add("You have denied RECORD_AUDIO permission, please enable it from settings.");
		List<string> deniedPermissions = new List<string> ();
		for (byte i = 0; i < myDangerousPermissions.Count; i++) {
			//Check this permision is already granted or not
			bool isGranted = objNextwavePermission.Call<bool>("checkSelfPermission", myDangerousPermissions [i]);
			if (isGranted == false) {
				//Already Denied permissions only
				deniedPermissions.Add (myDangerousPermissions [i]);
			}
		}

		if (deniedPermissions.Count > 0) { 
			//Some permissions are denied already
			bool isNeedExplanation = false;
			for (byte i = 0; i < deniedPermissions.Count; i++) {
				isNeedExplanation = false;
				isNeedExplanation = objNextwavePermission.Call<bool>( "shouldShowRequestPermissionRationale", deniedPermissions [i] );//that returns true if the user has previously denied the request.

				if (isNeedExplanation) {
					neededExplanationIndexe = 0;
					neededExplanationPermissions.Add (deniedPermissions [i]);
				} else if (PlayerPrefs.HasKey (deniedPermissions [i])) {
					neededSettingScreenIndexe = 0;
					neededSettingsScreenPermissions.Add (deniedPermissions [i]);
				} else {//Very first time request
					normalRequestPermissions.Add (deniedPermissions [i]);
				}
			}
			if (normalRequestPermissions.Count > 0) {
				//Very first time request
				requestInitialPermissions (normalRequestPermissions.ToArray ());
			} else if (neededExplanationPermissions.Count > 0) {
				showTheExplanationOnebyOne ();
			} else if (neededSettingsScreenPermissions.Count > 0) {
				showTheSettingScreenOnebyOne ();
			} else {
				moveToNextScene ();
			}
		} else {
			//All permission are already granted 
			moveToNextScene ();
		}

		deniedPermissions = null;
	}

	private void showTheExplanationOnebyOne ()
	{
		//Here show the common explanation alert.
		objNative.Call ("showTheAlertDialog", this.gameObject.name, "initialRequestExplanationAlertClicked", "None", myExplanationToPermissions [myDangerousPermissions.IndexOf (neededExplanationPermissions [neededExplanationIndexe])], positiveButton, "None", true, "None"); 
		//Never ask again option also come after this alert
		PlayerPrefs.SetString (neededExplanationPermissions [neededExplanationIndexe], "neveraskagain");
	}

	private void showTheSettingScreenOnebyOne ()
	{
		this.positiveButton = "Settings";
		this.negativeButton = "Cancel";
		objNative.Call ("showTheAlertDialog", this.gameObject.name, "showTheInitialSettingScreen", "None", mySettingMessages [myDangerousPermissions.IndexOf (neededSettingsScreenPermissions [neededSettingScreenIndexe])], this.positiveButton, this.negativeButton, true, "None");
		neededSettingScreenIndexe++;
	}

	private string[] deniedInitialPermissions;
	private void requestInitialPermissions (string[] initialPermissionss)
	{
		deniedInitialPermissions = initialPermissionss; 
		//Here request the permissions to denied permissions only. And your callback method is onRequestPermissionsResult()
		objNextwavePermission.Call( "requestPermissions", new object[] { deniedInitialPermissions, INITIAL_PERMISSIONS_REQUEST_CODE} );
	}

	private string singlePermission;
	private string singlePermissionPositiveButton, singlePermissionNegativeButton;
	private string singlePermissionCallBackGameobjectName, singlePermissionCallBackMethodName;

	public void requestThePermission (string permission, string explanationString, string positiveButton, string negativeButton, bool isCancelable, int requestCode, string callBackGameObjectName, string callBackMethodName)
	{
		if (isMarshMallow ()) {
			singlePermission = permission;
			SINGLE_PERMISSIONS_REQUEST_CODE = requestCode;

			singlePermissionPositiveButton = positiveButton;
			singlePermissionNegativeButton = negativeButton;
			singlePermissionCallBackGameobjectName = callBackGameObjectName;
			singlePermissionCallBackMethodName = callBackMethodName;

			//Check this permision is already granted or not
			bool isGranted = objNextwavePermission.Call<bool>("checkSelfPermission", permission);

			if (isGranted == false) {
				bool isNeedExplanation = objNextwavePermission.Call<bool>( "shouldShowRequestPermissionRationale", permission);

				if (isNeedExplanation) {
					//Here show the explanation alert by explanationString
					objNative.Call ("showTheAlertDialog", this.gameObject.name, "singleRequestExplanationAlertClicked", "None", explanationString, positiveButton, "None", isCancelable, "None");
					PlayerPrefs.SetString (permission, "neveraskagain");
				} else {
					if (PlayerPrefs.HasKey (permission)) {
						//Previously Never ask again options was clicked
						//Here show the alert to go to mobile setting screen.
						this.positiveButton = "Settings";
						this.negativeButton = "Cancel";
						explanationString = "You have denied CONTACT, please enable it from setting!";
						objNative.Call ("showTheAlertDialog", this.gameObject.name, "showTheSettingScreen", "None", explanationString, this.positiveButton, this.negativeButton, isCancelable, "None"); //UnityGameobjectName, UnityreceiverMethodName,title, message,positivebuttontext, negativebuttontext, isCancelable
					} else {//Very first time request
						//Here request the permissions to denied permissions only. And your callback method is onRequestPermissionsResult()
						string[] singlePermission = new string[]{ permission };
						objNextwavePermission.Call( "requestPermissions", new object[] { singlePermission, SINGLE_PERMISSIONS_REQUEST_CODE} );
					}
				}
			} else {
				if (PlayerPrefs.HasKey (permission)) {
					PlayerPrefs.DeleteKey (permission);
					PlayerPrefs.Save ();
				}
				//Granted already
				objNative.Call ("callTheUnityMethod", callBackGameObjectName, callBackMethodName);
			}
		} else {
			//No need to get permission.
			objNative.Call ("callTheUnityMethod", callBackGameObjectName, callBackMethodName);
		}

	}


	public void onRequestPermissionsResult (string data)
	{

		//Getting requestcode from data
		string[] result = data.Split ("|" [0]);
		int requestCode = int.Parse (result[0]);

		if (requestCode == INITIAL_PERMISSIONS_REQUEST_CODE) 
		{
			if (neededExplanationIndexe == -1 && neededSettingScreenIndexe == -1) 
			{//Very first time
				moveToNextScene ();
			}
			else
			{//After explanation
				if (neededExplanationPermissions.Count > 0 && neededExplanationIndexe < neededExplanationPermissions.Count) {
					showTheExplanationOnebyOne ();
				} else if (neededSettingsScreenPermissions.Count > 0 && neededSettingScreenIndexe < neededSettingsScreenPermissions.Count) {
					showTheSettingScreenOnebyOne ();
				} else {
					moveToNextScene ();
				}
			}
		} 
		else if (requestCode == SINGLE_PERMISSIONS_REQUEST_CODE) 
		{
			if (int.Parse (result[2]) == 0) 
			{//Granted
				objNative.Call ("callTheUnityMethod", singlePermissionCallBackGameobjectName, singlePermissionCallBackMethodName);
			}
			else
			{
				//Denied (i.e -1)
				  //GoogleManagerScript.instance.LoginLoadingScreen.SetActive(false);
				  // GameManager.instance.IsCloseBtnDisabled = false;
			}
		}
	}

	public void initialRequestExplanationAlertClicked (string result)
	{
		if (result.Equals (positiveButton)) {
			//PositiveButton action
			//Now the following permission alert have 'never ask again' option also
			requestInitialPermissions (new string[]{ neededExplanationPermissions [neededExplanationIndexe] });
			neededExplanationIndexe++;
		} 
	}

	public void singleRequestExplanationAlertClicked (string result)
	{
		if (result.Equals (singlePermissionPositiveButton)) {
			//PositiveButton action
			//Now the following permission alert have 'never ask again' option also
			//Here request the permissions to denied permissions only. And your callback method is onRequestPermissionsResult()
			string[] permission = new string[]{ singlePermission };
			objNextwavePermission.Call( "requestPermissions", new object[] { permission, SINGLE_PERMISSIONS_REQUEST_CODE} );
		}
	}

	public void showTheSettingScreen (string result)
	{
		if (result.Equals (positiveButton)) {
			objNative.Call ("showTheAppDetailSettings", "this");
		} else if (result.Equals (negativeButton)) {
			//Cancel
		}
	}

	private bool isFromSettingScreen = false;

	private void showTheInitialSettingScreen (string result)
	{
		if (result.Equals (positiveButton)) {
			isFromSettingScreen = true;
			objNative.Call ("showTheAppDetailSettings", "this");
		} else if (result.Equals (negativeButton)) {
			//Cancel
			if (neededSettingScreenIndexe < neededSettingsScreenPermissions.Count) {
				showTheSettingScreenOnebyOne ();
			} else {
				moveToNextScene ();
			}
		}
	}

	public bool isPermitted (string permission)
	{
		if (isMarshMallow ()) {
			return(objNextwavePermission.Call<bool>("checkSelfPermission", permission));
		} else {
			return true;
		}
	}

	public void moveToNextScene ()
	{
		//if (PreloadPageScript != null)
		//{
		//	StartCoroutine(PreloadPageScript.LoadMenuScene());
		//}

	}
	
	void OnApplicationPause (bool pauseStatus)
	{
		//Debug.Log("OnApplicationPause ::" + pauseStatus + "::" + isFromSettingScreen + "::" + isInitialPermissionRequested);
		if (!pauseStatus) {
			if (isFromSettingScreen) {
				isFromSettingScreen = false;
				bool canMoveToNextScene = true;
				while (neededSettingScreenIndexe < neededSettingsScreenPermissions.Count) {
					//Check this permision is already granted or not
					bool isGrantedBySettingScreen = objNextwavePermission.Call<bool>("checkSelfPermission", neededSettingsScreenPermissions [neededSettingScreenIndexe]);
					if (isGrantedBySettingScreen) {
						neededSettingScreenIndexe++;
					} else {
						canMoveToNextScene = false;
						showTheSettingScreenOnebyOne ();
						break;
					}
				}
				if (canMoveToNextScene) {
					moveToNextScene ();
				}
			}else if(isInitialPermissionRequested)
			{
				//moveToNextScene();
					
			}
		}
	}
	public void ShowToast(string msg)
	{


#if UNITY_ANDROID && !UNITY_EDITOR
		if (isMarshMallow () )
		{
			if (objNative != null)
			{
				//0--top		1--bottom		2--centre
				objNative.Call ("showTheToast", msg, 2);
			}
		}
		else
		{
			GameObject prefabGO ;
			GameObject tempGO ;
			prefabGO = Resources.Load ("Prefabs/Toast")as GameObject ;
			tempGO = Instantiate (prefabGO)as GameObject ;
			tempGO.name = "Toast";
			tempGO.GetComponent <Toast > ().setMessge (msg);
		}

#endif
	}



	//=================Google Play Install Referrer API=========================
	void OnSceneLoaded(Scene aScene, LoadSceneMode aMode)
	{
		if (aScene.name == "MainMenu")
		{
			getTheInstallReferrer();
		}
	}
	//getTheInstallReferrer(final String gameObjectName, final String methodName)
	public void getTheInstallReferrer()
	{
		//Debug.Log("getTheInstallReferrer() called");
		if (PlayerPrefs.GetInt("refererPushed", 0) == 0)
		{
			if (objNextwaveActivity != null)
			{
				objNextwaveActivity.Call("getTheInstallReferrer", this.gameObject.name, "referrerReceived");
			}
		}
		else
		{
			//Debug.Log("referrer already pushed");
		}
	}

	public void referrerReceived(string referrerData)
	{
		//Debug.Log("referrerData from jar to unity ====" + referrerData);
		if (PlayerPrefs.GetInt("refererPushed", 0) == 0)
		{
			//referrerData = data = install_referrer+"|"+install_version+"|"+google_play_instant+"|"+referrer_click_timestamp_seconds+"|"+install_begin_timestamp_seconds+"|"+referrer_click_timestamp_server_seconds+"|"+install_begin_timestamp_server_seconds;
			string nwReferrerData = referrerData;
			string[] data = referrerData.Split("|"[0]);
			if (data != null && data.Length > 0)
			{
				//Nextwave Naveen needed format => install_referrer + "|" + install_begin_timestamp_server_seconds + "|" + install_version
				nwReferrerData = data[0];
				if (data.Length >= 7)
				{
					nwReferrerData += "|" + data[6];
				}
				if (data.Length >= 2)
				{
					nwReferrerData += "|" + data[1];
				}
			}
			/*  install_referrer => The referrer URL of the installed package. (string i.e "utm_source=google-play&utm_medium=organic")
                referrer_click_timestamp_seconds => The client-side timestamp, in seconds, when the referrer click happened.(long)
                install_begin_timestamp_seconds => The client-side timestamp, in seconds, when app installation began.(long)
                referrer_click_timestamp_server_seconds	=> The server-side timestamp, in seconds, when the referrer click happened.(long)
                install_begin_timestamp_server_seconds => The server-side timestamp, in seconds, when app installation began.(long)
                install_version	=> The app's version at the time when the app was first installed.(string)
                google_play_instant => Indicates whether your app's instant experience was launched within the past 7 days. (boolean i.e true/false)
            */
#if UNITY_ANDROID
			if (CONTROLLER.BASE_URL.Equals(CONTROLLER.BaseURLProd) == false)
			{
				showAlert("Referrer API!", nwReferrerData, "OK");
			}
			if (PlayerPrefs.HasKey("refererPushedToAnalytics") == false)
			{
				PlayerPrefs.SetInt("refererPushedToAnalytics", 1);
				if (CONTROLLER.BASE_URL.Equals(CONTROLLER.BaseURLProd) == false)
				{
					showTheToast("referrer pushed to analytics");
				}
			}
			PlayerPrefs.SetInt("refererPushed", 1);
#endif
		}
		else
		{
			//Debug.Log("referrer already pushed");
		}
	}
	//===================================================================

	public bool alertDialogBoxShown = false;
	public void showAlert(string title, string msg, string positiveButtonText, bool isCancellable = true)
	{
		//Default Nextwave AlertDialog theme is THEME_DEVICE_DEFAULT_LIGHT = 5
		//Default Etceteraplugin AlertDialog theme is THEME_TRADITIONAL = 1.
		//objNative.Call("showTheAlertDialog", this.gameObject.name, "alertOkClicked", title, msg, positiveButtonText, "None", true, "None");
		objNative.Call("showTheAlertDialog", this.gameObject.name, "alertOkClicked", title, msg, positiveButtonText, "None", isCancellable, "None", 1);
		alertDialogBoxShown = true;
	}

	public void alertOkClicked(string buttonText)
	{
		alertDialogBoxShown = false;
	}

	public void showTheToast(string msg)
	{
		if (objNative != null)
		{
			objNative.Call("showTheToast", msg);
		}
	}

	private bool temp = true;
	public void printTheAndroidLog(string msg) //printTheAndroidLog
	{
		if (objNextwaveActivity != null)
		{
			if (CONTROLLER.isAdminUser)
			{
				if (temp)
				{
					temp = false;
					objNextwaveActivity.Call("printTheLog", "Yes I am a admin user");
				}
			}
			objNextwaveActivity.Call("printTheLog", msg);
		}
	}

	//validateTheAppSignature
	public void abc123xyz456pqr00pqr()
	{
		if (CONTROLLER.isAdminUser)
		{
			printTheAndroidLog("Unity abc123xyz456pqr00pqr called");
		}
		//Debug.Log("===========validateTheAppSignature called========");
		if (objNextwaveActivity != null)
		{
			//wcc blitz- keystore
            //SHA-1 certificate fingerprint =9C:4D:80:34:90:AD:C9:7B:B6:1C:69:84:E0:F9:55:D2:2C:DB:84:49
            //string appSignature1 = "9 C 4 D 803490 ADC 97 BB 61 C 6984 E 0 F 955 D 22 CDB 8449";

            int ss1 = 9;
			int ss2_1 = 4;
			int ss2_2 = 803490;
			int ss3 = 97;
			int ss4 = 61;
			int ss5 = 6984;
			int ss6 = 0;
			int ss7 = 955;
			int ss8 = 22;
			int ss9 = 8449;

			StringBuilder builder = new StringBuilder();
			builder.Clear();
			builder.Append(ss1);
			builder.Append("C");
			builder.Append(ss2_1);
			builder.Append("D");
			builder.Append(ss2_2);
            builder.Append("ADC");
            builder.Append(ss3);
			builder.Append("BB");
			builder.Append(ss4);
			builder.Append("C");
			builder.Append(ss5);
			builder.Append("E");
			builder.Append(ss6);
			builder.Append("F");
			builder.Append(ss7);
			builder.Append("D");
			builder.Append(ss8);
			builder.Append("CDB");
			builder.Append(ss9);
			string signature1 = builder.ToString();


			if (CONTROLLER.isAdminUser)
			{
				printTheAndroidLog("signature1:" + signature1);
			}

            //wcc blitz - google play
            //Google Play SHA - 1 : 4E:97:43:EA:EC:F1:47:0D:20:4A:9A:59:4C:ED:39:0E:BE:FB:64:EE
            //string appSignature2 = "4 E 9743 EAECF 1470 D 204 A 9 A 594 CED 390 EBEFB 64 EE";

            ss1 = 4;
			ss2_1 = 9743;
			ss3 = 1470;
			ss4 = 204;
			ss5 = 9;
			ss6 = 594;
			ss7 = 390;
			ss8 = 64;

			builder.Clear();
			builder.Append(ss1);
			builder.Append("E");
			builder.Append(ss2_1);
			builder.Append("EAECF");
			builder.Append(ss3);
			builder.Append("D");
			builder.Append(ss4);
			builder.Append("A");
			builder.Append(ss5);
			builder.Append("A");
			builder.Append(ss6);
			builder.Append("CED");
			builder.Append(ss7);
			builder.Append("EBEFB");
			builder.Append(ss8);
			builder.Append("EE");

			string signature2 = builder.ToString();

			if (CONTROLLER.isAdminUser)
			{
				printTheAndroidLog("signature2:" + signature2);
			}

            //wcc blitz - google play Internal app sharing
            //Internal App sharing SHA - 1 : 8F:D2:DE:35:F0:7E:37:35:5C:7C:A2:48:F7:4C:C4:56:ED:62:0F:0E
            //string appSignature3 = "8 FD 2 DE 35 F 0 7 E 37355 C 7 CA 248 F 74 CC 456 ED 620 F 0 E";

            ss1 = 8;
            ss2_1 = 2;
            ss3 = 35;
            ss4 = 0;
            ss5 = 7;
            ss6 = 37355;
            ss7 = 248;
            ss8 = 74;
            ss9 = 456;
            ss2_2 = 620;

            builder.Clear();
            builder.Append(ss1);
            builder.Append("FD");
            builder.Append(ss2_1);
            builder.Append("DE");
            builder.Append(ss3);
            builder.Append("F");
            builder.Append(ss4);
            builder.Append(ss5);
            builder.Append("E");
            builder.Append(ss6);
            builder.Append("C");
            builder.Append(ss5);
            builder.Append("CA");
            builder.Append(ss7);
            builder.Append("F");
            builder.Append(ss8);
            builder.Append("CC");
            builder.Append(ss9);
            builder.Append("ED");
            builder.Append(ss2_2);
            builder.Append("F");
            builder.Append(ss4);
            builder.Append("E");

            string signature3 = builder.ToString();

            if (CONTROLLER.isAdminUser)
            {
                printTheAndroidLog("signature3:" + signature3);
            }

            ObscuredPrefs.SetInt("impk", 1);//called to check this is mod apk or not
			objNextwaveActivity.Call("abc123xyz456pqr00pqr", signature1, signature2,signature3, CONTROLLER.isAdminUser.ToString(), this.gameObject.name, "nwhi456dqprt837b");
			if (CONTROLLER.isAdminUser)
			{
				printTheAndroidLog("objNextwaveActivity.Call(abc123xyz456pqr00pqr) called");
			}
		}
	}

	public void nwhi456dqprt837b(string data)
	{
		if (CONTROLLER.isAdminUser)
		{
			printTheAndroidLog("nwhi456dqprt837b called:" + data);
		}
		else
		{
			printTheAndroidLog("k124abcd454nldfal89 called:" + data);
		}

		ObscuredPrefs.SetInt("impk", 2); //Response came from jar to this is mod apk or not
										 //response data may be "NoData" or "true" or "false"
		if (data.Equals("false"))
		{
			//FirebaseAnalyticsManager.instance.logEvent("impk");

			ObscuredPrefs.SetInt("impk", 3); //Yes this is mod apk
											 //This is Mod Apk
			if (CONTROLLER.BASE_URL.Equals(CONTROLLER.BaseURLProd) == false)
			{
				showAlert("ValidateTheAppSignature", "App signature is mismatched!", "OK");
			}
			else
			{
				//Here we can make decision
				//But now the decision is in /users/details? API (UserDataSynchroniser.cs)
			}
		}
		else
		{
			if (CONTROLLER.BASE_URL.Equals(CONTROLLER.BaseURLProd) == false)
			{
				showAlert("ValidateTheAppSignature", data, "OK");
			}
		}

		if (CONTROLLER.isAdminUser)
		{
			printTheAndroidLog("ObscuredPrefs.GetInt(impk):" + ObscuredPrefs.GetInt("impk"));
		}

		CheckToShowModApkBlocker();
	}

	public void CheckToShowModApkBlocker()
	{

		int impkValue = ObscuredPrefs.GetInt("impk", 0); //Yes this is mod apk
														 //Debug.LogError("MOD 3333---" + impkValue);

		if (CONTROLLER.isAdminUser)
		{
			printTheAndroidLog("CheckToShowModApkBlocker called:" + impkValue);
		}

		if (impkValue == 3)
		{
			showTheAlertDialogToExit("INFORMATION", "YOU ARE USING A MOD VERSION OF THE GAME,SO YOU ARE NOT ALLOWED TO PLAY,TRY INSTALLING FROM GOOGLE PLAY STORE", "EXIT");
		}
	}

	private void checkThisIsAdminUser()
	{
		if (ObscuredPrefs.HasKey("isAdminUser"))
		{
			setThisIsAdminUser("yes");
		}
	}

	//It called from jar also. So don't change this function name
	public void setThisIsAdminUser(string data)
	{
		CONTROLLER.isAdminUser = true;
		Debug.unityLogger.logEnabled = true;
		ObscuredPrefs.SetInt("isAdminUser", 1);
		printTheAndroidLog("setThisIsAdminUser called:" + data);
		showTheToast("Now you are a admin user!");
	}

	//response from validateTheAppSignature
	

	public void showTheAlertDialogToExit(string title, string msg, string positiveButtonText, bool isCancellable = false)
	{
		if (objNative != null)
		{
			objNative.Call("showTheAlertDialog", this.gameObject.name, "exitTheGame", title, msg, positiveButtonText, "None", isCancellable, "None", 1);
		}
	}

	public void exitTheGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
	}
}