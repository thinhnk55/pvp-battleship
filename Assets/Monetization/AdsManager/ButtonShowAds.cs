using Framework;
using Monetization;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonShowAds : ButtonBase
{
    [SerializeField] UnityEvent rewardCallback;
    [SerializeField] int adsIndex;

    public void ShowAds(string customData = null)
    {
         AdsManager.ShowRewardAds(()=> rewardCallback?.Invoke(), AdsManager.RewardAdUnitId[adsIndex], customData);
    }

    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();
        Debug.Log("ShowAds");
        ShowAds();
    }

}
