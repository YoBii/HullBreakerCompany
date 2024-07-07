using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Scrap;

public class AC_RGBShoesEvent : HullEvent
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
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier) {
        Dictionary<String, int> scrapToSpawn = new() {
            { "Light shoes", 5 }
        };
        scrapToSpawn = CalculateScrapRarities(scrapToSpawn, levelModifier);
        if (scrapToSpawn.Count == 0) return false;
        levelModifier.AddSpawnableScrapRarityDict(scrapToSpawn);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}