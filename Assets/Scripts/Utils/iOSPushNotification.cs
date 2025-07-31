using UnityEngine;
using System.Collections;
using Prime31;

public class iOSPushNotification : MonoBehaviour
{

#if UNITY_IOS
	private string deviceToken = string.Empty;
	private int _currentSavedVersion = 0;
	public static int currentVersion;
	private string deviceID = string.Empty;
	private string targetPlatform = "ios";
	
	protected void Awake ()
	{
		DontDestroyOnLoad (this.gameObject);
		EtceteraManager.remoteRegistrationSucceededEvent += remoteRegistrationSucceededEvent;
		EtceteraManager.remoteRegistrationFailedEvent -= remoteRegistrationFailed;
	}
	
	protected void Start ()
	{
		deviceID = SystemInfo.deviceUniqueIdentifier;
		currentVersion = AppInfoController.BuildNumber;
		//Debug.Log ("build number  "+ AppInfoController.BuildNumber );
		//Debug.Log ("deviceID :: "+deviceID);
		CheckForVersion ();
	}
	
	void remoteRegistrationSucceededEvent (string _deviceToken)
	{
		deviceToken = _deviceToken;
		//Debug.Log ("_deviceToken :: "+_deviceToken);
		if (deviceToken != string.Empty && _deviceToken != null)
		{
			StartCoroutine (RegisterUserDevice ());
		}
	}
	
	void remoteRegistrationFailed (string error)
	{
		//Debug.Log ("remoteRegistrationFailed : " + error);
	}
	
	private void CheckForVersion ()
	{
		if (PlayerPrefs.HasKey ("cv"))
		{
			_currentSavedVersion = PlayerPrefs.GetInt ("cv");
			PlayerPrefs.SetInt ("previousversion", _currentSavedVersion);
		}
		//Debug.Log ("_currentSavedVersion :: "+_currentSavedVersion);
		//Debug.Log ("currentVersion :: "+currentVersion);
		if (_currentSavedVersion != currentVersion)
		{
			EtceteraBinding.registerForRemoteNotifications (P31RemoteNotificationType.Alert | P31RemoteNotificationType.Badge | P31RemoteNotificationType.Sound);
		}
	}
	
	private IEnumerator RegisterUserDevice ()
	{
		WWWForm form = new WWWForm ();
		form.AddField("action", "registeruserdevice");
		form.AddField("buildnum", currentVersion);
		form.AddField("deviceregistrationid", deviceToken);
		form.AddField("deviceid", deviceID);
		form.AddField("platform", targetPlatform);

		WWW download = new WWW(CONTROLLER.BASE_URL, form);
		yield return download;

		if (!string.IsNullOrEmpty(download.error))
		{
			//Debug.Log ("RegisterUserDevice Failed :: "+download.error);
		}
		else
		{
			SimpleJSON.JSONNode node = SimpleJSON.JSONNode.Parse (download.text);
			SaveToPlayerPrefs (node);
			//Debug.Log ("registeruserdevice response :: "+download.text);
		}
	}
	
	private static void SaveToPlayerPrefs (SimpleJSON.JSONNode node)
	{
		if ("" + node ["registeruserdevice"] ["status"] == "1")
		{
			PlayerPrefs.SetInt ("cv", currentVersion);
		}
	}
#endif
}