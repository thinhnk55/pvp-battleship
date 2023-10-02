using Framework;
using System;
using TMPro;
using UnityEngine;
[Serializable]

public struct RewardCoinInfo
{
    public int Id;
    public long Amount;
}

public class RewardCoinCard : CardBase<RewardCoinInfo>
{
    [SerializeField] private TextMeshProUGUI amountTxt;

    public override void BuildUI(RewardCoinInfo info)
    {
        base.BuildUI(info);

        amountTxt.text = "x" + info.Amount.ToString();

    }

    protected override void OnClicked(RewardCoinInfo info)
    {
        OnClick?.Invoke();
    }
}
