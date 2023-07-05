using Framework;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoyalPassPopup : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI progress;
    [SerializeField] private TextMeshProUGUI countDown;
    void Start()
    {
        level.text = (GameData.RoyalPass.Point / GameData.RoyalPass.PointPerLevel).ToString();
        progressBar.maxValue = 100;
        progressBar.value = GameData.RoyalPass.Point % GameData.RoyalPass.PointPerLevel;
        progress.text = (GameData.RoyalPass.Point % GameData.RoyalPass.PointPerLevel).ToString() + "/" + GameData.RoyalPass.PointPerLevel;
        Timer<RoyalPass>.Instance.TriggerIntervalInSecond = GameData.RoyalPass.End.NowFrom0001From1970().ToSecond();
        Timer<RoyalPass>.Instance.OnElapse += OnElapsed;
        Timer<RoyalPass>.Instance.OnTrigger += OnTrigger;
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
