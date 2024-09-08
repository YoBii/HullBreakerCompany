using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;
using Mono.Cecil;

namespace HullBreakerCompany.Events.Scrap;

public class DayDrinkingEvent : HullEvent
{
    public DayDrinkingEvent() {
        ID = "DayDrinking";
        Weight = 20;
        Description = "Spawns a lot of alcoholic beverages.";
        MessagesList = new List<string>() {
            { "Let's crack a beer or two!" },
            { "Alcohol use prohibited during work hours!" },
            { "There is something wrong with the scrap.." }
        };
        shortMessagesList = new List<string>() {
            { "DAYDRINKING" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier) {
        Dictionary<String, int> scrapToSpawn = new() {
            { "Bottles", 30 },
            { "Alcohol Flask", 25 },
            { "Wine bottle", 10 },
            { "Canteen", 25 }
        };
        scrapToSpawn = CalculateScrapRarities(scrapToSpawn, levelModifier);
        if (scrapToSpawn.Count == 0) return false;
        levelModifier.AddSpawnableScrapRarityDict(scrapToSpawn);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}