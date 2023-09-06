using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyShotReminder : ConditionalMono
{
    [SerializeField] bool isHome;
    protected override Predicate<object> SetCondition()
    {
        return (o) => Timer<LuckyShot>.Instance.TriggersFromBegin > 0 || (GameData.RocketCount.Data > 0 && isHome);
    }
}
