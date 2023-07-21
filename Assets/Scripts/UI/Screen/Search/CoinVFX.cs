using DG.Tweening;
using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class CoinVFX : CacheMonoBehaviour
{
    ParticleSystem particleSys;
    public bool isForce;
    public float multiplier;
    public Transform target;
    [SerializeField] float delay;
    private void Start()
    {
        particleSys = GetComponent<ParticleSystem>();
    }
    void OnEnable()
    {
        SoundType.COIN_VFX.PlaySound();
        DOVirtual.DelayedCall(delay, () =>
        {
            isForce = true;
        }
        );
    }


    // Update is called once per frame
    void Update()
    {
        if ( isForce)
        {
            SoundType.COIN_VFX.PlaySound();
            if (target)
            {
                particleSys.DirectToTarget(target.position, multiplier);
            }
        }

    }

    private void OnDisable()
    {
        isForce = false;
    }

    public static void CoinVfx(Transform target, Vector3 src1, Vector3 src2)
    {
        CoinVFX coin1 = ObjectPoolManager.SpawnObject<CoinVFX>(VFXFactory.Coin, src1, target.transform);
        CoinVFX coin2 = ObjectPoolManager.SpawnObject<CoinVFX>(VFXFactory.Coin, src2, target.transform);
        coin1.transform.localScale = Vector3.one;
        coin2.transform.localScale = Vector3.one;
        Debug.Log("Coin");
        coin1.target = target;
        coin2.target = target;
    }
}
    
