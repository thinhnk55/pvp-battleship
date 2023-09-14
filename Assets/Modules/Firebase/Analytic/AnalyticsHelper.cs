using System.Collections.Generic;
using UnityEngine;

namespace FirebaseIntegration
{
    public class AnalyticsHelper
    {
        public static void Login()
        {
            Analytics.Log(Firebase.Analytics.FirebaseAnalytics.EventLogin,
                new List<KeyValuePair<string, object>>()
                {
                });
        }
        public static void UnlockAchievement(int id, int level)
        {
            Analytics.Log(Firebase.Analytics.FirebaseAnalytics.EventUnlockAchievement,
                new List<KeyValuePair<string, object>>()
            {
                    new KeyValuePair<string, object>(Firebase.Analytics.FirebaseAnalytics.ParameterAchievementId, id),
                    new KeyValuePair<string, object>(Firebase.Analytics.FirebaseAnalytics.ParameterLevel, level),
            });
        }
        public static void WatchAds(MaxSdkBase.AdInfo adInfo)
        {
            Analytics.Log(Firebase.Analytics.FirebaseAnalytics.EventAdImpression,
                new List<KeyValuePair<string, object>>()
            {
                    //FirebaseAnalytics.id
                    //adInfo.
                    new KeyValuePair<string, object>(Firebase.Analytics.FirebaseAnalytics.ParameterAdPlatform, "AppLovin"),
                    new KeyValuePair<string, object>(Firebase.Analytics.FirebaseAnalytics.ParameterAdNetworkClickID, adInfo.NetworkName),
                    new KeyValuePair<string, object>(Firebase.Analytics.FirebaseAnalytics.ParameterAdUnitName, adInfo.AdUnitIdentifier),
                    new KeyValuePair<string, object>(Firebase.Analytics.FirebaseAnalytics.ParameterAdFormat, adInfo.AdFormat),
                    new KeyValuePair<string, object>(Firebase.Analytics.FirebaseAnalytics.ParameterValue, adInfo.Revenue),
                    new KeyValuePair<string, object>(Firebase.Analytics.FirebaseAnalytics.ParameterCurrency, "USD"),
            });
            Debug.LogWarning("EventAds");
        }
        public static void Transaction(string transaction)
        {
            Analytics.Log(Firebase.Analytics.FirebaseAnalytics.EventSpendVirtualCurrency,
                new List<KeyValuePair<string, object>>()
            {
                    new KeyValuePair<string, object>(Firebase.Analytics.FirebaseAnalytics.ParameterTransactionId, transaction),
            });
        }
        public static void SpendVirtualCurrency(string virtualCurrencyName, string source)
        {
            Analytics.Log(Firebase.Analytics.FirebaseAnalytics.EventSpendVirtualCurrency,
                new List<KeyValuePair<string, object>>()
            {
                    new KeyValuePair<string, object>(Firebase.Analytics.FirebaseAnalytics.ParameterVirtualCurrencyName, virtualCurrencyName),
                    new KeyValuePair<string, object>(Firebase.Analytics.FirebaseAnalytics.ParameterSource, source),
            });
        }
        public static void EarnVirtualCurrency(string virtualCurrencyName, string source)
        {
            Analytics.Log(Firebase.Analytics.FirebaseAnalytics.EventEarnVirtualCurrency,
                new List<KeyValuePair<string, object>>()
            {
                    new KeyValuePair<string, object>(Firebase.Analytics.FirebaseAnalytics.ParameterVirtualCurrencyName, virtualCurrencyName),
                    new KeyValuePair<string, object>(Firebase.Analytics.FirebaseAnalytics.ParameterSource, source),
            });
        }
    }
}
