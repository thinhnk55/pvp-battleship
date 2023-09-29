using Framework;
using System.Collections.Generic;

public class RewadCoinCollection : CardCollectionBase<RewardCoinInfo>
{

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
        List<RewardCoinInfo> infos = new(8);
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

