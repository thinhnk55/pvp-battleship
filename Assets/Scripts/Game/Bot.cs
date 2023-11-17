using DG.Tweening;
using Framework;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    private int CountShotPlayer = 0;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        ServerMessengerFake.AddListener<JSONNode>(ServerRequest._SUBMIT_SHIP, RequestSubmitShip);
        ServerMessengerFake.AddListener<JSONNode>(ServerRequest._ATTACK, EndTurn);
        yield return new WaitForSeconds(3f);
        Match();
    }


    #region ServerRequest
    private void RequestSubmitShip(JSONNode json)
    {
        DOVirtual.DelayedCall(2f, () =>
        {
            JSONNode startGameJson = JSONNode.Parse(GameConfig.StartJsonTuto);
            ServerMessenger.Broadcast<JSONNode>(ServerResponse._GAME_START, startGameJson);
        });
    }

    #endregion

    #region ServerRespone
    private void Match()
    {
        JSONNode json = JSONNode.Parse(GameConfig.MatchJsonTuto);
        ServerMessenger.Broadcast<JSONNode>(ServerResponse._MATCH, json);
    }

    private void EndTurn(JSONNode json)
    {
        if(CoreGame.Instance.playerTurn == true)
        {
            ServerMessenger.Broadcast<JSONNode>(ServerResponse._END_TURN, JSONNode.Parse(GameConfig.ListEndTurnJsonTuto[CountShotPlayer++]));
            if(CountShotPlayer == 4) 
            {
                DOVirtual.DelayedCall(4, () =>
                {
                    ServerMessenger.Broadcast<JSONNode>(ServerResponse._END_TURN, JSONNode.Parse(GameConfig.ListEndTurnJsonTuto[CountShotPlayer++]));
                });
            }
        }
        else
        {
            int x = json["x"].AsInt;
            int y = json["y"].AsInt;

            JSONNode d = new JSONClass();
            d.Add("c", 1.ToJson());
            d.Add("x", json["x"]);
            d.Add("y", json["y"]);
            d.Add("r", 1.ToJson());
            JSONNode jsonEndTurn = new JSONClass()
            {
                {"id", ServerResponse._END_TURN.ToJson() },
                {"e", 0.ToJson() },
                {"d", d },
            };
            Debug.Log(jsonEndTurn);
            ServerMessenger.Broadcast<JSONNode>(ServerResponse._END_TURN, jsonEndTurn);
        }
    }
    #endregion

}
