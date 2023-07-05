using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoyalPassFreeCollection : CardCollectionBase<RoyalPassInfo>
{
    private void Awake()
    {

        UpdateUIs();
    }
    public override void UpdateUIs()
    {
        List<RoyalPassInfo> infos = new List<RoyalPassInfo>();

        for (int i = 0; i < GameData.RoyalPass.RewardNormals.Length; i++)
        {
            int _i = i;
            int row = GameData.RoyalPass.RewardNormals.Length / GameData.RoyalPass.RewardNormals.Rank;
            GoodInfo[] goods = GameData.RoyalPass.RewardNormals[i].ToArray();
            infos.Add(new RoyalPassInfo()
            {
                Id = i,
                Obtained = GameData.RoyalPass.NormalObtains.Contains(i),
                Unlocked = (GameData.RoyalPass.Point/100) > i,
                Reward = goods,
                Elite = false,
                Obtain = (info) =>
                {
                    WSClient.RequestReceiveRoyalPass(_i,0);
                }
            });;
        }


        BuildUIs(infos);
    }

}
