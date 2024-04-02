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
            { "Cash register", 200 },
            { "Gold bar", 100 }
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