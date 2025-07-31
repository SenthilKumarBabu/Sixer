using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignInPanel : MonoBehaviour
{
    public static SignInPanel instance;
    public GameObject Holder;
    public GameObject GoogleLoginButton;
    public GameObject AppleLoginButton;
    void Awake()
    {
        instance = this;
        Holder.SetActive(false);
#if UNITY_ANDROID || UNITY_EDITOR
        GoogleLoginButton.SetActive(true);
        AppleLoginButton.SetActive(false);
#elif UNITY_IPHONE || UNITY_IOS
        GoogleLoginButton.SetActive(false);
        AppleLoginButton.SetActive(true);
#endif
    }

    public void Show()
    {
        Holder.SetActive(true) ;
        CONTROLLER.CurrentPage = "splashpage";
    }

    public void Hide()
    {
        Holder.SetActive(false);
    }
    public void OnButtonClicked(int idx)
    {
        AudioPlayer.instance.PlayButtonSnd();

        Hide();
        GameModeSelector._instance.ShowLandingPage(true);
        return;

        if (AdIntegrate.instance.checkTheInternet())
        {
            switch (idx)
            {
                case 0: //google login
#if UNITY_ANDROID
                  //  GoogleManagerScript.instance.signIn();
#endif
                    break;
                case 1: //apple login
#if UNITY_IPHONE || UNITY_IOS
                GameCenterManager.instance.Login();
#endif
                    break;
                case 2: //guest login
                    CONTROLLER.LoginType = 2;
                    CricMinisWebRequest.instance.CheckForLogin();
                    break;
            }
        }
        else
        {
            Popup.instance.ShowNoInternetPopup();
        }        
    }
}
