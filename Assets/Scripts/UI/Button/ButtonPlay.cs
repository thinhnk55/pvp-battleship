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
    public void Play()
    {
        WSClient.RequestReconnect();
        SceneTransitionHelper.Load(ESceneName.Bet);
    }

    public void RecieveReconnect(JSONNode data)
    {
        if (data["r"] != null)
        {
            SceneTransitionHelper.Load(ESceneName.MainGame);
            CoreGame.bet = int.Parse(data["bet"]);
            CoreGame.Instance.roomId = int.Parse(data["bet"]);
            CoreGame.Instance.playerChair = int.Parse(data["c"]);
            CoreGame.Instance.player.ships = new List<Ship> { };
            for (int i = 0; i < data["ship"].Count; i++)
            {
                Ship ship = new Ship();
                ship.FromJson(data["ship"][i]);
                //CoreGame.Instance.player.AssignShip(ship);

            }
            CoreGame.Instance.stateMachine.CurrentState = GameState.Turn;
        }
        else
        {
            SceneTransitionHelper.Load(ESceneName.Bet);
        }
    }
}
