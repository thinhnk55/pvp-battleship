using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RPConfirm : MonoBehaviour
{
    [SerializeField] Tween tweenProgess;
    [SerializeField] Tween tweenLevel;
    [SerializeField] float tweenDuration;
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI point;

    private void Awake()
    {
        point.text = (GameData.RoyalPass.Point.Data % GameData.RoyalPass.PointPerLevel).ToString();
        level.text = (GameData.RoyalPass.Point.Data / GameData.RoyalPass.PointPerLevel).ToString();
        progressBar.maxValue = GameData.RoyalPass.PointPerLevel;
        progressBar.value = GameData.RoyalPass.Point.Data % GameData.RoyalPass.PointPerLevel;
        GameData.RoyalPass.Point.OnDataChanged += OnChangeValueProgress;
    }

    public void OnChangeValueProgress(int oldValue, int newValue)
    {
        tweenProgess?.Kill();
        tweenProgess = DOTween.To(() => progressBar.value + float.Parse(level.text) * GameData.RoyalPass.PointPerLevel,
            (value) =>
            {
                point.text = ((int)value % GameData.RoyalPass.PointPerLevel).ToString();
                level.text = (Mathf.Floor(value / GameData.RoyalPass.PointPerLevel)).ToString();
                progressBar.value = value % GameData.RoyalPass.PointPerLevel;
            }
        , GameData.RoyalPass.Point.Data, tweenDuration);
    }

    public void OnChangeProgressBar(int oldValue, int newValue)
    {

    }

    private void OnDestroy()
    {
        GameData.RoyalPass.Point.OnDataChanged -= OnChangeValueProgress;
    }
}
