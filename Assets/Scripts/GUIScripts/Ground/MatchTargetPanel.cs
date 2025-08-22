using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class MatchTargetPanel : MonoBehaviour
{
    public GameObject Holder;


    public Text ScoreText;

    protected void Start()
    {
        BattingScoreCard.instance.HideMe();
        ShowMe();
    }

    public void ShowMe()
    {
        Holder.SetActive(true);
        CONTROLLER.TargetToChase = CONTROLLER.currentMatchScores + 1;
        ScoreText.text = (CONTROLLER.TargetToChase).ToString();
    }

    public void menuClicked(int index)
    {
        if (index == 0) 
        {
            StartCoroutine(GameModel.instance.GameQuitted());
        }
        else if (index == 1)    //continue
        {
            AudioPlayer.instance.PlayTheIntroSound();
            CONTROLLER.NewInnings = true;
            CONTROLLER.currentInnings = 1;
            CONTROLLER.InningsCompleted = false;

            GroundController.instance.ResetAll();
            GameModel.instance.ResetCurrentMatchDetails();
            GameModel.instance.ResetVariables();
            GameModel.instance.ResetAllLocalVariables();

            GameModel.instance.ShowIntroAnimation();

            //GroundController.instance.StartToBowl();
            HideMe();
        }

    }

    private void HideMe()
    {
        Destroy(this.gameObject);
        Resources.UnloadUnusedAssets();
    }

}
