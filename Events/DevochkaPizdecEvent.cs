using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;

namespace HullBreakerCompany.Events;

public class DevochkaPizdecEvent : HullEvent
{
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        string message = "<color=white>A lot of workers are going crazy here</color>";
        componentRarity.Add(typeof(DressGirlAI), 512);
        HUDManager.Instance.AddTextToChatOnServer(message);
    }
}