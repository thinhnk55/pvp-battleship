using Framework;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinShipCollection : TransactionCollection
{
    public int total;
    public bool isUnlocked;
    public bool isObtainable;
    void Awake()
    {
        UpdateUIs();
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_CHANGE_SKIN_SHIP, ReceiveChangeSkinShip);
        PNonConsumableType.SKIN_SHIP.GetData().OnDataChanged += OnDataChanged;

    }
    private void OnDestroy()
    {
        PNonConsumableType.SKIN_SHIP.GetData().OnDataChanged -= OnDataChanged;
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_CHANGE_AVATAR, ReceiveChangeSkinShip);
    }
    private void OnDataChanged(HashSet<int> arg1, HashSet<int> arg2)
    {
        UpdateUIs();
    }
    public override void UpdateUIs()
    {
        base.UpdateUIs();
        List<TransactionInfo> transactionInfos = new List<TransactionInfo>();
        transactionType = TransactionType.BERI_SKIN_SHIP;
        for (int i = 0; i < GameData.TransactionConfigs[transactionType].Count; i++)
        {
            var transaction = GameData.TransactionConfigs[transactionType][i];
            if ((isUnlocked == PNonConsumableType.SKIN_SHIP.GetValue().Contains((int)transaction.Product[0].Value) && isObtainable && transaction.Cost[0].Type >= 0)
                || (!isObtainable && transaction.Cost[0].Type == -1 && !PNonConsumableType.SKIN_SHIP.GetValue().Contains((int)transaction.Product[0].Value)))
            {
                transactionInfos.Add(transaction);
            }
        }
        BuildUIs(transactionInfos);
    }

    public void ReceiveChangeSkinShip(JSONNode json)
    {
        GameData.Player.SkinShip.Data = int.Parse(json["s"]);
        UpdateUIs();

    }
}
