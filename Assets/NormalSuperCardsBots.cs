using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class NormalSuperCardsBots : MonoBehaviour
{
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
    public Text WinnerText;
    public CardValuesBot thiscardvalue = new CardValuesBot();
    public RectTransform FrontSideRect;
    public RectTransform BackSideRect;
    private int listIndex;
    public Transform SuperCardBotHolder;


    public bool isAllRounder;
    public byte playerType;//0- batsman, 1- bowler

    private int selectedValue;
    private int prevSelection_ForHighlight = -1;
    //public Outline[] selectedHighLights;
    public Image[] AllTheButtons;
    public Sprite[] button_select;

    public void Reset()
    {
        prevSelection_ForHighlight = -1;
    }

    public void NormalSuperCardFillerBot_BatsmanStats(string _playername, float _runs, float _4s, float _6s, float _50s, float strikeRate, Sprite playerSprite, Sprite playerCountrySprite, Sprite playerTypeSprite, int _listIndex)
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

    public void UpdateBatsmanStatsText(string _playername, float _runs, float _4s, float _6s, float _50s, float strikeRate, Sprite playerSprite, Sprite playerCountrySprite, Sprite playerTypeSprite, int _listIndex)
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

    public void NormalSuperCardFillerBot_BowlerStats(string _playername, float _matches, float _overs, float _wickets, float _maidens, float _economy, Sprite playerSprite, Sprite playerCountrySprite, Sprite playerTypeSprite, int _listIndex)
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
    }
    public void RevertBack(float initDelay)
    {
        Sequence seq = DOTween.Sequence();
        seq.Insert(initDelay + 0.1f, BackSideRect.DOScaleX(0, 0.2f));
        seq.Insert(initDelay + 0.3f, FrontSideRect.DOScaleX(1, 0.2f));
    }
    public void TurnOn()
    {
        this.gameObject.SetActive(true);
    }

    public void TurnOFF()
    {
        this.gameObject.SetActive(false);
    }
    public void ShowWinnerTextBot()
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

    public void UpdateUserSelectionToBot(int i)
    {
        float[] Values = new float[5];
        Values[0] = float.Parse(buttonText[0].text);
        Values[1] = float.Parse(buttonText[1].text);
        Values[2] = float.Parse(buttonText[2].text);
        Values[3] = float.Parse(buttonText[3].text);
        Values[4] = float.Parse(buttonText[4].text);

        selectedValue = i;
        double temp;
        temp = System.Math.Round(Values[selectedValue], 2);
        SuperCardsUI.instance.GetSelectedValueBot(float.Parse(temp.ToString()));
        highlightSelection();
    }

    public void PushARandomValue()
    {
        float[] Values = new float[5];
        Values[0] = float.Parse(buttonText[0].text);
        Values[1] = float.Parse(buttonText[1].text);
        Values[2] = float.Parse(buttonText[2].text);
        Values[3] = float.Parse(buttonText[3].text);
        Values[4] = float.Parse(buttonText[4].text);

        selectedValue = Random.Range(0, 5);
        double temp;
        temp = System.Math.Round(Values[selectedValue], 2);
        SuperCardsUI.instance.GetSelectedValueBot(float.Parse(temp.ToString()));
        highlightSelection();
    }

    public void OnReceiveCardSelectionFromOpponent_Multiplayer(int i, byte type)
    {
        if (type == 2)
        {//after submission
            SuperCardsUI.instance.GetSelectedValueBot(float.Parse(buttonText[i].text));
        }
        
        selectedValue = i;
        highlightSelection();
    }

    public void CalculateTheHighest()
    {
        float[] Values = new float[5];
        Values[0] = float.Parse(buttonText[0].text);
        Values[1] = float.Parse(buttonText[1].text);
        Values[2] = float.Parse(buttonText[2].text);
        Values[3] = float.Parse(buttonText[3].text);
        Values[4] = float.Parse(buttonText[4].text);

        selectedValue = 0;
        float MaxValue = 0;
        for (int i = 0; i < Values.Length; i++)
        {
            if (Values[i] > MaxValue)
            {
                MaxValue = Values[i];
                selectedValue = i;
            }
        }
        double temp;
        temp = System.Math.Round(MaxValue, 2);
        SuperCardsUI.instance.GetSelectedValueBot(float.Parse(temp.ToString()));
        highlightSelection();
    }

    private void highlightSelection()
    {
        if (prevSelection_ForHighlight != selectedValue)
        {
            if (prevSelection_ForHighlight > -1)
            {
                if(prevSelection_ForHighlight == 3)
                {
                    AllTheButtons[prevSelection_ForHighlight].sprite = button_select[3];
                }
                else
                {
                    AllTheButtons[prevSelection_ForHighlight].sprite = button_select[2];
                }
                button_Label[prevSelection_ForHighlight].color = new Color32(249, 244, 19, 255);
                buttonText[prevSelection_ForHighlight].color = new Color32(255, 255, 255, 255);
            }
            //selectedHighLights[selectedValue].enabled = true;
            if (selectedValue == 3)
            {
                AllTheButtons[selectedValue].sprite = button_select[1];
            }
            else
            {
                AllTheButtons[selectedValue].sprite = button_select[0];
            }
            button_Label[selectedValue].color = new Color32(31, 31, 31, 255);
            buttonText[selectedValue].color = new Color32(31, 31, 31, 255);
            prevSelection_ForHighlight = selectedValue;
        }
    }
    public double CalculateTheHighestReturn()
    {
        float[] Values = new float[5];
        Values[0] = float.Parse(buttonText[0].text);
        Values[1] = float.Parse(buttonText[1].text);
        Values[2] = float.Parse(buttonText[2].text);
        Values[3] = float.Parse(buttonText[3].text);
        Values[4] = float.Parse(buttonText[4].text);

        selectedValue = 0;
        float MaxValue = 0;
        for (int i = 0; i < Values.Length; i++)
        {
            if (Values[i] > MaxValue)
            {
                MaxValue = Values[i];
                selectedValue = i;
            }
        }
        double temp;
        temp = System.Math.Round(MaxValue, 2);
        highlightSelection();
        return temp;
    }
    public void CalculateTheLowest()
    {
        float[] Values = new float[5];
        Values[0] = float.Parse(buttonText[0].text);
        Values[1] = float.Parse(buttonText[1].text);
        Values[2] = float.Parse(buttonText[2].text);
        Values[3] = float.Parse(buttonText[3].text);
        Values[4] = float.Parse(buttonText[4].text);

        float MinValue = 0;
        for (int i = 0; i < Values.Length; i++)
        {
            if (i == 0)
            {
                MinValue = Values[i];
            }
            if (Values[i] < MinValue)
            {
                MinValue = Values[i];
                selectedValue = i;
            }
        }
        double temp;
        temp = System.Math.Round(MinValue, 2);
        SuperCardsUI.instance.GetSelectedValueBot(float.Parse(temp.ToString()));

        highlightSelection();
    }
    //public void CompareValuesWithTheUser(float i)
    //{
        
    //}
    public void HideValues()
    {
        TotalGrade.DOFade(0f, 0.1f);
        PlayerName.DOFade(0f, 0.1f);
        buttonText[0].DOFade(0f, 0.1f);
        buttonText[1].DOFade(0f, 0.1f);
        buttonText[2].DOFade(0f, 0.1f);
        buttonText[3].DOFade(0f, 0.1f);
        buttonText[4].DOFade(0f, 0.1f);
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

    public int BotSelection()
    {
        return selectedValue;
    }
}

[System.Serializable]
public class CardValuesBot
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
    //All-Rounder
    //public float A_Runs;
    //public float A_4s;
    //public float A_Wickets;
    //public float A_BowlingStrikeRate;
    //public float A_Average;
    //All-Rounder
    public string Nationality;
    public int JerseyNumber;
}