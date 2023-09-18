using Framework;
using SimpleJSON;
using UnityEngine;

namespace Server
{
    /// <summary>
    /// This class maintain websocket connection and keep track lost internet connection status
    /// </summary>
    public class WSPingPong : SingletonMono<WSPingPong>
    {
        [SerializeField] float interval = 5;
        private float currentPingPongTime = 0;
        private float pingPongTime = 0; public float PingPongTime { get { return pingPongTime; } }

        void Start()
        {
            WSClient.Instance.OnConnect += () =>
            {
                ServerMessenger.AddListener<JSONNode>(ServerResponse.Pong, Pong);
            };
            WSClient.Instance.OnDisconnect += () =>
            {
                ServerMessenger.RemoveListener<JSONNode>(ServerResponse.Pong, Pong);
            };
            InvokeRepeating("Ping", 0, interval);
        }
        void Ping()
        {
            if ((!WSClient.Instance.ws.IsAlive) || Application.internetReachability == NetworkReachability.NotReachable)
            {
                Messenger.Broadcast(GameEvent.LostConnection);
                WSClient.Instance.Disconnect(true);
                return;
            }
            currentPingPongTime = Time.time;
            WSClient.Instance.Send(new JSONClass() { { "id", ServerRequest.Ping.ToJson() } });
            Debug.Log($"<color=#FFA500>ping</color>");
        }
        void Pong(JSONNode data)
        {
            pingPongTime = Time.time - currentPingPongTime;
            currentPingPongTime = Time.time;
        }
        public static void Create()
        {
            DontDestroyOnLoad(Instantiate(new GameObject("WSPingPong")).AddComponent(typeof(WSPingPong)));
        }
        public static void Destroy()
        {
            Destroy(Instance.gameObject);
        }

    }

}
