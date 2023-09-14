using DG.Tweening;
using Firebase.Messaging;
using UnityEngine;

namespace FirebaseIntegration
{
    public class CloudMessage : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            FirebaseInitialization.OnInitialized += () =>
            {
                Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
                Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
                DOVirtual.DelayedCall(3, GetTokenAsync);
            };
        }

        public async void GetTokenAsync()
        {
            var task = FirebaseMessaging.GetTokenAsync();

            await task;

            if (task.IsCompleted)
            {
                Debug.Log("GET TOKEN ASYNC " + task.Result);
            }
        }

        public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
        {
            UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
            WSClientFirebase.FirebaseUpdateToken(token.Token);
        }

        public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
        {
            UnityEngine.Debug.Log("From: " + e.Message.From);
            UnityEngine.Debug.Log("Message ID: " + e.Message.MessageId);
        }
    }
}
