using Framework;
using SimpleJSON;
using System;
using UnityEngine;
using WebSocketSharp;

namespace Framework {
    public abstract class WSClientBase : SingletonMono<WSClientBase>
    {
        public WebSocket ws;

        protected virtual void Start()
        {
            ws = new WebSocket(ServerConfig.WebSocketURL + "?id="+ PDataAuth.AuthData.userId + "&token=" + PDataAuth.AuthData.token);
            ws.OnOpen += OnOpen;
            ws.OnMessage += OnMessage;
            ws.OnError += OnError;
            ws.Connect();
            InvokeRepeating("Ping", 10, 10);

        }
        void Ping()
        {
            ws.Send("{\"id\":2}");
            Debug.Log("{\"id\":2}");
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
                    ServerResponse id = (ServerResponse)int.Parse(idJson);
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