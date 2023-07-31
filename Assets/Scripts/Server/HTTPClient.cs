using Framework;
using SimpleJSON;
using System;
using UnityEngine;


public class HTTPClient : SingletonMono<HTTPClient>    
{
    [SerializeField] GameObject websocket;

    private void HTTPPost(JSONNode json, String typeLogin)
    {
        StartCoroutine(HTTPClientBase.Post(ServerConfig.HttpURL + "/" +typeLogin, json.ToString()
            , (res) => {
                JSONNode jsonRes = JSONNode.Parse(res);
                Debug.Log("SendDone");
                if (int.Parse(jsonRes["error"]) == 0)
                {
                    PDataAuth.AuthData = new AuthData();
                    PDataAuth.AuthData.userId = int.Parse(jsonRes["data"]["userid"]);
                    PDataAuth.AuthData.username = jsonRes["data"]["username"];
                    PDataAuth.AuthData.refresh_token = jsonRes["data"]["refresh_token"];
                    PDataAuth.AuthData.token = jsonRes["data"]["token"];
                    if (WSClient.Instance == null)
                    {
                        Instantiate(websocket, transform.parent);
                    }
                }
                else
                {
                    Debug.Log(res);
                }
            }
        ));
    }

    public void LoginDeviceId()
    {

        string deviceId = SystemInfo.deviceUniqueIdentifier;
        char salt = (char)(deviceId[deviceId.Length / 2] + 15);
        string username = deviceId;
        string password = SHA256Hash.GetSHA256Hash(deviceId+salt);
        password = password.Substring(15, 12); //arg1: startIndex; arg2: passwordLength;
/*        SHA256 sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(deviceId));
        StringBuilder builder = new StringBuilder();
        foreach (byte b in hashBytes)
        {
            builder.Append(b.ToString("x2")); // Convert each byte to a hexadecimal string
        }
        string hash = builder.ToString();*/
        JSONNode json = new JSONClass()
        {
            {"password", password },
            {"username", username },
            {"device_id", deviceId},
            {"session_info", new JSONClass()}
        };

        HTTPPost(json, "login");
    }

    public void LoginGoogle(string idToken)
    {
        string deviceId = SystemInfo.deviceUniqueIdentifier;

        JSONNode json = new JSONClass()
        {
            {"id_token",  idToken},
            {"device_id", deviceId },
            {"session_info", new JSONClass()}
        };
        Debug.Log("SendJson");
        HTTPPost(json, "gg-login");
    }

    public void LoginApple(string authentication)
    {
        string deviceId = SystemInfo.deviceUniqueIdentifier;

        JSONNode json = new JSONClass()
        {
            {"appleCode",  authentication},
            {"device_id", deviceId },
            {"session_info", new JSONClass() },
            {"type", "ios" }
        };

        HTTPPost(json, "ap-register");
    }
}
