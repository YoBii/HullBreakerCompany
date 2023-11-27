using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class DevochkaPizdecEvent : HullEvent
{
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        const string message = "<color=white>A lot of workers are going crazy here</color>";
        componentRarity.Add(typeof(DressGirlAI), 1024);
        HullManager.SendChatMessage(message);
    }
}