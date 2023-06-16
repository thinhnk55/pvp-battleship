using Framework;
using TMPro;
using UnityEngine;

public struct StatiticInfo
{
    public int Info;
}
public class StatisticCard : CardBase<StatiticInfo>
{
    [SerializeField]TextMeshProUGUI Info;
    public override void BuildUI(StatiticInfo info)
    {
    }

    protected override void OnClicked(StatiticInfo info)
    {
        throw new System.NotImplementedException();
    }
}
