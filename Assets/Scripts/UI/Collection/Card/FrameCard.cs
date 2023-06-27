using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameCard : TransactionCard
{
    protected override string GetStatus(TransactionInfo info)
    {
        if (info.Index == GameData.Player.FrameAvatar)
        {
            return "Using";
        }
        else
        {
            return "Choose";
        }
    }
}
