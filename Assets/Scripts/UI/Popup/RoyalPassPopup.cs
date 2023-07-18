using DG.Tweening;
using Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RoyalPassPopup : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI progress;
    [SerializeField] private TextMeshProUGUI countDown;
    [SerializeField] private RoyalPassFreeCollection freeCollection;
    [SerializeField] private RoyalPassEliteCollection eliteCollection;
    [SerializeField] private GameObject upgradePass;
    [SerializeField] private TransactionCard upgradePassCard;

    private Tween pointTweenText;
    void Start()
    {
        GameData.RoyalPass.Point.OnDataChanged += OnPointChange;
        level.text = (GameData.RoyalPass.Point.Data / GameData.RoyalPass.PointPerLevel).ToString();
        progressBar.maxValue = GameData.RoyalPass.PointPerLevel;
        progressBar.value = GameData.RoyalPass.Point.Data % GameData.RoyalPass.PointPerLevel;
        progress.text = (GameData.RoyalPass.Point.Data % GameData.RoyalPass.PointPerLevel).ToString() + "/" + GameData.RoyalPass.PointPerLevel;
        TransactionInfo transactionInfo = new TransactionInfo()
        {
            Product = new GoodInfo[1] { new GoodInfo() { Type = (int)PNonConsumableType.ELITE, Value = 0 } },
            Cost = new GoodInfo[1] { GameData.TransactionConfigs[TransactionType.GEM_ELITE][0].Cost[0] },
            TransactionType = TransactionType.GEM_ELITE,
            Index = 0,
        };
        upgradePassCard.BuildUI(transactionInfo);

        Timer<RoyalPass>.Instance.TriggerInterval_Sec = GameData.RoyalPass.End.NowFrom0001From1970().ToSecond();
        Timer<RoyalPass>.Instance.OnElapse += OnElapsed;
        Timer<RoyalPass>.Instance.OnTrigger += OnTrigger;
        upgradePass.SetActive(!GameData.RoyalPass.UnlockedElite);
        PNonConsumableType.ELITE.GetData().OnDataChanged += RoyalPassPopup_OnDataChanged;
    }
    private void OnDestroy()
    {
        Timer<RoyalPass>.Instance.OnElapse -= OnElapsed;
        Timer<RoyalPass>.Instance.OnTrigger -= OnTrigger;
        PNonConsumableType.ELITE.GetData().OnDataChanged -= RoyalPassPopup_OnDataChanged;
    }
    private void RoyalPassPopup_OnDataChanged(HashSet<int> arg1, HashSet<int> arg2)
    {
        GameData.RoyalPass.UnlockedElite = true;
        upgradePass.SetActive(false);
        eliteCollection.UpdateUIs();
    }

    private void OnPointChange(int arg1, int arg2)
    {
        if (GameData.RoyalPass.Level != (arg2 / GameData.RoyalPass.PointPerLevel))
        {
            freeCollection.ModifyUIAt(GameData.RoyalPass.Level, new RoyalPassInfo()
            {
                Id = GameData.RoyalPass.Level,
                Obtained = GameData.RoyalPass.NormalObtains.Data.Contains(GameData.RoyalPass.Level),
                Unlocked = true,
                Reward = GameData.RoyalPass.RewardNormals[GameData.RoyalPass.Level].ToArray(),
                Elite = false,
                Obtain = (info) =>
                {
                    WSClient.RequestReceiveRoyalPass(GameData.RoyalPass.Level, 0);
                }   
            });
            eliteCollection.ModifyUIAt(GameData.RoyalPass.Level, new RoyalPassInfo()
            {
                Id = GameData.RoyalPass.Level,
                Obtained = GameData.RoyalPass.EliteObtains.Data.Contains(GameData.RoyalPass.Level),
                Unlocked = true,
                Reward = GameData.RoyalPass.RewardElites[GameData.RoyalPass.Level].ToArray(),
                Elite = true,
                Obtain = (info) =>
                {
                    WSClient.RequestReceiveRoyalPass(GameData.RoyalPass.Level, 0);
                }
            });
        }
        pointTweenText?.Kill();
        pointTweenText = DOTween.To(() => progressBar.value + float.Parse(level.text)* GameData.RoyalPass.PointPerLevel, 
            (value) => {
                progress.text = ((int)value % GameData.RoyalPass.PointPerLevel).ToString() + "/" + GameData.RoyalPass.PointPerLevel; 
                level.text = (Mathf.Floor(value/GameData.RoyalPass.PointPerLevel)).ToString(); 
                progressBar.value = value% GameData.RoyalPass.PointPerLevel;
            }
        , GameData.RoyalPass.Point.Data, 1.5f);
    }

    private void OnTrigger()
    {
        countDown.text = Timer<RoyalPass>.Instance.TriggersFromBegin >= 1 ? "Season Ended" : Timer<RoyalPass>.Instance.RemainTime_Sec.Hour_Minute_Second_1();
    }

    private void OnElapsed()
    {
        countDown.text = "Season Ended in : " + Timer<RoyalPass>.Instance.RemainTime_Sec.Hour_Minute_Second_1();
    }

    // Update is called once per frame
    void Update()
    {
        Timer<RoyalPass>.Instance.Elasping();
    }

    public void UpgradePass()
    {
        if (GameData.TransactionConfigs[TransactionType.GEM_ELITE][0].Cost[0].Value > PConsumableType.GEM.GetValue())
        {
            TransactionCard.RequestTransaction((int)TransactionType.GEM_ELITE, 0);
        }
        else
        {

        }
    }

    public void ClaimAll()
    {
        WSClient.RequestClaimAllRoyalPass();
    }
}
