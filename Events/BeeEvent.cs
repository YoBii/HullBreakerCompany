using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class BeeEvent : HullEvent
{
    public override string ID() => "Bee";
    public override int GetWeight() => 30;
    public override string GetDescription() => "Increased chance of bee hives spawning";
    public static List<String> MessagesList = new() {
        { "Hostile life form detected"},
        { "A few species dominate this moon"},
        { "Sticky golden goodness" },
        { "High risk, high reward. Grab and run!" },
        { "Don't get stung by the bees!" }
    };
    public static List<String> shortMessagesList = new() {
        { "HIGH POPULATION" },
        { "DOMINANT SPECIES" },
        { "BZZZZZ" },
        { "STICKY GOLD" }    
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        foreach (var unit in level.DaytimeEnemies.Where(unit => unit.enemyType.enemyPrefab.GetComponent<RedLocustBees>() != null))
        {
            unit.rarity = 256;
            break;
        }
        return true;
    }
}