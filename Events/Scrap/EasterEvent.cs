using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Scrap;

public class Easter : HullEvent
{
    public override string ID() => "Easter";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Spawns a lot of easter eggs.";
    public static List<string> MessagesList = new() {
        { "Festive decorations" },
        { "Help the Easter bunny collect the Easter eggs!" },
        { "Is this an Easter egg?" },
        { "↑ ↑ ↓ ↓ ← → ← → Ⓐ Ⓑ" }
    };
    public static List<string> shortMessagesList = new() {
        { "EGG" }
    };
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier) {
        string scrapToSpawn = "Easter egg";
        if (levelModifier.IsScrapSpawnable(scrapToSpawn)) {
            levelModifier.AddSpawnableScrapRarity(scrapToSpawn, 33);
            HullManager.AddChatEventMessage(this);
            return true;
        } else {
            return false;
        }
    }
}