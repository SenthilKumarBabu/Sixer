using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOutExtensions:MonoBehaviour
{
    #region FADE IN FADE OUT
    public  Coroutine FadeInOut;
    public void StopFadeInOutCoRoutine()
    {
        if (FadeInOut != null)
            StopCoroutine(FadeInOut);
    }

    public IEnumerator FadeInSound(AudioSource audioSource, float endValue, float stepvalue = 0.2f)
    {
        while (audioSource.volume < endValue)
        {
            audioSource.volume += (stepvalue * Time.deltaTime);
            yield return null;
        }

        yield break;
    }
    public IEnumerator FadeOutSound(AudioSource audioSource, float EndValue, float stepvalue = 0.2f)
    {
        while (audioSource.volume > EndValue)
        {
            audioSource.volume -= (stepvalue * Time.deltaTime);
            yield return null;
        }
        yield break;
    }
    #endregion
}
