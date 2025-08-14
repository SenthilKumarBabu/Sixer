using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class WebRequestManager : MonoBehaviour
{
    private AuthWr _authWr;
    private SessionWr _sessionWr;
    private WalletWr _walletWr;

    private void Awake()
    {
        _authWr = new AuthWr();
        _sessionWr = new SessionWr();
        _walletWr = new WalletWr();
    }

    async void Start()
    {     
        _sessionWr.SessionSimpleYear(new SessionInputData()
        {
            clientId = "unity_client321",
            clientVersion = "1.0.0",
            deviceInfo =  new DeviceInfoData()
            {
                platform = "Unity",
                version = "2022.3.0f1",
                deviceId = "unity_device_321"
            }
        });
        await UniTask.Delay(TimeSpan.FromSeconds(5));
        _authWr.AuthLogin("newplayer","SecurePass123");
        await UniTask.Delay(TimeSpan.FromSeconds(5));
        //_sessionWr.SessionChallenge();
        //await UniTask.Delay(TimeSpan.FromSeconds(5));
        _walletWr.WalletBalance();
        //SessionExchange();
        /*GetUserProfile();
        await UniTask.Delay(TimeSpan.FromSeconds(5));
        Logout();*/
    }
}