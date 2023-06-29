using Framework;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameData : PDataBlock<GameData>
{
    [SerializeField] private int progressGift; public static int ProgressGift { get { return Instance.progressGift; } set { Instance.progressGift = value % 6; } }
    [SerializeField] private string text; public static string Text { get { return Instance.text; } set { Instance.text = value; } }
    [SerializeField] private List<int> versions; public static List<int> Versions { get { return Instance.versions; } set { Instance.versions = value; } }
    [SerializeField] private int isBuyDiamondFirst; public static int IsBuyDiamondFirst { get { return Instance.isBuyDiamondFirst; } set { Instance.isBuyDiamondFirst = value; } }
    [SerializeField] private ProfileData player; public static ProfileData Player { get { return Instance.player; } set { Instance.player = value; } }
    [SerializeField] private ProfileData opponent; public static ProfileData Opponent { get { return Instance.opponent; } set { Instance.opponent = value; } }
    [SerializeField] private List<int> giftConfig; public static List<int> GiftConfig { get { return Instance.giftConfig; } set { Instance.giftConfig = value; } }
    [SerializeField] private List<int> luckyShotConfig; public static List<int> LuckyShotConfig { get { return Instance.luckyShotConfig; } set { Instance.luckyShotConfig = value; } }
    [SerializeField] private List<int> luckyShotResult; public static List<int> LuckyShotResult { get { return Instance.luckyShotResult; } set { Instance.luckyShotResult = value; } }
    [SerializeField] private Dictionary<TransactionType,List<TransactionInfo>> transactionConfigs; public static Dictionary<TransactionType, List<TransactionInfo>> TransactionConfigs { get { return Instance.transactionConfigs; } set { Instance.transactionConfigs = value; } }
    [SerializeField] private List<RankConfig> rankConfigs; public static List<RankConfig> RankConfigs { get { return Instance.rankConfigs; } set { Instance.rankConfigs = value; } }
    [SerializeField] private PDataUnit<int> rocketCount; public static PDataUnit<int> RocketCount { get { return Instance.rocketCount; } set { Instance.rocketCount = value; } }
    [SerializeField] private List<TreasureConfig> treasureConfigs; public static List<TreasureConfig> TreasureConfigs { get { return Instance.treasureConfigs; } set { Instance.treasureConfigs = value; } }
    [SerializeField] private JoinTreasureRoom joinTreasureRoom; public static JoinTreasureRoom JoinTreasureRoom { get { return Instance.joinTreasureRoom; } set { Instance.joinTreasureRoom = value; } }
    [SerializeField] private Dictionary<AchievementType, AchievementInfo> achievementConfig; public static Dictionary<AchievementType, AchievementInfo> AchievementConfig { get { return Instance.achievementConfig; } set { Instance.achievementConfig = value; } }


    protected override void Init()
    {
        base.Init();
        Instance.player = Instance.player ?? new ProfileData();
        Instance.player.Achievement = Instance.player.Achievement ?? new List<int>();
        Instance.player.AchievementObtained = Instance.player.AchievementObtained ?? new List<int>();
        Instance.achievementConfig = Instance.achievementConfig ?? new Dictionary<AchievementType, AchievementInfo>();
        Instance.luckyShotResult = Instance.luckyShotResult ?? new List<int>();
        Instance.luckyShotConfig = Instance.luckyShotConfig ?? new List<int>();
        Instance.giftConfig = Instance.giftConfig ?? new List<int>();
        Instance.player.AchievementSelected = Instance.player.AchievementSelected ?? new int[3] { -1, -1, -1 };
        Instance.transactionConfigs = Instance.transactionConfigs ?? new Dictionary<TransactionType, List<TransactionInfo>>();
        Instance.rankConfigs = Instance.rankConfigs ?? new List<RankConfig>();
        Instance.rocketCount = new PDataUnit<int>(0);
        Instance.versions = Instance.versions ?? new List<int>(8) { 0,0,0,0,0,0,0,0};
        Instance.treasureConfigs = Instance.treasureConfigs ?? new List<TreasureConfig>();
        Instance.joinTreasureRoom = Instance.joinTreasureRoom ?? new JoinTreasureRoom();
        Instance.joinTreasureRoom.Board = Instance.joinTreasureRoom.Board ?? new List<List<int>>();
    }
}


[Serializable]
public class ProfileData
{
    public PDataUnit<string> Username;
    public PDataUnit<int> Avatar;
    public PDataUnit<int> FrameAvatar;
    public PDataUnit<int> BattleField;
    public PDataUnit<int> SkinShip;
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
    public int PerfectGame { get { return GameData.AchievementConfig[AchievementType.TACTICAL_GENIUS].Progress; } }
    public int WinStreak;
    public int WinStreakMax { get { return GameData.AchievementConfig[AchievementType.DOMINATOR].Progress; } }
    [SerializeField] private int wins; public int Wins { get { return wins; } set { wins = value; winRate = wins / (wins + losts + 0.001f); } }
    [SerializeField] private int losts; public int Losts { get { return losts; } set { losts = value; winRate = wins / (wins + losts + 0.001f); } }
    public int Battles;
    [SerializeField] private float winRate; public float WinRate { get { return winRate; } }
    public List<int> Achievement;
    public int []AchievementSelected;
    public List<int> AchievementObtained;
    public List<int> AchievementProgress;


    public static ProfileData FromJson(ProfileData profileData, JSONNode data)
    {
        profileData.Username = new PDataUnit<string>(data["profile"]["n"]);
        profileData.Point = int.Parse(data["profile"]["p"]);
        profileData.Avatar = new PDataUnit<int>(int.Parse(data["profile"]["a"]));
        profileData.FrameAvatar = new PDataUnit<int>(int.Parse(data["profile"]["f"]));
        profileData.BattleField = new PDataUnit<int>(int.Parse(data["profile"]["ba"]));
        profileData.SkinShip = new PDataUnit<int>(int.Parse(data["profile"]["sk"]));
        profileData.WinStreak = int.Parse(data["statistics"]["wSN"]);
        profileData.Wins = int.Parse(data["statistics"]["wC"]);
        profileData.Losts = int.Parse(data["statistics"]["lC"]);
        profileData.Battles = profileData.Wins + profileData.Losts;
        profileData.Achievement = data["statistics"]["achie"].ToList();
        profileData.AchievementObtained = data["statistics"]["achie_r"].ToList();
        profileData.AchievementSelected = data["statistics"]["outst"].ToList().ToArray();
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
    public int Point;
    public int Reward;

    public static List<RankConfig> ListFromJson(JSONNode json)
    {
        List<RankConfig > rankConfigs = new List<RankConfig>();
        for (int i = 0; i < json["p"].Count; i++)
        {
            RankConfig rankConfig = new RankConfig()
            {
                Point = int.Parse(json["p"][i]),
                Title = json["n"][i],
                Reward = int.Parse(json["list"][i])
            };
            rankConfigs.Add(rankConfig);

        }
        return rankConfigs;
    }

}

public enum ConfigVersion
{
    RANK,
    ACHIEVEMENT,
    LUCKY_SHOT,
    GIFT,
    SHOP,
    TREASURE,
    COUNT_DOWN,
    ROYAL_PASS,
}

[Serializable]
public class TreasureConfig
{
    public int Id;
    public int PrizeAmount;
}

[Serializable]
//Goi tin duoc server tra ve khi join vao treasurehunt room
public class JoinTreasureRoom
{
    public int Id;
    public int IsSuccess;
    public int CurrentPrize;
    public List<List<int>> Board; //Trang thai cac o trong dao kho bau
}

