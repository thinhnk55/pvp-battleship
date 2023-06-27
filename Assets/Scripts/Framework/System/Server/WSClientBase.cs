using Framework;
using SimpleJSON;
using System;
using UnityEngine;
using WebSocketSharp;

namespace Framework {
    public abstract class WSClientBase : Singleton<WSClientBase>
    {
        public WebSocket ws;
        protected virtual void Start()
        {
            ws = new WebSocket(ServerConfig.WebSocketURL + "?id="+ PDataAuth.AuthData.userId + "&token=" + PDataAuth.AuthData.token);
            ws.OnOpen += OnOpen;
            ws.OnMessage += OnMessage;
            ws.OnError += OnError;
            ws.Connect();
            InvokeRepeating("Ping", 10, 14);

        }
        void Ping()
        {
             ws.Ping();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            ws.OnOpen -= OnOpen;
            ws.OnMessage -= OnMessage;
            ws.OnError -= OnError;
        }
        public void Send(JSONNode json)
        {
            ws.Send(json.ToString());
            Debug.Log(json);
        }
        public void OnOpen(object sender, EventArgs e)
        {
            Debug.Log("Open " + ((WebSocket)sender).Url);
        }
        public void OnMessage(object sender, MessageEventArgs e)
        {
            Debug.Log(e.Data);
            MainThreadDispatcher.ExecuteOnMainThread(() =>
            {
                JSONNode idJson = JSON.Parse(e.Data)["id"];
                if (idJson != null)
                {
                    GameServerEvent id = (GameServerEvent)int.Parse(idJson);
                    if (ServerMessenger.eventTable.ContainsKey(id))
                    {
                        ServerMessenger.Broadcast(id, JSON.Parse(e.Data));
                    }
                }
            });
        }
        public void OnError(object sender, ErrorEventArgs e)
        {
            Debug.Log("Error : " + e.Exception);
        }

    }
}