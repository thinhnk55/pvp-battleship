using DG.Tweening;
using Framework;
using TMPro;
using UnityEngine;

public class Octile : CacheMonoBehaviour
{
    public static float timeAttackAnim = 0.625f;
    public TextMeshPro textOccupied;
    [SerializeField] private int occupied; public int Occupied
    {
        get { return occupied; }
        set
        {
            occupied = value;
            if (occupied < 0)
                occupied = 0;
            textOccupied.text = occupied.ToString();
            if (occupied > 0)
            {
                //spriteRenderer.sprite = SpriteFactory.Occupied;
            }
            else
            {
                //spriteRenderer.sprite = SpriteFactory.Octile;
            }
        }
    }
    [SerializeField] private bool attacked; public bool Attacked
    {
        get { return attacked; }
        set
        {
            attacked = value;
            if (attacked)
            {
            }
            else
            {
                attackSpriteRenderer.sprite = SpriteFactory.Octile;
            }
        }
    }
    public Vector2Int pos;
    public Ship ship;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer attackSpriteRenderer;
    public GameObject attackedAnim;
    public bool BeingAttacked(bool hitShip)
    {
        Attacked = true;
        ObjectPoolManager.SpawnObject<ParticleSystem>(VFXFactory.SplashWater, Position);
        if (hitShip)
        {
            attackedAnim = ObjectPoolManager.SpawnObject<Transform>(VFXFactory.AttackedAnim, Position, transform).gameObject;
            ObjectPoolManager.SpawnObject<ParticleSystem>(VFXFactory.Explosion, Position);
            SoundType.SHIP_HIT.PlaySound();
            if (ship)
                ship.BeingAttacked(this);
            return true;
        }
        else
        {
            DOVirtual.DelayedCall(0.5f, () => spriteRenderer.sprite = SpriteFactory.Missed, true);
            SoundType.SHIP_MISS.PlaySound();
            return false;
        }
    }
    public static bool Check(Board board, int x, int y, out int _x, out int _y)
    {
        _x = x;
        _y = y;
        bool inside = false;

        inside = true;
        if (x < 0 || x >= board.column)
        {
            inside = false;
        }

        if (y < 0 || y >= board.row)
        {
            inside = false;
        }

        _x = x;
        _y = y;
        if (!inside)
            return false;

        if (board && board.octiles[y][x].attacked)
        {
            return false;
        }
        return true;
    }
}
