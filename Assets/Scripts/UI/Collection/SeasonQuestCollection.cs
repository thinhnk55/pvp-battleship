using Framework;
using System.Collections.Generic;

public class SeasonQuestCollection : CardCollectionBase<QuestInfo>
{
    private void Awake()
    {
        UpdateUIs();
        GameData.RoyalPass.SeasonQuestsObtained.OnDataChanged += OnChange;

    }

    private void OnChange(HashSet<int> arg1, HashSet<int> arg2)
    {
        UpdateUIs();
    }

    private void OnDestroy()
    {
        GameData.RoyalPass.SeasonQuestsObtained.OnDataChanged -= OnChange;
    }

    public override void UpdateUIs()
    {
        List<QuestInfo> infos = new List<QuestInfo>();
        for (int i = 0; i < GameData.RoyalPass.SeasonQuests.Length; i++)
        {
            int _i = i;
            infos.Add(new QuestInfo()
            {
                Id = i,
                Require = GameData.RoyalPass.SeasonQuests[i].Require,
                Progress = GameData.RoyalPass.SeasonQuestsProgress[i],
                Reward = GameData.RoyalPass.SeasonQuests[i].Reward,
                Description = RoyalPass.GetDescription((StatisticType)GameData.RoyalPass.SeasonQuests[i].Type, GameData.RoyalPass.SeasonQuests[i].Require),
                Obtained = GameData.RoyalPass.SeasonQuestsObtained.Data.Contains(i),
                OnCollect = (info) =>
                {
                    if (GameData.RoyalPass.SeasonQuestsProgress[_i] >= GameData.RoyalPass.SeasonQuests[_i].Require)
                    {
                        WSClientHandler.SeasonQuestReward(info.Id);
                    }
                },

            });
        }
        BuildUIs(infos);
    }
    public override void BuildUIs(List<QuestInfo> infos)
    {
        base.BuildUIs(infos);
    }
}
