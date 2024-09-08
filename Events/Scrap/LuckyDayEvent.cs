using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Scrap;

public class LuckyDayEvent : HullEvent
{
    public LuckyDayEvent() {
        ID = "LuckyDay";
        Weight = 5;
        Description = "Increases spawn chance of high value loot.";
        MessagesList = new List<string>() {
            { "High value scrap detected!" },
            { "Supposed to hold some very valuable scrap" },
            { "Today's your lucky day!" }
        };
        shortMessagesList = new List<string>() {
            { "LUCKY" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier) {
        Dictionary<String, int> scrapToSpawn = new() {
            { "Cash register", 20 },
            { "Gold bar", 20 }
        };
        scrapToSpawn = CalculateScrapRarities(scrapToSpawn, levelModifier);
        if (scrapToSpawn.Count == 0) return false;
        levelModifier.AddSpawnableScrapRarityDict(scrapToSpawn);
        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "green");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}