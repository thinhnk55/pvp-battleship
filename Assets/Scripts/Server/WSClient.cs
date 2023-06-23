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
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_SHOP_CONFIG, ReceiveShopConfig);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_TRANSACTION, RecieveTransaction);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECEIVE_RANK_CONFIG, ReceiveRankConfig);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.LOGIN, OnLogin);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_ACHIEVEMENT, ReceiveAchievementConfig);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_LUCKY_SHOT_CONFIG, ReceiveLuckyShotConfig);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_SHOP_CONFIG, ReceiveShopConfig);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_TRANSACTION, RecieveTransaction);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECEIVE_RANK_CONFIG, ReceiveRankConfig);
    }
    public void OnLogin(JSONNode data)
    {
        PResourceType.GEM.SetValue(int.Parse(data["d"]));
        PResourceType.BERI.SetValue(int.Parse(data["b"]));
        // GameData.IsBuyDiamondFirst = int.Parse(data["_p"]);
        ProfileData profile = GameData.Player;
        GameData.Player = ProfileData.FromJson(ref profile, data);
        Debug.Log(GameData.Player.ToString());
        for (int i = 0; i < data["v"].Count; i++)
        {
            if (GameData.Versions[i] != int.Parse(data["v"][i]))
            {
                GameData.Versions[i] = int.Parse(data["v"][i]);
                switch ((ConfigVersion)i)
                {
                    case ConfigVersion.RANK:
                        RequestRankConfig();
                        break;
                    case ConfigVersion.ACHIEVEMENT:
                        RequestAchievementConfig();
                        break;
                    case ConfigVersion.LUCKY_SHOT:
                        RequestLuckyShotConfig();
                        break;
                    case ConfigVersion.GIFT:
                        RequestGiftConfig();
                        break;
                    case ConfigVersion.SHOP:
                        RequestShopConfig();
                        break;
                    case ConfigVersion.TRESURE:
                        break;
                    default:
                        break;
                }
            }
        }
        Timer<LuckyShot>.Instance.LastTime = long.Parse(data["lfb"]).NowFrom0001From1970();
        Timer<Gift>.Instance.LastTime = long.Parse(data["lcr"]).NowFrom0001From1970();
        Timer<RankCollection>.Instance.LastTime = long.Parse(data["WRC"]).NowFrom0001From1970();
        Debug.Log(Timer<RankCollection>.Instance.LastTime);
        CoreGame.timeInit = int.Parse(data["t"]);
        CoreGame.bets = data["bet"].ToList();
    }
    public static void RecieveTransaction(JSONNode data)
    {
        TransactionType id = data["itemId"].ToEnum<TransactionType>();
        int index = int.Parse(data["itemIndex"]);
        GameData.TransactionConfigs[id][index].Transact();
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
    public void ReceiveRankConfig(JSONNode data)
    {
        Debug.Log(data);
        GameData.RankConfigs = RankConfig.ListFromJson(data);
    }
    public void RequestRankConfig()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_RANK_CONFIG.ToJson() },
        };
        Instance.Send(jsonNode);
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
            if (GameData.TransactionConfigs.ContainsKey((TransactionType)i))
            {
                GameData.TransactionConfigs[(TransactionType)i] = TransactionInfo.ListFromJson(data["list"][i], i);
            }
            else
            {
                GameData.TransactionConfigs.Add((TransactionType)i, TransactionInfo.ListFromJson(data["list"][i], i));
            }

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

    public static void RequestReconnect()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_RECONNECT.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    public static void RequestGift()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_GIFT.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    public static void RequestGiftConfig()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_GIFT_CONFIG.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    public static void ReceiveGiftConfig(JSONNode json)
    {
        GameData.GiftConfig = new List<int>();
        for (int i = 0; i < json["list"].Count; i++)
        {
            GameData.GiftConfig.Add(int.Parse(json["list"][i]));
        }
    }
    public static void RequestRank()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_RANK.ToJson() },
        };
        Instance.Send(jsonNode);
    }
}
