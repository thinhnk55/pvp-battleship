using Framework;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameCollection : TransactionCollection
{
    public int total;
    public bool isUnlocked;
    public bool isObtainable;

    void Awake()
    {
        UpdateUIs();
        ServerMessenger.AddListener<JSONNode>(ServerResponse.RECIEVE_CHANGE_FRAME, ReceiveChangeFrame);
        PNonConsumableType.AVATAR_FRAME.GetData().OnDataChanged += OnDataChanged;

    }

    private void OnDestroy()
    {
        PNonConsumableType.AVATAR_FRAME.GetData().OnDataChanged -= OnDataChanged;
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse.RECIEVE_CHANGE_FRAME, ReceiveChangeFrame);
    }
    private void OnDataChanged(HashSet<int> arg1, HashSet<int> arg2)
    {
        UpdateUIs();
    }
    public override void UpdateUIs()
    {
        base.UpdateUIs();
        List<TransactionInfo> transactionInfos = new List<TransactionInfo>();
        transactionType = TransactionType.BERI_AVATAR_FRAME;
        for (int i = 0; i < GameData.TransactionConfigs[transactionType].Count; i++)
        {
            var transaction = GameData.TransactionConfigs[transactionType][i];
            if ((isUnlocked == PNonConsumableType.AVATAR_FRAME.GetValue().Contains((int)transaction.Product[0].Value) && isObtainable && transaction.Cost[0].Type >= 0)
                || (!isObtainable && transaction.Cost[0].Type == -1 && !PNonConsumableType.AVATAR_FRAME.GetValue().Contains((int)transaction.Product[0].Value)))
            {

                transactionInfos.Add(transaction);
            }
        }
        BuildUIs(transactionInfos);
    }

    public void ReceiveChangeFrame(JSONNode json)
    {
        GameData.Player.FrameAvatar.Data = int.Parse(json["f"]);
        UpdateUIs();

    }
}
