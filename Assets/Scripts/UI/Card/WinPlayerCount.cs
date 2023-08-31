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
    [SerializeField] Image orderIcon;
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
        if (orderIcon)
        {
            if (info.Order <3)
            {
                order?.SetText("");
                orderIcon.sprite = SpriteFactory.OrderLeaderBoard[info.Order];
            }
            else
            {
                order?.SetText(info.Order.ToString());
                orderIcon.SetAlpha(0);
            }
        }
        rank?.SetSprite(SpriteFactory.Ranks[info.Rank]);
        userName?.SetText(info.UserName);
        //cup =  info.Cup;
        winCount?.SetText(info.WinCount.ToString());
        reward?.SetSprite(SpriteFactory.ResourceIcons[(int)PConsumableType.BERI].sprites.GetClamp(info.Order));
    }
}
