using UnityEngine;
using System.Collections;
using Prime31;
using UnityEngine .UI;

public class About : MonoBehaviour
{
	public Text  versionNumber;
	public GameObject textView;
	private Vector3 startPos;
	public ScrollRect creditsScrollView;

	void Awake ()
	{
		versionNumber.text = "VERSION ";
	}

	void Start () 
	{
		startPos = textView.transform.position;
	}

	void OnEnable()
	{
		creditsScrollView.normalizedPosition = new Vector2(creditsScrollView.normalizedPosition.x,1f);
	}

	void Update ()
	{
		if(!Input.GetMouseButton(0))
		{
			float ypos = creditsScrollView.normalizedPosition.y - (Time.unscaledDeltaTime * 0.05f);
			if(ypos < 0f)
			{
				ypos = 1f;
			}
			creditsScrollView.normalizedPosition = new Vector2(creditsScrollView.normalizedPosition.x, ypos);
		}

	}

	public void ButtonEvent (int value)
	{
		AudioPlayer.instance.PlayButtonSnd();
		if (value ==1)	
		{
			Application.OpenURL (CONTROLLER.PP_Link);
		}
		else if(value ==2)	 
		{
			Application.OpenURL (CONTROLLER.TC_Link);
		}
		else if (value ==3)	 
		{
			string _to = "cricket@nextwave.in";
			
			string _version = "Version:";
			string _subject = CONTROLLER.AppName+"-"+_version+"-Feedback-"+ CONTROLLER.Platform;
			string _body = "";

			//if (Application.isEditor == true)
			//{
				Application.OpenURL("mailto:cricket@nextwave.in?subject="+_subject+"&body=Hi");
			/*}
			else
			{
				#if UNITY_ANDROID
				EtceteraAndroid.showEmailComposer (_to, _subject, _body, false);
				#elif UNITY_IOS
				if (EtceteraBinding.isEmailAvailable () == true)
				{
					EtceteraBinding.showMailComposer (_to, _subject, _body, true);
				}
				else
				{
					EtceteraBinding.showAlertWithTitleMessageAndButton ("", "Please configure your mail id", "OK");
				}
				#elif UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
				string _text = "mailto:" + _to + "?subject=" + _subject + "&body=" + _body;
				Application.ExternalCall ("OpenInNewTab", _text); //This is called from html file for chrome store.
				#endif
			}*/
		}
		else if (value ==4)	 
		{
		}
	}
}
