using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class SpringManEvent : HullEvent
{
    public override string ID() => "SpringMan";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Increased chance of spring man spawning (coil-head)";
    public override string GetMessage() => "<color=white>It's impossible not to look at them</color>";
    public override string GetShortMessage() => "<color=white>SKULL COILS</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        componentRarity.Add(typeof(SpringManAI), 128);
        HullManager.SendChatEventMessage(this);
    }
}