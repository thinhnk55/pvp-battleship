using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSPingPong : SingletonMono<WSPingPong>
{
    [SerializeField]float interval = 10;
    void Start()
    {
        InvokeRepeating("Ping", interval, interval);  
    }
    void Ping()
    {
        if (!WSClient.Instance.ws.IsAlive)
        {
            ServerMessenger.Broadcast(ServerResponse.LostConnection);
            return;
        }
        WSClient.Instance.Ping();
    }
}
