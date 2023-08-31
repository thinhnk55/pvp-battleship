using Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WinPlayerCollection : CardCollectionBase<WinCountInfo>
{
    [SerializeField] WinPlayerCount myWinCount;
    private void Awake()
    {
        // UpdateUIs();
    }
    public override void UpdateUIs()
    {
        List<WinCountInfo> winCountInfosList = new List<WinCountInfo>();
        
        for(int i = 0; i < GameData.WinCountInfoList.Count; i++)
        {
            winCountInfosList.Add(new WinCountInfo
            {
                Order = i,
                Rank = GameData.WinCountInfoList[i].Rank,
                UserName = GameData.WinCountInfoList[i].UserName,
                WinCount = GameData.WinCountInfoList[i].WinCount,
                Reward = GameData.WinCountInfoList[i].Reward
            }); 
        }

        BuildUIs(winCountInfosList);
    }

    public override void BuildUIs(List<WinCountInfo> infos)
    {
        base.BuildUIs(infos);
    }
}
