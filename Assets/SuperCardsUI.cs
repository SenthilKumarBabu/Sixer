using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using SimpleJSON;
#if SUPERCARDS_MULTIPLAYER
using Photon.Pun;
using Photon.Realtime;
#endif
using UnityEngine.U2D;

public class SuperCardsUI : MonoBehaviour
{
    public Transform PlayerCard_Ref1;
    public Transform PlayerCard_Ref2;
    public Transform Opponent_Ref2;
    public GameObject watchVideoPopup;
    public GameObject Adloading;
    private RectTransform SuperCard_Bot;
    public AudioSource Card_AudioSource;
    public AudioClip[] Card_AudioClip;
    public ParticleSystem[] Card_Particles;
    public MultiplayerManager MultiplayerManager;
    public static SuperCardsUI instance;
    public SuperCards_Powerups SuperCards_Powerups;
    public Image BotCard_IMG;
    public Image UserCard_IMG;
    public GameObject BotSuperCardGAM;
    public GameObject Content;
    public GameObject cardsPlayMode;
    public GameObject cardDetailsInfo;
    public SuperBallUI SuperBallUI;
    public GameObject MainContent;
    public GameObject GameScreen;
    public GameObject settingScreen;
    public GameObject instructScreen;
    public GameObject PauseScreen;
    public TextAsset PlayerDetails;
    public byte[] CardNumberUser;
    public NormalSuperCards[] NormalSuperCards;

    public byte[] CardNumberBot;
    public NormalSuperCardsBots[] NormalSuperCardsBot;
    public List<SuperCard_PowerUpsList> BotSuperCards = new List<SuperCard_PowerUpsList>();

    private List<byte> dummyShuffle_Batsman_User = new List<byte>(9) {0, 1, 2, 3, 4, 5, 6, 7, 8};
    private List<byte> dummyShuffle_Bowler_User = new List<byte>(8) {9, 10, 11, 12, 13, 14, 15, 16};
    private List<byte> dummyShuffle_Allrounder_User = new List<byte>(8) {17, 18, 19, 20, 21, 22, 23, 24};

    private List<byte> dummyShuffle_Batsman_Bot = new List<byte>(9) { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
    private List<byte> dummyShuffle_Bowler_Bot = new List<byte>(8) { 9, 10, 11, 12, 13, 14, 15, 16 };
    private List<byte> dummyShuffle_Allrounder_Bot = new List<byte>(8) { 17, 18, 19, 20, 21, 22, 23, 24 };

    public Transform UserDeck;
    public GameObject CardsPrefab;

    public Transform BotDeck;
    public GameObject CardsPrefabBot;
    Sequence sq;
    Sequence opponentSq;
    public static int Count = 0;
    public static bool StartPlay;

    float UserSelectedValue;
    float BotSelectedValue;


    public int Counter = 20;
    public GameObject FillerGAM;
    public Image Filler;
    public Image Filler_back;
    public Text FillerText;
    Coroutine StartCountdownCoroutine;

    private int start_Counter = 3;
    public Text start_CounterText;

    //Score
    public int ScoreUser = 0;
    public int ScoreBot = 0;
    public Text ScoreUserText;
    public Text ScoreBotText;
    public RectTransform scoreadd_Anim;

    //Score

    //Round result
    public Text Round_text;
    public Text Big_status;
    public Text Small_status;
    public Text Big_Label;
    public Text Small_Label;
    public Text Big_stats;
    public Text Small_stats;
    public Image Big_Image;
    public Image Small_Image;
    public Sprite[] Round_Sprite;
    public GameObject RoundAnimation;
    public Image RoundWonScreen_main;
    public RectTransform RoundWonScreen;
    public bool issuperball;
    public bool stopexitpopup;
    //Round result

    //GameOverScreen
    public GameObject GameOverScreen;
    public GameObject Win_Screen;
    public GameObject Lose_Screen;
    public Text WonUserOrBot;
    public Image playerimage,playerimageGame;
    public Sprite defaultimage;
    public GameObject Coinvalue;
    //GameOverScreen

    public GameObject MasterButton;
    public Text[] PlayerNames;
    public GameObject MultiplayerPanel;

    public GameObject allRounderChoiceSelectionPanel_User;
    public GameObject allRounderChoiceSelectionPanel_Bot;
    public NormalSuperCards allRounderChoice_AsBatsman_User;
    public NormalSuperCards allRounderChoice_AsBowler_User;
    public NormalSuperCardsBots allRounderChoice_AsBatsman_Bot;
    public NormalSuperCardsBots allRounderChoice_AsBowler_Bot;

    public SpriteAtlas PlayerSprites;

    public Text instructionText;
    private const int NoOfCards = 25;

    public enum Turn : byte
    {
        User,
        Bot
    }

    private Turn turnBy;
    public bool isCurrentTurnByUser
    {
        get
        {
            return turnBy == Turn.User;
        }
    }
    private Turn firstTurnBy_InThisRound;
    public bool isUserPlayedfirst_InThisRound
    {
        get
        {
            return firstTurnBy_InThisRound == Turn.User;
        }
    }

    public bool isOpponentsTurnToPlayThisRound
    {
        get
        {
            return firstTurnBy_InThisRound == Turn.Bot && turnBy == Turn.Bot;
        }
    }

    public object Small_Value { get; private set; }

    [HideInInspector]
    public bool isValidatingScores;

    private string[] countries = new string[] { "IND", "RSA", "NZ", "ENG", "AUS", "WI" };

    public GameObject submitSelectedValueBtnObj;

    public GameObject reconnectionBGObj;
    public Text reconnectionText;

    private Coroutine multiplayerWaitForACKCoroutine;

    private void Awake()
    {
        instance = this;
        StartPlay = true;
        stopexitpopup = false;
        MultiplayerManager = GameObject.Find("MultiplayerManager").GetComponent<MultiplayerManager>();

        CardNumberUser = new byte[NoOfCards];
        NormalSuperCards = new NormalSuperCards[NoOfCards];

        CardNumberBot = new byte[NoOfCards];
        NormalSuperCardsBot = new NormalSuperCardsBots[NoOfCards];
        RoundWonScreen.gameObject.SetActive(false);
    }
    public void ResetMe()
    {
        //BotSuperCards.Clear();
        Count = 0;
        instructionText.text = "";
        instructionText.transform.parent.gameObject.SetActive(false);
        SuperCards_Powerups.enableSuperCardsButtonInteraction(false);
    }

    public void Audio_CardFlip()
    {
        if (turnBy == Turn.User) //&& firstTurnBy_InThisRound == Turn.User)
        {
            Card_AudioSource.clip = Card_AudioClip[5];
            Card_AudioSource.Play();
        }
        else
        {
            Card_AudioSource.clip = Card_AudioClip[6];
            Card_AudioSource.Play();
        }
    }

    public void Buttonchoose()
    {
        Card_AudioSource.clip = Card_AudioClip[8];
        Card_AudioSource.Play();
    }

    public void onClickSinglePlayer()
    {
        if (UserProfile.Tickets >= 1)
        {
            UserProfile.Tickets -= 1;
            UserProfile.SpentTickets += 1;
            UserProfile.spentTotTickets += 1;

            if (CONTROLLER.IsUserLoggedIn())
            {
                PlayerPrefsManager.SaveUserProfile();
            }
            else
            {
                PlayerPrefsManager.SaveCoins();
            }
            Proceed_AftercheckTicket();
        }
        else
        {
            if (AdIntegrate.instance.checkTheInternet() && AdIntegrate.instance.isRewardedReadyToPlay())
            {
                CONTROLLER.RewardedVideoClickedState = 10;
                watchVideoPopup.SetActive(true);
            }
            else
            {
                ShowToast("Insufficient tickets! \nBuy tickets from the store to continue.");
            }
        }  
    }

    public void Proceed_AftercheckTicket()
    {
        CONTROLLER.PLAYMULTIPLAYER = false;
        turnBy = Turn.User;
        firstTurnBy_InThisRound = Turn.User;
        OnClickPlay();
        start_Counter = 3;
        start_CounterText.gameObject.SetActive(true);
        StartCoroutine(Countdown_Start(1.0f));
        //FirebaseAnalyticsManager.instance.SCC_Start();
    }

    IEnumerator Countdown_Start(float time)
    {
        if (start_Counter >= 1)
        {
            Card_AudioSource.clip = Card_AudioClip[4];
            Card_AudioSource.Play();
            start_CounterText.text = start_Counter.ToString();
            yield return new WaitForSeconds(1f);
            start_Counter--;
            StartCoroutine(Countdown_Start(1.0f));
        }
        else
        {
            start_CounterText.gameObject.SetActive(false);
            ShowMe();
        }
    }

    public void OnClickMultiplayerPanel()
    {
        CONTROLLER.PLAYMULTIPLAYER = true;

        OnClickPlay();
        MultiplayerPanel.SetActive(true);
        MultiplayerManager.EstablishConnection();
    }
    public void ShowMe()
    {
        AdIntegrate.instance.HideAd();
        MultiplayerPanel.SetActive(false);
        cardsPlayMode.SetActive(true);
        if (CONTROLLER.PLAYMULTIPLAYER == false)
        {
            //for (byte i = 0; i < 24; i++)
            //{
            //    //CardNumberUser.Add(i);
            //    //CardNumberBot.Add(i);

            //    DummyShuffle.Add(i);
            //    DummyShuffleBot.Add(i);
            //}
            //Invoke("BlackJackShuffle", 0.1f);
            //Invoke("ShuffleAndParseDetails", 1.0f);
            ShuffleAndParseDetails();
        }
    }

    public bool isFirstTurn_ByUser()
    {
        return (firstTurnBy_InThisRound == Turn.User);
    }

    public void StartGame_MultiplayerAfterSharingShuffledCards()
    {
        //Debug.Log("start game");
#if SUPERCARDS_MULTIPLAYER
        if(PhotonNetwork.IsMasterClient)
#else
        if(true)
#endif
        {
            turnBy = Turn.User;
            firstTurnBy_InThisRound = Turn.User;
        }
        else
        {
            turnBy = Turn.Bot;
            firstTurnBy_InThisRound = Turn.Bot;
        }
        ShowMe();
        StartNewRound();
    }

    public void onReceiveShuffledCards_Multiplayer()
    {
        ShuffleAndParseDetails();
    }

    public void ShuffleAndParseDetails()
    {
        string _data = this.PlayerDetails.text;
        SimpleJSON.JSONNode SuperCards = SimpleJSON.JSONNode.Parse(_data);

        JSONNode PlayerDetails = SuperCards["SuperCards"]["PlayerDetails"];
#if SUPERCARDS_MULTIPLAYER
        bool cardsAreShuffled = CONTROLLER.PLAYMULTIPLAYER && PhotonNetwork.IsMasterClient == false;
#else
        bool cardsAreShuffled = false;
#endif
        byte randomUser = 0;
        byte randomBot = 0;

        if (cardsAreShuffled == false) // For Single player and multiplayer master player
        {
            List<byte> statsType = new List<byte>() { 0, 1, 2 };

            byte batsmanStats_ShuffledCount = (byte)dummyShuffle_Batsman_User.Count;
            byte bowlerStats_ShuffledCount = (byte)dummyShuffle_Bowler_User.Count;
            byte allRounderStats_ShuffledCount = (byte)dummyShuffle_Allrounder_User.Count;

            int random;

            for (byte i = 0; i < 25; i++)
            {
                switch (statsType[Random.Range(0, statsType.Count)])
                {
                    case 0://batsman stats
                        randomUser = dummyShuffle_Batsman_User[Random.Range(0, batsmanStats_ShuffledCount)];
                        random = Random.Range(0, batsmanStats_ShuffledCount);
                        randomBot = dummyShuffle_Batsman_Bot[random];
                        if (randomUser == randomBot)
                        {
                            byte temp;
                            if (batsmanStats_ShuffledCount > 1)
                            {
                                temp = dummyShuffle_Batsman_Bot[random];
                                dummyShuffle_Batsman_Bot[random] = dummyShuffle_Batsman_Bot[batsmanStats_ShuffledCount - 1];
                                dummyShuffle_Batsman_Bot[batsmanStats_ShuffledCount - 1] = temp;
                                random = Random.Range(0, (batsmanStats_ShuffledCount - 1));
                                randomBot = dummyShuffle_Batsman_Bot[random];
                            }
                            else
                            {
                                if (randomUser != dummyShuffle_Batsman_Bot[batsmanStats_ShuffledCount])
                                {
                                    temp = dummyShuffle_Batsman_Bot[batsmanStats_ShuffledCount];
                                    dummyShuffle_Batsman_Bot[batsmanStats_ShuffledCount] = dummyShuffle_Batsman_Bot[random];
                                    dummyShuffle_Batsman_Bot[random] = temp;
                                    random = batsmanStats_ShuffledCount;
                                }
                                else
                                {
                                    temp = dummyShuffle_Batsman_Bot[batsmanStats_ShuffledCount + 1];
                                    dummyShuffle_Batsman_Bot[batsmanStats_ShuffledCount + 1] = dummyShuffle_Batsman_Bot[random];
                                    dummyShuffle_Batsman_Bot[random] = temp;
                                    random = batsmanStats_ShuffledCount + 1;
                                }

                                byte index = (byte)System.Array.IndexOf(CardNumberBot, temp);
                                CardNumberBot[index] = dummyShuffle_Batsman_Bot[random];
                                randomBot = temp;
                            }
                        }

                        //=== Moving element to last =============
                        dummyShuffle_Batsman_User.Remove(randomUser);
                        dummyShuffle_Batsman_User.Add(randomUser);
                        dummyShuffle_Batsman_Bot.Remove(randomBot);
                        dummyShuffle_Batsman_Bot.Add(randomBot);
                        //====================================

                        batsmanStats_ShuffledCount--;
                        break;
                    case 1://bowler stats
                        randomUser = dummyShuffle_Bowler_User[Random.Range(0, bowlerStats_ShuffledCount)];
                        random = Random.Range(0, bowlerStats_ShuffledCount);
                        randomBot = dummyShuffle_Bowler_Bot[random];
                        if (randomUser == randomBot)
                        {
                            byte temp;
                            if (bowlerStats_ShuffledCount > 1)
                            {
                                temp = dummyShuffle_Bowler_Bot[random];
                                dummyShuffle_Bowler_Bot[random] = dummyShuffle_Bowler_Bot[bowlerStats_ShuffledCount - 1];
                                dummyShuffle_Bowler_Bot[bowlerStats_ShuffledCount - 1] = temp;
                                random = Random.Range(0, (bowlerStats_ShuffledCount - 1));
                                randomBot = dummyShuffle_Bowler_Bot[random];
                            }
                            else
                            {
                                if (randomUser != dummyShuffle_Bowler_Bot[bowlerStats_ShuffledCount])
                                {
                                    temp = dummyShuffle_Bowler_Bot[bowlerStats_ShuffledCount];
                                    dummyShuffle_Bowler_Bot[bowlerStats_ShuffledCount] = dummyShuffle_Bowler_Bot[random];
                                    dummyShuffle_Bowler_Bot[random] = temp;
                                    random = bowlerStats_ShuffledCount;
                                }
                                else
                                {
                                    temp = dummyShuffle_Bowler_Bot[bowlerStats_ShuffledCount + 1];
                                    dummyShuffle_Bowler_Bot[bowlerStats_ShuffledCount + 1] = dummyShuffle_Bowler_Bot[random];
                                    dummyShuffle_Bowler_Bot[random] = temp;
                                    random = bowlerStats_ShuffledCount + 1;
                                }

                                byte index = (byte)System.Array.IndexOf(CardNumberBot, temp);
                                CardNumberBot[index] = dummyShuffle_Bowler_Bot[random];
                                randomBot = temp;
                            }
                        }

                        //=== Moving element to last =============
                        dummyShuffle_Bowler_User.Remove(randomUser);
                        dummyShuffle_Bowler_User.Add(randomUser);
                        dummyShuffle_Bowler_Bot.Remove(randomBot);
                        dummyShuffle_Bowler_Bot.Add(randomBot);
                        //===================================

                        bowlerStats_ShuffledCount--;
                        break;
                    case 2://allrounder stats
                        randomUser = dummyShuffle_Allrounder_User[Random.Range(0, allRounderStats_ShuffledCount)];
                        random = Random.Range(0, allRounderStats_ShuffledCount);
                        randomBot = dummyShuffle_Allrounder_Bot[random];
                        if (randomUser == randomBot)
                        {
                            byte temp;
                            if (allRounderStats_ShuffledCount > 1)
                            {
                                temp = dummyShuffle_Allrounder_Bot[random];
                                dummyShuffle_Allrounder_Bot[random] = dummyShuffle_Allrounder_Bot[allRounderStats_ShuffledCount - 1];
                                dummyShuffle_Allrounder_Bot[allRounderStats_ShuffledCount - 1] = temp;
                                random = Random.Range(0, (allRounderStats_ShuffledCount - 1));
                                randomBot = dummyShuffle_Allrounder_Bot[random];
                            }
                            else
                            {
                                if (randomUser != dummyShuffle_Allrounder_Bot[allRounderStats_ShuffledCount])
                                {
                                    temp = dummyShuffle_Allrounder_Bot[allRounderStats_ShuffledCount];
                                    dummyShuffle_Allrounder_Bot[allRounderStats_ShuffledCount] = dummyShuffle_Allrounder_Bot[random];
                                    dummyShuffle_Allrounder_Bot[random] = temp;
                                    random = allRounderStats_ShuffledCount;
                                }
                                else
                                {
                                    temp = dummyShuffle_Allrounder_Bot[allRounderStats_ShuffledCount + 1];
                                    dummyShuffle_Allrounder_Bot[allRounderStats_ShuffledCount + 1] = dummyShuffle_Allrounder_Bot[random];
                                    dummyShuffle_Allrounder_Bot[random] = temp;
                                    random = allRounderStats_ShuffledCount + 1;
                                }

                                byte index = (byte)System.Array.IndexOf(CardNumberBot, temp);
                                CardNumberBot[index] = dummyShuffle_Allrounder_Bot[random];
                                randomBot = temp;
                            }
                        }

                        //=== Moving element to last =============
                        dummyShuffle_Allrounder_User.Remove(randomUser);
                        dummyShuffle_Allrounder_User.Add(randomUser);
                        dummyShuffle_Allrounder_Bot.Remove(randomBot);
                        dummyShuffle_Allrounder_Bot.Add(randomBot);
                        //===================================

                        allRounderStats_ShuffledCount--;
                        break;
                    default:
                        break;
                }
                //randomUser = DummyShuffle[Random.Range(0, DummyShuffle.Count)];
                CardNumberUser[i] = randomUser;
                //randomBot = DummyShuffleBot[Random.Range(0, DummyShuffleBot.Count)];
                CardNumberBot[i] = randomBot;

                if (batsmanStats_ShuffledCount <= 0 && statsType.Contains(0))
                {
                    statsType.Remove(0);
                }

                if (bowlerStats_ShuffledCount <= 0 && statsType.Contains(1))
                {
                    statsType.Remove(1);
                }

                if (allRounderStats_ShuffledCount <= 0 && statsType.Contains(2))
                {
                    statsType.Remove(2);
                }
            }
        }

        GameObject Go;

        for (int i = 0; i < PlayerDetails.Count; i++)
        {
            randomUser = CardNumberUser[i];
            randomBot = CardNumberBot[i];

            if (NormalSuperCards[i] == null)
            {
                Go = Instantiate(CardsPrefab, UserDeck);
                NormalSuperCards[i] = Go.GetComponent<NormalSuperCards>();
                Go = Instantiate(CardsPrefabBot, BotDeck);
                NormalSuperCardsBot[i] = Go.GetComponent<NormalSuperCardsBots>();
            }

            NormalSuperCards[i].name = PlayerDetails[randomUser]["name"];
            NormalSuperCardsBot[i].name = PlayerDetails[randomBot]["name"];

            string NationTypeUser = PlayerDetails[randomUser]["nt"];
            string NationTypeBot = PlayerDetails[randomBot]["nt"];
            NormalSuperCards[i].Reset();
            if (int.Parse(PlayerDetails[randomUser]["pid"]) <= 8)
            {
                NormalSuperCards[i].playerType = 0;
                NormalSuperCards[i].isAllRounder = false;

                NormalSuperCards[i].NormalSuperCardFiller_BatsmanStats(PlayerDetails[randomUser]["name"], float.Parse(PlayerDetails[randomUser]["runs"]), float.Parse(PlayerDetails[randomUser]["_4s"]), float.Parse(PlayerDetails[randomUser]["_6s"]), float.Parse(PlayerDetails[randomUser]["_50s"]), Mathf.Round(float.Parse(PlayerDetails[randomUser]["sr"])), PlayerSprites.GetSprite(randomUser.ToString()), PlayerSprites.GetSprite(NationTypeUser), PlayerSprites.GetSprite("bat"), i);
            }
            else if (int.Parse(PlayerDetails[randomUser]["pid"]) > 8 && int.Parse(PlayerDetails[randomUser]["pid"]) < 17)
            {
                NormalSuperCards[i].playerType = 1;
                NormalSuperCards[i].isAllRounder = false;
                NormalSuperCards[i].NormalSuperCardFiller_BowlerStats(PlayerDetails[randomUser]["name"], float.Parse(PlayerDetails[randomUser]["mat"]), Mathf.Round(float.Parse(PlayerDetails[randomUser]["ove"])), float.Parse(PlayerDetails[randomUser]["wk"]), float.Parse(PlayerDetails[randomUser]["mai"]), Mathf.Round(float.Parse(PlayerDetails[randomUser]["eco"])), PlayerSprites.GetSprite(randomUser.ToString()), PlayerSprites.GetSprite(NationTypeUser), PlayerSprites.GetSprite("bowl"), i);
            }
            else
            {
                //NormalSuperCards[i].NormalSuperCardFiller_BatsmanStats(float.Parse(PlayerDetails[randomUser]["tg"]), PlayerDetails[randomUser]["name"], float.Parse(PlayerDetails[randomUser]["runs"]), float.Parse(PlayerDetails[randomUser]["_4s"]), float.Parse(PlayerDetails[randomUser]["wk"]), float.Parse(PlayerDetails[randomUser]["bsr"]), float.Parse(PlayerDetails[randomUser]["avg"]), randomUser, NationTypeUser, i);

                NormalSuperCards[i].isAllRounder = true;
                NormalSuperCards[i].NormalSuperCardFiller_BatsmanStats(PlayerDetails[randomUser]["name"], float.Parse(PlayerDetails[randomUser]["runs"]), float.Parse(PlayerDetails[randomUser]["_4s"]), float.Parse(PlayerDetails[randomUser]["_6s"]), float.Parse(PlayerDetails[randomUser]["_50s"]), Mathf.Round(float.Parse(PlayerDetails[randomUser]["sr"])), PlayerSprites.GetSprite(randomUser.ToString()), PlayerSprites.GetSprite(NationTypeUser), PlayerSprites.GetSprite("bat"), i);
                NormalSuperCards[i].NormalSuperCardFiller_BowlerStats(PlayerDetails[randomUser]["name"], float.Parse(PlayerDetails[randomUser]["mat"]), Mathf.Round(float.Parse(PlayerDetails[randomUser]["ove"])), float.Parse(PlayerDetails[randomUser]["wk"]), float.Parse(PlayerDetails[randomUser]["mai"]), (float.Parse(PlayerDetails[randomUser]["eco"])), PlayerSprites.GetSprite(randomUser.ToString()), PlayerSprites.GetSprite(NationTypeUser), PlayerSprites.GetSprite("bowl"), i);
            }
            NormalSuperCards[i].TurnOFF();

            NormalSuperCardsBot[i].Reset();
            if (int.Parse(PlayerDetails[randomBot]["pid"]) <= 8)
            {
                NormalSuperCardsBot[i].playerType = 0;
                NormalSuperCardsBot[i].isAllRounder = false;
                NormalSuperCardsBot[i].NormalSuperCardFillerBot_BatsmanStats(PlayerDetails[randomBot]["name"], float.Parse(PlayerDetails[randomBot]["runs"]), float.Parse(PlayerDetails[randomBot]["_4s"]), float.Parse(PlayerDetails[randomBot]["_6s"]), float.Parse(PlayerDetails[randomBot]["_50s"]), float.Parse(PlayerDetails[randomBot]["sr"]), PlayerSprites.GetSprite(randomBot.ToString()), PlayerSprites.GetSprite(NationTypeBot), PlayerSprites.GetSprite("bat"), i);
            }
            else if (int.Parse(PlayerDetails[randomBot]["pid"]) > 8 && int.Parse(PlayerDetails[randomBot]["pid"]) < 17)
            {
                NormalSuperCardsBot[i].playerType = 1;
                NormalSuperCardsBot[i].isAllRounder = false;
                NormalSuperCardsBot[i].NormalSuperCardFillerBot_BowlerStats(PlayerDetails[randomBot]["name"], float.Parse(PlayerDetails[randomBot]["mat"]), Mathf.Round(float.Parse(PlayerDetails[randomBot]["ove"])), float.Parse(PlayerDetails[randomBot]["wk"]), float.Parse(PlayerDetails[randomBot]["mai"]), float.Parse(PlayerDetails[randomBot]["eco"]), PlayerSprites.GetSprite(randomBot.ToString()), PlayerSprites.GetSprite(NationTypeBot), PlayerSprites.GetSprite("bowl"), i);
            }
            else
            {
                //NormalSuperCardsBot[i].NormalSuperCardFillerBot(float.Parse(PlayerDetails[randomBot]["tg"]), PlayerDetails[randomBot]["name"], float.Parse(PlayerDetails[randomBot]["runs"]), float.Parse(PlayerDetails[randomBot]["_4s"]), float.Parse(PlayerDetails[randomBot]["wk"]), float.Parse(PlayerDetails[randomBot]["bsr"]), float.Parse(PlayerDetails[randomBot]["avg"]), randomBot, NationTypeBot, i);

                NormalSuperCardsBot[i].isAllRounder = true;
                NormalSuperCardsBot[i].NormalSuperCardFillerBot_BatsmanStats(PlayerDetails[randomBot]["name"], float.Parse(PlayerDetails[randomBot]["runs"]), float.Parse(PlayerDetails[randomBot]["_4s"]), float.Parse(PlayerDetails[randomBot]["_6s"]), float.Parse(PlayerDetails[randomBot]["_50s"]), float.Parse(PlayerDetails[randomBot]["sr"]), PlayerSprites.GetSprite(randomBot.ToString()), PlayerSprites.GetSprite(NationTypeBot), PlayerSprites.GetSprite("bat"), i);
                NormalSuperCardsBot[i].NormalSuperCardFillerBot_BowlerStats(PlayerDetails[randomBot]["name"], float.Parse(PlayerDetails[randomBot]["mat"]), Mathf.Round(float.Parse(PlayerDetails[randomBot]["ove"])), float.Parse(PlayerDetails[randomBot]["wk"]), float.Parse(PlayerDetails[randomBot]["mai"]), float.Parse(PlayerDetails[randomBot]["eco"]), PlayerSprites.GetSprite(randomBot.ToString()), PlayerSprites.GetSprite(NationTypeBot), PlayerSprites.GetSprite("bowl"), i);
            }
            NormalSuperCardsBot[i].TurnOFF();
        }

        //ShuffleList();
        if (CONTROLLER.PLAYMULTIPLAYER == false)
        {
            StartNewRound();
        }
        else
        {
#if SUPERCARDS_MULTIPLAYER
            if(PhotonNetwork.IsMasterClient)
            {
                MultiplayerManager.sendShuffledCardsList(ref CardNumberUser, ref CardNumberBot);
            }
#endif
        }
    }

    public void LetUserSelect()
    {
        BotCard_IMG.DOFade(.2f, 1f);
        UserCard_IMG.DOFade(1f, 1f);

        NormalSuperCards[Count].TurnOn(firstTurnBy_InThisRound == Turn.User);
        if(firstTurnBy_InThisRound == Turn.User)
        {
            sq = DOTween.Sequence();
            //sq.Insert(0f, NormalSuperCards[Count].GetComponent<RectTransform>().DOAnchorPos(new Vector2(315, 205), 0.4f));
            sq.Insert(0f, NormalSuperCards[Count].transform.DOMove(PlayerCard_Ref1.position, 0.4f));
            sq.Insert(0f, NormalSuperCards[Count].transform.DOScale(Vector3.one * 0.9f, 0.4f));

            if (NormalSuperCards[Count].isAllRounder == false)
            {// not allrounder
                timerType = 1;
                StartCountDownTimer();
            }

            //instructionText.text = "PICK A STAT";
            //instructionText.transform.parent.gameObject.SetActive(true);
            SuperCards_Powerups.enableSuperCardsButtonInteraction(true);
        }
        else 
        {
            NormalSuperCards[Count].onClick_Value(NormalSuperCardsBot[Count].BotSelection());
            onClickSubmit_ValueButton();
        }

        NormalSuperCards[Count].FlipImage();
    }

    public void afterUserSelectingTheCurrentPlayerStats()
    {
        instructionText.text = "";
        instructionText.transform.parent.gameObject.SetActive(false);
        submitSelectedValueBtnObj.SetActive(true);
    }

    public void onClickSubmit_ValueButton()
    {
        Card_AudioSource.clip = Card_AudioClip[9];
        Card_AudioSource.Play();
        NormalSuperCards[Count].submitSelectedValue();
    }

    public void InitialzeBot()
    {
        BotCard_IMG.DOFade(1f, 1f);
        UserCard_IMG.DOFade(.2f, 1f);

        opponentSq = DOTween.Sequence();
        NormalSuperCardsBot[Count].TurnOn();
        //opponentSq.Insert(0f, NormalSuperCardsBot[Count].GetComponent<RectTransform>().DOAnchorPos(new Vector2(-328, -175), 0.4f));
        opponentSq.Insert(0.1f, NormalSuperCardsBot[Count].transform.DOMove(Opponent_Ref2.position, 0.4f)).OnComplete(Complete_onBotReach);
        opponentSq.Insert(0f, NormalSuperCardsBot[Count].transform.DOScale(Vector3.one * 1.1f, 0.4f));
        NormalSuperCardsBot[Count].FlipImage();
        // NormalSuperCardsBot[Count].CalculateTheHighest();

        if (CONTROLLER.PLAYMULTIPLAYER && firstTurnBy_InThisRound == Turn.Bot)
        {
            if (NormalSuperCardsBot[Count].isAllRounder == false)
            {// not allrounder
                timerType = 1;
                StartCountDownTimer();
            }
        }
    }

    public void Complete_onBotReach()
    {
        if(SuperCard_Bot != null)
        {
            SuperCard_Bot.DOLocalMove(new Vector3(-15f, 50f, 0f), .8f);
            SuperCard_Bot.DORotate(Vector3.zero, .8f);
            SuperCard_Bot.DOScale(.8f, .8f);
            SuperCard_Bot = null;
        }
    }

    public void HideBotCardsToUser()
    {
        if (CONTROLLER.PLAYMULTIPLAYER == false)
        {
            NormalSuperCardsBot[Count].HideValues();
        }
    }
    public void HideOldCards()
    {
        stopexitpopup = false;
        Card_Particles[0].gameObject.SetActive(false);
        scoreadd_Anim.gameObject.SetActive(false);
        scoreadd_Anim.DOAnchorPos(new Vector2(0, 0f), 1.0f);
        NormalSuperCards[Count].TurnOFF();
        NormalSuperCards[Count].ResetMe();
        NormalSuperCardsBot[Count].TurnOFF();
        Count++;

        afterRoundCompleteSwapTurns();

        StartNewRound();

        StartPlay = true;
    }

    public void checkAndStartNewRoundMultiplayer()
    {
        if(isOpponentsTurnToPlayThisRound)
        {
            StartNewRound();
        }
    }

    public void updateReconnectionLoadingScreen_Multiplayer(string msg)
    {
        if(string.IsNullOrEmpty(msg))
        {
            reconnectionBGObj.SetActive(false);
        }
        else
        {
            reconnectionBGObj.SetActive(true);
            reconnectionText.text = msg;
        }
    }

    private void StartNewRound()
    {
        Round_text.text = "ROUND : "+ (Count + 1).ToString();
        //Debug.Log("StartNewRound");
        isValidatingScores = false;
        redrawCardIndex_User = -1;
        redrawCardIndex_Bot = -1;
        appliedSuperCard_User = SuperCard_PowerUpsList.LastElement;

        submitSelectedValueBtnObj.SetActive(false);
        instructionText.text = "";
        instructionText.transform.parent.gameObject.SetActive(false);
        SuperCards_Powerups.enableSuperCardsButtonInteraction(false);
        if (turnBy == Turn.User)
        {
            if (NormalSuperCards[Count].isAllRounder)
            {
                allRounderChoiceSelectionPanel_User.SetActive(true);
                instructionText.text = "PICK A CARD";
                instructionText.transform.parent.gameObject.SetActive(true);
                Card_AudioSource.clip = Card_AudioClip[7];
                Card_AudioSource.Play();
                allRounderChoice_AsBowler_User.GetComponent<RectTransform>().DOAnchorPos(new Vector2(180, 39), 0.5f);
                allRounderChoice_AsBatsman_User.NormalSuperCardFiller_BatsmanStats(NormalSuperCards[Count].thiscardvalue.PlayerName, NormalSuperCards[Count].thiscardvalue.Runs, NormalSuperCards[Count].thiscardvalue._4s, NormalSuperCards[Count].thiscardvalue._6s, NormalSuperCards[Count].thiscardvalue._50s, NormalSuperCards[Count].thiscardvalue.StrikeRate, NormalSuperCards[Count].PlayerImage.sprite, NormalSuperCards[Count].PlayerCountry.sprite, PlayerSprites.GetSprite("bat"), Count);
                allRounderChoice_AsBowler_User.NormalSuperCardFiller_BowlerStats(NormalSuperCards[Count].thiscardvalue.PlayerName, NormalSuperCards[Count].thiscardvalue.matches, NormalSuperCards[Count].thiscardvalue.overs, NormalSuperCards[Count].thiscardvalue.wickets, NormalSuperCards[Count].thiscardvalue.maidens, NormalSuperCards[Count].thiscardvalue.economy, NormalSuperCards[Count].PlayerImage.sprite, NormalSuperCards[Count].PlayerCountry.sprite, PlayerSprites.GetSprite("bowl"), Count);

                timerType = 0;
                StartCountDownTimer();
            }
            else
            {
                LetUserSelect();
            }
        }
        else
        {
            if (CONTROLLER.PLAYMULTIPLAYER)
            {
                if (MultiplayerManager.instance.opponentIsOutOfFocus)
                {
                    MultiplayerManager.instance.reconnecting_Multiplayer(false, true);
                }
                else
                {
                    if (NormalSuperCardsBot[Count].isAllRounder)
                    {
                        allRounderChoiceSelectionPanel_Bot.SetActive(true);
                        allRounderChoice_AsBatsman_Bot.NormalSuperCardFillerBot_BatsmanStats(NormalSuperCardsBot[Count].thiscardvalue.PlayerName, NormalSuperCardsBot[Count].thiscardvalue.Runs, NormalSuperCardsBot[Count].thiscardvalue._4s, NormalSuperCardsBot[Count].thiscardvalue._6s, NormalSuperCardsBot[Count].thiscardvalue._50s, NormalSuperCardsBot[Count].thiscardvalue.StrikeRate, NormalSuperCardsBot[Count].PlayerImage.sprite, NormalSuperCardsBot[Count].PlayerCountry.sprite, PlayerSprites.GetSprite("bat"), Count);
                        allRounderChoice_AsBowler_Bot.NormalSuperCardFillerBot_BowlerStats(NormalSuperCardsBot[Count].thiscardvalue.PlayerName, NormalSuperCardsBot[Count].thiscardvalue.matches, NormalSuperCardsBot[Count].thiscardvalue.overs, NormalSuperCardsBot[Count].thiscardvalue.wickets, NormalSuperCardsBot[Count].thiscardvalue.maidens, NormalSuperCardsBot[Count].thiscardvalue.economy, NormalSuperCardsBot[Count].PlayerImage.sprite, NormalSuperCardsBot[Count].PlayerCountry.sprite, PlayerSprites.GetSprite("bowl"), Count);

                        timerType = 0;
                        StartCountDownTimer();
                    }
                    else
                    {
                        InitialzeBot();
                        instructionText.text = "OPPONENT'S TURN";
                        instructionText.transform.parent.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                if (NormalSuperCardsBot[Count].isAllRounder)
                {
                    //allRounderChoiceSelectionPanel_Bot.SetActive(true);
                    allRounderChoice_AsBatsman_Bot.NormalSuperCardFillerBot_BatsmanStats(NormalSuperCardsBot[Count].thiscardvalue.PlayerName, NormalSuperCardsBot[Count].thiscardvalue.Runs, NormalSuperCardsBot[Count].thiscardvalue._4s, NormalSuperCardsBot[Count].thiscardvalue._6s, NormalSuperCardsBot[Count].thiscardvalue._50s, NormalSuperCardsBot[Count].thiscardvalue.StrikeRate, NormalSuperCardsBot[Count].PlayerImage.sprite, NormalSuperCardsBot[Count].PlayerCountry.sprite, NormalSuperCardsBot[Count].PlayerType_Img.sprite, Count);
                    allRounderChoice_AsBowler_Bot.NormalSuperCardFillerBot_BowlerStats(NormalSuperCardsBot[Count].thiscardvalue.PlayerName, NormalSuperCardsBot[Count].thiscardvalue.matches, NormalSuperCardsBot[Count].thiscardvalue.overs, NormalSuperCardsBot[Count].thiscardvalue.wickets, NormalSuperCardsBot[Count].thiscardvalue.maidens, NormalSuperCardsBot[Count].thiscardvalue.economy, NormalSuperCardsBot[Count].PlayerImage.sprite, NormalSuperCardsBot[Count].PlayerCountry.sprite, NormalSuperCardsBot[Count].PlayerType_Img.sprite, Count);
                    onClickAllRounderChoice(Random.Range(0, 2));
                }
                instructionText.text = "OPPONENT'S TURN";
                instructionText.transform.parent.gameObject.SetActive(true);
                Invoke("getBotsRandomCardSelection", 1.0f);
            }
        }
    }

    public void onClickAllRounderChoice(int index)
    {//0 batsman, 1 bowler
        instructionText.text = "";
        instructionText.transform.parent.gameObject.SetActive(false);
        if (index == 0)
        {
            if (turnBy == Turn.User)
            {
                timerType = 1;
                NormalSuperCards[Count].playerType = 0;
                NormalSuperCardsBot[Count].playerType = 0;

                NormalSuperCards[Count].NormalSuperCardFiller_BatsmanStats(NormalSuperCards[Count].thiscardvalue.PlayerName, NormalSuperCards[Count].thiscardvalue.Runs, NormalSuperCards[Count].thiscardvalue._4s, NormalSuperCards[Count].thiscardvalue._6s, NormalSuperCards[Count].thiscardvalue._50s, NormalSuperCards[Count].thiscardvalue.StrikeRate, NormalSuperCards[Count].PlayerImage.sprite, NormalSuperCards[Count].PlayerCountry.sprite, PlayerSprites.GetSprite("bat"), Count);
                NormalSuperCardsBot[Count].NormalSuperCardFillerBot_BatsmanStats(NormalSuperCardsBot[Count].thiscardvalue.PlayerName, NormalSuperCardsBot[Count].thiscardvalue.Runs, NormalSuperCardsBot[Count].thiscardvalue._4s, NormalSuperCardsBot[Count].thiscardvalue._6s, NormalSuperCardsBot[Count].thiscardvalue._50s, NormalSuperCardsBot[Count].thiscardvalue.StrikeRate, NormalSuperCardsBot[Count].PlayerImage.sprite, NormalSuperCardsBot[Count].PlayerCountry.sprite, PlayerSprites.GetSprite("bat"), Count);

                NormalSuperCards[Count].transform.position = allRounderChoice_AsBatsman_User.transform.position;
                NormalSuperCards[Count].transform.localScale = allRounderChoice_AsBatsman_User.transform.localScale;
                LetUserSelect();
                allRounderChoiceSelectionPanel_User.SetActive(false);
                allRounderChoice_AsBowler_User.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-134, 39), 0.5f);
                if (CONTROLLER.PLAYMULTIPLAYER)
                {
                    MultiplayerManager.instance.sendMySelectionToOpponent(0, Count, 0, appliedSuperCard_User);
                }
            }
            else
            {
                NormalSuperCardsBot[Count].playerType = 0;
                NormalSuperCards[Count].playerType = 0;

                NormalSuperCardsBot[Count].NormalSuperCardFillerBot_BatsmanStats(NormalSuperCardsBot[Count].thiscardvalue.PlayerName, NormalSuperCardsBot[Count].thiscardvalue.Runs, NormalSuperCardsBot[Count].thiscardvalue._4s, NormalSuperCardsBot[Count].thiscardvalue._6s, NormalSuperCardsBot[Count].thiscardvalue._50s, NormalSuperCardsBot[Count].thiscardvalue.StrikeRate, NormalSuperCardsBot[Count].PlayerImage.sprite, NormalSuperCardsBot[Count].PlayerCountry.sprite, PlayerSprites.GetSprite("bat"), Count);
                NormalSuperCards[Count].NormalSuperCardFiller_BatsmanStats(NormalSuperCards[Count].thiscardvalue.PlayerName, NormalSuperCards[Count].thiscardvalue.Runs, NormalSuperCards[Count].thiscardvalue._4s, NormalSuperCards[Count].thiscardvalue._6s, NormalSuperCards[Count].thiscardvalue._50s, NormalSuperCards[Count].thiscardvalue.StrikeRate, NormalSuperCards[Count].PlayerImage.sprite, NormalSuperCards[Count].PlayerCountry.sprite, PlayerSprites.GetSprite("bat"), Count);

                if (CONTROLLER.PLAYMULTIPLAYER)
                {
                    allRounderChoiceSelectionPanel_Bot.SetActive(false);
                    NormalSuperCardsBot[Count].transform.position = allRounderChoice_AsBatsman_Bot.transform.position;
                    NormalSuperCardsBot[Count].transform.localScale = allRounderChoice_AsBatsman_Bot.transform.localScale;
                    InitialzeBot();
                    instructionText.text = "OPPONENT'S TURN";
                    instructionText.transform.parent.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            if (turnBy == Turn.User)
            {
                timerType = 1;
                NormalSuperCards[Count].playerType = 1;
                NormalSuperCardsBot[Count].playerType = 1;

                NormalSuperCards[Count].NormalSuperCardFiller_BowlerStats(NormalSuperCards[Count].thiscardvalue.PlayerName, NormalSuperCards[Count].thiscardvalue.matches, NormalSuperCards[Count].thiscardvalue.overs, NormalSuperCards[Count].thiscardvalue.wickets, NormalSuperCards[Count].thiscardvalue.maidens, NormalSuperCards[Count].thiscardvalue.economy, NormalSuperCards[Count].PlayerImage.sprite, NormalSuperCards[Count].PlayerCountry.sprite, PlayerSprites.GetSprite("bowl"), Count);
                NormalSuperCardsBot[Count].NormalSuperCardFillerBot_BowlerStats(NormalSuperCardsBot[Count].thiscardvalue.PlayerName, NormalSuperCardsBot[Count].thiscardvalue.matches, NormalSuperCardsBot[Count].thiscardvalue.overs, NormalSuperCardsBot[Count].thiscardvalue.wickets, NormalSuperCardsBot[Count].thiscardvalue.maidens, NormalSuperCardsBot[Count].thiscardvalue.economy, NormalSuperCardsBot[Count].PlayerImage.sprite, NormalSuperCardsBot[Count].PlayerCountry.sprite, PlayerSprites.GetSprite("bowl"), Count);

                NormalSuperCards[Count].transform.position = allRounderChoice_AsBowler_User.transform.position;
                NormalSuperCards[Count].transform.localScale = allRounderChoice_AsBowler_User.transform.localScale;
                LetUserSelect();
                allRounderChoiceSelectionPanel_User.SetActive(false);
                allRounderChoice_AsBowler_User.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-134, 39), 0.5f);
                if (CONTROLLER.PLAYMULTIPLAYER)
                {
                    MultiplayerManager.instance.sendMySelectionToOpponent(0, Count, 1, appliedSuperCard_User);
                }
            }
            else
            {
                NormalSuperCardsBot[Count].playerType = 1;
                NormalSuperCards[Count].playerType = 1;

                NormalSuperCardsBot[Count].NormalSuperCardFillerBot_BowlerStats(NormalSuperCardsBot[Count].thiscardvalue.PlayerName, NormalSuperCardsBot[Count].thiscardvalue.matches, NormalSuperCardsBot[Count].thiscardvalue.overs, NormalSuperCardsBot[Count].thiscardvalue.wickets, NormalSuperCardsBot[Count].thiscardvalue.maidens, NormalSuperCardsBot[Count].thiscardvalue.economy, NormalSuperCardsBot[Count].PlayerImage.sprite, NormalSuperCardsBot[Count].PlayerCountry.sprite, PlayerSprites.GetSprite("bowl"), Count);
                NormalSuperCards[Count].NormalSuperCardFiller_BowlerStats(NormalSuperCards[Count].thiscardvalue.PlayerName, NormalSuperCards[Count].thiscardvalue.matches, NormalSuperCards[Count].thiscardvalue.overs, NormalSuperCards[Count].thiscardvalue.wickets, NormalSuperCards[Count].thiscardvalue.maidens, NormalSuperCards[Count].thiscardvalue.economy, NormalSuperCards[Count].PlayerImage.sprite, NormalSuperCards[Count].PlayerCountry.sprite, PlayerSprites.GetSprite("bowl"), Count);

                if (CONTROLLER.PLAYMULTIPLAYER)
                {
                    allRounderChoiceSelectionPanel_Bot.SetActive(false);
                    NormalSuperCardsBot[Count].transform.position = allRounderChoice_AsBowler_Bot.transform.position;
                    NormalSuperCardsBot[Count].transform.localScale = allRounderChoice_AsBowler_Bot.transform.localScale;
                    InitialzeBot();
                    instructionText.text = "OPPONENT'S TURN";
                    instructionText.transform.parent.gameObject.SetActive(true);
                }
            }
        }
    }

    private void afterRoundCompleteSwapTurns()
    {
        if (firstTurnBy_InThisRound == Turn.User)
        {
            firstTurnBy_InThisRound = Turn.Bot;
        }
        else
        {
            firstTurnBy_InThisRound = Turn.User;
        }

        turnBy = firstTurnBy_InThisRound;
    }

    private Coroutine countDownTimerCoroutine;
    private byte timerType = 1;
    private void StartCountDownTimer()
    {
        if(countDownTimerCoroutine != null)
        {
            StopCoroutine(countDownTimerCoroutine);
        }
            Counter = 20;
        FillerText.text = Counter.ToString();
        Filler_back.DOColor(new Color32(208, 154, 8, 246), 0.01f);
        ShowTimer(true);
        Filler.transform.DORotate(new Vector3(0, 0, 360), 30.0f, RotateMode.FastBeyond360);
        countDownTimerCoroutine = StartCoroutine(StartCountdown());
    }

    private void StopCounDownTimer()
    {
        ShowTimer(false);
        if(countDownTimerCoroutine != null)
        {
            StopCoroutine(countDownTimerCoroutine);
        }
    }

    IEnumerator StartCountdown()
    {
        while(Counter >= 0)
        {
            FillerText.text = Counter.ToString();
            yield return new WaitForSeconds(1f);
            Counter--;

            if (timerType == 0 && turnBy == Turn.User && Counter <= 10)
            {
                onClickAllRounderChoice(Random.Range(0, 2));
            }

            if(Counter <= 8)
            {
                Filler_back.DOColor(new Color32(255, 48, 0, 246), 2.0f);
            }
        }

        if(Counter <= 0)
        {
            if(turnBy == Turn.User)
            {
                NormalSuperCards[Count].Buttoninteract(false);
                //if (timerType == 0)
                //{
                //    onClickAllRounderChoice(Random.Range(0, 2));
                //}
                //else
                //{
                    NormalSuperCards[Count].PushARandomValueUser();
                //}
            }
        }
    }

    //private bool AFKOrNot;

    public void GetSelectedValueUser(float Value)
    {
        //SuperCards_Powerups.enablePowerCardsUndoButton(false);
        submitSelectedValueBtnObj.SetActive(false);
        instructionText.text = "";
        instructionText.transform.parent.gameObject.SetActive(false);
        SuperCards_Powerups.enableSuperCardsButtonInteraction(false);
        StopCounDownTimer();

        //sq.Insert(0.1f, NormalSuperCards[Count].GetComponent<RectTransform>().DOAnchorPos(new Vector2(365, 205), 0.4f));
        sq.Insert(0.1f, NormalSuperCards[Count].transform.DOMove(PlayerCard_Ref2.position, 0.4f));
        //sq.Insert(0.1f, NormalSuperCards[Count].GetComponent<RectTransform>().DOScale(Vector3.one * 1.1f, 0.4f));
        sq.Insert(0.1f, NormalSuperCards[Count].transform.DOScale(Vector3.one * 1.1f, 0.4f));

        UserSelectedValue = Value;
        //AFKOrNot = _AFKOrNot;

        if (CONTROLLER.PLAYMULTIPLAYER && firstTurnBy_InThisRound == Turn.User && turnBy == Turn.User)
        {
            MultiplayerManager.instance.sendMySubmissionToOpponent(NormalSuperCards[Count].playerType, Count, NormalSuperCards[Count].GiveSelectedValue(), appliedSuperCard_User);
            multiplayerWaitForACKCoroutine = StartCoroutine(waitForAckToReceiveFromOpponent_Multiplayer(2));
        }
        else
        {
            checkForRoundCompletionOrTurnChange();
        }
    }

    public void stopWaitACKCoroutine()
    {
        reconnectionBGObj.SetActive(false);
        if (multiplayerWaitForACKCoroutine != null)
        {
            StopCoroutine(multiplayerWaitForACKCoroutine);
        }
    }

    private byte super_ballSelection_ArrayIndex, super_ball_SelectedValue;
    public void waitForAckForSuperBallSelection(byte arrayIndex, byte Value)
    {
        super_ballSelection_ArrayIndex = arrayIndex;
        super_ball_SelectedValue = Value;
        multiplayerWaitForACKCoroutine = StartCoroutine(waitForAckToReceiveFromOpponent_Multiplayer(0));
    }

    IEnumerator waitForAckToReceiveFromOpponent_Multiplayer(byte type)
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(2f);

            reconnectionBGObj.SetActive(true);
            reconnectionText.text = "waiting for opponent...";
            if (type == 2)
            {
                MultiplayerManager.instance.sendMySubmissionToOpponent(NormalSuperCards[Count].playerType, Count, NormalSuperCards[Count].GiveSelectedValue(), appliedSuperCard_User);
            }
            else if(type == 0)
            {
                MultiplayerManager.instance.sendMySelectionInSuperBallToOpponent(super_ballSelection_ArrayIndex, super_ball_SelectedValue);
            }
        }
    }

    public void checkForRoundCompletionOrTurnChange()
    {
        if(firstTurnBy_InThisRound == Turn.User)
        {
            if (turnBy == Turn.User)
            {
                turnBy = Turn.Bot;
                //getBotsRandomCardSelection();

                NormalSuperCardsBot[Count].UpdateUserSelectionToBot(NormalSuperCards[Count].GiveSelectedValue());
                Invoke("InitialzeBot", 2.0f);
            }
            else
            {
                isValidatingScores = true;
                Invoke("CompareValues", 6f);
            }
        }
        else
        {
            if (turnBy == Turn.User)
            {
                isValidatingScores = true;
                Invoke("CompareValues", 5f);
            }
            else
            {
                turnBy = Turn.User;
                Invoke("LetUserSelect", 2.0f);
            }
        }
    }

    private void getBotsRandomCardSelection()
    {
        if (BotSuperCards.Count > 0 && Random.Range(0, 100) > 31)
        {
            int RandomSuperCard = Random.Range(0, BotSuperCards.Count);
            applyBotSuperCard(BotSuperCards[RandomSuperCard]);
            BotSuperCards.RemoveAt(RandomSuperCard);
        }
        else 
        {
            int Calc = Random.Range(0, 100);  
            if (Calc >= 0 && Calc <= 70)
            {
                NormalSuperCardsBot[Count].CalculateTheHighest();
            }
            else if (Calc >= 71 && Calc <= 80)
            {
                NormalSuperCardsBot[Count].CalculateTheLowest();//Easy Bot
            }
            else //if (Calc >= 81)
            {
                NormalSuperCardsBot[Count].PushARandomValue();
                //NormalSuperCardsBot[Count].CalculateTheHighest();
            }
        }
        InitialzeBot();
    }

    public void applyBotSuperCard(SuperCard_PowerUpsList botCard)
    {
        SuperCard_Bot = Instantiate(SuperCards_Powerups.Bot_supercardImages[(byte)botCard].GetComponent<RectTransform>());
        SuperCard_Bot.DOScale(new Vector3(1f, 1f, 1f), .001f);
        SuperCard_Bot.position = SuperCards_Powerups.Bot_supercardImages[(byte)botCard].transform.position;
        SuperCard_Bot.SetParent(NormalSuperCardsBot[Count].SuperCardBotHolder);
        //SuperCard_Bot.DOLocalMove(new Vector3(-15f, 50f, 0f), .8f);
        //SuperCard_Bot.DORotate(Vector3.zero,.8f);
        //SuperCard_Bot.DOScale(.8f,.8f);
        //SuperCard_Bot.localPosition = new Vector3(-15f, 50f, 0f);
        //SuperCard_Bot.localEulerAngles = Vector3.zero;
        //SuperCard_Bot.localScale = new Vector3(.8f, .8f, .8f);

        switch (botCard)
        {
            case SuperCard_PowerUpsList.Exchange:
                StartCoroutine(ExchangeNormalCardUser());
                SuperCards_Powerups.Bot_selectedSupercard((byte)botCard);
                break;
            case SuperCard_PowerUpsList.ReDraw:
                StartCoroutine(ReDrawCardUser());
                SuperCards_Powerups.Bot_selectedSupercard((byte)botCard);
                break;
            case SuperCard_PowerUpsList.GoWild:
                StartCoroutine(Go_Wildcard());
                SuperCards_Powerups.Bot_selectedSupercard((byte)botCard);
                break;
            default:
                break;
        }
    }
   
    public void GetSelectedValueBot(float Value)
    {
        BotSelectedValue = Value;
        StopCounDownTimer();
        checkForRoundCompletionOrTurnChange();
    }

    public void Winning_Anim()
    {
        stopexitpopup = true;
        Big_Image.sprite = Round_Sprite[0];
        Small_Image.sprite = Round_Sprite[3];
        Big_status.text = "YOU WON";
        Big_status.color = new Color32(0, 81, 254, 255);
        Small_status.text = "OPPONENT LOST";
        Small_status.color = new Color32(255, 255, 255, 255);
        Big_stats.color = new Color32(0, 81, 254, 255);
        Small_stats.color = new Color32(255, 255, 255, 255);
        if (!issuperball)
        {
            ScoreUser++;
            ScoreUserText.text = ScoreUser.ToString() + "/11";
            Big_stats.text = UserSelectedValue.ToString();
            Small_stats.text = BotSelectedValue.ToString();
            Big_Label.text = NormalSuperCards[Count].button_Label[NormalSuperCards[Count].GiveSelectedValue()].text;
            Small_Label.text = NormalSuperCardsBot[Count].button_Label[NormalSuperCardsBot[Count].BotSelection()].text;
        }
        else
        {
            Big_stats.text = SuperBallUI.UserValue.ToString();
            Small_stats.text = SuperBallUI.BotValue.ToString();
            Big_Label.text = "RUNS".ToString();
            Small_Label.text = "RUNS".ToString();
        }
        
        Big_Label.color = new Color32(0, 81, 254, 255);
        Small_Label.color = new Color32(255, 255, 255, 255);
        RoundWonScreen.gameObject.SetActive(true);
        RoundAnimation.SetActive(true);
        sq = DOTween.Sequence();
        sq.Insert(0f, RoundWonScreen_main.DOFade(1f, 1f));
        sq.Insert(0f, RoundWonScreen.DOScale(new Vector3(1f, 1f, 1f), 0.7f));
        Card_Particles[1].gameObject.SetActive(true);
        Card_AudioSource.clip = Card_AudioClip[0];
        Card_AudioSource.Play();
        Card_Particles[1].startColor = new Color32(255, 255, 0, 255);
        Card_Particles[1].Play();
        sq.AppendInterval(2f);
        sq.AppendCallback(() =>
        {
            if (!issuperball)
            {
                if (ScoreUser >= 11)
                {
                    Card_Particles[1].gameObject.SetActive(false);
                    ShowGameOverScreen(true);
                }
                else
                {
                    Card_Particles[1].gameObject.SetActive(false);
                    Card_Particles[0].gameObject.SetActive(true);
                    Card_Particles[0].Play();
                    scoreadd_Anim.gameObject.SetActive(true);
                    scoreadd_Anim.DOAnchorPos(new Vector2(0, 1000f), 3.0f);

                    MoveOldCards(1f);
                }
            }
            else
            {
                IncreaseScoreForUserSuperBall(true);
                Card_Particles[1].gameObject.SetActive(false);
                Card_Particles[0].gameObject.SetActive(true);
                Card_Particles[0].Play();
                scoreadd_Anim.gameObject.SetActive(true);
                scoreadd_Anim.DOAnchorPos(new Vector2(0, 1000f), 3.0f);
            }
            issuperball = false;
        });
    }

    public void Losing_Anim()
    {
        stopexitpopup = true;
        Big_Image.sprite = Round_Sprite[1];
        Small_Image.sprite = Round_Sprite[2];
        Big_status.text = "OPPONENT WON";
        Big_status.color = new Color32(255, 255, 255, 255);
        Small_status.text = "YOU LOST";
        Small_status.color = new Color32(0, 81, 254, 255);
        Big_stats.color = new Color32(255, 255, 255, 255);
        Small_stats.color = new Color32(0, 81, 254, 255);
        if (!issuperball)
        {
            ScoreBot++;
            ScoreBotText.text = ScoreBot.ToString() + "/11";
            Big_stats.text = BotSelectedValue.ToString();
            Small_stats.text = UserSelectedValue.ToString();
            Big_Label.text = NormalSuperCardsBot[Count].button_Label[NormalSuperCardsBot[Count].BotSelection()].text;
            Small_Label.text = NormalSuperCards[Count].button_Label[NormalSuperCards[Count].GiveSelectedValue()].text;
        }
        else
        {
            Big_stats.text = SuperBallUI.BotValue.ToString();
            Small_stats.text = SuperBallUI.UserValue.ToString();
            Big_Label.text = "RUNS".ToString();
            Small_Label.text = "RUNS".ToString();
        }

        Big_Label.color = new Color32(255, 255, 255, 255);
        Small_Label.color = new Color32(0, 81, 254, 255);
        RoundWonScreen.gameObject.SetActive(true);
        RoundAnimation.SetActive(true);
        sq = DOTween.Sequence();
        sq.Insert(0f, RoundWonScreen_main.DOFade(1f, 1f));
        sq.Insert(0f, RoundWonScreen.DOScale(new Vector3(1f, 1f, 1f), 0.7f));
        Card_Particles[1].gameObject.SetActive(true);
        Card_AudioSource.clip = Card_AudioClip[1];
        Card_AudioSource.Play();
        Card_Particles[1].startColor = new Color32(0, 0, 255, 255);
        Card_Particles[1].Play();
        sq.AppendInterval(2f);
        sq.AppendCallback(() =>
        {
            if (!issuperball)
            {
                if (ScoreBot >= 11)
                {
                    Card_Particles[1].gameObject.SetActive(false);
                    ShowGameOverScreen(false);

                }
                else
                {
                    Card_Particles[1].gameObject.SetActive(false);
                    MoveOldCards(1f);
                }
            }
            else
            {
                IncreaseScoreForUserSuperBall(false);
            }
            issuperball = false;
        });
    }

    public void CompareValues()
    {
        bool Economy_Cond = true;
        if (NormalSuperCards[Count].playerType == 1 && NormalSuperCards[Count].GiveSelectedValue() == 4)
        {
            Economy_Cond = false;
        }
        else
        {
            Economy_Cond = true;
        }
        if (UserSelectedValue == BotSelectedValue)
        {
            isValidatingScores = false;
            SuperBallUI.ShowMe();
            //SUPER BALL
        }
        
        else if ((UserSelectedValue > BotSelectedValue) == Economy_Cond)
        {
            //SuperBallUI.ShowMe();
            //return;
            Winning_Anim();
        }
        else
        {
            //SuperBallUI.ShowMe();
            //return;
            Losing_Anim();
        }
    }
    public void IncreaseScoreForUserSuperBall(bool _value)
    {
        if (_value)
        {
            ScoreUser++;
            ScoreUserText.text = ScoreUser.ToString() + "/11";
            if (ScoreUser >= 11)
            {
                sq = DOTween.Sequence();
                sq.AppendInterval(3f);
                sq.AppendCallback(() =>
                { 
                    ShowGameOverScreen(true);
                });
            }
            else
            {
                MoveOldCards(1f);
            }
        }
        else
        {
            ScoreBot++;
            ScoreBotText.text = ScoreBot.ToString() + "/11";
            if (ScoreBot >= 11)
            {
                sq = DOTween.Sequence();
                sq.AppendInterval(3f);
                sq.AppendCallback(() =>
                {
                    ShowGameOverScreen(false);
                });
            }
            else
            {
                MoveOldCards(1f);
            }
        }
    }
    public void MoveOldCards(float delay)
    {
        RoundWonScreen_main.DOFade(0f, 1f);
        RoundWonScreen.DOScale(new Vector3(0f, 0f, 0f), 0.5f);
        RoundAnimation.SetActive(false);
        sq = DOTween.Sequence();
        sq.Insert(delay + 0.1f, NormalSuperCards[Count].GetComponent<RectTransform>().DOAnchorPosY(1500, 1f));
        sq.Insert(delay + 0.1f, NormalSuperCardsBot[Count].GetComponent<RectTransform>().DOAnchorPosY(-1500, 1f));
        sq.AppendInterval(delay);
        sq.AppendCallback(() =>
        {
            HideOldCards();
        });
    }
    private void ShowTimer(bool _Active)
    {
        FillerGAM.SetActive(_Active);
    }
    public void AskBotToCalculate()
    {
        NormalSuperCardsBot[Count].CalculateTheHighest();
    }
    public IEnumerator ExchangeNormalCardUser(bool isUndo = false)
    {
        float time_value;
        if (turnBy == Turn.User)
        {
            time_value = 1.0f;
            yield return new WaitForSeconds(time_value);
        }
        else
        {
            time_value = 0.01f;
        }
        yield return new WaitForSeconds(time_value);
        var tempCard = new CardValues();

        tempCard.PlayerName = NormalSuperCards[Count].thiscardvalue.PlayerName;
        Sprite playerImage = NormalSuperCards[Count].PlayerImage.sprite;
        Sprite playerCountry = NormalSuperCards[Count].PlayerCountry.sprite;
        Sprite PlayerType_Img = NormalSuperCards[Count].PlayerType_Img.sprite;

        if (NormalSuperCards[Count].playerType == 0)
        {
            tempCard.Runs = NormalSuperCards[Count].thiscardvalue.Runs;
            tempCard._4s = NormalSuperCards[Count].thiscardvalue._4s;
            tempCard._6s = NormalSuperCards[Count].thiscardvalue._6s;
            tempCard._50s = NormalSuperCards[Count].thiscardvalue._50s;
            tempCard.StrikeRate = NormalSuperCards[Count].thiscardvalue.StrikeRate;

            NormalSuperCards[Count].NormalSuperCardFiller_BatsmanStats(NormalSuperCardsBot[Count].thiscardvalue.PlayerName, NormalSuperCardsBot[Count].thiscardvalue.Runs, NormalSuperCardsBot[Count].thiscardvalue._4s, NormalSuperCardsBot[Count].thiscardvalue._6s, NormalSuperCardsBot[Count].thiscardvalue._50s, NormalSuperCardsBot[Count].thiscardvalue.StrikeRate, NormalSuperCardsBot[Count].PlayerImage.sprite, NormalSuperCardsBot[Count].PlayerCountry.sprite,NormalSuperCardsBot[Count].PlayerType_Img.sprite, Count);
            NormalSuperCardsBot[Count].NormalSuperCardFillerBot_BatsmanStats(tempCard.PlayerName, tempCard.Runs, tempCard._4s, tempCard._6s, tempCard._50s, tempCard.StrikeRate, playerImage, playerCountry, PlayerType_Img, Count);
        }
        else
        {
            tempCard.matches = NormalSuperCards[Count].thiscardvalue.matches;
            tempCard.overs = NormalSuperCards[Count].thiscardvalue.overs;
            tempCard.wickets = NormalSuperCards[Count].thiscardvalue.wickets;
            tempCard.maidens = NormalSuperCards[Count].thiscardvalue.maidens;
            tempCard.economy = NormalSuperCards[Count].thiscardvalue.economy;

            NormalSuperCards[Count].NormalSuperCardFiller_BowlerStats(NormalSuperCardsBot[Count].thiscardvalue.PlayerName, NormalSuperCardsBot[Count].thiscardvalue.matches, NormalSuperCardsBot[Count].thiscardvalue.overs, NormalSuperCardsBot[Count].thiscardvalue.wickets, NormalSuperCardsBot[Count].thiscardvalue.maidens, NormalSuperCardsBot[Count].thiscardvalue.economy, NormalSuperCardsBot[Count].PlayerImage.sprite, NormalSuperCardsBot[Count].PlayerCountry.sprite, NormalSuperCardsBot[Count].PlayerType_Img.sprite, Count);
            NormalSuperCardsBot[Count].NormalSuperCardFillerBot_BowlerStats(tempCard.PlayerName, tempCard.matches, tempCard.overs, tempCard.wickets, tempCard.maidens, tempCard.economy, playerImage, playerCountry, PlayerType_Img, Count);
        }
        
        if (turnBy == Turn.User)
        {
            if (isUndo)
            {
                appliedSuperCard_User = SuperCard_PowerUpsList.LastElement;
            }
            else
            {
                appliedSuperCard_User = SuperCard_PowerUpsList.Exchange;
            }
        }
        else if (CONTROLLER.PLAYMULTIPLAYER == false)
        { 
            NormalSuperCardsBot[Count].CalculateTheHighest();
        }
    }

    public IEnumerator Go_Wildcard(bool isUndo = false)
    {
        float time_value;
        if (turnBy == Turn.User)
        {
            time_value = 1.0f;
            yield return new WaitForSeconds(time_value);
        }
        else
        {
            time_value = 0.01f;
        }
        yield return new WaitForSeconds(time_value);
        //var tempCard = new CardValues();
        if (turnBy == Turn.User)
        {
            if (NormalSuperCards[Count].playerType == 0)
            {
                if (isUndo)
                {
                    NormalSuperCards[Count].UpdateBatsmanStatsText(NormalSuperCards[Count].thiscardvalue.PlayerName, NormalSuperCards[Count].thiscardvalue.Runs, NormalSuperCards[Count].thiscardvalue._4s, NormalSuperCards[Count].thiscardvalue._6s, NormalSuperCards[Count].thiscardvalue._50s, NormalSuperCards[Count].thiscardvalue.StrikeRate, NormalSuperCards[Count].PlayerImage.sprite, NormalSuperCards[Count].PlayerCountry.sprite, NormalSuperCards[Count].PlayerType_Img.sprite, Count);
                    appliedSuperCard_User = SuperCard_PowerUpsList.LastElement;
                }
                else
                {
                    float factor = 1.25f;
                    NormalSuperCards[Count].UpdateBatsmanStatsText(NormalSuperCards[Count].thiscardvalue.PlayerName, Mathf.Round(NormalSuperCards[Count].thiscardvalue.Runs * factor), Mathf.Round(NormalSuperCards[Count].thiscardvalue._4s * factor), Mathf.Round(NormalSuperCards[Count].thiscardvalue._6s * factor), Mathf.Round(NormalSuperCards[Count].thiscardvalue._50s * factor), Mathf.Round(NormalSuperCards[Count].thiscardvalue.StrikeRate * factor), NormalSuperCards[Count].PlayerImage.sprite, NormalSuperCards[Count].PlayerCountry.sprite, NormalSuperCards[Count].PlayerType_Img.sprite, Count);
                    appliedSuperCard_User = SuperCard_PowerUpsList.GoWild;
                }
            }
            else
            {
                if (isUndo)
                {
                    NormalSuperCards[Count].UpdateBowlerStatsText(NormalSuperCards[Count].thiscardvalue.PlayerName, NormalSuperCards[Count].thiscardvalue.matches, NormalSuperCards[Count].thiscardvalue.overs, NormalSuperCards[Count].thiscardvalue.wickets, NormalSuperCards[Count].thiscardvalue.maidens, NormalSuperCards[Count].thiscardvalue.economy, NormalSuperCards[Count].PlayerImage.sprite, NormalSuperCards[Count].PlayerCountry.sprite, NormalSuperCards[Count].PlayerType_Img.sprite, Count);
                    appliedSuperCard_User = SuperCard_PowerUpsList.LastElement;
                }
                else
                {
                    float factor = 1.25f;
                    NormalSuperCards[Count].UpdateBowlerStatsText(NormalSuperCards[Count].thiscardvalue.PlayerName, Mathf.Round(NormalSuperCards[Count].thiscardvalue.matches * factor), Mathf.Round(NormalSuperCards[Count].thiscardvalue.overs * factor), Mathf.Round(NormalSuperCards[Count].thiscardvalue.wickets * factor), Mathf.Round(NormalSuperCards[Count].thiscardvalue.maidens * factor), Mathf.Round(NormalSuperCards[Count].thiscardvalue.economy / factor), NormalSuperCards[Count].PlayerImage.sprite, NormalSuperCards[Count].PlayerCountry.sprite, NormalSuperCards[Count].PlayerType_Img.sprite, Count);
                    appliedSuperCard_User = SuperCard_PowerUpsList.GoWild;
                }
            }
        }
        else
        {
            if (NormalSuperCardsBot[Count].playerType == 0)
            {
                if (isUndo)
                {
                    NormalSuperCardsBot[Count].UpdateBatsmanStatsText(NormalSuperCardsBot[Count].thiscardvalue.PlayerName, NormalSuperCardsBot[Count].thiscardvalue.Runs, NormalSuperCardsBot[Count].thiscardvalue._4s, NormalSuperCardsBot[Count].thiscardvalue._6s, NormalSuperCardsBot[Count].thiscardvalue._50s, NormalSuperCardsBot[Count].thiscardvalue.StrikeRate, NormalSuperCardsBot[Count].PlayerImage.sprite, NormalSuperCardsBot[Count].PlayerCountry.sprite, NormalSuperCardsBot[Count].PlayerType_Img.sprite, Count);
                }
                else
                {
                    float factor = 1.25f;
                    NormalSuperCardsBot[Count].UpdateBatsmanStatsText(NormalSuperCardsBot[Count].thiscardvalue.PlayerName, Mathf.Round(NormalSuperCardsBot[Count].thiscardvalue.Runs * factor), Mathf.Round(NormalSuperCardsBot[Count].thiscardvalue._4s * factor), Mathf.Round(NormalSuperCardsBot[Count].thiscardvalue._6s * factor), Mathf.Round(NormalSuperCardsBot[Count].thiscardvalue._50s * factor), Mathf.Round(NormalSuperCardsBot[Count].thiscardvalue.StrikeRate * factor), NormalSuperCardsBot[Count].PlayerImage.sprite, NormalSuperCardsBot[Count].PlayerCountry.sprite, NormalSuperCardsBot[Count].PlayerType_Img.sprite, Count);
                }
            }
            else
            {
                if (isUndo)
                {
                    NormalSuperCardsBot[Count].UpdateBowlerStatsText(NormalSuperCardsBot[Count].thiscardvalue.PlayerName, NormalSuperCardsBot[Count].thiscardvalue.matches, NormalSuperCardsBot[Count].thiscardvalue.overs, NormalSuperCardsBot[Count].thiscardvalue.wickets, NormalSuperCardsBot[Count].thiscardvalue.maidens, NormalSuperCardsBot[Count].thiscardvalue.economy, NormalSuperCardsBot[Count].PlayerImage.sprite, NormalSuperCardsBot[Count].PlayerCountry.sprite, NormalSuperCardsBot[Count].PlayerType_Img.sprite, Count);
                }
                else
                {
                    float factor = 1.25f;
                    NormalSuperCardsBot[Count].UpdateBowlerStatsText(NormalSuperCardsBot[Count].thiscardvalue.PlayerName, Mathf.Round(NormalSuperCardsBot[Count].thiscardvalue.matches * factor), Mathf.Round(NormalSuperCardsBot[Count].thiscardvalue.overs * factor), Mathf.Round(NormalSuperCardsBot[Count].thiscardvalue.wickets * factor), Mathf.Round(NormalSuperCardsBot[Count].thiscardvalue.maidens * factor), Mathf.Round(NormalSuperCardsBot[Count].thiscardvalue.economy / factor), NormalSuperCardsBot[Count].PlayerImage.sprite, NormalSuperCardsBot[Count].PlayerCountry.sprite, NormalSuperCardsBot[Count].PlayerType_Img.sprite, Count);
                }
            }

            if(CONTROLLER.PLAYMULTIPLAYER == false)
            {
                NormalSuperCardsBot[Count].CalculateTheHighest();
            }
        }
    }

    public SuperCard_PowerUpsList appliedSuperCard_User;

    private void getRedrawCardIndex_User()
    {
        redrawCardIndex_User = Count;// Random.Range(Count, NormalSuperCardsBot.Length);
        for (int j = Count + 1; j < NormalSuperCards.Length; j++)
        {
            if (NormalSuperCards[Count].isAllRounder == NormalSuperCards[j].isAllRounder)
            {
                if (NormalSuperCards[Count].playerType == NormalSuperCards[j].playerType)
                {
                    redrawCardIndex_User = j;
                    break;
                }
            }
        }
    }

    private void getReDrawIndex_Bot()
    {
        redrawCardIndex_Bot = Count;// Random.Range(Count, NormalSuperCardsBot.Length);
        for (int j = Count + 1; j < NormalSuperCardsBot.Length; j++)
        {
            if (NormalSuperCardsBot[Count].isAllRounder == NormalSuperCardsBot[j].isAllRounder)
            {
                if (NormalSuperCardsBot[Count].playerType == NormalSuperCardsBot[j].playerType)
                {
                    redrawCardIndex_Bot = j;
                    break;
                }
            }
        }
    }

    private void Apply_ReDrawSuperCard_User(int card1Index, int card2Index)
    {
        var tempCard = new CardValues();

        tempCard.PlayerName = NormalSuperCards[card1Index].thiscardvalue.PlayerName;
        Sprite playerImage = NormalSuperCards[card1Index].PlayerImage.sprite;
        Sprite playerCountry = NormalSuperCards[card1Index].PlayerCountry.sprite;
        Sprite PlayerType_Img = NormalSuperCards[card1Index].PlayerType_Img.sprite;

        if (NormalSuperCards[card1Index].playerType == 0)
        {
            tempCard.Runs = NormalSuperCards[card1Index].thiscardvalue.Runs;
            tempCard._4s = NormalSuperCards[card1Index].thiscardvalue._4s;
            tempCard._6s = NormalSuperCards[card1Index].thiscardvalue._6s;
            tempCard._50s = NormalSuperCards[card1Index].thiscardvalue._50s;
            tempCard.StrikeRate = NormalSuperCards[card1Index].thiscardvalue.StrikeRate;

            NormalSuperCards[card1Index].NormalSuperCardFiller_BatsmanStats(NormalSuperCards[card2Index].thiscardvalue.PlayerName, NormalSuperCards[card2Index].thiscardvalue.Runs, NormalSuperCards[card2Index].thiscardvalue._4s, NormalSuperCards[card2Index].thiscardvalue._6s, NormalSuperCards[card2Index].thiscardvalue._50s, NormalSuperCards[card2Index].thiscardvalue.StrikeRate, NormalSuperCards[card2Index].PlayerImage.sprite, NormalSuperCards[card2Index].PlayerCountry.sprite, NormalSuperCards[card2Index].PlayerType_Img.sprite, card1Index);
            NormalSuperCards[card2Index].NormalSuperCardFiller_BatsmanStats(tempCard.PlayerName, tempCard.Runs, tempCard._4s, tempCard._6s, tempCard._50s, tempCard.StrikeRate, playerImage, playerCountry, PlayerType_Img, card2Index);
        }
        else
        {
            tempCard.matches = NormalSuperCards[card1Index].thiscardvalue.matches;
            tempCard.overs = NormalSuperCards[card1Index].thiscardvalue.overs;
            tempCard.wickets = NormalSuperCards[card1Index].thiscardvalue.wickets;
            tempCard.maidens = NormalSuperCards[card1Index].thiscardvalue.maidens;
            tempCard.economy = NormalSuperCards[card1Index].thiscardvalue.economy;

            NormalSuperCards[card1Index].NormalSuperCardFiller_BowlerStats(NormalSuperCards[card2Index].thiscardvalue.PlayerName, NormalSuperCards[card2Index].thiscardvalue.matches, NormalSuperCards[card2Index].thiscardvalue.overs, NormalSuperCards[card2Index].thiscardvalue.wickets, NormalSuperCards[card2Index].thiscardvalue.maidens, NormalSuperCards[card2Index].thiscardvalue.economy, NormalSuperCards[card2Index].PlayerImage.sprite, NormalSuperCards[card2Index].PlayerCountry.sprite, NormalSuperCards[card2Index].PlayerType_Img.sprite, card1Index);
            NormalSuperCards[card2Index].NormalSuperCardFiller_BowlerStats(tempCard.PlayerName, tempCard.matches, tempCard.overs, tempCard.wickets, tempCard.maidens, tempCard.economy, playerImage, playerCountry, PlayerType_Img, card2Index);
        }
    }

    private void Apply_ReDrawSuperCard_Bot(int card1Index, int card2Index)
    {
        var tempCard = new CardValues();

        tempCard.PlayerName = NormalSuperCardsBot[card1Index].thiscardvalue.PlayerName;
        Sprite playerImage = NormalSuperCardsBot[card1Index].PlayerImage.sprite;
        Sprite playerCountry = NormalSuperCardsBot[card1Index].PlayerCountry.sprite;
        Sprite PlayerType_Img = NormalSuperCardsBot[card1Index].PlayerType_Img.sprite;

        if (NormalSuperCardsBot[card1Index].playerType == 0)
        {
            tempCard.Runs = NormalSuperCardsBot[card1Index].thiscardvalue.Runs;
            tempCard._4s = NormalSuperCardsBot[card1Index].thiscardvalue._4s;
            tempCard._6s = NormalSuperCardsBot[card1Index].thiscardvalue._6s;
            tempCard._50s = NormalSuperCardsBot[card1Index].thiscardvalue._50s;
            tempCard.StrikeRate = NormalSuperCardsBot[card1Index].thiscardvalue.StrikeRate;

            NormalSuperCardsBot[card1Index].NormalSuperCardFillerBot_BatsmanStats(NormalSuperCardsBot[card2Index].thiscardvalue.PlayerName, NormalSuperCardsBot[card2Index].thiscardvalue.Runs, NormalSuperCardsBot[card2Index].thiscardvalue._4s, NormalSuperCardsBot[card2Index].thiscardvalue._6s, NormalSuperCardsBot[card2Index].thiscardvalue._50s, NormalSuperCardsBot[card2Index].thiscardvalue.StrikeRate, NormalSuperCardsBot[card2Index].PlayerImage.sprite, NormalSuperCardsBot[card2Index].PlayerCountry.sprite, NormalSuperCardsBot[card2Index].PlayerType_Img.sprite, card1Index);
            NormalSuperCardsBot[card2Index].NormalSuperCardFillerBot_BatsmanStats(tempCard.PlayerName, tempCard.Runs, tempCard._4s, tempCard._6s, tempCard._50s, tempCard.StrikeRate, playerImage, playerCountry, PlayerType_Img, card2Index);
        }
        else
        {
            tempCard.matches = NormalSuperCardsBot[card1Index].thiscardvalue.matches;
            tempCard.overs = NormalSuperCardsBot[card1Index].thiscardvalue.overs;
            tempCard.wickets = NormalSuperCardsBot[card1Index].thiscardvalue.wickets;
            tempCard.maidens = NormalSuperCardsBot[card1Index].thiscardvalue.maidens;
            tempCard.economy = NormalSuperCardsBot[card1Index].thiscardvalue.economy;

            NormalSuperCardsBot[card1Index].NormalSuperCardFillerBot_BowlerStats(NormalSuperCardsBot[card2Index].thiscardvalue.PlayerName, NormalSuperCardsBot[card2Index].thiscardvalue.matches, NormalSuperCardsBot[card2Index].thiscardvalue.overs, NormalSuperCardsBot[card2Index].thiscardvalue.wickets, NormalSuperCardsBot[card2Index].thiscardvalue.maidens, NormalSuperCardsBot[card2Index].thiscardvalue.economy, NormalSuperCardsBot[card2Index].PlayerImage.sprite, NormalSuperCardsBot[card2Index].PlayerCountry.sprite, NormalSuperCardsBot[card2Index].PlayerType_Img.sprite, card1Index);
            NormalSuperCardsBot[card2Index].NormalSuperCardFillerBot_BowlerStats(tempCard.PlayerName, tempCard.matches, tempCard.overs, tempCard.wickets, tempCard.maidens, tempCard.economy, playerImage, playerCountry, PlayerType_Img, card2Index);
        }
    }

    private int redrawCardIndex_User;
    private int redrawCardIndex_Bot;
    public IEnumerator ReDrawCardUser(bool isUndo = false)
    {
        float time_value;
        if (turnBy == Turn.User)
        {
            time_value = 1.0f;
            yield return new WaitForSeconds(time_value);
        }
        else
        {
            time_value = 0.01f;
        }
        yield return new WaitForSeconds(time_value);
        if (turnBy == Turn.User)
        {
            if (isUndo == false)
            {
                getRedrawCardIndex_User();
                Apply_ReDrawSuperCard_User(Count, redrawCardIndex_User);
                appliedSuperCard_User = SuperCard_PowerUpsList.ReDraw;
            }
            else
            {
                Apply_ReDrawSuperCard_User(redrawCardIndex_User, Count);
                appliedSuperCard_User = SuperCard_PowerUpsList.LastElement;
            }
        }
        else
        {
            getReDrawIndex_Bot();
            Apply_ReDrawSuperCard_Bot(redrawCardIndex_Bot, Count);

            if (CONTROLLER.PLAYMULTIPLAYER == false)
            {
                NormalSuperCardsBot[Count].CalculateTheHighest();
            }
        }
    }

    public void RevealCardUser()
    {
        NormalSuperCardsBot[Count].TurnOn();
        NormalSuperCardsBot[Count].ShowValues();
    }
    public void RevelCardBot()
    {
        NormalSuperCards[Count].ShowValues();
        NormalSuperCardsBot[Count].CalculateTheHighest();
    }
    public void HideMe()
    {
        if (watchVideoPopup.activeInHierarchy == false)
        {
            MainContent.SetActive(false);
            cardsPlayMode.SetActive(false);
            Content.SetActive(false);
            //GameModeSelector._instance.//modeSelection.SetActive(true);
            //NormalSuperCards.Clear();
            CONTROLLER.CurrentPage = "instructionpage";
        }
    }

    void ShowToast(string text)
    {
        GameObject prefabGO;
        GameObject tempGO;
        prefabGO = Resources.Load("Prefabs/Toast") as GameObject;
        tempGO = Instantiate(prefabGO) as GameObject;
        tempGO.name = "Toast";
        tempGO.GetComponent<Toast>().setMessge(text);
    }

    public void CallShowRewardedVideo()
    {
        AdIntegrate.instance.ShowRewardedVideo();
    }

    private void OnClickPlay()
    {
        AdIntegrate.instance.HideAd();
        CONTROLLER.CurrentPage = "Cardgameplay";
        AdIntegrate.instance.SetTimeScale(1f);
        cardsPlayMode.SetActive(false);
        GameScreen.SetActive(true);
        //if (CONTROLLER.BGMusicVal == 1)
        //{
        //    CONTROLLER.sndController.BgSource.volume = 0.05f;
        //}
        if (CONTROLLER.GameMusicVal == 1)
        {
            Card_AudioSource.volume = 1f;
        }
        else
        {
            Card_AudioSource.volume = 0f;
        }

        if (CONTROLLER.IsUserLoggedIn())
        {
            playerimageGame.sprite = Sprite.Create(ImageSaver.RetriveTexture("Googleplayprofpic"), new Rect(0, 0, PlayerPrefs.GetInt("Googleplayprofpic_w"), PlayerPrefs.GetInt("Googleplayprofpic_h")), new Vector2(0, 0));
        }
        else
        {
            playerimageGame.sprite = defaultimage;
        }

        BotSuperCards.Clear();
        for (byte i = 0; i < (byte)SuperCard_PowerUpsList.LastElement; i++)
        {
            BotSuperCards.Add((SuperCard_PowerUpsList)i);
            SuperCards_Powerups.Bot_Supercard_Reset(i);
        }

        ScoreUser = 0;
        ScoreBot = 0;
        ResetMe();
        SuperCards_Powerups.Reset();

    }

    //public void LoadForAll()
    //{
    //    MultiplayerManager.LoadForAll();
    //}
    public void updateMyName(string name)
    {
        PlayerNames[0].text = name;
    }

    public void updateOpponentName(string name)
    {
        PlayerNames[1].text = name;
    }
    public void TurnOnMasterButton()
    {
        MasterButton.SetActive(true);
    }
    public void OnClickPause()
    {
        if (GameOverScreen.activeInHierarchy == false && stopexitpopup == false && watchVideoPopup.activeInHierarchy== false)
        {
            CONTROLLER.CurrentPage = "Cardpausescreen";
            AdIntegrate.instance.SetTimeScale(0f);
            PauseScreen.SetActive(true);
        }
    }
    public void OnClickResume()
    {
        if (GameOverScreen.activeInHierarchy == false)
        {
            CONTROLLER.CurrentPage = "Cardgameplay";
            AdIntegrate.instance.SetTimeScale(1f);
            PauseScreen.SetActive(false);
        }
    }
    public void OnClickExit()
    {
        AdIntegrate.instance.SetTimeScale(1f);
        ResetMe();
        ManageScene.LoadScene(Scenes.MainMenu);
        GameModeSelector._instance.SelectGameMode(5);
    }
    public void GotoSetting()
    {
        settingScreen.SetActive(true);
        CONTROLLER.CurrentPage = "CardpauseSetting";
    }
    public void GotoInstuction()
    {
        instructScreen.SetActive(true);
        CONTROLLER.CurrentPage = "CardpauseInstruct";
    }
    public void Instuctionback()
    {
        instructScreen.SetActive(false);
        CONTROLLER.CurrentPage = "Cardpausescreen";
    }
    public void Settingback()
    {
        settingScreen.SetActive(false);
        CONTROLLER.CurrentPage = "Cardpausescreen";
    }

    private void addPoints(int pointToAdd)
    {
        CONTROLLER.gameSyncPoint += pointToAdd;
        CONTROLLER.gameTotalPoints += pointToAdd;
        PlayerPrefsManager.saveUserPoints();
    }

    public void ShowGameOverScreen(bool UserWon, string disconnectReason = "")
    {
        if (UserWon == true)
        {
            Win_Screen.SetActive(true);
            Lose_Screen.SetActive(false);
            Card_Particles[2].gameObject.SetActive(true);
            Card_Particles[2].Play();
            //WonUserOrBot.text = "YOU WON";
            if (CONTROLLER.IsUserLoggedIn())
            {
                WonUserOrBot.text = CONTROLLER.UserName.ToString();
                playerimage.sprite = Sprite.Create(ImageSaver.RetriveTexture("Googleplayprofpic"), new Rect(0, 0, PlayerPrefs.GetInt("Googleplayprofpic_w"), PlayerPrefs.GetInt("Googleplayprofpic_h")), new Vector2(0, 0));
            }
            else
            {
                WonUserOrBot.text = "YOU WON";
                playerimage.sprite = defaultimage;
                Coinvalue.SetActive(true);
                //UnityPHPConnector.instance.SendPointsToPHP(100);
            }
            addPoints(100);
           Card_AudioSource.clip = Card_AudioClip[2];
            Card_AudioSource.Play();
            Card_Particles[3].gameObject.SetActive(true);
            Card_Particles[3].Play();
            //FirebaseAnalyticsManager.instance.SCC_Win();
        }
        else
        {
            //WonUserOrBot.text = "YOU LOST";
            //playerimage.sprite = defaultimage;
            Win_Screen.SetActive(false);
            Lose_Screen.SetActive(true);
            Coinvalue.SetActive(false);
            Card_AudioSource.clip = Card_AudioClip[3];
            Card_AudioSource.Play();
            //FirebaseAnalyticsManager.instance.SCC_Lost();
        }

        WonUserOrBot.text += disconnectReason;
        GameOverScreen.SetActive(true);
        //FirebaseAnalyticsManager.instance.SCC_Comp();
        if (CONTROLLER.PLAYMULTIPLAYER)
        {
            MultiplayerManager.instance.disconnectPhoton();
        }
    }

    public void Click_Home()
    {
        Card_Particles[2].Stop();
        Card_Particles[2].gameObject.SetActive(false);
        
        //if (AdIntegrate.instance.checkTheInternet() && AdIntegrate.instance.isInterstitialReadyToPlay() && !CONTROLLER.isAdRemoved && CONTROLLER.launchInternetAdEvent)
        //{
        //    Adloading.SetActive(true);
        //    StartCoroutine(openAD());
        //}
        //else
        {
            OnClickMainMenuAgain(); 
        }
    }


    IEnumerator openAD()
    {
        yield return new WaitForSecondsRealtime(2.0f);
       // AdIntegrate.instance.ShowInterestialAd();
    }

    public void GotoGameOverscreen()
    {
        Adloading.SetActive(false);
        OnClickMainMenuAgain();
    }

    public void OnClickMainMenuAgain()
    {
        ResetMe();
        CONTROLLER.PLAYMULTIPLAYER = false;
        ManageScene.LoadScene(Scenes.MainMenu);
    }
    public void OnClickReplay()
    {

    }
    public void ShowCardsDetails()
    {
        AdIntegrate.instance.HideAd();
        CONTROLLER.CurrentPage = "Cardinfo";
        cardsPlayMode.SetActive(false);
        cardDetailsInfo.SetActive(true);
    }
}

[System.Serializable]
public class PlayerDetailsData
{
    public float pid;
    public string Name;
    public float tg;
    public PlayerDetailsData(float i, string j, float k)
    {
        pid = i;
        Name = j;
        tg = k;
    }
}
