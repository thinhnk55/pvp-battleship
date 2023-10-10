using Authentication;
using Framework;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Server
{
    public class LinkAccount : MonoBehaviour
    {
        [SerializeField] Button buttonLinkAppleAccount;
        [SerializeField] TextMeshProUGUI textButtonLinkAppleAccount;
        [SerializeField] Button buttonLinkGoogleAccount;
        [SerializeField] TextMeshProUGUI textButtonLinkGoogleAccount;
        // Start is called before the first frame update
        void Start()
        {
            HTTPClientAuth.OnCheckLinkedAccount += OnCheckLinkedAccount;
            HTTPClientAuth.OnLinkedAppleAccount += OnLinkAppleAccount;
            HTTPClientAuth.OnLinkedGoogleAccount += OnLinkGoogleAccount;
        }

        private void OnDestroy()
        {
            HTTPClientAuth.OnCheckLinkedAccount -= OnCheckLinkedAccount;
            HTTPClientAuth.OnLinkedAppleAccount -= OnLinkAppleAccount;
            HTTPClientAuth.OnLinkedGoogleAccount -= OnLinkGoogleAccount;
        }

#if PLATFORM_IOS
        private void Update()
        {
            if(AuthenticationBase.Instance.auths != null)
                AuthenticationBase.Instance.auths[SocialAuthType.Apple].Update();  
        }
#endif

        private void OnCheckLinkedAccount(bool isLinkedGoogle, bool isLinkedApple)
        {
            if (isLinkedGoogle)
            {
                textButtonLinkGoogleAccount.SetText("Logged");
                buttonLinkGoogleAccount.interactable = false;
            }
            else
            {
                textButtonLinkGoogleAccount.SetText("Sign in");
            }

            if (isLinkedApple)
            {
                textButtonLinkAppleAccount.SetText("Logged");
                buttonLinkAppleAccount.interactable = false;
            }
            else
            {
                textButtonLinkAppleAccount.SetText("Sign in");
            }
        }

        public void LinkAppleAccount()
        {
            buttonLinkAppleAccount.interactable = false;
            AuthenticationBase.Instance.auths[SocialAuthType.Apple].SignIn();
        }

        public void LinkGoogleAccount()
        {
            buttonLinkAppleAccount.interactable = false;
            AuthenticationBase.Instance.auths[SocialAuthType.Google].SignIn();
        }

        private void OnLinkAppleAccount(bool isSuccess)
        {
            if(isSuccess) 
            {
                Debug.Log("Apple account Linking successful");
            }
            else
            {
                buttonLinkAppleAccount.interactable = true;
                Debug.Log("Apple account linking failed");
            }
        }

        private void OnLinkGoogleAccount(bool isSuccess)
        {
            if (isSuccess)
            {
                Debug.Log("Google account Linking successful");
            }
            else
            {
                buttonLinkGoogleAccount.interactable = true;
                Debug.Log("Google account linking failed");
            }
        }

    }
}

