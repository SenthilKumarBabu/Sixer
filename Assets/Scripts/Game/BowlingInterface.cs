using UnityEngine;

public class BowlingInterface : MonoBehaviour
{
    public GroundController groundcontrollerScript;
    public GameObject Holder;
    public GameObject BowlButton;

    void Start()
    {
        Holder.SetActive(false);
    }

    public void ShowMe()
    {
        Holder.SetActive(true);
        setBowlButtonState(true);
    }
    public void HideMe()
    {
        setBowlButtonState(false);
        Holder.SetActive(false);
    }
    void setBowlButtonState(bool state)
    {
        BowlButton.SetActive(state);
    }
    public void ButtonClickEvent()
    {
        HideMe();
        groundcontrollerScript.PlayBowlerBowlingAnimFromBowlingInterface();
    }
}
