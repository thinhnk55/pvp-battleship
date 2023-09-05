using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameCard : TransactionCard
{
    public override void BuildUI(TransactionInfo info)
    {
        base.BuildUI(info);
        if (Button)
        {
            if (PNonConsumableType.AVATAR_FRAME.GetValue().Contains(info.Index))
            {
                Button.onClick.RemoveAllListeners();
                Button.onClick.AddListener(() =>
                {
                    WSClientHandler.ChangeFrame(info.Index);
                });
            }
        }
        if (info.Cost[0].Value < 0)
        {
            otherText.text = "S" + GameData.RoyalPass.Season.ToString();
        }
    }
    protected override string GetStatus(TransactionInfo info)
    {
        if (info.Index == GameData.Player.FrameAvatar.Data)
        {
            return "Using";
        }
        else
        {
            return "Choose";
        }
    }
}
