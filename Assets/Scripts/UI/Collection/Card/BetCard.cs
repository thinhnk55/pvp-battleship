using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct BetInfo
{
    public int RewardAmount;
    public int EntryStake;
}

public class BetCard : CardBase<BetInfo>
{
    
    [SerializeField] TextMeshProUGUI RewardAmount;
    [SerializeField] TextMeshProUGUI EntryStake;
    RectTransform rectTransform;
    [SerializeField] RectTransform contentTransform;
    public override void BuildUI(BetInfo info)
    {
        RewardAmount.text = info.RewardAmount.ToString();
        EntryStake.text = info.EntryStake.ToString();
    }
    protected override void OnClicked(BetInfo info)
    {
    }
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        contentTransform = transform.parent.GetComponent<RectTransform>();
    }
    private void Update()
    {
        float scale = 1 + (1 - Mathf.Clamp01( Mathf.Pow((rectTransform.anchoredPosition.x + contentTransform.anchoredPosition.x - 960)/ 590, 2)) ) * 0.5f;
        transform.localScale = scale * Vector3.one;
    }
}
