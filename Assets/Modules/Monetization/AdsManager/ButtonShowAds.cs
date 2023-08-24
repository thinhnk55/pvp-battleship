
using Framework;
using Monetization;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Monetization{
    public enum AdsIndex
    {
        Get_Beri = 0,
        Get_Rocket = 1,
        Get_Quest = 2,
        Change_Quest = 3,
    }

    public class ButtonShowAds : ButtonBase
    {
        [SerializeField] UnityEvent rewardCallback;
        [SerializeField] RewardType rewardType;

        public void ShowAds(string customData = null)
        {
            AdsManager.ShowRewardAds(() => rewardCallback?.Invoke(), AdsData.adsUnitIdMap[rewardType], customData);
        }

        protected override void Button_OnClicked()
        {
            base.Button_OnClicked();
            Debug.Log("ShowAds");
            ShowAds();
        }

    }

}
