using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class BeeEvent : HullEvent
{
    public override string ID() => "Bee";
    public override int GetWeight() => 30;
    public override string GetDescription() => "Increased chance of bee hives spawning";
    public override string GetMessage() => "<color=white>Possibly a large amount of bee hives</color>";
    public override string GetShortMessage() => "<color=white>ANNOYING BUZZING</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        foreach (var unit in level.DaytimeEnemies.Where(unit => unit.enemyType.enemyPrefab.GetComponent<RedLocustBees>() != null))
        {
            unit.rarity = 256;
            break;
        }
        HullManager.SendChatEventMessage(this);
    }
}