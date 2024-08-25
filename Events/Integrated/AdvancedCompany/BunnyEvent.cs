using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Integrated.AdvancedCompany;

public class BunnyEvent : HullEvent
{
    public override string ID() => "AC_Bunny";
    public override int GetWeight() => 3;
    public override string GetDescription() => "Increases spawn chance of the unique bunny ears item (AdvancedCompany)";
    public static List<string> MessagesList = new() {
        { "Jump little bunny, jump!" },
        { "Hop, hop.." },
        { "Reports of silly bunny ears" }
    };
    public static List<string> shortMessagesList = new() {
        { "BUNNY" },
        { "HOPHOP" }
    };
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
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