using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Scrap;

public class ClownshowEvent : HullEvent
{
    public ClownshowEvent() {
        ID = "Clownshow";
        Weight = 20;
        Description = "Spawns a lot of noisy scrap.";
        MessagesList = new List<string>() {
            { "This is a clown show!" },
            { "Your clown nose fell off.." }
        };
        shortMessagesList = new List<string>() {
            { "CLOWN" }
        };
    }
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