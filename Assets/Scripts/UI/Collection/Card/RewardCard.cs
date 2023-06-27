using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct RewardInfo
{
    public Sprite Background;
    public Sprite ObtainedIndicator;
    public string Title;
    public PConsumableType GoodType;
    public GoodInfo Reward;
}

public class RewardCard : CardBase<RewardInfo>
{
    public Image Background;
    public Image Icon;
    public Image ObtainedIndicator;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Amount;
    public override void BuildUI(RewardInfo info)
    {
        base.BuildUI(info);
        if (Background)
            Background.sprite = info.Background;
        if (Icon)
            Icon.sprite = SpriteFactory.ResourceIcons[(int)info.Reward.Type].sprites.GetLoop((int)info.Reward.Value);
        if (ObtainedIndicator)
            ObtainedIndicator.sprite = info.ObtainedIndicator;
        if (Title)
            Title.text = info.Title;
        if (Amount)
            Amount.text = info.Reward.Value.ToString();
    }

    protected override void OnClicked(RewardInfo info)
    {
        throw new System.NotImplementedException();
    }
}
