using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class NothingEvent : HullEvent
{
    public override string ID() => "Nothing";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        const string message = "<color=white>---</color>";
        HullManager.SendChatMessage(message);
    }
}