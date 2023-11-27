using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class OneForAllEvent : HullEvent
{
    public override string ID() => "OneForAll";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        const string message = "<color=white>The ship will fly into orbit in an hour if one of the workers dies</color>";
        Plugin.OneForAllIsActive = true;
        HullManager.SendChatMessage(message);
    }
}