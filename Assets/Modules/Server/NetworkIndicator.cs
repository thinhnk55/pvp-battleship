using Framework;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Server
{
    public class NetworkIndicator : TextBase
    {
        [SerializeField] Sprite[] wifiIcon;
        [SerializeField] Sprite[] internetIcon;
        [SerializeField] Image networkIcon;
        [SerializeField] TextMeshProUGUI scenes;

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
            int ping = (int)(WSPingPong.Instance.PingPongTime * 1000);
            text.text = ping.ToString() + "ms";
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
            string n = "";
            foreach (var scene in SceneManager.GetAllScenes())
            {
                n += scene.name;
            }
            scenes.text = n;
        }
    }
}
