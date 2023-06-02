using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public struct AchievementInfo
{
    public string Title;
    public string Description;
    public TransactionItemInfo Reward;
}

public class AchievementCard : CardBase<AchievementInfo>
{
    public Image Icon ;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI RewardAmount;

    public override void BuildUI(AchievementInfo info)
    {
        base.BuildUI(info);
        if (Icon != null)
            Icon.sprite = info.Reward.Icon;
        if (Title)
            Title.text = info.Title;
        if (RewardAmount != null)
            RewardAmount.text = info.Reward.Amount.ToString();
    }

    protected override void OnClicked(AchievementInfo info)
    {
        throw new System.NotImplementedException();
    }
}
