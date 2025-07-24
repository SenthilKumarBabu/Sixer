using UnityEngine;
using System.Collections;

/// <summary>
/// Network manager used to check current Network status.
/// Use CheckInternetConnection to start checking the internet.
/// The state of network connectivity is stored in _IsNetworkConnected
/// </summary>
public class NetworkManager : MonoBehaviour 
{
	public static NetworkManager Instance
	{
		get
		{ 
			return _Instance;
		}
	}

	public bool IsNetworkConnected
	{
		get
		{ 
			return _IsNetworkConnected;
		}
	}

	public AndroidJavaObject objNative = null;

	private static NetworkManager _Instance;
	private bool _IsNetworkConnected = false;

	void Awake ()
	{
		if (_Instance == null)
		{
			_Instance = this;
			initTheNativeAndroid ();
		}
	}
	float timeOut = 10;
	public IEnumerator CheckInternetConnection ()
	{
		_IsNetworkConnected = false;
#if UNITY_ANDROID && !UNITY_EDITOR
//		ServerManager.Instance.uiText.text = "INTERNET CONNECTION ON "+objNative.Call<string> ("getNetworkClass");
#endif
		WWW WWWObject = new WWW ("https://clients3.google.com/generate_204");

		float timer = 0; 
		bool failed = false;
		
		while(!WWWObject.isDone){
			if(timer > timeOut){ failed = true; break; }
			timer += Time.deltaTime;
			yield return null;
		}
		if(failed)
		{
//			ServerManager.Instance.uiText.text = "TIME OUT ";
 			yield  return false;}//WWWObject.Dispose();

		yield return WWWObject;

		if (string.IsNullOrEmpty (WWWObject.error))
		{
//			if (WWWObject.text == string.Empty)
//			{
				_IsNetworkConnected = true;
//			}
//			ServerManager.Instance.uiText.text = "----";
		}

	}

	public void initTheNativeAndroid ()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR 
		if (objNative == null) {
			// First, obtain the current activity context
			AndroidJavaClass actClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			AndroidJavaObject  playerActivityContext = actClass.GetStatic<AndroidJavaObject> ("currentActivity");
			
			// Pass the context to a newly instantiated NativeAndroid object
			AndroidJavaClass pluginClass = new AndroidJavaClass ("com.nextwave.android.NativeAndroid");
			if (pluginClass != null) {
				objNative = pluginClass.CallStatic<AndroidJavaObject> ("instance");
				objNative.Call ("setContext", playerActivityContext);
				objNative.Call ("setActivity", playerActivityContext);
				//objNative.Call ("showTheToast", "welcome to all");
			}
		}
		#endif
	}

	public bool CheckInternetUsingNative ()
	{
		if (objNative != null) {
			bool result = objNative.Call<bool> ("isInternetOn");
			if (result == false) {
				//obj_PopupScript.showThePopup ("Please check your internet connection.");
			}
			return result; 
		} else {
			#if UNITY_EDITOR
			return true;
			#endif
			return false;
		}
	}
}
