using Framework;
using Monetization;
using SimpleJSON;
using System;
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
        progress?.SetText($"{GameData.ProgressGift}/5");
        OnGetAdsGift += GetAdsGift;
        Timer<Gift>.Instance.OnTrigger += OnTrigger;
        Timer<Gift>.Instance.OnElapse += OnElapse;
        ServerMessenger.AddListener<JSONNode>(ServerResponse._GIFT, GetGift);
        if (GameData.ProgressGift == 5)
        {
            if (smallGiftBar)
            {
                smallGiftBar.SetActive(false);
            }
            if (bigGiftBar)
            {
                bigGiftBar.SetActive(true);
            }
        }
        else
        {
            if (smallGiftBar)
            {
                smallGiftBar.SetActive(true);
            }
            if (bigGiftBar)
            {
                bigGiftBar.SetActive(false);
            }
            progress?.SetText($"{GameData.ProgressGift}/5");
        }
        if (Timer<Gift>.Instance.TriggersFromBegin >= 1)
        {
            countDown.text = "Collect";
        }
        else
        {
            countDown.text = Timer<Gift>.Instance.RemainTime_Sec.Hour_Minute_Second_1();
        }
        obtain?.onClick.AddListener(() =>
        {
            CreateConfirmReceiveGiftPopup();
        });
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
        reminder.UpdateObject();
    }

    private void OnElapse()
    {
        countDown.text = Timer<Gift>.Instance.TriggersFromBegin >= 1 ? "Collect" : Timer<Gift>.Instance.RemainTime_Sec.Hour_Minute_Second_1();
    }

    private void CreateConfirmReceiveGiftPopup()
    {
        PopupHelper.CreateConfirm(PrefabFactory.PopupReceiveGift, null, "+" + GameData.GiftConfig[GameData.ProgressGift].ToString(), null, (confirm) =>
        {
            if (confirm)
            {
                //Get beri
                Debug.LogWarning("GetX1");
                if (Timer<Gift>.Instance.TriggersFromBegin >= 1)
                    WSClientHandler.GetGift();
            }
            else
            {
                //Watch ads done => Get x3beri
                Debug.LogWarning("X2");
                AdsManager.ShowRewardAds(null, AdsData.adsUnitIdMap[RewardType.Get_X2DailyGift]);
            }
        });
    }

    void GetGift(JSONNode json)
    {
        if (json["e"].AsInt != 0)
            return;
        PConsumableType.BERRY.SetValue(int.Parse(json["d"]["g"]));
        Timer<Gift>.Instance.BeginPoint = DateTime.UtcNow.Ticks;
        GameData.ProgressGift++;

        //if (resource)
        //   CoinVFX.CoinVfx(resource, Position, Position);

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
        reminder.UpdateObject();
    }

    void GetAdsGift(JSONNode data)
    {
        PConsumableType.BERRY.AddValue(int.Parse(data["d"]["x"]["g"]));
        CoinVFX.CoinVfx(resource, Position, Position);
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
