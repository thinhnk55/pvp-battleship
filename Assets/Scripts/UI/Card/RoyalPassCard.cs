using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RoyalPassInfo
{
    public bool Unlocked;
    public GoodInfo Reward;
    public bool Obtained;
}
public class RoyalPassCard : CardBase<RoyalPassInfo>
{
    protected override void OnClicked(RoyalPassInfo info)
    {
        throw new System.NotImplementedException();
    }

    public override void BuildUI(RoyalPassInfo info)
    {
        base.BuildUI(info);
    }
}
