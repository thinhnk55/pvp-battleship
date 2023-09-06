using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankReminder : ConditionalMono
{
    protected override Predicate<object> SetCondition()
    {
        return (o) => { return Timer<RankCollection>.Instance.TriggersFromBegin > 0; };
    }
}
