using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Scrap;

public class LuckyDayEvent : HullEvent
{
    public override string ID() => "LuckyDay";
    public override int GetWeight() => 5;
    public override string GetDescription() => "Increases spawn chance of high value loot.";
    public static List<string> MessagesList = new() {
        { "High value scrap detected!" },
        { "Supposed to hold some very valuable scrap" },
        { "Today's your lucky day!" }
    };
    public static List<string> shortMessagesList = new() {
        { "LUCKY" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier) {
        Dictionary<String, int> scrapToSpawn = new() {
            { "Cash register", 20 },
            { "Gold bar", 20 }
        };
        scrapToSpawn = CalculateScrapRarities(scrapToSpawn, levelModifier);
        if (scrapToSpawn.Count == 0) return false;
        levelModifier.AddSpawnableScrapRarityDict(scrapToSpawn);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}