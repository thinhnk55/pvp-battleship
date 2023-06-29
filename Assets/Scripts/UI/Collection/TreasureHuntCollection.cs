using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureHuntCollection : CardCollectionBase<TreasureHuntInfo>
{
    public override void UpdateUIs()
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        List<TreasureHuntInfo> infos = new List<TreasureHuntInfo>();
        for(int i=0; i<GameData.TreasureConfigs.Count; i++)
        {
            infos.Add(new TreasureHuntInfo()
            {
                Id = GameData.TreasureConfigs[i].Id,
                PrizeAmount = GameData.TreasureConfigs[i].PrizeAmount,
                OnClick = LoadTreasureHuntScene
            }) ;
        }

        BuildUIs(infos);
    }

    private void LoadTreasureHuntScene()
    {
        if (GameData.JoinTreasureRoom.IsSuccess == 0)
        {
            Debug.Log("don't enough beri !!!");
            return;
        }
        SceneTransitionHelper.Load(ESceneName.TreasureHuntGame);
    }
}
