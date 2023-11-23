using Framework;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class AutoFireMode
{
    private static int[] D_COLUMN = new int[] { 1, -1, 0, 0 };
    private static int[] D_ROW = new int[] { 0, 0, 1, -1 };

    private static int boardWidth = 10;
    private static int boardHeight = 10;


    public static Vector2Int? GetFirePointNext()
    {
        if(CoreGame.Instance.curDirHitList.Count == 0) 
        {
            return CoreGame.Instance.opponent.remainOctiles.GetRandom();
        }
        else if(CoreGame.Instance.curDirHitList.Count == 1)
        {
            Vector2Int center = CoreGame.Instance.curDirHitList[0];
            return FindNear(center).Value;
        }
        else if (CoreGame.Instance.curDirHitList.Count >= 2)
        {
            Vector2Int point1 = CoreGame.Instance.curDirHitList[0];
            Vector2Int point2 = CoreGame.Instance.curDirHitList[1];

            if(point1.x == point2.x)
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

    private static Vector2Int? FindPointWithCollum(Vector2Int point1)
    {
        int random = Random.Range(0, 2);
        if(random == 0)
        {
            Vector2Int? result = FindPointLeft(point1);
            if(result == null)
                FindPointRight(point1);
        }
        else
        {
            Vector2Int? result = FindPointUp(point1);
            if (result == null)
                FindPointDown(point1);
        }
        return null;
    }

    private static void FindPointDown(Vector2Int point1)
    {
        for(int i = point1.x-1; i>=0; i--)
        {
            int x = point1.x;
            int y = i;

            if (Octile.Check(CoreGame.Instance.opponent, x, y, out int _x, out int _y)
            && !CoreGame.Instance.opponent.octiles[x][y].Attacked)
            {
                //return new Vector2Int(x, y);
            }
        }
    }

    private static Vector2Int? FindPointUp(Vector2Int point1)
    {
        throw new System.NotImplementedException();
    }

    private static void FindPointRight(Vector2Int point1)
    {
        throw new System.NotImplementedException();
    }

    private static Vector2Int? FindPointLeft(Vector2Int point1)
    {
        throw new System.NotImplementedException();
    }

    private static Vector2Int FindPointWithRow(Vector2Int point1)
    {
        throw new System.NotImplementedException();
    }

    private static Vector2Int? FindNear(Vector2Int center)
    {
        int random = Random.Range(0, D_COLUMN.Length);
        for (int i=random; i<random + D_COLUMN.Length; i++)
        {
            int index = i % D_COLUMN.Length;
            int x = center.x + D_COLUMN[index];
            int y = center.y + D_ROW[index];

            if (Octile.Check(CoreGame.Instance.opponent, x, y, out int _x, out int _y) 
                && !CoreGame.Instance.opponent.octiles[x][y].Attacked)
            {
                return new Vector2Int(x, y);
            }
        }

        return null;
    }
}
