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
                    PDataAuth.AuthData.userId = UnityEngine.Random.Range(0,100);// int.Parse(jsonRes["data"]["userid"]);
                    PDataAuth.AuthData.username = jsonRes["data"]["username"];
                    PDataAuth.AuthData.refresh_token = jsonRes["data"]["refresh_token"];
                    PDataAuth.AuthData.token = jsonRes["data"]["token"];
                    if (WSClient.Instance == null)
                    {
                        Instantiate(websocket, transform.parent);
                    }
                    else
                    {
                        WSClient.Instance.ws.Connect();
                    }
                }
                else
                {
                    PDataAuth.AuthData = new AuthData();
                    PDataAuth.AuthData.userId = UnityEngine.Random.Range(0, 100);
                    PDataAuth.AuthData.username = "";
                    PDataAuth.AuthData.refresh_token = "";
                    PDataAuth.AuthData.token = "";
                    Debug.Log(res);
                }
                Instantiate(websocket, transform.parent);
            }
         ));
    }
}
