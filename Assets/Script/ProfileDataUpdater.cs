using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileDataUpdater : MonoBehaviour
{
    public Text userName;
    public Image userProfilePic;
    public Sprite defaultProfilePic;

    private void Start()
    {
      userName.text = CONTROLLER.UserName;
#if UNITY_ANDROID
        if (CONTROLLER.LoginType==1 && CONTROLLER.bGooglePlayLoginSuccess && PlayerPrefs.HasKey("Googleplayprofpic"))
        {
            userProfilePic.sprite = Sprite.Create(ImageSaver.RetriveTexture("Googleplayprofpic"), new Rect(0, 0, PlayerPrefs.GetInt("Googleplayprofpic_w"), PlayerPrefs.GetInt("Googleplayprofpic_h")), new Vector2(0, 0));
        }
        else
            userProfilePic.sprite = defaultProfilePic;
#endif

#if UNITY_IOS
        userProfilePic.sprite = defaultProfilePic;
#endif
    }
}
