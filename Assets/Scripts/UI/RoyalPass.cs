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
    WIN_COUNT,
    SHIP_1_DESTROY,
    SHIP_2_DESTROY,
    SHIP_3_DESTROY,
    SHIP_4_DESTROY,
    PLAY_COUNT,
    LUCKY_SHOT_COUNT,
    DESTROY_SHIP_CONSECUTIVE,
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
    public long End;
    public int Point;
    public int PointPerLevel;
    public HashSet<int> NormalObtains =  new HashSet<int>();
    public int[] NormalProgresses;
    public List<Framework.GoodInfo>[] RewardNormals;

    public bool UnlockedElite;
    public int[] EliteProgresses;
    public HashSet<int> EliteObtains =  new HashSet<int>();
    public List<Framework.GoodInfo>[] RewardElites;

    public RoyalPassQuest[] Quests;
    public PDataUnit<int[]> CurrentQuests;
    public int[] CurrentQuestsProgress;
    public RoyalPassQuest[] SeasonQuests;
    public int[] SeasonQuestsProgress;
    public PDataUnit<HashSet<int>> SeasonQuestsObtained;
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
            case RoyalPassQuestType.DESTROY_SHIP_CONSECUTIVE:
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
        royalPass.End = long.Parse(json["end"]).NowFrom0001From1970() - DateTime.UtcNow.Ticks;

        royalPass.PointPerLevel = int.Parse(json["r"]);
        royalPass.SeasonQuests = new RoyalPassQuest[json["season_quest"].Count];
        for (int i = 0; i < json["season_quest"].Count; i++)
        {
            RoyalPassQuest quest = new RoyalPassQuest()
            {
                Reward = int.Parse(json["season_quest"][i]["reward"]),
                Type = json["season_quest"][i]["require_type"].ToEnum<RoyalPassQuestType>(),
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
                Type = json["daily_quest"][i]["require_type"].ToEnum<RoyalPassQuestType>(),
                Require = int.Parse(json["daily_quest"][i]["require"]),
            };
            royalPass.Quests[i] = quest;
        }
        royalPass.RewardNormals = new List<Framework.GoodInfo>[json["normal"].Count];
        for (int i = 0; i < json["normal"].Count; i++)
        {
            royalPass.RewardNormals[i] = new List<Framework.GoodInfo>();
            for (int j = 0; j < json["normal"][i]["reward"].Count; j++)
            {
                royalPass.RewardNormals[i].Add(new Framework.GoodInfo()
                {
                    Value = int.Parse(json["normal"][i]["reward"][j]),
                    Type = int.Parse(json["normal"][i]["reward_type"][j])
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

        royalPass.SeasonQuestsObtained = new PDataUnit<HashSet<int>>(new HashSet<int>());
        for (int i = 0; i < json["rsq"].Count; i++)
        {
            royalPass.SeasonQuestsObtained.Data.Add(json["rsq"][i].AsInt);
        }
        royalPass.SeasonQuestsProgress = new int[json["sq"].Count];
        for (int i = 0; i < json["sq"].Count; i++)
        {
            royalPass.SeasonQuestsProgress[i] = json["sq"][i].AsInt;
        }
        royalPass.CurrentQuests = new PDataUnit<int[]>(new int[3] {-1,-1,-1});
        royalPass.CurrentQuestsProgress = new int[json["pg"].Count];
        for (int i = 0; i < json["dq"].Count; i++)
        {
            royalPass.CurrentQuests.Data[i] = json["dq"][i].AsInt;
            royalPass.CurrentQuestsProgress[i] = json["pg"][i].AsInt;
        }
        royalPass.Point = int.Parse(json["rp"]);
        royalPass.UnlockedElite = int.Parse(json["u"]) == 1;
        if (royalPass.UnlockedElite)
        {
            royalPass.EliteObtains.AddRange(json["re"].ToList());
        }
        royalPass.NormalObtains.AddRange(json["rn"].ToList());
    }
}
