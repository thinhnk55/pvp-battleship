using Framework;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct GoodInfo
{
    public Sprite Icon;
    public int Number;
    public PResourceType Type;
}

public struct TransactionInfo
{
    public string Title;
    public GoodInfo[] Cost;
    public GoodInfo[] Product;
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
            GoodIcon.sprite = info.Product[0].Icon;
        if (GoodAmount)
            GoodAmount.text = info.Product[0].Number.ToString();
        if (CostIcon)
            CostIcon.sprite = info.Cost[0].Icon;
        if (CostAmount)
            CostAmount.text = info.Cost[0].Number.ToString();
    }

    protected override void OnClicked(TransactionInfo info)
    {
        throw new System.NotImplementedException();
    }
}