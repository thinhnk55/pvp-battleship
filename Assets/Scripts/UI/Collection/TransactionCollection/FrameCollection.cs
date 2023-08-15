using Framework;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FrameCollection : TransactionCollection
{
    public int total;
    public bool isUnlocked;
    public bool isObtainable;
    [SerializeField] TextMeshProUGUI title;

    void Awake()
    {
        UpdateUIs();
        ServerMessenger.AddListener<JSONNode>(ServerResponse._CHANGE_FRAME, ReceiveChangeFrame);
        PNonConsumableType.AVATAR_FRAME.GetData().OnDataChanged += OnDataChanged;

    }

    private void OnDestroy()
    {
        PNonConsumableType.AVATAR_FRAME.GetData().OnDataChanged -= OnDataChanged;
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._CHANGE_FRAME, ReceiveChangeFrame);
    }
    private void OnDataChanged(HashSet<int> arg1, HashSet<int> arg2)
    {
        UpdateUIs();
    }
    public override void UpdateUIs()
    {
        base.UpdateUIs();
        List<TransactionInfo> transactionInfos = new List<TransactionInfo>();
        transactionType = TransactionType.gold_frame;
        for (int i = 0; i < GameData.TransactionConfigs[transactionType].Count; i++)
        {
            var transaction = GameData.TransactionConfigs[transactionType][i];
            if ((isUnlocked == PNonConsumableType.AVATAR_FRAME.GetValue().Contains((int)transaction.Product[0].Value) && isObtainable)
                || (!isObtainable && transaction.Cost[0].Value == -1 && !PNonConsumableType.AVATAR_FRAME.GetValue().Contains((int)transaction.Product[0].Value)))
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

    public void ReceiveChangeFrame(JSONNode json)
    {
        GameData.Player.FrameAvatar.Data = int.Parse(json["d"]["f"]);
        UpdateUIs();
    }
}
