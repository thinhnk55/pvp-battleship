using Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class GameConfig : SingletonScriptableObject<GameConfig>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        if (_instance == null)
        {
            Instance.ToString();
        }
    }
    [SerializeField] private string[] achievementName; public static string[] AchievementName { get { return Instance.achievementName; } set { Instance.achievementName = value; } }
    [SerializeField] private string[] betNames; public static string[] BetNames { get { return Instance.betNames; } set { Instance.betNames = value; } }
    [SerializeField] private string[] betPVENames; public static string[] BetPVENames { get { return Instance.betPVENames; } set { Instance.betPVENames = value; } }
    [SerializeField] private string[] rankNames; public static string[] RankNames { get { return Instance.rankNames; } set { Instance.rankNames = value; } }
    [SerializeField] private string privatePolicy; public static string PrivatePolicy { get { return Instance.privatePolicy; } set { Instance.privatePolicy = value; } }
    [SerializeField] private string userAgreement; public static string UserAgreement { get { return Instance.userAgreement; } set { Instance.userAgreement = value; } }


}
