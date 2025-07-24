using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class JWTHandler
{
    public static bool isTimerFetched = false;
    private static ObscuredString secretKey = "sajfweoijrjaoclajiorjawlckneawieawnllsajfoei";

    public static string GetJwtToken(Dictionary<string, object> payload)
    {
        return JWT.JsonWebToken.Encode(payload, secretKey, JWT.JwtHashAlgorithm.HS256);
    }

    public static void SetTimeOffset(int timeDiff)
    {
        DateTime serverTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timeDiff);
        //int serverTimeOffset = (int)(GlobalTimer.instance.UtcNow() - serverTime).TotalSeconds;
        int serverTimeOffset = (int)(DateTime.Now - serverTime).TotalSeconds;
        PlayerPrefs.SetInt("serverTimeOffset", serverTimeOffset);
        isTimerFetched = true;
    }

    public static int GetStartTime()
    {
        int serverTimeOffset = 0;
        if (PlayerPrefs.HasKey("serverTimeOffset"))
        {
            serverTimeOffset = PlayerPrefs.GetInt("serverTimeOffset");
        }
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //int startTime = (int)(GlobalTimer.instance.UtcNow() - epochStart).TotalSeconds - serverTimeOffset;
        int startTime = (int)(DateTime.Now - epochStart).TotalSeconds - serverTimeOffset;
        return startTime;
    }

    public static string MD5Hash(string input)
    {
        System.Text.StringBuilder hash = new System.Text.StringBuilder();
        System.Security.Cryptography.MD5CryptoServiceProvider md5provider = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] bytes = md5provider.ComputeHash(new System.Text.UTF8Encoding().GetBytes(input));

        for (int i = 0; i < bytes.Length; i++)
        {
            hash.Append(bytes[i].ToString("x2"));
        }
        return hash.ToString();
    }
}
