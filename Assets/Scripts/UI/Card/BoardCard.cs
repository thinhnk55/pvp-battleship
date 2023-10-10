using Framework;
using System.Collections.Generic;
//using Unity.Android.Types;
using UnityEngine;
using UnityEngine.UI;

public struct ShipInfo
{
    public int type;
    public int dir;
    public int x;
    public int y;

    public static ShipInfo FromShip(Ship ship)
    {
        ShipInfo info = new ShipInfo();
        info.x = ship.octilesComposition[0].pos.x;
        info.y = ship.octilesComposition[0].pos.y;
        info.type = ship.octilesComposition.Count - 1;
        //Debug.LogError(info.x + " ; " + info.y + "=>>>>" + info.type);
        int d = 0;
        if (ship.Dir == Vector2Int.right)
        {
            d = 2;
        }
        else if (ship.Dir == Vector2Int.left)
        {
            d = 0;
        }
        else if (ship.Dir == Vector2Int.down)
        {
            d = 3;
        }
        else if (ship.Dir == Vector2Int.up)
        {
            d = 1;
        }
        info.dir = d;
        return info;
    }
}
public struct BoardInfo
{
    public int Id;
    public int row, column;
    public List<ShipInfo> boardInfo;
}
public class BoardCard : CardBase<BoardInfo>
{
    public List<List<Octile>> octiles;
    public List<GameObject> ships;
    [SerializeField] Transform rootOctiles;
    [SerializeField] Transform rootShips;
    RectTransform rect;
    [SerializeField] Button DeleteBtn;
    [SerializeField] GameObject Board;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    protected override void OnClicked(BoardInfo info)
    {
        throw new System.NotImplementedException();
    }

    public override void BuildUI(BoardInfo info)
    {
        base.BuildUI(info);
        Debug.Log(info.Id + "_" + info.boardInfo.Count);
        rootOctiles.DestroyChildrenImmediate();
        rootShips.DestroyChildrenImmediate();
        octiles = new List<List<Octile>>();
        float cellHieght = rect.sizeDelta.y / info.row;
        float cellWidth = rect.sizeDelta.x / info.column;
        for (int iRow = 0; iRow < info.row; iRow++)
        {
            octiles.Add(new List<Octile>());
            for (int iCol = 0; iCol < info.column; iCol++)
            {
                Octile octile = Instantiate(PrefabFactory.OctileUI, rootOctiles).GetComponent<Octile>();
                octile.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-rect.sizeDelta.x / 2 + cellWidth * (iCol + 0.5f), -rect.sizeDelta.y / 2 + cellHieght * (iRow + 0.5f));
                octile.name = iRow.ToString() + "_" + iCol.ToString();
                octile.pos = new Vector2Int(iCol, iRow);
                octiles[iRow].Add(octile);
            }
        }
        foreach (var item in ships)
        {
            DestroyImmediate(item);
        }
        ships = new List<GameObject>();
        for (int i = 0; i < info.boardInfo.Count; i++)
        {
            RenderShip(info.boardInfo[i]);
        }
        // O nay chua luu doi hinh
        if (info.boardInfo.Count == 0)
        {
            Board.SetActive(false);
            //DeleteBtn.gameObject.SetActive(false);
            Button.onClick.RemoveAllListeners();
            if (CoreGame.Instance.player.ships.Count == 10)
            {
                Button.onClick.AddListener(() =>
                {
                    Save();
                });
            }
            else
            {
                Button.onClick.AddListener(() =>
                {
                    PopupHelper.CreateConfirm(PrefabFactory.PopupInvalidFormation, "Invalid Formation", "Please get all your ship in position", null, null);

                });

            }
        }
        else
        {
            Board.SetActive(true);
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(Load);
            DeleteBtn.onClick.RemoveAllListeners();
            DeleteBtn.onClick.AddListener(Delete);
            //DeleteBtn.gameObject.SetActive(true);
        }
    }

    private void Load()
    {
        CoreGame.Instance.player.ships.Clear();
        for (int i = 0; i < Info.boardInfo.Count; i++)
        {
            CoreGame.Instance.shipsPlayer[i].OnSelected(null, null);
            CoreGame.Instance.shipsPlayer[i].Dir = Ship.GetDir(Info.boardInfo[i].dir);
            CoreGame.Instance.player.AssignShip(CoreGame.Instance.shipsPlayer[i], Info.boardInfo[i].x, Info.boardInfo[i].y);
            Collection.GetComponent<PopupBehaviour>().ForceClose();
        }
    }

    public void RenderShip(ShipInfo ship)
    {
        var s = Instantiate(PrefabFactory.ShipsUI[ship.type], rootShips).GetComponent<RectTransform>();
        s.anchoredPosition3D = octiles[ship.y][ship.x].GetComponent<RectTransform>().anchoredPosition3D;
        s.transform.eulerAngles = new Vector3(0, 0, -ship.dir * 90);
    }

    // Luu bang luu doi hinh vao game data
    public void Save()
    {
        Board.SetActive(true);
        BoardInfo info = new BoardInfo();
        info.Id = Info.Id;
        info.row = CoreGame.Instance.player.row;
        info.column = CoreGame.Instance.player.column;
        info.boardInfo = new List<ShipInfo>();
        for (int i = 0; i < CoreGame.Instance.player.ships.Count; i++)
        {
            info.boardInfo.Add(ShipInfo.FromShip(CoreGame.Instance.player.ships[i]));
        }
        info.boardInfo.Sort((ship1, ship2) =>
        {
            if (ship1.type > ship2.type)
            {
                return -1;
            }
            else if (ship1.type < ship2.type)
            {
                return 1;
            }
            else
                return 0;
        });
        List<BoardInfo> list = new List<BoardInfo>();
        for (int i = 0; i < GameData.ListBoard.Count; i++)
        {
            if (info.Id != i)
            {
                list.Add(GameData.ListBoard[i]);
            }
            else
            {
                list.Add(info);
            }
        }
        GameData.ListBoard = list;
        Collection.ModifyUIAt(Info.Id, info);
    }
    public void Delete()
    {
        Board.SetActive(false);
        BoardInfo info = new BoardInfo();
        info.Id = Info.Id;
        info.row = CoreGame.Instance.player.row;
        info.column = CoreGame.Instance.player.column;
        info.boardInfo = new List<ShipInfo>();
        List<BoardInfo> list = new List<BoardInfo>();
        for (int i = 0; i < GameData.ListBoard.Count; i++)
        {
            if (info.Id != i)
            {
                list.Add(GameData.ListBoard[i]);
            }
            else
            {
                list.Add(info);
            }
        }
        GameData.ListBoard = list;
        Collection.ModifyUIAt(Info.Id, info);
    }

}
