using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Scrap;

public class ArmdayEvent : HullEvent
{
    public override string ID() => "Armday";
    public override int GetWeight() => 20;
    public override string GetDescription() => "Spawns a lot of heavy loot.";
    public static List<string> MessagesList = new() {
        { "Expect a lot of of big heavy scrap!" },
        { "Grip strength required! Heavy duty scrap." },
        { "Slipped disks are not covered by company insurance!" },
        { "Lift heavy objects with a jerking twisting motion!" },
        { "Lower back pain" }
    };
    public static List<string> shortMessagesList = new() {
        { "HEAVY" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
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