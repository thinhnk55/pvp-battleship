using Framework;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSClientHandleFake : Framework.Singleton<WSClientHandler>
{
    public static void RequestSubmitShip(int bet, List<Ship> ships)
    {
        JSONNode jsonNode = new JSONClass();
        JSONArray jsonArray = new JSONArray();
        foreach (Ship ship in ships)
        {
            jsonArray.Add(ship.ToJson());
        }
        jsonNode.Add("id", ServerRequest._SUBMIT_SHIP.ToJson());
        jsonNode.Add("r", bet.ToJson());
        jsonNode.Add("s", jsonArray);
        Debug.Log(jsonNode);
        ServerMessengerFake.Broadcast(ServerRequest._SUBMIT_SHIP, jsonNode);
        Debug.Log(jsonNode);
    }

    public static void AttackOpponent()
    {
        ServerMessengerFake.Broadcast<JSONNode>(ServerRequest._ATTACK, GameConfig.ListShotPlayerJsonTuto[0]);
    }
}
