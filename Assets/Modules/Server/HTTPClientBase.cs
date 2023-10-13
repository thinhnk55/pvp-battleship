using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Framework;
using Server;

public class HTTPClientBase 
{

    static public IEnumerator Get(string url, Callback<string> callback, List<KeyValuePair<string, string>> headers = null)
    {
        using UnityWebRequest webRequest = UnityWebRequest.Get(url);
        
        if(headers != null)
        {
            foreach (var kvp in headers)
            {
                webRequest.SetRequestHeader(kvp.Key, kvp.Value);
            }
        }

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            string response = webRequest.downloadHandler.text;
            callback?.Invoke(response);
            Debug.Log("Response: " + response);
        }
        else
        {
            Debug.LogError("Error: " + webRequest.error);
        }
    }

    static public IEnumerator Post(string url, string data, Callback<string> callback, List<KeyValuePair<string, string>> headers = null)
    {
        /**/
        byte[] bodyRaw = UTF8Encoding.UTF8.GetBytes(data);
        using UnityWebRequest webRequest = new(url, "POST");

        if(headers != null)
        {
            foreach (var kvp in headers)
            {
                webRequest.SetRequestHeader(kvp.Key, kvp.Value);
            }
        }

        Debug.Log(url + data);
        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            string response = webRequest.downloadHandler.text;
            Debug.Log("Response: " + response);
            callback?.Invoke(response);
        }
        else
        {
            Debug.LogError("Error: " + webRequest.error);
        }
        
    }

}
