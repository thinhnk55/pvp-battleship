using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct BetInfo
{
    public int Index;
    public bool IsQualified;
    public int RewardAmount;
    public int EntryStake;
    public Callback OnClick;
}

public class BetCard : CardBase<BetInfo>
{
    [SerializeField] Image BG;
    [SerializeField] Image lockedImage;
    [SerializeField] TextMeshProUGUI rewardAmount;
    [SerializeField] TextMeshProUGUI entryStake;
    RectTransform rectTransform;
    RectTransform contentTransform;
    public override void BuildUI(BetInfo info)
    {
        base.BuildUI(info);
        if (rewardAmount)
        {
            rewardAmount.text = info.RewardAmount.ToString();
        }
        if (entryStake)
        {
            entryStake.text = info.EntryStake.ToString();
        }
        OnClick += info.OnClick;
        if (info.IsQualified)
        {
            Button.onClick.AddListener(() =>
            {
                OnClicked(info);
            });
            lockedImage.SetAlpha(0);
        }
        else
        {
            lockedImage.SetAlpha(1);
        }
        if (BG)
        {
            BG.sprite = SpriteFactory.Bets.GetLoop(info.Index);
        }

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
        //rectTransform.position += Vector3.right * Time.deltaTime;
        //float scale = 1 + (1 - Mathf.Clamp01( Mathf.Pow((rectTransform.anchoredPosition.x + contentTransform.anchoredPosition.x - Screen.width / 2) / (Screen.height / 2), 2)) ) * 0.5f;
        float scale = 1 + (1 - Mathf.Clamp01(Mathf.Pow((rectTransform.anchoredPosition.x + contentTransform.anchoredPosition.x - SnapScrollView.SnapPointXPos) / (Screen.currentResolution.height / 2), 2))) * 0.5f;
        transform.localScale = scale * Vector3.one;
    }
}
