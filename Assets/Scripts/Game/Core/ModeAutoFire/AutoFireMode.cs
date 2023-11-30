using Framework;
using UnityEngine;

public class AutoFireMode : MonoBehaviour
{
    private int[] D_COLUMN = new int[] { 1, -1, 0, 0 };
    private int[] D_ROW = new int[] { 0, 0, 1, -1 };

    private int boardWidth = 10;
    private int boardHeight = 10;

    public void Fire()
    {
        Vector2Int? firePoint = GetFirePointNext();
        if (firePoint != null)
        {
            WSClientHandler.AttackOpponent(CoreGame.roomId, firePoint.Value.x, firePoint.Value.y);
        }
        else
        {
            
        }

    }

    public Vector2Int? GetFirePointNext()
    {
        if (CoreGame.Instance.curDirHitList.Count == 0)
        {
            Vector2Int? randomPoint = CoreGame.Instance.opponent.remainOctiles.GetRandom();
            return randomPoint;
        }
        else if (CoreGame.Instance.curDirHitList.Count == 1)
        {
            Vector2Int center = CoreGame.Instance.curDirHitList[0];
            return FindNear(center).Value;
        }
        else if (CoreGame.Instance.curDirHitList.Count >= 2)
        {
            Vector2Int point1 = CoreGame.Instance.curDirHitList[0];
            Vector2Int point2 = CoreGame.Instance.curDirHitList[1];

            if (point1.x == point2.x)
            {
                return FindPointWithCollum(point1);
            }
            else
            {
                return FindPointWithRow(point1);
            }
        }
        return null;
    }

    private Vector2Int? FindPointWithRow(Vector2Int point1)
    {
        int random = Random.Range(0, 2);
        if (random == 0)
        {
            Vector2Int? result = FindPointLeft(point1);
            if (result == null)
            {
                result = FindPointRight(point1);
            }
            return result;
        }
        else
        {
            Vector2Int? result = FindPointRight(point1);
            if (result == null)
            {
                result = FindPointLeft(point1);
            }
            return result;
        }
    }

    private Vector2Int? FindPointWithCollum(Vector2Int point1)
    {
        int random = Random.Range(0, 2);
        if (random == 0)
        {
            Vector2Int? result = FindPointDown(point1);
            if (result == null)
            {
                result = FindPointUp(point1);
            }
            return result;
        }
        else
        {
            Vector2Int? result = FindPointUp(point1);
            if (result == null)
            {
                result = FindPointDown(point1);
            }
            return result;
        }
    }

    private Vector2Int? FindPointLeft(Vector2Int point1)
    {
        for (int i = point1.x - 1; i >= 0; i--)
        {
            int x = i;
            int y = point1.y;

            if(IsInBoard(x, y))
            {
                if (!IsAttacked(x, y))
                {
                    return new Vector2Int(x, y);
                }
                if (IsBlock(x, y))
                {
                    return null;
                }
            }
        }

        return null;
    }

    private Vector2Int? FindPointRight(Vector2Int point1)
    {
        for (int i = point1.x + 1; i < boardHeight; i++)
        {
            int x = i;
            int y = point1.y;

            if (IsInBoard(x, y))
            {
                if (!IsAttacked(x, y))
                {
                    return new Vector2Int(x, y);
                }
                if (IsBlock(x, y))
                {
                    return null;
                }
            }
        }

        return null;
    }

    private Vector2Int? FindPointUp(Vector2Int point1)
    {
        for (int i = point1.y + 1; i < boardWidth; i++)
        {
            int x = point1.x;
            int y = i;

            if (IsInBoard(x, y))
            {
                if (!IsAttacked(x, y))
                {
                    return new Vector2Int(x, y);
                }
                if (IsBlock(x, y))
                {
                    return null;
                }
            }
        }

        return null;
    }

    private Vector2Int? FindPointDown(Vector2Int point1)
    {
        for (int i = point1.y - 1; i >= 0; i--)
        {
            int x = point1.x;
            int y = i;

            if (IsInBoard(x, y))
            {
                if(!IsAttacked(x, y))
                {
                    return new Vector2Int(x, y);
                }
                if (IsBlock(x, y))
                {
                    return null;
                }
            }
        }

        return null;
    }

    private Vector2Int? FindNear(Vector2Int center)
    {
        int random = Random.Range(0, D_COLUMN.Length);
        for (int i = random; i < random + D_COLUMN.Length; i++)
        {
            int index = i % D_COLUMN.Length;
            int x = center.x + D_COLUMN[index];
            int y = center.y + D_ROW[index];

            if (IsInBoard(x, y) && !IsAttacked(x, y))
            {
                return new Vector2Int(x, y);
            }
        }

        return null;
    }

    private bool IsAttacked(int x, int y)
    {
        return CoreGame.Instance.opponent.octiles[y][x].Attacked;
    }

    private bool IsBlock(int x, int y)
    {
        //return !CoreGame.Instance.opponent.remainOctiles.Contains(new Vector2Int(y, x));
        return CoreGame.Instance.opponent.octiles[y][x].Attacked && !CoreGame.Instance.curDirHitList.Contains(new Vector2Int(x, y));
    }

    private bool IsInBoard(int x, int y)
    {
        if(x < 0 || y < 0 || x >= boardWidth || y >= boardHeight)
            return false;
        return true;
    }
}
