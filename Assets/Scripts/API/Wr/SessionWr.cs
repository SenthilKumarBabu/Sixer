using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class SessionWr
{
    public async Task<SessionData> SessionSimpleYear(SessionInputData sessionInputData)
    {
        string json = JsonConvert.SerializeObject(sessionInputData);
        
        string responseJson = await WebRequestHelper.PostAsync(Apis.Session.SimpleYear, json);

        if (!string.IsNullOrEmpty(responseJson))
        {
            APIResponse<SessionData> response = JsonConvert.DeserializeObject<APIResponse<SessionData>>(responseJson);
            if (response!.success)
            {
                WebRequestHelper.SessionData = response.data;
                return response.data;
            }
        }

        return default;
    }
}

[System.Serializable]
public class SessionInputData
{
    public string clientId;
    public string clientVersion;
    public DeviceInfoData deviceInfo;
}

[Serializable]
public class DeviceInfoData
{
    public string platform;
    public string version;
    public string deviceId;
}

[System.Serializable]
public class SessionData
{
    public bool success;
    public string sessionId;
    public string sessionKey;
    public string message;
}

