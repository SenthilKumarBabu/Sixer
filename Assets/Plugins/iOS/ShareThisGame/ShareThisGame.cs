using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;


public class ShareThisGame
{	
	#if UNITY_IOS
	[DllImport ("__Internal")]
	private static extern void sampleTextMethod (string message);

	[DllImport ("__Internal")]
	private static extern string getNetworkConnectionType ();

	[DllImport ("__Internal")]
	private static extern bool isConnected ();

	[DllImport ("__Internal")]
	private static extern void shareImageWithMessage(string message, string path);

	[DllImport ("__Internal")]
	private static extern void presentSplashAd();

	[DllImport ("__Internal")]
	private static extern void openAppReview (int appId);

	[DllImport ("__Internal")]
	private static extern void checkAnyNewIntent ();


	public static void sendTextWithOutPath (string message)
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer)
			sampleTextMethod (message);
		
	}
	public static string getNetworkModeFromNative()
	{
		return getNetworkConnectionType ();
		//Debug.Log ("Networktype ::" + getNetworkConnectionType ());
	}
	public static bool checkTheInternet ()
	{
		return isConnected ();
	}

	public static void shareImageWithMsg(string message,string path)
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer)
			shareImageWithMessage (message,path);
	}

	public static void presentInMobiSplashAd()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
			presentSplashAd ();
	}

	public static void rateUs()
	{
		openAppReview (1105859833);
	}

	public static void checkTheAnyNewIntent ()
	{
		checkAnyNewIntent ();
	}

	#endif
}
