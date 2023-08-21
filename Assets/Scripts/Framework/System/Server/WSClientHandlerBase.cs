using SimpleJSON;
using System;
using UnityEngine;

namespace Framework
{
    public abstract class WSClientHandlerBase
    {
        protected delegate int CheckLoginStatus(JSONNode data);
        protected CheckLoginStatus checkLoginStatus;
        public virtual void Connect()
        {
            ServerMessenger.AddListener<JSONNode>(ServerResponse.CheckLoginConnection, CheckLoginConnection);
            ServerMessenger.AddListener(ServerResponse.LostConnection, OnLostConnection);
            WSClient.Instance.Connect();
        }
        public virtual void Disconnect()
        {
            WSClient.Instance.Disconnect();
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse.CheckLoginConnection, CheckLoginConnection);
            ServerMessenger.RemoveListener(ServerResponse.LostConnection, OnLostConnection);
            OnDisconnect();
        }
        public abstract void OnConnect();
        public abstract void OnDisconnect();
        public abstract void OnLostConnection();
        public abstract void OnSystemError();
        public abstract void OnTokenInvalid();
        public abstract void OnLoginInOtherDevice();
        public abstract void OnAdminKick();

        /// <summary>
        /// Handle the login connection status
        /// </summary>
        /// <param name="data"></param>
        /// <returns> 
        /// 0: login successfully
        /// 1: system error
        /// 2: token invalid
        /// 3: login in other device
        /// 4: admin kick
        /// </returns>
        public void CheckLoginConnection(JSONNode data)
        {
            switch (data["e"].AsInt)
            {
                case 0: 
                    OnConnect();
                    break;
                case 1:
                    Disconnect();
                    OnSystemError();
                    break;
                case 2:
                    Disconnect();
                    OnTokenInvalid();
                    break;
                case 3:
                    Disconnect();
                    OnLoginInOtherDevice();
                    break;
                case 4:
                    Disconnect();
                    OnAdminKick();
                    break;
                default:
                    break;
            }
        }
    }
}