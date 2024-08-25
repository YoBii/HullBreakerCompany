using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Integrated.AdvancedCompany;

public class RGBShoesEvent : HullEvent
{
    public override string ID() => "AC_RGBShoes";
    public override int GetWeight() => 3;
    public override string GetDescription() => "Increases spawn chance of the unique light shoes item (AdvancedCompany)";
    public static List<string> MessagesList = new() {
        { "Fancy sneakers. Gaming RGB! +120fps" },
        { "Run at the speed of light" },
        { "Reports of RGB lights.. traveling at extreme speeds" }
    };
    public static List<string> shortMessagesList = new() {
        { "Shoes" },
        { "RGB" }
    };
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
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