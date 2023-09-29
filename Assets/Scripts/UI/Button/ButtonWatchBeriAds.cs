using DG.Tweening;
using Framework;
using Monetization;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ButtonWatchBeriAds : MonoBehaviour
{
    [SerializeField] Transform resource;
    [SerializeField] TextMeshProUGUI BeriBonusAmount;
    // Start is called before the first frame update
    void Start()
    {
        ServerMessenger.AddListener<JSONNode>(ServerResponse._REWARD_ADS, ReceiveWatchAdsReward);
        BeriBonusAmount.text += AdsData.RewardTypeToConfigMap[AdsData.AdsUnitIdMap[RewardType.Get_Beri]].reward[0];
    }

    private void OnDestroy()
    {
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._REWARD_ADS, ReceiveWatchAdsReward);
    }

    private void ReceiveWatchAdsReward(JSONNode data)
    {
        if(String.Equals(data["d"]["a"], AdsData.AdsUnitIdMap[RewardType.Get_Beri]))
        {
            PConsumableType.BERRY.SetValue(int.Parse(data["d"]["g"]));
            VFXReceiveBeri();
        }
    }

    private void VFXReceiveBeri()
    {
        CoinVFX.CoinVfx(resource, transform.position, transform.position);
    }
}
