using Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PVEBetCard : CardBase<PVEBetInfo>
{
    RectTransform rectTransform;
    RectTransform contentTransform;
    [SerializeField] Image BG;
    [SerializeField] Image lockImgae;
    [SerializeField] TextMeshProUGUI nameCard;
    [SerializeField] TextMeshProUGUI cost;
    [SerializeField] TextMeshProUGUI nameButton;
    protected override void OnClicked(PVEBetInfo info)
    {
    }

    public override void BuildUI(PVEBetInfo info)
    {
        base.BuildUI(info);
        Button?.onClick.RemoveAllListeners();
        Button?.onClick.AddListener(() =>
        {
            PDebug.Log("Onclick");
            info.onclick?.Invoke();
        });

        if (PVEData.TypeBoard != -1 && PVEData.IsDeadPlayer.Data == false) // Old game
        {
            if (info.id != PVEData.TypeBoard) // Not current bet pve
            {
                lockImgae.SetAlpha(1);
                Button.gameObject.SetActive(false);
            }
            else // Current bet pve
            {
                lockImgae.SetAlpha(0);
                nameButton.SetText("Resume");
            }
        }
        else // New game
        {
            lockImgae.SetAlpha(0);
        }



        BG.sprite = SpriteFactory.BetsPVE.GetLoop(info.id);
        nameCard.SetText(GameConfig.BetPVENames[info.id]);
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
