using Framework;
using SimpleJSON;
using System;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
[Serializable]

public struct RewardCoinInfo
{
    public int Id;
    public long Amount;
    
}

public class RewardCoinCard : CardBase<RewardCoinInfo>
{
    [HideInInspector] public int Id;
    [SerializeField] protected Image Icon;
    [HideInInspector] protected long Amount;

    public override void BuildUI(RewardCoinInfo info)
    {
        base.BuildUI(info);
        Id = info.Id;
        Amount = info.Amount;
    }

    protected override void OnClicked(RewardCoinInfo info)
    {
        OnClick?.Invoke();
    }
}
