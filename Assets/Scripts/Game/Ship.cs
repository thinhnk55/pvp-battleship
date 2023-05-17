using Framework;
using Lean.Common;
using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class Ship : CacheMonoBehaviour
{
    public Board board;
    public SpriteRenderer renderer;
    [SerializeField] SpriteRenderer occupyRenderer;
    LeanDragTranslate leanDrag;
    public bool onSelecting;
    public Vector3 previousPos;
    public List<Octile> octilesOccupy;
    public List<Octile> octilesComposition;
    public List<Vector2Int> poses;
    public List<Vector2Int> currentPoses
    {
        get
        {
            List<Vector2Int> curPoses= new List<Vector2Int>(poses);
            if (dir.x !=0)
            {
                for (int i = 0; i < poses.Count; i++)
                {
                    curPoses[i] = poses[i] * dir.x;
                }
            }
            else
            {
                for (int i = 0; i < poses.Count; i++)
                {
                    curPoses[i] = new Vector2Int(poses[i].y * dir.y, poses[i].x * dir.y);
                }
            }
            return curPoses;
        }
    }

    public bool isDestroyed;
    public int leftMost
    {
        get
        {
            List<Vector2Int> curPoses = currentPoses;
            int most = curPoses[0].x;
            for (int i = 0; i < curPoses.Count; i++)
            {
                if (curPoses[i].x< most)
                    most = curPoses[i].x;   
            }
            return most;
        }
    }
    public int rightMost
    {
        get
        {
            List<Vector2Int> curPoses = currentPoses;
            int most = curPoses[0].x;
            for (int i = 0; i < curPoses.Count; i++)
            {
                if (curPoses[i].x > most)
                    most = curPoses[i].x;
            }
            return most;
        }
    }
    public int upMost
    {
        get
        {
            List<Vector2Int> curPoses = currentPoses;
            int most = curPoses[0].y;
            for (int i = 0; i < curPoses.Count; i++)
            {
                if (curPoses[i].y > most)
                    most = curPoses[i].y;
            }
            return most;
        }
    }
    public int downMost
    {
        get
        {
            List<Vector2Int> curPoses = currentPoses;
            int most = curPoses[0].y;
            for (int i = 0; i < curPoses.Count; i++)
            {
                if (curPoses[i].y < most)
                    most = curPoses[i].y;
            }
            return most;
        }
    }
    [SerializeField] Vector2Int dir;  public Vector2Int Dir { 
        get {
            int rot = (int)((EulerAngles.z % 360) / 90);
            switch (rot)
            {
                case 0:
                    return Vector2Int.left;
                case 3:
                    return Vector2Int.up;
                case 2:
                    return Vector2Int.right;
                case 1:
                    return Vector2Int.down;
                default:
                    return Vector2Int.left;
            }
        } set {
            dir = value;
            if (value == Vector2Int.left)
            {
                Vector3 angle = Quaternion.identity.eulerAngles; 
                angle.z = 0;
                transform.rotation = Quaternion.Euler(angle);
            }
            else if (value == Vector2Int.up)
            {
                Vector3 angle = Quaternion.identity.eulerAngles;
                angle.z = 270   ;
                transform.rotation = Quaternion.Euler(angle);
            }
            else if (value == Vector2Int.right)
            {
                Vector3 angle = Quaternion.identity.eulerAngles;
                angle.z = 180;
                transform.rotation = Quaternion.Euler(angle);
            }
            else if (value == Vector2Int.down)
            {
                Vector3 angle = Quaternion.identity.eulerAngles;
                angle.z = 90;
                transform.rotation = Quaternion.Euler(angle);
            }
        }
    }

    private void Start()
    {
        leanDrag = GetComponent<LeanDragTranslate>();
        previousPos = transform.position;
        
    }

    private void Update()
    {

        if (onSelecting)
        {
            int x = (int)((LeanTouch.Fingers[0].GetLastWorldPosition(0).x - CoreGame.Instance.player.transform.position.x + CoreGame.Instance.player.width / 2) / CoreGame.Instance.player.cellWidth);
            int y = (int)((LeanTouch.Fingers[0].GetLastWorldPosition(0).y - CoreGame.Instance.player.transform.position.y + CoreGame.Instance.player.height / 2) / CoreGame.Instance.player.cellHieght);
            CheckShip(CoreGame.Instance.player, x, y, out int _x, out int _y, out bool inside);
        }
    }
    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }
    public void BeingAttacked(Octile octile)
    {
        for (int i = 0; i < octilesComposition.Count; i++)
        {
            if (!octilesComposition[i].Attacked)
            {
                return ;
            }
        }
        BeingDestroyed();
    }

    public void BeingDestroyed()
    {
        isDestroyed = true;
        board.ships.Remove(this);
        Debug.Log("Destroyed");
        for (int i = 0; i < octilesOccupy.Count; i++)
        {
            octilesOccupy[i].Attacked = true;
            octilesOccupy[i].spriteRenderer.sprite = SpriteFactory.Occupied;
        }
        for (int i = 0; i < octilesComposition.Count; i++)
        {
            octilesComposition[i].attackSpriteRenderer.sprite = SpriteFactory.Destroyed;
        }
    }

    public void OnSelected(LeanSelectByFinger leanSelectByFinger,LeanFinger leanSelect)
    {
        if (CoreGame.Instance.stateMachine.CurrentState != GameState.Pre)
            return;
        onSelecting = true;
        previousPos = transform.position;
        if (octilesOccupy.Count>0)
        {
            List<Vector2Int> curPoses = currentPoses;
            for (int i = 0; i < curPoses.Count; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int posY = octilesOccupy[0].pos.y + curPoses[i].y + j;
                    if (posY >= 0 && posY < CoreGame.Instance.player.row)
                    {
                        for (int k = -1; k < 2; k++)
                        {
                            int posX = octilesOccupy[0].pos.x + curPoses[i].x + k;
                            if (posX >= 0 && posX < CoreGame.Instance.player.column)
                            {
                                CoreGame.Instance.player.octiles[posY][posX].Occupied = 0;
                                CoreGame.Instance.player.octiles[posY][posX].ship = null;
                                CoreGame.Instance.player.ships.Remove(this);
                            }
                        }
                    }

                }
            }
            octilesOccupy.Clear();
        }

        foreach (Ship ship in CoreGame.Instance.player.ships)
        {
            List<Vector2Int> curPoses = ship.currentPoses;
            for (int i = 0; i < curPoses.Count; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int posY = ship.octilesOccupy[0].pos.y + curPoses[i].y + j;
                    if (posY >= 0 && posY < CoreGame.Instance.player.row)
                    {
                        for (int k = -1; k < 2; k++)
                        {
                            int posX = ship.octilesOccupy[0].pos.x + curPoses[i].x + k;
                            if (posX >= 0 && posX < CoreGame.Instance.player.column)
                            {
                                CoreGame.Instance.player.octiles[posY][posX].Occupied = 1;
                                CoreGame.Instance.player.octiles[posY][posX].ship = ship;
                            }
                        }
                    }

                }
            }
        }
    }

    public void CheckAndAssignShip(LeanSelectByFinger leanSelectByFinger, LeanFinger leanSelect)
    {
        if (CoreGame.Instance.stateMachine.CurrentState != GameState.Pre)
            return;
        onSelecting = false;
        int x = (int)((leanSelect.GetLastWorldPosition(0).x - CoreGame.Instance.player.transform.position.x + CoreGame.Instance.player.width / 2) / CoreGame.Instance.player.cellWidth);
        int y = (int)((leanSelect.GetLastWorldPosition(0).y - CoreGame.Instance.player.transform.position.y + CoreGame.Instance.player.height / 2) / CoreGame.Instance.player.cellHieght);
        CheckAndAssignShip(CoreGame.Instance.player, x, y, true);
    }
    public bool CheckAndAssignShip(Board board,int x, int y, bool assignBackup)
    {
        if (CoreGame.Instance.stateMachine.CurrentState != GameState.Pre && CoreGame.Instance.stateMachine.CurrentState != GameState.Search)
            return false;
        if (CheckShip(board, x, y, out int _x, out int _y, out bool inside))
        {
            board.AssignShip(this, _x, _y);
            occupyRenderer.enabled = false;
            transform.parent = board.shipRoot.transform;
            Position = board.octiles[_y][_x].Position;
            return true;
        }
        else
        {
            Position = previousPos;
            _x = (int)((Position.x - CoreGame.Instance.player.transform.position.x + CoreGame.Instance.player.width / 2) / CoreGame.Instance.player.cellWidth);
            _y = (int)((Position.y - CoreGame.Instance.player.transform.position.y + CoreGame.Instance.player.height / 2) / CoreGame.Instance.player.cellHieght);
            if (CheckShip(board, _x, _y, out _x, out _y, out inside) && inside || assignBackup)
            {
                board.AssignShip(this, _x, _y);
            }
            occupyRenderer.enabled = false;
            return false;
        }

    }
    public bool CheckShip(Board board, int x, int y, out int _x, out int _y, out bool inside)
    {
        _x = x;
        _y = y;
        inside = false;
        if (CoreGame.Instance.stateMachine.CurrentState != GameState.Pre && CoreGame.Instance.stateMachine.CurrentState != GameState.Search)
            return false;
        inside = true;
        if (x + leftMost < 0)
        {
            x = (-leftMost);
            inside = false;
        }
        if (x + rightMost >= board.column)
        {
            x = (board.column - 1 - rightMost);
            inside = false;
        }
        if (y + downMost < 0)
        {
            y = (-downMost);
            inside = false;
        }
        if (y + upMost >= board.row)
        {
            y = (board.row - 1 - upMost);
            inside = false;
        }
        if (inside)
        {
            Position = board.octiles[y][x].Position;
            if (leanDrag)
            {
                leanDrag.enabled = false;
                leanDrag = null;
            }
        }
        occupyRenderer.enabled = true;
        _x = x;
        _y = y;
        if (board.CheckUnoccupied(this, x, y))
        {
            occupyRenderer.sprite = SpriteFactory.Occupied;
            return true;
        }
        occupyRenderer.sprite = SpriteFactory.Unoccupiable;
        return false;
    }
    public void RotateShip(LeanFinger leanFinger)
    {
        if (CoreGame.Instance.stateMachine.CurrentState != GameState.Pre)
            return;
        if (leanFinger.Age<0.1f)
        {
            //Debug.Log("Tap");
            if (Dir == Vector2Int.left)
            {
                Dir = Vector2Int.up;
            }
            else if (Dir == Vector2Int.up)
            {
                Dir = Vector2Int.right;
            }
            else if (Dir == Vector2Int.right)
            {
                Dir = Vector2Int.down;
            }
            else if (Dir == Vector2Int.down)
            {
                Dir = Vector2Int.left;
            }
        }
        if (octilesOccupy.Count>0)
        {
            OnSelected(null,null);
            CheckAndAssignShip(CoreGame.Instance.player ,octilesOccupy[0].pos.x, octilesOccupy[0].pos.y, true);
        }
    }

}
