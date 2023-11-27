using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class LizardsEvent : HullEvent
{
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        const string message = "<color=white>Horrible smell from toxic lizards</color>";
        componentRarity.Add(typeof(PufferAI), 64);
        HullManager.SendChatMessage(message);
    }
}