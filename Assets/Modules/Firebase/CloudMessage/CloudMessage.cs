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
                FirebaseMessaging.TokenReceived += OnTokenReceived;
                FirebaseMessaging.MessageReceived += OnMessageReceived;
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

        public void OnTokenReceived(object sender, TokenReceivedEventArgs token)
        {
            Debug.Log("Received Registration Token: " + token.Token);
        }

        public void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Debug.Log("From: " + e.Message.From);
            Debug.Log("Message ID: " + e.Message.MessageId);
        }
    }
}
