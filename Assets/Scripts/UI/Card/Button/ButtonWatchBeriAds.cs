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
        BeriBonusAmount.text += AdsData.rewardTypeToConfigMap[AdsData.adsUnitIdMap[RewardType.Get_Beri]].reward[0];
    }



    private void OnDestroy()
    {
    }
}
