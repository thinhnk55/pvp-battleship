using Framework;
using GooglePlayGames.BasicApi;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class WSClient : WSClientBase
{
    protected override void Start()
    {
        base.Start();
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.LOGIN, OnLogin);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_ACHIEVEMENT, ReceiveAchievementConfig);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_LUCKY_SHOT_CONFIG, ReceiveLuckyShotConfig);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.LOGIN, OnLogin);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_ACHIEVEMENT, ReceiveAchievementConfig);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_LUCKY_SHOT_CONFIG, ReceiveLuckyShotConfig);

    }
    public void OnLogin(JSONNode data)
    {
        PResourceType.Diamond.SetValue(int.Parse(data["d"]));
        PResourceType.Beri.SetValue(int.Parse(data["b"]));
        ProfileData profile = GameData.Player;
        GameData.Player = ProfileData.FromJson(ref profile, data);
        if (GameData.Version != int.Parse(data["v"]) || GameData.Player.AchievementConfig == null || GameData.Player.AchievementConfig.Count==0)
        {
            GameData.Version = int.Parse(data["v"]);
            RequestAchievementConfig();
            RequestLuckyShotConfig();
        }

        CoreGame.timeInit = int.Parse(data["t"]);
        CoreGame.bets = data["bet"].ToList();
        GameData.LuckyShotConfig.Log();
    }
    public void ReceiveAchievementConfig(JSONNode data)
    {
        var achie = GameData.Player;
        achie.AchievementConfig = new Dictionary<AchievementType, AchievementInfo>();
        GameData.Player = achie;
        for (int i = 0; i < data["achie"].Count; i++)
        {
            GameData.Player.AchievementConfig.Add((AchievementType)i, AchievementInfo.FromJson(data["achie"][i], i));
        }
        Debug.Log(GameData.Player.ToString());
    }
    public void ReceiveLuckyShotConfig(JSONNode data)
    {
        List<int> luckyShots = data["list"].ToList(true);
        GameData.LuckyShotConfig = luckyShots;

        Debug.Log(data);
    }
    public static void RequestLuckyShotConfig()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_LUCKY_SHOT_CONFIG.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    public static void RequestAchievementConfig()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_ACHIEVEMENT.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    public static void RequestShot()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_LUCKY_SHOT.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    public static void SearchOpponent(int bet, List<Ship> ships)
    {
        JSONNode jsonNode = new JSONClass();
        JSONArray jsonArray = new JSONArray();
        foreach (Ship ship in ships)
        {
            jsonArray.Add(ship.ToJson());
        }
        jsonNode.Add("id", GameServerEvent.SEARCHOPPONENT.ToJson() );
        jsonNode.Add("b", bet.ToJson());
        jsonNode.Add("ship", jsonArray);
        Instance.Send(jsonNode);
    }

    public static void AttackOpponent(int room, int x, int y)
    {
        JSONNode jsonNode = new JSONClass
        {
            { "id", GameServerEvent.ATTACK.ToJson() },
            { "r",  room.ToJson() },
            { "x",  x.ToJson() },
            { "y",  y.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    public static void QuitSearch(int bet)
    {
        JSONNode jsonNode = new JSONClass
        {
            { "id", GameServerEvent.QUIT_SEARCH.ToJson() },
            { "r", bet.ToJson() },
        };
        Instance.Send(jsonNode);
    }
    public static void QuitGame(int room)
    {
        JSONNode jsonNode = new JSONClass
        {
            { "id", GameServerEvent.QUIT_GAME.ToJson() },
            { "r", room.ToJson() },
        };
        Instance.Send(jsonNode);
    }

    public static void RequestObtainAchievemnt(int id, int obtained)
    {
        JSONNode jsonNode = new JSONClass
        {
            { "id", GameServerEvent.REQUEST_OBTAIN_ACHIEVEMENT.ToJson() },
            { "achieId", id.ToJson() },
            { "achieIndex", (obtained + 1).ToJson() }
        };
        Instance.Send(jsonNode);
    }
    public static void RequesRematch()
    {
        JSONNode jsonNode = new JSONClass()
        {
            { "id", GameServerEvent.REQUEST_LUCKY_SHOT.ToJson() },
        };
        Instance.Send(jsonNode);
    }
}
