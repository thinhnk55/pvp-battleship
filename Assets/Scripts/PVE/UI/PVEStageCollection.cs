using Framework;
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
        for (int i = 1; i < PVEData.StageMulReward[PVEData.TypeBoard.Value].Count; i++)
        {
            list.Add(new StageInfo()
            {
                id = i,
                rewardMul = PVEData.StageMulReward[PVEData.TypeBoard.Value][i],
            });
        }
        BuildUIs(list);
    }

    public void OnStageChange(int o, int n)
    {
        if (n < 1)
        {
            if (n < o) // replay
            {
                ((PVEStageCard)cards[o - 1]).id.color = Color.white;
            }
            currentStageIndicator.gameObject.SetActive(false);
            return;
        }
        currentStageIndicator.gameObject.SetActive(true);
        PDebug.Log("Move to card " + n);
        currentStageIndicator.transform.parent = cards[n - 1].transform;
        currentStageIndicator.anchoredPosition3D = new Vector3(0, 0, 0);
        if (o >= 1)
        {
            ((PVEStageCard)cards[o - 1]).id.color = Color.white;
        }
        ((PVEStageCard)cards[n - 1]).id.color = Color.yellow;
    }
}
