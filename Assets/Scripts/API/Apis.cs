using UnityEngine;

public static class Apis
{
    private const string BaseUrl = "http://34.121.250.197/api/v1/";
    
    public static class Auth
    {
        public const string Login = BaseUrl + "auth/login";
        public const string GetUserProfile = BaseUrl + "auth/me";
        public const string Logout = BaseUrl + "auth/logout";
    }

    public static class Session
    {
        public const string Challenge = BaseUrl + "session/challenge?clientPublicKey=-----BEGIN PUBLIC KEY-----...";
        public const string Exchange = BaseUrl + "session/exchange";
        public const string Simple = BaseUrl + "session/simple";
    }
    
    public static class Wallet
    {
        public const string Balance = BaseUrl + "wallet/balance";
    }
}