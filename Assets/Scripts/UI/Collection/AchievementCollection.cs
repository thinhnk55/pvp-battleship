using Framework;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
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
            OnChangeCard += (oldCard, newCard) =>
            {
                if(oldCard && ((AchievementCard)oldCard).BG)
                    ((AchievementCard)oldCard).BG.sprite = SpriteFactory.UnselectedAchievementBG;
                if (((AchievementCard)newCard).BG)
                    ((AchievementCard)newCard).BG.sprite = SpriteFactory.SelectedAchievementBG;
                SetCardPreview(newCard.Info);
                Debug.Log("Change");
            };
        infos = new List<AchievementInfo>();
        var list = isPlayer == 1 ? GameData.Player.AchievementConfig.ToList() : GameData.Player.AchievementConfig.ToList().GetRange(0, numberOfChild);
        for (int i = 0; i < list.Count; i++)
        {
            AchievementInfo info = list[i].Value;
            if (isSelection)
            {
                if (!GameData.Player.AchievementSelected.Contains(info.Id))
                {
                    info.onClick = () =>
                    {
                        AchievementSelectionCollection.idNewSelect = info.Id;
                        Messenger.Broadcast(GameEvent.SELECT_ACHIEVEMENT);
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
            selectedCard = cards[0];
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
        AchievementInfo info = GameData.Player.AchievementConfig[(AchievementType)int.Parse(json["achieId"])];
        if (int.Parse(json["s"]) == 1)
        {
            GameData.Player.AchievementObtained[int.Parse(json["achieId"])] = GameData.Player.AchievementObtained[int.Parse(json["achieId"])] + 1;
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
}
