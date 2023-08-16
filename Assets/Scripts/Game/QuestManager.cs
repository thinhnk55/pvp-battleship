using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
    SHIP_DESTROY,//ShipDestroyed
    AVATAR,// OnAvatarChange
    WIN_COUNT,// GameEnd
    WIN_STREAK, // GameEnd Todo
    SHIP_DESTROY_CONSECUTIVE, // ShipDestroyed Todo
    SHIP_0_DESTROY,// ShipDestroyed
    SHIP_1_DESTROY,// ShipDestroyed
    SHIP_2_DESTROY,// ShipDestroyed
    SHIP_3_DESTROY,// ShipDestroyed
    PERFECT_GAME,// GameEnd
    ALIVE_1_SHIP,// GameEnd
    GEM_USED,// OnGemChange
    AVATAR_FRAME,// OnAvatarFrameChange

    LUCKY_SHOT_COUNT,// LuckyShotFire
    DESTROY_SHIP_CONSECUTIVE_3 // ShipDestroyed Todo

}
public static class QuestManager
{
    static public Dictionary<QuestType, PDataUnit<int>> quests;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        quests = new Dictionary<QuestType, PDataUnit<int>>();
        foreach (QuestType value in Enum.GetValues(typeof(QuestType)))
        {
            value.AddQuest();
        }

        Messenger.AddListener<Ship>(GameEvent.SHIP_DESTROY, ShipDestroyed);
        Messenger.AddListener<bool>(GameEvent.GAME_END, GameEnd);
        ServerMessenger.AddListener(ServerResponse._LUCKYSHOT_FIRE, LuckyShotFire);
        PConsumableType.GEM.GetData().OnDataChanged += OnGemChange;
        PNonConsumableType.AVATAR.GetData().OnDataChanged += OnAvatarChange;
        PNonConsumableType.AVATAR_FRAME.GetData().OnDataChanged += OnAvatarFrameChange;
    }
    public static void AddQuest(this QuestType requireTypes)
    {
        quests.Add(requireTypes, new PDataUnit<int>(0));
    }
    public static void AddListenerOnProgress(this QuestType requireTypes, Callback<int, int> onProgress)
    {
        quests[requireTypes].OnDataChanged += onProgress;
    }
    public static void RemoveListenerOnProgress(this QuestType requireTypes, Callback<int, int> onProgress)
    {
        quests[requireTypes].OnDataChanged -= onProgress;
    }
    public static void SetProgress(this QuestType requireTypes, int progress)
    {
        quests[requireTypes].Data = progress;
    }
    public static void AddProgress(this QuestType requireTypes, int progress)
    {
        quests[requireTypes].Data = quests[requireTypes].Data + progress;
    }

    private static void OnAvatarFrameChange(HashSet<int> oValue, HashSet<int> nValue)
    {
        QuestType.AVATAR_FRAME.SetProgress(nValue.Count);
    }
    private static void OnAvatarChange(HashSet<int> oValue, HashSet<int> nValue)
    {
        QuestType.AVATAR.SetProgress(nValue.Count);
    }

    private static void OnGemChange(int oValue, int nValue)
    {
        if (nValue < oValue)
        {
            QuestType.GEM_USED.AddProgress(oValue - nValue);
        }
    }

    private static void GameEnd(bool win)
    {
        if (win)
        {
            QuestType.WIN_COUNT.AddProgress(1);
            int countDestroy = CoreGame.Instance.player.ships.FindAll((ship) => ship.isDestroyed).Count;
            if (countDestroy == 0)
            {
                QuestType.PERFECT_GAME.AddProgress(1);
            }
            if (countDestroy == 9)
            {
                QuestType.ALIVE_1_SHIP.AddProgress(1);
            }
            GameData.Player.WinStreak++;
        }
        else
        {
            GameData.Player.WinStreak = 0;
        }
    }

    private static void ShipDestroyed(Ship ship)
    {
        if (ship.board != CoreGame.Instance.player)
        {
            QuestType.SHIP_DESTROY.AddProgress(1);
            int length = ship.octilesComposition.Count - 1;
            switch (length)
            {
                case 0:
                    QuestType.SHIP_0_DESTROY.AddProgress(1);
                    break;
                case 1:
                    QuestType.SHIP_1_DESTROY.AddProgress(1);
                    break;
                case 2:
                    QuestType.SHIP_2_DESTROY.AddProgress(1);
                    break;
                case 3:
                    QuestType.SHIP_3_DESTROY.AddProgress(1);
                    break;
                default:
                    break;
            }
        }
    }
    private static void LuckyShotFire()
    {
        QuestType.LUCKY_SHOT_COUNT.AddProgress(1);
    }
}

