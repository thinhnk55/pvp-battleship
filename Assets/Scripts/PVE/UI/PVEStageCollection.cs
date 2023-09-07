using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVEStageCollection : CardCollectionBase<StageInfo>
{
    [SerializeField] RectTransform currentStageIndicator;
    private void Awake()
    {
        UpdateUIs();
    }
    public override void UpdateUIs()
    {
        List<StageInfo> list = new List<StageInfo>();
        for (int i = 0; i < PVEData.StageMulReward[PVEData.TypeBoard.Value].Count; i++)
        {
            list.Add(new StageInfo()
            {
                id = i+1,
                rewardMul = PVEData.StageMulReward[PVEData.TypeBoard.Value][i],
            });
        }
        BuildUIs(list);
    }

    public void OnStageChange(int o, int n)
    {
        Debug.Log("Move to card " + n);
        currentStageIndicator.transform.parent = cards[n].transform;
        currentStageIndicator.anchoredPosition3D = new Vector3(0,0,0);
        if (o>=0)
        {
            ((PVEStageCard)cards[o]).id.color = Color.white;
        }
        ((PVEStageCard)cards[n]).id.color = Color.yellow;
    }
}
