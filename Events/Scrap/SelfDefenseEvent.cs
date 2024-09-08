using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Scrap;

public class SelfDefenseEvent : HullEvent
{
    public SelfDefenseEvent() {
        ID = "SelfDefense";
        Weight = 20;
        Description = "Spawns a lot of scrap that can be utilized as weapon.";
        MessagesList = new List<string>() {
            { "Weapons scattered all around!" },
            { "DIY self defense training today!" }
        };
        shortMessagesList = new List<string>() {
            { "WEAPONS" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier) {
        Dictionary<String, int> scrapToSpawn = new() {
            { "Plunger", 20 },
            { "Stop sign", 15 },
            { "Yield sign", 15 },
            { "Baseball bat", 15 },
            { "Toy Hammer", 10 },
            { "Fireaxe", 5 },
        };
        scrapToSpawn = CalculateScrapRarities(scrapToSpawn, levelModifier);
        if (scrapToSpawn.Count == 0) return false;
        levelModifier.AddSpawnableScrapRarityDict(scrapToSpawn);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}