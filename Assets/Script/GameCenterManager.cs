#if UNITY_IOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine .SocialPlatforms .GameCenter ;

public class GameCenterManager : MonoBehaviour 
{

	public static  GameCenterManager instance;

	[HideInInspector]
	public bool isSignedIn;
	[HideInInspector]
	public bool _IsNetworkConnected;
[HideInInspector]
	float timeOut = 10;
[HideInInspector]
	bool LoginSuccess;
[HideInInspector]
	string playerName;
[HideInInspector]
	string playerID;
[HideInInspector]
	string  emailID;


	// Use this for initialization
	void Start () 
	{
		instance = this;

		/*if(!CONTROLLER .bGooglePlayLoginSuccess && !PlayerPrefs .HasKey ("firstsignin") )
		{
			playerName = "";
			playerID = "";
			emailID ="";
			isSignedIn = false;
			LoginSuccess = false;
			PlayerPrefs.SetInt ("firstsignin", 1);  

            Login ();
		}*/
	}

	public void Login()
	{
		StartCoroutine (GameCentreLogin());
	}


    IEnumerator GameCentreLogin()
    {

        yield return StartCoroutine(CheckInternetConnection());
        if (_IsNetworkConnected)
        {
            isSignedIn = false;
            //Debug.Log("Authenticated value before: " + Social.localUser.authenticated);
            Social.localUser.Authenticate((bool success) => 
            {

                    if (success)
                    {
                        //Debug.Log("Success");

                        isSignedIn = CONTROLLER.bGooglePlayLoginSuccess = true;
                        playerName = CONTROLLER.profilePlayerName = Social.localUser.userName;
                        playerID = CONTROLLER.profilePlayerID = Social.localUser.id;
                        emailID = CONTROLLER.profileEmailID = "";
                        CONTROLLER.profilePicURL = "";

                        //Debug.Log(" Name: " + playerName + " id: " + playerID + " email: " + emailID);

                        if (CONTROLLER.profileEmailID == null || CONTROLLER.profileEmailID == string.Empty)
                        {
                            CONTROLLER.profileEmailID = "";
                        }

                        PlayerPrefsManager.saveuserGooglePlayStatus();

                        if (CONTROLLER.bGooglePlayLoginSuccess)
                            GameModeSelector._instance.SignoutBut.text = "Sign out";
                        else
                            GameModeSelector._instance.SignoutBut.text = "Sign In";

                        //if (InterfaceHandler._instance != null)
                        //    InterfaceHandler._instance.AccountCreate();
                        //else
                            //Debug.Log("===account create req gopi interface null ");

                    //if ( InterfaceHandler._instance != null)
                    //{
                    //    //Debug.Log(" manual login game center  failed display");
                    //    InterfaceHandler._instance.HideSignInfailedPopup();
                    //}

                    }
                    else
                    {

                        //Debug.Log("unsuccessful");

						Popup.instance.showGenericPopup("", " Oops..You've failed to sign in. Please Enable GameCenter from Settings!");


                        if (Achievements.instance != null)
                            Achievements.instance.GoogleplaySignIn(1);
                    }
                });

            //Debug.Log(" end of Game center authentication ");

            if(!isSignedIn && InterfaceHandler._instance != null && CONTROLLER.bShowLoginFailedPopup)
            {
                //Debug.Log(" manual login game center  failed display");
               // InterfaceHandler._instance.ShowSigninfailedPopup();
            }
        }
        else
        {
            //Debug.Log("Please check internet connection.");
        }
    }


	/*IEnumerator GameCentreLogin ()
	{
		
		yield return StartCoroutine (CheckInternetConnection ());
		if (_IsNetworkConnected) {

            //Debug.Log("Authenticate called");
            //Debug.Log("Authenticated value before: " + Social.localUser.authenticated);
			if (!Social.localUser.authenticated ) 
			{				
				Social.localUser.Authenticate ((bool success) => {

					if (success) 
					{
                        //Debug.Log("Success");

                        isSignedIn = CONTROLLER.bGooglePlayLoginSuccess = true;
                        playerName = CONTROLLER.profilePlayerName = Social.localUser.userName;
                        playerID = CONTROLLER.profilePlayerID = Social.localUser.id;
                        emailID = CONTROLLER.profileEmailID = "";              
                        CONTROLLER.profilePicURL = "";                                   
                      
                        //Debug.Log(" Name: " + playerName + " id: " + playerID + " email: " + emailID);

                        if (CONTROLLER.profileEmailID == null || CONTROLLER.profileEmailID == string.Empty)
                        {
                            CONTROLLER.profileEmailID = "";
                        }

                        PlayerPrefsManager.saveuserGooglePlayStatus();

                        if (CONTROLLER.bGooglePlayLoginSuccess)
                            GameModeSelector._instance.SignoutBut.text = "Sign out";
                        else
                            GameModeSelector._instance.SignoutBut.text = "Sign In";

                         //if(InterfaceHandler ._instance !=null)
                         //     InterfaceHandler ._instance .AccountCreate (); 
                         // else
                              //Debug.Log ("===account create req gopi interface null "); 						

					} else {
                        
                        //Debug.Log("unsuccessful");

						//if(InterfaceHandler ._instance !=null && CONTROLLER .bShowLoginFailedPopup)
						//	InterfaceHandler ._instance.ShowSigninfailedPopup (); 

						if(Achievements .instance !=null)
							Achievements.instance.GoogleplaySignIn (1);
					}
				});

			} else {
				//Debug.Log ("Already authenticated.");
			}
		} else {
			//Debug.Log ("Please check internet connection.");
		}
	}*/


	public IEnumerator CheckInternetConnection ()
	{
		_IsNetworkConnected = false;
		WWW WWWObject = new WWW ("https://clients3.google.com/generate_204");
		float timer = 0; 
		bool failed = false;
		while (!WWWObject.isDone) {
			if (timer > timeOut) { 
				failed = true; 
				break; 
			}
			timer += Time.deltaTime;
			yield return null;
		} 
		//Debug.Log ("CheckInternetConnection1");
		if (failed) {
			//Debug.Log ("CheckInternetConnection2");
			yield return false;
		}
		yield return WWWObject;

		if (string.IsNullOrEmpty (WWWObject.error)) {
			//Debug.Log ("CheckInternetConnection3");
			_IsNetworkConnected = true;

		}
	}

	public  IEnumerator GetProfilePicture(string url) 
	{
		//Debug.Log ("===get profile pic "+url ); 
		WWW www = new WWW(url);
		yield return www;

		Texture2D tex = www.texture;
		ImageSaver.SaveTexture (tex, "Googleplayprofpic");
		GameModeSelector ._instance .userProfilePic.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
		GameModeSelector._instance.userProfilePic.enabled = true;
	}
}
#endif
