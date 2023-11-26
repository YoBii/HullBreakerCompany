using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Event;

namespace HullBreakerCompany.Events;

public class BeeEvent : HullEvent
{
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        string message = "<color=white>Possibly a large amount of bee hives</color>";
        foreach (var unit in level.DaytimeEnemies.Where(unit => unit.enemyType.enemyPrefab.GetComponent<RedLocustBees>() != null))
        {
            unit.rarity = 256;
            break;
        }
        HUDManager.Instance.AddTextToChatOnServer(message);
    }
}