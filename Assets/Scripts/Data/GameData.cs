using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : PDataBlock<GameData>
{
    [SerializeField] private ProfileData player; public static ProfileData Player { get { return Instance.player; } set { Instance.player = value; } }
    [SerializeField] private ProfileData opponent; public static ProfileData Opponent { get { return Instance.opponent; } set { Instance.opponent = value; } }
    
}
[Serializable]
public struct ProfileData
{
    public string Username;
    public int Avatar;
    public int Point;
    public int ShipDestroyed;
    public int PerfectGame;
    public int WinStreak;
    public int WinStreakMax;
    public int Wins;
    public int Losts;
    public int Battles;
    public int WinRate;
    public List<int> Achievement;
    public List<int> AchievementObtained;
}