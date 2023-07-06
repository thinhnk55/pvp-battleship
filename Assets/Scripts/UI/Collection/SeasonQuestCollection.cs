using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            infos.Add(new QuestInfo()
            {
                Id = i,
                Require = GameData.RoyalPass.SeasonQuests[i].Require,
                Progress = GameData.RoyalPass.SeasonQuestsProgress[i],
                Reward = GameData.RoyalPass.SeasonQuests[i].Reward,
                Description = RoyalPass.GetDescription(GameData.RoyalPass.SeasonQuests[i].Type, GameData.RoyalPass.SeasonQuests[i].Require),
                Obtained = GameData.RoyalPass.SeasonQuestsObtained.Data.Contains(i),
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
