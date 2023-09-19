using Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct LeaderBoardGoldInfo
{
    public int UserId;
    public int Order;
    public int Rank;
    public string UserName;
    public int SpendingCount;
    public int Reward;
}

public class LeaderBoardGoldCard : CardBase<LeaderBoardGoldInfo>
{
    [SerializeField] TextMeshProUGUI order;
    [SerializeField] Image orderIcon;
    [SerializeField] Image rank;
    [SerializeField] TextMeshProUGUI userName;
    [SerializeField] TextMeshProUGUI spendingCount;
    [SerializeField] Image reward;
    protected override void OnClicked(LeaderBoardGoldInfo info)
    {

    }

    public override void BuildUI(LeaderBoardGoldInfo info)
    {
        base.BuildUI(info);
        if (orderIcon)
        {
            if (info.Order < 3)
            {
                order?.SetText("");
                orderIcon.sprite = SpriteFactory.OrderLeaderBoard[info.Order];
            }
            else
            {
                if (info.Order > 50)
                {
                    order?.SetText("51+");
                }
                else
                {
                    order?.SetText(info.Order.ToString());
                }
                orderIcon.SetAlpha(0);
            }
        }
        rank?.SetSprite(SpriteFactory.Ranks[info.Rank]);
        userName?.SetText(info.UserName);
        //cup =  info.Cup;
        spendingCount?.SetText(info.SpendingCount.ToString());
        reward?.SetSprite(SpriteFactory.ResourceIcons[(int)PConsumableType.BERRY].sprites[LeaderBoard.GetIconReward(info.Order)]);


    }
}
