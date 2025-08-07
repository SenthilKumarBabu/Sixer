using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
/// <summary>
/// To Create Session Key
/// </summary>
public static class SHAEncryptionHelper
{
    public static string SessionKey = "";

    public static string Password = "SIXER_SESSION_KEY_SALT";

    public static byte[] GenerateSessionKey(string sessionId)
    {
        string dataToHash = sessionId + Password;
    
        byte[] data = Encoding.UTF8.GetBytes(dataToHash);
    
        using (var sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(data);
        
            byte[] sessionKey = new byte[32];
            Array.Copy(hashBytes, 0, sessionKey, 0, 32);

            SessionKey = sessionKey.ToString();
            
            return sessionKey;
        }
    }
    
    public static byte[] GenerateSessionKey1(string sessionId)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(sessionId + "SIXER_SESSION_KEY_SALT"));
        
        byte[] result = new byte[32];
        Array.Copy(hash, 0, result, 0, 32);
        return result;
    }

// Alternative more concise version:
    public static byte[] GenerateSessionKey2(string sessionId)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(sessionId + "SIXER_SESSION_KEY_SALT"));
            return hash[0..32]; // C# 8.0+ range syntax (equivalent to slice(0, 32))
        }
    }
}
