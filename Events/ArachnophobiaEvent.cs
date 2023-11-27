using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class ArachnophobiaEvent : HullEvent
{
    public override string ID() => "Arachnophobia";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        const string message = "<color=white>Possible habitat of spiders</color>";
        componentRarity.Add(typeof(SandSpiderAI), 256);
        HullManager.SendChatMessage(message);
    }
}