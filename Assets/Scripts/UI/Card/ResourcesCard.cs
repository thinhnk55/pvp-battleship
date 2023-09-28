using DG.Tweening;
using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourcesCard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI beri;
    [SerializeField] TextMeshProUGUI gem;
    [SerializeField] float tweenDuration;
    [SerializeField] Tween tweenBeri;
    [SerializeField] Tween tweenGem;

    private void Awake()
    {
        beri.text = PConsumableType.BERRY.GetValue().ToString();
        gem.text = PConsumableType.GEM.GetValue().ToString();
        PConsumableType.GEM.GetData().OnDataChanged += OnChangeValueGem;
        PConsumableType.BERRY.GetData().OnDataChanged += OnChangeValueBeri;
    }
    void OnChangeValueBeri(int oldValue, int newValue)
    {
        tweenBeri.Kill();
        tweenBeri = DOTween.To(() => int.Parse(beri.text), (value) => beri.text = value.ToString(), newValue, tweenDuration).OnComplete(() => beri.text = newValue.ToString());
    }
    void OnChangeValueGem(int oldValue, int newValue)
    {
        tweenGem.Kill();
        tweenGem = DOTween.To(() => int.Parse(gem.text), (value) => gem.text = value.ToString(), newValue, tweenDuration).OnComplete(() => gem.text = newValue.ToString());
    }
    private void OnDestroy()
    {
        PConsumableType.GEM.GetData().OnDataChanged -= OnChangeValueGem;
        PConsumableType.BERRY.GetData().OnDataChanged -= OnChangeValueBeri;
    }
}
