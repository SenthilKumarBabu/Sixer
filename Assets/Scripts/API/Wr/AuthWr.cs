using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class AuthWr
{
    public async Task<LoginData> AuthLogin(string playerName, string pass)
    {
        var payload = new
        {
            identifier = playerName,
            password = pass
        };

        string loginJson = JsonConvert.SerializeObject(payload);

        string responseJson = await WebRequestHelper.PostAsync(Apis.Auth.Login, loginJson);

        if (!string.IsNullOrEmpty(responseJson))
        {
            var response = JsonConvert.DeserializeObject<APIResponse<LoginData>>(responseJson);
            if (response!.success)
            {
                WebRequestHelper.LoggedInUser = response.data;
                return response.data;
            }
        }

        return default;
    }
    
    public async Task<RegisterData> AuthRegister(string email, string playerName, string pass)
    {
        var payload = new
        {
            email = email,
            username = playerName,
            password = pass
        };

        string registerJson = JsonConvert.SerializeObject(payload);

        string responseJson = await WebRequestHelper.PostAsync(Apis.Auth.Register, registerJson);

        if (!string.IsNullOrEmpty(responseJson))
        {
            var response = JsonConvert.DeserializeObject<APIResponse<RegisterData>>(responseJson);
            if (response!.success)
            {
                return response.data;
            }
        }

        return default;
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
                WebRequestHelper.LoggedInUser.user = response.data;
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
            refreshToken = WebRequestHelper.LoggedInUser.tokens.refreshToken,
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
                WebRequestHelper.LoggedInUser.tokens = new Tokens();
            }
            else
            {
                Debug.LogWarning("Logout Failed: " + response.message);
            }
        }
    }
   
   /*
    RootData encResponse = JsonConvert.DeserializeObject<RootData>(responseJson);
            var response = AESEncryptionHelper.DecryptData<APIResponse<LoginData>>(encResponse!.data,  WebRequestHelper.SessionData.sessionKey);
            if (response!.success)
            {
                Debug.Log("Login Success. Access Token: " + response.data.tokens.accessToken);
                WebRequestHelper.LoggedInUser = response.data;
            }
            else
            {
                Debug.LogWarning("Login Failed: " + response.message);
            }
            */
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
public class RegisterData
{
    public string id;
    public string email;
    public string username;
    public string firstName;
    public string lastName;
    public string message;
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

