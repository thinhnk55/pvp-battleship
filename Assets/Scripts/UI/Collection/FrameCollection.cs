using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameCollection : TransactionCollection
{
    public int total;
    public bool isUnlocked;
    void Awake()
    {
        List<TransactionInfo> transactionInfos = new List<TransactionInfo>();
        transactionType = TransactionType.BERI_AVATAR_FRAME;
        for (int i = 0; i < GameData.TransactionConfigs[transactionType].Count; i++)
        {
            var transaction = GameData.TransactionConfigs[transactionType][i];
            if (isUnlocked == PNonConsumableType.AVATAR_FRAME.GetValue().Contains((int)transaction.Product[0].Value))
            {
                if (!isUnlocked)
                {
                    
                }
                transactionInfos.Add(transaction);
            }
        }
        BuildUIs(transactionInfos);
    }

}
