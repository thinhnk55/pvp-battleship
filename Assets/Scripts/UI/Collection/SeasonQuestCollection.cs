using FirebaseIntegration;
using Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeasonQuestCollection : CardCollectionBase<QuestInfo>
{
    private void Awake()
    {
        UpdateUIs();
        GameData.RoyalPass.SeasonQuestsObtained.OnDataChanged += OnChange;

    }
    private void OnEnable()
    {
        AnalyticsHelper.SelectContent("royalpass_season_quest");
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
                    else
                    {
                        if (GameData.RoyalPass.SeasonQuests[_i].Type == StatisticType.LUCKY_SHOT_COUNT)
                        {
                            PopupHelper.Create(PrefabFactory.PopupLuckyshot);
                        }
                        else
                        {
                            SceneTransitionHelper.Load(ESceneName.Bet);
                        }
                    }

                },

            });
        }
        
        try 
        {
            var sortedInfos = infos
                .OrderByDescending(info => info.Progress / info.Require == 1 && !info.Obtained)
                .ThenByDescending(info => info.Progress / info.Require < 1)
                .ThenByDescending(info => (float)info.Progress / info.Require)
                .ToList();
            BuildUIs(sortedInfos);  
        }
        catch(System.Exception ex)
        {
            BuildUIs(infos);
            Debug.LogError("An exception occurred: " + ex.Message);
        }
        

    }
    public override void BuildUIs(List<QuestInfo> infos)
    {
        base.BuildUIs(infos);
    }
}
