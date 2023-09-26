using Framework;
using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardGoldCollection : CardCollectionBase<LeaderBoardGoldInfo>
{
    [SerializeField] LeaderBoardGoldCard mySpendingCount;
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
        List<LeaderBoardGoldInfo> winCountInfosList = new List<LeaderBoardGoldInfo>();
        int playerOrder = 51;
        if (GameData.LeaderBoard.goldInfos.Count > 0)
        {
            for (int i = 0; i < GameData.LeaderBoard.goldInfos.Count; i++)
            {
                if (GameData.LeaderBoard.goldInfos[i].UserId == GameData.Player.UserId)
                {
                    playerOrder = i;
                }
                winCountInfosList.Add(new LeaderBoardGoldInfo
                {
                    Order = i,
                    Rank = GameData.RankMilestone.GetMileStone(GameData.LeaderBoard.goldInfos[i].Rank),
                    UserName = GameData.LeaderBoard.goldInfos[i].UserId == GameData.Player.UserId ? GameData.Player.Username.Data : GameData.LeaderBoard.goldInfos[i].UserName,
                    SpendingCount = GameData.LeaderBoard.goldInfos[i].SpendingCount,
                    Reward = GameData.LeaderBoard.goldInfos[i].Reward
                });
            }

        }
        mySpendingCount.BuildUI(new LeaderBoardGoldInfo()
        {
            Order = playerOrder,
            Rank = GameData.Player.Rank,
            UserName = GameData.Player.Username.Data,
            SpendingCount = GameData.LeaderBoard.goldSpend,
            Reward = GameData.LeaderBoard.goldReward.GetClamp(playerOrder),
        });
        BuildUIs(winCountInfosList);
    }
    public void OnUpdate(JSONNode data)
    {
        UpdateUIs();
    }
}
