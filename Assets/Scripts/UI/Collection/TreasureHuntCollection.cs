using Framework;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureHuntCollection : CardCollectionBase<TreasureHuntInfo>
{
    [SerializeField] GameObject treasureHuntPopup;

    public override void UpdateUIs()
    {
        List<TreasureHuntInfo> infos = new List<TreasureHuntInfo>();
        for (int i = 0; i < GameData.TreasureConfigs.Count; i++)
        {
            infos.Add(new TreasureHuntInfo()
            {
                Id = GameData.TreasureConfigs[i].Id,
                PrizeAmount = GameData.TreasureConfigs[i].PrizeAmount,
                OnClick = TryJoinRoom
            });
        }

        BuildUIs(infos);
    }
    private void OnEnable()
    {
        ServerMessenger.AddListener<JSONNode>(ServerResponse.RECIEVE_JOIN_TREASURE_ROOM, ReceiveJoinTreasureRoom);
    }

    private void OnDisable()
    {
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse.RECIEVE_JOIN_TREASURE_ROOM, ReceiveJoinTreasureRoom);
    }

    private void Awake()
    {
        UpdateUIs();
    }

    private void TryJoinRoom()
    {
        if (GameData.JoinTreasureRoom.IsSuccess == 0)
        {
            Debug.Log("Not enough beri !!!");
            return;
        }
    }

    void ReceiveJoinTreasureRoom(JSONNode data)
    {
        Debug.Log("Receive :" + data);
        GameData.JoinTreasureRoom.Id = int.Parse(data["id"]);
        GameData.JoinTreasureRoom.IsSuccess = int.Parse(data["s"]);
        if (GameData.JoinTreasureRoom.IsSuccess == 0)
        {
            return;
        }
        GameData.JoinTreasureRoom.CurrentPrize = int.Parse(data["beri"]);
        GameData.JoinTreasureRoom.Board = new List<List<int>>();

        for (int row = 0; row < 10; row++)
        {
            List<int> rowList = new List<int>();
            for (int col = 0; col < 10; col++)
            {
                rowList.Add(int.Parse(data["board"][col][row]));
            }
            GameData.JoinTreasureRoom.Board.Add(rowList);
        }

        if (treasureHuntPopup != null)
        {
            PopupHelper.Create(treasureHuntPopup);
            TreasureHuntManager.Instance.UpdateBoard();
        }
    }
}
