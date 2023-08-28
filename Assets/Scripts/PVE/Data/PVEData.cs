using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
public class PVEData : PDataBlock<PVEData>
{
    [SerializeField] private List<int> bets; public static List<int> Bets { get { return Instance.bets; } set { Instance.bets = value; } }
    [SerializeField] private List<int> stageMulReward; public static List<int> StageMulReward { get { return Instance.stageMulReward; } set { Instance.stageMulReward = value; } }
    protected override void Init()
    {
        base.Init();
        Instance.bets = Instance.bets ?? new List<int>();
        Instance.stageMulReward = Instance.stageMulReward ?? new List<int>() { 0,0,0,0,0,2,3,5,10,20,50 };
    }
}
