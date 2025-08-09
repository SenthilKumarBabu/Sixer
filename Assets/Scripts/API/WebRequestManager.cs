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
        _sessionWr.SessionSimple("-chris");
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