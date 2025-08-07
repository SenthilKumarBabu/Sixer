using Newtonsoft.Json;
using UnityEngine;

public class SessionWr
{
    public async void SessionChallenge()
    {
        string responseJson = await WebRequestHelper.GetAsync(Apis.Session.Challenge);

        if (!string.IsNullOrEmpty(responseJson))
        {
            APIResponse<SessionChallengeData> response =
                JsonConvert.DeserializeObject<APIResponse<SessionChallengeData>>(responseJson);
            if (response!.success)
            {
                Debug.Log($"{response.message} {response.data}");
                WebRequestHelper.SessionChallenge = response.data;
            }
            else
            {
                Debug.LogWarning(response.message);
            }
        }
    }
}

[System.Serializable]
public class SessionChallengeData
{
    public string challengeId;
    public long timestamp;
    public string nonce;
    public string serverPublicKey;
    public string serverECPublicKey;
    public long sessionTimeout;
    public string signature;
    public string sessionId;
}