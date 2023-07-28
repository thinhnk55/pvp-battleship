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
        starter,
        usd,
        diamond,
        gold_avatar,
        gold_frame,
        gold_battlefield,
        gold_skinship,
        GEM_ELITE,
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
