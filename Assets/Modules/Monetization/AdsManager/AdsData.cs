using Framework;
using System.Collections.Generic;
using UnityEngine;


public struct AdsRewardConfig
{
    public List<int> reward;
    public string rewardAdUnitId;
}

public enum RewardType
{
    Get_Beri = 2,
    Get_Rocket = 3,
    Get_Quest = 4,
    Change_Quest = 5,
    Get_X2DailyGift = 6,
    Get_RevivalOnlyPVE = 7,
}

public class AdsData : PDataBlock<AdsData>
{
    [SerializeField] private Dictionary<RewardType, string> adsUnitIdMap; public static Dictionary<RewardType, string> AdsUnitIdMap { get { return Instance.adsUnitIdMap; } set { Instance.adsUnitIdMap = value; } }
    [SerializeField] private Dictionary<string, AdsRewardConfig> rewardTypeToConfigMap; public static Dictionary<string, AdsRewardConfig> RewardTypeToConfigMap { get { return Instance.rewardTypeToConfigMap; } set { Instance.rewardTypeToConfigMap = value; } }
    [SerializeField] private int versionAds; public static int VersionAds { get { return Instance.versionAds; } set { Instance.versionAds = value; } }

    protected override void Init()
    {
        base.Init();
        Instance.adsUnitIdMap = Instance.adsUnitIdMap ?? new Dictionary<RewardType, string>();
        Instance.rewardTypeToConfigMap = Instance.rewardTypeToConfigMap ?? new Dictionary<string, AdsRewardConfig>();
        //Instance.VersionAds = 0;
    }
}

