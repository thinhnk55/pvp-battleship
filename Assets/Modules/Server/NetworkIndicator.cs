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
            text.text = (WSPingPong.Instance.PingPongTime * 1000).ToString();
            Sprite[] images = Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ? wifiIcon : internetIcon;
            if (WSPingPong.Instance.PingPongTime > 0 && WSPingPong.Instance.PingPongTime < 150)
            {
                networkIcon.sprite = images[0];
            }
            else if (WSPingPong.Instance.PingPongTime > 150 && WSPingPong.Instance.PingPongTime < 300)
            {
                networkIcon.sprite = images[1];
            }
            else
            {
                networkIcon.sprite = images[2];
            }
        }
    }
}
