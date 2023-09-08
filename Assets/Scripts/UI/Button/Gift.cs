using Framework;
using Monetization;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gift : CacheMonoBehaviour
{
    [SerializeField] Transform resource;
    [SerializeField] TextMeshProUGUI countDown;
    [SerializeField] TextMeshProUGUI progress;
    [SerializeField] Button obtain;
    [SerializeField] GameObject smallGiftBar;
    [SerializeField] GameObject bigGiftBar;
    [SerializeField] GiftReminder reminder;
    void Start()
    {
        progress.text = $"{GameData.ProgressGift}/5";
        OnGetAdsGift += GetAdsGift;
        Timer<Gift>.Instance.OnTrigger += OnTrigger;
        Timer<Gift>.Instance.OnElapse += OnElapse;
        ServerMessenger.AddListener<JSONNode>(ServerResponse._GIFT, GetGift);
        if (GameData.ProgressGift == 5)
        {
            smallGiftBar.SetActive(false);
            bigGiftBar.SetActive(true);
        }
        else
        {
            smallGiftBar.SetActive(true);
            bigGiftBar.SetActive(false);
            progress.text = $"{GameData.ProgressGift}/5";
        }
        if (Timer<Gift>.Instance.TriggersFromBegin >= 1)
        {
            countDown.text = "Collect";
            obtain.onClick.AddListener(() =>
            {
                CreateConfirmReceiveGiftPopup();
            });
        }
        else
        {
            countDown.text = Timer<Gift>.Instance.RemainTime_Sec.Hour_Minute_Second_1();
        }
    }
    private void OnDestroy()
    {
        OnGetAdsGift -= GetAdsGift;
        Timer<Gift>.Instance.OnTrigger -= OnTrigger;
        Timer<Gift>.Instance.OnElapse -= OnElapse;
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._GIFT, GetGift);
    }
    private void OnTrigger()
    {
        countDown.text = Timer<Gift>.Instance.TriggersFromBegin >= 1 ? "Collect" : Timer<Gift>.Instance.RemainTime_Sec.Hour_Minute_Second_1();
        obtain.onClick.RemoveAllListeners();
        obtain.onClick.AddListener(() =>
        {
            CreateConfirmReceiveGiftPopup();
        });
        reminder.UpdateObject();
    }

    private void OnElapse()
    {
        countDown.text = Timer<Gift>.Instance.TriggersFromBegin >= 1 ? "Collect" : Timer<Gift>.Instance.RemainTime_Sec.Hour_Minute_Second_1();
    }

    private void CreateConfirmReceiveGiftPopup()
    {
        PopupHelper.CreateConfirm(PrefabFactory.PopupReceiveGift, null, "+" +GameData.GiftConfig[GameData.ProgressGift].ToString(), null, (confirm) =>
        {
            if (confirm)
            {
                //Get beri
                Debug.LogWarning("GetX1");
                WSClientHandler.GetGift();
            }
            else
            {
                //Watch ads done => Get x3beri
                Debug.LogWarning("X3");
                AdsManager.ShowRewardAds(null, AdsData.adsUnitIdMap[RewardType.Get_X2DailyGift]);
            }
        });
    }

    void GetGift(JSONNode json)
    {
        PConsumableType.BERRY.SetValue(int.Parse(json["d"]["g"]));
        CoinVFX.CoinVfx(resource, Position, Position);
        obtain.onClick.RemoveAllListeners();
        Timer<Gift>.Instance.BeginPoint = DateTime.UtcNow.Ticks;
        GameData.ProgressGift++;
        if (GameData.ProgressGift==5)
        {
            smallGiftBar.SetActive(false);
            bigGiftBar.SetActive(true);
        }
        else
        {
            smallGiftBar.SetActive(true);
            bigGiftBar.SetActive(false);
            progress.text = $"{GameData.ProgressGift}/5";
        }
        reminder.UpdateObject();
    }

    void GetAdsGift(JSONNode data)
    {
        int giftAmount = int.Parse(data["d"]["x"]["g"]);
        int mulGif = int.Parse(data["d"]["x"]["i"]);
        PConsumableType.BERRY.SetValue(giftAmount * mulGif);
        CoinVFX.CoinVfx(resource, Position, Position);
        obtain.onClick.RemoveAllListeners();
        Timer<Gift>.Instance.BeginPoint = DateTime.UtcNow.Ticks;
        GameData.ProgressGift += int.Parse(data["d"]["x"]["i"]) + 1;
    }

    // Update is called once per frame
    void Update()
    {
        Timer<Gift>.Instance.Elasping();
    }


    public static Action<JSONNode> OnGetAdsGift;
}
