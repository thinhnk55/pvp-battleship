using Framework;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPlay : MonoBehaviour
{
    private void Awake()
    {
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_RECONNECT, RecieveReconnect);

    }
    private void OnDestroy()
    {
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_RECONNECT, RecieveReconnect);
    }
    public void Play()
    {
        WSClient.RequestReconnect();
    }
    public void RecieveReconnect(JSONNode data)
    {
        if (data["r"] != null)
        {
            CoreGame.reconnect = data;
            SceneTransitionHelper.Load(ESceneName.MainGame);
        }
        else
        {
            SceneTransitionHelper.Load(ESceneName.Bet);
        }
    }
}
