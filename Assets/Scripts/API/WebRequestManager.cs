using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class WebRequestManager : MonoBehaviour
{

    async void Start()
    {
        AuthLogin("newplayer","SecurePass123");
        await UniTask.Delay(TimeSpan.FromSeconds(5));
        GetUserProfile();
        await UniTask.Delay(TimeSpan.FromSeconds(5));
        Logout();
    }

    private async void AuthLogin(string playerName, string pass)
    {
        var payload = new
        {
            identifier = playerName,
            password = pass
        };

        string loginJson = JsonConvert.SerializeObject(payload);
        Debug.Log(loginJson);
        
        string responseJson = await WebRequestHelper.PostAsync(Apis.Auth.Login, loginJson);

        if (!string.IsNullOrEmpty(responseJson))
        {
            APIResponse<LoginData> response = JsonConvert.DeserializeObject<APIResponse<LoginData>>(responseJson);
            if (response!.success)
            {
                Debug.Log("Login Success. Access Token: " + response.data.accessToken);
                WebRequestHelper.AuthToken = response.data.accessToken;
                WebRequestHelper.RefreshToken = response.data.refreshToken;
                WebRequestHelper.User = response.data.user;
            }
            else
            {
                Debug.LogWarning("Login Failed: " + response.message);
            }
        }
    }
    
    private async void GetUserProfile()
    {
        string responseJson = await WebRequestHelper.GetAsync(Apis.Auth.GetUserProfile);

        if (!string.IsNullOrEmpty(responseJson))
        {
            APIResponse<User> response = JsonConvert.DeserializeObject<APIResponse<User>>(responseJson);
            if (response!.success)
            {
                Debug.Log($"{response.message} {response.data.DebugInfo()}");
                WebRequestHelper.User = response.data;
            }
            else
            {
                Debug.LogWarning("Logout Failed: " + response.message);
            }
        }
    }

    private async void Logout()
    {
        var payload = new
        {
            refreshToken = WebRequestHelper.RefreshToken,
        };

        string logoutJson = JsonConvert.SerializeObject(payload);
        Debug.Log(logoutJson);
        
        string responseJson = await WebRequestHelper.PostAsync(Apis.Auth.Logout, logoutJson);

        if (!string.IsNullOrEmpty(responseJson))
        {
            APIResponse<object> response = JsonConvert.DeserializeObject<APIResponse<object>>(responseJson);
            if (response!.success)
            {
                Debug.Log($"{response.message}");
                WebRequestHelper.AuthToken = "";
                WebRequestHelper.RefreshToken = "";
            }
            else
            {
                Debug.LogWarning("Logout Failed: " + response.message);
            }
        }
    }
}