using Framework;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ServerMessengerFake.AddListener<JSONNode>(ServerRequest._SUBMIT_SHIP, RequestSubmitShip);
        ServerMessengerFake.AddListener<JSONNode>(ServerRequest._ATTACK, EndTurn);
        Match();
    }


    #region ServerRequest
    private void RequestSubmitShip(JSONNode json)
    {

        JSONNode startGameJson = JSONNode.Parse(GameConfig.StartJsonTuto);
        ServerMessenger.Broadcast<JSONNode>(ServerResponse._GAME_START, startGameJson);
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

    }
    #endregion

}
