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
        [SerializeField] float interval = 2;
        private float currentPingPongTime = 0;
        private float pingPongTime = 0; public float PingPongTime { get { return pingPongTime; } }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Init()
        {
            WSClient.Instance.OnConnect += () =>
            {
                ServerMessenger.AddListener<JSONNode>(ServerResponse.Pong, Pong);
            };
            WSClient.Instance.OnDisconnect += () =>
            {
                ServerMessenger.RemoveListener<JSONNode>(ServerResponse.Pong, Pong);
            };
        }
        protected void Start()
        {
            InvokeRepeating("Ping", 1, interval);
        }
        void Ping()
        {
            if ((!WSClient.Instance.ws.IsAlive) || Application.internetReachability == NetworkReachability.NotReachable)
            {
                Messenger.Broadcast(GameEvent.LostConnection);
                PDebug.Log("Ping failed");
                WSClient.Instance.Disconnect(true);
                return;
            }
            currentPingPongTime = Time.time;
            WSClient.Instance.Send(new JSONClass() { { "id", ServerRequest.Ping.ToJson() } });

        }
        static void Pong(JSONNode data)
        {
            Instance.pingPongTime = Time.time - Instance.currentPingPongTime;
            Instance.currentPingPongTime = Time.time;
        }
        public static void Create()
        {
            DontDestroyOnLoad(Instantiate(new GameObject("WSPingPong")).AddComponent(typeof(WSPingPong)));
        }
        public static void Destroy()
        {
            if (Instance)
            {
                Destroy(Instance.gameObject);
            }
        }

    }

}
