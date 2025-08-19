using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Sixer.UI
{
    public class SplashScreenPage : UIPage
    {
        [SerializeField] private Image loadingBar;
        private readonly float _loadingDuration = 1f;
        
        private SessionWr _sessionWr;

        private void Awake()
        {
            _sessionWr = new SessionWr();
        }

        public override async void OnShow(object data = null)
        {
            loadingBar.DOFillAmount(1, _loadingDuration);

            var sessionData = await _sessionWr.SessionSimpleYear(new SessionInputData()
            {
                clientId = "unity_client321",
                clientVersion = "1.0.0",
                deviceInfo = new DeviceInfoData()
                {
                    platform = "Unity",
                    version = "2022.3.0f1",
                    deviceId = "unity_device_321"
                }
            });

            SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}

