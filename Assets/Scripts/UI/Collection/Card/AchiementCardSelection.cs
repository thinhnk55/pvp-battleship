using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiementCardSelection : AchievementCard
{
    public override void BuildUI(AchievementInfo info)
    {
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

        int obtain = Mathf.Clamp(GameData.Player.AchievementObtained[info.Id], 0, 4);

        //show unobtained info
        if (Icon != null)
            Icon.sprite = info.AchivementUnits[obtain].Icon;
        if (Title)
            Title.text = info.Title;
        if (Description)
            Description.text = info.AchivementUnits[obtain].Description;
        if (RewardAmount != null)
            RewardAmount.text = "x " + info.AchivementUnits[obtain].RewardAmount.ToString();
        if (Progress != null)
        {
            Progress.maxValue = info.AchivementUnits[obtain].Task;
            Progress.value = info.Progress;
        }
        if (TextProgress)
        {
            TextProgress.text = info.Progress.ToString() + "/" + info.AchivementUnits[obtain].Task.ToString();
        }
        if (Button)
        {
            Button.onClick.RemoveAllListeners();
            Debug.Log(transform.GetSiblingIndex());
            Button.onClick.AddListener(() =>
            {
                Debug.Log(transform.GetSiblingIndex());
                ((AchievementSelectionCollection)collection).Select(transform.GetSiblingIndex());
            });
        }


    }
}
