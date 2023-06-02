using Framework;
using GooglePlayGames.BasicApi;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSClient : WSClientBase
{
    protected override void Awake()
    {
        base.Awake();
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.LOGIN, OnLogin);
    }
    public void OnLogin(JSONNode data)
    {
        PResourceType.Diamond.SetValue(int.Parse(data["d"]));
        PResourceType.Beri.SetValue(int.Parse(data["b"]));

        ProfileData a = new ProfileData();
        /*{
            Username = data["n"],
            Point = int.Parse(data["p"]),
            Avatar = int.Parse(data["a"]),
            ShipDestroyed = int.Parse(data["sD"]),
            PerfectGame = int.Parse(data["pG"]),
            WinStreak = int.Parse(data["wSN"]),
            WinStreakMax = int.Parse(data["wSM"]),
            Wins = int.Parse(data["wC"]),
            Losts = int.Parse(data["lC"]),
            Battles = int.Parse(data["wC"]) + int.Parse(data["lC"]),
            WinRate = int.Parse(data["wR"]),
            Achievement = (List<int>)JsonUtility.FromJson(data["achie"], typeof(List<int>)),
            AchievementObtained = (List<int>)JsonUtility.FromJson(data["achie_r"], typeof(List<int>)),
        };*/
        {
            a.Username = data["n"];
            a.Point = int.Parse(data["p"]);
            a.Avatar = int.Parse(data["a"]);
            a.ShipDestroyed = int.Parse(data["sD"]);
            a.PerfectGame = int.Parse(data["pG"]);
            a.WinStreak = int.Parse(data["wSN"]);
            a.WinStreakMax = int.Parse(data["wSM"]);
            a.Wins = int.Parse(data["wC"]);
            a.Losts = int.Parse(data["lC"]);
            a.Battles = a.Wins + a.Losts;
            a.WinRate = int.Parse(data["wR"]);
            a.Achievement = (List<int>)JsonUtility.FromJson(data["achie"], typeof(List<int>));
            a.AchievementObtained = (List<int>)JsonUtility.FromJson(data["achie_r"], typeof(List<int>));
        };
        GameData.Player = a;
        CoreGame.timeInit = int.Parse(data["t"]);
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
        Send(jsonNode);
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
        Send(jsonNode);
    }
    public static void QuitSearch(int bet)
    {
        JSONNode jsonNode = new JSONClass
        {
            { "id", GameServerEvent.QUIT_SEARCH.ToJson() },
            { "r", bet.ToJson() },
        };
        Send(jsonNode);
    }
    public static void QuitGame(int room)
    {
        JSONNode jsonNode = new JSONClass
        {
            { "id", GameServerEvent.QUIT_GAME.ToJson() },
            { "r", room.ToJson() },
        };
        Send(jsonNode);
    }
}
