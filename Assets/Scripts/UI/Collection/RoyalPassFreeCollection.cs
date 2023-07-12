using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoyalPassFreeCollection : CardCollectionBase<RoyalPassInfo>
{
    private void OnEnable()
    {
        GameData.RoyalPass.NormalObtains.OnDataChanged += OnObtain;
        UpdateUIs();
    }

    private void OnObtain(HashSet<int> arg1, HashSet<int> arg2)
    {
        UpdateUIs();
    }
    private void OnDisable()
    {
        GameData.RoyalPass.NormalObtains.OnDataChanged -= OnObtain;
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
                Id = _i,
                Obtained = GameData.RoyalPass.NormalObtains.Data.Contains(_i),
                Unlocked = GameData.RoyalPass.Level >= _i,
                Reward = goods,
                Elite = false,
                Obtain = (info) =>
                {
                    WSClient.RequestReceiveRoyalPass(info.Id, 0);
                }
            });;
        }


        BuildUIs(infos);
    }

}
