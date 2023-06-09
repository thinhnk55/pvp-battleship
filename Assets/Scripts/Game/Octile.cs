using DG.Tweening;
using Framework;
using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Octile : CacheMonoBehaviour
{
    public TextMeshPro textOccupied;
    [SerializeField] private int occupied; public int Occupied
    {
        get { return occupied; }
        set
        {
            occupied = value;
            if (occupied < 0)
                occupied= 0;
            textOccupied.text = occupied.ToString();
            if (occupied>0)
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
                DOVirtual.DelayedCall(1.25f, ()=> spriteRenderer.sprite = SpriteFactory.Occupied, true);
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
    public bool BeingAttacked(bool hitShip)
    {
        Attacked = true;
        ObjectPoolManager.GenerateObject<ParticleSystem>(VFXFactory.SplashWater, Position);
        if (hitShip)
        {
            ObjectPoolManager.GenerateObject<ParticleSystem>(VFXFactory.Explosion, Position);
            attackSpriteRenderer.sprite = SpriteFactory.Attacked;
            SoundType.SHIP_HIT.PlaySound();
            return true;
        }
        else
        {
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
        if (x  < 0 || x >= board.column)
        {
            inside = false;
        }

        if (y  < 0 || y >= board.row)
        {
            inside = false;
        }

        _x = x;
        _y = y;
        if (!inside)
            return false;

        if (board.octiles[y][x].attacked)
        {
            return false;
        }
        return true;
    }
}
