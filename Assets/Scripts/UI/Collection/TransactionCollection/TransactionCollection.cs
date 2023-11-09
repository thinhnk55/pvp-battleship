using System.Collections.Generic;

namespace Framework
{
    public enum TransactionType
    {
        starter2 = 0,
        usd = 1,
        diamond = 2,
        gold_avatar = 3,
        gold_frame = 4,
        gold_battlefield = 5,
        gold_skinship = 6,
        elite2 = 7,
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
