using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class ArachnophobiaEvent : HullEvent
{
    public override string ID() => "Arachnophobia";
    public override int GetWeight() => 20;
    public override string GetDescription() => "Increased chance of spider spawning";
    public override string GetMessage() => "<color=white>Possible habitat of spiders</color>";
    public override string GetShortMessage() => "<color=white>ARACHNOPHOBIA</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        enemyComponentRarity.Add(typeof(SandSpiderAI), 256);
        HullManager.SendChatEventMessage(GetMessage());
    }
}