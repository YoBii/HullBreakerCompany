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
    public override string GetDescription() => "Increased chance for heavy scrap to spawn";
    public static List<string> MessagesList = new() {
        { "Roll up your sleeves and collect the scrap!" },
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
            { "Large axle", 2000 },
            { "V-type Engine", 2000 },
            { "Metal sheet", 2000 },
            { "Stop sign", 2000 },
            { "Yield sign", 2000 },
            { "Fire hydrant", 1000 },
            { "Broken engine", 1000 },
            { "Anvil", 250 }
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