using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Rendering;

public class SuperBallUI : MonoBehaviour
{
    public AudioSource Superball_AudioSource;
    public AudioClip Superball_AudioClip;
    //public ParticleSystem Card_Particles;
    public static SuperBallUI instance;
    public RectTransform Front;
    public RectTransform Back;
    public RectTransform[] CardsFront;
    public RectTransform[] CardsBack;
    public Text[] CardsValue;
    public Button[] LockAllCards;
    public byte UserValue, BotValue;
    //Timer
    public GameObject TimerGAM;
    public Image TimerIMG;
    public Text TimerText;
    public Image TimerFiller;
    int Counter = 5;
    Coroutine StartTimerCoroutine;
    //Timer
    public Text SuperBallText;
    //public Image winning_text;
    //public Sprite[] winning_sprite;
    private byte[] CardsOrder = { 0, 1, 2, 3, 4, 5 };
    Tweener sq;

    private bool isFirstTurn_ByUser;
    private bool isTurnBYUser;
    public bool turnBYUser
    {
        get { return isTurnBYUser; }
    }

    public GameObject multiplayerLoadingScreen;

    private void Awake()
    {
        instance = this;
        SuperCardsUI.instance.issuperball = false;
    }
    public void ShowMe()
    {
        this.gameObject.SetActive(true);
        SuperBallText.text = "";
        //winning_text.gameObject.SetActive(false);
        isFirstTurn_ByUser = isTurnBYUser = SuperCardsUI.instance.isFirstTurn_ByUser();
        CalculateRandom();
    }
    public void CalculateRandom() 
    {
        for (byte i = 0; i < CardsOrder.Length; i++)
        {
            byte temp = CardsOrder[i];
            int random = Random.Range(i, CardsOrder.Length);
            CardsOrder[i] = CardsOrder[random];
            CardsValue[i].text = CardsOrder[i].ToString();
            LockAllCards[i].interactable = isTurnBYUser;
            CardsOrder[random] = temp;
        }

        ShowSuperBall();
    }

    public void ShowSuperBall()
    {
        //for (int i = 0; i < CardsFront.Length; i++)
        //{
        //    CardsFront[i].DOScale(Vector3.one, 2f);
        //}
        
        if (isTurnBYUser)
        {
            SuperBallText.text = "SELECT YOUR CARD";
            StartTimerCoroutine = StartCoroutine(StartTimer());
        }
        else
        {
            if(CONTROLLER.PLAYMULTIPLAYER)
            {
                SuperBallText.text = "OPPONENT'S TURN";
            }
            else
            {
                GetSelectedCard(Random.Range(0, 6));
            }
        }
    }

    public void onReceiveOpponentSelection_Multiplayer(byte arrayIndex, byte value)
    {
        if(CardsOrder[arrayIndex] != value)
        {
            for(byte i = 0; i < CardsOrder.Length; i++)
            {
                if(CardsOrder[i] == value)
                {
                    byte temp = CardsOrder[arrayIndex];
                    CardsOrder[arrayIndex] = value;
                    CardsValue[arrayIndex].text = "" + value;
                    CardsOrder[i] = temp;
                    CardsValue[i].text = "" + value;
                    break;
                }
            }
        }

        GetSelectedCard(arrayIndex);
    }

    byte userSelectionIndex;
    byte botSelectionIndex;
    public void GetSelectedCard(int i)
    {
        //sq.Kill();
        if (isTurnBYUser)
        {
            userSelectionIndex = (byte)i;
            UserValue = CardsOrder[i];
            FlipImage(i);
            StopCoroutine(StartTimerCoroutine);
            TimerGAM.SetActive(false);
            if (CONTROLLER.PLAYMULTIPLAYER)
            {
                MultiplayerManager.instance.sendMySelectionInSuperBallToOpponent(userSelectionIndex, UserValue);
            }
        }
        else
        {
            botSelectionIndex = (byte)i;
            BotValue = CardsOrder[i];
            FlipImageBot(i);
        }

        for (int j = 0; j < LockAllCards.Length; j++)
        {
            LockAllCards[j].interactable = false;
        }

        if (CONTROLLER.PLAYMULTIPLAYER && isTurnBYUser)
        {
           
            SuperCardsUI.instance.waitForAckForSuperBallSelection(userSelectionIndex, UserValue);
        }
        else
        {
            checkForNextTurn();
        }

        CardsValue[i].DOFade(1f, 1f);
        //sq.Kill();
    }

    public void checkForNextTurn()
    {
        if(isFirstTurn_ByUser == isTurnBYUser)
        {
            isTurnBYUser = !isTurnBYUser;

            for (int i = 0; i < LockAllCards.Length; i++)
            {
                LockAllCards[i].interactable = isTurnBYUser;
            }

            // go for next player
            if (isTurnBYUser)
            {
                SuperBallText.text = "SELECT YOUR CARD";
                StartTimerCoroutine = StartCoroutine(StartTimer());
            }
            else
            {
                SuperBallText.text = "OPPONENT'S TURN";

                if (CONTROLLER.PLAYMULTIPLAYER == false)
                {
                    Sequence seq = DOTween.Sequence();
                    seq.AppendInterval(1f);
                    seq.AppendCallback(() =>
                    {
                        GetSelectedCard(getRandomSelection(userSelectionIndex));
                    });
                }
            }
        }
        else
        {
            //isTurnBYUser = !isTurnBYUser;
            StartCoroutine(CompareTwoValues(UserValue, BotValue));
        }
    }

    //public void MoveDownForUser(int i)
    //{
    //    CardsFront[i].DOAnchorPosY(-407, 1f);
    //    MoveDownForBot(i);
    //}

    public void FlipImage(int i)
    {
        Superball_AudioSource.clip = Superball_AudioClip;
        Superball_AudioSource.Play();
        Sequence seq = DOTween.Sequence();
        seq.Insert(0.1f, CardsFront[i].DOScaleX(0, 0.2f));
        seq.Insert(0.3f, CardsBack[i].DOScaleX(1, 0.2f));
    }
    public void FlipImageBot(int i)
    {
        Superball_AudioSource.clip = Superball_AudioClip;
        Superball_AudioSource.Play();
        Sequence seq = DOTween.Sequence();
        seq.Insert(0.1f, CardsFront[i].DOScaleX(0, 0.3f));
        seq.Insert(0.3f, CardsBack[i].DOScaleX(1, 0.2f));
    }
    public IEnumerator MoveDownForBot(int i, float timer)
    {
        yield return new WaitForSeconds(timer);
        
    }

    private int getRandomSelection(int userSelctedIndex)
    {
        int j = Random.Range(0, 6);
        if (userSelctedIndex == j)
        {
            if (userSelctedIndex == 0)
            {
                j = j + 2;
            }
            else if (j == 5)
            {
                j = j - 3;
            }
            else
            {
                j = j + 1;
            }
        }

        return j;
    }

    public IEnumerator CompareTwoValues(int UserValue, int BotValue)
    {
        yield return new WaitForSeconds(2.0f);
        SuperCardsUI.instance.isValidatingScores = true;

        for (int i = 0; i < LockAllCards.Length; i++)
        {
            LockAllCards[i].interactable = false;
        }
        SuperCardsUI.instance.issuperball = true;
        if (UserValue < BotValue)
        {
            SuperBallText.text = "YOU LOST THE ROUND";
            SuperCardsUI.instance.Losing_Anim();
            Invoke("HideMe", 3f);
        }
        else
        {
            SuperBallText.text = "YOU WON THE ROUND";
            SuperCardsUI.instance.Winning_Anim();
            Invoke("HideMe", 3f);
        }
    }
    public void HideMe()
    {
        SuperBallText.text = "";
        TimerFiller.DOFillAmount(1, 0.1f);
        Counter = 5;
        TimerText.text = Counter.ToString();
        TimerGAM.SetActive(false);
        for (int i = 0; i < CardsFront.Length; i++)
        {
            CardsFront[i].localScale = Vector3.one;
            CardsBack[i].localScale = Vector2.up;
        }

        sq.Kill();
        //Card_Particles.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
    IEnumerator StartTimer()
    {

        Counter = 5;
        TimerText.text = Counter.ToString();
        TimerGAM.SetActive(true);
        sq = TimerFiller.DOFillAmount(0f, Counter).SetEase(Ease.Linear);

        while (Counter > 0)
        {
            yield return new WaitForSeconds(1f);
            Counter--;
            TimerText.text = Counter.ToString();
        }

        TimerGAM.SetActive(false);
        if (isTurnBYUser)
        {
            if (isFirstTurn_ByUser)
            {
                GetSelectedCard(Random.Range(0, 6));
            }
            else
            {
                GetSelectedCard(getRandomSelection(botSelectionIndex));
            }
        }
    }
}
