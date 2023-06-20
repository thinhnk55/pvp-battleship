using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct BetInfo
{
    public int RewardAmount;
    public int EntryStake;
    public Callback onClick;
}

public class BetCard : CardBase<BetInfo>
{
    
    [SerializeField] TextMeshProUGUI RewardAmount;
    [SerializeField] TextMeshProUGUI EntryStake;
    RectTransform rectTransform;
    RectTransform contentTransform;
    public override void BuildUI(BetInfo info)
    {
        base.BuildUI(info);
        RewardAmount.text = info.RewardAmount.ToString();
        EntryStake.text = info.EntryStake.ToString();
        OnClick += info.onClick;

        Button.onClick.AddListener(() =>
        {
            OnClicked(info);
        });
    }
    protected override void OnClicked(BetInfo info)
    {
        OnClick?.Invoke();
    }
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        contentTransform = transform.parent.GetComponent<RectTransform>();
    }
    private void Update()
    {
        float scale = 1 + (1 - Mathf.Clamp01( Mathf.Pow((rectTransform.anchoredPosition.x + contentTransform.anchoredPosition.x - Screen.currentResolution.width / 2) / (Screen.currentResolution.height / 2), 2)) ) * 0.5f;
        transform.localScale = scale * Vector3.one;
    }
}
