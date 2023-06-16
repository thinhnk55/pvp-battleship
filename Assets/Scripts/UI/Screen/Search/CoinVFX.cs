using DG.Tweening;
using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.ParticleSystem;

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
            particleSys.DirectToTarget(target.position, multiplier);
        }

    }

    private void OnDisable()
    {
        isForce = false;
    }
}
    
