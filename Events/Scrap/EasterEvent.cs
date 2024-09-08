using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Scrap;

public class EasterEvent : HullEvent
{
    public EasterEvent() {
        ID = "Easter";
        Weight = 10;
        Description = "Spawns a lot of easter eggs.";
        MessagesList = new List<string>() {
            { "Festive decorations" },
            { "Help the Easter bunny collect the Easter eggs!" },
            { "Is this an Easter egg?" },
            { "↑ ↑ ↓ ↓ ← → ← → Ⓐ Ⓑ" }
        };
        shortMessagesList = new List<string>() {
            { "EGG" }
        };
    }
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