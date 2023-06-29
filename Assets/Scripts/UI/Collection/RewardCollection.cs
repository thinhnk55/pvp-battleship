using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardCollection : CardCollectionBase<RewardInfo>
{
    public override void BuildUIs(List<RewardInfo> infos)
    {
        base.BuildUIs(infos);
    }

    public override void UpdateUIs()
    {
        throw new System.NotImplementedException();
    }

    public void ObtainRewards()
    {

    }
}
