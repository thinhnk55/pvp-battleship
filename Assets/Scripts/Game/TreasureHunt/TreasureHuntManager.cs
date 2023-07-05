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
    [SerializeField] Transform resourceUI;
    [SerializeField] GameObject blockInteract;
    [SerializeField] TextMeshProUGUI countDown;
    [SerializeField] TextMeshProUGUI message;
    [SerializeField] List<GameObject> treasureDigits;
    [SerializeField] Transform treasureTransform;
    [SerializeField] Toggle autoFillToggle;

    Dictionary<Vector2, TreasureBoardCell> cells = new Dictionary<Vector2, TreasureBoardCell>();
    private bool autoFillMode;
    List<Vector2> cellsShotQueue = new List<Vector2>();
    private bool isInResetAnim;

    private void OnEnable()
    {
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_TREASURE_SHOT, OnCellShot);
        message.text = "";
        delayShootCell = false;
    }

    private void OnDisable()
    {
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_TREASURE_SHOT, OnCellShot);
    }

    private void Awake()
    {
        InitBoard();
        if (autoFillToggle != null) autoFillToggle.onValueChanged.AddListener(OnToggleAutoFill);
    }

    private void OnDestroy()
    {
        if (autoFillToggle != null) autoFillToggle.onValueChanged.RemoveListener(OnToggleAutoFill);
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
        GameData.JoinTreasureRoom.CurrentPrize = value;
        var digits = value.ToString().ToList<char>();
        int digitCount = digits.Count - 1;
        for (int i = 0; i < digits.Count; i++)
        {
            treasureDigits[treasureDigits.Count - 1 - i].GetComponentInChildren<TextMeshProUGUI>().text = digits[digitCount - i].ToString();
        }
    }

    public void ShootCell(int x, int y, bool treasureHit)
    {
        var c = new Vector2(x, y);
        if (cells.ContainsKey(c))
        {
            cells[c].PlayShootAnim(treasureHit);
        }
    }

    public void ResetCells()
    {
        isInResetAnim = true;
        cellsShotQueue.Clear();
        for (int i = 0; i < cells.Count; i++)
        {
            //cells.ElementAt(i).Value.SetIsShot(false);
            cells.ElementAt(i).Value.ResetCell();
        }
        if (setCellsAfterResetCoroutine != null) StopCoroutine(setCellsAfterResetCoroutine);
        setCellsAfterResetCoroutine = StartCoroutine(SetShotCellsAfterReset(2f));
    }

    Coroutine setCellsAfterResetCoroutine;

    IEnumerator SetShotCellsAfterReset(float delay)
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < cellsShotQueue.Count; i++)
        {
            ShootCell((int) cellsShotQueue[i].x, (int) cellsShotQueue[i].y, false);
        }
        isInResetAnim = false;
    }

    private bool delayShootCell = false;

    IEnumerator DelayShootCell(float duration)
    {
        delayShootCell = true;
        yield return new WaitForSeconds(duration);
        delayShootCell = false;
    }

    Coroutine delayShootCellCoroutine;

    public void TryShootCell(int x, int y)
    {
        if (delayShootCell || isInResetAnim)
            return;

        if (PConsumableType.BERI.GetValue() < GameData.JoinTreasureRoom.ShotCost)
        {
            return;
        }
        PConsumableType.BERI.AddValue(-GameData.JoinTreasureRoom.ShotCost);
        delayShootCellCoroutine = StartCoroutine(DelayShootCell(1));
        Debug.Log($"shooting cell {x} {y}");
        WSClient.RequestShootTreasure(x, y);
    }

    public void OnCellShot(JSONNode node)
    {
        int status = int.Parse(node["s"]);
        int y = int.Parse(node["x"]);       // x from server is y in game
        int x = int.Parse(node["y"]);       // y from server is x in game
        var c = new Vector2(x, y);

        if (status == -2)
        {
            if (cells.ContainsKey(c))
            {
                cells[c].SetIsShot(true);
            }
        }
        if (status >= 0 && status <= 2)
        {
            if (isInResetAnim)
                cellsShotQueue.Add(c);
            else
                ShootCell(x, y, status == 2);
        }
            
        if (status == 2 || status == 1)
            ResetCells();
        if (status == 2)
        {
            string name = node["name"];
            int userId = int.Parse(node["userId"]);
            string beri = node["beri"];
            message.text = $"{name} hit the treasure ship and get {beri} beri.\n" + message.text;
            if (message.text.Length > 400)
                message.text = message.text.Substring(0, 400);
            UpdatePrize(GameData.JoinTreasureRoom.InitPrize);
            if (treasureTransform && PDataAuth.AuthData.userId == userId)
            {
                PlayTreasureGetAnim(x, y, int.Parse(node["beri"]));
            }
        } else if (status == 0 || status == 1)
        {
            UpdatePrize(GameData.JoinTreasureRoom.CurrentPrize + GameData.JoinTreasureRoom.ShotCost);
        }
    }

    public void PlayTreasureGetAnim(int cellX, int cellY, int prizeAmount)
    {
        var c = new Vector2(cellX, cellY);
        if (cells.ContainsKey(c))
        {
             
        }

        if (treasureTransform != null && resourceUI != null)
        {
            CoinVFX2.CreateCoinFx(resourceUI, treasureTransform.position, CoinVFX2.VFXState.JACKPOT);
        }

        StartCoroutine(DelayAddBeri(prizeAmount));
    }

    IEnumerator DelayAddBeri(int prizeAmount)
    {
        yield return new WaitForSeconds(2f);
        PConsumableType.BERI.AddValue(prizeAmount);
    }

    public void TestFx()
    {
        if (treasureTransform != null && resourceUI != null)
        {
            CoinVFX2.CreateCoinFx(resourceUI, treasureTransform.position, CoinVFX2.VFXState.JACKPOT);
        }
    }

    public void ExitRoom()
    {
        WSClient.RequestExitTreasureRoom();
        GetComponent<PopupBehaviour>().ForceClose();
    }

    public void OnToggleAutoFill(bool on)
    {
        autoFillMode = on;
    }
}
