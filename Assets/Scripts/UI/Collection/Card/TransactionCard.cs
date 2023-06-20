using Framework;
using SimpleJSON;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct GoodInfo
{
    public int Number;
    public PGoodType Type;
}

public struct TransactionInfo
{
    public string Title;
    public GoodInfo[] Cost;
    public GoodInfo[] Product;

    public static TransactionInfo FromJson(JSONNode data)
    {
        TransactionInfo transactionInfo = new TransactionInfo();
        for (int i = 0; i < data["cost"].Count; i++)
        {
            transactionInfo.Cost[i] = new GoodInfo()
            {
                Number = int.Parse(data["cost"][i]),
                Type = data["cost_type"][i].ToEnum<PGoodType>(),
            };
        }
        for (int i = 0; i < data["product"].Count; i++)
        {
            transactionInfo.Cost[i] = new GoodInfo()
            {
                Number = int.Parse(data["product"][i]),
                Type = data["product_type"][i].ToEnum<PGoodType>(),
            };
        }
        return transactionInfo;
    }
    public static List<TransactionInfo> ListFromJson(JSONNode data)
    {
        List<TransactionInfo> transactionInfos = new List<TransactionInfo>();
        for (int i = 0; i < data.Count; i++)
        {
            transactionInfos.Add(TransactionInfo.FromJson(data[i]));
        }
        return transactionInfos;
    }
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
            GoodIcon.sprite = SpriteFactory.ResourceIcons[(int)info.Product[0].Type];
        if (GoodAmount)
            GoodAmount.text = info.Product[0].Number.ToString();
        if (CostIcon)
            CostIcon.sprite = SpriteFactory.ResourceIcons[(int)info.Cost[0].Type];
        if (CostAmount)
            CostAmount.text = info.Cost[0].Number.ToString();
    }

    protected override void OnClicked(TransactionInfo info)
    {
        throw new System.NotImplementedException();
    }
}