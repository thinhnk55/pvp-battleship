using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransactionType
{
    USD_GEM,
    GEM_BERI,
    BERI_AVATAR,
    BERI_AVATAR_FRAME,
    BERI_SKIN_SHIP,
}
public class TransactionCollection : CardCollectionBase<TransactionInfo>
{
    [SerializeField] protected TransactionType transactionType;
}
