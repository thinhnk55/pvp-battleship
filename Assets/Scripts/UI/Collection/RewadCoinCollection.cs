using FirebaseIntegration;
using Framework;
using SimpleJSON;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewadCoinCollection : CardCollectionBase<RewardCoinInfo>
{
    protected List<RewardCoinInfo> infos = new(8);

    private void OnEnable()
    {
        UpdateUIs();
        
    }
    public override void BuildUIs(List<RewardCoinInfo> infos)
    {
        base.BuildUIs(infos);
    }

    public override void UpdateUIs()
    {
        //base.UpdateUIs();
        for (int i = 0; i < 8; i++)
        {
            RewardCoinInfo info = new RewardCoinInfo
            {
                Id = i,
                Amount = 5000
            };
            infos.Add(info);
        }
        BuildUIs(infos);
    }    
}

