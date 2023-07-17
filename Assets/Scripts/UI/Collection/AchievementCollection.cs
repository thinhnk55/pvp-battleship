using Framework;
using SimpleJSON;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AchievementCollection : CardCollectionBase<AchievementInfo>
{
    public int isPlayer;
    public bool isSelection;
    [SerializeField] protected PopupBehaviour popup;
    public int numberOfChild;
    protected List<AchievementInfo> infos;
    [SerializeField] AchievementCard previewCard;
    private void Start()
    {
        if (!isSelection)
            OnSelectedCard += (oldCard, newCard) =>
            {
                if(oldCard && ((AchievementCard)oldCard).BG)
                    ((AchievementCard)oldCard).BG.sprite = SpriteFactory.UnselectedAchievementBG;
                if (((AchievementCard)newCard).BG)
                    ((AchievementCard)newCard).BG.sprite = SpriteFactory.SelectedAchievementBG;
                SetCardPreview(newCard.Info);
            };
        infos = new List<AchievementInfo>();
        var list = isPlayer == 1 ? GameData.AchievementConfig.ToList() : GameData.AchievementConfig.ToList().GetRange(0, numberOfChild);
        for (int i = 0; i < list.Count; i++)
        {
            AchievementInfo info = list[i];
            if (isSelection)
            {
                if (!GameData.Player.AchievementSelected.Any((select) => select!=-1 && select /100 == info.Id))
                {
                    info.onClick = () =>
                    {
                        AchievementSelectionCollection.idNewSelect = info.Id * 100 + Mathf.Clamp(GameData.Player.AchievementObtained[info.Id], 0, 4);
                        int[] arr = new int[3] { GameData.Player.AchievementSelected[0], GameData.Player.AchievementSelected[1], GameData.Player.AchievementSelected[2] };
                        arr[AchievementSelectionCollection.slot] = info.Id * 100 + Mathf.Clamp(GameData.Player.AchievementObtained[info.Id], 0, 4);
                        WSClient.RequestChangeAchievement(arr);
                        popup.ForceClose();
                    };
                }
            }
            else
            {
                if (previewCard != null)
                    info.onClick = () =>
                    {
                        SetCardPreview(info);
                    };
            }

            infos.Add(info);
        }
        BuildUIs(infos);
        if (previewCard != null)
        {
            SelectedCard = cards[0];
            ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_OBTAIN_ACHIEVEMENT, RecieveObtainAchievemnt);
        }

    }
    public override void BuildUIs(List<AchievementInfo> infos)
    {
        base.BuildUIs(infos);
    }

    public void SetCardPreview(AchievementInfo info)
    {
        AchievementInfo _info = info;
        info.onClick = ()=> WSClient.RequestObtainAchievemnt(_info.Id, GameData.Player.AchievementObtained[info.Id]);
        previewCard.BuildUI(info);
    }

    void RecieveObtainAchievemnt(JSONNode json)
    {
        AchievementInfo info = GameData.AchievementConfig[(AchievementType)int.Parse(json["achieId"])];
        if (int.Parse(json["s"]) == 1)
        {
            GameData.Player.AchievementObtained[int.Parse(json["achieId"])] = GameData.Player.AchievementObtained[int.Parse(json["achieId"])] + 1;
            if (previewCard != null)
                info.onClick = () =>
                {
                    SetCardPreview(info);
                };
            cards[int.Parse(json["achieId"])].BuildUI(info);
        }
        SetCardPreview(info);
    }

    private void OnDestroy()
    {
        if (previewCard != null)
        {
            ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_OBTAIN_ACHIEVEMENT, RecieveObtainAchievemnt);
        }
    }

    public override void UpdateUIs()
    {
        throw new NotImplementedException();
    }
}
