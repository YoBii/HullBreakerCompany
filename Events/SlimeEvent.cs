using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;

namespace HullBreakerCompany.Events;

public class SlimeEvent : HullEvent
{
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        string message = "<color=white>Inhabited with slime</color>";
        componentRarity.Add(typeof(BlobAI), 128);
        HUDManager.Instance.AddTextToChatOnServer(message);
    }
}