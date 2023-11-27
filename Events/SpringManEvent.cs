using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class SpringManEvent : HullEvent
{
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        const string message = "<color=white>It's impossible not to look at them</color>";
        componentRarity.Add(typeof(SpringManAI), 128);
        HullManager.SendChatMessage(message);
    }
}