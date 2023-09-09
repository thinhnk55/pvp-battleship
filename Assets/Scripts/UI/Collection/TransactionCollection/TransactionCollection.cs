using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Framework
{
    public enum TransactionType
    {
        starter = 0,
        usd = 1,
        diamond = 2,
        gold_avatar = 3,
        gold_frame = 4,
        gold_battlefield = 5,
        gold_skinship = 6,
        elite = 7,
    }
    public class TransactionCollection : CardCollectionBase<TransactionInfo>
    {
        public TransactionType transactionType;
        public override void BuildUIs(List<TransactionInfo> infos)
        {
            base.BuildUIs(infos);
        }

        public override void UpdateUIs()
        {
            contentRoot.DestroyChildrenImmediate();
        }
    }

}
