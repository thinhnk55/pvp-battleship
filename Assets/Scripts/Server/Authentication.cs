using System;
using UnityEngine;

namespace Authentication
{
    public class Authentication : MonoBehaviour
    {
        [SerializeField] GameObject LoadingUI;
        [SerializeField] GameObject ButtonAppleLogin;

        protected void Awake()
        {
            SetUpLoginScreen();
        }

        void SetUpLoginScreen()
        {
#if PLATFORM_ANDROID
            ButtonAppleLogin.SetActive(false);
#else
            ButtonAppleLogin.SetActive(true);
#endif
        }


#if PLATFORM_IOS
        private void Update()
        {
            if(AuthenticationBase.Instance.auths != null)
                AuthenticationBase.Instance.auths[SocialAuthType.Apple].Update();  
        }
#endif

        public void Signin(int type)
        {
            if (!IsAllowedLogin()) return;

            AuthenticationBase.Instance.auths[(SocialAuthType)type].SignIn();
            GameData.TypeLogin = (SocialAuthType)type;
            LoadingUI.SetActive(true);
        }

        public bool IsAllowedLogin()
        {
            if (GameData.AcceptLoginTerm[0] && GameData.AcceptLoginTerm[1])
            {
                return true;
            }
            else
            {
                if (!GameData.AcceptLoginTerm[0])
                {
                    ButtonOpenTermPopup.OnOpenTermsPopup(ButtonOpenTermPopup.TermType.PRIVATE_POLICY);
                }
                else
                {
                    ButtonOpenTermPopup.OnOpenTermsPopup(ButtonOpenTermPopup.TermType.USER_AGREEMENT);
                }
                return false;
            }
        }

        public void Signout(int type)
        {
            AuthenticationBase.Instance.auths[(SocialAuthType)type].SignOut();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}

