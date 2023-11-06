using Google;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace Authentication
{
    public class LoginGoogle : ISocialAuth
    {
        public string webClientId = "555442977696-0e53ilirr6l6hvu7u567bu4a4c0ekodg.apps.googleusercontent.com";

        private GoogleSignInConfiguration configuration;

        private string idToken = null;

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
                idToken = task.Result.IdToken;
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
            if (idToken != null)
            {
                return;
            }


            GoogleSignIn.Configuration = configuration;
            GoogleSignIn.Configuration.UseGameSignIn = false;
            GoogleSignIn.Configuration.RequestIdToken = true;

            GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished, TaskScheduler.FromCurrentSynchronizationContext());
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
        public void SignOut()
        {
            if (idToken != null)
            {
                idToken = null;
                GoogleSignIn.DefaultInstance.SignOut();
            }
        }

        public void DisableAccount()
        {
            idToken = null;

            GoogleSignIn.DefaultInstance.SignOut();
        }

        public void DeleteAccount()
        {
            idToken = null;

            GoogleSignIn.DefaultInstance.SignOut();
        }
    }
}