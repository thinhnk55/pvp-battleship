using Framework;
using Server;
using SimpleJSON;
using System;
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
        public static void HTTPGetCheckLinkedAccount(JSONNode json, string linkAccountRouter)
        {
            PCoroutine.PStartCoroutine(HTTPClientBase.Get(ServerConfig.HttpURL + linkAccountRouter,
                (res) =>
                {
                    JSONNode jsonParse = JSONNode.Parse(res);
                    if (int.Parse(jsonParse["error"]) != 0) return;
                    bool islinkedGoogle = jsonParse["data"]["gg"] != null;
                    bool islinkedApple = jsonParse["data"]["ap"] != null;
                    OnCheckLinkedAccount?.Invoke(islinkedGoogle, islinkedApple);
                })
            );
        }

        public static void CheckLinkedAccount(string idToken, string userId)
        {
            JSONNode json = new JSONClass()
            {
                {"userid", userId},
                {"token", idToken},
            };

            HTTPGetCheckLinkedAccount(json, "/link/check");
        }
        public static void LinkAccount(string idToken, string userId, string route)
        {
            JSONNode json = new JSONClass()
            {
                {"userid", userId},
                {"token", idToken},
            };
            PCoroutine.PStartCoroutine(HTTPClientBase.Post(ServerConfig.HttpURL + "/link" + route, json.ToString(),
                (res) =>
                {
                    JSONNode jsonParse = JSONNode.Parse(res);
                    OnLinkGoogleAccount(jsonParse["error"].AsInt == 0);
                })
            );
        }
        public static void LinkGoogleAccount(string idToken, string userId)
        {
            LinkAccount(idToken, userId, "/gg");
        }

        public static void LinkAppleAccount(string idToken, string userId)
        {
            LinkAccount(idToken, userId, "/apple");
        }
        #endregion
    }

}
