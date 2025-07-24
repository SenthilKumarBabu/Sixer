using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.U2D;

public class SupercarddetailsUI : MonoBehaviour
{
    // Start is called before the first frame update
    public static SupercarddetailsUI instance;
    public GameObject mainContent;
    public GameObject playerDetails;
    public GameObject powerupDetails;
    public Transform UserDeck;
    public GameObject CardsPrefab;
    public TextAsset PlayerDetails;
    public Button[] toogleButtons;
    public Sprite[] toggleSprite;
    public NormalSuperCards[] NormalSuperCards;
    public SpriteAtlas PlayerSprites;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        AdIntegrate.instance.HideAd();
        mainContent.SetActive(true);
        playerDetails.SetActive(true);
        string _data = this.PlayerDetails.text;
        SimpleJSON.JSONNode SuperCards = SimpleJSON.JSONNode.Parse(_data);
        JSONNode PlayerDetails = SuperCards["SuperCards"]["PlayerDetails"];
        for (int i = 0; i < PlayerDetails.Count; i++)
        {
            GameObject Go = Instantiate(CardsPrefab, UserDeck);
            NormalSuperCards[i] = Go.GetComponent<NormalSuperCards>();
            Go.name = PlayerDetails[i]["name"];
            string NationTypeUser = PlayerDetails[i]["nt"];
            if (int.Parse(PlayerDetails[i]["pid"]) <= 8)
            {

                NormalSuperCards[i].NormalSuperCardFiller_BatsmanStats(PlayerDetails[i]["name"], float.Parse(PlayerDetails[i]["runs"]), float.Parse(PlayerDetails[i]["_4s"]), float.Parse(PlayerDetails[i]["_6s"]), float.Parse(PlayerDetails[i]["_50s"]), Mathf.Round(float.Parse(PlayerDetails[i]["sr"])), PlayerSprites.GetSprite(i.ToString()), PlayerSprites.GetSprite(NationTypeUser), PlayerSprites.GetSprite("bat"), i);
            }
            else if (int.Parse(PlayerDetails[i]["pid"]) > 8 && int.Parse(PlayerDetails[i]["pid"]) < 17)
            {
                NormalSuperCards[i].NormalSuperCardFiller_BowlerStats(PlayerDetails[i]["name"], float.Parse(PlayerDetails[i]["mat"]), Mathf.Round(float.Parse(PlayerDetails[i]["ove"])), float.Parse(PlayerDetails[i]["wk"]), float.Parse(PlayerDetails[i]["mai"]), Mathf.Round(float.Parse(PlayerDetails[i]["eco"])), PlayerSprites.GetSprite(i.ToString()), PlayerSprites.GetSprite(NationTypeUser), PlayerSprites.GetSprite("bowl"), i);
            }
            else
            {
                NormalSuperCards[i].NormalSuperCardFiller_BatsmanStats(PlayerDetails[i]["name"], float.Parse(PlayerDetails[i]["runs"]), float.Parse(PlayerDetails[i]["_4s"]), float.Parse(PlayerDetails[i]["_6s"]), float.Parse(PlayerDetails[i]["_50s"]), Mathf.Round(float.Parse(PlayerDetails[i]["sr"])), PlayerSprites.GetSprite(i.ToString()), PlayerSprites.GetSprite(NationTypeUser), PlayerSprites.GetSprite("allrounder"), i);
            }

        }
    }
    public void Togglecard(int index)
    {
        if(index ==0)
        {
            playerDetails.SetActive(true);
            powerupDetails.SetActive(false);
            toogleButtons[0].GetComponent<Image>().sprite = toggleSprite[0];
            toogleButtons[1].GetComponent<Image>().sprite = toggleSprite[1];

        }
        else if (index == 1)
        {
            playerDetails.SetActive(false);
            powerupDetails.SetActive(true);
            toogleButtons[0].GetComponent<Image>().sprite = toggleSprite[1];
            toogleButtons[1].GetComponent<Image>().sprite = toggleSprite[0];
        }
    }
    public void CloseCarddetails()
    {
        CONTROLLER.CurrentPage = "Cardplaymenu";
        mainContent.SetActive(false);
        SuperCardsUI.instance.cardsPlayMode.SetActive(true);
    }
}
