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
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_TREASURE_CONFIG, ReceiveTreasureConfig);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECEIVE_COUNTDOWN_CONFIG, ReceiveCountDownConfig);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_ROYAL_CONFIG, ReceiveRoyalPassConfig);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_JOIN_TREASURE_ROOM, ReceiveJoinTreasureRoom);


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
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_TREASURE_CONFIG, ReceiveTreasureConfig);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECEIVE_COUNTDOWN_CONFIG, ReceiveCountDownConfig);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_ROYAL_CONFIG, ReceiveRoyalPassConfig);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_JOIN_TREASURE_ROOM, ReceiveJoinTreasureRoom);
    }
    public void OnLogin(JSONNode data)
    {
        PConsumableType.GEM.SetValue(int.Parse(data["profile"]["d"]));
        PConsumableType.BERI.SetValue(int.Parse(data["profile"]["b"]));
        PNonConsumableType.AVATAR.FromJson(data["statistics"]["a_a"]);
        PNonConsumableType.AVATAR_FRAME.FromJson(data["statistics"]["a_f"]);
        PNonConsumableType.BATTLE_FIELD.FromJson(data["statistics"]["bfA"]);
        PNonConsumableType.SKIN_SHIP.FromJson(data["statistics"]["ssA"]);
        GameData.Player = ProfileData.FromJson(GameData.Player, data);
        Debug.Log(GameData.Player.ToString());
        Timer<LuckyShot>.Instance.LastTime = long.Parse(data["timer"]["lfb"]).NowFrom0001From1970();
        Timer<Gift>.Instance.LastTime = long.Parse(data["timer"]["lcr"]).NowFrom0001From1970();
        Timer<RankCollection>.Instance.LastTime = long.Parse(data["timer"]["WRC"]).NowFrom0001From1970();
        GameData.ProgressGift = int.Parse(data["timer"]["cr"]);
        CoreGame.timeInit = int.Parse(data["t"]);
        CoreGame.bets = data["bet"].ToList();
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
                    case ConfigVersion.TREASURE:
                        RequestTreasureConfig();
                        break;
                    case ConfigVersion.COUNT_DOWN:
                        RequestCountDownConfig();
                        break;
                    case ConfigVersion.ROYAL_PASS:
                        RequestRoyalPassConfig();
                        break;
                    default:
                        break;
                }
            }
        }
        RequestTreasureConfig();
        SceneTransitionHelper.Load(ESceneName.Home);
        Timer<LuckyShot>.Instance.TriggerIntervalInSecond = GameData.LuckyShotCoolDown;
        Timer<Gift>.Instance.TriggerIntervalInSecond = GameData.GiftCoolDown;
        Timer<RankCollection>.Instance.TriggerIntervalInSecond = GameData.RankReceiveCoolDown;
    }
    public void LoadHomeScene()
    {

    }
    private void RequestRoyalPassConfig()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_COUNTDOWN_CONFIG.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    private void ReceiveRoyalPassConfig(JSONNode data)
    {
        SceneTransitionHelper.Load(ESceneName.Home);
    }
    private void RequestCountDownConfig()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_ROYAL_CONFIG.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    private void ReceiveCountDownConfig(JSONNode data)
    {
        GameData.LuckyShotCoolDown = int.Parse(data["lucky_shot"]) / 1000;
        GameData.RankReceiveCoolDown = int.Parse(data["rank_receive"]) / 1000;
        GameData.GiftCoolDown = int.Parse(data["consolation_gift"]) / 1000;
        Timer<LuckyShot>.Instance.TriggerIntervalInSecond = int.Parse(data["lucky_shot"]) / 1000;
        Timer<Gift>.Instance.TriggerIntervalInSecond = int.Parse(data["consolation_gift"]) / 1000;
        Timer<RankCollection> .Instance.TriggerIntervalInSecond = int.Parse(data["rank_receive"]) / 1000;
        SceneTransitionHelper.Load(ESceneName.Home);
    }
    public static void RecieveTransaction(JSONNode data)
    {
        TransactionType id = data["itemId"].ToEnum<TransactionType>();
        int index = int.Parse(data["itemIndex"]);
        GameData.TransactionConfigs[id][index].Transact();
    }
    public void ReceiveAchievementConfig(JSONNode data)
    {
        GameData.AchievementConfig = new Dictionary<AchievementType, AchievementInfo>();
        for (int i = 0; i < data["achie"].Count; i++)
        {
            GameData.AchievementConfig.Add((AchievementType)i, AchievementInfo.FromJson(data["achie"][i], i));
        }
        SceneTransitionHelper.Load(ESceneName.Home);
    }
    public void ReceiveLuckyShotConfig(JSONNode data)
    {
        List<int> luckyShots = data["list"].ToList();
        GameData.LuckyShotConfig = luckyShots;
        GameData.LuckyShotConfig.Log();
        SceneTransitionHelper.Load(ESceneName.Home);
    }
    public void ReceiveRankConfig(JSONNode data)
    {
        GameData.RankConfigs = RankConfig.ListFromJson(data);
        SceneTransitionHelper.Load(ESceneName.Home);
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
        for (int i = 0; i < data["list"].Count; i++)
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
        SceneTransitionHelper.Load(ESceneName.Home);
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

    private static bool waitingJoinTreasureRoom = false;

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
        for(int i=0; i<data["list"].Count; i++)
        {
            TreasureConfig treasureConfig = new TreasureConfig()
            {
                Id = int.Parse(data["list"][i]["id"]),
                PrizeAmount = int.Parse(data["list"][i]["re"]),
                InitPrize = int.Parse(data["list"][i]["init"]),
            };
            GameData.TreasureConfigs.Add(treasureConfig);
        }
        SceneTransitionHelper.Load(ESceneName.Home);
    }

    public static void RequestJoinTreasureRoom(int rom)
    {
        if (waitingJoinTreasureRoom) return;
        Debug.Log("Request:");
        JSONNode jsonNode = new JSONClass()
        {
            {"id", GameServerEvent.REQUEST_JOIN_TREASURE_ROOM.ToJson() },
            {"b" , rom.ToJson() },
        };
        Instance.Send(jsonNode);
        GameData.JoinTreasureRoom.RoomId = rom;
        foreach (var r in GameData.TreasureConfigs)
        {
            if (r.Id == rom)
            {
                GameData.JoinTreasureRoom.ShotCost = r.PrizeAmount;
                GameData.JoinTreasureRoom.InitPrize = r.InitPrize;
                break;
            }
        }
        waitingJoinTreasureRoom = true;
    }

    public static void ReceiveJoinTreasureRoom(JSONNode data)
    {
        //Debug.Log("Receive :"+ data);
        //GameData.JoinTreasureRoom.Id = int.Parse(data["id"]);
        //GameData.JoinTreasureRoom.IsSuccess = int.Parse(data["s"]);
        //GameData.JoinTreasureRoom.CurrentPrize = int.Parse(data["beri"]);
        //GameData.JoinTreasureRoom.Board = new List<List<int>>();

        //for (int row=0; row<10; row++)
        //{
        //    List<int> rowList = new List<int>();
        //    for(int col=0; col<10; col++)
        //    {
        //        rowList.Add(int.Parse(data["board"][col][row]));
        //    }
        //    GameData.JoinTreasureRoom.Board.Add(rowList);
        //}
        waitingJoinTreasureRoom = false;
    }

    public static void RequestShootTreasure(int x, int y)
    {
        JSONNode jsonNode = new JSONClass()
        {
            {"id", GameServerEvent.REQUEST_SHOOT_TREASURE.ToJson() },
            {"b" , GameData.JoinTreasureRoom.RoomId.ToJson() },
            {"x" , y.ToJson() },
            {"y" , x.ToJson() },
        };
        Instance.Send(jsonNode);
    }

    public static void RequestExitTreasureRoom()
    {
        JSONNode jsonNode = new JSONClass()
        {
            {"id", GameServerEvent.REQUEST_EXIT_TREASURE_ROOM.ToJson() },
        };
        Instance.Send(jsonNode);
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
    public static void RequestChangeAchievement(int[] indexs)
    {
        JSONArray array = new JSONArray();
        for (int i = 0; i < indexs.Length; i++)
        {
            array.Add(indexs[i].ToString());
        }
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_ACHIEVEMENT_CHANGE.ToJson() },
            { "outst", array },
        };
        Instance.Send(jsonNode);
    }

    public static void RequestChangeName(string name)
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_CHANGE_NAME.ToJson() },
            { "n", name.ToString()}
        };
        Instance.Send(jsonNode);
    }

    public static void RequestChangeAvatar(int i)
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_CHANGE_AVATAR.ToJson() },
            { "a", i.ToString()}
        };
        Instance.Send(jsonNode);
    }
    public static void RequestChangeFrame(int i)
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_CHANGE_FRAME.ToJson() },
            { "f", i.ToString()}
        };
        Instance.Send(jsonNode);
    }
    public static void RequestChangeBattleField(int i)
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_CHANGE_BATTLEFIELD.ToJson() },
            { "a", i.ToString()}
        };
        Instance.Send(jsonNode);
    }
    public static void RequestChangeSkinShip(int i)
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_CHANGE_SKIN_SHIP.ToJson() },
            { "f", i.ToString()}
        };
        Instance.Send(jsonNode);
    }
}
