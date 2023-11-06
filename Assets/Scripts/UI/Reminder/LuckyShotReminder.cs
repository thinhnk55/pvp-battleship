using Framework;
using System;
using UnityEngine;

public class LuckyShotReminder : ConditionalMono
{
    [SerializeField] bool isHome;
    protected override Predicate<object> SetCondition()
    {
        return (o) => (Timer<LuckyShot>.Instance.TriggersFromBegin > 0) || (GameData.RocketCount.Data > 0 && isHome);
    }
    protected override void Awake()
    {
        base.Awake();
        Timer<LuckyShot>.Instance.OnTrigger += LuckyShotReminder_onTrigger;
    }

    private void LuckyShotReminder_onTrigger()
    {
        UpdateObject(typeof(LuckyShotReminder));
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        Timer<LuckyShot>.Instance.OnTrigger -= LuckyShotReminder_onTrigger;
    }
    private void Update()
    {
        Timer<LuckyShot>.Instance.Elasping();
    }
}
