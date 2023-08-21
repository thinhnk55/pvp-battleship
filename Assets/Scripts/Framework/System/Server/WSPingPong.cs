using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class maintain websocket connection and keep track lost internet connection status
/// </summary>
public class WSPingPong : SingletonMono<WSPingPong>
{
    [SerializeField]float interval = 10;
    void Start()
    {
        InvokeRepeating("Ping", interval, interval);  
    }
    void Ping()
    {
        if ((!WSClient.Instance.ws.IsAlive) || Application.internetReachability == NetworkReachability.NotReachable)
        {
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
