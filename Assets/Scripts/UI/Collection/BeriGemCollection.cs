using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeriGemCollection : TransactionCollection
{
    [SerializeField] TransactionCard bigCard;
    protected void Awake()
    {
        List<TransactionInfo> transactionInfos = GameData.TransactionConfigs[transactionType];
        bigCard.BuildUI(transactionInfos.Last());
        BuildUIs(transactionInfos.GetRange(0, transactionInfos.Count - 1));
    }
}
