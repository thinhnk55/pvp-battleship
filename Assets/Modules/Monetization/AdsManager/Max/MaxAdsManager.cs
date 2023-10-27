using Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Monetization
{
    public class MaxAdsManager : MaxAdsManagerBase
    {
        public override void Initialize()
        {
            MaxSdk.SetSdkKey(MonetizationConfig.SdkKey);
            MaxSdk.InitializeSdk();
            Debug.Log("Initialize SDK");
        }

        public override void InitializeSDKEventOnInitialized()
        {
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
        }

        protected override void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            base.OnRewardedAdRevenuePaidEvent(adUnitId, adInfo);
            Messenger.Broadcast<MaxSdkBase.AdInfo>(GameEvent.REWARD_ADS_INFO, adInfo);
        }
    }
}