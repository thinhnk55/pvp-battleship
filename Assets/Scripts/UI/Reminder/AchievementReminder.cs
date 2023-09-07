using Framework;
using System;
using UnityEngine;

public class AchievementReminder : ConditionalMono
{
    protected override Predicate<object> SetCondition()
    {
        return (o) =>
        {
            for (int i = 0; i < GameData.AchievementConfig.Count; i++)
            {
                AchievementType type = (AchievementType)i;
                int progress = GameData.Player.AchievementProgress[i];
                if (GameData.Player.AchievementObtained[i] < GameData.AchievementConfig[type].AchivementUnits.Length &&
                GameData.Player.AchievementProgress[i] >= GameData.AchievementConfig[type].AchivementUnits[GameData.Player.AchievementObtained[i]].Task)
                {
                    return true;
                }
            }
            return false;
        };
    }
    protected override void Start()
    {
        base.Start();

    }
}
