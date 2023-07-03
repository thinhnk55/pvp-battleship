using Facebook.Unity;
using Framework;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
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
            Debug.Log("Change");
        };
        UpdateUIs();
        indicator.transform.parent = cards[GameData.Player.Rank].transform;
        indicator.GetComponent<RectTransform>().anchoredPosition = Vector3.up * offsetIndicator;
        SetCardPreview(cards[GameData.Player.Rank].Info);
        rankIndicator.sprite = SpriteFactory.Avatars[GameData.Player.Avatar.Data];
        Timer<RankCollection>.Instance.Init(OnTrigger, OnElapse);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_RANK, ReceiveRank);
    }
    private void OnDestroy()
    {
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_RANK, ReceiveRank);

    }
    public void SetCardPreview(RankInfo info)
    {
        info.OnClick = () =>
        {
            WSClient.RequestRank();
        };
        reward.text = GameData.RankConfigs[info.Id].Reward.ToString();
        if (info.Id < GameData.Player.Rank)
        {
            countDown.text = "Received";
        }
        else if (info.Id > GameData.Player.Rank)
        {
            countDown.text = "Unlocked when reaching " + GameData.RankConfigs[info.Id].Point;
        }
        previewCard.BuildUI(info);
    }

    private void OnTrigger()
    {
        //countDown.text = Timer<RankCollection>.Instance.TriggerCountTotal >= 1 ? "Obtain" : Timer<RankCollection>.Instance.RemainTimeInsecond.Hour_Minute_Second_1();
    }

    private void OnElapse()
    {
        countDown.text = Timer<RankCollection>.Instance.TriggerCountTotal >= 1 ? "Obtain" : "Received salary in " + Timer<RankCollection>.Instance.RemainTimeInsecond.Hour_Minute_Second_1();
    }

    void ReceiveRank(JSONNode json)
    {
        PConsumableType.BERI.AddValue(int.Parse(json["value"]));
        CoinVFX.CoinVfx(resource, Position, Position);
        Timer<RankCollection>.Instance.LastTime = DateTime.UtcNow.Ticks;
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
                    selectedCard = cards[_i];
                }
            });
        }
        BuildUIs(infos);
    }
}
