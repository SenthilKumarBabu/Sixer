using UnityEngine;
using UnityEngine.UI;

public class BattingInterface : MonoBehaviour
{
    public GroundController groundcontrollerScript;
    public GameObject Holder;

    [Header("Batting Timing Meter")]
    public GameObject TimingMeter;
    public GameObject BattingMeterFiller;
    public GameObject battingTimingNeedle;
    public Text battingTimingNeedleText;

    void Start()
    {
        Holder.SetActive(false);
    }

    public void ShowMe()
    {
        Holder.SetActive(true);
        TimingMeter.SetActive(true);
    }
    public void HideMe()
    {
        Holder.SetActive(false);
    }

}
