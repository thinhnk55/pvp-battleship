using GooglePlayGames;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;


namespace Framework
{
    public class Authentication : Singleton<Authentication>, IAuthentication
    {
        public string Token;
        public TextMeshProUGUI Error;
        Dictionary<SocialAuthType, ISocialAuth> auths;

        protected async override void Awake()
        {
            base.Awake();
            try
            {
                await UnityServices.InitializeAsync();
            }
            catch (Exception e){
                Debug.LogException(e);
            }
            SetupEvents();

            auths = new Dictionary<SocialAuthType, ISocialAuth>();
            for (int i = 0; i <= (int)SocialAuthType.Anonymous; i++)
            {
                ISocialAuth auth = null;
                switch ((SocialAuthType)i)
                {
                    case SocialAuthType.Google:
#if UNITY_ANDROID || UNITY_IOS
                        auth = new GoogleAuth();
#endif
                        break;
                    case SocialAuthType.GooglePlay:
#if UNITY_ANDROID
                        auth = new GooglePlayAuth();
#endif
                        break;
                    case SocialAuthType.Facebook:
#if UNITY_ANDROID || UNITY_IOS
                        auth = new FacebookAuth();
#endif
                        break;
                    case SocialAuthType.Apple:
#if UNITY_IOS
                        auth = new ApleAuth();
#endif
                        break;
                    case SocialAuthType.Anonymous:
                        auth = new AnonymousAuth();
                        break;
                    default:
                        auth = new AnonymousAuth();
                        break;
                }
                if (auth != null)
                {
                    auth.Initialize();
                    auths.Add((SocialAuthType)i, auth);
                }

            }
        }
        void SetupEvents()
        {
            AuthenticationService.Instance.SignedIn += () => {
                // Shows how to get a playerID
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

                // Shows how to get an access token
                Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
                SceneTransitionHelper.Load(ESceneName.Home);


            };
            AuthenticationService.Instance.SignInFailed += (err) => {
                Debug.LogError("fail:"+ err);
                SceneTransitionHelper.Load(ESceneName.Home);
            };

            AuthenticationService.Instance.SignedOut += () => {
                Debug.Log("Player signed out.");
            };

            AuthenticationService.Instance.Expired += () =>
            {
                Debug.Log("Player session could not be refreshed and expired.");
            };
        }
        async Task SignInAnonymouslyAsync()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Sign in anonymously succeeded!");

                // Shows how to get the playerID
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
        }


        public void Authenticate()
        {
            throw new NotImplementedException();
        }

        public void Signup(int type)
        {
            auths[(SocialAuthType)type].SignUp();
        }

        public void Signin(int type)
        {
            auths[(SocialAuthType)type].SignIn();
        }

        public void Signout(int type)
        {
            auths[(SocialAuthType)type].SignOut();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public async void UpdatePlayerName()
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync("newPlayerName");
            Debug.Log(AuthenticationService.Instance.PlayerName);
        }
    }
}

