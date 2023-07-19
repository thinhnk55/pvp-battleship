using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoginGoogle : ISocialAuth
{
    public string webClientId = "555442977696-0e53ilirr6l6hvu7u567bu4a4c0ekodg.apps.googleusercontent.com";

    private GoogleSignInConfiguration configuration;

/*    private void Udate()
    {
        Debug.Log("config");
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
        Debug.LogError(configuration);
    }*/

    public void SignInWithGoogle() { OnSignIn(); }
    public void SignOutFromGoogle() { OnSignOut(); }

    private void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        Debug.Log("Call Sigin");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    private void OnSignOut()
    {
        GoogleSignIn.DefaultInstance.SignOut();
    }

    public void OnDisconnect()
    {
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                }
                else
                {
                }
            }
        }
        else if (task.IsCanceled)
        {
        }
        else
        {
            HTTPClient.Instance.LoginGoogle(task.Result.IdToken);
            Debug.Log(task.Result.IdToken);
            
        }
    }

    public void OnSignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
    }

    public void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;


        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

/*    private void AddToInformation(string str) { infoText.text += "\n" + str; }*/

    public void Initialize()
    {
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
    }

    public void SignUp()
    {
        throw new NotImplementedException();
    }

    public void SignIn()
    {
        SignInWithGoogle();
    }

    public void SignOut()
    {
        throw new NotImplementedException();
    }
}