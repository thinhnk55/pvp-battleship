using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealCard : TransactionCard
{
    [SerializeField] GoodCollection goods;
    public override void BuildUI(TransactionInfo info)
    {
        base.BuildUI(info);
        List<GoodInfo> goodsList = new List<GoodInfo>();
        for (int i = 0; i < info.Product.Length; i++)
        {
            goodsList.Add(info.Product[i]);
        }
        goods.BuildUIs(goodsList);
    }

}
