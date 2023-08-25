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
        ServerMessenger.AddListener<JSONNode>(ServerResponse._CHANGE_ACHIEVEMENT, ReceiveChangeAchievement);
        List<AchievementInfo> infosArr = new List<AchievementInfo>();
        if (isPlayer == 1)
        {
            for (int i = 0; i < GameData.Player.AchievementSelected.Length; i++)
            {
                if (GameData.Player.AchievementSelected[i]>=0)
                {
                    AchievementInfo info = GameData.AchievementConfig[(AchievementType)(GameData.Player.AchievementSelected[i])];
                    info.Obtained = GameData.Player.AchievementObtained[GameData.Player.AchievementSelected[i]];
                    infosArr.Add(info);
                }
                else
                {
                    infosArr.Add(new AchievementInfo() { Id = -1 });
                }
            }
        }
        else
        {
            for (int i = 0; i < GameData.Opponent.AchievementSelected.Length; i++)
            {
                if (GameData.Opponent.AchievementSelected[i] >= 0)
                {
                    AchievementInfo info = GameData.AchievementConfig[(AchievementType)(GameData.Opponent.AchievementSelected[i])];
                    info.Obtained = GameData.Opponent.AchievementObtained[GameData.Opponent.AchievementSelected[i]];
                    infosArr.Add(info);
                }
                else
                {
                    infosArr.Add(new AchievementInfo() { Id = -1});
                }
            }
        }
        infos = infosArr.ToList();
        BuildUIs(infos);
    }

    private void OnDestroy()
    {
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._CHANGE_ACHIEVEMENT, ReceiveChangeAchievement);
    }

    public void Select(int slot)
    {
        AchievementSelectionCollection.slot = slot;
        PopupHelper.Create(PrefabFactory.PopupAchie);
    }

    public void ReceiveChangeAchievement(JSONNode json)
    {
        var info = GameData.AchievementConfig[(AchievementType)(idNewSelect)];
        info.Obtained = GameData.Player.AchievementObtained[idNewSelect];
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
