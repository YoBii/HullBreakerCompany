using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class LandMineEvent : HullEvent
{
    public override string ID() => "LandMine";
    public override int GetWeight() => 30;
    public override string GetDescription() => "Increased chance of landmines spawning";
    public override string GetMessage() => "<color=white>Watch your step, there are a lot of landmines</color>";
    public override string GetShortMessage() => "<color=white>LANDMINE</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        Plugin.LevelUnits(level, false, true);
        HullManager.SendChatEventMessage(this);
    }
}