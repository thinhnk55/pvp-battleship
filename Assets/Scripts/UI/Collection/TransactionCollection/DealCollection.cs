using Framework;
using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

public class DealCollection : TransactionCollection
{
    [SerializeField] Tabs tab;
    protected void Awake()
    {
        UpdateUIs();
    }
    private void OnEnable()
    {
        ServerMessenger.AddListener<JSONNode>(ServerResponse._TRANSACTION, UpdateUIs);
    }
    private void OnDisable()
    {
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._TRANSACTION, UpdateUIs);

    }
    public override void UpdateUIs()
    {
        base.UpdateUIs();
        transactionType = TransactionType.starter2;
        List<TransactionInfo> transactionInfos = new List<TransactionInfo>();
        if (!GameData.Starter)
        {
            transactionInfos.AddRange(GameData.TransactionConfigs[TransactionType.starter2]);
        }
        if (transactionInfos.Count == 0)
        {
            tab.DeleteTab(0);
        }
        BuildUIs(transactionInfos);
    }

    public void UpdateUIs(JSONNode data)
    {
        TransactionType id = data["d"]["s"].ToEnum<TransactionType>();
        if (id == TransactionType.starter2)
        {
            GameData.Starter = true;
            UpdateUIs();
        }
    }
}