using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using System;

public class PVEData : PDataBlock<PVEData>
{
    [SerializeField] private int verisonPVEData; public static int VerisonPVEData { get { return Instance.verisonPVEData; } set { Instance.verisonPVEData = value; } }
    [SerializeField] private PVE_PlayerData playerData; public static PVE_PlayerData PlayerData { get { return Instance.playerData; } set { Instance.playerData = value; } }
    [SerializeField] private List<int> bets; public static List<int> Bets { get { return Instance.bets; } set { Instance.bets = value; } }
    [SerializeField] private List<List<int>> stageMulReward; public static List<List<int>> StageMulReward { get { return Instance.stageMulReward; } set { Instance.stageMulReward = value; } }
    [SerializeField] private List<List<int>> winRate; public static List<List<int>> WinRate { get { return Instance.winRate; } set { Instance.winRate = value; } }
    protected override void Init()
    {
        base.Init();
        Instance.playerData = Instance.playerData ?? new PVE_PlayerData();
        Instance.playerData.CurrentStep = new PDataUnit<int>(0);
        Instance.bets = Instance.bets ?? new List<int>();
        Instance.stageMulReward = Instance.stageMulReward ?? new List<List<int>>();
    }
}


[Serializable]
public class PVE_PlayerData
{
    public int TypeBoard;
    public PDataUnit<int> CurrentStep;
    //public int Point;
    public bool IsDead;
    public bool IsRevived;
}
