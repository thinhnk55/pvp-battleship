using Framework;
using UnityEngine;

/// <summary>
/// This class maintain websocket connection and keep track lost internet connection status
/// </summary>
namespace Server
{
    public class WSPingPong : SingletonMono<WSPingPong>
    {
        [SerializeField] float interval = 15;
        void Start()
        {
            InvokeRepeating("Ping", interval, interval);
        }
        void Ping()
        {
            if ((!WSClient.Instance.ws.IsAlive) || Application.internetReachability == NetworkReachability.NotReachable)
            {
                WSClient.Instance.Disconnect(true);
                Messenger.Broadcast(GameEvent.LostConnection);
                return;
            }
            WSClient.Instance.Ping();
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
