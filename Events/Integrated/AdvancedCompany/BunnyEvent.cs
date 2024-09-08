using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Integrated.AdvancedCompany;

public class BunnyEvent : HullEvent
{
    public BunnyEvent() {
        ID = "AC_Bunny";
        Weight = 3;
        Description = "Increases spawn chance of the unique bunny ears item (AdvancedCompany)";
        MessagesList = new List<string>() {
            { "Jump little bunny, jump!" },
            { "Hop, hop.." },
            { "Reports of silly bunny ears" }
        };
        shortMessagesList = new List<string>() {
            { "BUNNY" },
            { "HOPHOP" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        Dictionary<string, int> scrapToSpawn = new() {
            { "Bunny Ears", 5 }
        };
        scrapToSpawn = CalculateScrapRarities(scrapToSpawn, levelModifier);
        if (scrapToSpawn.Count == 0) return false;
        levelModifier.AddSpawnableScrapRarityDict(scrapToSpawn);
        if (Plugin.ColoredEventMessages)
        {
            HullManager.AddChatEventMessageColored(this, "green");
        }
        else
        {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}