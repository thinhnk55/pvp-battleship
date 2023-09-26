using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Authentication
{
    public class Authentication : AuthenticationBase
    {
        [SerializeField] GameObject LoadingUI;
        [SerializeField] GameObject ButtonAppleLogin;

        protected override void Awake()
        {
            base.Awake();
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

        public override void Authenticate()
        {
            throw new NotImplementedException();
        }

        public override void Signup(int type)
        {
            throw new NotImplementedException();
        }

        public override void Signin(int type)
        {
            if (!IsAllowedLogin()) return;

            auths[(SocialAuthType)type].SignIn();
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
                    ButtonOpenTermPopup.OnOpenTermsPopup(ButtonOpenTermPopup.TermType.PRIVATE_POLICY);
                }
                else
                {
                    ButtonOpenTermPopup.OnOpenTermsPopup(ButtonOpenTermPopup.TermType.USER_AGREEMENT);
                }
                return false;
            }
        }

        public override void Signout(int type)
        {
            throw new NotImplementedException();
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }

        public override void UpdatePlayerName()
        {
            throw new NotImplementedException();
        }

    }
}

