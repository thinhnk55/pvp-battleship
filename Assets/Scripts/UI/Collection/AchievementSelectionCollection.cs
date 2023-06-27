using Framework;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AchievementSelectionCollection : AchievementCollection
{
    public static int slot, idOldSelect, idNewSelect;

    private void Start()
    {
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_ACHIEVEMENT_CHANGE, ReceiveChangeAchievement);
        List<AchievementInfo> infosArr = new List<AchievementInfo>();
        if (isPlayer == 1)
        {
            for (int i = 0; i < GameData.Player.AchievementSelected.Length; i++)
            {
                if (GameData.Player.AchievementSelected[i]>=0)
                {
                    AchievementInfo info = GameData.AchievementConfig[(AchievementType)(GameData.Player.AchievementSelected[i] / 100)];
                    info.Obtained = GameData.Player.AchievementObtained[GameData.Player.AchievementSelected[i] / 100];
                    infosArr.Add(info);
                }
            }
        }
        else
        {
            for (int i = 0; i < GameData.Opponent.AchievementSelected.Length; i++)
            {
                if (GameData.Opponent.AchievementSelected[i] >= 0)
                {
                    AchievementInfo info = GameData.AchievementConfig[(AchievementType)(GameData.Opponent.AchievementSelected[i] / 100)];
                    info.Obtained = GameData.Opponent.AchievementObtained[GameData.Opponent.AchievementSelected[i] / 100];
                    infosArr.Add(info);
                }
            }
        }
        infos = infosArr.ToList();
        BuildUIs(infos);
    }

    private void OnDestroy()
    {
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_ACHIEVEMENT_CHANGE, ReceiveChangeAchievement);
    }

    public void Select(int slot)
    {
        AchievementSelectionCollection.slot = slot;
        PopupHelper.Create(popup.gameObject);
    }

    public void ReceiveChangeAchievement(JSONNode json)
    {
        var info = GameData.AchievementConfig[(AchievementType)(idNewSelect / 100)];
        info.Obtained = GameData.Player.AchievementObtained[idNewSelect / 100];
        if (slot >= contentRoot.childCount)
        {
            AddUI(info);
        }
        else
        {
            ModifyUIAt(slot, info);
        }
        GameData.Player.AchievementSelected[slot] = idNewSelect;
    }
}
