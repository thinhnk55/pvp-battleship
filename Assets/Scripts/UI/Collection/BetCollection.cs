using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetCollection : CardCollectionBase<BetInfo>
{
    private void Awake()
    {
        List<BetInfo> infos = new List<BetInfo>();
        for (int i = 0; i < 10; i++)
        {
            infos.Add(new BetInfo()
            {
                RewardAmount = (i + 1) * 1000000,
                EntryStake = (i + 1) * 500000
            });
        }
        BuildUI(infos);
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].Button.onClick.AddListener(() => {  CoreGame.bet = cards.Count - i; SceneTransitionHelper.Load(ESceneName.MainGame); });
        }
    }
}
