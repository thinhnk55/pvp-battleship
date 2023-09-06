using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFieldCard : TransactionCard
{
    public override void BuildUI(TransactionInfo info)
    {
        base.BuildUI(info);
        if (Button)
        {
            if (PNonConsumableType.BATTLE_FIELD.GetValue().Contains(info.Index))
            {
                Button.onClick.RemoveAllListeners();
                Button.onClick.AddListener(() =>
                {
                    WSClientHandler.ChangeBattleField(info.Index);
                });
            }
        }
        if (info.Cost[0].Value < 0)
        {
            otherText?.SetText("S" + GameData.RoyalPass.Season.ToString());
        }
    }
    protected override string GetStatus(TransactionInfo info)
    {
        if (info.Index == GameData.Player.BattleField.Data)
        {
            return "Using";
        }
        else
        {
            return "Choose";
        }
    }
}
