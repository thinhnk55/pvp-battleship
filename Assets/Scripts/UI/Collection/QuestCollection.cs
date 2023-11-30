using FirebaseIntegration;
using Framework;
using Monetization;
using Server;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestCollection : CardCollectionBase<QuestInfo>
{
    [SerializeField] PopupBehaviour popupRoyalPass;
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
            if (GameData.RoyalPass.CurrentQuests.Data[_i] > -1)
            {
                infos.Add(new QuestInfo()
                {
                    Id = GameData.RoyalPass.CurrentQuests.Data[_i],
                    Require = GameData.RoyalPass.Quests[GameData.RoyalPass.CurrentQuests.Data[_i]].Require,
                    Progress = GameData.RoyalPass.CurrentQuestsProgress[_i],
                    Reward = GameData.RoyalPass.Quests[GameData.RoyalPass.CurrentQuests.Data[_i]].Reward,
                    Description = RoyalPass.GetDescription((StatisticType)GameData.RoyalPass.Quests[GameData.RoyalPass.CurrentQuests.Data[_i]].Type, GameData.RoyalPass.Quests[GameData.RoyalPass.CurrentQuests.Data[_i]].Require),
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
                                popupRoyalPass.ForceClose();
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

        try
        {
            var sortedInfos = infos
                .OrderByDescending(info => info.Progress / info.Require < 1)
                .ThenByDescending(info => (float)info.Progress / info.Require)
                .ToList();
            if (sortedInfos.Count < 3)
            {
                sortedInfos.Add(new QuestInfo()
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
            BuildUIs(sortedInfos);
        }
        catch (Exception ex)
        {
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
            Debug.LogError("An exception occurred: " + ex.Message);
        }

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
        else if (String.Equals(adsUnitId, AdsData.AdsUnitIdMap[RewardType.Change_Quest]))
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
