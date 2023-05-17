using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class MonetizationConfig : SingletonScriptableObject<MonetizationConfig>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            if (_instance == null)
            {
                Instance.ToString();
            }
        }
        [SerializeField] private string bannerAdsIdAndroid; public static string BannerAdsIdAndroid { get { return Instance.bannerAdsIdAndroid; } }
        [SerializeField] private string bannerAdsIdIOS; public static string BannerAdsIdIOS { get { return Instance.bannerAdsIdIOS; } }
        [SerializeField] private string intertitialAdsIdAndroid; public static string IntertitialAdsIdAndroid { get { return Instance.intertitialAdsIdAndroid; } }
        [SerializeField] private string intertitialAdsIdIOS; public static string IntertitialAdsIdIOS { get { return Instance.intertitialAdsIdIOS; } }
        [SerializeField] private string rewardAdsIdAndroid; public static string RewardAdsIdAndroid { get { return Instance.rewardAdsIdAndroid; } }
        [SerializeField] private string rewardAdsIdIOS; public static string RewardAdsIdIOS { get { return Instance.rewardAdsIdIOS; } }

    }

}
