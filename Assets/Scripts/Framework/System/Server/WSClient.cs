using Framework;
using SimpleJSON;
using System;
using UnityEngine;
using WebSocketSharp;

namespace Framework {
    public class WSClient : Singleton<WSClient>
    {
        public WebSocket ws;

        public void Connect()
        {
            ws = new WebSocket(ServerConfig.WebSocketURL + "?id="+ PDataAuth.AuthData?.userId + "&token=" + PDataAuth.AuthData?.token);
            //ws = new WebSocket(ServerConfig.WebSocketURL + "?id="+ "1" + "&token=" + "test");
            ws.OnOpen += OnOpen;
            ws.OnMessage += OnMessage;
            ws.OnError += OnError;
            ws.Connect();
            //DontDestroyOnLoad.DontDestroyOnLoad(GameObject.Instantiate(new GameObject("WSPingPong")).AddComponent(typeof(WSPingPong)));
        }
        public void Disconnect()
        {
            ws.OnOpen -= OnOpen;
            ws.OnMessage -= OnMessage;
            ws.OnError -= OnError;
            ws.Close();
            //GameObject.Destroy(WSPingPong.Instance.gameObject);
        }
        public void Ping()
        {
            ws.Send("{\"id\":2}");
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
            MainThreadDispatcher.ExecuteOnMainThread(() =>
            {
                Debug.Log("Error : " + e.Exception);
            });
        }

    }
}