using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Integrated.AdvancedCompany;

public class ControllerEvent : HullEvent
{
    public ControllerEvent() {
        ID = "AC_Controller";
        Weight = 3;
        Description = "Increases spawn chance of the green Pietsmiet controller item (AdvancedCompany)";
        MessagesList = new List<string>() {
            { "There will be gaming!" },
            { "This moon is.. doom-ed?!" },
            { "The only thing they fear is you" },
            { "\"Ich bin himmelblau, himmelblau.. Scha-la-la!\"" }
        };
        shortMessagesList = new List<string>() {
            { "CONTROLLER" },
            { "PIETSMIET" }
        };
    }
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