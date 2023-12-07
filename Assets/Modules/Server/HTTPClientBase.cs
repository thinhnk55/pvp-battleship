using Framework;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HTTPClientBase
{

    static public IEnumerator Get(string url, Callback<string> callback, List<KeyValuePair<string, string>> headers = null)
    {
        using UnityWebRequest webRequest = UnityWebRequest.Get(url);

        if (headers != null)
        {
            foreach (var kvp in headers)
            {
                webRequest.SetRequestHeader(kvp.Key, kvp.Value);
            }
        }

        PDebug.Log(url);

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            string response = webRequest.downloadHandler.text;
            callback?.Invoke(response);
            PDebug.Log("Response: {0}", response);
        }
        else
        {
            PDebug.LogError("Error: {0}", webRequest.error);
        }
    }

    static public IEnumerator Post(string url, string data, Callback<string> callback, List<KeyValuePair<string, string>> headers = null)
    {
        /**/
        byte[] bodyRaw = UTF8Encoding.UTF8.GetBytes(data);
        using UnityWebRequest webRequest = new(url, "POST");

        if (headers != null)
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
