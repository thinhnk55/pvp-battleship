using Framework;
using SimpleJSON;
using System;
using UnityEngine;
using WebSocketSharp;

namespace Server
{
    public class WSClient : Singleton<WSClient>
    {
        public event Callback OnConnect;
        public event Callback OnDisconnect;
        public event Callback OnLostConnection;
        public event Callback OnSystemError;
        public event Callback OnTokenInvalid;
        public event Callback OnLoginInOtherDevice;
        public event Callback OnAdminKick;
        public WebSocket ws;


        public void Connect(int userId, string token)
        {
            ServerMessenger.AddListener<JSONNode>(ServerResponse.CheckLoginConnection, CheckLoginConnection);
            Messenger.AddListener(GameEvent.LostConnection, OnLostConnection);
            ws = new WebSocket(ServerConfig.WebSocketURL + "?id=" + userId + "&token=" + token);
            //ws = new WebSocket(ServerConfig.WebSocketURL + "?id="+ 12 + "&token=" + "7lnyeclvtjlk49en9b63dsx8e6q5tqyi");
            ws.OnOpen += OnOpen;
            ws.OnMessage += OnMessage;
            ws.OnError += OnError;
            ws.OnClose += OnClose;
            ws.Connect();
        }
        public void Disconnect(bool unlisten)
        {
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse.CheckLoginConnection, CheckLoginConnection);
            Messenger.RemoveListener(GameEvent.LostConnection, OnLostConnection);
            Debug.Log("Disconnect");
            if (unlisten)
            {
                OnDisconnect?.Invoke();
            }
            ws.OnOpen -= OnOpen;
            ws.OnMessage -= OnMessage;
            ws.OnError -= OnError;
            ws.OnClose -= OnClose;
            ws.Close();
        }

        private void OnClose(object sender, CloseEventArgs e)
        {
            MainThreadDispatcher.ExecuteOnMainThread(() =>
            {
                Debug.Log("Close " + ((WebSocket)sender).Url + " : " + e.Reason + ". Code " + e.Code);
                WSPingPong.Destroy();
            });
        }

        public void Send(JSONNode json)
        {
            try
            {
                ws.Send(json.ToString());
                Debug.Log($"<color=#FFA500>{json}</color>");
            }
            catch (Exception e)
            {
                Messenger.Broadcast(GameEvent.LostConnection);
                Instance.Disconnect(true);
                throw e;
            }
        }
        public void OnOpen(object sender, EventArgs e)
        {
            Debug.Log("Open " + ((WebSocket)sender).Url);
            WSPingPong.Create();
        }
        public void OnMessage(object sender, MessageEventArgs e)
        {
            MainThreadDispatcher.ExecuteOnMainThread(() =>
            {
                Debug.Log($"<color=yellow>{e.Data}</color>");
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
        /// <summary>
        /// Handle the login connection status
        /// </summary>
        /// <param name="data">
        /// 0: login successfully
        /// 1: system error
        /// 2: token invalid
        /// 3: login in other device
        /// 4: admin kick
        /// </param>
        public void CheckLoginConnection(JSONNode data)
        {
            switch (data["e"].AsInt)
            {
                case 0:
                    OnConnect?.Invoke();
                    break;
                case 1:
                    OnSystemError?.Invoke();
                    Disconnect(true);
                    break;
                case 2:
                    ws.Close();
                    OnTokenInvalid?.Invoke();
                    Disconnect(false);
                    break;
                case 3:
                    OnLoginInOtherDevice?.Invoke();
                    Disconnect(true);
                    break;
                case 4:
                    OnAdminKick?.Invoke();
                    Disconnect(true);
                    break;
                default:
                    break;
            }
        }
    }
}