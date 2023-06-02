using GooglePlayGames.BasicApi;
using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Framework;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;

public class GoogleAuth : ISocialAuth
{
    public void Initialize()
    {
        var config = new PlayGamesClientConfiguration.Builder()
            .AddOauthScope("profile")
            .RequestIdToken()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }
    public void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(OnGoogleLogin);
    }
    void OnGoogleLogin(bool sucess)
    {
        if (sucess)
        {
            SceneTransitionHelper.Load(ESceneName.Home);
            Debug.Log("Login with Google done. IdToken: " + ((PlayGamesLocalUser)Social.localUser).GetIdToken());
        }
        else
        {
            Debug.Log("Unsuccessful login");
        }
    }
    void OnGoogleLogin(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // Call Unity Authentication SDK to sign in or link with Google.
            SceneTransitionHelper.Load(ESceneName.Home);
            Debug.Log("Login with Google done. IdToken: " + ((PlayGamesLocalUser)Social.localUser).GetIdToken());
        }
        else
        {
            SceneTransitionHelper.Load(ESceneName.Home);
            Debug.Log("Unsuccessful login");
        }
    }

    async Task SignInWithGoogleAsync(string idToken)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithGoogleAsync(idToken);
            Debug.Log("SignIn is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
    async Task LinkWithGoogleAsync(string idToken)
    {
        try
        {
            await AuthenticationService.Instance.LinkWithGoogleAsync(idToken);
            Debug.Log("Link is successful.");
        }
        catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
        {
            Debug.LogError("This user is already linked with another account. Log in instead.");
        }

        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    public void SignUp()
    {
        throw new NotImplementedException();
    }



    public void SignOut()
    {
        throw new NotImplementedException();
    }
}
