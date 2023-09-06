using Framework;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatisticType
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
public static class StatisticTracker
{
    static public Dictionary<StatisticType, PDataUnit<int>> quests;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        quests = new Dictionary<StatisticType, PDataUnit<int>>();
        foreach (StatisticType value in Enum.GetValues(typeof(StatisticType)))
        {
            value.AddQuest();
        }

        Messenger.AddListener<Ship>(GameEvent.SHIP_DESTROY, ShipDestroyed);
        Messenger.AddListener<bool>(GameEvent.GAME_END, GameEnd);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._LUCKYSHOT_FIRE, LuckyShotFire);
        PConsumableType.GEM.GetData().OnDataChanged += OnGemChange;
        PNonConsumableType.AVATAR.GetData().OnDataChanged += OnAvatarChange;
        PNonConsumableType.AVATAR_FRAME.GetData().OnDataChanged += OnAvatarFrameChange;

        Messenger.AddListener(GameEvent.LostConnection, LostConnection);

    }
    public static void AddQuest(this StatisticType requireTypes)
    {
        quests.Add(requireTypes, new PDataUnit<int>(0));
    }
    public static void AddListenerOnProgress(this StatisticType requireTypes, Callback<int, int> onProgress)
    {
        quests[requireTypes].OnDataChanged += onProgress;
    }
    public static void RemoveListenerOnProgress(this StatisticType requireTypes, Callback<int, int> onProgress)
    {
        quests[requireTypes].OnDataChanged -= onProgress;
    }
    public static void RemoveAllListenerOnProgress(this StatisticType requireTypes)
    {
        quests[requireTypes].OnDataChanged = null;
    }
    public static void SetProgress(this StatisticType requireTypes, int progress)
    {
        quests[requireTypes].Data = progress;
    }
    public static void AddProgress(this StatisticType requireTypes, int progress)
    {
        quests[requireTypes].Data = quests[requireTypes].Data + progress;
    }

    private static void OnAvatarFrameChange(HashSet<int> oValue, HashSet<int> nValue)
    {
        StatisticType.AVATAR_FRAME.SetProgress(nValue.Count);
    }
    private static void OnAvatarChange(HashSet<int> oValue, HashSet<int> nValue)
    {
        StatisticType.AVATAR.SetProgress(nValue.Count);
    }

    private static void OnGemChange(int oValue, int nValue)
    {
        if (nValue < oValue)
        {
            StatisticType.GEM_USED.AddProgress(oValue - nValue);
        }
    }

    private static void GameEnd(bool win)
    {
        if (win)
        {
            StatisticType.WIN_COUNT.AddProgress(1);
            int countDestroy = CoreGame.Instance.player.ships.FindAll((ship) => ship.isDestroyed).Count;
            if (countDestroy == 0)
            {
                StatisticType.PERFECT_GAME.AddProgress(1);
            }
            if (countDestroy == 9)
            {
                StatisticType.ALIVE_1_SHIP.AddProgress(1);
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

            StatisticType.SHIP_DESTROY.AddProgress(1);
            int length = ship.octilesComposition.Count - 1;
            switch (length)
            {
                case 0:
                    StatisticType.SHIP_0_DESTROY.AddProgress(1);
                    break;
                case 1:
                    StatisticType.SHIP_1_DESTROY.AddProgress(1);
                    break;
                case 2:
                    StatisticType.SHIP_2_DESTROY.AddProgress(1);
                    break;
                case 3:
                    StatisticType.SHIP_3_DESTROY.AddProgress(1);
                    break;
                default:
                    break;
            }
        }
    }
    private static void LuckyShotFire(JSONNode data)
    {
        StatisticType.LUCKY_SHOT_COUNT.AddProgress(1);
    }

    private static void LostConnection()
    {
        foreach (StatisticType quest in Enum.GetValues(typeof(StatisticType)))
        {
            quest.RemoveAllListenerOnProgress();
        }
    }
}

