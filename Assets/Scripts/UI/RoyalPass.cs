using SimpleJSON;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoyalPassQuestType
{

}
public class RoyalPass
{

    public class RoyalPassQuest
    {
        public int Reward;
        public RoyalPassQuestType Type;
        public int Require;
    }
    public int Point;
    public int PointPerLevel;
    public HashSet<int> NormalObtains;
    public int[] NormalProgresses;
    public Framework.GoodInfo[] RewardNormals;

    public bool UnlockedElite;
    public int[] EliteProgresses;
    public HashSet<int> EliteObtains;
    public Framework.GoodInfo[] RewardElites;

    public RoyalPassQuest[] Quests;
    public int[] CurrentQuests;
    public int[] CurrentQuestsProgress;
    public RoyalPassQuest[] SeasonQuests;
    public int[] SeasonQuestsProgress;

    public static RoyalPass ConfigFromJson(RoyalPass royalPass, JSONNode json)
    {
        royalPass.PointPerLevel = int.Parse(json["r"]);
        royalPass.SeasonQuests = new RoyalPassQuest[json["season quest"].Count];
        for (int i = 0; i < json["season quest"].Count; i++)
        {
            RoyalPassQuest quest = new RoyalPassQuest()
            {
                Reward = int.Parse(json["season quest"][i]["reward"]),
                Type = json["season quest"][i]["reward"].ToEnum<RoyalPassQuestType>(),
                Require = int.Parse(json["season quest"][i]["require"]),
            };
            royalPass.SeasonQuests[i] = quest;
        }
        royalPass.Quests = new RoyalPassQuest[json["daily quest"].Count];
        for (int i = 0; i < json["daily quest"].Count; i++)
        {
            RoyalPassQuest quest = new RoyalPassQuest()
            {
                Reward = int.Parse(json["daily quest"][i]["reward"]),
                Type = json["daily quest"][i]["reward"].ToEnum<RoyalPassQuestType>(),
                Require = int.Parse(json["daily quest"][i]["require"]),
            };
            royalPass.Quests[i] = quest;
        }
        for (int i = 0; i < json["normal"].Count; i++)
        {
            royalPass.RewardNormals[i].Value = int.Parse(json["normal"][i]["reward"]);
            royalPass.RewardNormals[i].Type = int.Parse(json["normal"][i]["reward_type"]);
        }
        for (int i = 0; i < json["elite"].Count; i++)
        {
            royalPass.RewardElites[i].Value = int.Parse(json["elite"][i]["reward"]);
            royalPass.RewardElites[i].Type = int.Parse(json["elite"][i]["reward_type"]);
        }
        return royalPass;
    }
    public static void DataFromJson(RoyalPass royalPass, JSONNode json)
    {

        royalPass.Point = int.Parse(json["rp"]);
        royalPass.UnlockedElite = int.Parse(json["u"]) == 1;
        if (royalPass.UnlockedElite)
        {

        }
        royalPass.SeasonQuestsProgress = new int[json["sq"].Count];
        for (int i = 0; i < json["sq"].Count; i++)
        {
            royalPass.SeasonQuestsProgress[i] = json["sq"][i].AsInt;
        }
        royalPass.CurrentQuests = new int[json["sq"].Count];
        royalPass.CurrentQuestsProgress = new int[json["pg"].Count];
        for (int i = 0; i < json["dq"].Count; i++)
        {
            royalPass.CurrentQuests[i] = json["dq"][i].AsInt;
            royalPass.CurrentQuestsProgress[i] = json["pg"][i].AsInt;
        }
        //royalPass.Point = int.Parse(json["rp"]);
        //royalPass.NormalObtains.AddRange(json["re"].ToList());
    }
}
