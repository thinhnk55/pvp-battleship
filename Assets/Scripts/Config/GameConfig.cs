using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : SingletonScriptableObject<GameConfig>
{
    [SerializeField] private string[] achievementName; public static string[] AchievementName { get { return Instance.achievementName; } set { Instance.achievementName = value; } }
    [SerializeField] private string[] betNames; public static string[] BetNames { get { return Instance.betNames; } set { Instance.betNames = value; } }
    [SerializeField] private string[] rankNames; public static string[] RankNames { get { return Instance.rankNames; } set { Instance.rankNames = value; } }

}