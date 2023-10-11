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
        public static Callback<int> OnLinkGoogleAccount;
        public static Callback<int> OnLinkAppleAccount;
        #endregion

        #region LOGIN
        private static void HTTPPostLogin(JSONNode json, string loginRoute)
        {
            PCoroutine.PStartCoroutine(HTTPClientBase.Post(ServerConfig.HttpURL + loginRoute, json.ToString()
                , (res) =>
                {
                    Debug.Log("2");
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
            Debug.Log("1");
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
            var header = GenHeaderUseridAndToken();

            PCoroutine.PStartCoroutine(HTTPClientBase.Get(ServerConfig.HttpURL + "/link/check",
                (res) =>
                {
                    JSONNode jsonParse = JSONNode.Parse(res);
                    if (int.Parse(jsonParse["error"]) != 0) return;
                    bool islinkedGoogle = jsonParse["data"]["gg"] != null;
                    bool islinkedApple = jsonParse["data"]["ap"] != null;
                    DataAuth.IsLinkedGoogleAccount.Data = jsonParse["data"]["gg"] != null;
                    DataAuth.IsLinkedAppleAccount.Data = jsonParse["data"]["ap"] != null;
                }
                , header)
            );
        }

        public static void LinkAccount(string idToken, Callback<int> onLinkedAccount, string route)
        {
            var header = GenHeaderUseridAndToken();

            JSONNode json = new JSONClass()
            {
                { "token", idToken },
            };

            PCoroutine.PStartCoroutine(HTTPClientBase.Post(ServerConfig.HttpURL + "/link" + route, json.ToString(),
                (res) =>
                {
                    JSONNode jsonParse = JSONNode.Parse(res);
                    onLinkedAccount?.Invoke(jsonParse["error"].AsInt);
                }
                , header)
            );
        }
        public static void LinkGoogleAccount(string idToken)
        {
            LinkAccount(idToken, OnLinkGoogleAccount, "/google");
        }

        public static void LinkAppleAccount(string idToken)
        {
            LinkAccount(idToken, OnLinkAppleAccount, "/apple");
        }
        #endregion

        #region LOGOUT-DISABLE-DELETE ACCOUNT
        public static void Logout()
        {
            var header = GenHeaderUseridAndToken();

            PCoroutine.PStartCoroutine(HTTPClientBase.Get(ServerConfig.HttpURL + "/logout"
                , (res) =>
                {
                    JSONNode jsonParse = JSONNode.Parse(res);
                    if (int.Parse(jsonParse["error"]) == 0)
                    {
                        PopupHelper.CreateMessage(PrefabFactory.PopupMessage, "Message", "Log out successful", null);
                    }
                }
                , header));
        }

        public static void DisableAccount()
        {
            var header = GenHeaderUseridAndToken();

            PCoroutine.PStartCoroutine(HTTPClientBase.Get(ServerConfig.HttpURL + "/disable"
                , (res) =>
                {
                    JSONNode jsonParse = JSONNode.Parse(res);
                    if (int.Parse(jsonParse["error"]) == 0)
                    {
                        PopupHelper.CreateMessage(PrefabFactory.PopupMessage, "Message", "Disable account successful", null);
                    }
                }
                , header));
        }

        public static void DeleteAccount()
        {
            var header = GenHeaderUseridAndToken();

            PCoroutine.PStartCoroutine(HTTPClientBase.Get(ServerConfig.HttpURL + "/delete"
                , (res) =>
                {
                    JSONNode jsonParse = JSONNode.Parse(res);
                    if (int.Parse(jsonParse["error"]) == 0)
                    {
                        PopupHelper.CreateMessage(PrefabFactory.PopupMessage, "Message", "Delete account successful", null);
                    }
                }
                , header));
        }
        #endregion

        #region GEN_HEADER
        public static List<KeyValuePair<string, string>> GenHeaderUseridAndToken()
        {
            List<KeyValuePair<string, string>> header = new List<KeyValuePair<string, string>>();
            header.Add(new KeyValuePair<string, string>("userid", DataAuth.AuthData.userId.ToString()));
            header.Add(new KeyValuePair<string, string>("token", DataAuth.AuthData.token.ToString()));

            return header;
        }
        #endregion
    }

}
