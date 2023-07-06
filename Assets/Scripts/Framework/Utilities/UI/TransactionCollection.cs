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
        USD_GEM,
        GEM_BERI,
        BERI_AVATAR,
        BERI_AVATAR_FRAME,
        BERI_BATTLE_FIELD,
        BERI_SKIN_SHIP,
        USD_ELITE,
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
