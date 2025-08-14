using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Sixer.UI
{
    public class SplashScreenPage : UIPage
    {
        [SerializeField] private Image loadingBar;
        private float loadingDuration = 1f;

        public override void OnShow(object data = null)
        {
            loadingBar.DOFillAmount(1, loadingDuration).OnComplete(() =>
            {
                UIManager.Instance.OpenPage<LoginPage>(new LoginPageData(status: LoginPage.LoginPageStatus.SignInPage));
            });
        }
    }
}

