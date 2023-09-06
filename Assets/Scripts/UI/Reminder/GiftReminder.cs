using Framework;
using System;

public class GiftReminder : ConditionalMono
{
    protected override Predicate<object> SetCondition()
    {
        return (o) => Timer<Gift>.Instance.TriggersFromBegin > 0;
    }
}
