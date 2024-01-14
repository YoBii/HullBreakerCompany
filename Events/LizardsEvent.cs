using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class LizardsEvent : HullEvent
{
    public override string ID() => "Lizards";
    public override int GetWeight() => 15;
    public override string GetDescription() => "Increased chance of puffers spawn";
    public override string GetMessage() => "<color=white>Horrible smell from toxic lizards</color>";
    public override string GetShortMessage() => "<color=white>LIZARDSSS</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        if (level.Enemies.All(unit => unit.enemyType.enemyPrefab.GetComponent<PufferAI>() == null)) return;
        
        enemyComponentRarity.Add(typeof(PufferAI), 64);
        HullManager.SendChatEventMessage(this);
    }
}