using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpendingPlayerCollection : CardCollectionBase<SpendingCountInfo>
{
    [SerializeField] SpendingPlayerCount mySpendingCount;
    private void Awake()
    {
        UpdateUIs();
        mySpendingCount.BuildUI(GameData.MySpendingCountInfo);
    }

    public override void UpdateUIs()
    {
        List<SpendingCountInfo> winCountInfosList = new List<SpendingCountInfo>();

        for (int i = 0; i < GameData.SpendingCountInfoList.Count; i++)
        {
            winCountInfosList.Add(new SpendingCountInfo
            {
                Order = GameData.SpendingCountInfoList[i].Order,
                Rank = GameData.SpendingCountInfoList[i].Rank,
                UserName = GameData.SpendingCountInfoList[i].UserName,
                SpendingCount = GameData.SpendingCountInfoList[i].SpendingCount,
                Reward = GameData.SpendingCountInfoList[i].Reward
            });
        }

        BuildUIs(winCountInfosList);
    }

    public override void BuildUIs(List<SpendingCountInfo> infos)
    {
        base.BuildUIs(infos);
    }
}
