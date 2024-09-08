using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Integrated.AdvancedCompany;

public class RGBShoesEvent : HullEvent
{
    public RGBShoesEvent() {
        ID = "AC_RGBShoes";
        Weight = 3;
        Description = "Increases spawn chance of the unique light shoes item (AdvancedCompany)";
        MessagesList = new List<string>() {
            { "Fancy sneakers. Gaming RGB! +120fps" },
            { "Run at the speed of light" },
            { "Reports of RGB lights.. traveling at extreme speeds" }
        };
        shortMessagesList = new List<string>() {
            { "Shoes" },
            { "RGB" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        Dictionary<string, int> scrapToSpawn = new() {
            { "Light shoes", 5 }
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