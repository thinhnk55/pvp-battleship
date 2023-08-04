using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : SingletonScriptableObject<GameConfig>
{
    [SerializeField] private string[] achievementName; public static string[] AchievementName { get { return Instance.achievementName; } set { Instance.achievementName = value; } }
    [SerializeField] private string[] betNames; public static string[] BetNames { get { return Instance.betNames; } set { Instance.betNames = value; } }
    [SerializeField] private string[] rankNames; public static string[] RankNames { get { return Instance.rankNames; } set { Instance.rankNames = value; } }

    [SerializeField] private string privatePolicy; public static string PrivatePolicy { get { return Instance.privatePolicy; } set { Instance.privatePolicy = value; } }
    [SerializeField] private string userAgreement; public static string UserAgreement { get { return Instance.userAgreement; } set { Instance.userAgreement = value; } }


}