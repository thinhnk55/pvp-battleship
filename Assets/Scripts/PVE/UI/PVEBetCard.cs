using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PVEBetCard : CardBase<PVEBetInfo>
{
    RectTransform rectTransform;
    RectTransform contentTransform;
    [SerializeField] TextMeshProUGUI nameCard;
    [SerializeField] TextMeshProUGUI cost;
    protected override void OnClicked(PVEBetInfo info)
    {
    }

    public override void BuildUI(PVEBetInfo info)
    {
        base.BuildUI(info);
        Button?.onClick.RemoveAllListeners();
        Button?.onClick.AddListener(() =>
        {
            Debug.Log("Onclick");
            info.onclick?.Invoke();
        });
        //nameCard.SetText(info.name);
        cost?.SetText(info.cost.ToString());
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
