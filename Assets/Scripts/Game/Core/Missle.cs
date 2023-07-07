using DG.Tweening;
using Framework;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Missle : CacheMonoBehaviour
{
    Camera cam;
    int dir;
    Vector3 des;
    [SerializeField] SkeletonAnimation skeletonAnimation;
    private void OnEnable()
    {
        cam = Camera.main;
    }
    public void Init(Vector3 pos)
    {
        skeletonAnimation.Initialize(true);
        dir = Random.Range(0, 4);
        Debug.Log(dir);
        float dis = 2f;
        switch (dir)
        {
            case 0:
                Position = new Vector3(pos.x - dis, pos.y, 5);
                break;
            case 1:
                Position = new Vector3(pos.x, pos.y + dis, 5);
                break;
            case 2:
                Position = new Vector3(pos.x + dis, pos.y, 5);
                break;
            case 3:
                Position = new Vector3(pos.x, pos.y - dis, 5);
                break;
            default:
                break;
        }
        des = new Vector3(pos.x, pos.y, 0);
        transform.DOMove(des, Octile.timeAttackAnim).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
