using Framework;
using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardWinCollection : CardCollectionBase<LeaderBoardWinInfo>
{
    [SerializeField] LeaderBoardWinCard myWinCount;
    [SerializeField] Image receiveReward;
    private void Start()
    {
        ServerMessenger.AddListener<JSONNode>(ServerResponse._LEADER_BOARD_DATA, WSClientHandler.LeaderBoardData);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._LEADER_DATA, WSClientHandler.LeaderData);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._LEADER_DATA, OnUpdate);
        receiveReward.gameObject.SetActive(GameData.LeaderBoard.rankWinPreviousAvailable);
        receiveReward.sprite = SpriteFactory.ResourceIcons[(int)PConsumableType.BERRY].sprites[LeaderBoard.GetIconReward(GameData.LeaderBoard.rankWinPrevious)];
    }
    private void OnEnable()
    {
        WSClientHandler.LeaderBoardData();
        WSClientHandler.LeaderData();
    }
    private void OnDestroy()
    {
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._LEADER_BOARD_DATA, WSClientHandler.LeaderBoardData);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._LEADER_DATA, WSClientHandler.LeaderData);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._LEADER_DATA, OnUpdate);

    }
    public override void UpdateUIs()
    {
        List<LeaderBoardWinInfo> winCountInfosList = new List<LeaderBoardWinInfo>();
        int playerOrder = 51;
        if (GameData.LeaderBoard.winInfos.Count > 0)
        {
            for (int i = 0; i < GameData.LeaderBoard.winInfos.Count; i++)
            {
                if (GameData.LeaderBoard.winInfos[i].UserId == GameData.Player.UserId)
                {
                    playerOrder = i;
                }
                winCountInfosList.Add(new LeaderBoardWinInfo
                {
                    Order = i,
                    Rank = GameData.RankMilestone.GetMileStone(GameData.LeaderBoard.winInfos[i].Rank),
                    UserName = GameData.LeaderBoard.winInfos[i].UserId == GameData.Player.UserId ? GameData.Player.Username.Data : GameData.LeaderBoard.winInfos[i].UserName,
                    WinCount = GameData.LeaderBoard.winInfos[i].WinCount,
                    Reward = GameData.LeaderBoard.winInfos[i].Reward
                });

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
    public void OnUpdate(JSONNode data)
    {
        UpdateUIs();
    }
}
