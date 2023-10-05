using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;

namespace Authentication
{
    public class AuthenticationBase : Singleton<AuthenticationBase>
    {
        public Dictionary<SocialAuthType, ISocialAuth> auths = new Dictionary<SocialAuthType, ISocialAuth>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static async void Init()
        {
            try
            {
                await UnityServices.InitializeAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            Instance.auths = new Dictionary<SocialAuthType, ISocialAuth>();
            for (int i = 0; i <= (int)SocialAuthType.Guest; i++)
            {
                ISocialAuth auth = null;
                switch ((SocialAuthType)i)
                {
#if UNITY_ANDROID || UNITY_IOS
                    case SocialAuthType.Google:
                        auth = new LoginGoogle();
                        break;
#endif
#if UNITY_ANDROID
                    case SocialAuthType.GooglePlay:
                        break;
#endif
#if UNITY_ANDROID || UNITY_IOS
                    case SocialAuthType.Facebook:
                        break;
#endif
#if UNITY_IOS
                    case SocialAuthType.Apple:
                        auth = new LoginApple();
                        break;
#endif
                    case SocialAuthType.Guest:
                        auth = new LoginGuest();
                        break;
                    default:
                        break;
                }
                if (auth != null)
                {
                    auth.Initialize();
                    Instance.auths.Add((SocialAuthType)i, auth);
                }
            }
        }
    } 
}
