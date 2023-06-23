using Framework;
using GooglePlayGames.BasicApi;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class WSClient : WSClientBase
{
    protected override void Start()
    {
        base.Start();
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.LOGIN, OnLogin);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_ACHIEVEMENT, ReceiveAchievementConfig);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_LUCKY_SHOT_CONFIG, ReceiveLuckyShotConfig);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_SHOP_CONFIG, ReceiveLuckyShotConfig);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_TREASURE_CONFIG, ReceiveTreasureConfig);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_JOIN_TREASURE_ROOM, ReceiveJoinTreasureRoom);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.LOGIN, OnLogin);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_ACHIEVEMENT, ReceiveAchievementConfig);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_LUCKY_SHOT_CONFIG, ReceiveLuckyShotConfig);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_SHOP_CONFIG, ReceiveLuckyShotConfig);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_TREASURE_CONFIG, ReceiveTreasureConfig);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_JOIN_TREASURE_ROOM, ReceiveJoinTreasureRoom);
    }
    public void OnLogin(JSONNode data)
    {
        PResourceType.GEM.SetValue(int.Parse(data["d"]));
        PResourceType.BERI.SetValue(int.Parse(data["b"]));
        // GameData.IsBuyDiamondFirst = int.Parse(data["_p"]);
        ProfileData profile = GameData.Player;
        GameData.Player = ProfileData.FromJson(ref profile, data);
        Debug.Log(GameData.Player.ToString());
       /* if (GameData.Version != int.Parse(data["v"]) || GameData.Player.AchievementConfig == null || GameData.Player.AchievementConfig.Count==0)*/
        {
            Debug.Log("RequestConfig");
        //  GameData.Version = int.Parse(data["v"]);
            RequestAchievementConfig();
            RequestLuckyShotConfig();
        }
        RequestShopConfig();
        RequestTreasureConfig();

        CoreGame.timeInit = int.Parse(data["t"]);
        CoreGame.bets = data["bet"].ToList();
    }
    public void ReceiveAchievementConfig(JSONNode data)
    {
        var achie = GameData.Player;
        achie.AchievementConfig = new Dictionary<AchievementType, AchievementInfo>();
        GameData.Player = achie;
        for (int i = 0; i < data["achie"].Count; i++)
        {
            GameData.Player.AchievementConfig.Add((AchievementType)i, AchievementInfo.FromJson(data["achie"][i], i));
        }
    }
    public void ReceiveLuckyShotConfig(JSONNode data)
    {
        List<int> luckyShots = data["list"].ToList();
        GameData.LuckyShotConfig = luckyShots;
        GameData.LuckyShotConfig.Log();
    }
    public static void RequestLuckyShotConfig()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_LUCKY_SHOT_CONFIG.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    public static void RequestAchievementConfig()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_ACHIEVEMENT.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    public static void RequestShopConfig()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_SHOP_CONFIG.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    public void ReceiveShopConfig(JSONNode data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            GameData.TransactionConfigs[(TransactionType)i].AddRange(TransactionInfo.ListFromJson(data[i]));

        }
    }
    public static void RequestShot()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_LUCKY_SHOT.ToJson() },
        };
        Instance.Send(jsonNode);
    }

    #region TREASUREHUNT

    public static void RequestTreasureConfig()
    {
        JSONNode jsonNode = new JSONClass()
        {
            {"id", GameServerEvent.REQUEST_TREASURE_CONFIG.ToJson() },
        };
        Instance.Send(jsonNode);
    }

    public static void ReceiveTreasureConfig(JSONNode data)
    {
        GameData.TreasureConfigs.Clear();
        for(int i=0; i<data.Count; i++)
        {
            TreasureConfig treasureConfig = new TreasureConfig()
            {
                Id = int.Parse(data["list"][i]["id"]),
                PrizeAmount = int.Parse(data["list"][i]["re"]),
            };
            GameData.TreasureConfigs.Add(treasureConfig);
        }
    }

    public static void RequestJoinTreasureRoom(int rom)
    {
        Debug.Log("Request:");
        JSONNode jsonNode = new JSONClass()
        {
            {"id", GameServerEvent.REQUEST_JOIN_TREASURE_ROOM.ToJson() },
            {"b" , rom.ToJson() },
        };
        Instance.Send(jsonNode);
    }

    public static void ReceiveJoinTreasureRoom(JSONNode data)
    {
        Debug.Log("Receive :"+ data);
        GameData.JoinTreasureRoom.Id = int.Parse(data["id"]);
        GameData.JoinTreasureRoom.IsSuccess = int.Parse(data["s"]);
        GameData.JoinTreasureRoom.CurrentPrize = int.Parse(data["beri"]);

        for(int row=0; row<10; row++)
        {
            List<int> rowList = new List<int>();
            for(int col=0; col<10; col++)
            {
                rowList.Add(int.Parse(data["board"][col][row]));
            }
            GameData.JoinTreasureRoom.Board.Add(rowList);
        }
    }

    #endregion

    public static void SearchOpponent(int bet, List<Ship> ships)
    {
        JSONNode jsonNode = new JSONClass();
        JSONArray jsonArray = new JSONArray();
        foreach (Ship ship in ships)
        {
            jsonArray.Add(ship.ToJson());
        }
        jsonNode.Add("id", GameServerEvent.SEARCH_OPPONENT.ToJson() );
        jsonNode.Add("b", bet.ToJson());
        jsonNode.Add("ship", jsonArray);
        Instance.Send(jsonNode);
    }

    public static void AttackOpponent(int room, int x, int y)
    {
        JSONNode jsonNode = new JSONClass
        {
            { "id", GameServerEvent.ATTACK.ToJson() },
            { "r",  room.ToJson() },
            { "x",  x.ToJson() },
            { "y",  y.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    public static void QuitSearch(int bet)
    {
        JSONNode jsonNode = new JSONClass
        {
            { "id", GameServerEvent.QUIT_SEARCH.ToJson() },
            { "r", bet.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    public static void QuitGame(int room)
    {
        JSONNode jsonNode = new JSONClass
        {
            { "id", GameServerEvent.QUIT_GAME_REQUEST.ToJson() },
            { "r", room.ToJson() },
        };
        Instance.Send(jsonNode);
    }

    public static void RequestObtainAchievemnt(int id, int obtained)
    {
        JSONNode jsonNode = new JSONClass
        {
            { "id", GameServerEvent.REQUEST_OBTAIN_ACHIEVEMENT.ToJson() },
            { "achieId", id.ToJson() },
            { "achieIndex", (obtained + 1).ToJson() }
        };
        Instance.Send(jsonNode);
    }
    public static void RequesRematch()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_LUCKY_SHOT.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    public static void Reconnect(JSONNode data)
    {
        
    }
}
