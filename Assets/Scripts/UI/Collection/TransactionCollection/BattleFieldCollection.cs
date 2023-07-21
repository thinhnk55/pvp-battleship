using Framework;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFieldCollection : TransactionCollection
{
    public int total;
    public bool isUnlocked;
    public bool isObtainable;
    void Awake()
    {
        UpdateUIs();
        ServerMessenger.AddListener<JSONNode>(ServerResponse.RECIEVE_CHANGE_BATTLEFIELD, ReceiveChangeBattleField);
        PNonConsumableType.BATTLE_FIELD.GetData().OnDataChanged += OnDataChanged;
    }
    private void OnDestroy()
    {
        PNonConsumableType.BATTLE_FIELD.GetData().OnDataChanged -= OnDataChanged;
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse.RECIEVE_CHANGE_BATTLEFIELD, ReceiveChangeBattleField);
    }
    private void OnDataChanged(HashSet<int> arg1, HashSet<int> arg2)
    {
        UpdateUIs();
    }
    public override void UpdateUIs()
    {
        base.UpdateUIs();
        List<TransactionInfo> transactionInfos = new List<TransactionInfo>();
        transactionType = TransactionType.BERI_BATTLE_FIELD;
        for (int i = 0; i < GameData.TransactionConfigs[transactionType].Count; i++)
        {
            var transaction = GameData.TransactionConfigs[transactionType][i];
            if ((isUnlocked == PNonConsumableType.BATTLE_FIELD.GetValue().Contains((int)transaction.Product[0].Value) && isObtainable && transaction.Cost[0].Type >= 0)
                || (!isObtainable && transaction.Cost[0].Type == -1 && !PNonConsumableType.BATTLE_FIELD.GetValue().Contains((int)transaction.Product[0].Value)))
            {
                transactionInfos.Add(transaction);
            }
        }
        BuildUIs(transactionInfos);
    }

    public void ReceiveChangeBattleField(JSONNode json)
    {
        GameData.Player.BattleField.Data = int.Parse(json["b"]);
        UpdateUIs();

    }
}