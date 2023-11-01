using Framework;
using Server;
using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatisticType
{
    SHIP_DESTROY = 0,//ShipDestroyed
    AVATAR = 1,// OnAvatarChange
    WIN_COUNT = 2,// GameEnd
    WIN_STREAK = 3, // GameEnd Todo
    SHIP_DESTROY_CONSECUTIVE_MAX = 4, // ShipDestroyed, Ship Hit, End Game
    SHIP_0_DESTROY = 5,// ShipDestroyed
    SHIP_1_DESTROY = 6,// ShipDestroyed
    SHIP_2_DESTROY = 7,// ShipDestroyed
    SHIP_3_DESTROY = 8,// ShipDestroyed
    PERFECT_GAME = 9,// GameEnd
    ALIVE_1_SHIP = 10,// GameEnd
    GEM_USED = 11,// OnGemChange
    AVATAR_FRAME = 12,// OnAvatarFrameChange
    BATTLEFIELD = 13,// OnBattleFieldChange

    PLAY_COUNT = 14,// GameEnd
    LUCKY_SHOT_COUNT = 15,// LuckyShotFire
    SHIP_DESTROY_CONSECUTIVE_2 = 16, // ShipDestroyed, Ship Hit, End Game
    DESTROY_SHIP_CONSECUTIVE_3 = 17 // ShipDestroyed, Ship Hit, End Game

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
        Messenger.AddListener<Ship>(GameEvent.SHIP_HIT, ShipHit);
        Messenger.AddListener<bool>(GameEvent.GAME_END, GameEnd);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._LUCKYSHOT_FIRE, LuckyShotFire);
        quests[StatisticType.SHIP_DESTROY_CONSECUTIVE_MAX].OnDataChanged += OnConsecutiveKillShipChange;
        PConsumableType.GEM.GetData().OnDataChanged += OnGemChange;
        PNonConsumableType.AVATAR.GetData().OnDataChanged += OnAvatarChange;
        PNonConsumableType.AVATAR_FRAME.GetData().OnDataChanged += OnAvatarFrameChange;
        PNonConsumableType.BATTLE_FIELD.GetData().OnDataChanged += OnBattleFieldChange;
        Messenger.AddListener(GameEvent.LostConnection, LostConnection);
        WSClient.Instance.OnDisconnect += LostConnection;
    }
    public static void AddQuest(this StatisticType statisticTypes)
    {
        quests.Add(statisticTypes, new PDataUnit<int>(0));
    }
    public static void AddListenerOnProgress(this StatisticType statisticTypes, Callback<int, int> onProgress)
    {
        quests[statisticTypes].OnDataChanged += onProgress;
    }
    public static void RemoveListenerOnProgress(this StatisticType statisticTypes, Callback<int, int> onProgress)
    {
        quests[statisticTypes].OnDataChanged -= onProgress;
    }
    public static void RemoveAllListenerOnProgress(this StatisticType statisticTypes)
    {
        quests[statisticTypes].OnDataChanged = null;
    }
    public static void SetProgress(this StatisticType statisticTypes, int progress)
    {
        quests[statisticTypes].Data = progress;
    }
    public static void AddProgress(this StatisticType statisticTypes, int progress)
    {
        quests[statisticTypes].Data = quests[statisticTypes].Data + progress;
        Debug.Log(statisticTypes.ToString() + " progress changed " + progress);
    }

    private static void OnAvatarFrameChange(HashSet<int> oValue, HashSet<int> nValue)
    {
        StatisticType.AVATAR_FRAME.SetProgress(nValue.Count);
    }
    private static void OnAvatarChange(HashSet<int> oValue, HashSet<int> nValue)
    {
        StatisticType.AVATAR.SetProgress(nValue.Count);
    }
    private static void OnBattleFieldChange(HashSet<int> oValue, HashSet<int> nValue)
    {
        StatisticType.BATTLEFIELD.SetProgress(nValue.Count);
    }
    private static void OnGemChange(int oValue, int nValue)
    {
        if (nValue < oValue)
        {
            StatisticType.GEM_USED.AddProgress(oValue - nValue);
        }
    }
    private static void OnConsecutiveKillShipChange(int oValue, int nValue)
    {
        switch (nValue)
        {
            case 2:
                StatisticType.SHIP_DESTROY_CONSECUTIVE_2.AddProgress(1);
                break;
            case 3:
                StatisticType.DESTROY_SHIP_CONSECUTIVE_3.AddProgress(1);
                break;
            default:
                break;
        }
        Debug.Log("Consecutive track:" + nValue);
        StatisticType.SHIP_DESTROY_CONSECUTIVE_MAX.SetProgress(nValue);
    }
    private static void GameEnd(bool win)
    {
        if (win)
        {
            StatisticType.WIN_COUNT.AddProgress(1);
            int countRemain = CoreGame.Instance.player.ships.Count;
            if (countRemain == 10)
            {
                StatisticType.PERFECT_GAME.AddProgress(1);
            }
            if (countRemain == 1)
            {
                StatisticType.ALIVE_1_SHIP.AddProgress(1);
            }
            GameData.Player.WinStreak++;
        }
        else
        {
            GameData.Player.WinStreak = 0;
            GameData.Player.Losts++;
        }
        StatisticType.PLAY_COUNT.AddProgress(1);
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
            CoreGame.Instance.consecutiveKill++;
            StatisticType.SHIP_DESTROY_CONSECUTIVE_MAX.SetProgress(Math.Max(quests[StatisticType.SHIP_DESTROY_CONSECUTIVE_MAX].Data, CoreGame.Instance.consecutiveKill));
        }
        else
        {
            CoreGame.Instance.consecutiveKill = 0;
        }
        Debug.Log("Current Consecutive " + CoreGame.Instance.consecutiveKill);

    }
    private static void ShipHit(Ship ship)
    {
        if (ship.board != CoreGame.Instance.player)
        {

        }
        else
        {
            CoreGame.Instance.consecutiveKill = 0;
        }
        Debug.Log("Current Consecutive " + CoreGame.Instance.consecutiveKill);
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

