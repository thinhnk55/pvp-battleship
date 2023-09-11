using Framework;
using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardGoldCollection : CardCollectionBase<LeaderBoardGoldInfo>
{
    [SerializeField] LeaderBoardGoldCard mySpendingCount;
    private void Start()
    {
        ServerMessenger.AddListener<JSONNode>(ServerResponse._LEADER_BOARD_DATA, WSClientHandler.LeaderBoardData);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._LEADER_DATA, WSClientHandler.LeaderData);
        WSClientHandler.LeaderBoardData();
        WSClientHandler.LeaderData();
    }
    private void OnEnable()
    {
        UpdateUIs();
    }
    private void OnDestroy()
    {
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._LEADER_BOARD_DATA, WSClientHandler.LeaderBoardData);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._LEADER_DATA, WSClientHandler.LeaderData);
    }
    public override void UpdateUIs()
    {
        List<LeaderBoardGoldInfo> winCountInfosList = new List<LeaderBoardGoldInfo>();
        int playerOrder = 51;
        for (int i = 0; i < GameData.LeaderBoard.goldInfos.Count; i++)
        {
            winCountInfosList.Add(new LeaderBoardGoldInfo
            {
                Order = GameData.LeaderBoard.goldInfos[i].Order,
                Rank = GameData.LeaderBoard.goldInfos[i].Rank,
                UserName = GameData.LeaderBoard.goldInfos[i].UserName,
                SpendingCount = GameData.LeaderBoard.goldInfos[i].SpendingCount,
                Reward = GameData.LeaderBoard.goldInfos[i].Reward
            });
        }
        mySpendingCount.BuildUI(new LeaderBoardGoldInfo()
        {
            Order = playerOrder,
            Rank = playerOrder,
            UserName = GameData.Player.Username.Data,
            SpendingCount = GameData.LeaderBoard.win,
            Reward = GameData.LeaderBoard.winReward[playerOrder]
        });
        BuildUIs(winCountInfosList);
    }

}
