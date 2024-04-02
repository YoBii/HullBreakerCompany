using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;
using Mono.Cecil;

namespace HullBreakerCompany.Events.Scrap;

public class DayDrinkingEvent : HullEvent
{
    public override string ID() => "DayDrinking";
    public override int GetWeight() => 20;
    public override string GetDescription() => "Spawns a lot of alcoholic beverages.";
    public static List<string> MessagesList = new() {
        { "Let's crack a beer or two!" },
        { "Alcohol use prohibited during work hours!" },
        { "There is something wrong with the scrap.." }
    };
    public static List<string> shortMessagesList = new() {
        { "DAYDRINKING" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier) {
        Dictionary<String, int> scrapToSpawn = new() {
            { "Bottles", 2000 },
            { "Wine bottle", 500 },
            { "Alcohol Flask", 1000 },
            { "Canteen", 1000 }
        };
        if (scrapToSpawn.Any(scrap => levelModifier.IsScrapSpawnable(scrap.Key))) {
            levelModifier.AddSpawnableScrapRarityDict(scrapToSpawn);
            HullManager.AddChatEventMessage(this);
            return true;
        } else {
            return false;
        }
    }
}