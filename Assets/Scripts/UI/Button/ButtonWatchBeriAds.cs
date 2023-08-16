using Framework;
using Monetization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWatchBeriAds : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI BeriBonusAmount;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID
        BeriBonusAmount.text += GameData.AdsUnitConfigs[MonetizationConfig.RewardAdsIdAndroid[(int)AdsIndex.Get_Beri]][0];
#endif
#if UNITY_IOS
        BeriBonusAmount.text += GameData.AdsUnitConfigs[MonetizationConfig.RewardAdsIdIOS[(int)AdsIndex.Get_Beri]][0];
#endif
    }

    private void OnDestroy()
    {

    }
}
