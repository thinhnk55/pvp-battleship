using Framework;
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
                WSClientHandler.GetGift();
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
            WSClientHandler.GetGift();
        });
    }

    private void OnElapse()
    {
        countDown.text = Timer<Gift>.Instance.TriggersFromBegin >= 1 ? "Collect" : Timer<Gift>.Instance.RemainTime_Sec.Hour_Minute_Second_1();
    }

    void GetGift(JSONNode json)
    {
        PConsumableType.BERI.SetValue(int.Parse(json["d"]["g"]));
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
    }

    void GetAdsGift(JSONNode data)
    {
        PConsumableType.BERI.SetValue(int.Parse(data["d"]["g"]));
        CoinVFX.CoinVfx(resource, Position, Position);
        Timer<Gift>.Instance.BeginPoint = DateTime.UtcNow.Ticks;
        GameData.ProgressGift += int.Parse(data["d"]["i"]) + 1;
    }

    // Update is called once per frame
    void Update()
    {
        Timer<Gift>.Instance.Elasping();
    }


    public static Action<JSONNode> OnGetAdsGift;
}
