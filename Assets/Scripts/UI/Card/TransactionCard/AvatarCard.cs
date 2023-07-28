using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarCard : TransactionCard
{
    public override void BuildUI(TransactionInfo info)
    {
        base.BuildUI(info);
        if (Button)
        {
            if (PNonConsumableType.AVATAR.GetValue().Contains(info.Index))
            {
                Button.onClick.RemoveAllListeners();
                Button.onClick.AddListener(() =>
                {
                    WSClient.ChangeAvatar(info.Index);
                });
            }
        }

    }
    protected override string GetStatus(TransactionInfo info)
    {
        if (info.Index == GameData.Player.Avatar.Data)
        {
            return "Using";
        }
        else
        {
            return "Choose";
        }
    }
}
