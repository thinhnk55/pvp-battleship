using Authentication;
using Framework;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;

namespace Authentication
{
    public class HTTPClientAuth : Singleton<HTTPClientAuth>
    {
        private static void HTTPPostLogin(JSONNode json, string loginRoute)
        {
            PCoroutine.PStartCoroutine(HTTPClientBase.Post(ServerConfig.HttpURL + loginRoute, json.ToString()
                , (res) =>
                {
                    JSONNode jsonRes = JSONNode.Parse(res);
                    if (int.Parse(jsonRes["error"]) == 0)
                    {
                        DataAuth.AuthData = new AuthData();
                        DataAuth.AuthData.userId = int.Parse(jsonRes["data"]["id"]);
                        Debug.Log("User Id: " + DataAuth.AuthData.userId);
                        DataAuth.AuthData.username = jsonRes["data"]["username"];
                        //PDataAuth.AuthData.refresh_token = jsonRes["data"]["refresh_token"];
                        DataAuth.AuthData.token = jsonRes["data"]["token"];
                        WSClient.Instance.Connect(DataAuth.AuthData.userId, DataAuth.AuthData.token);
                    }
                    else
                    {
                        Debug.Log(res);
                    }
                })

            ); ;
        }
        public static void LoginDeviceId()
        {

            string deviceId = SystemInfo.deviceUniqueIdentifier;
            char salt = (char)(deviceId[deviceId.Length / 2] + 15);
            string username = deviceId;
            string password = SHA256Hash.GetSHA256Hash(deviceId + salt);
            password = password.Substring(15, 12);
            JSONNode json = new JSONClass()
        {
            {"password", password },
            {"username", username },
            {"device_id", deviceId},
            {"session_info", new JSONClass()}
        };

            HTTPPostLogin(json, "login");
        }

        public static void LoginByGuest(string token)
        {
            JSONNode json = new JSONClass()
        {
            {"token", token}
        };
            HTTPPostLogin(json, "/login/guest");
        }

        public static void LoginGoogle(string idToken)
        {
            string deviceId = SystemInfo.deviceUniqueIdentifier;

            JSONNode json = new JSONClass()
        {
            {"token",  idToken},
        };
            HTTPPostLogin(json, "/login/google");
        }

        public static void LoginApple(string authentication)
        {
            string deviceId = SystemInfo.deviceUniqueIdentifier;

            JSONNode json = new JSONClass()
        {
            {"token",  authentication},
        };

            HTTPPostLogin(json, "/login/apple");
        }
    }

}
