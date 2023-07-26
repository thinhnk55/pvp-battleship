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
    public class Authentication : SingletonMono<Authentication>, IAuthentication
    {
        public string Token;
        public SocialAuthType type;
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
                        auth = new LoginGoogle();
#endif                  
                        break;
                    case SocialAuthType.GooglePlay:
#if UNITY_ANDROID
#endif
                        break;
                    case SocialAuthType.Facebook:
#if UNITY_ANDROID || UNITY_IOS
#endif
                        break;
                    case SocialAuthType.Apple:
#if UNITY_IOS
                        auth = new LoginApple();
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

                Debug.Log(auth);
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

        public void Initialize()
        {
            auths[this.type].Initialize();
        }

#if PLATFORM_IOS
        private void Update()
        {
            if(auths!=null)
                auths[this.type].Update();  
        }
#endif

        public void SignUp()
        {
            throw new NotImplementedException();
        }

        public void SignOut()
        {
            throw new NotImplementedException();
        }

        public void Authenticate()
        {
            throw new NotImplementedException();
        }

        public void Signup(int type)
        {
            throw new NotImplementedException();
        }

        public void Signin(int type)
        {
            auths[(SocialAuthType)type].SignIn();
        }

        public void Signout(int type)
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void UpdatePlayerName()
        {
            throw new NotImplementedException();
        }
    }
}

