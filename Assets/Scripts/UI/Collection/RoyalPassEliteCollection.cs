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
        GameData.RoyalPass.EliteObtains.OnDataChanged += OnObtain;
        UpdateUIs();
    }
    private void OnObtain(HashSet<int> arg1, HashSet<int> arg2)
    {
        UpdateUIs();
    }
    private void OnDestroy()
    {
        GameData.RoyalPass.EliteObtains.OnDataChanged -= OnObtain;
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
                Id = _i,
                Obtained = GameData.RoyalPass.EliteObtains.Data.Contains(_i),
                Unlocked = GameData.RoyalPass.UnlockedElite && GameData.RoyalPass.Level >= _i,
                Reward = goods,
                Elite = true,
                Obtain = (info) =>
                {
                    WSClient.RequestReceiveRoyalPass(info.Id, 1);
                }
            });
        }

        BuildUIs(infos);

        int previewIndex = 0;
        for (int i = 0; i < GameData.RoyalPass.RewardElites.Length / 10; i++)
        {
            previewIndex = i * 10;
            if (!GameData.RoyalPass.EliteObtains.Data.Contains(previewIndex))
            {
                break;
            }
        }
        elitePreview.BuildUI(infos[previewIndex]);
    }
}
