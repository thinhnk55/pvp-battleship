using Framework;
using Server;
using SimpleJSON;
using UnityEngine;
public class WSClientPVE : Singleton<WSClientPVE>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        WSClient.Instance.OnConnect += () =>
        {
            GetConfigTreasure();
            ServerMessenger.AddListener<JSONNode>(ServerResponse._CONFIG_TREASURE, GetConfigTreasure);
        };
        WSClient.Instance.OnDisconnect += () =>
        {
            ServerMessenger.RemoveListener<JSONNode>(ServerResponse._CONFIG_TREASURE, GetConfigTreasure);
        };
    }
    static void GetConfigTreasure()
    {
        new JSONClass()
        {
            {"id" , ServerRequest._CONFIG_TREASURE.ToJson() },
            {"v", new JSONData(PVEData.VerisonPVEConfig)}
        }.RequestServer();
    }
    static void GetConfigTreasure(JSONNode data)
    {
        if (PVEData.VerisonPVEConfig == int.Parse(data["d"]["version"]))
        {
            return;
        }

        PVEData.VerisonPVEConfig = int.Parse(data["d"]["version"]);
        PVEData.Bets.Clear();
        PVEData.StageMulReward.Clear();
        for (int i = 0; i < data["d"]["treasure"].Count; i++)
        {
            PVEData.Bets.Add(int.Parse(data["d"]["treasure"][i]["ticket"]));
            PVEData.StageMulReward.Add(data["d"]["treasure"][i]["gift"].ToListInt());
        }
    }
}

