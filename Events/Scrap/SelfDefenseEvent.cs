using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Scrap;

public class SelfDefenseEvent : HullEvent
{
    public override string ID() => "SelfDefense";
    public override int GetWeight() => 20;
    public override string GetDescription() => "Increased chance for weaopn scrap to spawn";
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
            { "Plunger", 2000 },
            { "Fireaxe", 500 },
            { "Toy Hammer", 1000 },
            { "Stop sign", 2000 },
            { "Yield sign", 2000 },
            { "Baseball bat", 500 }
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