using Framework;
using Monetization;
using Server;
using SimpleJSON;
using System.Collections.Generic;

public class QuestCollection : CardCollectionBase<QuestInfo>
{
    private void Awake()
    {
        UpdateUIs();
        GameData.RoyalPass.CurrentQuests.OnDataChanged += OnChange;

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
                    Description = RoyalPass.GetDescription(GameData.RoyalPass.Quests[GameData.RoyalPass.CurrentQuests.Data[i]].Type, GameData.RoyalPass.Quests[GameData.RoyalPass.CurrentQuests.Data[i]].Require),
                    Obtained = false,
                    OnCollect = (info) =>
                    {
                        if (GameData.RoyalPass.CurrentQuestsProgress[_i] > GameData.RoyalPass.Quests[GameData.RoyalPass.CurrentQuests.Data[_i]].Require)
                        {
                            WSClientHandler.DailyQuestReward(_i);
                        }
                    },
                    OnChange = (info) =>
                    {
                        AdsManager.ShowRewardAds(null, AdsData.adsUnitIdMap[RewardType.Change_Quest], _i.ToString());
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
}
