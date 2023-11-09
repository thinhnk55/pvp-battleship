using Authentication;
using DG.Tweening;
using Framework;
using Monetization;
using Server;
using SimpleJSON;
//using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WSClientHandler : Framework.Singleton<WSClientHandler>
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
            ServerMessenger.AddListener<JSONNode>(ServerResponse._LEADERBOARD_CONFIG, LeaderBoardConfig);
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
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse._LEADERBOARD_CONFIG, LeaderBoardConfig);

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
            PopupReconnect();
        };
    }

    public static void PopupReconnect()
    {
        if (!PopupDisconnect.Instance)
        {
            PopupHelper.CreateConfirm(PrefabFactory.PopupDisconnect, "Disconnect", "Please reconnect", null, (confirm) =>
            {
                if (confirm)
                {
                    if (Application.internetReachability == NetworkReachability.NotReachable)
                    {
                        SceneTransitionHelper.Load(ESceneName.PreHome);
                        DOVirtual.DelayedCall(1.5f, () => PopupReconnect());
                    }
                    else
                    {
                        SceneManager.LoadScene("Loading");
                    }
                }
            });
        }
    }
    public static void GetData(JSONNode data)
    {
        MusicType.MAINMENU.PlayMusic();
        Firebase.Crashlytics.Crashlytics.SetUserId(DataAuth.AuthData.userId.ToString());
        FirebaseIntegration.AnalyticsHelper.Login();
        AdsManager.SetUserId(DataAuth.AuthData.userId.ToString());
        HTTPClientAuth.CheckLinkedAccount();

        GetConfig();
        GetConfigShop();
        GetConfigAchievement();
        GetConfigRoyalPass();
        RequestAdsConfig();
        GetConfigGift();
        LeaderBoardConfig(); //You need to place this configuration retrieval command after other configuration retrieval commands to ensure that the load home scene command is called after the configuration has been successfully retrieved.

        GetCheckRank();
        PConsumableType.GEM.SetValue(int.Parse(data["d"]["d"]));
        PConsumableType.BERRY.SetValue(int.Parse(data["d"]["g"]));
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
        GameData.StarterShow = false;
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
            { "v", new JSONData(GameData.VersionConfig) },
        }.RequestServer();
    }
    static void GetConfig(JSONNode data)
    {
        if (data["d"]["version"].AsInt == GameData.VersionConfig)
            return;

        // rank
        GameData.RankConfigs = RankConfig.ListFromJson(data["d"]["level"]);
        // bet
        try
        {
            GameData.Bets = BetData.ListFromJson(data["d"]["match"]["classic"]);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        // luckyshot
        Timer<LuckyShot>.Instance.TriggerInterval_Sec = data["d"]["lucky_shot"]["rocket_restore_period"].AsInt / 1000;
        GameData.LuckyShotCoolDown = data["d"]["lucky_shot"]["rocket_restore_period"].AsInt / 1000;
        GameData.VersionConfig = data["d"]["version"].AsInt;
    }
    #endregion
    #region Shop
    static void GetConfigShop()
    {
        new JSONClass()
        {
            { "id", ServerRequest._CONFIG_SHOP.ToJson() },
            { "v", new JSONData(GameData.VersionShopConfig) },
        }.RequestServer();
    }
    static void GetConfigShop(JSONNode data)
    {
        if (data["d"]["version"].AsInt == GameData.VersionShopConfig)
            return;

        GameData.TransactionConfigs = new Dictionary<TransactionType, List<TransactionInfo>>();
        Array @enum = Enum.GetValues(typeof(TransactionType));
        for (int i = 0; i < @enum.Length; i++)
        {
            string name = ((TransactionType)i == TransactionType.starter2 || (TransactionType)i == TransactionType.elite2) ? @enum.GetValue(i).ToString().Substring(0, @enum.GetValue(i).ToString().Length - 1) : @enum.GetValue(i).ToString();
            if (i > 0)
            {
                GameData.TransactionConfigs.Add((TransactionType)i, TransactionInfo.ListFromJson(data["d"]["shops"][name], i));
            }
            else
            {
                List<TransactionInfo> infos = new List<TransactionInfo>
                {
                    TransactionInfo.FromJson(data["d"]["shops"][name], i, 0)
                };
                GameData.TransactionConfigs.Add((TransactionType)i, infos);
            }
        }

        GameData.VersionShopConfig = data["d"]["version"].AsInt;
    }
    public static void Transaction(JSONNode data)
    {
        if (data["e"].AsInt != 0)
        {
            return;
        }

        TransactionType id = data["d"]["s"].ToEnum<TransactionType>();
        int index = data["d"]["p"].AsInt;
        GameData.TransactionConfigs[id][index].Transact();
        FirebaseIntegration.AnalyticsHelper.Transaction(id.ToString() + index.ToString());

        SoundType.RPCONFIRM.PlaySound();
    }
    #endregion
    #region Achievement
    static void GetConfigAchievement()
    {
        new JSONClass()
        {
            { "id", ServerRequest._CONFIG_ACHIEVEMENT.ToJson() },
            { "v", new JSONData(GameData.VersionAchievementConfig) },
        }.RequestServer();
    }
    static void GetConfigAchievement(JSONNode data)
    {
        if (data["d"]["version"].AsInt != GameData.VersionAchievementConfig)
        {
            GameData.AchievementConfig = new Dictionary<AchievementType, AchievementInfo>();
            GameData.VersionAchievementConfig = data["d"]["version"].AsInt;
            for (int i = 0; i < data["d"]["achievements"].Count; i++)
            {
                GameData.AchievementConfig.Add((AchievementType)i, AchievementInfo.FromJson(data["d"]["achievements"][i], i));
            }
        }
        for (int i = 0; i < GameData.AchievementConfig.Count; i++)
        {
            int _i = i;
            ((StatisticType)_i).AddListenerOnProgress((oValue, nValue) =>
            {
                int oProgress = GameData.Player.AchievementProgress[_i];
                AchievementType type = (AchievementType)_i;
                if (type == AchievementType.WIN_STREAK_MAX)
                {
                    GameData.Player.AchievementProgress[_i] = (nValue);
                }
                else
                {
                    GameData.Player.AchievementProgress[_i] += (nValue - oValue);
                }
                Debug.Log("Achievement Progress _ " + type + "_" + GameData.Player.AchievementProgress[_i]);
                int nextMilestone = 0;
                for (int j = 0; j < GameData.AchievementConfig[type].AchivementUnits.Length; j++)
                {
                    if (oProgress >= GameData.AchievementConfig[type].AchivementUnits[j].Task)
                    {
                        nextMilestone++;
                    }
                }
                if (nextMilestone < GameData.AchievementConfig[type].AchivementUnits.Length &&
                GameData.Player.AchievementProgress[_i] >= GameData.AchievementConfig[type].AchivementUnits[nextMilestone].Task
                && oProgress < GameData.AchievementConfig[type].AchivementUnits[nextMilestone].Task)
                {
                    PopupHelper.CreateMessage(PrefabFactory.PospupQuestCompleted, null, AchievementInfo.GetDescription(type, GameData.AchievementConfig[type].AchivementUnits[nextMilestone].Task), null);
                    ConditionalMono.UpdateObject(typeof(AchievementReminder));
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
            { "v",  new JSONData(AdsData.VersionAds)}
        }.RequestServer();
    }
    public static void ReceiveAdsConfig(JSONNode data)
    {
        if (int.Parse(data["d"]["version"]) != AdsData.VersionAds)
        {
            AdsData.VersionAds = int.Parse(data["d"]["version"]);

            int platform;
#if PLATFORM_ANDROID || UNITY_ANDROID
            platform = 2;
#else
            platform = 1;
#endif

            AdsData.AdsUnitIdMap.Clear();
            AdsData.RewardTypeToConfigMap.Clear();
            for (int i = 0; i < data["d"]["ad_unit"].Count; i++)
            {
                if (int.Parse(data["d"]["ad_unit"][i]["platform"]) != platform)
                    continue;
                AdsData.AdsUnitIdMap.Add((RewardType)int.Parse(data["d"]["ad_unit"][i]["reward_type"][0]), data["d"]["ad_unit"][i]["ad_unit_id"]);
                string key = data["d"]["ad_unit"][i]["ad_unit_id"];
                AdsRewardConfig value = new AdsRewardConfig();
                value.reward = data["d"]["ad_unit"][i]["reward"].ToListInt();
                value.rewardAdUnitId = data["d"]["ad_unit"][i]["ad_unit_id"];
                AdsData.RewardTypeToConfigMap.Add(key, value);
            }
            AdsData.VersionAds = int.Parse(data["d"]["version"]);

        }

        AdsManager.adsManager.Initialize();
    }

    public static void ReceiveRewardAds(JSONNode data)
    {
        string ads_unit_id = data["d"]["a"];
        if (String.Equals(ads_unit_id, AdsData.AdsUnitIdMap[RewardType.Get_Beri]))
        {
            //PConsumableType.BERRY.SetValue(int.Parse(data["d"]["g"]));
        }
        else if (String.Equals(ads_unit_id, AdsData.AdsUnitIdMap[RewardType.Get_Rocket]))
        {
            GameData.RocketCount.Data = int.Parse(data["d"]["l"]["r"]);
        }
        else if (String.Equals(ads_unit_id, AdsData.AdsUnitIdMap[RewardType.Get_Quest]))
        {
            //Debug.Log("AddQuest");
            //GameData.RoyalPass.CurrentQuestsProgress = data["d"]["q"]["p"].ToArrayInt(true);
            //GameData.RoyalPass.CurrentQuests.Data = data["d"]["q"]["q"].ToArrayInt(true);
        }
        else if (String.Equals(ads_unit_id, AdsData.AdsUnitIdMap[RewardType.Change_Quest]))
        {
            //int[] arr = new int[3];
            //for (int i = 0; i < arr.Length; i++)
            //{
            //    if (i == int.Parse(data["d"]["i"]))
            //    {
            //        arr[i] = int.Parse(data["d"]["n"]);
            //        continue;
            //    }
            //    arr[i] = GameData.RoyalPass.CurrentQuests.Data[i];
            //}
            //GameData.RoyalPass.CurrentQuestsProgress[int.Parse(data["d"]["i"])] = int.Parse(data["d"]["q"]["p"][int.Parse(data["d"]["i"])]);
            //GameData.RoyalPass.CurrentQuests.Data = arr;
        }
        else if (String.Equals(ads_unit_id, AdsData.AdsUnitIdMap[RewardType.Get_X2DailyGift]))
        {
            //Gift.OnGetAdsGift(data);
        }
        else if (String.Equals(ads_unit_id, AdsData.AdsUnitIdMap[RewardType.Get_RevivalOnlyPVE]))
        {
            PVEData.TypeBoard = int.Parse(data["d"]["t"]["t"]);
            PVE.Instance.CurrentStep.Data = int.Parse(data["d"]["t"]["s"]);
            PVEData.IsDeadPlayer.Data = int.Parse(data["d"]["t"]["d"]) == 1 ? true : false;
            PVE.Instance.IsRevived = int.Parse(data["d"]["t"]["s"]) == 1 ? true : false;
        }
    }
    #endregion
    #region Lucky Shot
    public static void LuckyShotEarn(JSONNode data)
    {
        GameData.RocketCount.Data = data["d"]["l"]["r"].AsInt;
        Timer<LuckyShot>.Instance.Begin(); //= data["d"]["l"]["t"].AsLong.NowFrom0001From1970();
        ConditionalMono.UpdateObject(typeof(LuckyShotReminder));
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
    #region Old
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
        for (int i = 0; i < data["list"].Count; i++)
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
        if (data["e"].AsInt == 0)
        {
            CoreGame.reconnect = data["d"];
            LoadingScene.Instance.LoadScene("MainGame");
        }
        //SceneTransitionHelper.Load(ESceneName.MainGame);
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
            { "v", new JSONData(GameData.VersionRoyalPassConfig) },
        }.RequestServer();
    }
    static void GetConfigRoyalPass(JSONNode data)
    {
        if (data["d"]["version"].AsInt == GameData.VersionRoyalPassConfig)
            return;

        RoyalPass.ConfigFromJson(GameData.RoyalPass, data["d"]);
        GameData.RoyalPass.Version = data["v"].AsInt;
        GetDataRoyalPass();

        GameData.VersionRoyalPassConfig = data["d"]["version"].AsInt;
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
        if (json["d"]["e"].AsInt == 0)
        {
            PopupHelper.Create(PrefabFactory.PopupRPConfirm);
            SoundType.RPCONFIRM.PlaySound();
            int[] receive = new int[3] { GameData.RoyalPass.CurrentQuests.Data[0], GameData.RoyalPass.CurrentQuests.Data[1], GameData.RoyalPass.CurrentQuests.Data[2] };
            receive[json["d"]["i"].AsInt] = -1;
            GameData.RoyalPass.CurrentQuests.Data = receive;
            GameData.RoyalPass.Point.Data += json["d"]["r"].AsInt;
            ConditionalMono.UpdateObject(typeof(RoyalPassReminder));
        }

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
        if (json["d"]["e"].AsInt == 0)
        {
            PopupHelper.Create(PrefabFactory.PopupRPConfirm);
            SoundType.RPCONFIRM.PlaySound();

            GameData.RoyalPass.Point.Data += json["d"]["r"].AsInt;
            HashSet<int> receive = new HashSet<int>();

            receive.AddRange(GameData.RoyalPass.SeasonQuestsObtained.Data);
            receive.Add(json["d"]["i"].AsInt);
            GameData.RoyalPass.SeasonQuestsObtained.Data = receive;
            ConditionalMono.UpdateObject(typeof(RoyalPassReminder));
        }
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

        SoundType.RPCONFIRM.PlaySound();

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
            if (goods.Count > 0)
            {
                if (PNonConsumableType.ELITE.GetValue().Contains(0))
                {
                    PopupHelper.CreateGoods(PrefabFactory.PopupGood, "", goods);

                }
                else
                {
                    PopupHelper.CreateGoods(PrefabFactory.PopupRPGood, "Unlock Elite now to receive", goods);

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
        ConditionalMono.UpdateObject(typeof(RoyalPassReminder));
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
            { "v", new JSONData(GameData.VersionGiftConfig) },
        }.RequestServer();
    }
    public static void GetConfigGift(JSONNode data)
    {
        if (data["d"]["version"].AsInt == GameData.VersionGiftConfig) return;

        GameData.GiftConfig = data["d"]["gold"].ToListInt();
        Timer<Gift>.Instance.TriggerInterval_Sec = data["d"]["bonus_period"].AsInt / 1000;
        GameData.GiftCoolDown = Timer<Gift>.Instance.TriggerInterval_Sec;
        GameData.VersionGiftConfig = data["d"]["version"].AsInt;
    }
    public static void GetGift()
    {
        new JSONClass()
        {
            { "id", ServerRequest._GIFT.ToJson() },
        }.RequestServer();
    }
    #endregion
    #region LeaderBoard
    public static void LeaderBoardConfig()
    {
        new JSONClass
        {
            { "id", ServerRequest._LEADERBOARD_CONFIG.ToJson() },
            { "v", new JSONData(GameData.VersionLeaderBoardConfig) },
        }.RequestServer();
    }
    public static void LeaderBoardConfig(JSONNode data)
    {
        if (data["e"].AsInt == 0)
        {
            if (data["d"]["version"].AsInt != GameData.VersionLeaderBoardConfig)
            {
                GameData.LeaderBoard.Period = data["d"]["leader_period"].AsInt / 1000;
                Timer<LeaderBoard>.Instance.TriggerInterval_Sec = GameData.LeaderBoard.Period;
                GameData.LeaderBoard.goldReward = new List<int>();
                GameData.LeaderBoard.winReward = new List<int>();
                for (int i = 0; i < data["d"]["reward_gold"].Count; i++)
                {
                    GameData.LeaderBoard.goldReward.Add(data["d"]["reward_gold"][i].AsInt);
                }
                for (int i = 0; i < data["d"]["reward_win"].Count; i++)
                {
                    GameData.LeaderBoard.winReward.Add(data["d"]["reward_win"][i].AsInt);
                }

                GameData.VersionLeaderBoardConfig = data["d"]["version"].AsInt;
            }
        }

        if (CoreGame.reconnect == null)
        {
            if (SceneManager.GetActiveScene().name == "PreHome")
            {
                SceneTransitionHelper.Load(ESceneName.Home);
            }
            else if (SceneManager.GetActiveScene().name == "Loading")
            {
                LoadingScene.Instance.LoadScene("Home");
            }
        }
    }
    public static void LeaderBoardData()
    {
        new JSONClass
        {
            { "id", ServerRequest._LEADER_BOARD_DATA.ToJson() },
        }.RequestServer();
    }
    public static void LeaderData()
    {
        new JSONClass
        {
            { "id", ServerRequest._LEADER_DATA.ToJson() },
        }.RequestServer();
    }
    public static void LeaderBoardData(JSONNode data)
    {
        Timer<LeaderBoard>.Instance.TriggerInterval_Sec = GameData.LeaderBoard.Period;
        Timer<LeaderBoard>.Instance.BeginPoint = data["d"]["s"].AsLong.NowFrom0001From1970();
        GameData.LeaderBoard.goldInfos = new List<LeaderBoardGoldInfo>();
        GameData.LeaderBoard.winInfos = new List<LeaderBoardWinInfo>();
        for (int i = 0; i < data["d"]["g"].Count; i++)
        {
            GameData.LeaderBoard.goldInfos.Add(new LeaderBoardGoldInfo()
            {
                UserId = data["d"]["g"][i]["u"].AsInt,
                Order = i,
                Rank = data["d"]["g"][i]["e"].AsInt,
                Reward = GameData.LeaderBoard.goldReward[i],
                SpendingCount = data["d"]["g"][i]["g"].AsInt,
                UserName = data["d"]["g"][i]["n"],

            });
        }
        for (int i = 0; i < data["d"]["w"].Count; i++)
        {
            GameData.LeaderBoard.winInfos.Add(new LeaderBoardWinInfo()
            {
                UserId = data["d"]["w"][i]["u"].AsInt,
                Order = i,
                Rank = data["d"]["w"][i]["e"].AsInt,
                Reward = GameData.LeaderBoard.winReward[i],
                WinCount = data["d"]["w"][i]["w"].AsInt,
                UserName = data["d"]["w"][i]["n"],

            });
        }
    }

    public static void LeaderData(JSONNode data)
    {
        GameData.LeaderBoard.win = data["d"]["w"].AsInt;
        GameData.LeaderBoard.goldSpend = data["d"]["g"].AsInt;
        GameData.Player.Point = data["d"]["e"].AsInt;
        if (data["d"]["t"]["g"].AsObject != null)
        {
            GameData.LeaderBoard.rankGoldSpendPrevious = data["d"]["t"]["g"]["r"].AsInt;
            GameData.LeaderBoard.rankGoldSpendPreviousAvailable = data["d"]["t"]["g"]["t"].AsInt == 1;
        }
        if (data["d"]["t"]["w"].AsObject != null)
        {
            GameData.LeaderBoard.rankWinPrevious = data["d"]["t"]["w"]["r"].AsInt;
            GameData.LeaderBoard.rankWinPreviousAvailable = data["d"]["t"]["w"]["t"].AsInt == 1;
        }

    }
    #endregion
}
