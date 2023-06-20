using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetCollection : CardCollectionBase<BetInfo>
{
    private void Awake()
    {
        List<BetInfo> infos = new List<BetInfo>();
        for (int i = 0; i < CoreGame.bets.Count; i++)
        {
            int _i = i;
            infos.Add(new BetInfo()
            {
                RewardAmount = CoreGame.bets[i] * 2,
                EntryStake = CoreGame.bets[i],
                onClick = () =>
                {
                    if (CoreGame.bets[_i] <= PResourceType.BERI.GetValue())
                    {
                        SceneTransitionHelper.Load(ESceneName.MainGame);
                    }
                }
            });
        }
        BuildUIs(infos);
    }
}
