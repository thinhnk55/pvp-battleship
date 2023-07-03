using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoyalPassFreeCollection : CardCollectionBase<RoyalPassInfo>
{  
    public override void UpdateUIs()
    {
        List<RoyalPassInfo> infos = new List<RoyalPassInfo>();
        for (int i = 0; i < GameData.RoyalPass.RewardNormals.Length; i++)
        {
            infos.Add(new RoyalPassInfo()
            {
                Reward = GameData.RoyalPass.RewardNormals[i]
            });
        }
        for (int i = 0; i < GameData.RoyalPass.RewardElites.Length; i++)
        {
            infos.Add(new RoyalPassInfo()
            {
                Reward = GameData.RoyalPass.RewardElites[i]
            });
        }

        BuildUIs(infos);
    }

}
