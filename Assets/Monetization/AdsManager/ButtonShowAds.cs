using Framework;
using Monetization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonShowAds : ButtonBase
{
    enum AdsType
    {
        Banner,
        Inter,
        Reward,
    }
    [SerializeField] AdsType adsType;
    [SerializeField] UnityEvent rewardCallback;
    [SerializeField] int adsIndex;
    public void ShowAds()
    {
        switch (adsType)
        {
            case AdsType.Banner:
                AdsManager.ShowBannerAds();
                break;
            case AdsType.Inter:
                AdsManager.ShowInterstialAds();
                break;
            case AdsType.Reward:
                AdsManager.ShowRewardAds(()=> rewardCallback?.Invoke(), AdsManager.RewardAdUnitId[adsIndex]);
                break;
            default:
                break;
        }
    }

    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();
        Debug.Log("ShowAds");
        ShowAds();
    }

}
