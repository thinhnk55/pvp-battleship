using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonQuestCollection : CardCollectionBase<QuestInfo>
{
    private void Awake()
    {
        UpdateUIs();
    }
    public override void UpdateUIs()
    {
        List<QuestInfo> infos = new List<QuestInfo>();
        for (int i = 0; i < GameData.RoyalPass.SeasonQuests.Length; i++)
        {
            infos.Add(new QuestInfo()
            {
                Id = i,
                Require = GameData.RoyalPass.SeasonQuests[i].Require,
                Progress = GameData.RoyalPass.SeasonQuestsProgress[i],
                Reward = GameData.RoyalPass.SeasonQuests[i].Reward,
                Description = RoyalPass.GetDescription(GameData.RoyalPass.SeasonQuests[i].Type, GameData.RoyalPass.SeasonQuests[i].Require),
                OnCollect = (info) =>
                {
                    WSClient.RequestSeasonQuest(info.Id);
                },
                OnChange = (info) =>
                {

                }
            });
        }
        BuildUIs(infos);
    }
    public override void BuildUIs(List<QuestInfo> infos)
    {
        base.BuildUIs(infos);
    }
}
