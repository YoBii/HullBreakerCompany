using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Event;

namespace HullBreakerCompany.Events;

public class OneForAllEvent : HullEvent
{
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        string message = "<color=white>The ship will fly into orbit in an hour if one of the workers dies</color>";
        Plugin.OneForAllIsActive = true;
        HUDManager.Instance.AddTextToChatOnServer(message);
    }
}