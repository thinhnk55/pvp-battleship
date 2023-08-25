using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct StageInfo
{
    public int id;
    public int rewardMul;
}
public class PVEStageCard : CardBase<StageInfo>
{
    [SerializeField] TextMeshProUGUI id;
    [SerializeField] TextMeshProUGUI mulReward;

    public override void BuildUI(StageInfo info)
    {
        base.BuildUI(info);
        id?.SetText(info.id.ToString());
        mulReward?.SetText("x"+ info.rewardMul.ToString());
    }
    protected override void OnClicked(StageInfo info)
    {
        throw new System.NotImplementedException();
    }
}
