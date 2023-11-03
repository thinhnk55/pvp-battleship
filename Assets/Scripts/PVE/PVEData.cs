using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using System;

public class PVEData : PDataBlock<PVEData>
{
    [SerializeField] public int verisonPVEData; public static int VerisonPVEConfig { get { return Instance.verisonPVEData; } set { Instance.verisonPVEData = value; } }
    [SerializeField] public int? typeBoard; public static int? TypeBoard { get { return Instance.typeBoard; } set { Instance.typeBoard = value; } }
    [SerializeField] public PDataUnit<bool> isDeadPlayer; public static PDataUnit<bool> IsDeadPlayer { get { return Instance.isDeadPlayer; } set { Instance.isDeadPlayer = value; } }
    [SerializeField] public List<int> bets; public static List<int> Bets { get { return Instance.bets; } set { Instance.bets = value; } }
    [SerializeField] public List<List<int>> stageMulReward; public static List<List<int>> StageMulReward { get { return Instance.stageMulReward; } set { Instance.stageMulReward = value; } }
    [SerializeField] public List<List<int>> winRate; public static List<List<int>> WinRate { get { return Instance.winRate; } set { Instance.winRate = value; } }
    protected override void Init()
    {
        base.Init();
        Instance.typeBoard = Instance.typeBoard ?? new int?(-1);
        Instance.isDeadPlayer = new PDataUnit<bool>(false);
        Instance.bets = Instance.bets ?? new List<int>();
        Instance.stageMulReward = Instance.stageMulReward ?? new List<List<int>>();
    }
}