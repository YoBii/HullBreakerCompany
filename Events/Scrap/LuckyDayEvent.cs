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
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
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