using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankCollection : CardCollectionBase<RankInfo>
{
    private void Start()
    {
        List<RankInfo> infos = new List<RankInfo>();
        for (int i = 0; i < GameData.RankConfigs.Count; i++)
        {
            infos.Add(new RankInfo()
            {
                Icon = SpriteFactory.Ranks[i],
                Title = GameData.RankConfigs[i].Title,
                Point = GameData.RankConfigs[i].Point,
            });
        }
        BuildUIs(infos);
    }
}
