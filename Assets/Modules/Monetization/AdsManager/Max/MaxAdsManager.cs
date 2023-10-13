using Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Monetization
{
    public class MaxAdsManager : MaxAdsManagerBase
    {
        public override void Initialize(string userId)
        {
            Debug.Log("Initialize");
            MaxSdk.SetSdkKey(MonetizationConfig.SdkKey);
            Debug.Log("SetSdkKey");
            MaxSdk.SetUserId(userId);
            Debug.Log("SetUserId");
            MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
            {
                // AppLovin SDK is initialized, start loading ads
                //InitializeBannerAds();
                //InitializeInterstitialAds();
                InitializeRewardedAds();            // Load reward Ads
                Debug.Log("InitializeRewardedAds");
                foreach (KeyValuePair<RewardType, string> kvp in AdsData.AdsUnitIdMap)
                {
                    LoadAds(kvp.Value, AdsType.Reward);
                }
                Debug.Log("LoadAds");
            };
            MaxSdk.InitializeSdk();
            Debug.Log("InitializeSdk");


        }


        protected override void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            base.OnRewardedAdRevenuePaidEvent(adUnitId, adInfo);
            Messenger.Broadcast<MaxSdkBase.AdInfo>(GameEvent.REWARD_ADS_INFO, adInfo);
        }
    }
}