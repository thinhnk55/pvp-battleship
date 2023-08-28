using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct SpendingCountInfo
{
    public int Order;
    public int Rank;
    public string UserName;
    public int SpendingCount;
    public int Reward;
}

public class SpendingPlayerCount : CardBase<SpendingCountInfo>
{
    [SerializeField] TextMeshProUGUI order;
    [SerializeField] Image rank;
    [SerializeField] TextMeshProUGUI userName;
    [SerializeField] TextMeshProUGUI spendingCount;
    [SerializeField] Image reward;
    protected override void OnClicked(SpendingCountInfo info)
    {

    }

    public override void BuildUI(SpendingCountInfo info)
    {
        base.BuildUI(info);

        order.SetText(info.Order.ToString());
        rank.sprite = SpriteFactory.Ranks[info.Rank];
        userName.SetText(info.UserName);
        //cup =  info.Cup;
        spendingCount.SetText(info.SpendingCount.ToString());
        reward.sprite = SpriteFactory.RewardLeaderBoard[info.Reward];


    }
}
