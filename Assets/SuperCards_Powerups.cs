using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SuperCards_Powerups : MonoBehaviour
{
    public ParticleSystem card_Particles;
    public AudioSource Powerup_AudioSource;
    public AudioClip Powerup_AudioClip;
    public RectTransform card_tutorial;
    public Sprite PlayercardBG;
    public Text[] powerUpText;
    public Text[] powerUpBotText;
    //public Text powerup_desc;
    public Image[] desc_card;
    private string[] powerUpWord_New = { "UPGRADE", "RE DRAW", "SWAP", "Positive Grade", "Negate Grade","Equalizer", "Re-Select", "Reveal Card" };
    public Sprite[] powerUpSprite;
    public Image[] powerUpImage;
    public Image[] powerUpBotImage;
    public float TempCurrentCardValue, updatedCurrentCardValue;
    public SuperCard_PowerUpsList currentState;
    public RectTransform SuperCard;
    

    public Button[] superCardSelectionButtons;
    public Image[] supercardImages;
    public Image[] Bot_supercardImages;
    private List<int> usedCards = new List<int>();

    Vector4 powerCardSelectedColor = new Vector4(1f, 1f, 1f, 100f/255f);

    void Start()
    {
        
        AssignTexttoCards();
        buttonvalue = -1;
    }
    public void AssignTexttoCards()
    {
        for (int i = 0; i < 3; i++)
        {
            //=========== user ======
            powerUpImage[i].sprite = powerUpSprite[i];
            powerUpBotImage[i].sprite = powerUpSprite[i];
            //powerUpBotSprite
            powerUpText[i].text = powerUpWord_New[i];
            powerUpBotText[i].text = powerUpWord_New[i];
            //=========================
        }
    }

    public void Reset()
    {
        usedCards.Clear();
    }

    public void enableSuperCardsButtonInteraction(bool enableInteraction, int selectedPowerCard = -1)
    {
        for(byte i = 0; i < superCardSelectionButtons.Length; i++)
        {
            superCardSelectionButtons[i].interactable = enableInteraction;
            if (usedCards.Contains(i))
            {
                supercardImages[i].gameObject.SetActive(false);
            }
            if (selectedPowerCard == i)
            {
                superCardSelectionButtons[i].interactable = false;
                supercardImages[i].color = new Color32(255, 255, 255, 150);
            }
            else
            {
                supercardImages[i].color = new Color32(255, 255, 255, 255);
                Bot_supercardImages[i].color = new Color32(255, 255, 255, 255);
            }
        }
        if (enableInteraction)
        {
            Invoke("Delay_card_tutorial", 4.0f);
        }
        else
        {
            card_tutorial.gameObject.SetActive(enableInteraction);
            card_tutorial.DOAnchorPos(new Vector2(-330, 0), .3f);
        }
    }


    public void Delay_card_tutorial()
    {
        if (SuperCardsUI.instance.isCurrentTurnByUser && buttonvalue == -1)
        {
            if (usedCards.Count != 3)
            {
                card_tutorial.gameObject.SetActive(true);
                card_tutorial.DOAnchorPos(new Vector2(-200, 0), .3f);
            }
        }
        buttonvalue = -1;
    }

    public void  Bot_selectedSupercard(int i)
    {
        Bot_supercardImages[i].color = new Color32(255, 255, 255, 150);
        Bot_supercardImages[i].gameObject.SetActive(false);
    }
    public void Bot_Supercard_Reset(int i)
    {
        Bot_supercardImages[i].color = new Color32(255, 255, 255, 255);
    }
    public int buttonvalue=-1;
    public GameObject undo_button;

    public void  Apply_particles()
    {
        SuperCardsUI.instance.NormalSuperCards[SuperCardsUI.Count].PlayercardImage.sprite = PlayercardBG;
        SuperCardsUI.instance.NormalSuperCards[SuperCardsUI.Count].PlayercardImage.DOFade(1f, 1f);
        card_Particles.gameObject.SetActive(true);
        card_Particles.Play();
    }

    public void Player_Supercards_Powerups(int buttonIndex)
    {

        card_Particles.gameObject.SetActive(false);
        buttonvalue = buttonIndex;
        enableSuperCardsButtonInteraction(false, buttonIndex);
        SuperCardsUI.StartPlay = false;
        SuperCard = Instantiate(superCardSelectionButtons[buttonIndex].GetComponent<RectTransform>());
        SuperCard.DOScale(new Vector3(1f, 1f, 1f), .001f);
        SuperCard.position = superCardSelectionButtons[buttonIndex].transform.position;
        SuperCard.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        Powerup_AudioSource.clip = Powerup_AudioClip;
        Powerup_AudioSource.Play();
        SuperCard.SetParent(SuperCardsUI.instance.NormalSuperCards[SuperCardsUI.Count].SuperCardHolder);
        SuperCardsUI.instance.NormalSuperCards[SuperCardsUI.Count].PlayercardImage.DOFade(.7f, .5f);
        Invoke("Apply_particles", 1f);
        SuperCard.DOAnchorPos(new Vector3(0f, 280f, 0f), 1f);
        SuperCard.DORotate(Vector3.zero, 1f);
        SuperCard.DOScale(new Vector3 (.8f,.8f,.8f), 1f);
        currentState = (SuperCard_PowerUpsList)buttonIndex;
            switch (currentState)
            {
                case SuperCard_PowerUpsList.Exchange:
                    StartCoroutine(SuperCardsUI.instance.ExchangeNormalCardUser());
                    break;
                case SuperCard_PowerUpsList.ReDraw:
                    StartCoroutine(SuperCardsUI.instance.ReDrawCardUser());
                    break;
                case SuperCard_PowerUpsList.GoWild:
                    StartCoroutine(SuperCardsUI.instance.Go_Wildcard());
                    break;
                
                default:
                    break;
            }

        if (usedCards.Contains(buttonvalue) == false)
        {
            usedCards.Add(buttonvalue);
        }
        Invoke("Reset_Value", 8.0f);
    }

    public void Reset_Value()
    {
        buttonvalue = -1;
    }

    //public void MakeCardBigAgain()
    //{

    //}
    public void Powerup_manual(int i)
    {
        switch (i)
        {
            case 0:
                desc_card[i].gameObject.SetActive(true);
                desc_card[i].DOFade(1f, 1f);
                StartCoroutine(reset_desc_card(i));
                break;
            case 1:
                desc_card[i].gameObject.SetActive(true);
                desc_card[i].DOFade(1f, 1f);
                StartCoroutine(reset_desc_card(i));
                break;
            case 2:
                desc_card[i].gameObject.SetActive(true);
                desc_card[i].DOFade(1f, 1f);
                StartCoroutine(reset_desc_card(i));
                break;
            default:
                desc_card[i].gameObject.SetActive(true);
                desc_card[i].DOFade(1f, 1f);
                StartCoroutine(reset_desc_card(i));
                break;
        }
    }

    IEnumerator reset_desc_card(int i)
    {
        yield return new WaitForSeconds(5f);
        switch (i)
        {
            case 0:
                
                desc_card[i].DOFade(0f, .5f);
                desc_card[i].gameObject.SetActive(false);
                break;
            case 1:
                
                desc_card[i].DOFade(0f, .5f);
                desc_card[i].gameObject.SetActive(false);
                break;
            case 2:
                
                desc_card[i].DOFade(0f, .5f);
                desc_card[i].gameObject.SetActive(false);
                break;
            default:
                
                desc_card[i].DOFade(0f, .5f);
                desc_card[i].gameObject.SetActive(false);
                break;
        }
    }

    public int setGradeRandom;
    public float[] incrementValues = {.5f,1f,2f,3f,4f,6f};
    public int incrementRandom;
    public void generateRandom()
    {
        
    }

    public void resetValues()
    {
        TempCurrentCardValue = 0;
        updatedCurrentCardValue = 0;
        setGradeRandom = 0;
        incrementRandom = 0;
    }

}

public enum SuperCard_PowerUpsList : byte
{
    GoWild,
    ReDraw,
    Exchange,
    LastElement,

    PositiveGrade,
    NegativeGrade,
    //Exchange,
    //ReDraw,
    //GoWild,
    Equalizer,
    ReSelect,
    RevealCard,

}