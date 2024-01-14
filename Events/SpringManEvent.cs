using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class SpringManEvent : HullEvent
{
    public override string ID() => "SpringMan";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Increased chance of spring man spawning (coil-head)";
    public override string GetMessage() => "<color=white>It's impossible not to look at them</color>";
    public override string GetShortMessage() => "<color=white>SKULL COILS</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        if (level.Enemies.All(unit => unit.enemyType.enemyPrefab.GetComponent<SpringManAI>() == null)) return;
        
        enemyComponentRarity.Add(typeof(SpringManAI), 128);
        HullManager.SendChatEventMessage(this);
    }
}