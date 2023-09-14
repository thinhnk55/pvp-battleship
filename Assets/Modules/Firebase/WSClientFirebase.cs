using Firebase.Messaging;
using Framework;
using Server;
using SimpleJSON;
using UnityEngine;

namespace FirebaseIntegration
{
    public class WSClientFirebase : Singleton<WSClientFirebase>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Init()
        {
            WSClient.Instance.OnConnect += () =>
            {
                Messenger.AddListener<MaxSdkBase.AdInfo>(GameEvent.REWARD_ADS_INFO, AnalyticsHelper.WatchAds);
                ServerMessenger.AddListener<JSONNode>(ServerResponse._FIREBASE_ASK_UPDATE_TOKEN, FirebaseAskForUpdateToken);
            };

            WSClient.Instance.OnDisconnect += () =>
            {
                Messenger.RemoveListener<MaxSdkBase.AdInfo>(GameEvent.REWARD_ADS_INFO, AnalyticsHelper.WatchAds);
                ServerMessenger.RemoveListener<JSONNode>(ServerResponse._FIREBASE_ASK_UPDATE_TOKEN, FirebaseAskForUpdateToken);
            };
        }


        #region FirebaseCloudMessaging
        private static async void FirebaseAskForUpdateToken(JSONNode data)
        {
            var task = FirebaseMessaging.GetTokenAsync();

            await task;

            if (task.IsCompleted)
            {
                FirebaseUpdateToken(task.Result);
            }
        }

        public static void FirebaseUpdateToken(string token)
        {
            new JSONClass()
        {
            { "id", ServerRequest._FIREBASE_UPDATE_TOKEN.ToJson() },
            { "token", token}
        }.RequestServer();
        }
        #endregion
    }
}
