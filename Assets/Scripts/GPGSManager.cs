using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames.BasicApi;
using GooglePlayGames;
using TMPro;


#if UNITY_ANDROID || UNITY_IOS
public class GPGSManager : MonoBehaviour
{
    PlayGamesClientConfiguration config;
    [SerializeField] TextMeshProUGUI text;
    private void Start()
    {
        ConfigGPGS();
        SignInGPGS(SignInInteractivity.CanPromptOnce, config);
    }

    internal void ConfigGPGS()
    {
        config = new PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false).Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
    }

    internal void SignInGPGS(SignInInteractivity signInInteractivity, PlayGamesClientConfiguration config)
    {
        Debug.Log("Auth");
        PlayGamesPlatform.Instance.Authenticate(signInInteractivity, (code) =>
        {
            text.text = "Authenticating...";
            if (code == SignInStatus.Success)
            {
                text.text = "Successful :" + Social.localUser.userName + " Id :" + Social.localUser.id;
            }
            else
            {
                text.text = "Failed :" + code;
            }

        });
    }

    public void SignIn()
    {
        SignInGPGS(SignInInteractivity.CanPromptAlways, config);

    }
    public void SignOut()
    {
        PlayGamesPlatform.Instance.SignOut();
    }
}
#endif