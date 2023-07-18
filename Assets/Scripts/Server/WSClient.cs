using Framework;
using Monetization;
using SimpleJSON;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
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
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_BET_CONFIG, ReceiveBetConfig);


        //not config
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_RECEIVE_ROYALPASS, ReceiveReceiveRoyalPass);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_RECEIVE_ROYALPASS_QUEST, ReceiveQuest);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_CHANGE_QUEST, ReceiveChangeQuest);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_RECEIVE_ROYALPASS_SEASON_QUEST, ReceiveSeasonQuest);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_CLAIM_ALL_ROYALPASS, ReceiveClaimAllRoyalPass);

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
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_BET_CONFIG, ReceiveBetConfig);

        //not config
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_RECEIVE_ROYALPASS, ReceiveReceiveRoyalPass);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_RECEIVE_ROYALPASS_QUEST, ReceiveQuest);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_CHANGE_QUEST, ReceiveChangeQuest);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_RECEIVE_ROYALPASS_SEASON_QUEST, ReceiveSeasonQuest);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_CLAIM_ALL_ROYALPASS, ReceiveClaimAllRoyalPass);
    }
    public void OnLogin(JSONNode data)
    {
        MusicType.MAINMENU.PlayMusic();
        PConsumableType.GEM.SetValue(int.Parse(data["profile"]["d"]));
        PConsumableType.BERI.SetValue(int.Parse(data["profile"]["b"]));
        PNonConsumableType.AVATAR.FromJson(data["statistics"]["a_a"]);
        PNonConsumableType.AVATAR_FRAME.FromJson(data["statistics"]["a_f"]);
        PNonConsumableType.BATTLE_FIELD.FromJson(data["statistics"]["bfA"]);
        PNonConsumableType.SKIN_SHIP.FromJson(data["statistics"]["ssA"]);
        GameData.Player = ProfileData.FromJson(GameData.Player, data);
        RoyalPass.DataFromJson(GameData.RoyalPass, data["royalPass"]);
        Timer<LuckyShot>.Instance.BeginPoint = long.Parse(data["timer"]["lfb"]).NowFrom0001From1970();
        Timer<Gift>.Instance.BeginPoint = long.Parse(data["timer"]["lcr"]).NowFrom0001From1970();
        Timer<RankCollection>.Instance.BeginPoint = long.Parse(data["timer"]["WRC"]).NowFrom0001From1970();
        Timer<RoyalPass>.Instance.BeginPoint = GameData.RoyalPass.End;
        Timer<QuestCard>.Instance.BeginPoint = 300000000; //long.Parse(data["timer"]["as"]).NowFrom0001From1970();
        GameData.ProgressGift = int.Parse(data["timer"]["cr"]);
        CoreGame.timeInit = int.Parse(data["t"]);
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
                    case ConfigVersion.BET:
                        RequestBetConfig();
                        break;
                    default:
                        break;
                }
            }
        }
        SceneTransitionHelper.Load(ESceneName.Home);
        Timer<LuckyShot>.Instance.TriggerInterval_Sec = GameData.LuckyShotCoolDown;
        Timer<Gift>.Instance.TriggerInterval_Sec = GameData.GiftCoolDown;
        Timer<RankCollection>.Instance.TriggerInterval_Sec = GameData.RankReceiveCoolDown;
        AdsManager.SetUserId(PDataAuth.AuthData.userId.ToString());
    }
    #region Config
    private void RequestBetConfig()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_BET_CONFIG.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    private void ReceiveBetConfig(JSONNode json)
    {
        GameData.Bets = json["bet"].ToList().ToArray();
        GameData.BetRequires = json["require"].ToList().ToArray();
        GameData.BetRankPoint = json["point"].ToList().ToArray();
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
        GameData.RoyalPass = RoyalPass.ConfigFromJson(GameData.RoyalPass, data);
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
        GameData.ChangeQuestCoolDown = 60;//int.Parse(data["consolation_gift"]) / 1000;
        Timer<LuckyShot>.Instance.TriggerInterval_Sec = int.Parse(data["lucky_shot"]) / 1000;
        Timer<Gift>.Instance.TriggerInterval_Sec = int.Parse(data["consolation_gift"]) / 1000;
        Timer<RankCollection>.Instance.TriggerInterval_Sec = int.Parse(data["rank_receive"]) / 1000;
        Timer<QuestCard>.Instance.TriggerInterval_Sec = 60; //int.Parse(data["quest"]) / 1000;
        SceneTransitionHelper.Load(ESceneName.Home);
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
    #endregion
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
    #region CoreGame
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
    #endregion
    #region Other Feature
    public static void RecieveTransaction(JSONNode data)
    {
        TransactionType id = data["itemId"].ToEnum<TransactionType>();
        int index = int.Parse(data["itemIndex"]);
        GameData.TransactionConfigs[id][index].Transact();
        Messenger.Broadcast(GameEvent.TRANSACTION,id, index);
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
    
    public static void RequestGift()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_GIFT.ToJson() },
        };
        Instance.Send(jsonNode);
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
        JSONNode node = new JSONClass();
        for (int i = 0; i < indexs.Length; i++)
        {
            array.Add(new JSONData(indexs[i]));
        }
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_ACHIEVEMENT_CHANGE.ToJson() },
            { "outst", array },
        };
        Instance.Send(jsonNode);
    }
    public static void RequestShot()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_LUCKY_SHOT.ToJson() },
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

    public static void RequestQuest(int index)
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_RECEIVE_ROYALPASS_QUEST.ToJson() },
            { "index", new JSONData(index)}
        };
        Instance.Send(jsonNode);
    }
    public static void ReceiveQuest(JSONNode json)
    {
        int[] receive = new int[3] { GameData.RoyalPass.CurrentQuests.Data[0], GameData.RoyalPass.CurrentQuests.Data[1], GameData.RoyalPass.CurrentQuests.Data[2] };
        receive[json["index"].AsInt] = -1;
        GameData.RoyalPass.CurrentQuests.Data = receive;
        GameData.RoyalPass.Point.Data += GameData.RoyalPass.Quests[json["index"].AsInt].Reward;
    }
    public static void RequestSeasonQuest(int index)
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_RECEIVE_ROYALPASS_SEASON_QUEST.ToJson() },
            { "index", new JSONData(index)}
        };
        Instance.Send(jsonNode);
    }
    public static void ReceiveSeasonQuest(JSONNode json)
    {
        GameData.RoyalPass.Point.Data += GameData.RoyalPass.SeasonQuests[json["index"].AsInt].Reward;
        HashSet<int> receive = new HashSet<int>();
        receive.AddRange(GameData.RoyalPass.SeasonQuestsObtained.Data);
        receive.Add(json["index"].AsInt);
        GameData.RoyalPass.SeasonQuestsObtained.Data = receive;
    }
    public static void RequestReceiveRoyalPass(int index, int elite)
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_RECEIVE_ROYALPASS.ToJson() },
            { "m", new JSONData(index)},
            { "type", new JSONData(elite)},
        };
        Instance.Send(jsonNode);
    }
    public static void ReceiveReceiveRoyalPass(JSONNode json)
    {
        if(json["m"].AsInt == -1)
            return;
        if (json["type"].AsInt == 0)
        {
            HashSet<int> receive = new HashSet<int>();
            receive.AddRange(GameData.RoyalPass.NormalObtains.Data);
            receive.Add(json["m"].AsInt);
            List<GoodInfo> goods = GameData.RoyalPass.RewardNormals[json["m"].AsInt];
            for (int i = 0; i < goods.Count; i++)
            {
                goods[i].Type.Transact((int)goods[i].Value);
            }
            PopupHelper.CreateGoods(PrefabFactory.PopupGood, "You have received", goods);
            GameData.RoyalPass.NormalObtains.Data = receive;
        }
        else
        {
            HashSet<int> receive = new HashSet<int>(); 
            receive.AddRange(GameData.RoyalPass.EliteObtains.Data);
            receive.Add(json["m"].AsInt);
            List<GoodInfo> goods = GameData.RoyalPass.RewardElites[json["m"].AsInt];
            for (int i = 0; i < goods.Count; i++)
            {
                goods[i].Type.Transact((int)goods[i].Value);
            }
            PopupHelper.CreateGoods(PrefabFactory.PopupGood, "You have received", goods);
            GameData.RoyalPass.EliteObtains.Data = receive;
        }
    }
    public static void RequestClaimAllRoyalPass()
    {
        JSONArray jsonNormal = new JSONArray();
        JSONArray jsonElite = new JSONArray();
        for (int i = 0; i <= GameData.RoyalPass.Level; i++)
        {
            if (!GameData.RoyalPass.NormalObtains.Data.Contains(i))
            {
                jsonNormal.Add(new JSONData(i));
            }
            if (!GameData.RoyalPass.EliteObtains.Data.Contains(i))
            {
                jsonElite.Add(new JSONData(i));
            }
        }
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_CLAIM_ALL_ROYALPASS.ToJson() },
            { "normal",  jsonNormal},
            { "elite",  jsonElite}
        };
        Instance.Send(jsonNode);
    }
    public static void ReceiveClaimAllRoyalPass(JSONNode json)
    {
        Dictionary<int, GoodInfo> goods = new Dictionary<int, GoodInfo>();
        HashSet<int> normalReceive = new HashSet<int>();
        normalReceive.AddRange(GameData.RoyalPass.NormalObtains.Data);
        HashSet<int> eliteReceive = new HashSet<int>();
        eliteReceive.AddRange(GameData.RoyalPass.NormalObtains.Data);
        for (int i = 0; i < json["normal"].Count; i++)
        {
            normalReceive.Add(json["normal"][i].AsInt);
            var list = GameData.RoyalPass.RewardNormals[json["normal"][i].AsInt];
            for (int j = 0; j < list.Count; j++)
            {
                if (list[j].Type.GetPResourceType() == PResourceType.Nonconsumable)
                {
                    goods.Add(list[j].Type * 100 + (int)list[j].Value, list[j]);
                }
                else
                {
                    if (goods.ContainsKey(list[j].Type * 100))
                    {
                        goods[list[j].Type * 100] = new GoodInfo()
                        {
                            Type = list[j].Type,
                            Value = list[j].Value + goods[list[j].Type * 100].Value,
                        };
                    }
                    else
                    {
                        goods.Add(list[j].Type * 100, list[j]);
                    }
                }
            }
        }
        for (int i = 0; i < json["elite"].Count; i++)
        {
            eliteReceive.Add(json["elite"][i].AsInt);
            var list = GameData.RoyalPass.RewardElites[json["normal"][i].AsInt];
            for (int j = 0; j < list.Count; j++)
            {
                if (list[j].Type.GetPResourceType() == PResourceType.Nonconsumable)
                {
                    goods.Add(list[j].Type * 100 + (int)list[j].Value, list[j]);
                }
                else
                {
                    if (goods.ContainsKey(list[j].Type * 100))
                    {
                        goods[list[j].Type * 100] = new GoodInfo()
                        {
                            Type = list[j].Type,
                            Value = list[j].Value + goods[list[j].Type * 100].Value,
                        };
                    }
                    else
                    {
                        goods.Add(list[j].Type * 100, list[j]);
                    }
                }
            }
        }
        if (goods.Count>0)
        {
            PopupHelper.CreateGoods(PrefabFactory.PopupGood, "You have received", goods.ToList());
            GameData.RoyalPass.NormalObtains.Data = normalReceive;
            GameData.RoyalPass.EliteObtains.Data = eliteReceive;
        }


    }
    private void ReceiveChangeQuest(JSONNode json)
    {
        GameData.RoyalPass.CurrentQuests.Data[json["index"].AsInt] = json["n"].AsInt;
        Timer<QuestCard>.Instance.Begin();
    }
    #endregion
}
