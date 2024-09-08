using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Scrap;

public class ArmdayEvent : HullEvent
{
    public ArmdayEvent() {
        ID = "Armday";
        Weight = 20;
        Description = "Spawns a lot of heavy loot.";
        MessagesList = new List<string>() {
            { "Expect a lot of big heavy scrap!" },
            { "Grip strength required! Heavy duty scrap." },
            { "Slipped disks are not covered by company insurance!" },
            { "Lift heavy objects with a jerking twisting motion!" },
            { "Lower back pain" }
        };
        shortMessagesList = new List<string>() {
            { "HEAVY" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier) {
        Dictionary<string, int> scrapToSpawn = new() {
            { "Metal sheet", 20 },
            { "Large axle", 10 },
            { "V-type Engine", 10 },
            { "Stop sign", 10 },
            { "Yield sign", 10 },
            { "Fire hydrant", 15 },
            { "Broken engine", 10 },
            { "Anvil", 5 }
        };
        scrapToSpawn = CalculateScrapRarities(scrapToSpawn, levelModifier);
        if (scrapToSpawn.Count == 0) return false;
        levelModifier.AddSpawnableScrapRarityDict(scrapToSpawn);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}