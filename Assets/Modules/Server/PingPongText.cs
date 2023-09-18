using Framework;
using WebSocketSharp;

namespace Server
{
    public class PingPongText : TextBase
    {
        protected override void Awake()
        {
            base.Awake();
            WSClient.Instance.OnConnect += () =>
            {
                WSClient.Instance.ws.OnMessage += Pong;
            };
            WSClient.Instance.OnDisconnect += () =>
            {
                WSClient.Instance.ws.OnMessage -= Pong;
            };
        }

        void Pong(object sender, MessageEventArgs e)
        {
            MainThreadDispatcher.ExecuteOnMainThread(() =>
            {
                text.text = (WSPingPong.Instance.PingPongTime * 1000).ToString();
            });
        }
    }
}
