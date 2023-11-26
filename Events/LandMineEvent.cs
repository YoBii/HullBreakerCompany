using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;

namespace HullBreakerCompany.Events;

public class LandMineEvent : HullEvent
{
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        string message = "<color=white>Watch your step, there are a lot of landmines</color>";
        Plugin.LevelUnits(level, false, true);
        HUDManager.Instance.AddTextToChatOnServer(message);
    }
}