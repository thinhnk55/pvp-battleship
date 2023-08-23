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
        [SerializeField] GameObject ButtonGroupIos;
        [SerializeField] GameObject ButtonGroupAndroid;

        protected override void Awake()
        {
            base.Awake();
            SetUpLoginScreen();
        }

        void SetUpLoginScreen()
        {
#if PLATFORM_ANDROID
            ButtonGroupIos.SetActive(false);
#else
            ButtonGroupAndroid.SetActive(false);
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
                    ButtonOpenTermPopup.OnOpenTermPopup(ButtonOpenTermPopup.TermType.PRIVATE_POLICY);
                }
                else
                {
                    ButtonOpenTermPopup.OnOpenTermPopup(ButtonOpenTermPopup.TermType.USER_AGREEMENT);
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

