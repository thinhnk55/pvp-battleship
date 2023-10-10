using Framework;
using Server;
using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Authentication
{
    public class HTTPClientAuth : Singleton<HTTPClientAuth>
    {
        #region EVENT
        //Paramater 1: IsLinkedGoogleAccount
        //Paramater 2: IsLinkedAppleAccount
        public static Action<bool, bool> OnCheckLinkedAccount;
        //Paramater : IsSuccess
        public static Action<bool> OnLinkGoogleAccount;
        //Paramater : IsSuccess
        public static Action<bool> OnLinkAppleAccount;
        
        #endregion

        #region LOGIN
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
                        Debug.Log("HTTP Login");
                        WSClient.Instance.Connect(DataAuth.AuthData.userId, DataAuth.AuthData.token);
                    }
                    else
                    {
                        Debug.Log(res);
                    }
                })

            );
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
        #endregion

        #region LINK ACCOUNT
        public static void CheckLinkedAccount()
        {
            var header = new List<KeyValuePair<string, string>>();
            header.Add(new KeyValuePair<string, string>("userid", DataAuth.AuthData.userId.ToString()));
            header.Add(new KeyValuePair<string, string>("token", DataAuth.AuthData.token.ToString()));

            PCoroutine.PStartCoroutine(HTTPClientBase.Get(ServerConfig.HttpURL + "/link/check",
                (res) =>
                {
                    JSONNode jsonParse = JSONNode.Parse(res);
                    if (int.Parse(jsonParse["error"]) != 0)
                        return;


                    if (jsonParse["data"].Count == 0)
                    {
                        OnCheckLinkedAccount(false, false);
                    }
                    else
                    {
                        if (jsonParse["data"]["gg"] == null)
                        {
                            OnCheckLinkedAccount(false, true);
                        }
                        else if (jsonParse["data"]["ap"] == null)
                        {
                            OnCheckLinkedAccount(true, false);
                        }
                        else
                        {
                            OnCheckLinkedAccount(true, true);
                        }
                    }

                }
                , header)
            );
        }
        public static void LinkGoogleAccount(string idToken)
        {
            var header = new List<KeyValuePair<string, string>>();
            header.Add(new KeyValuePair<string, string>("userid", DataAuth.AuthData.userId.ToString()));
            header.Add(new KeyValuePair<string, string>("token", DataAuth.AuthData.token.ToString()));

            PCoroutine.PStartCoroutine(HTTPClientBase.Post(ServerConfig.HttpURL + "/link/google", idToken,
                (res) =>
                {
                    JSONNode jsonParse = JSONNode.Parse(res);
                    if (int.Parse(jsonParse["error"]) == 0)
                    {
                        OnLinkGoogleAccount(true);
                    }
                    else
                    {
                        OnLinkGoogleAccount(false);
                        Debug.Log(res.ToString());
                    }
                }
                , header)
            );
        }

        public static void LinkAppleAccount(string idToken)
        {
            var header = new List<KeyValuePair<string, string>>();
            header.Add(new KeyValuePair<string, string>("userid", DataAuth.AuthData.userId.ToString()));
            header.Add(new KeyValuePair<string, string>("token", DataAuth.AuthData.token.ToString()));

            PCoroutine.PStartCoroutine(HTTPClientBase.Post(ServerConfig.HttpURL + "/link/apple", idToken,
                (res) =>
                {
                    JSONNode jsonParse = JSONNode.Parse(res);
                    if (int.Parse(jsonParse["error"]) == 0)
                    {
                        OnLinkAppleAccount(true);
                    }
                    else
                    {
                        OnLinkAppleAccount(false);
                        Debug.Log(res.ToString());
                    }
                }
                , header)
            );

        }
        #endregion
    }

}
