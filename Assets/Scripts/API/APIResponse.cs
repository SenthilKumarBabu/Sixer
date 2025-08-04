using UnityEngine;

[System.Serializable]
public class APIResponse<T>
{
    public bool success;
    public string message;
    public T data;

    public APIResponse(bool responseStatus, string responseMessage, T responseData)
    {
        success = responseStatus;
        message = responseMessage;
        data = responseData;
    }
}

[System.Serializable]
public class LoginData
{
    public User user;
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
