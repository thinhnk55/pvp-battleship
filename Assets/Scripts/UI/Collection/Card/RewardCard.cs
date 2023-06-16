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
    public GoodType GoodType;
    public TransactionItemInfo Reward;
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
        if (Background)
            Background.sprite = info.Background;
        if (Icon)
            Icon.sprite = info.Reward.Icon;
        if (ObtainedIndicator)
            ObtainedIndicator.sprite = info.ObtainedIndicator;
        if (Title)
        Title.text = info.Title;
        if (Amount)
            Amount.text = info.Reward.Amount.ToString();
    }

    protected override void OnClicked(RewardInfo info)
    {
        throw new System.NotImplementedException();
    }
}
