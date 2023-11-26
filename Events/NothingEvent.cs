using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;

namespace HullBreakerCompany.Events;

public class NothingEvent : HullEvent
{
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        string message = "<color=white>---</color>";
        HUDManager.Instance.AddTextToChatOnServer(message);
    }
}