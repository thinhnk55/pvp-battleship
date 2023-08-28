using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct WinCountInfo
{
    public int Order;
    public int Rank;
    public string UserName;
    public int WinCount;
    public int Reward;
}

public class WinPlayerCount : CardBase<WinCountInfo>
{
    [SerializeField] TextMeshProUGUI order;
    [SerializeField] Image rank;
    [SerializeField] TextMeshProUGUI userName;
    [SerializeField] TextMeshProUGUI winCount;
    [SerializeField] Image reward;
    protected override void OnClicked(WinCountInfo info)
    {
        
    }

    public override void BuildUI(WinCountInfo info)
    {
        base.BuildUI(info);

        order.SetText(info.Order.ToString());
        rank.sprite = SpriteFactory.Ranks[info.Rank];
        userName.SetText(info.UserName);
        //cup =  info.Cup;
        winCount.SetText(info.WinCount.ToString());
        reward.sprite = SpriteFactory.RewardLeaderBoard[info.Reward];


    }
}
