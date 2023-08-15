using Framework;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AvatarCollection : TransactionCollection
{
    public int total;
    public bool isUnlocked;
    public bool isObtainable;
    [SerializeField] TextMeshProUGUI title;
    void Awake()
    {
        UpdateUIs();
        ServerMessenger.AddListener<JSONNode>(ServerResponse._CHANGE_AVATAR, ReceiveChangeAvatar);
        PNonConsumableType.AVATAR.GetData().OnDataChanged += OnDataChanged;
    }

    private void OnDataChanged(HashSet<int> arg1, HashSet<int> arg2)
    {
        UpdateUIs();
    }

    private void OnDestroy()
    {
        PNonConsumableType.AVATAR.GetData().OnDataChanged -= OnDataChanged;
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._CHANGE_AVATAR, ReceiveChangeAvatar);
    }
    public override void UpdateUIs()
    {
        base.UpdateUIs();
        List<TransactionInfo> transactionInfos = new List<TransactionInfo>();
        transactionType = TransactionType.gold_avatar;

        for (int i = 0; i < GameData.TransactionConfigs[transactionType].Count; i++)
        {
            var transaction = GameData.TransactionConfigs[transactionType][i];
            if ((isUnlocked == PNonConsumableType.AVATAR.GetValue().Contains((int)transaction.Product[0].Value)  && isObtainable) 
                || (!isObtainable && transaction.Cost[0].Value==-1 && !PNonConsumableType.AVATAR.GetValue().Contains((int)transaction.Product[0].Value)))
            {
                transactionInfos.Add(transaction);
            }
        }
        if (transactionInfos.Count == 0)
        {
            title.gameObject.SetActive(false);
            title.GetComponentInParent<LayoutElement>().minHeight = 0;
            contentRoot.gameObject.SetActive(false);
        }
        else
        {
            title.gameObject.SetActive(true);
            title.GetComponentInParent<LayoutElement>().minHeight = 100;
            contentRoot.gameObject.SetActive(true);
        }
        BuildUIs(transactionInfos);
    }

    public void ReceiveChangeAvatar(JSONNode json)
    {
        GameData.Player.Avatar.Data = json["d"]["a"].AsInt;
        UpdateUIs();
    }
}
