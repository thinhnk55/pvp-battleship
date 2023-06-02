using Framework;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GoodType
{
    BERI,
    DIAMOND,
    REMOVE_ADS,
}
public enum CostType
{
    BERI,
    DIAMOND,
    MONEY,
    ADS
}

public struct TransactionItemInfo
{
    public Sprite Icon;
    public int Amount;
}

public struct TransactionInfo
{
    public string Title;
    public TransactionItemInfo GoodItem;
    public TransactionItemInfo CostItem;
}

public class TransactionCard : CardBase<TransactionInfo>
{
    public TextMeshProUGUI Title;

    public Image GoodIcon;
    public TextMeshProUGUI GoodAmount;

    public Image CostIcon;
    public TextMeshProUGUI CostAmount;

    public override void BuildUI(TransactionInfo info)
    {
        base.BuildUI(info);
        if (Title)
            Title.text = info.Title;
        if (GoodIcon)
            GoodIcon.sprite = info.GoodItem.Icon;
        if (GoodAmount)
            GoodAmount.text = info.GoodItem.Amount.ToString();
        if (CostIcon)
            CostIcon.sprite = info.CostItem.Icon;
        if (CostAmount)
            CostAmount.text = info.CostItem.Amount.ToString();
    }

    protected override void OnClicked(TransactionInfo info)
    {
        throw new System.NotImplementedException();
    }
}