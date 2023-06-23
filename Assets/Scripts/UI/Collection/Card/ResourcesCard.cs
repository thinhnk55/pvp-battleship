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
    

    void Start()
    {
        beri.text = PResourceType.BERI.GetValue().ToString();
        gem.text = PResourceType.GEM.GetValue().ToString();
        PResourceType.GEM.GetData().OnDataChanged += OnChangeValueGem;
        PResourceType.BERI.GetData().OnDataChanged += OnChangeValueBeri;
    }
    void OnChangeValueBeri(int oldValue, int newValue)
    {
        tweenBeri.Kill();
        tweenBeri = DOTween.To(() => int.Parse(beri.text), (value) => beri.text = value.ToString(), newValue, tweenDuration);
    }
    void OnChangeValueGem(int oldValue, int newValue)
    {
        tweenGem.Kill();
        tweenGem = DOTween.To(() => int.Parse(gem.text), (value) => gem.text = value.ToString(), newValue, tweenDuration);
    }

    private void OnDestroy()
    {
        PResourceType.GEM.GetData().OnDataChanged -= OnChangeValueGem;
        PResourceType.BERI.GetData().OnDataChanged -= OnChangeValueBeri;
    }
}
