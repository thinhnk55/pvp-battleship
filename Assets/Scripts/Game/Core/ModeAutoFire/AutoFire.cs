using Framework;
using UnityEngine;

public class AutoFire : SingletonMono<AutoFire>
{
    private int[] D_COLUMN = new int[] { 1, -1, 0, 0 };
    private int[] D_ROW = new int[] { 0, 0, 1, -1 };

    private int boardWidth = 10;
    private int boardHeight = 10;
    // Start is called before the first frame update
    void Start()
    {

    }

    public Vector2Int? GetFirePointNext()
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

    private Vector2Int? FindPointWithCollum(Vector2Int point1)
    {
        throw new System.NotImplementedException();
    }

    private Vector2Int FindPointWithRow(Vector2Int point1)
    {
        throw new System.NotImplementedException();
    }

    private Vector2Int? FindNear(Vector2Int center)
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
