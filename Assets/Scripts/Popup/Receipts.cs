using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

    /// <summary>
    /// Holds data to proccess unsynced receipt
    /// </summary>
    public class Receipts
    {
        public List<Receipt> receipts;

        public Receipts()
        {
            receipts = new List<Receipt>();
        }
        
        /// <summary>
        /// Save Receipts to hidden path
        /// </summary>
        public void SaveReceipts()
        {
           AdIntegrate.instance.writeInToFile(JsonConvert.SerializeObject(this, Formatting.Indented), "Receipts", "Receipts.dat");
        }

    /// <summary>
    /// Clear Receipts from the path
    /// </summary>
    public static void ClearReceipts()
    {
        if (AdIntegrate.instance.IsFileExits("Receipts", "Receipts.dat"))
        {
            AdIntegrate.instance.deleteFile("Receipts", "Receipts.dat");
        }
    }

        /// <summary>
        /// Add new receipt to existing unsynced receipts and saving to the path
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="receipt"></param>
        /// <param name="args"></param>
        public static void AddReceipt(string productID, JObject receipt, object[] args)
        {
            Receipts receipts = Load();
            if (receipts == null)
            {
                receipts = new Receipts();
            }
            receipts.receipts.Add(new Receipt(productID, receipt, args));
           AdIntegrate.instance.writeInToFile(JsonConvert.SerializeObject(receipts, Formatting.Indented), "Receipts", "Receipts.dat");
        }

        /// <summary>
        /// Add temporary receipt while purchase sync is in progress
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="receipt"></param>
        /// <param name="args"></param>
        public static void AddTempReceipt(string productID, JObject receipt, object[] args)
        {
            Receipts receipts = new Receipts();
            receipts.receipts.Add(new Receipt(productID, receipt, args));
            PlayerPrefs.SetString("TempReceipt", JsonConvert.SerializeObject(receipts));
        }

        /// <summary>
        /// To clear temporary receipt on successful sync or on adding the receipt to permanent unsynced receipts
        /// </summary>
        public static void ClearTempReceipts()
        {
            if (PlayerPrefs.HasKey("TempReceipt"))
            {
                PlayerPrefs.DeleteKey("TempReceipt");
            }
        }

        
        public static Receipt CheckForProductInReceipts(SKU skuToBeChecked, params object[] args)
        {
                //UsageType.Consumable
            return null;
        }
        /// <summary>
        /// Load the receipts
        /// </summary>
        /// <returns></returns>
        public static Receipts Load()
        {
            if (AdIntegrate.instance.IsFileExits("Receipts", "Receipts.dat"))
            {
                return JsonConvert.DeserializeObject<Receipts>(AdIntegrate.instance.readFromFile("Receipts", "Receipts.dat"), AdIntegrate.jsonSerializerSettings);
            }
            else
            {
                return null;
            }
        }

        public class Receipt
        {
            public string productID;
            public JObject receipt;
            public object[] args;

            public Receipt(string productID, JObject receipt, object[] args)
            {
                this.productID = productID;
                this.receipt = receipt;
                this.args = args;
            }
        }
    }
