using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
public class PVEData : PDataBlock<PVEData>
{
    [SerializeField] private List<int> bets; public static List<int> Bets { get { return Instance.bets; } set { Instance.bets = value; } }
    [SerializeField] private int stageCount; public static int StageCount { get { return Instance.stageCount; } set { Instance.stageCount = value; } }
    protected override void Init()
    {
        base.Init();
        Instance.bets = Instance.bets ?? new List<int>();
        stageCount = 10;
    }
}
