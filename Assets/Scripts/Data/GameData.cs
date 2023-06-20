using Framework;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameData : PDataBlock<GameData>
{
    [SerializeField] private int version; public static int Version { get { return Instance.version; } set { Instance.version = value; } }
    [SerializeField] private int isBuyDiamondFirst; public static int IsBuyDiamondFirst { get { return Instance.isBuyDiamondFirst; } set { Instance.isBuyDiamondFirst = value; } }
    [SerializeField] private ProfileData player; public static ProfileData Player { get { return Instance.player; } set { Instance.player = value; } }
    [SerializeField] private ProfileData opponent; public static ProfileData Opponent { get { return Instance.opponent; } set { Instance.opponent = value; } }
    [SerializeField] private List<int> luckyShotConfig; public static List<int> LuckyShotConfig { get { return Instance.luckyShotConfig; } set { Instance.luckyShotConfig = value; } }
    [SerializeField] private List<int> luckyShotResult; public static List<int> LuckyShotResult { get { return Instance.luckyShotResult; } set { Instance.luckyShotResult = value; } }


    [SerializeField] private List<RankConfig> rankConfigs; public static List<RankConfig> RankConfigs { get { return Instance.rankConfigs; } set { Instance.rankConfigs = value; } }
    [SerializeField] private int rocketCount; public static int RocketCount { get { return Instance.rocketCount; } set { Instance.rocketCount = value; } }
    [SerializeField] private long lastLuckyShot; public static long LastLuckyShot { get { return Instance.lastLuckyShot; } set { Instance.lastLuckyShot = value; } }
    [SerializeField] private int restoreRocketInterval; public static int RestoreRocketInterval { get { return Instance.restoreRocketInterval; } set { Instance.restoreRocketInterval = value; } }
    [SerializeField] private long lastSalaryObtained; public static long LastSalaryObtained { get { return Instance.lastSalaryObtained; } set { Instance.lastSalaryObtained = value; } }
    [SerializeField] private long lastDailyReward; public static long LastDailyReward { get { return Instance.lastDailyReward; } set { Instance.lastDailyReward = value; } }
    [SerializeField] private int restoreDailyInterval; public static int RestoreDailyInterval { get { return Instance.restoreDailyInterval; } set { Instance.restoreDailyInterval = value; } }


    protected override void Init()
    {
        base.Init();
        Instance.lastLuckyShot = Instance.lastLuckyShot != 0 ?  Instance.lastLuckyShot : 0;
        Instance.player.Achievement = Instance.player.Achievement ?? new List<int>();
        Instance.player.AchievementObtained = Instance.player.AchievementObtained ?? new List<int>();
        Instance.player.AchievementConfig = Instance.player.AchievementConfig ?? new Dictionary<AchievementType, AchievementInfo>();
        Instance.luckyShotResult = Instance.luckyShotResult ?? new List<int>();
        Instance.luckyShotConfig = Instance.luckyShotConfig ?? new List<int>();
        Instance.player.AchievementSelected = Instance.player.AchievementSelected ?? new int[3] { -1, -1, -1 };
        Instance.rankConfigs = Instance.rankConfigs ?? new List<RankConfig>();
        restoreRocketInterval = 60;
    }
}


[Serializable]
public struct ProfileData
{
    public string Username;
    public int Avatar;
    [SerializeField] private int point; public int Point { get { return point; } set { point = value; } }
    [SerializeField] private int rank; 
    public int Rank { get {
            List<int> rankMilestone = new List<int>();
            for (int i = 0; i < GameData.RankConfigs.Count; i++)
            {
                rankMilestone.Add(GameData.RankConfigs[i].Point);
            }
            rank = rankMilestone.GetMileStone(point);
            return rank; 
        }}
    public int PerfectGame { get { return GameData.Player.AchievementConfig[AchievementType.TACTICAL_GENIUS].Progress; } }
    public int WinStreak;
    public int WinStreakMax { get { return GameData.Player.AchievementConfig[AchievementType.DOMINATOR].Progress; } }
    [SerializeField] private int wins; public int Wins { get { return wins; } set { wins = value; winRate = wins / (wins + losts + 0.001f); } }
    [SerializeField] private int losts; public int Losts { get { return losts; } set { losts = value; winRate = wins / (wins + losts + 0.001f); } }
    public int Battles;
    [SerializeField] private float winRate; public float WinRate { get { return winRate; } }
    public List<int> Achievement;
    public int []AchievementSelected;
    public List<int> AchievementObtained;
    public Dictionary<AchievementType, AchievementInfo> AchievementConfig;


    public static ProfileData FromJson(ref ProfileData profileData, JSONNode data)
    {
        profileData.Username = data["n"];
        profileData.Point = int.Parse(data["p"]);
        profileData.Avatar = int.Parse(data["a"]);
        profileData.WinStreak = int.Parse(data["wSN"]);
        profileData.Wins = int.Parse(data["wC"]);
        profileData.Losts = int.Parse(data["lC"]);
        profileData.Battles = profileData.Wins + profileData.Losts;
        profileData.Achievement = data["achie"].ToList();
        profileData.AchievementObtained = data["achie_r"].ToList();
        profileData.AchievementSelected = profileData.AchievementSelected ?? new int[3] {-1,-1,-1};
        return profileData;
    }
    public override string ToString()
    {
        string s = "";
        s += "achieObtained: ";
        foreach (var item in AchievementObtained)
        {
            s += item.ToString() + "_";
        }
        s += "achieCompleted: ";
        foreach (var item in Achievement)
        {
            s += item.ToString() + "_";
        }
        s += "achieConfig";
        //foreach (var item in AchievementConfig)
        //{
        //    s += item.Value.ToString() + "_";
        //}
        return s;
    }
}

[Serializable]
public class RankConfig
{
    public string Title;
    public Sprite Icon;
    public int Point;

}