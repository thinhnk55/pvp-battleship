using DG.Tweening;
using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : CacheMonoBehaviour
{
    Camera cam;
    int dir;
    Vector3 des;
    private void OnEnable()
    {
        cam = Camera.main;
    }
    public void Init(Vector3 pos)
    {
        dir = Random.Range(0, 4);
        Debug.Log(dir);
        float yMax = cam.ScreenToWorldPoint(new Vector3(0, Screen.height + 2, 0)).y;
        float xMax = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x; 
        switch (dir)
        {
            case 0:
                Position = new Vector3(- xMax, pos.y, 0);
                des = new Vector3(xMax, pos.y, 0);
                break;
            case 1:
                Position = new Vector3(pos.x, yMax, 0);
                des = new Vector3(pos.x, -yMax, 0);
                break;
            case 2:
                Position= new Vector3(xMax, pos.y, 0);
                des = new Vector3(-xMax, pos.y, 0);
                break;
            case 3:
                Position = new Vector3(pos.x, -yMax, 0);
                des = new Vector3(pos.x, yMax, 0);
                break;
            default:
                break;
        }
        transform.DOMove(des, Octile.timeAttackAnim).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
        DOVirtual.DelayedCall((pos - Position).magnitude / (des - Position).magnitude - 0.2f, ()=> ObjectPoolManager.GenerateObject<Transform>(PrefabFactory.Missle, Position));
    }
}
