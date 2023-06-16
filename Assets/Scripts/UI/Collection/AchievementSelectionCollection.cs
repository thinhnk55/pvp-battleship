using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AchievementSelectionCollection : AchievementCollection
{
    public static int slot, idOldSelect, idNewSelect;
    List<int> selection;
    public void Select(int slot)
    {
        AchievementSelectionCollection.slot = slot;
        PopupHelper.Create(popup.gameObject);
    }
    void SelectAchievement()
    {
        if (slot >= contentRoot.childCount)
        {
            BuildNewUI(GameData.Player.AchievementConfig[(AchievementType)idNewSelect]);
        }
        else
        {
            RebuildUI(slot, GameData.Player.AchievementConfig[(AchievementType)idNewSelect]);
        }
        GameData.Player.AchievementSelected[slot] = idNewSelect;
    }
    private void Start()
    {
        Messenger.AddListener(GameEvent.SELECT_ACHIEVEMENT, SelectAchievement);
        List<AchievementInfo> infosArr = new List<AchievementInfo>();
        if (isPlayer == 1)
        {
            for (int i = 0; i < GameData.Player.AchievementSelected.Length; i++)
            {
                if (GameData.Player.AchievementSelected[i]>=0)
                {
                    infosArr.Add(GameData.Player.AchievementConfig[(AchievementType)GameData.Player.AchievementSelected[i]]);
                }
            }
        }
        else
        {
            for (int i = 0; i < GameData.Opponent.AchievementSelected.Length; i++)
            {
                if (GameData.Opponent.AchievementSelected[i] >= 0)
                {
                    infosArr.Add(GameData.Opponent.AchievementConfig[(AchievementType)GameData.Opponent.AchievementSelected[i]]);
                }
            }
        }
        infos = infosArr.ToList();
        BuildUIs(infos);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.SELECT_ACHIEVEMENT, SelectAchievement);
    }
}
