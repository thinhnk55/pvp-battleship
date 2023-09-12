using Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct LeaderBoardWinInfo
{
    public int Order;
    public int Rank;
    public string UserName;
    public int WinCount;
    public int Reward;
}

public class LeaderBoardWinCard : CardBase<LeaderBoardWinInfo>
{
    [SerializeField] TextMeshProUGUI order;
    [SerializeField] Image orderIcon;
    [SerializeField] Image rank;
    [SerializeField] TextMeshProUGUI userName;
    [SerializeField] TextMeshProUGUI winCount;
    [SerializeField] Image reward;
    protected override void OnClicked(LeaderBoardWinInfo info)
    {

    }

    public override void BuildUI(LeaderBoardWinInfo info)
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
        winCount?.SetText(info.WinCount.ToString());
        reward?.SetSprite(SpriteFactory.ResourceIcons[(int)PConsumableType.BERRY].sprites.GetClamp(info.Order));
    }
}
