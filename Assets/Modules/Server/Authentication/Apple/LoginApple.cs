#if UNITY_IOS
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Extensions;
using AppleAuth.Interfaces;
using AppleAuth.Native;
using System;
using System.Text;
using UnityEngine;
namespace Authentication
{
    public class LoginApple : ISocialAuth
    {
        private const string AppleUserIdKey = "AppleUserId";

        private IAppleAuthManager _appleAuthManager;

        private string idToken = null;

        public void Initialize()
        {
            // If the current platform is supported
            if (AppleAuthManager.IsCurrentPlatformSupported)
            {
                // Creates a default JSON deserializer, to transform JSON Native responses to C# instances
                var deserializer = new PayloadDeserializer();
                // Creates an Apple Authentication manager with the deserializer
                this._appleAuthManager = new AppleAuthManager(deserializer);
            }
        }

        public void SignUp()
        {
            throw new NotImplementedException();
        }

        public void SignIn()
        {
            if(idToken != null)
            {
                return;
            }

            var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName);

            this._appleAuthManager.LoginWithAppleId(
                loginArgs,
                credential =>
                {
                    var appleIdCredential = credential as IAppleIDCredential;
                    Debug.Log("Sign in with apple: " + Encoding.UTF8.GetString(appleIdCredential.AuthorizationCode));
                    idToken = Encoding.UTF8.GetString(appleIdCredential.AuthorizationCode);
                    HTTPClientAuth.LoginApple(idToken);
                },
                error =>
                {
                    var authorizationErrorCode = error.GetAuthorizationErrorCode();
                    Debug.LogWarning("Sign in with Apple failed " + authorizationErrorCode.ToString() + " " + error.ToString());
                });
        }

        public void LinkAccount()
        {
            var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName);

            this._appleAuthManager.LoginWithAppleId(
                loginArgs,
                credential =>
                {
                    // If a sign in with apple succeeds, we should have obtained the credential with the user id, name, and email, save it
                    PlayerPrefs.SetString(AppleUserIdKey, credential.User);
                    var appleIdCredential = credential as IAppleIDCredential;
                    Debug.Log("Sign in with apple: " + Encoding.UTF8.GetString(appleIdCredential.AuthorizationCode));

                    HTTPClientAuth.LinkAppleAccount(Encoding.UTF8.GetString(appleIdCredential.AuthorizationCode));
                },
                error =>
                {
                    var authorizationErrorCode = error.GetAuthorizationErrorCode();
                    Debug.LogWarning("Sign in with Apple failed " + authorizationErrorCode.ToString() + " " + error.ToString());
                });
        }

        public void SignOut()
        {
            idToken = null;
        }

        void ISocialAuth.Update()
        {
            if (this._appleAuthManager != null)
            {
                this._appleAuthManager.Update();
            }
        }

        public void DisableAccount()
        {
            idToken = null;
        }

        public void DeleteAccount()
        {
            idToken = null;
        }
    }
}
#endif