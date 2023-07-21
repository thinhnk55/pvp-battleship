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
using Lean.Touch;

public class TreasureHuntManager : SingletonMono<TreasureHuntManager>
{
    [SerializeField] TreasureBoardCell cellPrefab;
    [SerializeField] Transform treasureGrid;
    [SerializeField] Transform resourceUI;
    [SerializeField] GameObject blockInteract;
    [SerializeField] TextMeshProUGUI message;
    [SerializeField] List<GameObject> treasureDigits;
    [SerializeField] Transform treasureTransform;
    [SerializeField] Toggle autoFillToggle;
    [SerializeField] GameObject horizontalLine;
    [SerializeField] GameObject verticalLine;
    [SerializeField] RectTransform touchPointRelativeToBoard;

    Dictionary<Vector2, TreasureBoardCell> cells = new Dictionary<Vector2, TreasureBoardCell>();
    private bool autoFillMode;
    List<Vector2> cellsShotQueue = new List<Vector2>();
    private bool isInResetAnim;

    private void OnEnable()
    {
        ServerMessenger.AddListener<JSONNode>(ServerResponse.RECIEVE_TREASURE_SHOT, OnCellShot);
        message.text = "";
        delayShootCell = false;
        LeanTouch.OnFingerUpdate += Instance.SelectingTarget;
        LeanTouch.OnFingerUp += Instance.OnFingerUp;
    }

    private void OnDisable()
    {
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse.RECIEVE_TREASURE_SHOT, OnCellShot);
        LeanTouch.OnFingerUp -= Instance.OnFingerUp;
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

    IEnumerator ResetCells()
    {
        yield return new WaitForSeconds(.5f);
        isInResetAnim = true;
        cellsShotQueue.Clear();
        //for (int i = 0; i < cells.Count; i++)
        //{
        //    cells.ElementAt(i).Value.ResetCell();
        //}
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                cells[new Vector2(i, j)].ResetCell();
            }
            yield return new WaitForSeconds(.15f);
        }
        if (setCellsAfterResetCoroutine != null) StopCoroutine(setCellsAfterResetCoroutine);
        setCellsAfterResetCoroutine = StartCoroutine(SetShotCellsAfterReset(4f));
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
        if (delayShootCell || isInResetAnim || !cells.ContainsKey(new Vector2(x, y)) || cells[new Vector2(x, y)].IsShot)
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
            StartCoroutine(ResetCells());
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

    public void SelectingTarget(LeanFinger leanFinger)
    {
        if (touchPointRelativeToBoard == null) return;
        var grid = treasureGrid.GetComponent<GridLayoutGroup>();
        touchPointRelativeToBoard.position = new Vector2(leanFinger.GetLastWorldPosition(0).x, leanFinger.GetLastWorldPosition(0).y);
        int x = (int)(touchPointRelativeToBoard.anchoredPosition.x / (grid.cellSize.x + grid.spacing.x));
        int y = (int)(touchPointRelativeToBoard.anchoredPosition.y / (grid.cellSize.y + grid.spacing.y));

        if (touchPointRelativeToBoard.anchoredPosition.x < 0 || touchPointRelativeToBoard.anchoredPosition.y < 0)
            x = -1;
        ShowTargeting(x, y);
    }

    public void OnFingerUp(LeanFinger leanFinger)
    {
        HideTargeting();
        if (touchPointRelativeToBoard == null) return;
        var grid = treasureGrid.GetComponent<GridLayoutGroup>();
        touchPointRelativeToBoard.position = new Vector2(leanFinger.GetLastWorldPosition(0).x, leanFinger.GetLastWorldPosition(0).y);
        int x = (int)(touchPointRelativeToBoard.anchoredPosition.x / (grid.cellSize.x + grid.spacing.x));
        int y = (int)(touchPointRelativeToBoard.anchoredPosition.y / (grid.cellSize.y + grid.spacing.y));

        if (touchPointRelativeToBoard.anchoredPosition.x < 0 || touchPointRelativeToBoard.anchoredPosition.y < 0)
            x = -1;
        TryShootCell(x, y);
    }

    public void ShowTargeting(int x, int y)
    {
        if (delayShootCell || isInResetAnim || !cells.ContainsKey(new Vector2(x, y)) || horizontalLine == null || verticalLine == null)
        {
            HideTargeting();
            return;
        }

        var c = cells[new Vector2(x, y)];
        horizontalLine.SetActive(true);
        horizontalLine.transform.position = new Vector2(horizontalLine.transform.position.x, c.transform.position.y);
        verticalLine.SetActive(true);
        verticalLine.transform.position = new Vector2(c.transform.position.x, verticalLine.transform.position.y);
    }

    public void HideTargeting()
    {
        if (horizontalLine == null || verticalLine == null)
            return;
        horizontalLine.SetActive(false);
        verticalLine.SetActive(false);
    }
}
