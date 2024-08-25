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
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
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