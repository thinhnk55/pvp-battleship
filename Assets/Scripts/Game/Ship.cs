using DG.Tweening;
using Framework;
using Lean.Common;
using Lean.Touch;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class Ship : CacheMonoBehaviour
{
    public Board board;
    public SpriteRenderer renderer;
    [SerializeField] SpriteRenderer occupyRenderer;
    [SerializeField] Sprite destroyedSprite;
    LeanDragTranslate leanDrag;
    public bool onSelecting;
    public float timeSelecting;
    public Vector2Int initRot;
    public Vector3 initPos;
    public Vector3 previousPos;
    public Vector2Int previousIndex;
    public List<Octile> octilesOccupy;
    public List<Octile> octilesComposition;
    Tween tweenMove;
    Tween tweenRotate;
    float tweenTime = 0.5f;
    public bool isDestroyed;
    #region Transform

    public List<Vector2Int> poses;
    public List<Vector2Int> currentPoses
    {
        get
        {
            List<Vector2Int> curPoses = new List<Vector2Int>(poses);
            if (dir.x != 0)
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

    public int leftMost
    {
        get
        {
            List<Vector2Int> curPoses = currentPoses;
            int most = curPoses[0].x;
            for (int i = 0; i < curPoses.Count; i++)
            {
                if (curPoses[i].x < most)
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
    [SerializeField] Vector2Int dir; public Vector2Int Dir
    {
        get
        {
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
        }
        set
        {
            dir = value;
            Vector3 angle = Vector3.zero;
            if (dir == Vector2Int.left)
            {
                angle = Quaternion.identity.eulerAngles;
                angle.z = 0;
                //transform.rotation = Quaternion.Euler(angle);
            }
            else if (dir == Vector2Int.up)
            {
                angle = Quaternion.identity.eulerAngles;
                angle.z = 270;
                //transform.rotation = Quaternion.Euler(angle);
            }
            else if (dir == Vector2Int.right)
            {
                angle = Quaternion.identity.eulerAngles;
                angle.z = 180;
                //transform.rotation = Quaternion.Euler(angle);
            }
            else if (dir == Vector2Int.down)
            {
                angle = Quaternion.identity.eulerAngles;
                angle.z = 90;
                //transform.rotation = Quaternion.Euler(angle);
            }
            tweenRotate.Kill();
            tweenRotate = transform.DORotate(angle, tweenTime);

        }
    }

    #endregion
    private void Start()
    {
        initRot = Dir;
        initPos = Position;   
        leanDrag = GetComponent<LeanDragTranslate>();
        previousPos = Position;
    }

    private void Update()
    {
        if (onSelecting)
        {
            timeSelecting += Time.deltaTime;
            int x = (int)((LeanTouch.Fingers[0].GetLastWorldPosition(0).x - CoreGame.Instance.player.transform.position.x + CoreGame.Instance.player.width / 2) / CoreGame.Instance.player.cellWidth);
            int y = (int)((LeanTouch.Fingers[0].GetLastWorldPosition(0).y - CoreGame.Instance.player.transform.position.y + CoreGame.Instance.player.height / 2) / CoreGame.Instance.player.cellHieght);
            CheckShip(CoreGame.Instance.player, x, y, out int _x, out int _y, out bool inside, timeSelecting > LeanTouch.CurrentTapThreshold);
        }
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
        renderer.sprite = destroyedSprite;
        board.ships.Remove(this);
        Debug.Log("Destroyed");
        SoundType.SHIP_EXPLOSION.PlaySound();
        for (int i = 0; i < octilesOccupy.Count; i++)
        {
            octilesOccupy[i].Attacked = true;
            octilesOccupy[i].spriteRenderer.sprite = SpriteFactory.Occupied;
        }
        for (int i = 0; i < octilesComposition.Count; i++)
        {
            octilesComposition[i].attackSpriteRenderer.sprite = null;
            ObjectPoolManager.GenerateObject<ParticleSystem>(VFXFactory.Explosion, octilesComposition[i].Position);
            if (i==0 || i == 2)
            {
                ObjectPoolManager.GenerateObject<ParticleSystem>(VFXFactory.Smoke, octilesComposition[i].Position, gameObject);
            }
        }
    }

    public void OnSelected(LeanSelectByFinger leanSelectByFinger,LeanFinger leanFinger)
    {
        if (CoreGame.Instance.stateMachine.CurrentState != GameState.Pre)
            return;
        timeSelecting = 0;
        if (octilesOccupy.Count > 0)
            previousIndex = octilesOccupy[0].pos;
        previousPos = Position;
        onSelecting = true;
        Position = leanFinger.GetLastWorldPosition(-CoreGame.Instance.cam.transform.position.z);
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
                            }
                        }
                    }

                }
            }
            octilesOccupy.Clear();
            CoreGame.Instance.player.ships.Remove(this);
        }
        /**/
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

    public void CheckAndAssignShip(LeanSelectByFinger leanSelectByFinger, LeanFinger leanFinger)
    {
        onSelecting = false;
        if (leanFinger.Age < LeanTouch.CurrentTapThreshold)
            return;
        if (CoreGame.Instance.stateMachine.CurrentState != GameState.Pre)
            return;
        int x = (int)((leanFinger.GetLastWorldPosition(0).x - CoreGame.Instance.player.transform.position.x + CoreGame.Instance.player.width / 2) / CoreGame.Instance.player.cellWidth);
        int y = (int)((leanFinger.GetLastWorldPosition(0).y - CoreGame.Instance.player.transform.position.y + CoreGame.Instance.player.height / 2) / CoreGame.Instance.player.cellHieght);
        Debug.Log("fingerup");
        CheckAndAssignShip(CoreGame.Instance.player, x, y, true);
    }
    public bool CheckAndAssignShip(Board board,int x, int y, bool assignBackup, bool isRotate = false)
    {
        if (CoreGame.Instance.stateMachine.CurrentState != GameState.Pre && CoreGame.Instance.stateMachine.CurrentState != GameState.Search)
            return false;
        tweenMove.Kill();
        if (CheckShip(board, x, y, out int _x, out int _y, out bool inside))
        {
            board.AssignShip(this, _x, _y);
            occupyRenderer.enabled = false;
            transform.parent = board.shipRoot.transform;
            tweenMove = transform.DOMove(board.octiles[_y][_x].Position, tweenTime);
            return true;
        }
        else
        {
            occupyRenderer.enabled = false;
            if (!inside && !isRotate)
            {
                transform.parent = CoreGame.Instance.shipListPlayer.transform;
                tweenMove = transform.DOMove(initPos, tweenTime);
                Dir = initRot;
                octilesOccupy.Clear();
                octilesComposition.Clear();
                return false;
            }
            else
            {
                tweenMove = transform.DOMove(initPos, tweenTime);
            }
            if (previousIndex!=null && assignBackup)
            {
                board.AssignShip(this, previousIndex.x, previousIndex.y);
                return false;
            }
            return false;
        }

    }
    public bool CheckShip(Board board, int x, int y, out int _x, out int _y, out bool inside, bool hold = false)
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
            if (hold)
            {
                occupyRenderer.enabled = true;
                Position = board.octiles[_y][_x].Position;
            }
            else
            {
                tweenMove.Kill();
                tweenMove = transform.DOMove(board.octiles[_y][_x].Position, tweenTime);
            }

            if (leanDrag)
            {
                leanDrag.enabled = false;
            }
        }
        else
        {
            if (onSelecting)
            {
                Position = LeanTouch.Fingers[0].GetLastWorldPosition(-CoreGame.Instance.cam.transform.position.z);
            }
            if (leanDrag && !leanDrag.enabled)
            {
                leanDrag.enabled = true;
            }
            return false;
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
        if (leanFinger.Age< LeanTouch.CurrentTapThreshold)
        {
            var direct = Vector2Int.up;
            if (Dir == Vector2Int.left)
            {
                direct = Vector2Int.up;
            }
            else if (Dir == Vector2Int.up)
            {
                direct = Vector2Int.right;
            }
            else if (Dir == Vector2Int.right)
            {
                direct = Vector2Int.down;
            }
            else if (Dir == Vector2Int.down)
            {
                direct = Vector2Int.left;
            }
            Dir = direct;
            int x = previousIndex.x;
            int y = previousIndex.y;
            OnSelected(null, leanFinger);
            if (octilesComposition.Count == 0)
            {

            }
            else if ( !CheckAndAssignShip(CoreGame.Instance.player, x, y, false, true))
            {
                for (int i = 1; i < 10; i++)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        OnSelected(null, leanFinger);

                        if (CheckAndAssignShip(CoreGame.Instance.player, x + i, y + j, false, true))
                        {
                            Debug.Log("Find" + i +"_"+ j);
                            return;
                        }
                        OnSelected(null, leanFinger);

                        if (CheckAndAssignShip(CoreGame.Instance.player, x + j, y + i, false, true))
                        {
                            Debug.Log("Find" + i + "_" + j);

                            return;
                        }
                        OnSelected(null, leanFinger);

                        if (CheckAndAssignShip(CoreGame.Instance.player, x - i, y - j, false, true))
                        {
                            Debug.Log("Find" + i + "_" + j);

                            return;
                        }
                        OnSelected(null, leanFinger);

                        if (CheckAndAssignShip(CoreGame.Instance.player, x - j, y - i, false, true))
                        {
                            Debug.Log("Find" + i + "_" + j);

                            return;
                        }
                        OnSelected(null, leanFinger);

                        if (CheckAndAssignShip(CoreGame.Instance.player, x + i, y - j, false, true))
                        {
                            Debug.Log("Find" + i + "_" + j);

                            return;
                        }
                        OnSelected(null, leanFinger);

                        if (CheckAndAssignShip(CoreGame.Instance.player, x + j, y - i, false, true))
                        {
                            Debug.Log("Find" + i + "_" + j);

                            return;
                        }
                        OnSelected(null, leanFinger);

                        if (CheckAndAssignShip(CoreGame.Instance.player, x - i, y + j, false, true))
                        {
                            Debug.Log("Find" + i + "_" + j);

                            return;
                        }
                        OnSelected(null, leanFinger);

                        if (CheckAndAssignShip(CoreGame.Instance.player, x - j, y + i, false, true))
                        {
                            Debug.Log("Find" + i + "_" + j);

                            return;
                        }
                    }
                }
                OnSelected(null, leanFinger);
                CheckAndAssignShip(CoreGame.Instance.player, x, y, false, true);
                Debug.Log("NotFind");
            }
            else
            {
                Debug.Log("Assigned");
            }
        }

    }

    public JSONClass ToJson() {
        int d = 0;
        int dir = ((int)(EulerAngles.z % 360) / 90);
        switch (dir)
        {
            case 0:
                d = 0;
                break;
            case 1:
                d = 3;
                break;
            case 2:
                d = 2;
                break;
            case 3:
                d = 1;
                break;
            default:
                break;
        }
        JSONClass json = new JSONClass
        {
            { "type", (poses.Count - 1).ToJson()  },
            { "x", octilesComposition[0].pos.x.ToJson()  },
            { "y", octilesComposition[0].pos.y.ToJson()  },
            { "dir", d.ToJson()  }
        };
        return json;
    }

    public Ship FromJson(JSONNode json)
    {
        poses = CoreGame.shipConfigs[int.Parse(json["type"])];
        int dir = int.Parse(json["dir"]);
        switch (dir)
        {
            case 0:
                Dir = Vector2Int.left;
                break;
            case 1:
                Dir = Vector2Int.up;
                break;
            case 2:
                Dir = Vector2Int.right;
                break;
            case 3:
                Dir = Vector2Int.down;
                break;
            default:
                break;
        }
        Position = board.octiles[int.Parse(json["y"])][int.Parse(json["x"])].Position;
        tweenRotate.Kill();
        Vector3 angle = Vector3.zero;
        if (this.dir == Vector2Int.left)
        {
            angle = Quaternion.identity.eulerAngles;
            angle.z = 0;
            //transform.rotation = Quaternion.Euler(angle);
        }
        else if (this.dir == Vector2Int.up)
        {
            angle = Quaternion.identity.eulerAngles;
            angle.z = 270;
            //transform.rotation = Quaternion.Euler(angle);
        }
        else if (this.dir == Vector2Int.right)
        {
            angle = Quaternion.identity.eulerAngles;
            angle.z = 180;
            //transform.rotation = Quaternion.Euler(angle);
        }
        else if (this.dir == Vector2Int.down)
        {
            angle = Quaternion.identity.eulerAngles;
            angle.z = 90;
            //transform.rotation = Quaternion.Euler(angle);
        }
        EulerAngles = angle;
        transform.parent = board.shipRoot.transform;
        board.AssignShip(this, int.Parse(json["x"]), int.Parse(json["y"]));
        return this;
    }
}
