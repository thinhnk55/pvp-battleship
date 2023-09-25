using Framework;
using SimpleJSON;

namespace Server
{
    public class PingPongText : TextBase
    {
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
        }
    }
}
