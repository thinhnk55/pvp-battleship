using Framework;
using System;
using UnityEngine;

public class RoyalPassReminder : ConditionalMono
{
    [SerializeField] bool isReward;
    [SerializeField] bool isQuest;

    protected override Predicate<object> SetCondition()
    {
        return (o) =>
        {
            try
            {
                bool hasReward = false;
                for (int i = 0; i <= GameData.RoyalPass.Level; i++)
                {
                    if (!GameData.RoyalPass.NormalObtains.Data.Contains(i) && i != 0)
                    {
                        hasReward = true;
                        break;
                    }
                    if (PNonConsumableType.ELITE.GetValue().Contains(0) && !GameData.RoyalPass.EliteObtains.Data.Contains(i))
                    {
                        hasReward = true;
                        break;
                    }
                }
                bool hasQuest = false;
                for (int i = 0; i < GameData.RoyalPass.SeasonQuests.Length; i++)
                {
                    if (GameData.RoyalPass.SeasonQuestsProgress[i] >= GameData.RoyalPass.SeasonQuests[i].Require && !GameData.RoyalPass.SeasonQuestsObtained.Data.Contains(i))
                    {
                        hasQuest = true;
                        break;
                    }
                }
                for (int i = 0; i < GameData.RoyalPass.CurrentQuestsProgress.Length; i++)
                {
                    if (GameData.RoyalPass.CurrentQuests.Data[i] >= 0
                        && GameData.RoyalPass.CurrentQuestsProgress[i] >= GameData.RoyalPass.Quests[GameData.RoyalPass.CurrentQuests.Data[i]].Require)
                    {
                        hasQuest = true;
                        break;
                    }
                }
                return (isReward && hasReward) || (isQuest && hasQuest);
            }
            catch (Exception)
            {
                return false;
            }

        };
    }
}
