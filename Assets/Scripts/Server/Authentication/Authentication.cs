using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Framework
{
    public class Authentication : SingletonMono<Authentication>, IAuthentication
    {
       // public string Token;
        public SocialAuthType type;
        //public TextMeshProUGUI Error;
        private Dictionary<SocialAuthType, ISocialAuth> auths = new Dictionary<SocialAuthType, ISocialAuth>();

        [SerializeField] GameObject LoadingUI;
        [SerializeField] GameObject ButtonGroupIos;
        [SerializeField] GameObject ButtonGroupAndroid;

        protected async override void Awake()
        {
            base.Awake();
            SetUpLoginScreen();
            try
            {
                await UnityServices.InitializeAsync();
            }
            catch (Exception e){
                Debug.LogException(e);
            }
            //SetupEvents();


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
                        auth = new LoginGuest();
                        break;
                    default:
                        auth = new LoginGuest();
                        break;
                }
                if (auth != null)
                {
                    auth.Initialize();
                    auths.Add((SocialAuthType)i, auth);
                }

                //Debug.Log(((SocialAuthType)i).ToString() + auth);
            }
        }

        void SetUpLoginScreen()
        {
#if PLATFORM_ANDROID
            ButtonGroupIos.SetActive(false);
#else
            ButtonGroupAndroid.SetActive(false);
#endif
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
            //auths[this.type].Initialize();
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

        public void Signin(LoginType type)
        {
            if (!IsAllowedLogin()) return;

            auths[type.authType].SignIn();
            LoadingUI.SetActive(true);
        }

        public bool IsAllowedLogin()
        {
            if(GameData.AcceptLoginTerm[0] && GameData.AcceptLoginTerm[1])
            {
                return true;
            }
            else
            {
                if (!GameData.AcceptLoginTerm[0])
                {
                    ButtonOpenTermPopup.OnOpenTermPopup(ButtonOpenTermPopup.TermType.PRIVATE_POLICY);
                }
                else
                {
                    ButtonOpenTermPopup.OnOpenTermPopup(ButtonOpenTermPopup.TermType.USER_AGREEMENT);
                }
                return false;
            }
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

