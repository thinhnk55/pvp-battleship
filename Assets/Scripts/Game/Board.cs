using Framework;
using Lean.Common;
using Lean.Touch;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Board : CacheMonoBehaviour
{
    public int row, column;
    public float width, height, cellWidth, cellHieght;

    public List<Ship> ships;
    public List<List<Octile>> octiles;
    [SerializeField] GameObject octileRoot;
    [SerializeField] public GameObject shipRoot;
    Camera cam;
    private void Awake()
    {
        Debug.Log(0);
        if (octiles.IsNullOrEmpty())
        {
            InitBoard(10, 10);
        }
        Debug.Log(1);
    }
    void Start()
    {
    }
    public void InitBoard(int row, int column)
    {
        octiles = new List<List<Octile>>();
        cam = Camera.main;
        this.row = row;
        this.column = column;
        float size = cam.orthographicSize * 1.2f;
        width = height = size;
        transform.localScale = new Vector3(size,size);
        cellHieght = height / row;
        cellWidth = width / column;

        for (int iRow = 0; iRow < row; iRow++)
        {
            octiles.Add(new List<Octile>());
            for (int iCol = 0; iCol < column; iCol++)
            {
                Octile octile = Instantiate(PrefabFactory.Octile, new Vector3(Position.x - width/2 + cellWidth * (iCol + 0.5f) , Position.y - height / 2 + cellHieght * (iRow + 0.5f)), Quaternion.identity, octileRoot.transform).GetComponent<Octile>();
                octile.name = iRow.ToString() + "_" + iCol.ToString();
                octile.pos = new Vector2Int(iCol, iRow);
                octiles[iRow].Add(octile);
            }
        }
    }
    public void BeingAttacked(LeanFinger leanFinger)
    {
        int x = (int)((leanFinger.GetLastWorldPosition(0).x - transform.position.x + width / 2) / cellWidth);
        int y = (int)((leanFinger.GetLastWorldPosition(0).y - transform.position.y + height / 2) / cellHieght);
        if (Octile.Check(this, x, y , out int _x, out int _y) && !octiles[_y][_x].Attacked)
        {
            Messenger.Broadcast(GameEvent.Attack, BeingAttacked(_x, _y));
        }
    }
    public bool BeingAttacked(int x, int y)
    {
        return octiles[y][x].BeingAttacked();
    }

    public void AssignShip(Ship ship,int x,int y)
    {
        List<Vector2Int> curPoses = ship.currentPoses;
        // unassign old pos
        ship.octilesOccupy.Clear();
        ship.octilesComposition.Clear();
        ship.board = this;
        // assign new pos
        ship.octilesOccupy.Add(octiles[y][x]);
        for (int i = 0; i < curPoses.Count; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int posY = y + curPoses[i].y + j;
                if (posY >= 0 && posY < row)
                {
                    for (int k = -1; k < 2; k++)
                    {
                        int posX = x + curPoses[i].x + k;
                        if (posX >= 0 && posX < column)
                        {
                            octiles[posY][posX].Occupied = 1;
                            if (posY != y || posX != x)
                            {
                                if (!ship.octilesOccupy.Contains(octiles[posY][posX]))
                                {
                                    ship.octilesOccupy.Add(octiles[posY][posX]);
                                }
                            }
                        }

                        if (j ==0 && k == 0)
                        {
                            octiles[posY][posX].ship = ship;
                            ship.octilesComposition.Add(octiles[posY][posX]);
                        }
                    }
                }
            }
        }

        ships.Add(ship);
        ship.GetComponent<LeanSelectableByFinger>().Deselect();
    }
    public void RandomShip(List<Ship> ships)
    {
        foreach (var ship in this.ships)
        {
            ship.octilesOccupy.Clear();
        }
        foreach (var octileRow in octiles)
        {
            foreach (var octile in octileRow)
            {
                octile.Occupied = 0;
                octile.ship = null;
            }
        }
        this.ships.Clear();
        int x, y;
        foreach (var ship in ships)
        {
            int dir = Random.Range(0, 4);
            switch (dir)
            {
                case 0:
                    ship.Dir = Vector2Int.left;
                    break;
                case 1:
                    ship.Dir = Vector2Int.up;
                    break;
                case 2:
                    ship.Dir = Vector2Int.right;
                    break;
                case 3:
                    ship.Dir = Vector2Int.down;
                    break;
                default:
                    break;
            }
            x = Random.Range(0, column);
            y = Random.Range(0, row);
            int i = 0;
            while (!ship.CheckAndAssignShip(this, x, y, false) && i < 20)
            {
                dir = Random.Range(0, 4);
                switch (dir)
                {
                    case 0:
                        ship.Dir = Vector2Int.left;
                        break;
                    case 1:
                        ship.Dir = Vector2Int.up;
                        break;
                    case 2:
                        ship.Dir = Vector2Int.right;
                        break;
                    case 3:
                        ship.Dir = Vector2Int.down;
                        break;
                    default:
                        break;
                }
                i++;
                x = Random.Range(0, column);
                y = Random.Range(0, row);
                if (i == 20)
                {
                    RandomShip(ships);
                    break;
                }
            }
            if (i == 20)
            {
                break;
            }

        }
    }
    public bool CheckUnoccupied(Ship ship, int x, int y)
    {
        for (int i = 0; i < ship.currentPoses.Count; i++)
        {
            List<Vector2Int> shipPoses = ship.currentPoses;
            if (octiles[shipPoses[i].y + y][shipPoses[i].x + x].Occupied > 0)
            {
                return false;
            }
        }
        return true;
    }
}
