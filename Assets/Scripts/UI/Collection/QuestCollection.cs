using Framework;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (GameData.RoyalPass.CurrentQuests.Data[i]>-1)
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
                        WSClient.RequestQuest(_i);
                    },
                    OnChange = (info) =>
                    {
                        RequestChangeQuest(_i);
                    }
                });
            }

        }
        BuildUIs(infos);
    }
    public override void BuildUIs(List<QuestInfo> infos)
    {
        base.BuildUIs(infos);
    }

    private void RequestChangeQuest(int index)
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_CHANGE_QUEST.ToJson() },
            { "index", new JSONData(index)},
        };
        WSClient.Instance.Send(jsonNode);
    }
}
