using Framework;
using TMPro;
using UnityEngine;

public struct StatiticInfo
{
    public int Info;
}
public class StatisticCard : CardBase<StatiticInfo>
{
    public override void BuildUI(StatiticInfo info)
    {
        base.BuildUI(info);
    }

    protected override void OnClicked(StatiticInfo info)
    {
        throw new System.NotImplementedException();
    }
}
