using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class FlowerManEvent : HullEvent
{
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        string message = "<color=white>So many eyes in the dark, carefully</color>";
        componentRarity.Add(typeof(FlowermanAI), 256);
        HUDManager.Instance.AddTextToChatOnServer(message);
    }
}