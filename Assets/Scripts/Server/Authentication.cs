using Framework;
using Server;
using SimpleJSON;
using UnityEngine;

namespace Authentication
{
    public class Authentication : MonoBehaviour
    {
        [SerializeField] GameObject LoadingUI;
        [SerializeField] GameObject ButtonAppleLogin;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Init()
        {
            HTTPClientAuth.HandleLoginAccountResponse += HandleLoginAccountResponse;
            HTTPClientAuth.HandleCheckLinkAccoutResponse += HandleCheckLinkAccoutResponse;
            HTTPClientAuth.HandleLogoutResponse += HandleLogoutResponse;
            HTTPClientAuth.HandleDisableAccountResponse += HandleDisableAccountResponse;
            HTTPClientAuth.HandleDeleteAccountResponse += HandleDeleteAccountResponse;
            WSClient.Instance.OnDisconnect += () => { HTTPClientAuth.Logout(); };
        } 

        protected void Awake()
        {
            SetUpLoginScreen();
        }

        void SetUpLoginScreen()
        {
#if PLATFORM_ANDROID
            if (ButtonAppleLogin != null)
            {
                ButtonAppleLogin?.SetActive(false);
            }
#else
            if(ButtonAppleLogin != null)
            {
                ButtonAppleLogin?.SetActive(true);
            }
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

        public void Logout()
        {
            HTTPClientAuth.Logout();
        }

        public void Disable()
        {
            HTTPClientAuth.DisableAccount();
        }

        public void Delete()
        {
            PopupHelper.CreateConfirm(PrefabFactory.PopupDeleteAccount, null, null, null, (ok) =>
            {
                if (ok)
                {
                    PopupHelper.Create(PrefabFactory.PopupDeleteConfirm);
                }
                else
                {
                    HTTPClientAuth.DisableAccount();
                }
            });
            //HTTPClientAuth.DeleteAccount();
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

        #region HANDLE EVENT
        #region LOGIN ACCOUNT
        public static void HandleLoginAccountResponse(string res)
        {
            JSONNode jsonRes = JSONNode.Parse(res);
            if (int.Parse(jsonRes["error"]) == 0)
            {
                DataAuth.AuthData = new AuthData();
                DataAuth.AuthData.userId = int.Parse(jsonRes["data"]["id"]);
                DataAuth.AuthData.username = jsonRes["data"]["username"];
                //PDataAuth.AuthData.refresh_token = jsonRes["data"]["refresh_token"];
                DataAuth.AuthData.token = jsonRes["data"]["token"];
                WSClient.Instance.Connect(DataAuth.AuthData.userId, DataAuth.AuthData.token);
            }
            else
            {
                Debug.Log(res);
            }
        }
        #endregion  

        #region CHECK LINK ACCOUNT
        public static void HandleCheckLinkAccoutResponse(string res)
        {
            JSONNode jsonParse = JSONNode.Parse(res);
            if (int.Parse(jsonParse["error"]) == 0)
            {
                DataAuth.IsLinkedGoogleAccount.Data = jsonParse["data"]["gg"] != null;
                DataAuth.IsLinkedAppleAccount.Data = jsonParse["data"]["ap"] != null;
            }
        }

        #endregion

        #region LOGOUT-DISABLE-DELETE ACCOUNT
        public static void HandleLogoutResponse(string res)
        {
            JSONNode jsonParse = JSONNode.Parse(res);
            if (int.Parse(jsonParse["error"]) == 0)
            {
                AuthenticationBase.Instance.auths[GameData.TypeLogin].SignOut();
                WSClient.Instance.Disconnect(true);
                SceneTransitionHelper.Load(ESceneName.PreHome);
            }
        }

        public static void HandleDisableAccountResponse(string res)
        {
            JSONNode jsonParse = JSONNode.Parse(res);
            if (int.Parse(jsonParse["error"]) == 0)
            {
                AuthenticationBase.Instance.auths[GameData.TypeLogin].DisableAccount();
                WSClient.Instance.Disconnect(true);
                SceneTransitionHelper.Load(ESceneName.PreHome);
            }
        }

        public static void HandleDeleteAccountResponse(string res)
        {
            JSONNode jsonParse = JSONNode.Parse(res);
            if (int.Parse(jsonParse["error"]) == 0)
            {
                AuthenticationBase.Instance.auths[GameData.TypeLogin].DeleteAccount();
                WSClient.Instance.Disconnect(true);
                SceneTransitionHelper.Load(ESceneName.PreHome);
            }
        }
        #endregion
        #endregion
    }
}

