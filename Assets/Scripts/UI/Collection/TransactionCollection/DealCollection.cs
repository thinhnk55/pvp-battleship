using Framework;
using System.Collections.Generic;

public class DealCollection : TransactionCollection
{
    protected void Awake()
    {
        UpdateUIs();
    }
    public override void UpdateUIs()
    {
        base.UpdateUIs();
        transactionType = TransactionType.starter;
        List<TransactionInfo> transactionInfos = new List<TransactionInfo>();
        if (!GameData.Starter)
        {
            transactionInfos.AddRange(GameData.TransactionConfigs[TransactionType.starter]);
        }
        BuildUIs(transactionInfos);
    }
}