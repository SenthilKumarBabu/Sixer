using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BatLoopAnimation : MonoBehaviour
{
    public float startDelay;
    public float delaybtwnAnim;
    public float animDuration;
    public string animName;
    private void OnEnable()
    {
        InvokeRepeating("playAnim", startDelay, animDuration+delaybtwnAnim);
    }

    void playAnim()
    {
        DOTween.Rewind(animName);
        DOTween.Play(animName);
    }

    private void OnDisable()
    {
        CancelInvoke("playAnim");
    }
}
