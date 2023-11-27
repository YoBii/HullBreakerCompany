using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class TurretEvent : HullEvent
{
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        const string message = "<color=white>Alert, turrets detected</color>";
        Plugin.LevelUnits(level, true);
        HullManager.SendChatMessage(message);
    }
}