using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealCollection : TransactionCollection
{
    protected void Awake()
    {
        transactionType = TransactionType.starter;
        List<TransactionInfo> transactionInfos = GameData.TransactionConfigs[transactionType];
        transactionInfos.Reverse();
        BuildUIs(transactionInfos);
    }

}