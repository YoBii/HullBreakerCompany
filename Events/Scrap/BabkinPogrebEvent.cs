using HullBreakerCompany.Hull;
using System.Collections.Generic;

namespace HullBreakerCompany.Events.Scrap;

public class BabkinPogrebEvent : HullEvent
{
    public BabkinPogrebEvent()
    {
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
    public static List<SpawnableItemWithRarity> scrapList = new();
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        string scrapToSpawn = "Jar of pickles";
        if (levelModifier.IsScrapSpawnable(scrapToSpawn))
        {
            levelModifier.AddSpawnableScrapRarity(scrapToSpawn, 100);
            HullManager.AddChatEventMessage(this);
            return true;
        }
        else
        {
            return false;
        }
    }
}