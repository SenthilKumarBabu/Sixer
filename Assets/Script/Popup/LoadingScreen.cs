using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance;
    public GameObject holder;
    public Text loadingtext;

    public Text LoadingScreenCountdown;
    [HideInInspector]
    public float fLoadingCountDownStrtTime, fLoadingCountDownTotTime = 30f;

    public Image FillingAnim;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void Show(string LoadingText = "Loading...Please Wait...")
    {
        holder.SetActive(true);
        loadingtext.text = LoadingText;
        LoadingScreenCountdown.gameObject.SetActive(false);

        endValue = 1;
        FillingAnim.fillClockwise = true;
        FillingAnim.DOFillAmount(0, 0f);
        startFillAnimation();
    }

    public void Update()
    {
        if (LoadingScreenCountdown.gameObject.activeSelf)
        {
            int val = (int)(fLoadingCountDownTotTime - (Time.time - fLoadingCountDownStrtTime));
            if (val <= 0)
            {
                Hide();
            }

            if (val < 0)
                val = 0;
            if (val < fLoadingCountDownTotTime - 4f)
                LoadingScreenCountdown.text = val.ToString();
        }
    }

    public void Hide()
    {
        LoadingScreenCountdown.gameObject.SetActive(false);
        holder.SetActive(false);
    }

    float endValue;
    void startFillAnimation()
    {
        FillingAnim.DOFillAmount(endValue, 1.5f).OnComplete(ResetAndStart).SetUpdate(true).SetEase(Ease.Linear);
    }

    void ResetAndStart()
    {
        if (endValue == 1)
            endValue = 0f;
        else
            endValue = 1;

        if (FillingAnim.fillClockwise == true)
            FillingAnim.fillClockwise = false;
        else
            FillingAnim.fillClockwise = true;

        DOTween.Kill(FillingAnim);
        startFillAnimation();
    }
}
