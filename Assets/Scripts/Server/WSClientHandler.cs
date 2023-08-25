using Authentication;
using Framework;
using Monetization;
using SimpleJSON;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class WSClientHandler : Singleton<WSClientHandler>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        WSClient.Instance.OnConnect += () =>
        {
            Debug.Log("Onconnect");
            ServerMessenger.AddListener<JSONNode>(ServerResponse._CONFIG, GetConfig);
            ServerMessenger.AddListener<JSONNode>(ServerResponse._CONFIG_SHOP, GetConfigShop);
            ServerMessenger.AddListener<JSONNode>(ServerResponse._CONFIG_ACHIEVEMENT, GetConfigAchievement);
            ServerMessenger.AddListener<JSONNode>(ServerResponse._CONFIG_RP, GetConfigRoyalPass);
            ServerMessenger.AddListener<JSONNode>(ServerResponse._CONFIG_ADS, ReceiveAdsConfig);
            ServerMessenger.AddListener<JSONNode>(ServerResponse._GIFT_CONFIG, GetConfigGift);

            //not config
            ServerMessenger.AddListener<JSONNode>(ServerResponse._PROFILE, GetData);
            ServerMessenger.AddListener<JSONNode>(ServerResponse._CHECK_RANK, GetCheckRank);
            ServerMessenger.AddListener<JSONNode>(ServerResponse._TRANSACTION, Transaction);
            ServerMessenger.AddListener<JSONNode>(ServerResponse._LUCKYSHOT_EARN, LuckyShotEarn);
            ServerMessenger.AddListener<JSONNode>(ServerResponse._RP_DAILYQUEST_REWARD, DailyQuestReward);
            ServerMessenger.AddListener<JSONNode>(ServerResponse._RP_SEASONQUEST_REWARD, SeasonQuestReward);
            ServerMessenger.AddListener<JSONNode>(ServerResponse._RP_REWARD, RoyalPassReward);
            ServerMessenger.AddListener<JSONNode>(ServerResponse._RP_CHANGE_QUEST, ReceiveChangeQuest);
            ServerMessenger.AddListener<JSONNode>(ServerResponse._RP_DATA, GetDataRoyalPass);
            ServerMessenger.AddListener<JSONNode>(ServerResponse._REWARD_ADS, ReceiveRewardAds);
            //ServerMessenger.AddListener<JSONNode>(ServerResponse.RECIEVE_CLAIM_ALL_ROYALPASS, ReceiveClaimAllRoyalPass);
            ServerMessenger.AddListener<JSONNode>(ServerResponse._GAME_RECONNECT, RecieveReconnect);
            ServerMessenger.AddListener<JSONNode>(ServerResponse.RECIEVE_TREASURE_CONFIG, ReceiveTreasureConfig);
            ServerMessenger.AddListener<JSONNode>(ServerResponse.RECIEVE_JOIN_TREASURE_ROOM, ReceiveJoinTreasureRoom);
        };
        WSClient.Instance.OnDisconnect += () =>
        {
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse._CONFIG, GetConfig);
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse._CONFIG_SHOP, GetConfigShop);
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse._CONFIG_ACHIEVEMENT, GetConfigAchievement);
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse._CONFIG_RP, GetConfigRoyalPass);
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse._CONFIG_ADS, ReceiveAdsConfig);
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse._GIFT_CONFIG, GetConfigGift);


            //not config
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse._PROFILE, GetData);
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse._CHECK_RANK, GetCheckRank);
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse._TRANSACTION, Transaction);
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse._LUCKYSHOT_EARN, LuckyShotEarn);
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse._RP_DAILYQUEST_REWARD, DailyQuestReward);
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse._RP_SEASONQUEST_REWARD, SeasonQuestReward);
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse._RP_REWARD, RoyalPassReward);
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse._RP_CHANGE_QUEST, ReceiveChangeQuest);
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse._RP_DATA, GetDataRoyalPass);
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse._REWARD_ADS, ReceiveRewardAds);
            //ServerMessenger.RemoveListener<JSONNode>(ServerResponse.RECIEVE_CLAIM_ALL_ROYALPASS, ReceiveClaimAllRoyalPass);
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse._GAME_RECONNECT, RecieveReconnect);
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse.RECIEVE_TREASURE_CONFIG, ReceiveTreasureConfig);
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse.RECIEVE_JOIN_TREASURE_ROOM, ReceiveJoinTreasureRoom);
        };
        WSClient.Instance.OnSystemError += () =>
        {
            SceneTransitionHelper.Load(ESceneName.PreHome);
        };
        WSClient.Instance.OnAdminKick += () =>
        {
            SceneTransitionHelper.Load(ESceneName.PreHome);
        };
        WSClient.Instance.OnTokenInvalid += () =>
        {
            if (SceneManager.GetActiveScene().name == "Loading")
            {
                LoadingScene.Instance.LoadScene("PreHome");
            }
            else
            {
                SceneTransitionHelper.Load(ESceneName.PreHome);
            }
        };
        WSClient.Instance.OnLoginInOtherDevice += () =>
        {
            SceneTransitionHelper.Load(ESceneName.PreHome);
        };
        WSClient.Instance.OnLostConnection += () =>
        {
            WSClient.Instance.Disconnect();
            PopupHelper.CreateConfirm(PrefabFactory.PopupDisconnect, "Disconnect", "Please reconnect", null, (confirm) =>
            {
                if (confirm)
                {
                    SceneManager.LoadScene("Reconnect");
                }
            });
        };
    }
    public static void GetData(JSONNode data)
    {
        GetConfig();
        GetConfigShop();
        GetCheckRank();
        GetConfigAchievement();
        GetConfigRoyalPass();
        RequestAdsConfig();
        GetConfigGift();

        AdsManager.SetUserId(DataAuth.AuthData.userId.ToString());
        MusicType.MAINMENU.PlayMusic();
        PConsumableType.GEM.SetValue(int.Parse(data["d"]["d"]));
        PConsumableType.BERI.SetValue(int.Parse(data["d"]["g"]));
        PNonConsumableType.AVATAR.FromJson(data["d"]["a"]["k"]["al"]);
        PNonConsumableType.AVATAR_FRAME.FromJson(data["d"]["a"]["k"]["fl"]);
        PNonConsumableType.BATTLE_FIELD.FromJson(data["d"]["a"]["k"]["bl"]);
        PNonConsumableType.ELITE.GetData().Data = new HashSet<int>();
        if (data["d"]["a"]["r"]["t"].AsInt == 1)
        {
            PNonConsumableType.ELITE.GetValue().Add(0);
        }
        GameData.RocketCount.Data = data["d"]["a"]["l"]["r"].AsInt;
        GameData.Player = ProfileData.FromJson(GameData.Player, data);
        RoyalPass.DataFromJson(GameData.RoyalPass, data["d"]["a"]["r"]);
        
        Timer<LuckyShot>.Instance.BeginPoint = data["d"]["a"]["l"]["t"].AsLong.NowFrom0001From1970();
        Timer<LuckyShot>.Instance.TriggerInterval_Sec = GameData.LuckyShotCoolDown;
        Timer<Gift>.Instance.TriggerInterval_Sec = GameData.GiftCoolDown;
        Timer<Gift>.Instance.BeginPoint = data["d"]["a"]["d"]["t"].AsLong.NowFrom0001From1970();
        Timer<RankCollection>.Instance.TriggerInterval_Sec = GameData.RankReceiveCoolDown;
        GameData.Starter = data["d"]["a"]["s"].AsInt == 1;
        GameData.ProgressGift = data["d"]["a"]["d"]["i"].AsInt;

    }
    #region Rank
    private static void GetCheckRank()
    {
        new JSONClass
        {
            { "id", ServerRequest._CHECK_RANK.ToJson() },
        }.RequestServer();
    }
    private static void GetCheckRank(JSONNode data)
    {
        Timer<RankCollection>.Instance.BeginPoint = long.Parse(data["d"]["t"]).NowFrom0001From1970();
    }
    public static void GetRankReward()
    {
        new JSONClass
        {
            { "id", ServerRequest._RANK_REWARD.ToJson() },
        }.RequestServer();
    }
    #endregion
    #region Profile
    public static void ChangeAvatar(int i)
    {
        new JSONClass()
        {
            { "id", ServerResponse._CHANGE_AVATAR.ToJson() },
            { "a", i.ToJson()}
        }.RequestServer();
    }
    public static void ChangeFrame(int i)
    {
        new JSONClass()
        {
            { "id", ServerResponse._CHANGE_FRAME.ToJson() },
            { "f", i.ToJson()}
        }.RequestServer();
    }
    public static void ChangeBattleField(int i)
    {
        new JSONClass()
        {
            { "id", ServerResponse._CHANGE_BATTLE_FIELD.ToJson() },
            { "b", i.ToString()}
        }.RequestServer();
    }
    public static void ChangeSkinShip(int i)
    {
        new JSONClass()
        {
            //{ "id", ServerResponse.REQUEST_CHANGE_SKIN_SHIP.ToJson() },
            { "s", i.ToString()}
        }.RequestServer();
    }
    public static void ChangeName(string name)
    {
        new JSONClass()
        {
            { "id", ServerResponse._CHANGE_NAME.ToJson() },
            { "n", name}
        }.RequestServer();
    }

    static void GetConfig()
    {
        new JSONClass()
        {
            { "id", ServerRequest._CONFIG.ToJson() },
            { "v", new JSONData(0) },
        }.RequestServer();
    }
    static void GetConfig(JSONNode data)
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
        // luckyshot
        Timer<LuckyShot>.Instance.TriggerInterval_Sec = data["d"]["lucky_shot"]["rocket_restore_period"].AsInt / 1000;

        // gift
        if (SceneManager.GetActiveScene().name == "PreHome")
        {
            SceneTransitionHelper.Load(ESceneName.Home);
        }
        else
        {
            LoadingScene.Instance.LoadScene("Home");
            //SceneTransitionHelper.Load(ESceneName.Home);
        }

    }
    #endregion
    #region Shop
    static void GetConfigShop()
    {
        new JSONClass()
        {
            { "id", ServerRequest._CONFIG_SHOP.ToJson() },
            { "v", new JSONData(0) },
        }.RequestServer();
    }
    static void GetConfigShop(JSONNode data)
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
                List<TransactionInfo> infos = new List<TransactionInfo>
                {
                    TransactionInfo.FromJson(data["d"]["shops"][@enum.GetValue(i).ToString()], i, 0)
                };
                GameData.TransactionConfigs.Add((TransactionType)i, infos);
            }

        }
    }
    public static void Transaction(JSONNode data)
    {
        TransactionType id = data["d"]["s"].ToEnum<TransactionType>();
        int index = data["d"]["p"].AsInt;
        GameData.TransactionConfigs[id][index].Transact();
    }
    #endregion
    #region Achievement
    static void GetConfigAchievement()
    {
        new JSONClass()
        {
            { "id", ServerRequest._CONFIG_ACHIEVEMENT.ToJson() },
            { "v", new JSONData(0) },
        }.RequestServer();
    }
    static void GetConfigAchievement(JSONNode data)
    {
        GameData.AchievementConfig = new Dictionary<AchievementType, AchievementInfo>();
        for (int i = 0; i < data["d"]["achievements"].Count; i++)
        {
            GameData.AchievementConfig.Add((AchievementType)i, AchievementInfo.FromJson(data["d"]["achievements"][i], i));
            int _i = i;
            ((QuestType)_i).AddListenerOnProgress((oValue, nValue) => {
                int oProgress = GameData.Player.AchievementProgress[_i];
                GameData.Player.AchievementProgress[_i] += (nValue - oValue);
                AchievementType type = (AchievementType)_i;
                int nextMilestone = 0;
                for (int i = 0; i < GameData.AchievementConfig[type].AchivementUnits.Length; i++)
                {
                    if (oProgress >= GameData.AchievementConfig[type].AchivementUnits[i].Task)
                    {
                        nextMilestone++;
                    }
                }
                if (nextMilestone < GameData.AchievementConfig[type].AchivementUnits.Length &&
                GameData.Player.AchievementProgress[_i] >= GameData.AchievementConfig[type].AchivementUnits[nextMilestone].Task 
                && oProgress < GameData.AchievementConfig[type].AchivementUnits[nextMilestone].Task )
                {
                    PopupHelper.CreateMessage(PrefabFactory.PospupQuestCompleted, null ,AchievementInfo.GetDescription(type, GameData.AchievementConfig[type].AchivementUnits[nextMilestone].Task), null);
                }
            });         
        }
    }
    public static void RequestObtainAchievemnt(int id)
    {
        new JSONClass
        {
            { "id", ServerResponse._ACHIEVEMENT_REWARD.ToJson() },
            { "a", id.ToJson() },
        }.RequestServer();
    }
    public static void RequestChangeAchievement(int[] indexs)
    {
        JSONArray array = new JSONArray();
        JSONNode node = new JSONClass();
        for (int i = 0; i < indexs.Length; i++)
        {
            array.Add(new JSONData(indexs[i]));
        }
        new JSONClass()
        {
            { "id", ServerRequest._CHANGE_ACHIEVEMENT.ToJson() },
            { "a", array },
        }.RequestServer();
    }
    #endregion
    #region Ads
    public static void RequestAdsConfig()
    {
        new JSONClass()
        {
            { "id", ServerRequest._CONFIG_ADS.ToJson() },
            { "v",  new JSONData(0)}
        }.RequestServer();
    }
    public static void ReceiveAdsConfig(JSONNode data)
    {
        if (int.Parse(data["d"]["version"]) == AdsData.versionAds)
            return;

        AdsData.versionAds = int.Parse(data["d"]["version"]);


        int platform;
#if PLATFORM_ANDROID || UNITY_ANDROID
        platform = 2;
#else
        platform = 1;
#endif
        for (int i = 0; i < data["d"]["ad_unit"].Count; i++)
        {
            if (int.Parse(data["d"]["ad_unit"][i]["platform"]) != platform)
                continue;
            AdsData.adsUnitIdMap.Add((RewardType)int.Parse(data["d"]["ad_unit"][i]["reward_type"][0]), data["d"]["ad_unit"][i]["ad_unit_id"]);
            string key = data["d"]["ad_unit"][i]["ad_unit_id"];
            AdsRewardConfig value = new AdsRewardConfig();
            value.reward = data["d"]["ad_unit"][i]["reward"].ToListInt();
            value.rewardAdUnitId = data["d"]["ad_unit"][i]["ad_unit_id"];
            AdsData.rewardTypeToConfigMap.Add(key, value);
        }
    }

    public static void ReceiveRewardAds(JSONNode data)
    {
        string ads_unit_id = data["d"]["a"];
        if (String.Equals(ads_unit_id, AdsData.adsUnitIdMap[RewardType.Get_Beri]))
        {
            PConsumableType.BERI.AddValue(int.Parse(data["d"]["g"]));
        }
        else if (String.Equals(ads_unit_id, AdsData.adsUnitIdMap[RewardType.Get_Rocket]))
        {
            GameData.RocketCount.Data = int.Parse(data["d"]["l"]["r"]);
        }
        else if (String.Equals(ads_unit_id, AdsData.adsUnitIdMap[RewardType.Get_Quest]))
        {
            GameData.RoyalPass.CurrentQuests.Data = data["d"]["q"]["q"].ToArrayInt(true);
            GameData.RoyalPass.CurrentQuestsProgress = data["d"]["q"]["p"].ToArrayInt(true);
        }
        else if (String.Equals(ads_unit_id, AdsData.adsUnitIdMap[RewardType.Change_Quest]))
        {
            GameData.RoyalPass.CurrentQuests.Data[int.Parse(data["d"]["i"])] = int.Parse(data["d"]["n"]);
            GameData.RoyalPass.CurrentQuestsProgress[int.Parse(data["d"]["i"])] = int.Parse(data["d"]["q"]["p"][int.Parse(data["d"]["i"])]);
        }
        else if (String.Equals(ads_unit_id, AdsData.adsUnitIdMap[RewardType.Get_X2DailyGift]))
        {
            Gift.OnGetAdsGift(data);
        }
    }
    #endregion
    #region Lucky Shot
    public static void LuckyShotEarn(JSONNode data)
    {
        GameData.RocketCount.Data = data["d"]["l"]["r"].AsInt;
        Timer<LuckyShot>.Instance.BeginPoint = data["d"]["l"]["t"].AsLong.NowFrom0001From1970();
    }
    public static void LuckyShotEarn()
    {
        new JSONClass()
        {
            { "id", ServerRequest._LUCKYSHOT_EARN.ToJson() },
        }.RequestServer();
    }
    public static void RequestShot()
    {
        new JSONClass()
        {
            { "id", ServerRequest._LUCKYSHOT_FIRE.ToJson() },
        }.RequestServer();
    }
    #endregion
    #region TREASUREHUNT

    private static bool waitingJoinTreasureRoom = false;

    public static void RequestTreasureConfig()
    {
        new JSONClass()
        {
            {"id", ServerResponse.REQUEST_TREASURE_CONFIG.ToJson() },
        }.RequestServer();
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
        new JSONClass()
        {
            {"id", ServerResponse.REQUEST_JOIN_TREASURE_ROOM.ToJson() },
            {"b" , rom.ToJson() },
        }.RequestServer();
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
        new JSONClass()
        {
            {"id", ServerResponse.REQUEST_SHOOT_TREASURE.ToJson() },
            {"b" , GameData.JoinTreasureRoom.RoomId.ToJson() },
            {"x" , y.ToJson() },
            {"y" , x.ToJson() },
        }.RequestServer();
    }

    public static void RequestExitTreasureRoom()
    {
        new JSONClass()
        {
            {"id", ServerResponse.REQUEST_EXIT_TREASURE_ROOM.ToJson() },
        }.RequestServer();
    }

    #endregion
    #region CoreGame
    public static void SearchOpponent(int bet, List<Ship> ships)
    {
        new JSONClass
        {
            { "id", ServerRequest._FIND_MATCH.ToJson() },
            { "t", bet.ToJson() }
        }.RequestServer();
    }
    public static void SubmitShip(int bet, List<Ship> ships)
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
        jsonNode.RequestServer();
    }
    public static void AttackOpponent(int room, int x, int y)
    {
        new JSONClass
        {
            { "id", ServerResponse._ATTACK.ToJson() },
            { "r",  room.ToJson() },
            { "x",  x.ToJson() },
            { "y",  y.ToJson() },
        }.RequestServer();
    }
    public static void QuitSearch(int bet)
    {
        new JSONClass
        {
            { "id", ServerRequest._CANCEL_FIND_MATCH.ToJson() },
            { "t", bet.ToJson() },
        }.RequestServer();
    }
    public static void QuitGame(int room)
    {
        new JSONClass
        {
            { "id", ServerResponse._QUIT_GAME.ToJson() },
            { "r", room.ToJson() },
        }.RequestServer();
    }
    public static void RequesRematch(int room)
    {
        new JSONClass()
        {
            { "id", ServerRequest._REMATCH.ToJson() },
            { "r", room.ToJson() },
        }.RequestServer();
    }
    public static void RecieveReconnect(JSONNode data)
    {
        CoreGame.reconnect = data["d"];
        SceneTransitionHelper.Load(ESceneName.MainGame);
    }
    #endregion
    #region Royal Pass
    static void GetDataRoyalPass()
    {
        new JSONClass()
        {
            { "id", ServerRequest._RP_DATA.ToJson() },
        }.RequestServer();
    }
    static void GetDataRoyalPass(JSONNode data)
    {
        RoyalPass.DataFromJson(GameData.RoyalPass, data["d"]);
    }
    static void GetConfigRoyalPass()
    {
        new JSONClass()
        {
            { "id", ServerRequest._CONFIG_RP.ToJson() },
            { "v", new JSONData(0) },
        }.RequestServer();
    }
    static void GetConfigRoyalPass(JSONNode data)
    {
        RoyalPass.ConfigFromJson(GameData.RoyalPass, data["d"]);
        GameData.RoyalPass.Version = data["v"].AsInt;
        GetDataRoyalPass();
    }
    public static void DailyQuestReward(int index)
    {
        new JSONClass()
        {
            { "id", ServerResponse._RP_DAILYQUEST_REWARD.ToJson() },
            { "i", new JSONData(index)}
        }.RequestServer();
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
        new JSONClass()
        {
            { "id", ServerResponse._RP_SEASONQUEST_REWARD.ToJson() },
            { "i", new JSONData(index)}
        }.RequestServer();
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
        new JSONClass()
        {
            { "id", ServerResponse._RP_REWARD.ToJson() },
            { "m", new JSONData(index)},
            { "t", new JSONData(elite)},
        }.RequestServer();
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
                if (PNonConsumableType.ELITE.GetValue().Contains(0))
                {
                    PopupHelper.CreateGoods(PrefabFactory.PopupGood, "", goods);

                }
                else
                {
                    PopupHelper.CreateGoods(PrefabFactory.PopupRPGood, "Unlock elite to receive", goods);

                }
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
                PopupHelper.CreateGoods(PrefabFactory.PopupGood, "You have received", goods);
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
        new JSONClass()
        {
            //{ "id", ServerResponse.REQUEST_CLAIM_ALL_ROYALPASS.ToJson() },
            { "normal",  jsonNormal},
            { "elite",  jsonElite}
        }.RequestServer();
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
            var list = GameData.RoyalPass.RewardElites[json["elite"][i].AsInt];
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
    private static void ReceiveChangeQuest(JSONNode json)
    {
        GameData.RoyalPass.CurrentQuests.Data[json["d"]["i"].AsInt] = json["d"]["n"].AsInt;
        Timer<QuestCard>.Instance.Begin();
    }
    private static void AddQuest()
    {
        JSONNode jsonNode = new JSONClass()
        {
            //{ "id", ServerResponse.REQUEST_CLAIM_ALL_ROYALPASS.ToJson() },
        };
    }
    private static void AddQuest(JSONNode json)
    {
        GameData.RoyalPass.CurrentQuests.Data[json["d"]["i"].AsInt] = json["d"]["n"].AsInt;
    }
    #endregion
    #region Gift
    public static void GetConfigGift()
    {
        new JSONClass()
        {
            { "id", ServerRequest._GIFT_CONFIG.ToJson() },
            { "v", new JSONData(0) },
        }.RequestServer();
    }
    public static void GetConfigGift(JSONNode data)
    {
        GameData.GiftConfig = data["d"]["gold"].ToListInt();
        Timer<Gift>.Instance.TriggerInterval_Sec = data["d"]["bonus_period"].AsInt / 1000;
    }
    public static void GetGift()
    {
        new JSONClass()
        {
            { "id", ServerRequest._GIFT.ToJson() },
        }.RequestServer();
    }
    #endregion

}
