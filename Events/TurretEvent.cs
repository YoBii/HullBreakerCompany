using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;

namespace HullBreakerCompany.Events;

public class TurretEvent : HullEvent
{
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        string message = "<color=white>Alert, turrets detected</color>";
        Plugin.LevelUnits(level, true);
        HUDManager.Instance.AddTextToChatOnServer(message);
    }
}