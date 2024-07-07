using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Scrap;

public class AC_BunnyEvent : HullEvent
{
    public override string ID() => "AC_Bunny";
    public override int GetWeight() => 3;
    public override string GetDescription() => "Increases spawn chance of the unique bunny ears item (AdvancedCompany)";
    public static List<string> MessagesList = new() {
        { "Jump little bunny, jump!" },
        { "Hop, hop.." },
        { "Reports of silly bunny ears" }
    };
    public static List<string> shortMessagesList = new() {
        { "BUNNY" },
        { "HOPHOP" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier) {
        Dictionary<String, int> scrapToSpawn = new() {
            { "Bunny Ears", 5 }
        };
        scrapToSpawn = CalculateScrapRarities(scrapToSpawn, levelModifier);
        if (scrapToSpawn.Count == 0) return false;
        levelModifier.AddSpawnableScrapRarityDict(scrapToSpawn);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}