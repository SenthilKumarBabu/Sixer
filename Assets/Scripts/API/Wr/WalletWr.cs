using Newtonsoft.Json;
using UnityEngine;

public class WalletWr
{
    public async void WalletBalance()
    {
        string responseJson = await WebRequestHelper.GetAsync(Apis.Wallet.Balance);

        if (!string.IsNullOrEmpty(responseJson))
        {
            RootData response = JsonConvert.DeserializeObject<RootData>(responseJson);
            if (response != null)
            {
                var data = AESEncryptionHelper.DecryptData<APIResponse<WalletData>>(response.data,  WebRequestHelper.SessionData.sessionKey);
            }
        }
    }
}

[System.Serializable]
public class WalletData
{
    public string balance;
    public string currency;
    public bool isActive;
    public bool isFrozen;
    public string lastUpdated;
}