using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class TurretEvent : HullEvent
{
    public override string ID() => "Turret";
    public override int GetWeight() => 5;
    public override string GetDescription() => "Increased chance of turrets spawning";
    public override string GetMessage() => "<color=white>Alert, turrets detected</color>";
    public override string GetShortMessage() => "<color=white>TURRETS</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        Plugin.LevelUnits(level, true);
        HullManager.SendChatEventMessage(this);
    }
}