using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;

namespace HullBreakerCompany.Events;

public class ArachnophobiaEvent : HullEvent
{
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        string message = "<color=white>Possible habitat of spiders</color>";
        componentRarity.Add(typeof(SandSpiderAI), 512);
        HUDManager.Instance.AddTextToChatOnServer(message);
    }
}