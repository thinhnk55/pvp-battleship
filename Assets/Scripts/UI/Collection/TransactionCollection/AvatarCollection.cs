using Framework;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarCollection : TransactionCollection
{
    public int total;
    public bool isUnlocked;
    void Awake()
    {
        UpdateUIs();
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_CHANGE_AVATAR, ReceiveChangeAvatar);
        PNonConsumableType.AVATAR.GetData().OnDataChanged += OnDataChanged;
    }

    private void OnDataChanged(HashSet<int> arg1, HashSet<int> arg2)
    {
        UpdateUIs();
    }

    private void OnDestroy()
    {
        PNonConsumableType.AVATAR.GetData().OnDataChanged -= OnDataChanged;
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_CHANGE_AVATAR, ReceiveChangeAvatar);
    }
    public override void UpdateUIs()
    {
        base.UpdateUIs();
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

    public void ReceiveChangeAvatar(JSONNode json)
    {
        GameData.Player.Avatar.Data = int.Parse(json["a"]);
        UpdateUIs();
    }
}
