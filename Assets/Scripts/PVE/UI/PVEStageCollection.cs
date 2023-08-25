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
        for (int i = 0; i < PVEData.StageCount; i++)
        {
            list.Add(new StageInfo()
            {
                id = i,
                rewardMul = 0,
            });
        }
        BuildUIs(list);
    }

    public void OnStageChange(int o, int n)
    {
        currentStageIndicator.transform.parent = cards[n].transform;
        currentStageIndicator.anchoredPosition3D = cards[n].GetComponent<RectTransform>().anchoredPosition3D;
    }
}
