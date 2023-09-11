using Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct LeaderBoardGoldInfo
{
    public int Order;
    public int Rank;
    public string UserName;
    public int SpendingCount;
    public int Reward;
}

public class LeaderBoardGoldCard : CardBase<LeaderBoardGoldInfo>
{
    [SerializeField] TextMeshProUGUI order;
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

        order.SetText(info.Order.ToString());
        rank.sprite = SpriteFactory.Ranks[info.Rank];
        userName.SetText(info.UserName);
        //cup =  info.Cup;
        spendingCount.SetText(info.SpendingCount.ToString());
        reward.sprite = SpriteFactory.RewardLeaderBoard[info.Reward];


    }
}
