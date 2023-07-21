using Framework;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Windows;

public class HTTPClient : SingletonMono<HTTPClient>    
{
    [SerializeField] GameObject websocket;
    public void LoginDeviceId()
    {
        string deviceId = SystemInfo.deviceUniqueIdentifier;
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
            {"deviceId", deviceId},
            {"sessionInfo", new JSONClass()}
        };
        StartCoroutine(HTTPClientBase.Post(ServerConfig.HttpURL +"/logindevice", json.ToString()
            , (res) => {
                JSONNode jsonRes = JSONNode.Parse(res);
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

    public void LoginGoogle(string idToken)
    {
        string deviceId = SystemInfo.deviceUniqueIdentifier;

        JSONNode json = new JSONClass()
        {
            {"id_token",  idToken},
            {"device_id", deviceId },
            {"session_info", new JSONClass()}
        };

        StartCoroutine(HTTPClientBase.Post(ServerConfig.HttpURL + "/gg-login", json.ToString()
            , (res) =>
            {
                JSONNode jsonRes = JSONNode.Parse(res);
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
}
