using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinShipCard : TransactionCard
{
    public override void BuildUI(TransactionInfo info)
    {
        base.BuildUI(info);
        if (Button)
        {
            if (PNonConsumableType.SKIN_SHIP.GetValue().Contains(info.Index))
            {
                Button.onClick.RemoveAllListeners();
                Button.onClick.AddListener(() =>
                {
                    WSClient.RequestChangeSkinShip(info.Index);
                });
            }
        }

    }
    protected override string GetStatus(TransactionInfo info)
    {
        if (info.Index == GameData.Player.SkinShip.Data)
        {
            return "Using";
        }
        else
        {
            return "Choose";
        }
    }
}
