using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

public class ServeData
{
    public static bool IsTutorialComplete;
    public static List<BoardInfo> ListBoard;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        IsTutorialComplete = false;
        ListBoard = new List<BoardInfo>() { new BoardInfo() { boardInfo = new List<ShipInfo>()}, new BoardInfo() { boardInfo = new List<ShipInfo>() }, new BoardInfo() { boardInfo = new List<ShipInfo>() },
            new BoardInfo() { boardInfo = new List<ShipInfo>()} , new BoardInfo() { boardInfo = new List<ShipInfo>()} , new BoardInfo() { boardInfo = new List<ShipInfo>()} };
    } 

    public static JSONClass ConvertToJson()
    {
        return new JSONClass
        {
            "t", IsTutorialComplete.ToString(),
            "l", ConvertDataOfListBoardToJson()
        };
    }

    public static JSONNode ConvertDataOfListBoardToJson()
    {
        JSONNode json = new JSONClass();
        JSONArray listBoardInfo = new JSONArray();
        for(int i = 0; i<ListBoard.Count; i++)
        {
            JSONArray listShipInfo = new JSONArray();
            for(int j = 0; j < ListBoard[i].boardInfo.Count; j++)
            {
                JSONNode shipInfoJson= new JSONClass();
                ShipInfo shipInfo = ListBoard[i].boardInfo[j];
                shipInfoJson.Add("x", ((short)shipInfo.x).ToJson());
                shipInfoJson.Add("y", ((short)shipInfo.y).ToJson());
                shipInfoJson.Add("t", ((short)shipInfo.type).ToJson());
                shipInfoJson.Add("d", ((short)shipInfo.dir).ToJson());

                listShipInfo.Add(shipInfoJson);
            }
            listBoardInfo.Add(listShipInfo);
        }
        json.Add("b", listBoardInfo);
        return listBoardInfo;
    }


}
