using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Scrap;

public class AC_ControllerEvent : HullEvent
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
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier) {
        Dictionary<String, int> scrapToSpawn = new() {
            { "Controller", 5 }
        };
        scrapToSpawn = CalculateScrapRarities(scrapToSpawn, levelModifier);
        if (scrapToSpawn.Count == 0) return false;
        levelModifier.AddSpawnableScrapRarityDict(scrapToSpawn);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}