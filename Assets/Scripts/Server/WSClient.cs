using Framework;
using JetBrains.Annotations;
using Monetization;
using SimpleJSON;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WSClient : WSClientBase
{
    [SerializeField] string text;

    protected override void Start()
    {
        base.Start();
        ServerMessenger.AddListener<JSONNode>(ServerResponse._PROFILE, OnLogin);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._CONFIG, GetConfig);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._CONFIG_SHOP, GetConfigShop);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._CHECK_RANK, GetCheckRank);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._CONFIG_ACHIEVEMENT, GetConfigAchievement);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._TRANSACTION, Transaction);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._LUCKYSHOT_EARN, LuckyShotEarn);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._CONFIG_RP, GetConfigRoyalPass);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._RP_DAILYQUEST_REWARD, DailyQuestReward);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._RP_SEASONQUEST_REWARD, SeasonQuestReward);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._RP_REWARD, RoyalPassReward);

        ServerMessenger.AddListener<JSONNode>(ServerResponse._CONFIG_ADS, ReceiveAdsConfig);
        ServerMessenger.AddListener<JSONNode>(ServerResponse.RECIEVE_TREASURE_CONFIG, ReceiveTreasureConfig);
        ServerMessenger.AddListener<JSONNode>(ServerResponse.RECIEVE_JOIN_TREASURE_ROOM, ReceiveJoinTreasureRoom);

        //not config
        ServerMessenger.AddListener<JSONNode>(ServerResponse.RECIEVE_CHANGE_QUEST, ReceiveChangeQuest);
        ServerMessenger.AddListener<JSONNode>(ServerResponse.RECIEVE_CLAIM_ALL_ROYALPASS, ReceiveClaimAllRoyalPass);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._GAME_RECONNECT, RecieveReconnect);

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._PROFILE, OnLogin);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._CONFIG, GetConfig);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._CONFIG_SHOP, GetConfigShop);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._CHECK_RANK, GetCheckRank);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._CONFIG_ACHIEVEMENT, GetConfigAchievement);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._TRANSACTION, Transaction);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._LUCKYSHOT_EARN, LuckyShotEarn);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._CONFIG_RP, GetConfigRoyalPass);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._RP_DAILYQUEST_REWARD, DailyQuestReward);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._RP_SEASONQUEST_REWARD, SeasonQuestReward);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._RP_REWARD, RoyalPassReward);

        ServerMessenger.RemoveListener<JSONNode>(ServerResponse.RECIEVE_TREASURE_CONFIG, ReceiveTreasureConfig);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse.RECIEVE_JOIN_TREASURE_ROOM, ReceiveJoinTreasureRoom);

        //not config
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse.RECIEVE_CHANGE_QUEST, ReceiveChangeQuest);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse.RECIEVE_CLAIM_ALL_ROYALPASS, ReceiveClaimAllRoyalPass);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._GAME_RECONNECT, RecieveReconnect);

    }
    public void OnLogin(JSONNode data)
    {
        GetConfig();
        GetConfigShop();
        GetCheckRank();
        GetConfigAchievement();
        GetConfigRoyalPass();

        RequestAdsConfig();
        AdsManager.SetUserId(PDataAuth.AuthData.userId.ToString());
        MusicType.MAINMENU.PlayMusic();
        PConsumableType.GEM.SetValue(int.Parse(data["d"]["d"]));
        PConsumableType.BERI.SetValue(int.Parse(data["d"]["g"]));
        PNonConsumableType.AVATAR_FRAME.FromJson(data["d"]["a"]["a_l"]);
        PNonConsumableType.AVATAR_FRAME.FromJson(data["d"]["a"]["f_l"]);
        PNonConsumableType.BATTLE_FIELD.FromJson(data["d"]["a"]["b_l"]);
        GameData.RocketCount.Data = data["d"]["a"]["l"]["r"].AsInt;
        Timer<LuckyShot>.Instance.BeginPoint = data["d"]["a"]["l"]["t"].AsLong.NowFrom0001From1970();
        RoyalPass.DataFromJson(GameData.RoyalPass, data["d"]["a"]["r"]);
        //PNonConsumableType.SKIN_SHIP.FromJson(data["d"]["a"]["ssA"]);
        GameData.Player = ProfileData.FromJson(GameData.Player, data);
        //RoyalPass.DataFromJson(GameData.RoyalPass, data["royalPass"]);
        //Timer<LuckyShot>.Instance.BeginPoint = long.Parse(data["timer"]["lfb"]).NowFrom0001From1970();
        //Timer<Gift>.Instance.BeginPoint = long.Parse(data["timer"]["lcr"]).NowFrom0001From1970();
        //Timer<RoyalPass>.Instance.BeginPoint = GameData.RoyalPass.End;
        //Timer<QuestCard>.Instance.BeginPoint = 300000000; //long.Parse(data["timer"]["as"]).NowFrom0001From1970();
        //GameData.ProgressGift = int.Parse(data["timer"]["cr"]);
        //CoreGame.timeInit = int.Parse(data["t"]);
        Timer<LuckyShot>.Instance.TriggerInterval_Sec = GameData.LuckyShotCoolDown;
        Timer<Gift>.Instance.TriggerInterval_Sec = GameData.GiftCoolDown;
        Timer<RankCollection>.Instance.TriggerInterval_Sec = GameData.RankReceiveCoolDown;
    }
    #region Rank
    private void GetCheckRank()
    {
        JSONNode jsonNode = new JSONClass
        {
            { "id", ServerRequest._CHECK_RANK.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    private void GetCheckRank(JSONNode data)
    {
        Timer<RankCollection>.Instance.BeginPoint = long.Parse(data["d"]["t"]).NowFrom0001From1970();
    }
    public static void GetRankReward()
    {
        JSONNode jsonNode = new JSONClass
        {
            { "id", ServerRequest._RANK_REWARD.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    #endregion
    #region Profile
    public static void ChangeAvatar(int i)
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", ServerResponse._CHANGE_AVATAR.ToJson() },
            { "a", i.ToJson()}
        };
        Instance.Send(jsonNode);
    }
    public static void ChangeFrame(int i)
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", ServerResponse._CHANGE_FRAME.ToJson() },
            { "f", i.ToJson()}
        };
        Instance.Send(jsonNode);
    }
    public static void ChangeBattleField(int i)
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", ServerResponse._CHANGE_BATTLE_FIELD.ToJson() },
            { "b", i.ToString()}
        };
        Instance.Send(jsonNode);
    }
    public static void ChangeSkinShip(int i)
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", ServerResponse.REQUEST_CHANGE_SKIN_SHIP.ToJson() },
            { "s", i.ToString()}
        };
        Instance.Send(jsonNode);
    }
    public static void ChangeName(string name)
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", ServerResponse._CHANGE_NAME.ToJson() },
            { "n", name}
        };
        Instance.Send(jsonNode);
    }

    void GetConfig()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", ServerRequest._CONFIG.ToJson() },
            { "v", new JSONData(0) },
        };
        Instance.Send(jsonNode);
    }
    void GetConfig(JSONNode data)
    {
        // rank
        GameData.RankConfigs = RankConfig.ListFromJson(data["d"]["level"]);
        // bet
        try
        {
            GameData.Bets = BetData.ListFromJson(data["d"]["match"]["classic"]);
        }catch(Exception e)
        {
            Debug.Log(e.ToString());
        }
        // royalpass

        // luckyshot
        Timer<LuckyShot>.Instance.TriggerInterval_Sec = data["d"]["lucky_shot"]["rocket_restore_period"].AsInt / 1000;

        // gift
        SceneTransitionHelper.Load(ESceneName.Home);

    }

    private void RequestRoyalPassConfig()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", ServerResponse.REQUEST_COUNTDOWN_CONFIG.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    private void ReceiveRoyalPassConfig(JSONNode data)
    {
        GameData.RoyalPass = RoyalPass.ConfigFromJson(GameData.RoyalPass, data);
        SceneTransitionHelper.Load(ESceneName.Home);
    }

    public static void RequestGiftConfig()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", ServerResponse.REQUEST_GIFT_CONFIG.ToJson() },
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
    #region Shop
    void GetConfigShop()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", ServerRequest._CONFIG_SHOP.ToJson() },
            { "v", new JSONData(0) },
        };
        Instance.Send(jsonNode);
    }
    void GetConfigShop(JSONNode data)
    {
        GameData.TransactionConfigs = new Dictionary<TransactionType, List<TransactionInfo>>();
        Array @enum = Enum.GetValues(typeof(TransactionType));
        for (int i = 0; i < @enum.Length; i++)
        {
            if (i > 0)
            {
                GameData.TransactionConfigs.Add((TransactionType)i, TransactionInfo.ListFromJson(data["d"]["shops"][@enum.GetValue(i).ToString()], i));
            }
            else
            {
                List<TransactionInfo> infos = new List<TransactionInfo>();
                infos.Add(TransactionInfo.FromJson(data["d"]["shops"][@enum.GetValue(i).ToString()], i, 0));
                GameData.TransactionConfigs.Add((TransactionType)i, infos);
            }

        }
        SceneTransitionHelper.Load(ESceneName.Home);
    }
    public static void Transaction(JSONNode data)
    {
        TransactionType id = data["d"]["s"].ToEnum<TransactionType>();
        int index = data["d"]["p"].AsInt;
        GameData.TransactionConfigs[id][index].Transact();
        Messenger.Broadcast(GameEvent.TRANSACTION, id, index);
    }
    #endregion
    #region Achievement
    void GetConfigAchievement()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", ServerRequest._CONFIG_ACHIEVEMENT.ToJson() },
            { "v", new JSONData(0) },
        };
        Instance.Send(jsonNode);
    }
    void GetConfigAchievement(JSONNode data)
    {
        GameData.AchievementConfig = new Dictionary<AchievementType, AchievementInfo>();
        for (int i = 0; i < data["d"]["achievements"].Count; i++)
        {
            GameData.AchievementConfig.Add((AchievementType)i, AchievementInfo.FromJson(data["d"]["achievements"][i], i));
        }
    }
    public static void RequestObtainAchievemnt(int id)
    {
        JSONNode jsonNode = new JSONClass
        {
            { "id", ServerResponse._ACHIEVEMENT_REWARD.ToJson() },
            { "a", id.ToJson() },
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
            { "id", ServerRequest._CHANGE_ACHIEVEMENT.ToJson() },
            { "a", array },
        };
        Instance.Send(jsonNode);
    }
    #endregion
    #region Lucky Shot
    public void RequestAdsConfig()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", ServerRequest._CONFIG_ADS.ToJson() },
            { "v",  new JSONData(0)}
        };
        Instance.Send(jsonNode);
    }
    public void ReceiveAdsConfig(JSONNode data) 
    {
        Debug.LogError(data["d"]["ad_unit"].Count);
        for (int i=0; i < data["d"]["ad_unit"].Count; i++)
        {
            switch (data["d"]["ad_unit"][i]["name"].ToString())
            {
                case "battleship_android_luckyshot_rocket":
                    Debug.Log("alo");
                    GameData.BeriBonusAmount = int.Parse(data["d"]["ad_unit"][i]["reward"]);
                    Debug.Log(GameData.BeriBonusAmount);
                    break;
            }
        }
    }
    public void LuckyShotEarn(JSONNode data)
    {
        GameData.RocketCount.Data = data["d"]["l"]["r"].AsInt;
        Timer<LuckyShot>.Instance.BeginPoint = data["d"]["l"]["t"].AsLong.NowFrom0001From1970();
    }
    public static void LuckyShotEarn()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", ServerRequest._LUCKYSHOT_EARN.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    public static void RequestShot()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", ServerRequest._LUCKYSHOT_FIRE.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    #endregion
    #region TREASUREHUNT

    private static bool waitingJoinTreasureRoom = false;

    public static void RequestTreasureConfig()
    {
        JSONNode jsonNode = new JSONClass()
        {
            {"id", ServerResponse.REQUEST_TREASURE_CONFIG.ToJson() },
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
            {"id", ServerResponse.REQUEST_JOIN_TREASURE_ROOM.ToJson() },
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
            {"id", ServerResponse.REQUEST_SHOOT_TREASURE.ToJson() },
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
            {"id", ServerResponse.REQUEST_EXIT_TREASURE_ROOM.ToJson() },
        };
        Instance.Send(jsonNode);
    }

    #endregion
    #region CoreGame
    public static void SearchOpponent(int bet, List<Ship> ships)
    {
        JSONNode jsonNode = new JSONClass
        {
            { "id", ServerRequest._FIND_MATCH.ToJson() },
            { "t", bet.ToJson() }
        };
        Instance.Send(jsonNode);
    }
    public static void SubmitShip(int bet,List<Ship> ships)
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
        Instance.Send(jsonNode);
    }
    public static void AttackOpponent(int room, int x, int y)
    {
        JSONNode jsonNode = new JSONClass
        {
            { "id", ServerResponse._ATTACK.ToJson() },
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
            { "id", ServerRequest._CANCEL_FIND_MATCH.ToJson() },
            { "t", bet.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    public static void QuitGame(int room)
    {
        JSONNode jsonNode = new JSONClass
        {
            { "id", ServerResponse._QUIT_GAME.ToJson() },
            { "r", room.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    public static void RequesRematch(int room)
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", ServerRequest._REMATCH.ToJson() },
            { "r", room.ToJson() },
        };
        Instance.Send(jsonNode);
    }

    public static void RequestReconnect()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", ServerResponse.REQUEST_RECONNECT.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    public static void RecieveReconnect(JSONNode data)
    {
        CoreGame.reconnect = data["d"];
        SceneTransitionHelper.Load(ESceneName.MainGame);
    }
    #endregion
    #region Royal Pass
    void GetConfigRoyalPass()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", ServerRequest._CONFIG_RP.ToJson() },
            { "v", new JSONData(0) },
        };
        Instance.Send(jsonNode);
    }
    void GetConfigRoyalPass(JSONNode data)
    {
        RoyalPass.ConfigFromJson(GameData.RoyalPass, data["d"]);
    }
    public static void DailyQuestReward(int index)
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", ServerResponse._RP_DAILYQUEST_REWARD.ToJson() },
            { "i", new JSONData(index)}
        };
        Instance.Send(jsonNode);
    }
    public static void DailyQuestReward(JSONNode json)
    {
        int[] receive = new int[3] { GameData.RoyalPass.CurrentQuests.Data[0], GameData.RoyalPass.CurrentQuests.Data[1], GameData.RoyalPass.CurrentQuests.Data[2] };
        receive[json["d"]["i"].AsInt] = -1;
        GameData.RoyalPass.CurrentQuests.Data = receive;
        GameData.RoyalPass.Point.Data += json["d"]["r"].AsInt;
    }
    public static void SeasonQuestReward(int index)
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", ServerResponse._RP_SEASONQUEST_REWARD.ToJson() },
            { "i", new JSONData(index)}
        };
        Instance.Send(jsonNode);
    }
    public static void SeasonQuestReward(JSONNode json)
    {
        GameData.RoyalPass.Point.Data += json["d"]["r"].AsInt;
        HashSet<int> receive = new HashSet<int>();
        receive.AddRange(GameData.RoyalPass.SeasonQuestsObtained.Data);
        receive.Add(json["d"]["i"].AsInt);
        GameData.RoyalPass.SeasonQuestsObtained.Data = receive;
    }
    public static void RoyalPassReward(int index, int elite)
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", ServerResponse._RP_REWARD.ToJson() },
            { "m", new JSONData(index)},
            { "t", new JSONData(elite)},
        };
        Instance.Send(jsonNode);
    }
    public static void RoyalPassReward(JSONNode json)
    {
        if (json["e"].AsInt != 0 || json["d"]["m"].AsInt == -1)
            return;
        if (json["d"]["t"].AsInt == 0)
        {
            HashSet<int> receive = new HashSet<int>();
            receive.AddRange(GameData.RoyalPass.NormalObtains.Data);
            receive.Add(json["d"]["m"].AsInt);
            List<GoodInfo> goods = GameData.RoyalPass.RewardNormals[json["d"]["m"].AsInt];
            for (int i = 0; i < goods.Count; i++)
            {
                goods[i].Type.Transact((int)goods[i].Value);
            }
            if (goods.Count>0)
            {
                PopupHelper.CreateGoods(PrefabFactory.PopupRPGood, "You have received", goods);
            }
            GameData.RoyalPass.NormalObtains.Data = receive;
        }
        else
        {
            HashSet<int> receive = new HashSet<int>();
            receive.AddRange(GameData.RoyalPass.EliteObtains.Data);
            receive.Add(json["d"]["m"].AsInt);
            List<GoodInfo> goods = GameData.RoyalPass.RewardElites[json["d"]["m"].AsInt];
            for (int i = 0; i < goods.Count; i++)
            {
                goods[i].Type.Transact((int)goods[i].Value);
            }
            if (goods.Count > 0)
            {
                PopupHelper.CreateGoods(PrefabFactory.PopupRPGood, "You have received", goods);
            }
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
            { "id", ServerResponse.REQUEST_CLAIM_ALL_ROYALPASS.ToJson() },
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
        if (goods.Count > 0)
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
    #region Other Feature
    public static void RequestGift()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", ServerResponse.REQUEST_GIFT.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    #endregion
    public void RequestServer()
    {
        Instance.Send(JSONNode.Parse(text));
    }
}
