using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public static class AESEncryptionHelper
{
    public static T DecryptData<T>(EncryptedData encryptedData, string sessionKey)
    {
        byte[] keyBuffer = Convert.FromBase64String(sessionKey);
        byte[] iv = Convert.FromBase64String(encryptedData.iv);
        string encrypted = encryptedData.encrypted;

        using (Aes aes = Aes.Create())
        {
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = keyBuffer;
            aes.IV = iv;

            using (ICryptoTransform decrypt = aes.CreateDecryptor())
            {
                    
                byte[] encryptedBytes = Convert.FromBase64String(encrypted);
                byte[] decryptedBytes = decrypt.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                string decrypted = Encoding.UTF8.GetString(decryptedBytes);

                return JsonUtility.FromJson<T>(decrypted);
            }
        }
        try
        {
           
        }
        catch (Exception error)
        {
            Debug.LogError($"Decryption failed: {error.Message}");
            throw new Exception($"Decryption failed: {error.Message}");
        }
    }
}
