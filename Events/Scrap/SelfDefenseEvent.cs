using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Scrap;

public class SelfDefenseEvent : HullEvent
{
    public override string ID() => "SelfDefense";
    public override int GetWeight() => 20;
    public override string GetDescription() => "Spawns a lot of scrap that can be utilized as weapon.";
    public static List<string> MessagesList = new() {
        { "Weapons scattered all around!" },
        { "DIY self defense training today!" }
    };
    public static List<string> shortMessagesList = new() {
        { "WEAPONS" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier) {
        Dictionary<String, int> scrapToSpawn = new() {
            { "Plunger", 20 },
            { "Stop sign", 15 },
            { "Yield sign", 15 },
            { "Baseball bat", 15 },
            { "Toy Hammer", 10 },
            { "Fireaxe", 5 },
        };
        scrapToSpawn = CalculateScrapRarities(scrapToSpawn, levelModifier);
        if (scrapToSpawn.Count == 0) return false;
        levelModifier.AddSpawnableScrapRarityDict(scrapToSpawn);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}