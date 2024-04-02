using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Scrap;

public class BabkinPogrebEvent : HullEvent
{
    public override string ID() => "BabkinPogreb";
    public override int GetWeight() => 20;
    public override string GetDescription() => "Spawns a lot of pickle jars.";
    public static List<string> MessagesList = new() {
        { "Scans indicate all scrap is.. organic?" },
        { "There is something wrong with the scrap.." },
        { "We lost Rick. Find him!" },
        { "Quite a pickle indeed!" }
    };
    public static List<string> shortMessagesList = new() {
        { "QUITE A PICKLE" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public static List<SpawnableItemWithRarity> scrapList = new();
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        string scrapToSpawn = "Jar of pickles";
        if (levelModifier.IsScrapSpawnable(scrapToSpawn)) {
            levelModifier.AddSpawnableScrapRarity(scrapToSpawn, 1500);
            HullManager.AddChatEventMessage(this);
            return true;
        } else {
            return false;
        }
    }
}