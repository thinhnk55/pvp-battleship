using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiementCardSelection : AchievementCard
{
    public override void BuildUI(AchievementInfo info)
    {
        //base.BuildUI(info);
        Id = info.Id;
        int completed = 0;

        for (int i = 0; i < info.AchivementUnits.Length; i++)
        {
            if (info.Progress >= info.AchivementUnits[i].Task)
            {
                completed = i + 1;
            }
            else
                break;
        }


        //show unobtained info
        if (Icon != null)
            Icon.sprite = SpriteFactory.Achievements[info.Id].sprites[info.Obtained];
        if (Title)
            Title.text = info.Title;
        if (Description)
            Description.text = info.AchivementUnits[info.Obtained].Description;
        if (RewardAmount != null)
            RewardAmount.text = "x " + info.AchivementUnits[info.Obtained].RewardAmount.ToString();
        if (Progress != null)
        {
            Progress.maxValue = info.AchivementUnits[info.Obtained].Task;
            Progress.value = info.Progress;
        }
        if (TextProgress)
        {
            TextProgress.text = info.Progress.ToString() + "/" + info.AchivementUnits[info.Obtained].Task.ToString();
        }
        if (Button)
        {
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(() =>
            {
                ((AchievementSelectionCollection)Collection).Select(transform.GetSiblingIndex());
            });
        }


    }
}
