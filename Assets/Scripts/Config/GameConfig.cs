using Framework;
using UnityEngine;

public class GameConfig : SingletonScriptableObject<GameConfig>
{
#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        if (_instance == null)
        {
            Instance.ToString();
        }
    }
#endif
    [SerializeField] private string[] achievementName; public static string[] AchievementName { get { return Instance.achievementName; } set { Instance.achievementName = value; } }
    [SerializeField] private string[] betNames; public static string[] BetNames { get { return Instance.betNames; } set { Instance.betNames = value; } }
    [SerializeField] private string[] betPVENames; public static string[] BetPVENames { get { return Instance.betPVENames; } set { Instance.betPVENames = value; } }
    [SerializeField] private string[] rankNames; public static string[] RankNames { get { return Instance.rankNames; } set { Instance.rankNames = value; } }
    [SerializeField] private string privatePolicy; public static string PrivatePolicy { get { return Instance.privatePolicy; } set { Instance.privatePolicy = value; } }
    [SerializeField] private string userAgreement; public static string UserAgreement { get { return Instance.userAgreement; } set { Instance.userAgreement = value; } }
    [SerializeField] private string matchJsonTuto; public static string MatchJsonTuto { get { return Instance.matchJsonTuto; } set { Instance.matchJsonTuto = value; } }
    [SerializeField] private string startJsonTuto; public static string StartJsonTuto { get { return Instance.startJsonTuto; } set { Instance.startJsonTuto = value; } }
    [SerializeField] private string[] listShotPlayerJsonTuto; public static string[] ListShotPlayerJsonTuto { get { return Instance.listShotPlayerJsonTuto; } set { Instance.listShotPlayerJsonTuto = value; } }
    [SerializeField] private string gameDestroyTuto; public static string GameDestroyTuto { get { return Instance.gameDestroyTuto; } set { Instance.gameDestroyTuto = value; } }
    [SerializeField] private string[] listShotBotJsonTuto; public static string[] ListShotBotJsonTuto { get { return Instance.listShotBotJsonTuto; } set { Instance.listShotBotJsonTuto = value; } }
    [SerializeField] private string[] listEndTurnJsonTuto; public static string[] ListEndTurnJsonTuto { get { return Instance.listEndTurnJsonTuto; } set { Instance.listEndTurnJsonTuto = value; } }
}
