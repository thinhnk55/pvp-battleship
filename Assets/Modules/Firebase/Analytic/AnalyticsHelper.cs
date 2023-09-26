using Firebase.Analytics;
using System.Collections.Generic;
using UnityEngine;

namespace FirebaseIntegration
{
    public class AnalyticsHelper
    {
        public static void TutorialBegin()
        {
            Analytics.Log(FirebaseAnalytics.EventTutorialBegin,
            new List<KeyValuePair<string, object>>()
            {
            });
        }

        public static void TutorialComplete()
        {
            Analytics.Log(FirebaseAnalytics.EventTutorialComplete,
            new List<KeyValuePair<string, object>>()
            {
            });
        }

        public static void SelectContent(string type)
        {
            Analytics.Log(FirebaseAnalytics.EventSelectContent,
            new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>(FirebaseAnalytics.ParameterContentType, type),
            });
        }

        public static void Login()
        {
            Analytics.Log(FirebaseAnalytics.EventLogin,
                new List<KeyValuePair<string, object>>()
                {
                });
        }
        public static void UnlockAchievement(int id, int level)
        {
            Analytics.Log(FirebaseAnalytics.EventUnlockAchievement,
                new List<KeyValuePair<string, object>>()
            {
                    new KeyValuePair<string, object>(FirebaseAnalytics.ParameterAchievementId, id),
                    new KeyValuePair<string, object>(FirebaseAnalytics.ParameterLevel, level),
            });
        }
        public static void WatchAds(MaxSdkBase.AdInfo adInfo)
        {
            Analytics.Log(FirebaseAnalytics.EventAdImpression,
                new List<KeyValuePair<string, object>>()
            {
                    new KeyValuePair<string, object>(FirebaseAnalytics.ParameterAdPlatform, "AppLovin"),
                    new KeyValuePair<string, object>(FirebaseAnalytics.ParameterAdSource, adInfo.NetworkName),
                    new KeyValuePair<string, object>(FirebaseAnalytics.ParameterAdUnitName, adInfo.AdUnitIdentifier),
                    new KeyValuePair<string, object>(FirebaseAnalytics.ParameterAdFormat, adInfo.AdFormat),
                    new KeyValuePair<string, object>(FirebaseAnalytics.ParameterValue, adInfo.Revenue),
                    new KeyValuePair<string, object>(FirebaseAnalytics.ParameterCurrency, "USD"),

            });
            Debug.LogWarning("EventAds");
        }
        public static void Transaction(string transaction)
        {
            Analytics.Log(FirebaseAnalytics.EventSpendVirtualCurrency,
                new List<KeyValuePair<string, object>>()
            {
                    new KeyValuePair<string, object>(FirebaseAnalytics.ParameterTransactionId, transaction),
            });
        }
        public static void SpendVirtualCurrency(string virtualCurrencyName, string source)
        {
            Analytics.Log(FirebaseAnalytics.EventSpendVirtualCurrency,
                new List<KeyValuePair<string, object>>()
            {
                    new KeyValuePair<string, object>(FirebaseAnalytics.ParameterVirtualCurrencyName, virtualCurrencyName),
                    new KeyValuePair<string, object>(FirebaseAnalytics.ParameterSource, source),
            });
        }
        public static void EarnVirtualCurrency(string virtualCurrencyName, string source)
        {
            Analytics.Log(FirebaseAnalytics.EventEarnVirtualCurrency,
                new List<KeyValuePair<string, object>>()
            {
                    new KeyValuePair<string, object>(FirebaseAnalytics.ParameterVirtualCurrencyName, virtualCurrencyName),
                    new KeyValuePair<string, object>(FirebaseAnalytics.ParameterSource, source),
            });
        }
    }
}
