using UnityEngine;

[System.Serializable]
public class RootData
{
    public bool encrypted;
    public string sessionId;
    public EncryptedData data;
    public long timestamp;
}

[System.Serializable]
public class EncryptedData
{
    public string iv;
    public string encrypted;
    public string tag;
}

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