using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeriGemCollection : TransactionCollection
{
    [SerializeField] TransactionCard bigCard;
    [SerializeField] TransactionCard dealCard;
    protected void Awake()
    {
        List<TransactionInfo> transactionInfos = GameData.TransactionConfigs[transactionType];
        dealCard.BuildUI(transactionInfos.Last());
        bigCard.BuildUI(transactionInfos[transactionInfos.Count-2]);
        var list = transactionInfos.GetRange(0, transactionInfos.Count - 2);
        list.Reverse();
        BuildUIs(list);
    }

    public override void UpdateUIs()
    {
       
    }
}
