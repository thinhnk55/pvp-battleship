using Framework;
using System.Collections.Generic;

namespace Monetization
{
    public class MaxAdsManager : MaxAdsManagerBase
    {
        public override void Initialize(string userId)
        {
            MaxSdk.SetSdkKey(MonetizationConfig.SdkKey);
            MaxSdk.SetUserId(userId);
            MaxSdk.InitializeSdk();
            MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
            {
                // AppLovin SDK is initialized, start loading ads
                //InitializeBannerAds();
                //InitializeInterstitialAds();
                InitializeRewardedAds();
            };

            // Load reward Ads
            foreach (KeyValuePair<RewardType, string> kvp in AdsData.AdsUnitIdMap)
            {
                LoadAds(kvp.Value, AdsType.Reward);
            }
        }


        protected override void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            base.OnRewardedAdRevenuePaidEvent(adUnitId, adInfo);
            Messenger.Broadcast<MaxSdkBase.AdInfo>(GameEvent.REWARD_ADS_INFO, adInfo);
        }
    }
}