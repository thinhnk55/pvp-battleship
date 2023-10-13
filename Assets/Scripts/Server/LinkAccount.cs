using Authentication;
using Framework;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SimpleJSON;

namespace Server
{
    public class LinkAccount : MonoBehaviour
    {
        [SerializeField] Button buttonLinkAppleAccount;
        [SerializeField] TextMeshProUGUI textButtonLinkAppleAccount;
        [SerializeField] Button buttonLinkGoogleAccount;
        [SerializeField] TextMeshProUGUI textButtonLinkGoogleAccount;
        // Start is called before the first frame update
        private void Awake()
        {
            SetUpButtonLinkAccount();
        }

        void Start()
        {
            DataAuth.IsLinkedGoogleAccount.OnDataChanged += OnDataIsLinkedGoogleAccountChange;
            DataAuth.IsLinkedAppleAccount.OnDataChanged += OnDataIsLinkedAppleAccountChange;
            HTTPClientAuth.OnLinkAppleAccount += OnLinkAppleAccount;
            HTTPClientAuth.OnLinkGoogleAccount += OnLinkGoogleAccount;
        }

        private void OnDestroy()
        {
            DataAuth.IsLinkedGoogleAccount.OnDataChanged -= OnDataIsLinkedGoogleAccountChange;
            DataAuth.IsLinkedAppleAccount.OnDataChanged -= OnDataIsLinkedAppleAccountChange;
            HTTPClientAuth.OnLinkAppleAccount -= OnLinkAppleAccount;
            HTTPClientAuth.OnLinkGoogleAccount -= OnLinkGoogleAccount;
        }

#if PLATFORM_IOS
        private void Update()
        {
            if(AuthenticationBase.Instance.auths != null)
                AuthenticationBase.Instance.auths[SocialAuthType.Apple].Update();  
        }
#endif
        private void SetUpButtonLinkAccount()
        {
#if PLATFORM_ANDROID
            buttonLinkAppleAccount.gameObject.SetActive(false);
#else
            buttonLinkAppleAccount.gameObject.SetActive(true);
#endif

            UpdateButtonLinkAppleAccountState(DataAuth.IsLinkedAppleAccount.Data);
            UpdateButtonLinkGoogleAccountState(DataAuth.IsLinkedGoogleAccount.Data);
        }


        #region EVENT HANDLE
        private void OnDataIsLinkedGoogleAccountChange(bool oValue, bool nValue)
        {
            UpdateButtonLinkGoogleAccountState(nValue);
        }

        private void OnDataIsLinkedAppleAccountChange(bool oValue, bool nValue)
        {
            UpdateButtonLinkAppleAccountState(nValue);
        }

        public void LinkAppleAccount()
        {
            buttonLinkAppleAccount.interactable = false;
            AuthenticationBase.Instance.auths[SocialAuthType.Apple].LinkAccount();
        }

        public void LinkGoogleAccount()
        {
            buttonLinkAppleAccount.interactable = false;
            AuthenticationBase.Instance.auths[SocialAuthType.Google].LinkAccount();
        }

        private void UpdateButtonLinkAppleAccountState(bool enable)
        {
            if (enable)
            {
                textButtonLinkAppleAccount.SetText("Logged");
                buttonLinkAppleAccount.interactable = false;
            }
            else
            {
                textButtonLinkAppleAccount.SetText("Sign in");
            }
        }

        private void UpdateButtonLinkGoogleAccountState(bool enable)
        {
            if (enable)
            {
                textButtonLinkGoogleAccount.SetText("Logged");
                buttonLinkGoogleAccount.interactable = false;
            }
            else
            {
                textButtonLinkGoogleAccount.SetText("Sign in");
            }
        }

        /// <summary>
        /// Handle event link account
        /// </summary>
        /// <param name="error">
        /// 0: link successful
        /// 1: system_error
        /// 10: invalid_token
        /// 11: account_linked
        /// </param>
        private void OnLinkAppleAccount(string res)
        {
            JSONNode error = JSONNode.Parse(res);
            switch (error["e"].AsInt)
            {
                case 0:
                    PopupHelper.CreateMessage(PrefabFactory.PopupMessage, "Message", "Apple account linking successful", null);
                    break;
                case 1:
                    PopupHelper.CreateMessage(PrefabFactory.PopupMessage, "Message", "System error!", null);
                    break;
                case 10:
                    PopupHelper.CreateMessage(PrefabFactory.PopupMessage, "Message", "Invalid token!", null);
                    break;
                case 11:
                    PopupHelper.CreateMessage(PrefabFactory.PopupMessage, "Message", "Account linked!", null);
                    break;
            }
        }

        private void OnLinkGoogleAccount(string res)
        {
            JSONNode error = JSONNode.Parse(res);
            switch (error["e"].AsInt)
            {
                case 0:
                    PopupHelper.CreateMessage(PrefabFactory.PopupMessage, "Message", "Google account linking successful", null);
                    break;
                case 1:
                    PopupHelper.CreateMessage(PrefabFactory.PopupMessage, "Message", "System error!", null);
                    break;
                case 10:
                    PopupHelper.CreateMessage(PrefabFactory.PopupMessage, "Message", "Invalid token!", null);
                    break;
                case 11:
                    PopupHelper.CreateMessage(PrefabFactory.PopupMessage, "Message", "Account linked!", null);
                    break;
            }
            #endregion


        }
    }
}

