using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChallengeManager : MonoBehaviour 
{

    [Header("New Components")]
    public Image BGimage;
    public Image RightSideImage;
    public GameObject RightSideBat;
    public Image LeftChallengeBG;
    public Text ChallengeNumber;
    public Text LevelHeading;
    public Text BowlingType;

    [Header("Animation")]
    public GameObject maskHolder;
    public Image TopShine;
    public Image BottomShine;
}
