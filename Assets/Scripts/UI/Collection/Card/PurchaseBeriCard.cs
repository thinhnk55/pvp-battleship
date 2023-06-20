using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseBeriCard : TransactionCard
{
    [SerializeField] TextMeshProUGUI goodAmount;
    [SerializeField] Image goodIcon;
    [SerializeField] TextMeshProUGUI costAmount;
    [SerializeField] Image costIcon;

    public override void BuildUI(TransactionInfo info)
    {
        base.BuildUI(info);
        
    }

    protected override void OnClicked(TransactionInfo info)
    {
        throw new System.NotImplementedException();
    }
}
