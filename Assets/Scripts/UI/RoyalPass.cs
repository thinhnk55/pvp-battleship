using Framework;
using SimpleJSON;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum RoyalPassNormalQuestType
{
    SHIP_DESTROY = 0,
    PLAY_COUNT = 14,
    SHIP_1_DESTROY = 5,
    SHIP_2_DESTROY = 6,
    SHIP_3_DESTROY = 7,
    SHIP_4_DESTROY = 8,
    WIN_COUNT = 2,
    LUCKY_SHOT_COUNT = 15,
    DESTROY_SHIP_CONSECUTIVE_2 = 16,
}
public enum RoyalPassSeasonQuestType
{
    WIN_COUNT = 2,
    SHIP_1_DESTROY = 5,
    SHIP_2_DESTROY = 6,
    SHIP_3_DESTROY = 7,
    SHIP_4_DESTROY = 8,
    LUCKY_SHOT_COUNT = 15,
    PLAY_COUNT = 14,
    DESTROY_SHIP_CONSECUTIVE_3 = 17,
    ALIVE_1_SHIP = 10,
}
public enum RoyalPassQuestType
{
    SHIP_DESTROY = 0,
    PLAY_COUNT = 14,
    SHIP_1_DESTROY = 5,
    SHIP_2_DESTROY = 6,
    SHIP_3_DESTROY = 7,
    SHIP_4_DESTROY = 8,
    WIN_COUNT = 2,
    LUCKY_SHOT_COUNT = 15,
    DESTROY_SHIP_CONSECUTIVE_2 = 16,
    DESTROY_SHIP_CONSECUTIVE_3 = 17,
    ALIVE_1_SHIP = 10,
}
public class RoyalPass
{

    public class RoyalPassQuest
    {
        public int Reward;
        public StatisticType Type;
        public int Require;
    }
    public int Season;
    public int Version;
    public PDataUnit<int> Point = new PDataUnit<int>(0);
    public int PointPerLevel;
    public int Level { get => Mathf.Clamp(Point.Data / PointPerLevel, 0, RewardElites.Length - 1); }
    public PDataUnit<HashSet<int>> NormalObtains = new PDataUnit<HashSet<int>>(new HashSet<int>());
    public int[] NormalProgresses;
    public List<Framework.GoodInfo>[] RewardNormals;

    public int[] EliteProgresses;
    public PDataUnit<HashSet<int>> EliteObtains = new PDataUnit<HashSet<int>>(new HashSet<int>());
    public List<Framework.GoodInfo>[] RewardElites;

    public RoyalPassQuest[] Quests;
    public PDataUnit<int[]> CurrentQuests;
    public int[] CurrentQuestsProgress;
    public RoyalPassQuest[] SeasonQuests;
    public int[] SeasonQuestsProgress;
    public PDataUnit<HashSet<int>> SeasonQuestsObtained = new PDataUnit<HashSet<int>>(new HashSet<int>());
    static int currentIndexQuestChange;
    public static string GetDescription(StatisticType type, int value)
    {
        string s = "";
        switch (type)
        {
            case StatisticType.SHIP_DESTROY:
                s = $"Destroy {value} ships";
                break;
            case StatisticType.WIN_COUNT:
                s = $"Win {value} battles";
                break;
            case StatisticType.SHIP_0_DESTROY:
                s = $"Destroy {value} single-deck ships";
                break;
            case StatisticType.SHIP_1_DESTROY:
                s = $"Destroy {value} two-deck ships";
                break;
            case StatisticType.SHIP_2_DESTROY:
                s = $"Destroy {value} three-deck ships";
                break;
            case StatisticType.SHIP_3_DESTROY:
                s = $"Destroy {value} four-deck ships";
                break;
            case StatisticType.PLAY_COUNT:
                s = $"Play {value} batlles";
                break;
            case StatisticType.LUCKY_SHOT_COUNT:
                s = $"Play {value} times Lucky Shot";
                break;
            case StatisticType.SHIP_DESTROY_CONSECUTIVE_2:
                s = $"Destroy 2 ships consecutively in {value} battles";
                break;
            case StatisticType.DESTROY_SHIP_CONSECUTIVE_3:
                s = $"Destroy 3 ships consecutively in {value} battles";
                break;
            case StatisticType.ALIVE_1_SHIP:
                s = $"Win battle when you have only one ship left that hasn't been destroyed {value} time";
                break;
            default:
                break;
        }
        return s;
    }
    public static RoyalPass ConfigFromJson(RoyalPass royalPass, JSONNode json)
    {
        Timer<RoyalPass>.Instance.BeginPoint = long.Parse(json["start_timestamp"]).NowFrom0001From1970();
        Timer<RoyalPass>.Instance.TriggerInterval_Sec = (int)(long.Parse(json["end_timestamp"]).NowFrom0001From1970() - Timer<RoyalPass>.Instance.BeginPoint);
        royalPass.PointPerLevel = int.Parse(json["point_per_milestone"]);
        royalPass.SeasonQuests = new RoyalPassQuest[json["season_quest"].Count];
        for (int i = 0; i < json["season_quest"].Count; i++)
        {
            RoyalPassQuest quest = new RoyalPassQuest()
            {
                Reward = int.Parse(json["season_quest"][i]["reward"]),
                Type = (StatisticType)Enum.GetValues(typeof(RoyalPassQuestType)).GetValue(json["season_quest"][i]["type"].AsInt),
                Require = int.Parse(json["season_quest"][i]["require"]),
            };
            royalPass.SeasonQuests[i] = quest;
        }
        royalPass.Quests = new RoyalPassQuest[json["daily_quest"].Count];
        for (int i = 0; i < json["daily_quest"].Count; i++)
        {
            RoyalPassQuest quest = new RoyalPassQuest()
            {
                Reward = int.Parse(json["daily_quest"][i]["reward"]),
                Type = (StatisticType)Enum.GetValues(typeof(RoyalPassQuestType)).GetValue(json["daily_quest"][i]["type"].AsInt),
                Require = int.Parse(json["daily_quest"][i]["require"]),
            };
            royalPass.Quests[i] = quest;
        }
        royalPass.RewardNormals = new List<Framework.GoodInfo>[json["free"].Count];
        for (int i = 0; i < json["free"].Count; i++)
        {
            royalPass.RewardNormals[i] = new List<Framework.GoodInfo>();
            for (int j = 0; j < json["free"][i]["reward"].Count; j++)
            {
                royalPass.RewardNormals[i].Add(new Framework.GoodInfo()
                {
                    Value = int.Parse(json["free"][i]["reward"][j]),
                    Type = int.Parse(json["free"][i]["reward_type"][j])
                });
            }

        }
        royalPass.RewardElites = new List<Framework.GoodInfo>[json["elite"].Count];
        for (int i = 0; i < json["elite"].Count; i++)
        {
            royalPass.RewardElites[i] = new List<Framework.GoodInfo>();
            for (int j = 0; j < json["elite"][i]["reward"].Count; j++)
            {
                royalPass.RewardElites[i].Add(new Framework.GoodInfo()
                {
                    Value = int.Parse(json["elite"][i]["reward"][j]),
                    Type = int.Parse(json["elite"][i]["reward_type"][j])
                });
            }
        }
        return royalPass;
    }
    public static void DataFromJson(RoyalPass royalPass, JSONNode json)
    {
        if (royalPass.SeasonQuests != null && royalPass.SeasonQuests.Length > 0)
        {
            try
            {
                royalPass.Season = json["i"].AsInt;
                royalPass.Version = 0;
                royalPass.SeasonQuestsObtained = new PDataUnit<HashSet<int>>(new HashSet<int>());
                if (json["s"]["r"].Count > 0)
                {
                    royalPass.SeasonQuestsObtained.Data.AddRange(json["s"]["r"].ToListInt(true));
                }
                royalPass.SeasonQuestsProgress = new int[royalPass.SeasonQuests.Length]; // config come after data so init 20
                for (int i = 0; i < royalPass.SeasonQuests.Length; i++)
                {
                    int _i = i;
                    switch (royalPass.SeasonQuests[i].Type)
                    {
                        case StatisticType.WIN_COUNT:
                            royalPass.SeasonQuestsProgress[i] = json["s"]["w"].AsInt;
                            break;
                        case StatisticType.SHIP_0_DESTROY:
                            royalPass.SeasonQuestsProgress[i] = json["s"]["s"][0].AsInt;
                            break;
                        case StatisticType.SHIP_1_DESTROY:
                            royalPass.SeasonQuestsProgress[i] = json["s"]["s"][1].AsInt;
                            break;
                        case StatisticType.SHIP_2_DESTROY:
                            royalPass.SeasonQuestsProgress[i] = json["s"]["s"][2].AsInt;
                            break;
                        case StatisticType.SHIP_3_DESTROY:
                            royalPass.SeasonQuestsProgress[i] = json["s"]["s"][3].AsInt;
                            break;

                        case StatisticType.LUCKY_SHOT_COUNT:
                            royalPass.SeasonQuestsProgress[i] = json["s"]["l"].AsInt;
                            break;
                        case StatisticType.PLAY_COUNT:
                            royalPass.SeasonQuestsProgress[i] = json["s"]["p"].AsInt;
                            break;
                        case StatisticType.DESTROY_SHIP_CONSECUTIVE_3:
                            royalPass.SeasonQuestsProgress[i] = json["s"]["k3"].AsInt;
                            break;
                        case StatisticType.ALIVE_1_SHIP:
                            royalPass.SeasonQuestsProgress[i] = json["s"]["w1"].AsInt;
                            break;
                        default:
                            break;
                    }
                    StatisticTracker.RemoveAllListenerOnProgress((StatisticType)royalPass.SeasonQuests[_i].Type);
                    StatisticTracker.AddListenerOnProgress((StatisticType)royalPass.SeasonQuests[_i].Type, (o, n) =>
                    {
                        royalPass.SeasonQuestsProgress[_i] += (n - o);
                    });
                }
                royalPass.CurrentQuests = new PDataUnit<int[]>(new int[3] { -1, -1, -1 });
                royalPass.CurrentQuestsProgress = new int[json["d"]["q"].Count];
                royalPass.CurrentQuests.OnDataChanged += (o, n) =>
                {
                    for (int i = 0; i < n.Length; i++)
                    {
                        if (n[i] != o[i])
                        {
                            currentIndexQuestChange = i;
                            if (o[currentIndexQuestChange] > 0)
                                StatisticTracker.RemoveListenerOnProgress((StatisticType)Enum.GetValues(typeof(RoyalPassNormalQuestType)).GetValue(o[currentIndexQuestChange]), OnChangeQuestProgress);
                            if (n[currentIndexQuestChange] > 0)
                                StatisticTracker.AddListenerOnProgress((StatisticType)Enum.GetValues(typeof(RoyalPassNormalQuestType)).GetValue(n[currentIndexQuestChange]), OnChangeQuestProgress);
                        }
                    }
                };
                for (int i = 0; i < json["d"]["q"].Count; i++)
                {
                    int _i = i;
                    royalPass.CurrentQuests.Data[i] = json["d"]["q"][i].AsInt;
                    royalPass.CurrentQuestsProgress[i] = json["d"]["p"][i].AsInt;
                    StatisticTracker.RemoveAllListenerOnProgress((StatisticType)Enum.GetValues(typeof(RoyalPassNormalQuestType)).GetValue(royalPass.CurrentQuests.Data[_i]));
                    Debug.Log((StatisticType)Enum.GetValues(typeof(RoyalPassNormalQuestType)).GetValue(royalPass.CurrentQuests.Data[_i]));
                    Debug.Log(Enum.GetValues(typeof(RoyalPassNormalQuestType)).GetValue(royalPass.CurrentQuests.Data[_i]));
                    StatisticTracker.AddListenerOnProgress((StatisticType)Enum.GetValues(typeof(RoyalPassNormalQuestType)).GetValue(royalPass.CurrentQuests.Data[_i]), (o, n) =>
                        GameData.RoyalPass.CurrentQuestsProgress[_i] += (n - o));
                }

                royalPass.Point.Data = int.Parse(json["p"]);
                royalPass.EliteObtains = new PDataUnit<HashSet<int>>(new HashSet<int>());
                royalPass.EliteObtains.Data.AddRange(json["e"].ToListInt());
                royalPass.NormalObtains = new PDataUnit<HashSet<int>>(new HashSet<int>());
                royalPass.NormalObtains.Data.AddRange(json["f"].ToListInt());
            }
            catch (Exception e)
            {

                throw e;
            }
        }

    }
    private static void OnChangeQuestProgress(int o, int n)
    {
        GameData.RoyalPass.CurrentQuestsProgress[currentIndexQuestChange] += (n - o);
    }
}
