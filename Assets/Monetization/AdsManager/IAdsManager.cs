using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAdsManager
{
    public Callback OnRewardShowed { get; set; }      
    public void ShowBannerAds();
    public void HideBannerAds();
    public void ShowInterstialAds();
    public void ShowRewardAds(Callback onRewardShowed);
    public void Initialize();
}
