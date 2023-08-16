using Framework;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinShipCollection : TransactionCollection
{
    public int total;
    public bool isUnlocked;
    public bool isObtainable;
    [SerializeField] TextMeshProUGUI title;
    void Awake()
    {
        UpdateUIs();
        //ServerMessenger.AddListener<JSONNode>(ServerResponse.RECIEVE_CHANGE_SKIN_SHIP, ReceiveChangeSkinShip);
        PNonConsumableType.SKIN_SHIP.GetData().OnDataChanged += OnDataChanged;

    }
    private void OnDestroy()
    {
        PNonConsumableType.SKIN_SHIP.GetData().OnDataChanged -= OnDataChanged;
        //ServerMessenger.RemoveListener<JSONNode>(ServerResponse.RECIEVE_CHANGE_SKIN_SHIP, ReceiveChangeSkinShip);
    }
    private void OnDataChanged(HashSet<int> arg1, HashSet<int> arg2)
    {
        UpdateUIs();
    }
    public override void UpdateUIs()
    {
        base.UpdateUIs();
        List<TransactionInfo> transactionInfos = new List<TransactionInfo>();
        transactionType = TransactionType.gold_skinship;
        for (int i = 0; i < GameData.TransactionConfigs[transactionType].Count; i++)
        {
            var transaction = GameData.TransactionConfigs[transactionType][i];
            if ((isUnlocked == PNonConsumableType.SKIN_SHIP.GetValue().Contains((int)transaction.Product[0].Value) && isObtainable && ((isUnlocked) || (!isUnlocked && transaction.Cost[0].Value >= 0)))
                || (!isObtainable && transaction.Cost[0].Value == -1 && !PNonConsumableType.SKIN_SHIP.GetValue().Contains((int)transaction.Product[0].Value)))
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

    public void ReceiveChangeSkinShip(JSONNode json)
    {
        GameData.Player.SkinShip.Data = int.Parse(json["s"]);
        UpdateUIs();
    }
}
