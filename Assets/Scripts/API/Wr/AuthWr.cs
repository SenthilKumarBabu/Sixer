using Newtonsoft.Json;
using UnityEngine;

public class AuthWr
{
   public async void AuthLogin(string playerName, string pass)
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
            Debug.Log(responseJson);
            APIResponse<LoginData> response = JsonConvert.DeserializeObject<APIResponse<LoginData>>(responseJson);
            if (response!.success)
            {
                Debug.Log("Login Success. Access Token: " + response.data.tokens.accessToken);
                WebRequestHelper.AuthToken = response.data.tokens.accessToken;
                WebRequestHelper.RefreshToken = response.data.tokens.refreshToken;
                WebRequestHelper.User = response.data.user;
            }
            else
            {
                Debug.LogWarning("Login Failed: " + response.message);
            }
        }
    }
    
   public async void GetUserProfile()
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

   public async void Logout()
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

[System.Serializable]
public class LoginData
{
    public User user;
    public Tokens tokens;
}

[System.Serializable]
public class Tokens
{
    public string accessToken;
    public string refreshToken;
}

[System.Serializable]
public class User
{
    public string id;
    public string email;
    public string username;
    public string firstName;
    public string lastName;
    public string[] roles;
    public string[] permissions;

    public string DebugInfo()
    {
        return ($"User => ID: {id}, Email: {email}, Username: {username}, First: {firstName}, Last: {lastName}, Roles: [{string.Join(", ", roles)}], Permissions: [{string.Join(", ", permissions)}]");
    }
}

