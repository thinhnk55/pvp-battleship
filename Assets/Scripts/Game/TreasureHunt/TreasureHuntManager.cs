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

    Dictionary<Vector2, TreasureBoardCell> cells = new Dictionary<Vector2, TreasureBoardCell>();

    private void OnEnable()
    {
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_LUCKY_SHOT, OnCellShot);
    }

    private void OnDisable()
    {
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_LUCKY_SHOT, OnCellShot);
    }

    private void Start()
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

    public void ResetCells()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            cells.ElementAt(i).Value.SetNotShot();
        }
    }

    public void TryShootCell(int x, int y)
    {
        Debug.Log($"shooting cell {x} {y}");
    }

    public void OnCellShot(JSONNode node)
    {
        int status = int.Parse(node["s"]);
    }
}
