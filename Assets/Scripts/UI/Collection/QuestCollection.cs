using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCollection : CardCollectionBase<QuestInfo>
{
    private void Awake()
    {
        UpdateUIs();
    }
    public override void UpdateUIs()
    {
        List<QuestInfo> infos = new List<QuestInfo>();
        for (int i = 0; i < GameData.RoyalPass.CurrentQuests.Data.Length; i++)
        {
            infos.Add(new QuestInfo()
            {
                Id = GameData.RoyalPass.CurrentQuests.Data[i],
                Require = GameData.RoyalPass.Quests[GameData.RoyalPass.CurrentQuests.Data[i]].Require,
                Progress = GameData.RoyalPass.CurrentQuestsProgress[i],
                Reward = GameData.RoyalPass.Quests[GameData.RoyalPass.CurrentQuests.Data[i]].Reward,
                Description = RoyalPass.GetDescription(GameData.RoyalPass.Quests[GameData.RoyalPass.CurrentQuests.Data[i]].Type, GameData.RoyalPass.Quests[GameData.RoyalPass.CurrentQuests.Data[i]].Require),
                OnCollect = (info) =>
                {
                    WSClient.RequestSeasonQuest(info.Id);
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
