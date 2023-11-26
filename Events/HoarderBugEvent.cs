using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class HoarderBugEvent : HullEvent
{
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        string message = "<color=white>Keep an eye on the loot, Hoarding Bugs nearby</color>";
        componentRarity.Add(typeof(HoarderBugAI), 512);
        HUDManager.Instance.AddTextToChatOnServer(message);
    }
}