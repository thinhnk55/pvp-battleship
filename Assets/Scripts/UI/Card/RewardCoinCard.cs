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
    public int _Id;
    public long _Amount;
    
    public int Id { get; set; }
    public long Amount { get; set; }
}

public class RewardCoinCard : CardBase<RewardCoinInfo>
{
    [HideInInspector] public int Id;  
    [HideInInspector] protected long Amount;
    [SerializeField] private TextMeshProUGUI amountTxt;

    public override void BuildUI(RewardCoinInfo info)
    {
        
        this.Id = info._Id;
        this.Amount = info._Amount;
        amountTxt.text ="x"+ Amount.ToString();


        base.BuildUI(info);
        
    }

    protected override void OnClicked(RewardCoinInfo info)
    {
        OnClick?.Invoke();
    }
}
