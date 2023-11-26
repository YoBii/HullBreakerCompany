using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;

namespace HullBreakerCompany.Events;

public class SpringManEvent : HullEvent
{
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        string message = "<color=white>It's impossible not to look at them</color>";
        componentRarity.Add(typeof(SpringManAI), 128);
        HUDManager.Instance.AddTextToChatOnServer(message);
    }
}