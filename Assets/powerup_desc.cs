using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class powerup_desc : MonoBehaviour
{
    public Image desc_card;
    public void fade_image()
    {
        desc_card.DOFade(1f, .5f);
    } 
}
