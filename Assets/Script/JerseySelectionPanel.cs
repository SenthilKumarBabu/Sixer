using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JerseySelectionPanel : MonoBehaviour
{
    public Sprite[] JerseyImages;
    public Image[] colorPalette;
    public Image[] selectedImage;

    public Image jerseyImage;

    private void Awake()
    {
        jerseyImage.sprite = JerseyImages[CONTROLLER.JerseyIDX];
        this.gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        CONTROLLER.CurrentPage = "jerseyselection";
        ShowSelectedImage();

        jerseyImage.sprite = JerseyImages[CONTROLLER.JerseyIDX];
    }

    private void ShowSelectedImage()
    {
        for (int i = 0; i < selectedImage.Length; i++)
        {
            if (i == CONTROLLER.JerseyIDX)
                selectedImage[i].enabled = true;
            else
                selectedImage[i].enabled = false;
        }
    }
    public void ButtonClick(int idx)
    {
        CONTROLLER.JerseyIDX = idx;
        PlayerPrefs.SetInt("jerseyidx",idx);
        PlayerPrefs.Save();
        jerseyImage.sprite = JerseyImages[CONTROLLER.JerseyIDX];
        ShowSelectedImage();
        close();
    }
    public void close()
    {
        if (GameModeSelector._instance != null)
        {
            GameModeSelector._instance.close(5);
            CONTROLLER.CurrentPage = "splashpage";
        }
    }
}
