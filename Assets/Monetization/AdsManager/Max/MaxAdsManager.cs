using DG.Tweening;
using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monetization
{
    public class MaxAdsManager : MaxAdsManagerBase
    {
        public override void Initialize()
        {

            MaxSdk.SetSdkKey(MonetizationConfig.SdkKey);
            MaxSdk.SetUserId("USER_ID");
            MaxSdk.InitializeSdk();
            MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
            {
                // AppLovin SDK is initialized, start loading ads
                InitializeBannerAds();
                //InitializeInterstitialAds();
                InitializeRewardedAds();
            };
        }

        public override void ShowBannerAds()
        { 
            MaxSdk.ShowBanner(AdsManager.BannerAdUnitId);
        }

        public override void HideBannerAds()
        {
            MaxSdk.HideBanner(AdsManager.BannerAdUnitId);
        }
        public override void ShowInterstialAds()
        {
            if (MaxSdk.IsInterstitialReady(AdsManager.InterAdUnitId))
            {
                MaxSdk.ShowInterstitial(AdsManager.InterAdUnitId);
            }
        }
        public override void ShowRewardAds(Callback onRewardShowed)
        {
            OnRewardShowed = onRewardShowed;
            if (MaxSdk.IsRewardedAdReady(AdsManager.RewardAdUnitId))
            {
                MaxSdk.ShowRewardedAd(AdsManager.RewardAdUnitId);
            }
        }
    }
}