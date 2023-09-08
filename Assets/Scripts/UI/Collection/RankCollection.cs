using Framework;
using SimpleJSON;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankCollection : CardCollectionBase<RankInfo>
{
    [SerializeField] RankCard previewCard;
    [SerializeField] Image rankIndicator;
    [SerializeField] Image indicator;
    [SerializeField] float offsetIndicator;
    [SerializeField] TextMeshProUGUI countDown;
    [SerializeField] TextMeshProUGUI reward;
    [SerializeField] TextMeshProUGUI rankRequired;
    [SerializeField] Transform resource;
    private void Start()
    {
        OnSelectedCard += (oldCard, newCard) =>
        {
            if (oldCard && ((RankCard)oldCard).BG)
                ((RankCard)oldCard).BG.sprite = SpriteFactory.UnselectedRankBG;
            if (((RankCard)newCard).BG)
                ((RankCard)newCard).BG.sprite = SpriteFactory.SelectedRankBG;
            SetCardPreview(newCard.Info);
        };
        UpdateUIs();
        indicator.transform.parent = cards[GameData.Player.Rank].transform;
        indicator.GetComponent<RectTransform>().anchoredPosition = Vector3.up * offsetIndicator;
        SelectedCard = cards[GameData.Player.Rank];
        rankIndicator.sprite = SpriteFactory.ResourceIcons[(int)PNonConsumableType.AVATAR].sprites[GameData.Player.Avatar.Data];
        Timer<RankCollection>.Instance.Init(OnTrigger, OnElapse);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._RANK_REWARD, ReceiveRank);
    }
    private void OnDestroy()
    {
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._RANK_REWARD, ReceiveRank);

    }
    public void SetCardPreview(RankInfo info)
    {
        if (Timer<RankCollection>.Instance.TriggersFromBegin >= 1 && info.Id == GameData.Player.Rank)
        {
            info.OnClick = () =>
            {
                WSClientHandler.GetRankReward();
            };
        }
        else
        {
            info.OnClick = null;
        }
        reward.text = GameData.RankConfigs[info.Id].Reward.ToString();
        if (info.Id <= GameData.Player.Rank)
        {
            countDown.gameObject.SetActive(true);
            rankRequired.transform.parent.gameObject.SetActive(false);
        }
        if (info.Id < GameData.Player.Rank)
        {
            countDown.text = "Received";

        }
        else if (info.Id > GameData.Player.Rank)
        {
            countDown.gameObject.SetActive(false);
            rankRequired.transform.parent.gameObject.SetActive(true);
            rankRequired.text = "Unlocked when reaching " + GameData.RankConfigs[info.Id].Point;
            rankRequired.GetComponentInParent<LayoutCalibrator>().Calibrate();
        }
        previewCard.BuildUI(info);
    }

    private void OnTrigger()
    {
        if (Timer<RankCollection>.Instance.TriggersFromBegin == 1)
        {
            SetCardPreview(SelectedCard.Info);
        }
    }

    private void OnElapse()
    {
        countDown.text = Timer<RankCollection>.Instance.TriggersFromBegin >= 1 ? "Obtain" : "Received salary in " + Timer<RankCollection>.Instance.RemainTime_Sec.Hour_Minute_Second_1();
    }

    void ReceiveRank(JSONNode json)
    {
        PConsumableType.BERRY.SetValue(int.Parse(json["d"]["g"]));
        CoinVFX.CoinVfx(resource, previewCard.Position, previewCard.Position);
        Timer<RankCollection>.Instance.BeginPoint = DateTime.UtcNow.Ticks;
        SetCardPreview(SelectedCard.Info);
        ConditionalMono.conditionalEvents[typeof(RankReminder)].ForEach((con) => con.UpdateObject());
    }
    // Update is called once per frame
    void Update()
    {
        if (previewCard.Info.Id == GameData.Player.Rank)
        {
            Timer<RankCollection>.Instance.Elasping();
        }
    }

    public override void UpdateUIs()
    {

        List<RankInfo> infos = new List<RankInfo>();
        for (int i = 0; i < GameData.RankConfigs.Count; i++)
        {
            int _i = i;
            infos.Add(new RankInfo()
            {
                Id = _i,
                Icon = SpriteFactory.Ranks[i],
                Title = GameData.RankConfigs[i].Title,
                Point = GameData.RankConfigs[i].Point,
                OnClick = () =>
                {
                    SelectedCard = cards[_i];
                }
            });
        }
        BuildUIs(infos);
    }
}
