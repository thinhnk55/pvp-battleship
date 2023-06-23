using Framework;
using SimpleJSON;
using System;
using System.Collections;
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
    [SerializeField] Transform resource;
    private void Start()
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
                    SetCardPreview(infos[_i]);
                }
            });
        }
        BuildUIs(infos);
        indicator.transform.parent = cards[GameData.Player.Rank].transform;
        indicator.GetComponent<RectTransform>().anchoredPosition = Vector3.up * offsetIndicator;
        SetCardPreview(cards[GameData.Player.Rank].Info);
        rankIndicator.sprite = SpriteFactory.Avatars[GameData.Player.Avatar];

        Timer<RankCollection>.Instance.Init(60, OnTrigger, OnElapse);

        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_RANK, ReceiveRank);
    }

    public void SetCardPreview(RankInfo info)
    {
        info.OnClick = () =>
        {
            WSClient.RequestRank();
        };
        previewCard.BuildUI(info);
    }

    private void OnTrigger()
    {
        countDown.text = Timer<Gift>.Instance.TriggerCountTotal >= 1 ? "Obtain" : Timer<Gift>.Instance.RemainTimeInsecond.Hour_Minute_Second_1();
        previewCard.Button.onClick.RemoveAllListeners();
    }

    private void OnElapse()
    {
        countDown.text = Timer<Gift>.Instance.TriggerCountTotal >= 1 ? "Obtain" : Timer<Gift>.Instance.RemainTimeInsecond.Hour_Minute_Second_1();
    }

    void ReceiveRank(JSONNode json)
    {
        PResourceType.BERI.AddValue(int.Parse(json["value"]));
        CoinVFX.CoinVfx(resource, Position, Position);
        Timer<RankCollection>.Instance.LastTime = DateTime.UtcNow.Ticks;
    }
    // Update is called once per frame
    void Update()
    {
        Timer<RankCollection>.Instance.Elasping();
    }
}
