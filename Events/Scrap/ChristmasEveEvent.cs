using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Scrap;

public class ChristmasEveEvent : HullEvent
{
    public ChristmasEveEvent() {
        ID = "ChristmasEve";
        Weight = 20;
        Description = "Spawns a lot of gifts.";
        MessagesList = new List<string>() {
            { "Is it Christmas already?" },
            { "Help Santa collect the presents!" },
            { "The Company wishes a Merry Christmas!" }
        };
        shortMessagesList = new List<string>() {
            { "XMAS" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier) {
        string scrapToSpawn = "Gift";
        if (levelModifier.IsScrapSpawnable(scrapToSpawn)) {
            levelModifier.AddSpawnableScrapRarity(scrapToSpawn, 300);
            if (Plugin.ColoredEventMessages) {
                HullManager.AddChatEventMessageColored(this, "green");
            } else {
                HullManager.AddChatEventMessage(this);
            }
            return true;
        } else {
            return false;
        }
    }
}