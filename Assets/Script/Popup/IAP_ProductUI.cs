using UnityEngine;
using UnityEngine.UI;

public class IAP_ProductUI:MonoBehaviour
{
    public Text price;
    public Text title;
    public GameObject offerHolder;
    public Text offerText;
    public Button BuyButton;
    public Image BuyButtonTexture;
}

public class Product
{
    public string title;
    public string priceValue;
    //public string description;
    public string offerText;

    public Product()
    {
        title = "100";
        priceValue = "₹ 95.00";
        offerText = null;
    }

    public bool HasOffer()
    {
        return !string.IsNullOrEmpty(offerText);
    }
    public string GetFormattedOffer()
    {
        if (offerText != null && offerText != string.Empty)
        {
            string[] offerArray = offerText.Split('\n');
            return offerArray[0] + "\n" + offerArray[1];
        }
        else
        {
            return null;
        }
    }
}

