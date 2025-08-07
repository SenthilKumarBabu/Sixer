using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public static class AESEncryptionHelper
{
    public static T DecryptData<T>(EncryptedData encryptedData, byte[] sessionKey)
    {
        byte[] iv = HexStringToBytes(encryptedData.iv);
        byte[] encrypted = HexStringToBytes(encryptedData.encrypted);
        byte[] tag = HexStringToBytes(encryptedData.tag);
        byte[] aad = Encoding.UTF8.GetBytes("api-gateway");

        Debug.Log(Encoding.UTF8.GetString(sessionKey));
        using (AesGcm aesGcm = new AesGcm(sessionKey))
        {
            byte[] decrypted = new byte[encrypted.Length];
            aesGcm.Decrypt(iv, encrypted, tag, decrypted, aad);
                
            string decryptedJson = Encoding.UTF8.GetString(decrypted);
            return JsonConvert.DeserializeObject<T>(decryptedJson);
        }
        try
        {
           
        }
        catch (Exception error)
        {
            UnityEngine.Debug.LogError($"Data decryption failed: {error.Message}");
            throw new Exception($"Data decryption failed: {error.Message}");
        }
    }

    private static byte[] HexStringToBytes(string hex)
    {
        if (string.IsNullOrEmpty(hex))
            return Array.Empty<byte>();

        int length = hex.Length;
        byte[] bytes = new byte[length / 2];

        for (int i = 0; i < length; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }

        return bytes;
    }

}
