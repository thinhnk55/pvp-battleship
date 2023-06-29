using DG.Tweening;
using Framework;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TreasureHuntManager : SingletonMono<TreasureHuntManager>
{
    [SerializeField] TreasureBoardCell cellPrefab;
    [SerializeField] Transform treasureGrid;
    [SerializeField] GameObject resourceUI;
    [SerializeField] GameObject blockInteract;
    [SerializeField] TextMeshProUGUI countDown;
    [SerializeField] TextMeshProUGUI message;
    [SerializeField] List<GameObject> treasureDigits;

    Dictionary<Vector2, TreasureBoardCell> cells = new Dictionary<Vector2, TreasureBoardCell>();

    private void OnEnable()
    {
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_TREASURE_SHOT, OnCellShot);
        message.text = "";
    }

    private void OnDisable()
    {
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_TREASURE_SHOT, OnCellShot);
    }

    private void Awake()
    {
        InitBoard();
    }

    public void InitBoard()
    {
        treasureGrid.DestroyChildrenImmediate();
        cells.Clear();
        for (int i = 9; i >= 0; i--)
        {
            for (int j = 0; j < 10; j++)
            {
                var newCell = Instantiate<TreasureBoardCell>(cellPrefab, treasureGrid);
                newCell.InitCell(j, i);
                cells.Add(new Vector2(j, i), newCell);
            }
        }
    }

    public void UpdateBoard()
    {
        int count = 0;
        var board = GameData.JoinTreasureRoom.Board;
        for (int i = 0; i < board.Count; i++)
        {
            for (int j = 0; j < board[i].Count; j++)
            {
                var c = new Vector2(i, j);
                if (cells.ContainsKey(c))
                {
                    if (board[i][j] == 0)
                        cells[c].SetIsShot(false);
                    else
                        cells[c].SetIsShot(true);
                }
                count++;
            }
        }
        UpdatePrize(GameData.JoinTreasureRoom.CurrentPrize);
    }

    public void UpdatePrize(int value)
    {
        var digits = value.ToString().ToList<char>();
        int digitCount = digits.Count - 1;
        for (int i = 0; i < digits.Count; i++)
        {
            treasureDigits[treasureDigits.Count - 1 - i].GetComponentInChildren<TextMeshProUGUI>().text = digits[digitCount - i].ToString();
        }
    }

    public void UpdateCell(int x, int y, bool isShot)
    {
        var c = new Vector2(x, y);
        if (cells.ContainsKey(c))
        {
            cells[c].SetIsShot(isShot);

        }
    }

    public void ResetCells()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            cells.ElementAt(i).Value.SetIsShot(false);
        }
    }

    public void TryShootCell(int x, int y)
    {
        Debug.Log($"shooting cell {x} {y}");
        WSClient.RequestShootTreasure(x, y);
    }

    public void OnCellShot(JSONNode node)
    {
        int status = int.Parse(node["s"]);
        int y = int.Parse(node["x"]);       // x from server is y in game
        int x = int.Parse(node["y"]);       // y from server is x in game
        Debug.Log($"cell {x} {y} is shot: result {status}");

        UpdateCell(x, y, status >= 0 && status <= 2);
        if (status == 2 || status == 1)
            ResetCells();
        if (status == 2)
        {
            string name = node["name"];
            string beri = node["beri"];
            message.text = $"{name} hit the treasure ship and get {beri} beri.\n" + message.text;
            if (message.text.Length > 400)
                message.text = message.text.Substring(0, 400);
        }
    }

    public void ExitRoom()
    {
        WSClient.RequestExitTreasureRoom();
        GetComponent<PopupBehaviour>().ForceClose();
    }
}
