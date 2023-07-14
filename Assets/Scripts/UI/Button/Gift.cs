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
        Timer<Gift>.Instance.OnTrigger += OnTrigger;
        Timer<Gift>.Instance.OnElapse += OnElapse;
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_GIFT, ReceiveGift);
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
                WSClient.RequestGift();
            });
        }
        else
        {
            countDown.text = Timer<Gift>.Instance.RemainTime_Sec.Hour_Minute_Second_1();
        }
    }
    private void OnDestroy()
    {
        Timer<Gift>.Instance.OnTrigger -= OnTrigger;
        Timer<Gift>.Instance.OnElapse -= OnElapse;
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_GIFT, ReceiveGift);
    }
    private void OnTrigger()
    {
        countDown.text = Timer<Gift>.Instance.TriggersFromBegin >= 1 ? "Collect" : Timer<Gift>.Instance.RemainTime_Sec.Hour_Minute_Second_1();
        obtain.onClick.AddListener(() =>
        {
            WSClient.RequestGift();
        });
    }

    private void OnElapse()
    {
        countDown.text = Timer<Gift>.Instance.TriggersFromBegin >= 1 ? "Collect" : Timer<Gift>.Instance.RemainTime_Sec.Hour_Minute_Second_1();
    }

    void ReceiveGift(JSONNode json)
    {
        PConsumableType.BERI.AddValue(int.Parse(json["value"]));
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
    // Update is called once per frame
    void Update()
    {
        Timer<Gift>.Instance.Elasping();
    }
}
