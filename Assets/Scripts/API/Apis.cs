using UnityEngine;

public static class Apis
{
    public const string BaseUrl = "http://34.121.250.197/api/v1/";
    
    public static class Auth
    {
        public const string Login = BaseUrl + "auth/login";
        public const string GetUserProfile = BaseUrl + "auth/me";
        public const string Logout = BaseUrl + "auth/logout";
    }
}