using Authentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkAccount : SingletonMono<LinkAccount>
{
    [SerializeField] Button buttonLinkAppleAccount;
    [SerializeField] Button buttonLinkGoogleAccount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

#if PLATFORM_IOS
        private void Update()
        {
            if(AuthenticationBase.Instance.auths != null)
                AuthenticationBase.Instance.auths[SocialAuthType.Apple].Update();  
        }
#endif


}
