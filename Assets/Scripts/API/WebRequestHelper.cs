using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BestHTTP;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public static class WebRequestHelper
{
    public static LoginData LoggedInUser;
    public static SessionData SessionData;
    
    public static async Task<string> GetAsync(string url)
    {
        var tcs = new TaskCompletionSource<string>();

        var request = new HTTPRequest(new System.Uri(url), HTTPMethods.Get, (req, res) =>
        {
            if (res.IsSuccess)
                tcs.SetResult(res.DataAsText);
            else
                tcs.SetException(new System.Exception($"GET Error: {res.StatusCode} - {res.Message}"));
        });

        request.AddHeader("Content-Type", "application/json");
        if (!string.IsNullOrEmpty(LoggedInUser.tokens.accessToken))
        {
            request.AddHeader("Authorization", $"Bearer {LoggedInUser.tokens.accessToken}");
        }
        if (SessionData != null && !string.IsNullOrEmpty(SessionData.sessionId))
        {
            request.AddHeader("X-Session-Id", SessionData.sessionId);
        }
        
        request.Send();

        return await tcs.Task;
    }

    public static async Task<string> PostAsync(string url, string jsonBody)
    {
        var tcs = new TaskCompletionSource<string>();

        var request = new HTTPRequest(new System.Uri(url), HTTPMethods.Post, (req, res) =>
        {
            if (res.IsSuccess)
                tcs.SetResult(res.DataAsText);
            else
                tcs.SetException(new System.Exception($"POST Error: {res.StatusCode} - {res.Message}"));
        });

        request.AddHeader("Content-Type", "application/json");
        if (!string.IsNullOrEmpty(LoggedInUser.tokens.accessToken))
        {
            request.AddHeader("Authorization", $"Bearer {LoggedInUser.tokens.accessToken}");
        }
        if (SessionData != null && !string.IsNullOrEmpty(SessionData.sessionId))
        {
            request.AddHeader("X-Session-Id", SessionData.sessionId);
        }
        request.RawData = System.Text.Encoding.UTF8.GetBytes(jsonBody);

        request.Send();

        return await tcs.Task;
    }
}