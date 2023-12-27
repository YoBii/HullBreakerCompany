using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class TurretEvent : HullEvent
{
    public override string ID() => "Turret";
    public override int GetWeight() => 5;
    public override string GetDescription() => "Increased chance of turrets spawning";
    public override string GetMessage() => "<color=white>High security compound</color>";
    public override string GetShortMessage() => "<color=white>HIGH SECURITY</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        Plugin.LevelUnits(level, true);
        HullManager.SendChatEventMessage(this);
    }
}