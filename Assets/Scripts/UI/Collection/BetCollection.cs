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
        for (int i = 0; i < GameData.Bets.Length; i++)
        {
            int _i = i;
            bool isQualified = GameData.Player.Rank >= GameData.BetRequires[_i];
            infos.Add(new BetInfo()
            {
                Index = _i,
                IsQualified = isQualified,
                RewardAmount = GameData.Bets[i] * 2,
                EntryStake = GameData.Bets[i],
                OnClick = () =>
                {
                    if (GameData.Bets[_i] <= PConsumableType.BERI.GetValue())
                    {
                        SceneTransitionHelper.Load(ESceneName.MainGame);
                    }
                }
            });
        }
        BuildUIs(infos);
    }


}