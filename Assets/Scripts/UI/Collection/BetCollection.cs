using DG.Tweening;
using Framework;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetCollection : CardCollectionBase<BetInfo>
{
    public override void UpdateUIs()
    {
        List<BetInfo> infos = new List<BetInfo>();
        for (int i = 0; i < GameData.Bets.Length; i++)
        {
            int _i = i;
            bool isQualified = GameData.Player.Rank >= GameData.Bets[_i].BetRequire;
            infos.Add(new BetInfo()
            {
                Index = _i,
                IsQualified = isQualified,
                RewardAmount = GameData.Bets[i].BetRankPoint,
                EntryStake = GameData.Bets[i].Bet,
                OnClick = () =>
                {
                    if (GameData.Bets[_i].Bet <= PConsumableType.BERRY.GetValue())
                    {
                        CoreGame.bet = _i;
                        SceneTransitionHelper.Load(ESceneName.MainGame);
                    }
                    else
                    {
                        PopupHelper.CreateConfirm(PrefabFactory.PopupOutOfResource, "Message", "Not enough money", null, (ok) =>
                        {
                            if (ok)
                            {
                                PopupBehaviour.CloseAll();
                                PopupHelper.Create(PrefabFactory.PopupShop).GetComponentInChildren<Tabs>().Activate(1);
                            }
                        });
                    }
                }
            });
        }
        BuildUIs(infos);
    }

    private void Awake()
    {
        UpdateUIs();
        if (GameData.Tutorial[1] == 0)
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                PopupHelper.Create(PrefabFactory.PopupTuTorBet);
            });
        }
    }
}
