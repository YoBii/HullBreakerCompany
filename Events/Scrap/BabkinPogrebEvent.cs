using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Scrap;

public class BabkinPogrebEvent : HullEvent
{
    public BabkinPogrebEvent() {
        ID = "BabkinPogreb";
        Weight = 20;
        Description = "Spawns a lot of pickle jars.";
        MessagesList = new List<string>() {
            { "Scans indicate all scrap is.. organic?" },
            { "There is something wrong with the scrap.." },
            { "We lost Rick. Find him!" },
            { "Quite a pickle indeed!" }
        };
        shortMessagesList = new List<string>() {
            { "QUITE A PICKLE" }
        };
    }
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
    public static List<SpawnableItemWithRarity> scrapList = new();
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        string scrapToSpawn = "Jar of pickles";
        if (levelModifier.IsScrapSpawnable(scrapToSpawn)) {
            levelModifier.AddSpawnableScrapRarity(scrapToSpawn, 100);
            HullManager.AddChatEventMessage(this);
            return true;
        } else {
            return false;
        }
    }
}