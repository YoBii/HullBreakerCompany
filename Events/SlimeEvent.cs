using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class SlimeEvent : HullEvent
{
    public override string ID() => "Slime";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        const string message = "<color=white>Inhabited with slime</color>";
        componentRarity.Add(typeof(BlobAI), 128);
        HullManager.SendChatMessage(message);
    }
}