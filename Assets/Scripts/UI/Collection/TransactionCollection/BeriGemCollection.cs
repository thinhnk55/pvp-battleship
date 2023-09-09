using Framework;
using System.Collections.Generic;
using UnityEngine;

public class BeriGemCollection : TransactionCollection
{
    [SerializeField] TransactionCard bigCard1;
    [SerializeField] TransactionCard bigCard2;
    [SerializeField] TransactionCard bigCard3;
    protected void Awake()
    {
        List<TransactionInfo> transactionInfos = GameData.TransactionConfigs[transactionType];
        bigCard1.BuildUI(transactionInfos[transactionInfos.Count - 1]);
        bigCard2.BuildUI(transactionInfos[transactionInfos.Count - 2]);
        bigCard3.BuildUI(transactionInfos[transactionInfos.Count - 3]);
        var list = transactionInfos.GetRange(0, transactionInfos.Count - 4);
        list.Reverse();
        BuildUIs(list);
    }

    public override void UpdateUIs()
    {

    }
}
