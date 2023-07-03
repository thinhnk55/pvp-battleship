using Framework;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetCollection : CardCollectionBase<BetInfo>
{
    public override void UpdateUIs()
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        List<BetInfo> infos = new List<BetInfo>();
        for (int i = 0; i < CoreGame.bets.Count; i++)
        {
            int _i = i;
            bool isQualified = GameData.Player.Rank >= _i;
            infos.Add(new BetInfo()
            {
                Index = _i,
                IsQualified = isQualified,
                RewardAmount = CoreGame.bets[i] * 2,
                EntryStake = CoreGame.bets[i],
                OnClick = () =>
                {
                    if (CoreGame.bets[_i] <= PConsumableType.BERI.GetValue())
                    {
                        SceneTransitionHelper.Load(ESceneName.MainGame);
                    }
                }
            });
        }
        BuildUIs(infos);
    }


}
