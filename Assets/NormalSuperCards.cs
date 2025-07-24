using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using DG.Tweening;
using System.Security.Cryptography.X509Certificates;

[System.Serializable]
public class NormalSuperCards:MonoBehaviour
{
    public GameObject vibe_Anim;
    public RectTransform pick_tutorial;
    public Text TotalGrade;
    public Text PlayerName;
    public Image PlayerImage;
    public Image PlayerCountry;
    public Image PlayerType_Img;
    //public Text button1Text;
    //public Text button2Text;
    //public Text button3Text;
    //public Text button4Text;
    //public Text button5Text;
    public Text[] buttonText;
    //public Text button1_Label;
    //public Text button2_Label;
    //public Text button3_Label;
    //public Text button4_Label;
    //public Text button5_Label;
    public Text[] button_Label;
    //public Text Runs_Label;
    //public Text _4s_Label;
    //public Text _6s_Label;
    //public Text _50s_Label;
    //public Text StrikeRate_Label;
    public Text WinnerText;
    public CardValues thiscardvalue = new CardValues();
    private int SelectedValue;
    public Button[] AllTheButtons;
    public Transform SuperCardHolder;
    public RectTransform FrontSideRect;
    public RectTransform BackSideRect;
    public Image PlayercardImage;
    
    private int listIndex;
    public Sprite[] button_select;
    private int prevSelection_ForHighlight = -1;


    public bool isAllRounder; // 0 = not allrounder, 1 = all rounder - Batsman, 2 = allrounder - Bowler
    public byte playerType;//0- batsman, 1- bowler

    //private Outline selectionOutline;

    private bool disableSelection;

    public void Reset()
    {
        SelectedValue = -1;
        //if (selectionOutline != null)
        //{
        //    selectionOutline.enabled = false;
        //}
    }

    public void NormalSuperCardFiller_BatsmanStats(string _playername,float _runs,float _4s,float _6s,float _50s,float strikeRate, Sprite playerSprite, Sprite playerCountrySprite, Sprite playerTypeSprite, int _listIndex)
    {
        PlayerImage.sprite = playerSprite; 
        PlayerCountry.sprite = playerCountrySprite;
        PlayerType_Img.sprite = playerTypeSprite;
        //thiscardvalue.TotalGrade = _tg;
        thiscardvalue.PlayerName = _playername;
        thiscardvalue.Runs = _runs;
        thiscardvalue._4s = _4s;
        thiscardvalue._6s = _6s;
        thiscardvalue._50s = _50s;
        thiscardvalue.StrikeRate = strikeRate;

        //TotalGrade.text = "" + thiscardvalue.TotalGrade;
        PlayerName.text = thiscardvalue.PlayerName;
        buttonText[0].text = "" + thiscardvalue.Runs;
        buttonText[1].text = "" + thiscardvalue._4s;
        buttonText[2].text = "" + thiscardvalue._6s;
        buttonText[3].text = "" + thiscardvalue._50s;
        buttonText[4].text = "" + thiscardvalue.StrikeRate;

        button_Label[0].text = "RUNS";
        button_Label[1].text = "4s";
        button_Label[2].text = "6s";
        button_Label[3].text = "50s";
        button_Label[4].text = "SR";

        listIndex = _listIndex;

    }

    public void UpdateBatsmanStatsText(string _playername, float _runs, float _4s, float _6s, float _50s, float strikeRate, Sprite playerSprite, Sprite playerCountrySprite,Sprite playerTypeSprite, int _listIndex)
    {
        PlayerImage.sprite = playerSprite;
        PlayerCountry.sprite = playerCountrySprite;
        PlayerType_Img.sprite = playerTypeSprite;
        PlayerName.text = _playername;
        buttonText[0].text = "" + _runs;
        buttonText[1].text = "" + _4s;
        buttonText[2].text = "" + _6s;
        buttonText[3].text = "" + _50s;
        buttonText[4].text = "" + strikeRate;

        button_Label[0].text = "RUNS";
        button_Label[1].text = "4s";
        button_Label[2].text = "6s";
        button_Label[3].text = "50s";
        button_Label[4].text = "SR";
    }

    public void NormalSuperCardFiller_BowlerStats(string _playername, float _matches, float _overs, float _wickets, float _maidens, float _economy, Sprite playerSprite, Sprite playerCountrySprite, Sprite playerTypeSprite, int _listIndex)
    {
        PlayerImage.sprite = playerSprite;
        PlayerCountry.sprite = playerCountrySprite;
        PlayerType_Img.sprite = playerTypeSprite;
        //thiscardvalue.TotalGrade = _tg;
        thiscardvalue.PlayerName = _playername;
        thiscardvalue.matches = _matches;
        thiscardvalue.overs = _overs;
        thiscardvalue.wickets = _wickets;
        thiscardvalue.maidens = _maidens;
        thiscardvalue.economy = _economy;

        //TotalGrade.text = "" + thiscardvalue.TotalGrade;
        PlayerName.text = thiscardvalue.PlayerName;
        buttonText[0].text = "" + thiscardvalue.matches;
        buttonText[1].text = "" + thiscardvalue.overs;
        buttonText[2].text = "" + thiscardvalue.wickets;
        buttonText[3].text = "" + thiscardvalue.maidens;
        buttonText[4].text = "" + thiscardvalue.economy;

        button_Label[0].text = "MAT";
        button_Label[1].text = "OVER";
        button_Label[2].text = "WKTS";
        button_Label[3].text = "MAID";
        button_Label[4].text = "ECON";

        listIndex = _listIndex;
    }

    public void UpdateBowlerStatsText(string _playername, float _matches, float _overs, float _wickets, float _maidens, float _economy, Sprite playerSprite, Sprite playerCountrySprite, Sprite playerTypeSprite, int _listIndex)
    {
        PlayerImage.sprite = playerSprite;
        PlayerCountry.sprite = playerCountrySprite;
        PlayerType_Img.sprite = playerTypeSprite;
        PlayerName.text = _playername;
        buttonText[0].text = "" + _matches;
        buttonText[1].text = "" + _overs;
        buttonText[2].text = "" + _wickets;
        buttonText[3].text = "" + _maidens;
        buttonText[4].text = "" + _economy;

        button_Label[0].text = "MAT";
        button_Label[1].text = "OVER";
        button_Label[2].text = "WKTS";
        button_Label[3].text = "MAID";
        button_Label[4].text = "ECON";
    }

    public void FlipImage()
    {
        if (isAllRounder)
        {
            FrontSideRect.localScale = Vector2.up;
            BackSideRect.localScale = Vector2.one;
        }
        else
        {
            Sequence seq = DOTween.Sequence();
            seq.Insert(0.1f, FrontSideRect.DOScaleX(0, 0.2f));
            seq.Insert(0.3f, BackSideRect.DOScaleX(1, 0.2f));
        }
        SuperCardsUI.instance.Audio_CardFlip();
        if (SuperCardsUI.instance.isUserPlayedfirst_InThisRound)
        {
            vibe_Anim.SetActive(true);
            Invoke("Delay_pick_tutorial", 1f);
            Invoke("Vibe_Animoff", 2.5f);
        }
        else
        {
            Vibe_Animoff();
        }
    }

    public void Delay_pick_tutorial()
    {
        if (SuperCardsUI.instance.isCurrentTurnByUser && SelectedValue == -1)
        {
            pick_tutorial.gameObject.SetActive(true);
            pick_tutorial.DOAnchorPos(new Vector2(3, 70), .3f);
        }
    }

    public void Vibe_Animoff()
    {
        vibe_Anim.SetActive(false);
    }
    public void RevertBack(float initDelay)
    {
        Sequence seq = DOTween.Sequence();
        seq.Insert(initDelay + 0.1f, BackSideRect.DOScaleX(0, 0.2f));
        seq.Insert(initDelay + 0.3f, FrontSideRect.DOScaleX(1, 0.2f));
    }
    public void TurnOn(bool interaction)
    {
        for (int j = 0; j < AllTheButtons.Length; j++)
        {
            AllTheButtons[j].interactable = interaction;
        }
        this.gameObject.SetActive(true);
    }

    public void TurnOFF()
    {
        this.gameObject.SetActive(false);
    }
    public void ShowWinnerTextUser()
    {
        WinnerText.DOFade(1f, 1f);
        TotalGrade.DOFade(0f, 1f);
        PlayerName.DOFade(0f, 1f);
        buttonText[0].DOFade(0f, 1f);
        buttonText[1].DOFade(0f, 1f);
        buttonText[2].DOFade(0f, 1f);
        buttonText[3].DOFade(0f, 1f);
        buttonText[4].DOFade(0f, 1f);
    }
    public void PushARandomValueUser()
    {
        float[] Values = new float[5];
        Values[0] = float.Parse(buttonText[0].text);
        Values[1] = float.Parse(buttonText[1].text);
        Values[2] = float.Parse(buttonText[2].text);
        Values[3] = float.Parse(buttonText[3].text);
        Values[4] = float.Parse(buttonText[4].text);

        if(SelectedValue == -1)
        {
            SelectedValue = Random.Range(0, 5);
        }
        //AllTheButtons[SelectedValue].GetComponent<Image>().sprite = button_select[0];
        //button_Label[SelectedValue].color = new Color32(255, 255, 255, 255);
        //buttonText[SelectedValue].color = new Color32(255, 255, 255, 255);
        highlightSelection();
        double temp;
        temp = System.Math.Round(Values[SelectedValue], 2);
        //Debug.LogError(temp);
        SuperCardsUI.instance.GetSelectedValueUser(float.Parse(temp.ToString()));
    }

    private void highlightSelection()
    {
        if (prevSelection_ForHighlight != SelectedValue)
        {
            if (prevSelection_ForHighlight > -1)
            {
                //Debug.LogError(prevSelection_ForHighlight);
                if (prevSelection_ForHighlight == 3)
                {
                    AllTheButtons[prevSelection_ForHighlight].GetComponent<Image>().sprite = button_select[3];
                }
                else
                {
                    AllTheButtons[prevSelection_ForHighlight].GetComponent<Image>().sprite = button_select[2];
                }
                button_Label[prevSelection_ForHighlight].color = new Color32(31, 31, 31, 255);
                buttonText[prevSelection_ForHighlight].color = new Color32(0, 81, 254, 255);
            }

            if (SelectedValue == 3)
            {
                AllTheButtons[SelectedValue].GetComponent<Image>().sprite = button_select[1];
            }
            else
            {
                AllTheButtons[SelectedValue].GetComponent<Image>().sprite = button_select[0];
            }
            button_Label[SelectedValue].color = new Color32(255, 255, 255, 255);
            buttonText[SelectedValue].color = new Color32(255, 255, 255, 255);
            prevSelection_ForHighlight = SelectedValue;
        }
    }

    public void onClick_Value(int i)
    {
        Vibe_Animoff();
        pick_tutorial.gameObject.SetActive(false);
        pick_tutorial.DOAnchorPos(new Vector2(3, 100), .3f);
        SuperCardsUI.instance.Buttonchoose();
        if (i != SelectedValue)
        {
            SelectedValue = i;

            //if (selectionOutline != null)
            //{
            //    selectionOutline.enabled = false;
            //}

            //selectionOutline = AllTheButtons[i].GetComponent<Outline>();
            //selectionOutline.enabled = true;
            highlightSelection(); 
            SuperCardsUI.instance.afterUserSelectingTheCurrentPlayerStats();

            if (CONTROLLER.PLAYMULTIPLAYER && SuperCardsUI.instance.isCurrentTurnByUser)
            {
                MultiplayerManager.instance.sendMySelectionToOpponent(1, listIndex, SelectedValue, SuperCardsUI.instance.appliedSuperCard_User);
            }
        }
    }

    public void Buttoninteract(bool interact)
    {
        for (int j = 0; j < AllTheButtons.Length; j++)
        {
            AllTheButtons[j].interactable = interact;
        }
    }
    
    public void submitSelectedValue()
    {
        int i = SelectedValue;
        SuperCardsUI.instance.GetSelectedValueUser(float.Parse(buttonText[i].text));

        //SuperCardsUI.instance.StartGameplay();
        Buttoninteract(false);
        TotalGrade.raycastTarget = false;
        PlayerName.raycastTarget = false;
        buttonText[0].raycastTarget = false;
        buttonText[1].raycastTarget = false;
        buttonText[2].raycastTarget = false;
        buttonText[3].raycastTarget = false;
        buttonText[4].raycastTarget = false;
    }
    public void ResetMe()
    {
        for (int j = 0; j < AllTheButtons.Length; j++)
        {
            AllTheButtons[j].interactable = true;
        }
        TotalGrade.raycastTarget = true;
        PlayerName.raycastTarget = true;
        buttonText[0].raycastTarget = true;
        buttonText[1].raycastTarget = true;
        buttonText[2].raycastTarget = true;
        buttonText[3].raycastTarget = true;
        buttonText[4].raycastTarget = true;
    }
    public void ShowValues()
    {
        TotalGrade.DOFade(1f, 1f);
        PlayerName.DOFade(1f, 1f);
        buttonText[0].DOFade(1f, 1f);
        buttonText[1].DOFade(1f, 1f);
        buttonText[2].DOFade(1f, 1f);
        buttonText[3].DOFade(1f, 1f);
        buttonText[4].DOFade(1f, 1f);
    }
    public void LockValues()
    {
        TotalGrade.raycastTarget = false;
        PlayerName.raycastTarget = false;
        buttonText[0].raycastTarget = false;
        buttonText[1].raycastTarget = false;
        buttonText[2].raycastTarget = false;
        buttonText[3].raycastTarget = false;
        buttonText[4].raycastTarget = false;
    }
    public int GiveSelectedValue()
    {
        if (SelectedValue == -1)
        {
            return Random.Range(0, 5);
        }
        else
        {
            return SelectedValue;
        }
    }
}

[System.Serializable]
public class CardValues
{
    public float TotalGrade;
    public string PlayerName;
    //Batsman
    public float Runs;
    public float _4s;
    public float _6s;
    public float _50s;
    public float StrikeRate;
    //Batsman
    //Bowler
    public float matches;
    public float overs;
    public float wickets;
    public float maidens;
    public float economy;
    //Bowler
    ////All-Rounder
    //public float A_Runs;
    //public float A_4s;
    //public float A_Wickets;
    //public float A_BowlingStrikeRate;
    //public float A_Average;
    ////All-Rounder
    public string Nationality;
    public int JerseyNumber;
}

