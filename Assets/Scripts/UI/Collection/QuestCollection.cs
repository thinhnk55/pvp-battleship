using FirebaseIntegration;
using Framework;
using Monetization;
using Server;
using SimpleJSON;
using System;
using System.Collections.Generic;

public class QuestCollection : CardCollectionBase<QuestInfo>
{
    private void Awake()
    {
        UpdateUIs();
        GameData.RoyalPass.CurrentQuests.OnDataChanged += OnChange;

    }
    private void OnEnable()
    {
        ServerMessenger.AddListener<JSONNode>(ServerResponse._REWARD_ADS, ReceiveRewardAds);
        AnalyticsHelper.SelectContent("royalpass_normal_quest");
    }

    private void OnDisable()
    {
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._REWARD_ADS, ReceiveRewardAds);
    }

    private void OnDestroy()
    {
        GameData.RoyalPass.CurrentQuests.OnDataChanged -= OnChange;
    }

    private void OnChange(int[] arg1, int[] arg2)
    {
        UpdateUIs();
    }

    public override void UpdateUIs()
    {
        List<QuestInfo> infos = new List<QuestInfo>();
        for (int i = 0; i < GameData.RoyalPass.CurrentQuests.Data.Length; i++)
        {
            int _i = i;
            if (GameData.RoyalPass.CurrentQuests.Data[i] > -1)
            {
                infos.Add(new QuestInfo()
                {
                    Id = GameData.RoyalPass.CurrentQuests.Data[i],
                    Require = GameData.RoyalPass.Quests[GameData.RoyalPass.CurrentQuests.Data[i]].Require,
                    Progress = GameData.RoyalPass.CurrentQuestsProgress[i],
                    Reward = GameData.RoyalPass.Quests[GameData.RoyalPass.CurrentQuests.Data[i]].Reward,
                    Description = RoyalPass.GetDescription((StatisticType)GameData.RoyalPass.Quests[GameData.RoyalPass.CurrentQuests.Data[i]].Type, GameData.RoyalPass.Quests[GameData.RoyalPass.CurrentQuests.Data[i]].Require),
                    Obtained = false,
                    OnCollect = (info) =>
                    {
                        if (GameData.RoyalPass.CurrentQuestsProgress[_i] >= GameData.RoyalPass.Quests[GameData.RoyalPass.CurrentQuests.Data[_i]].Require)
                        {
                            WSClientHandler.DailyQuestReward(_i);
                        }
                        else
                        {
                            if (GameData.RoyalPass.Quests[GameData.RoyalPass.CurrentQuests.Data[_i]].Type == StatisticType.LUCKY_SHOT_COUNT)
                            {
                                PopupHelper.Create(PrefabFactory.PopupLuckyshot);
                            }
                            else
                            {
                                SceneTransitionHelper.Load(ESceneName.Bet);
                            }
                        }
                    },
                    OnChange = (info) =>
                    {
                        AdsManager.ShowRewardAds(null, AdsData.AdsUnitIdMap[RewardType.Change_Quest], _i.ToString());
                    }
                });
            }
        }
        if (infos.Count < 3)
        {
            infos.Add(new QuestInfo()
            {
                Id = -1,
                Require = 0,
                Progress = 0,
                Reward = 0,
                Description = "",
                Obtained = false,
                OnCollect = null,
                OnChange = null
            });
        }
        BuildUIs(infos);
    }
    public override void BuildUIs(List<QuestInfo> infos)
    {
        base.BuildUIs(infos);
    }

    private void RequestChangeQuest(int index)
    {
        new JSONClass()
        {
            { "id", ServerResponse._RP_CHANGE_QUEST.ToJson() },
            { "index", new JSONData(index)},
        }.RequestServer();
    }

    private void ReceiveRewardAds(JSONNode data)
    {
        string adsUnitId = data["d"]["a"];
        if (String.Equals(adsUnitId, AdsData.AdsUnitIdMap[RewardType.Get_Quest]))
        {
            GameData.RoyalPass.CurrentQuestsProgress = data["d"]["q"]["p"].ToArrayInt(false);
            GameData.RoyalPass.CurrentQuests.Data = data["d"]["q"]["q"].ToArrayInt(false);
        }
        else if(String.Equals(adsUnitId, AdsData.AdsUnitIdMap[RewardType.Change_Quest]))
        {
            int[] arr = new int[3];
            for (int i = 0; i < arr.Length; i++)
            {
                if (i == int.Parse(data["d"]["i"]))
                {
                    arr[i] = int.Parse(data["d"]["n"]);
                    continue;
                }
                arr[i] = GameData.RoyalPass.CurrentQuests.Data[i];
            }
            GameData.RoyalPass.CurrentQuestsProgress[int.Parse(data["d"]["i"])] = int.Parse(data["d"]["q"]["p"][int.Parse(data["d"]["i"])]);
            GameData.RoyalPass.CurrentQuests.Data = arr;
        }
    }
}
