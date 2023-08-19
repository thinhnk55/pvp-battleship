using Framework;
using SimpleJSON;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoyalPassQuestType
{
    SHIP_DESTROY,
    PLAY_COUNT,
    SHIP_1_DESTROY,
    SHIP_2_DESTROY,
    SHIP_3_DESTROY,
    SHIP_4_DESTROY,
    WIN_COUNT,
    LUCKY_SHOT_COUNT,
    DESTROY_SHIP_CONSECUTIVE_2,
    DESTROY_SHIP_CONSECUTIVE_3,
    ALIVE_1_SHIP
}
public class RoyalPass
{

    public class RoyalPassQuest
    {
        public int Reward;
        public RoyalPassQuestType Type;
        public int Require;
    }
    public int Season;
    public int Version;
    public PDataUnit<int> Point = new PDataUnit<int>(0);
    public int PointPerLevel;
    public int Level { get => Mathf.Clamp(Point.Data / PointPerLevel, 0, RewardElites.Length -1 ); }
    public PDataUnit<HashSet<int>> NormalObtains = new PDataUnit<HashSet<int>>(new HashSet<int>());
    public int[] NormalProgresses;
    public List<Framework.GoodInfo>[] RewardNormals;

    public int[] EliteProgresses;
    public PDataUnit<HashSet<int>> EliteObtains =  new PDataUnit<HashSet<int>>(new HashSet<int>());
    public List<Framework.GoodInfo>[] RewardElites;

    public RoyalPassQuest[] Quests;
    public PDataUnit<int[]> CurrentQuests;
    public int[] CurrentQuestsProgress;
    public RoyalPassQuest[] SeasonQuests;
    public int[] SeasonQuestsProgress;
    public PDataUnit<HashSet<int>> SeasonQuestsObtained = new PDataUnit<HashSet<int>>(new HashSet<int>());
    public static string GetDescription(RoyalPassQuestType type, int value)
    {
        string s = "";
        switch (type)
        {
            case RoyalPassQuestType.SHIP_DESTROY:
                s = $"Destroy {value} ships";
                break;
            case RoyalPassQuestType.WIN_COUNT:
                s = $"Win {value} battles";
                break;
            case RoyalPassQuestType.SHIP_1_DESTROY:
                s = $"Destroy {value} single-deck ships";
                break;
            case RoyalPassQuestType.SHIP_2_DESTROY:
                s = $"Destroy {value} two-deck ships";
                break;
            case RoyalPassQuestType.SHIP_3_DESTROY:
                s = $"Destroy {value} three-deck ships";
                break;
            case RoyalPassQuestType.SHIP_4_DESTROY:
                s = $"Destroy {value} four-deck ships";
                break;
            case RoyalPassQuestType.PLAY_COUNT:
                s = $"Play {value} batlles";
                break;
            case RoyalPassQuestType.LUCKY_SHOT_COUNT:
                s = $"Play {value} times Lucky Shot";
                break;
            case RoyalPassQuestType.DESTROY_SHIP_CONSECUTIVE_2:
                s = $"Destroy 2 ships consecutively in {value} different battles";
                break;
            case RoyalPassQuestType.DESTROY_SHIP_CONSECUTIVE_3:
                s = $"Destroy 3 ships consecutively in {value} different battles";
                break;
            case RoyalPassQuestType.ALIVE_1_SHIP:
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
                Type = json["season_quest"][i]["type"].ToEnum<RoyalPassQuestType>(),
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
                Type = json["daily_quest"][i]["type"].ToEnum<RoyalPassQuestType>(),
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
        royalPass.Season = json["i"].AsInt;
        royalPass.Version = 0;
        royalPass.SeasonQuestsObtained = new PDataUnit<HashSet<int>>(new HashSet<int>());
        if (json["s"]["r"].Count>0)
        {
            royalPass.SeasonQuestsObtained.Data.AddRange(json["s"]["r"].ToListInt(true));
        }
        royalPass.SeasonQuestsProgress = new int[20]; // config come after data so init 20
        for (int i = 0; i < royalPass.SeasonQuests.Length; i++)
        {
            switch (royalPass.SeasonQuests[i].Type)
            {
                case RoyalPassQuestType.SHIP_DESTROY:
                    royalPass.SeasonQuestsProgress[i] = json["s"]["t"].AsInt;
                    break;
                case RoyalPassQuestType.PLAY_COUNT:
                    royalPass.SeasonQuestsProgress[i] = json["s"]["p"].AsInt;
                    break;
                case RoyalPassQuestType.SHIP_1_DESTROY:
                    royalPass.SeasonQuestsProgress[i] = json["s"]["s"][0].AsInt;
                    break;
                case RoyalPassQuestType.SHIP_2_DESTROY:
                    royalPass.SeasonQuestsProgress[i] = json["s"]["s"][1].AsInt;
                    break;
                case RoyalPassQuestType.SHIP_3_DESTROY:
                    royalPass.SeasonQuestsProgress[i] = json["s"]["s"][2].AsInt;
                    break;
                case RoyalPassQuestType.SHIP_4_DESTROY:
                    royalPass.SeasonQuestsProgress[i] = json["s"]["s"][3].AsInt;
                    break;
                case RoyalPassQuestType.WIN_COUNT:
                    royalPass.SeasonQuestsProgress[i] = json["s"]["w"].AsInt;
                    break;
                case RoyalPassQuestType.LUCKY_SHOT_COUNT:
                    royalPass.SeasonQuestsProgress[i] = json["s"]["l"].AsInt;
                    break;
                case RoyalPassQuestType.DESTROY_SHIP_CONSECUTIVE_2:
                    royalPass.SeasonQuestsProgress[i] = json["s"]["t"].AsInt;
                    break;
                case RoyalPassQuestType.DESTROY_SHIP_CONSECUTIVE_3:
                    royalPass.SeasonQuestsProgress[i] = json["s"]["k3"].AsInt;
                    break;
                case RoyalPassQuestType.ALIVE_1_SHIP:
                    royalPass.SeasonQuestsProgress[i] = json["s"]["w1"].AsInt;
                    break;
                default:
                    break;
            }
        }
        royalPass.CurrentQuests = new PDataUnit<int[]>(new int[3] {-1,-1,-1});
        royalPass.CurrentQuestsProgress = new int[json["d"]["q"].Count];
        for (int i = 0; i < json["d"]["q"].Count; i++)
        {
            royalPass.CurrentQuests.Data[i] = json["d"]["q"][i].AsInt;
            royalPass.CurrentQuestsProgress[i] = json["d"]["p"][i].AsInt;
        }
        royalPass.Point.Data = int.Parse(json["p"]);
        royalPass.EliteObtains = new PDataUnit<HashSet<int>>(new HashSet<int>());
        royalPass.EliteObtains.Data.AddRange(json["e"].ToListInt());
        royalPass.NormalObtains = new PDataUnit<HashSet<int>>(new HashSet<int>());
        royalPass.NormalObtains.Data.AddRange(json["f"].ToListInt());

    }
}
