using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Scrap;

public class ClownshowEvent : HullEvent
{
    public override string ID() => "Clownshow";
    public override int GetWeight() => 20;
    public override string GetDescription() => "Spawns a lot of noisy scrap.";
    public static List<string> MessagesList = new() {
        { "This is a clown show!" },
        { "Your clown nose fell off.." }
    };
    public static List<string> shortMessagesList = new() {
        { "CLOWN" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier) {
        Dictionary<String, int> scrapToSpawn = new() {
            { "Airhorn", 30 },
            { "Clown horn", 30 },
            { "Candy", 30 },
            { "Whoopie cushion", 10 }
        };
        scrapToSpawn = CalculateScrapRarities(scrapToSpawn, levelModifier);
        if (scrapToSpawn.Count == 0) return false;
        levelModifier.AddSpawnableScrapRarityDict(scrapToSpawn);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}