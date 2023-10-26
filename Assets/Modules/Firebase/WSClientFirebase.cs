using Firebase.Messaging;
using Framework;
using Server;
using SimpleJSON;
using UnityEngine;

namespace FirebaseIntegration
{
    public class WSClientFirebase : Singleton<WSClientFirebase>
    {
        public static bool isNewTokenCloudMessage = false;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Init()
        {
            WSClient.Instance.OnConnect += () =>
            {
                Messenger.AddListener<MaxSdkBase.AdInfo>(GameEvent.REWARD_ADS_INFO, AnalyticsHelper.WatchAds);
                ServerMessenger.AddListener<JSONNode>(ServerResponse._FIREBASE_ASK_UPDATE_TOKEN, FirebaseAskForUpdateToken);
#if UNITY_EDITOR == false
                if(isNewTokenCloudMessage)
                {
                    FirebaseUpdateToken(FirebaseData.TokenCloudMessage);
                }
#endif
            };

            WSClient.Instance.OnDisconnect += () =>
            {
                Messenger.RemoveListener<MaxSdkBase.AdInfo>(GameEvent.REWARD_ADS_INFO, AnalyticsHelper.WatchAds);
                ServerMessenger.RemoveListener<JSONNode>(ServerResponse._FIREBASE_ASK_UPDATE_TOKEN, FirebaseAskForUpdateToken);
            };
        }


        #region FirebaseCloudMessaging
        private static void FirebaseAskForUpdateToken(JSONNode data)
        {
            FirebaseUpdateToken(FirebaseData.TokenCloudMessage);
        }

        public static void FirebaseUpdateToken(string tokenCloudMessage)
        {
            Debug.Log("UpdateToken");
            new JSONClass()
            {
                { "id", ServerRequest._FIREBASE_UPDATE_TOKEN.ToJson() },
                { "token", tokenCloudMessage}
            }.RequestServer();
        }
        #endregion
    }
}
