using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;

namespace HullBreakerCompany.Events;

public class LizardsEvent : HullEvent
{
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        string message = "<color=white>Horrible smell from toxic lizards</color>";
        componentRarity.Add(typeof(PufferAI), 512);
        HUDManager.Instance.AddTextToChatOnServer(message);
    }
}