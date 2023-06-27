using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarCollection : TransactionCollection
{
    public int total;
    public bool isUnlocked;
    void Awake()
    {
        List<TransactionInfo> transactionInfos = new List<TransactionInfo>();
        transactionType = TransactionType.BERI_AVATAR;
        for (int i = 0; i < GameData.TransactionConfigs[transactionType].Count; i++)
        {
            var transaction = GameData.TransactionConfigs[transactionType][i];
            if (isUnlocked == PNonConsumableType.AVATAR.GetValue().Contains((int)transaction.Product[0].Value))
            {
                transactionInfos.Add(transaction);
            }
        }
        BuildUIs(transactionInfos);
    }

}
