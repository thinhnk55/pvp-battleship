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
            {"v", new JSONData(0)}
        }.RequestServer();
    }
    static void GetConfigTreasure(JSONNode data)
    {
        Debug.Log(PVEData.VerisonPVEData.ToString());
        if (PVEData.VerisonPVEData == int.Parse(data["d"]["version"]))
        {
            return;
        }

        PVEData.VerisonPVEData = int.Parse(data["d"]["version"]);
        for (int i = 0; i < data["d"]["treasure"].Count; i++)
        {
            PVEData.Bets.Add(int.Parse(data["d"]["treasure"][i]["ticket"]));
            PVEData.StageMulReward.Add(data["d"]["treasure"][i]["gift"].ToListInt());
        }
    }
}

