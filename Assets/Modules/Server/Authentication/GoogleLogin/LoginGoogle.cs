using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Google;
namespace Authentication
{
    public class LoginGoogle : ISocialAuth
    {
        public string webClientId = "555442977696-0e53ilirr6l6hvu7u567bu4a4c0ekodg.apps.googleusercontent.com";

        private GoogleSignInConfiguration configuration;

        private void OnSignIn()
        {
            GoogleSignIn.Configuration = configuration;
            GoogleSignIn.Configuration.UseGameSignIn = false;
            GoogleSignIn.Configuration.RequestIdToken = true;

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
                        Debug.Log(error);
                    }
                    else
                    {
                        Debug.Log("Error!!!!!!!!");
                    }
                }
            }
            else if (task.IsCanceled)
            {
                Debug.Log("isCanceled!!!");
            }
            else
            {
                Debug.Log("Login google Done");
                Debug.Log(task.Result.IdToken);
                HTTPClientAuth.LoginGoogle(task.Result.IdToken);
            }
        }

        internal void OnLinkAcountFinished(Task<GoogleSignInUser> task)
        {
            if (task.IsFaulted)
            {
                using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                        Debug.Log(error);
                    }
                    else
                    {
                        Debug.Log("Error!!!!!!!!");
                    }
                }
            }
            else if (task.IsCanceled)
            {
                Debug.Log("isCanceled!!!");
            }
            else
            {
                Debug.Log("link google account Done");
                Debug.Log(task.Result.IdToken);
                HTTPClientAuth.LinkGoogleAccount(task.Result.IdToken);
            }
        }

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
            GoogleSignIn.Configuration = configuration;
            GoogleSignIn.Configuration.UseGameSignIn = false;
            GoogleSignIn.Configuration.RequestIdToken = true;

            GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
        }

        public void SignOut()
        {
            HTTPClientAuth.Logout();
            GoogleSignIn.DefaultInstance.SignOut();
        }
        public void LinkAccount()
        {
            GoogleSignIn.Configuration = configuration;
            GoogleSignIn.Configuration.UseGameSignIn = false;
            GoogleSignIn.Configuration.RequestIdToken = true;

            GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnLinkAcountFinished);
        }

        public void Update()
        {
        }

        public void DisableAccount()
        {
            HTTPClientAuth.DisableAccount();
            GoogleSignIn.DefaultInstance.SignOut();
        }

        public void DeleteAccount()
        {
            HTTPClientAuth.DeleteAccount();
            GoogleSignIn.DefaultInstance.SignOut();
        }
    }
}