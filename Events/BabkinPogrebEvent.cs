using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class BabkinPogrebEvent : HullEvent
{
    public override string ID() => "BabkinPogreb";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Only jars of pickles spawn on the moon";
        public static List<String> MessagesList = new() {
        { "Scans indicate all scrap is... organic?" },
        { "There is something wrong with the scrap.." },
        { "We lost Rick. Find him!" },
        { "Quite a pickle indeed!" }
    };
    public static List<String> shortMessagesList = new() {
        { "SCRAP ANOMALY" },
        { "QUITE A PICKLE" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public static List<SpawnableItemWithRarity> scrapList = new();
    public override bool Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        try
        {
            if (HullManager.Instance == null)
            {
                Plugin.Mls.LogError("HullManager.Instance is null");
                return false;
            }

            if (level == null)
            {
                Plugin.Mls.LogError("level is null");
                return false;
            }
            
            // check on a copy of spawnableScrap if there are pickes in loot table
            scrapList = level.spawnableScrap;
            scrapList.RemoveAll(item => item.spawnableItem.itemName != "Jar of pickles");
            if (scrapList.Count == 0)
            {
                Plugin.Mls.LogWarning($"No jars of pickles found in spawnableScrap list!");
                scrapList.Clear();
                return false;
            }

            // backup loot table and actually remove all non pickle items
            scrapList = level.spawnableScrap;
            level.spawnableScrap.RemoveAll(item => item.spawnableItem.itemName != "Jar of pickles");
            
            foreach (var item in level.spawnableScrap.Where(item => item.spawnableItem.itemName == "Jar of pickles"))
            {
                item.rarity = 100;
            }
            
            HullManager.Instance.ExecuteAfterDelay(() => { DelayedReturnList(level); }, 12f);
            HullManager.AddChatEventMessage(this);
            return true;
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Plugin.Mls.LogError($"ArgumentOutOfRangeException caught in BabkinPogrebEvent.Execute: {ex.Message}");
            return false;
        }
    }

    private void DelayedReturnList(SelectableLevel level)
    {
        Plugin.Mls.LogInfo(ID() + " Event: Restoring loot table...");
        level.spawnableScrap.Clear();
        foreach (var item  in scrapList)
        {
            level.spawnableScrap.Add(item);
        }
        scrapList.Clear();
    }

}