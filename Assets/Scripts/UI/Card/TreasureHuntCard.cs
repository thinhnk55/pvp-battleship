using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using TMPro;

public struct TreasureHuntInfo
{
    public int Id;
    public int PrizeAmount;
    public Callback OnClick;
}

public class TreasureHuntCard : CardBase<TreasureHuntInfo>
{
    [SerializeField] TextMeshProUGUI PrizeAmount;
    [SerializeField] TextMeshProUGUI Praticipant;
    RectTransform rectTransform;
    RectTransform contentTransform;

    public override void BuildUI(TreasureHuntInfo info)
    {
        base.BuildUI(info);
        PrizeAmount.text = info.PrizeAmount.ToString();
        OnClick += info.OnClick;

        Button.onClick.AddListener(() =>
        {
            OnClicked(info);
        });
    }
    protected override void OnClicked(TreasureHuntInfo info)
    {
        Debug.Log("Onclick "+info.Id);
        WSClientHandler.RequestJoinTreasureRoom(info.Id);
        OnClick?.Invoke();
    }
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        contentTransform = transform.parent.GetComponent<RectTransform>();
    }
    private void Update()
    {
        float scale = 1 + (1 - Mathf.Clamp01(Mathf.Pow((rectTransform.anchoredPosition.x + contentTransform.anchoredPosition.x - SnapScrollView.SnapPointXPos) / (Screen.currentResolution.height / 2), 2))) * 0.5f;
        transform.localScale = scale * Vector3.one;
    }
}
