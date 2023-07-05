using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoyalPassEliteCollection : CardCollectionBase<RoyalPassInfo>
{
    [SerializeField] RoyalPassCard elitePreview;
    private void Awake()
    {
        UpdateUIs();
    }
    public override void UpdateUIs()
    {
        List<RoyalPassInfo> infos = new List<RoyalPassInfo>();

        for (int i = 0; i < GameData.RoyalPass.RewardElites.Length; i++)
        {
            int _i = i;
            int row = GameData.RoyalPass.RewardElites.Length / GameData.RoyalPass.RewardElites.Rank;
            GoodInfo[] goods = GameData.RoyalPass.RewardElites[i].ToArray();
            infos.Add(new RoyalPassInfo()
            {
                Id = i,
                Obtained = GameData.RoyalPass.EliteObtains.Contains(i),
                Unlocked = (GameData.RoyalPass.Point / 100) > i,
                Reward = goods,
                Elite = true,
                Obtain = (info) =>
                {
                    WSClient.RequestReceiveRoyalPass(_i, 0);
                }
            });
        }

        BuildUIs(infos);

        int previewIndex = 0;
        for (int i = 0; i < GameData.RoyalPass.EliteObtains.Count / 10; i++)
        {
            previewIndex = i * 10;
            if (!GameData.RoyalPass.EliteObtains.Contains(previewIndex))
            {
                break;
            }
        }
        elitePreview.BuildUI(infos[previewIndex]);
    }
}
