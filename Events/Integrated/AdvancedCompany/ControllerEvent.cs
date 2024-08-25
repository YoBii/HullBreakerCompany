using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Integrated.AdvancedCompany;

public class ControllerEvent : HullEvent
{
    public override string ID() => "AC_Controller";
    public override int GetWeight() => 3;
    public override string GetDescription() => "Increases spawn chance of the green Pietsmiet controller item (AdvancedCompany)";
    public static List<string> MessagesList = new() {
        { "There will be gaming!" },
        { "This moon is.. doom-ed?!" },
        { "The only thing they fear is you" },
        { "\"Ich bin himmelblau, himmelblau.. Scha-la-la!\"" }
    };
    public static List<string> shortMessagesList = new() {
        { "CONTROLLER" },
        { "PIETSMIET" }
    };
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        Dictionary<string, int> scrapToSpawn = new() {
            { "Controller", 5 }
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