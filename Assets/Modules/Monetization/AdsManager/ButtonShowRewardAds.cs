
using Framework;
using UnityEngine;
using UnityEngine.Events;

namespace Monetization
{

    public class ButtonShowRewardAds : ButtonBase
    {
        [SerializeField] UnityEvent rewardCallback;
        [SerializeField] RewardType rewardType;

        public void ShowAds(string customData = null)
        {
            AdsManager.ShowRewardAds(() => rewardCallback?.Invoke(), AdsData.AdsUnitIdMap[rewardType], customData);
        }

        protected override void Button_OnClicked()
        {
            base.Button_OnClicked();
            PDebug.Log("ShowAds");
            ShowAds();
        }

    }

}
