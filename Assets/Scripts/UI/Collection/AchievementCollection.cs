using FirebaseIntegration;
using Framework;
using SimpleJSON;
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
    [SerializeField] GameObject resource;
    private void OnEnable()
    {
        UpdateUIs();
        AnalyticsHelper.SelectContent(isSelection ? "achievement_select" : "achievement");
    }
    public override void BuildUIs(List<AchievementInfo> infos)
    {
        base.BuildUIs(infos);
    }

    public void SetCardPreview(AchievementInfo info)
    {
        AchievementInfo _info = info;
        if (info.Obtained < info.AchivementUnits.Length && info.AchivementUnits[info.Obtained].Task <= info.Progress)
        {
            info.Upgradable = true;
            info.onClick = () => WSClientHandler.RequestObtainAchievemnt(_info.Id);
        }
        else if (info.Obtained >= info.AchivementUnits.Length)
        {
            info.Upgradable = false;
        }
        info.IsPreview = true;
        previewCard.BuildUI(info);
    }

    void RecieveObtainAchievemnt(JSONNode json)
    {
        if (json["e"].AsInt == 0)
        {
            CoinVFX.CoinVfx(resource.transform, previewCard.Button.transform.position, previewCard.Button.transform.position);
            AchievementInfo info = GameData.AchievementConfig[(AchievementType)int.Parse(json["d"]["a"])];
            GameData.Player.AchievementObtained[int.Parse(json["d"]["a"])] = int.Parse(json["d"]["l"]) + 1;
            info.Progress = GameData.Player.AchievementProgress[int.Parse(json["d"]["a"])];
            info.Obtained = json["d"]["l"].AsInt + 1;
            if (previewCard != null)
                info.onClick = () =>
                {
                    SetCardPreview(info);
                };
            cards.Find((card) => card.Info.Id == info.Id).BuildUI(info);
            PConsumableType.BERRY.SetValue(json["d"]["g"].AsInt);
            SetCardPreview(info);
            ConditionalMono.conditionalEvents[typeof(AchievementReminder)].ForEach((con) => con.UpdateObject());
            FirebaseIntegration.AnalyticsHelper.UnlockAchievement(info.Id, info.Obtained);
        }

    }
    void Sort()
    {
        infos.Sort((x, y) =>
        {
            if (x.Obtained <= x.AchivementUnits.Length - 1 && y.Obtained <= y.AchivementUnits.Length - 1)
            {
                if (x.AchivementUnits[x.Obtained].Task > x.Progress && y.AchivementUnits[y.Obtained].Task <= y.Progress)
                {
                    return 1; // x comes before y
                }
                else if (x.AchivementUnits[x.Obtained].Task <= x.Progress && y.AchivementUnits[y.Obtained].Task > y.Progress)
                {
                    return -1; // x comes after y
                }
                else
                {
                    return 0; // x and y are equal in terms of sorting order
                }
            }
            else if (x.Obtained <= x.AchivementUnits.Length - 1)
            {
                return -1; // x comes before y (y is not in the specified range)
            }
            else if (y.Obtained <= y.AchivementUnits.Length - 1)
            {
                return 1; // x comes after y (x is not in the specified range)
            }
            else
            {
                return 0; // x and y are equal in terms of sorting order (both are not in the specified range)
            }

        });
    }
    private void OnDestroy()
    {
        if (previewCard != null && !isSelection)
        {
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse._ACHIEVEMENT_REWARD, RecieveObtainAchievemnt);
        }
    }

    public override void UpdateUIs()
    {
        infos = new List<AchievementInfo>();
        var list = GameData.AchievementConfig.ToList();
        var progress = isPlayer == 1 ? GameData.Player.AchievementProgress : GameData.Opponent.AchievementProgress;
        var obtain = isPlayer == 1 ? GameData.Player.AchievementObtained : GameData.Opponent.AchievementObtained;
        for (int i = 0; i < list.Count; i++)
        {
            AchievementInfo info = list[i];
            if (isSelection)
            {
                if (!GameData.Player.AchievementSelected.Any((select) => select != -1 && select == info.Id))
                {
                    info.onClick = () =>
                    {
                        AchievementSelectionCollection.idNewSelect = info.Id;//* 100 + Mathf.Clamp(GameData.Player.AchievementObtained[info.Id], 0, 4);
                        int[] arr = new int[3] { GameData.Player.AchievementSelected[0], GameData.Player.AchievementSelected[1], GameData.Player.AchievementSelected[2] };
                        arr[AchievementSelectionCollection.slot] = info.Id; //* 100 + Mathf.Clamp(GameData.Player.AchievementObtained[info.Id], 0, 4);
                        WSClientHandler.RequestChangeAchievement(arr);
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
            info.Progress = progress[i];
            info.Obtained = GameData.Player.AchievementObtained[i];

            infos.Add(info);
        }
        if (isSelection)
        {
            infos.RemoveAll((info) => info.Obtained == 0);
        }
        else
        {
            Sort();
        }
        BuildUIs(infos);
        if (!isSelection)
        {
            if (previewCard)
            {
                SelectedCard = cards[0];
                SetCardPreview(cards[0].Info);
                ServerMessenger.AddListener<JSONNode>(ServerResponse._ACHIEVEMENT_REWARD, RecieveObtainAchievemnt);
            }
            OnSelectedCard += (oldCard, newCard) =>
            {
                SoundType.CLICK.PlaySound();
                if (oldCard && ((AchievementCard)oldCard).BG)
                    ((AchievementCard)oldCard).BG.sprite = SpriteFactory.UnselectedAchievementBG;
                if (((AchievementCard)newCard).BG)
                    ((AchievementCard)newCard).BG.sprite = SpriteFactory.SelectedAchievementBG;
                SetCardPreview(newCard.Info);
            };
        }
    }
}
