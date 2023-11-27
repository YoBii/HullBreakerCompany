using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class LandMineEvent : HullEvent
{
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        const string message = "<color=white>Watch your step, there are a lot of landmines</color>";
        Plugin.LevelUnits(level, false, true);
        HullManager.SendChatMessage(message);
    }
}