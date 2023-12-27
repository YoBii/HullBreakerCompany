using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class FlowerManEvent : HullEvent
{
    public override string ID() => "FlowerMan";
    public override int GetWeight() => 20;
    public override string GetDescription() => "Increased chance of flowerman spawn";
    public override string GetMessage() => "<color=white>Detected signs of paranormal acitivy</color>";
    public override string GetShortMessage() => "<color=white>PARANORMAL</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        enemyComponentRarity.Add(typeof(FlowermanAI), 256);
        HullManager.SendChatEventMessage(this);
    }
}