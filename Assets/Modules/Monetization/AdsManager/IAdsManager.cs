using Monetization;

public interface IAdsManager
{
    public Callback OnRewardShowed { get; set; }
    public void ShowBannerAds();
    public void HideBannerAds();
    public void ShowInterstialAds();
    public void ShowRewardAds(Callback onRewardShowed, string id, string customData = null);
    public void Initialize(string userId);
    public void SetUserId(string id);
    public void LoadAds(string id, AdsType type);
}
