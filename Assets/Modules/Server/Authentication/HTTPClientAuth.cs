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
        public static Callback<string> HandleLoginAccountResponse;
        public static Callback<string> HandleCheckLinkAccoutResponse;
        public static Callback<string> HandleLogoutResponse;
        public static Callback<string> HandleDeleteAccountResponse;
        public static Callback<string> HandleDisableAccountResponse;
        public static Callback<string> OnLinkGoogleAccount;
        public static Callback<string> OnLinkAppleAccount;
        #endregion

        #region LOGIN
        private static void HTTPPostLogin(JSONNode json, string loginRoute)
        {
            PCoroutine.PStartCoroutine(HTTPClientBase.Post(ServerConfig.HttpURL + loginRoute, json.ToString()
                , (res) =>
                {
                    HandleLoginAccountResponse(res);
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
            JSONNode json = new JSONClass()
            {
                {"token",  idToken},
            };
            Debug.Log("1");
            HTTPPostLogin(json, "/login/google");
        }

        public static void LoginApple(string authentication)
        {
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
                    HandleCheckLinkAccoutResponse(res);
                }
                , header)
            );
        }

        public static void LinkAccount(string idToken, Callback<string> onLinkedAccount, string route)
        {
            var header = GenHeaderUseridAndToken();

            JSONNode json = new JSONClass()
            {
                { "token", idToken },
            };

            PCoroutine.PStartCoroutine(HTTPClientBase.Post(ServerConfig.HttpURL + "/link" + route, json.ToString(),
                (res) =>
                {
                    onLinkedAccount?.Invoke(res);
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
                    HandleLogoutResponse(res);
                }
                , header));
        }

        public static void DisableAccount()
        {
            var header = GenHeaderUseridAndToken();

            PCoroutine.PStartCoroutine(HTTPClientBase.Get(ServerConfig.HttpURL + "/disable"
                , (res) =>
                {
                    HandleDisableAccountResponse(res);
                }
                , header));
        }

        public static void DeleteAccount()
        {
            var header = GenHeaderUseridAndToken();

            PCoroutine.PStartCoroutine(HTTPClientBase.Get(ServerConfig.HttpURL + "/delete"
                , (res) =>
                {
                    HandleDeleteAccountResponse(res);
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
