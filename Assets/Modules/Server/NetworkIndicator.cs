using Framework;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

namespace Server
{
    public class NetworkIndicator : TextBase
    {
        [SerializeField] Sprite[] wifiIcon;
        [SerializeField] Sprite[] internetIcon;
        [SerializeField] Image networkIcon;

        protected override void Awake()
        {
            base.Awake();
            WSClient.Instance.OnConnect += () =>
            {
                ServerMessenger.AddListener<JSONNode>(ServerResponse.Pong, Pong);
            };
            WSClient.Instance.OnDisconnect += () =>
            {
                ServerMessenger.RemoveListener<JSONNode>(ServerResponse.Pong, Pong);
            };
        }
        void Pong(JSONNode data)
        {
            float ping = WSPingPong.Instance.PingPongTime * 1000;
            text.text = ping.ToString();
            Sprite[] images = Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ? wifiIcon : internetIcon;
            if (ping > 0 && ping < 150)
            {
                networkIcon.sprite = images[0];
            }
            else if (ping > 150 && ping < 300)
            {
                networkIcon.sprite = images[1];
            }
            else if (ping > 300)
            {
                networkIcon.sprite = images[2];
            }
        }
    }
}
