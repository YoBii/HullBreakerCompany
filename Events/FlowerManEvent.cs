using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class FlowerManEvent : HullEvent
{
    public override string ID() => "FlowerMan";
    public override int GetWeight() => 20;
    public override string GetDescription() => "Increased chance of flowerman spawn";
    public override string GetMessage() => "<color=white>So many eyes in the dark, carefully</color>";
    public override string GetShortMessage() => "<color=white>WHITE EYES...</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        if (level.Enemies.All(unit => unit.enemyType.enemyPrefab.GetComponent<FlowermanAI>() == null)) return;
        
        enemyComponentRarity.Add(typeof(FlowermanAI), 256);
        HullManager.SendChatEventMessage(this);
    }
}