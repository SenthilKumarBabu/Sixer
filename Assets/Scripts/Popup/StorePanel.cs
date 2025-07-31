//using Beebyte.Obfuscator;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StorePanel : MonoBehaviour
{
    public static StorePanel instance;
    public GameObject container;



    public IAP_ProductUI Store_Ticket_UI;
    public IAP_ProductUI Multiplayer_Ticket_UI;
    public IAP_ProductUI Remove_Ads_UI;

    public Sprite[] disableTexture;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SyncUnSyncedPurchases()
    {
        buttonClickedIdx = -1;
        Invoke("forInvokeSync", 4f);
    }

    private void forInvokeSync()
    {
        /*if (IAPHandler.instance.HasUnSyncedPurchases() && CONTROLLER.IsUserLoggedIn())
        {
            LoadingScreen.instance.Show("Syncing Unsynced Purchases...");
            IAPHandler.instance.SyncUnSyncedReceipts(() =>
            {
                LoadingScreen.instance.Hide();
                AdIntegrate.instance.hasPendingPurchase = false;
            });
        }*/
    }
    private void SyncOldPurchases(System.Action<int> OnCompletion=null)
    {
        /*if (IAPHandler.instance.HasUnSyncedPurchases() && CONTROLLER.IsUserLoggedIn())
        {
            LoadingScreen.instance.Show("Syncing Unsynced Purchases...");
            IAPHandler.instance.SyncUnSyncedReceipts(() =>
            {
                LoadingScreen.instance.Hide();
                AdIntegrate.instance.hasPendingPurchase = false;

                if (buttonClickedIdx != -1 && OnCompletion != null)
                {
                    OnCompletion?.Invoke(buttonClickedIdx);
                }
            });
        }*/
    }

    public void Show()
    {
        OpenStore();
    }
    public void Hide()
    {
        container.SetActive(false);
    }

    public int buttonClickedIdx;
    public void OnPurchaseButClicked(int idx)
    {
        AudioPlayer.instance.PlayButtonSnd();
        if (!AdIntegrate.instance.checkTheInternet())
        {
        }
        else
        {
            buttonClickedIdx=idx;

            SKU temp;
            if (idx == 0)
                temp = SKU.blitz100tickets;
            else
                temp = SKU.blitzadsfree;
           
            /*if (IAPHandler.instance.HasUnSyncedPurchases())
            {
                SyncOldPurchases(OnPurchaseButClicked);
            }*/
           /* else if (IAPHandler.instance.HasdeferredProductsListContains(temp))
            {
                Popup.instance.Show(heading: "PURCHASE IN PROCESS", message: "Your recent purchase is still in process. Please try signing out after a while. You can still safely exit the app though.", yesString: "OK", size: 1);
            }*/
            /*else
            {
                if (idx == 0)
                    IAPHandler.PurchaseProduct(SKU.blitz100tickets, OnPaymentCompletion);
                else if (idx == 1)
                    IAPHandler.PurchaseProduct(SKU.blitzadsfree, OnPaymentCompletion);
            }*/
        }
    }

    private void OnPaymentCompletion(bool isSuccessfull, SKU sku, byte statusCode)
    {
        if (isSuccessfull)
        {

        }
        else
        {
            if (statusCode == 0)
            {
            }
            else if (statusCode == 1)
            {
                Popup.instance.showGenericPopup("OOPS!!!", "Something went wrong. \nPlease try again later.");
            }
            else if (statusCode == 2)
            {
                //OpenPurchasePlatinumPanel();
            }
            else if (statusCode == 3 || statusCode == 15)
            {
                //Should not show anything as it is already Handled
            }
        }
    }

    public void restore_ios()
    {

    }
    public void OpenStore(int screenIndex = -1)
    {
        if (AdIntegrate.instance.checkTheInternet())
        {
            LoadingScreen.instance.Show("Loading Store...");
        }
        else
        {
        }
    }

    public void PopulateUI()
    {
        DisplayValuesInProducts();
        container.SetActive(true);
        CONTROLLER.CurrentPage = "storepage";
        LoadingScreen.instance.Hide();
        CancelInvoke("ForWorstScneario");
        GameModeSelector._instance.ShowLandingPage(false);
    }

    public void DisplayValuesInProducts()
    {
#if UNITY_EDITOR
        Store_Ticket_UI.offerHolder.SetActive(false);
        Multiplayer_Ticket_UI.offerHolder.SetActive(false);
        Remove_Ads_UI.offerHolder.SetActive(false);
#endif

        //Tickets
        //Store_Ticket_UI.title.text = Multiplayer_Ticket_UI.title.text = IAPHandler.instance.TICKET.title;
        //Store_Ticket_UI.price.text = Multiplayer_Ticket_UI.price.text = IAPHandler.instance.TICKET.priceValue;

        // if (IAPHandler.instance.TICKET.HasOffer())
        // {
        //     Store_Ticket_UI.offerHolder.SetActive(true);
        //     Multiplayer_Ticket_UI.offerHolder.SetActive(true);
        //
        //     //Store_Ticket_UI.offerText.text=Multiplayer_Ticket_UI.offerText.text= IAPHandler.instance.TICKET.GetFormattedOffer( );
        // }
        // else
        // {
        //     Store_Ticket_UI.offerHolder.SetActive(false);
        //     Multiplayer_Ticket_UI.offerHolder.SetActive(false);
        // }
        //
        // // Remove Ads
        // if (CONTROLLER.isAdRemoved)
        // {
        //     Remove_Ads_UI.BuyButtonTexture.sprite = disableTexture[1];
        //     Remove_Ads_UI.BuyButton.enabled = false;
        //     Remove_Ads_UI.price.text = "Purchased";
        //     Remove_Ads_UI.offerHolder.SetActive(false);
        // }
        // else
        // {
        //     Remove_Ads_UI.BuyButtonTexture.sprite = disableTexture[0];
        //     Remove_Ads_UI.BuyButton.enabled = true;
        //     Remove_Ads_UI.price.text = IAPHandler.instance.REMOVE_ADs.priceValue;
        //     if (IAPHandler.instance.REMOVE_ADs.HasOffer())
        //     {
        //         Remove_Ads_UI.offerHolder.SetActive(true);
        //         Remove_Ads_UI.offerText.text = IAPHandler.instance.REMOVE_ADs.GetFormattedOffer();
        //     }
        //     else
        //     {
        //         Remove_Ads_UI.offerHolder.SetActive(false);
        //     }
        //
        // }
    }
    private void ForWorstScneario()
    {
        LoadingScreen.instance.Hide();
        Popup.instance.Show(heading: "Google Play Store not responding!", message: "We are unable to receive any info from Google Play Store.\n Try to open Google Play Store in your phone before accessing the store.",yesString:"OK", size: 1);
        //Popup.instance.showGenericPopup("Google Play Store not responding!", "We are unable to receive any info from Google Play Store.\n Try to open Google Play Store in your phone before accessing the store.");
        //CricMinisWebRequest.instance.ShowSomethingWentWrong();
    }


    public static bool IsAllPacksInitialised()
    {
        return false;
        //return IAPHandler.instance.IsInitialized;
    }
    private byte validationCount;
    private System.Action<bool> OnInitialized;
    public static void InitiliazeIAPProducts(System.Action<bool> OnCompleted)
    {
        instance.OnInitialized = OnCompleted;
        instance.validationCount = 1;
        // if (!IAPHandler.instance.IsInitialized)
        // {
        //     if (IAPHandler.instance.InitializingInProgress)
        //     {
        //         IAPHandler.instance.OnInitialize += instance.OnForceIAPInitialize;
        //     }
        //     else
        //     {
        //         IAPHandler.instance.InitiazeIAP(instance.OnForceIAPInitialize);
        //     }
        // }
        // else
        // {
        //     instance.OnForceIAPInitialize(true);
        // }
    }
    private void OnForceIAPInitialize(bool success)
    {
        if (success)
        {
            validationCount--;
            if (validationCount == 0)
            {
                OnInitialized?.Invoke(true);
            }
        }
        else
        {
            OnInitialized?.Invoke(false);
        }
    }
    public event VoidDelgate OnPurchase;

    public static void TriggerOnPurchase()
    {
        instance.OnPurchase?.Invoke();
    }
    public void SavePurchaseData(bool restore = false)
    {
        if (!restore)
        {
            AdIntegrate.instance.writeInToFile(JsonConvert.SerializeObject(this, Formatting.Indented), "Store", "purchaseData.dat");
        }
        else
        {
            AdIntegrate.instance.writeInToFile(JsonConvert.SerializeObject(this, Formatting.Indented), "Store", "restoreData.dat");
        }
    }
}

