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
            Debug.Log("Connect");
            ws = new WebSocket(ServerConfig.WebSocketURL + "?id=" + userId + "&token=" + token);
            ws.OnOpen += OnOpen;
            //ws = new WebSocket(ServerConfig.WebSocketURL + "?id="+ 12 + "&token=" + "7lnyeclvtjlk49en9b63dsx8e6q5tqyi");
            ws.Connect();
            if (ws.IsAlive)
            {
                ServerMessenger.AddListener<JSONNode>(ServerResponse.CheckLoginConnection, CheckLoginConnection);
                Messenger.AddListener(GameEvent.LostConnection, OnLostConnection);
            }
        }
        public void Disconnect(bool unlisten)
        {
            if (unlisten)
            {
                Debug.Log("Unlisten");
                OnDisconnect?.Invoke();
            }
            ws.Close();
        }
        public void OnOpen(object sender, EventArgs e)
        {
            Debug.Log("Open " + ((WebSocket)sender).Url);
            ws.OnMessage += OnMessage;
            ws.OnError += OnError;
            ws.OnClose += OnClose;
            WSPingPong.Create();
        }
        private void OnClose(object sender, CloseEventArgs e)
        {
            MainThreadDispatcher.ExecuteOnMainThread(() =>
            {
                Debug.Log("Close " + ((WebSocket)sender).Url + " : " + e.Reason + " - " + e.Code);
                if (e.Code != 1005 && e.Code != 1000)
                {
                    Messenger.Broadcast(GameEvent.LostConnection);
                    OnDisconnect?.Invoke();
                    Debug.Log("Network shutdown unintentionally");
                }
                else
                {
                    Debug.Log("Close websocket connection manually");
                }
                ServerMessenger.RemoveListener<JSONNode>(ServerResponse.CheckLoginConnection, CheckLoginConnection);
                Messenger.RemoveListener(GameEvent.LostConnection, OnLostConnection);
                WSPingPong.Destroy();
                ws.OnOpen -= OnOpen;
                ws.OnMessage -= OnMessage;
                ws.OnError -= OnError;
                ws.OnClose -= OnClose;
            });
        }

        public void Send(JSONNode json)
        {
            try
            {
                if (ws != null)
                {
                    if (json != null)
                    {
                        ws.Send(json.ToString());
                        Debug.Log($"<color=#FFA500>{(ServerRequest)json["id"].AsInt} - {json}</color>");
                    }
                    else
                    {
                        Debug.LogError("Json null");
                    }
                }
                else
                {
                    Messenger.Broadcast(GameEvent.LostConnection);
                    Instance.Disconnect(true);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

        }

        public void OnMessage(object sender, MessageEventArgs e)
        {
            MainThreadDispatcher.ExecuteOnMainThread(() =>
            {
                JSONNode idJson = JSON.Parse(e.Data)["id"];
                if (idJson != null)
                {
                    ServerResponse id = (ServerResponse)int.Parse(idJson);
                    if (ServerMessenger.eventTable.ContainsKey(id))
                    {
                        Debug.Log($"<color=yellow>{id} - {e.Data}</color>");
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