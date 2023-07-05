using DG.Tweening;
using Framework;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class CoinVFX2 : MonoBehaviour
{
    ParticleSystem particleSys;
    VFXState state;
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
            if (state == VFXState.JACKPOT)
                state = VFXState.JACKPOT_GET;
        }
        );
    }

    public enum VFXState
    {
        IDLE,
        JACKPOT,
        JACKPOT_GET,
    }

    void Update()
    {
        //if (state == VFXState.JACKPOT_GET)
        if (state == VFXState.JACKPOT_GET || state == VFXState.JACKPOT)
        {
            SoundType.COIN_VFX.PlaySound();
            if (target)
            {
                //particleSys.DirectToTarget(target.position, multiplier);
                particleSys.DirectToTargetAfterTime(target.position, multiplier, 2.5f);
            }
        }

    }

    private void OnDisable()
    {
        SetState(VFXState.IDLE);
    }

    public void SetState(VFXState s)
    {
        state = s;
    }

    public static void CreateCoinFx(Transform target, Vector3 src, VFXState state)
    {
        CoinVFX2 coinFx = ObjectPoolManager.GenerateObject<CoinVFX2>(VFXFactory.Coin2, src, target.gameObject);
        coinFx.target = target;
        coinFx.state = state;
    }
}
