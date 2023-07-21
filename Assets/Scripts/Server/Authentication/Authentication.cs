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

            auths = new Dictionary<SocialAuthType, ISocialAuth>();
            for (int i = 0; i <= (int)SocialAuthType.Anonymous; i++)
            {
                ISocialAuth auth = null;
                switch ((SocialAuthType)i)
                {
                    case SocialAuthType.Google:
#if UNITY_ANDROID || UNITY_IOS
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

