using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monetization
{
    public enum AdsType
    {
        Banner,
        Inter,
        Reward,
        Open,
        Native
    }
    public class AdsManager : MonoBehaviour
    {
#if UNITY_IOS
        public static string BannerAdUnitId { get => MonetizationConfig.BannerAdsIdIOS; set { } }
        public static string InterAdUnitId { get => MonetizationConfig.IntertialAdsIdIOS; set { } }
        public static string RewardAdUnitId { get => MonetizationConfig.RewardAdsIdIOS; set { } }
#else
        public static string BannerAdUnitId { get => MonetizationConfig.BannerAdsIdAndroid; set { } }
        public static string InterAdUnitId { get => MonetizationConfig.IntertialAdsIdAndroid; set { } }
        public static string[] RewardAdUnitId { get => MonetizationConfig.RewardAdsIdAndroid; set { } }

#endif
        static IAdsManager adsManager;
        void Awake()
        {
            adsManager = new MaxAdsManager();
            adsManager.Initialize();

        }
        public static void ShowBannerAds()
        {
            adsManager.ShowBannerAds();
        }
        public static void HideBannerAds()
        {
            adsManager.HideBannerAds();
        }
        public static void ShowInterstialAds()
        {
            adsManager.ShowInterstialAds();
        }
        public static void ShowRewardAds(Callback onRewardShowed, string id)
        {
            adsManager.ShowRewardAds(onRewardShowed, id);
        }
        public static void SetUserId(string id)
        {
            adsManager.SetUserId(id);
        }

    }

}