using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;

namespace Authentication
{
    public abstract class AuthenticationBase : SingletonMono<AuthenticationBase>
    {
        protected Dictionary<SocialAuthType, ISocialAuth> auths = new Dictionary<SocialAuthType, ISocialAuth>();
        protected async override void Awake()
        {
            base.Awake();
            try
            {
                await UnityServices.InitializeAsync();
            }
            catch (Exception e)
            {
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
                        Debug.LogError("Anonymous");
                        auth = new LoginGuest();
                        break;
                    default:
                        break;
                }
                if (auth != null)
                {
                        Debug.LogError("Anonymous");
                    auth.Initialize();
                    auths.Add((SocialAuthType)i, auth);
                }
            }
        }
        public abstract void Authenticate();
        public abstract void Signup(int type);
        public abstract void Signin(int type);
        public abstract void Signout(int type);
        public abstract void Delete();
        public abstract void UpdatePlayerName();

    } 
}
