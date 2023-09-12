using Framework;
using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;
public class LeaderBoardWinCollection : CardCollectionBase<LeaderBoardWinInfo>
{
    [SerializeField] LeaderBoardWinCard myWinCount;
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
        List<LeaderBoardWinInfo> winCountInfosList = new List<LeaderBoardWinInfo>();
        int playerOrder = 51;
        if (GameData.LeaderBoard.winInfos.Count > 0)
        {
            for (int i = 0; i < GameData.LeaderBoard.winInfos.Count; i++)
            {
                winCountInfosList.Add(new LeaderBoardWinInfo
                {
                    Order = i,
                    Rank = GameData.RankMilestone.GetMileStone(GameData.LeaderBoard.winInfos[i].Rank),
                    UserName = GameData.LeaderBoard.winInfos[i].UserName,
                    WinCount = GameData.LeaderBoard.winInfos[i].WinCount,
                    Reward = GameData.LeaderBoard.winInfos[i].Reward
                });
                if (GameData.LeaderBoard.winInfos[i].UserName == GameData.Player.Username.Data)
                {
                    playerOrder = i;
                }
            }

        }
        myWinCount.BuildUI(new LeaderBoardWinInfo()
        {
            Order = playerOrder,
            Rank = GameData.Player.Rank,
            UserName = GameData.Player.Username.Data,
            WinCount = GameData.LeaderBoard.win,
            Reward = GameData.LeaderBoard.winReward.GetClamp(playerOrder)
        });
        BuildUIs(winCountInfosList);

    }

}
