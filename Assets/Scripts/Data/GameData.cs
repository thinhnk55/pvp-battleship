using Authentication;
using Framework;
using Newtonsoft.Json;
using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameData : PDataBlock<GameData>
{
    #region VERSION_CONFIG
    [SerializeField] public int versionConfig; public static int VersionConfig { get { return Instance.versionConfig; } set { Instance.versionConfig = value; } }
    [SerializeField] public int versionShopConfig; public static int VersionShopConfig { get { return Instance.versionShopConfig; } set { Instance.versionShopConfig = value; } }
    [SerializeField] public int versionAchievementConfig; public static int VersionAchievementConfig { get { return Instance.versionAchievementConfig; } set { Instance.versionAchievementConfig = value; } }
    [SerializeField] public int versionRoyalPassConfig; public static int VersionRoyalPassConfig { get { return Instance.versionRoyalPassConfig; } set { Instance.versionRoyalPassConfig = value; } }
    [SerializeField] public int versionGiftConfig; public static int VersionGiftConfig { get { return Instance.versionGiftConfig; } set { Instance.versionGiftConfig = value; } }
    [SerializeField] public int versionLeaderBoardConfig; public static int VersionLeaderBoardConfig { get { return Instance.versionLeaderBoardConfig; } set { Instance.versionLeaderBoardConfig = value; } }
    #endregion
    [SerializeField] public BetData[] bets; public static BetData[] Bets { get { return Instance.bets; } set { Instance.bets = value; } }
    [SerializeField] public int progressGift; public static int ProgressGift { get { return Instance.progressGift; } set { Instance.progressGift = value % 6; } }
    [SerializeField] public int beriBonusAmount; public static int BeriBonusAmount { get { return Instance.beriBonusAmount; } set { Instance.beriBonusAmount = value; } }
    [SerializeField] public string text; public static string Text { get { return Instance.text; } set { Instance.text = value; } }
    [SerializeField] public int isBuyDiamondFirst; public static int IsBuyDiamondFirst { get { return Instance.isBuyDiamondFirst; } set { Instance.isBuyDiamondFirst = value; } }
    [JsonIgnore][SerializeField] public ProfileData player; public static ProfileData Player { get { return Instance.player; } set { Instance.player = value; } }
    [JsonIgnore][SerializeField] public ProfileData opponent; public static ProfileData Opponent { get { return Instance.opponent; } set { Instance.opponent = value; } }
    [SerializeField] public List<int> giftConfig; public static List<int> GiftConfig { get { return Instance.giftConfig; } set { Instance.giftConfig = value; } }
    [SerializeField] public List<int> rankMilestone; public static List<int> RankMilestone
    {
        get
        {
            if (Instance.rankMilestone.IsNullOrEmpty())
            {
                Instance.rankMilestone = new List<int>();
                for (int i = 0; i < GameData.RankConfigs.Count; i++)
                {
                    Instance.rankMilestone.Add(GameData.RankConfigs[i].Point);
                }
            }
            return Instance.rankMilestone;
        }
        set { Instance.rankMilestone = value; }
    }
    [SerializeField] public Dictionary<TransactionType, List<TransactionInfo>> transactionConfigs; public static Dictionary<TransactionType, List<TransactionInfo>> TransactionConfigs { get { return Instance.transactionConfigs; } set { Instance.transactionConfigs = value; } }
    [SerializeField] public List<RankConfig> rankConfigs; public static List<RankConfig> RankConfigs { get { return Instance.rankConfigs; } set { Instance.rankConfigs = value; } }
    [SerializeField] public PDataUnit<int> rocketCount; public static PDataUnit<int> RocketCount { get { return Instance.rocketCount; } set { Instance.rocketCount = value; } }
    [SerializeField] public List<TreasureConfig> treasureConfigs; public static List<TreasureConfig> TreasureConfigs { get { return Instance.treasureConfigs; } set { Instance.treasureConfigs = value; } }
    [SerializeField] public JoinTreasureRoom joinTreasureRoom; public static JoinTreasureRoom JoinTreasureRoom { get { return Instance.joinTreasureRoom; } set { Instance.joinTreasureRoom = value; } }
    [SerializeField] public Dictionary<AchievementType, AchievementInfo> achievementConfig; public static Dictionary<AchievementType, AchievementInfo> AchievementConfig { get { return Instance.achievementConfig; } set { Instance.achievementConfig = value; } }
    [SerializeField] public int luckyShotCoolDown; public static int LuckyShotCoolDown { get { return Instance.luckyShotCoolDown; } set { Instance.luckyShotCoolDown = value; } }
    [SerializeField] public int rankReceiveCoolDown; public static int RankReceiveCoolDown { get { return Instance.rankReceiveCoolDown; } set { Instance.rankReceiveCoolDown = value; } }
    [SerializeField] public int giftCoolDown; public static int GiftCoolDown { get { return Instance.giftCoolDown; } set { Instance.giftCoolDown = value; } }
    [SerializeField] public int changeQuestCoolDown; public static int ChangeQuestCoolDown { get { return Instance.changeQuestCoolDown; } set { Instance.changeQuestCoolDown = value; } }
    [SerializeField] public RoyalPass royalPass; public static RoyalPass RoyalPass { get { return Instance.royalPass; } set { Instance.royalPass = value; } }
    [SerializeField] public List<BoardInfo> listBoard; public static List<BoardInfo> ListBoard { get { return Instance.listBoard; } set { Instance.listBoard = value; } }
    [SerializeField] public bool[] acceptLoginTerm; public static bool[] AcceptLoginTerm { get { return Instance.acceptLoginTerm; } set { Instance.acceptLoginTerm = value; } }
    [SerializeField] public bool starter; public static bool Starter { get { return Instance.starter; } set { Instance.starter = value; } }
    [SerializeField] public bool starterShow; public static bool StarterShow { get { return Instance.starterShow; } set { Instance.starterShow = value; } }
    [SerializeField] public List<int> tutorial; public static List<int> Tutorial { get { return Instance.tutorial; } set { Instance.tutorial = value; } }
    [SerializeField] public LeaderBoard leaderBoard; public static LeaderBoard LeaderBoard { get { return Instance.leaderBoard; } set { Instance.leaderBoard = value; } }
    [SerializeField] public SocialAuthType typeLogin; public static SocialAuthType TypeLogin { get { return Instance.typeLogin; } set { Instance.typeLogin = value; } }
    protected override void Init()
    {
        base.Init();
        Instance.player = Instance.player ?? new ProfileData();
        Instance.opponent = Instance.opponent ?? new ProfileData();
        Instance.opponent.AchievementSelected = Instance.opponent.AchievementSelected ?? new int[3] { -1, -1, -1 };
        Instance.opponent.AchievementProgress = Instance.opponent.AchievementProgress ?? new List<int>();

        Instance.achievementConfig = Instance.achievementConfig ?? new Dictionary<AchievementType, AchievementInfo>();
        Instance.giftConfig = Instance.giftConfig ?? new List<int>();
        Instance.transactionConfigs = Instance.transactionConfigs ?? new Dictionary<TransactionType, List<TransactionInfo>>();
        Instance.rankConfigs = Instance.rankConfigs ?? new List<RankConfig>();
        Instance.rocketCount = new PDataUnit<int>(0);
        Instance.treasureConfigs = Instance.treasureConfigs ?? new List<TreasureConfig>();
        Instance.joinTreasureRoom = Instance.joinTreasureRoom ?? new JoinTreasureRoom();
        Instance.joinTreasureRoom.Board = Instance.joinTreasureRoom.Board ?? new List<List<int>>();
        Instance.royalPass = Instance.royalPass ?? new RoyalPass();
        Instance.listBoard = Instance.listBoard ?? new List<BoardInfo>() { new BoardInfo() { boardInfo = new List<ShipInfo>()}, new BoardInfo() { boardInfo = new List<ShipInfo>() }, new BoardInfo() { boardInfo = new List<ShipInfo>() },
            new BoardInfo(){ boardInfo = new List<ShipInfo>()} , new BoardInfo(){ boardInfo = new List<ShipInfo>()} , new BoardInfo(){ boardInfo = new List<ShipInfo>()} };
        Instance.leaderBoard = Instance.leaderBoard ?? new LeaderBoard();
        Instance.acceptLoginTerm = Instance.acceptLoginTerm ?? new bool[2] { true, true };
        if (ServeData.IsTutorialComplete == false)
        {
            Instance.tutorial = new List<int>() { 0, 0, 0, 0 };
        }
        else
        {
            Instance.tutorial = new List<int>() { 1, 1, 1, 1 };
        }
    }
}


[Serializable]
public class ProfileData
{
    public int UserId;
    public PDataUnit<string> Username;
    public PDataUnit<int> Avatar;
    public PDataUnit<int> FrameAvatar;
    public PDataUnit<int> BattleField;
    public PDataUnit<int> SkinShip;
    [SerializeField] public int point; public int Point { get { return point; } set { point = value; } }
    [SerializeField] public int rank;
    public int Rank
    {
        get
        {
            rank = GameData.RankMilestone.GetMileStone(point);
            return rank;
        }
    }
    public int PerfectGame
    {
        get
        {
            if (!AchievementProgress.IsNullOrEmpty())
                return AchievementProgress[(int)AchievementType.PERFECT_GAME];
            else return 0;
        }
    }
    [SerializeField] public int winStreak; public int WinStreak { get { return winStreak; } set { winStreak = value; if (winStreakMax.HasValue && winStreak > winStreakMax.Value) WinStreakMax = winStreak; } }

    [SerializeField] public int? winStreakMax = null;
    public int WinStreakMax
    {
        get
        {
            if (winStreakMax == null)
            {
                if (!AchievementProgress.IsNullOrEmpty()) return AchievementProgress[(int)AchievementType.WIN_STREAK_MAX];
                else return 0;
            }
            else
            {
                return winStreakMax.Value;
            }
        }
        set { AchievementProgress[(int)AchievementType.WIN_STREAK_MAX] = value; }
    }
    [SerializeField] public int loseStreak; public int LoseStreak { get; set; }
    [SerializeField] public int loseStreakMax; public int LoseStreakMax { get; set; }
    [SerializeField] public int wins; public int Wins
    {
        get { return (!AchievementProgress.IsNullOrEmpty()) ? AchievementProgress[(int)AchievementType.WIN] : 0; }
        set { AchievementProgress[(int)AchievementType.WIN] = value; }
    }
    [SerializeField] public int losts; public int Losts { get { return losts; } set { losts = value; } }
    public int Battles { get => Wins + Losts; }
    [SerializeField] public float winRate; public float WinRate { get { return Battles == 0 ? 0 : ((float)Wins / Battles); } }
    public int[] AchievementSelected;
    public List<int> AchievementObtained;
    public List<int> AchievementProgress;

    public static ProfileData FromJson(ProfileData profileData, JSONNode data)
    {
        profileData.UserId = data["d"]["u"].AsInt;
        profileData.Username = new PDataUnit<string>(data["d"]["n"]);
        profileData.Point = data["d"]["e"].AsInt;
        profileData.Avatar = new PDataUnit<int>(data["d"]["a"]["k"]["a"].AsInt);
        profileData.FrameAvatar = new PDataUnit<int>(data["d"]["a"]["k"]["f"].AsInt);
        profileData.BattleField = new PDataUnit<int>(data["d"]["a"]["k"]["b"].AsInt);
        profileData.SkinShip = new PDataUnit<int>(0); // new PDataUnit<int>(int.Parse(data["profile"]["sk"]));
        PNonConsumableType.AVATAR.SetValue(data["d"]["a"]["k"]["al"].ToListInt());
        PNonConsumableType.AVATAR_FRAME.SetValue(data["d"]["a"]["k"]["fl"].ToListInt());
        PNonConsumableType.BATTLE_FIELD.SetValue(data["d"]["a"]["k"]["bl"].ToListInt());
        StatisticTracker.quests[StatisticType.SHIP_DESTROY_CONSECUTIVE_MAX] = new(data["d"]["s"]["k"].AsInt);
        // statistic / achievement
        //profileData.Achievement = data["statistics"]["achie"].ToList();
        profileData.AchievementProgress = new List<int>() { data["d"]["s"]["t"].AsInt,
            data["d"]["s"]["a"].AsInt,
            data["d"]["s"]["w"].AsInt,
            data["d"]["s"]["mw"].AsInt,
            data["d"]["s"]["k"].AsInt,
            data["d"]["s"]["s"][0].AsInt,
            data["d"]["s"]["s"][1].AsInt,
            data["d"]["s"]["s"][2].AsInt,
            data["d"]["s"]["s"][3].AsInt,
            data["d"]["s"]["wa"].AsInt,
            data["d"]["s"]["w1"].AsInt,
            data["d"]["s"]["d"].AsInt,
            data["d"]["s"]["f"].AsInt,
            data["d"]["s"]["b"].AsInt,
        };
        profileData.Losts = data["d"]["s"]["l"].AsInt;
        profileData.WinStreak = data["d"]["s"]["ws"].AsInt;
        //

        profileData.AchievementObtained = new List<int>(data["d"]["a"]["a"]["r"].Count);
        for (int i = 0; i < Enum.GetValues(typeof(AchievementType)).Length; i++)
        {
            profileData.AchievementObtained.Add(0);
        }

        for (int i = 0; i < data["d"]["a"]["a"]["r"].Count; i++)
        {
            profileData.AchievementObtained[data["d"]["a"]["a"]["r"][i]["a"].AsInt] = data["d"]["a"]["a"]["r"][i]["l"].AsInt + 1;
        }
        //profileData.AchievementObtained = data["statistics"]["achie_r"].ToList();
        profileData.AchievementSelected = new int[3] { -1, -1, -1 };
        for (int i = 0; i < data["d"]["a"]["a"]["a"].Count; i++)
        {
            profileData.AchievementSelected[i] = data["d"]["a"]["a"]["a"][i].AsInt;
        }
        return profileData;
    }
    public static ProfileData FromJsonOpponent(ProfileData profileData, JSONNode data)
    {
        profileData.Username = new PDataUnit<string>(data["n"]);
        profileData.Point = data["e"].AsInt;
        profileData.Avatar = new PDataUnit<int>(data["a"]["k"]["a"].AsInt);
        profileData.FrameAvatar = new PDataUnit<int>(data["a"]["k"]["f"].AsInt);
        profileData.BattleField = new PDataUnit<int>(data["a"]["k"]["b"].AsInt);
        profileData.SkinShip = new PDataUnit<int>(0);//= new PDataUnit<int>(int.Parse(data["sk"]));
        //
        profileData.AchievementProgress = new List<int>() {
            data["s"]["t"].AsInt,
            data["s"]["a"].AsInt,
            data["s"]["w"].AsInt,
            data["s"]["wm"].AsInt,
            data["s"]["k"].AsInt,
            data["s"]["s"][0].AsInt,
            data["s"]["s"][1].AsInt,
            data["s"]["s"][2].AsInt,
            data["s"]["s"][3].AsInt,
            data["s"]["wa"].AsInt,
            data["s"]["w1"].AsInt,
            data["s"]["d"].AsInt,
            data["s"]["f"].AsInt,
            data["s"]["b"].AsInt,
        };
        profileData.Wins = data["s"]["w"].AsInt;
        profileData.Losts = data["s"]["l"].AsInt;
        profileData.WinStreak = data["s"]["ws"].AsInt;
        profileData.WinStreakMax = data["s"]["mw"].AsInt;

        profileData.AchievementObtained = new List<int>(data["a"]["a"]["r"].Count);
        for (int i = 0; i < Enum.GetValues(typeof(AchievementType)).Length; i++)
        {
            profileData.AchievementObtained.Add(0);
        }

        for (int i = 0; i < data["a"]["a"]["r"].Count; i++)
        {
            profileData.AchievementObtained[data["a"]["a"]["r"][i]["a"].AsInt] = data["a"]["a"]["r"][i]["l"].AsInt + 1;
        }
        //profileData.AchievementObtained = data["statistics"]["achie_r"].ToList();
        profileData.AchievementSelected = new int[3] { -1, -1, -1 };
        for (int i = 0; i < data["a"]["a"]["a"].Count; i++)
        {
            profileData.AchievementSelected[i] = data["a"]["a"]["a"][i].AsInt;
        }
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
        foreach (var item in AchievementProgress)
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

    public static List<RankConfig> ListFromJson(JSONNode data)
    {
        List<RankConfig> rankConfigs = new List<RankConfig>();
        for (int i = 0; i < data["level"].Count; i++)
        {
            RankConfig rankConfig = new RankConfig()
            {
                Point = int.Parse(data["level"][i]),
                Title = GameConfig.RankNames[i],
                Reward = int.Parse(data["bonus"][i])
            };
            rankConfigs.Add(rankConfig);

        }
        GameData.RankReceiveCoolDown = int.Parse(data["bonus_period"]) / 1000;
        Timer<RankCollection>.Instance.TriggerInterval_Sec = GameData.RankReceiveCoolDown;
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
    BET,
}

[Serializable]
public class TreasureConfig
{
    public int Id;
    public int PrizeAmount;
    public int InitPrize;
}

[Serializable]
//Goi tin duoc server tra ve khi join vao treasurehunt room
public class JoinTreasureRoom
{
    public int Id;
    public int RoomId;
    public int IsSuccess;
    public int CurrentPrize;
    public List<List<int>> Board; //Trang thai cac o trong dao kho bau
    public int ShotCost;
    public int InitPrize;
}

