using DG.Tweening;
using Framework;
using System;
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

    private Tween pointTweenText;
    void Start()
    {
        GameData.RoyalPass.Point.OnDataChanged += OnPointChange;
        level.text = (GameData.RoyalPass.Point.Data / GameData.RoyalPass.PointPerLevel).ToString();
        progressBar.maxValue = GameData.RoyalPass.PointPerLevel;
        progressBar.value = GameData.RoyalPass.Point.Data % GameData.RoyalPass.PointPerLevel;
        progress.text = (GameData.RoyalPass.Point.Data % GameData.RoyalPass.PointPerLevel).ToString() + "/" + GameData.RoyalPass.PointPerLevel;
        Timer<RoyalPass>.Instance.TriggerIntervalInSecond = GameData.RoyalPass.End.NowFrom0001From1970().ToSecond();
        Timer<RoyalPass>.Instance.OnElapse += OnElapsed;
        Timer<RoyalPass>.Instance.OnTrigger += OnTrigger;
    }

    private void OnPointChange(int arg1, int arg2)
    {
        if (GameData.RoyalPass.Level != (arg2 / GameData.RoyalPass.PointPerLevel))
        {
            freeCollection.ModifyUIAt(GameData.RoyalPass.Level, new RoyalPassInfo()
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
                Debug.Log(value);
                progress.text = ((int)value % GameData.RoyalPass.PointPerLevel).ToString() + "/" + GameData.RoyalPass.PointPerLevel; 
                level.text = (Mathf.Floor(value/GameData.RoyalPass.PointPerLevel)).ToString(); 
                progressBar.value = value% GameData.RoyalPass.PointPerLevel;
            }
        , GameData.RoyalPass.Point.Data, 1.5f);
    }

    private void OnTrigger()
    {
        countDown.text = Timer<RoyalPass>.Instance.TriggerCountTotal >= 1 ? "Season Ended" : Timer<RoyalPass>.Instance.RemainTimeInsecond.Hour_Minute_Second_1();
    }

    private void OnElapsed()
    {
        countDown.text = Timer<RoyalPass>.Instance.RemainTimeInsecond.Hour_Minute_Second_1();
    }

    // Update is called once per frame
    void Update()
    {
        Timer<RoyalPass>.Instance.Elasping();
    }
}
