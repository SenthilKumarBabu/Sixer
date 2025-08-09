using Newtonsoft.Json;
using UnityEngine;

public class SessionWr
{
    
    public async void SessionSimple(string clientId)
    {
        var payload = new
        {
            clientId = clientId,
        };

        string json = JsonConvert.SerializeObject(payload);
        Debug.Log(json);
        
        string responseJson = await WebRequestHelper.PostAsync(Apis.Session.Simple, json);

        if (!string.IsNullOrEmpty(responseJson))
        {
            Debug.Log(responseJson);
            APIResponse<SessionData> response = JsonConvert.DeserializeObject<APIResponse<SessionData>>(responseJson);
            if (response!.success)
            {
                Debug.Log("Success");
                WebRequestHelper.SessionData = response.data;
            }
            else
            {
                Debug.LogWarning("Login Failed: " + response.message);
            }
        }
    }
}

[System.Serializable]
public class SessionData
{
    public bool success;
    public string sessionId;
    public string sessionKey;
    public string message;
}