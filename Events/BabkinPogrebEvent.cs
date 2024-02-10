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
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        try
        {
            if (HullManager.Instance == null)
            {
                Plugin.Mls.LogError("HullManager.Instance is null");
                return;
            }

            if (level == null)
            {
                Plugin.Mls.LogError("level is null");
                return;
            }
            
            level.spawnableScrap.RemoveAll(item => item.spawnableItem.itemName != "Jar of pickles");
            if (level.spawnableScrap.Count == 0)
            {
                Plugin.Mls.LogError("No jars of pickles found in spawnableScrap list!");
                DelayedReturnList(level);
                return false;
            }
            
            foreach (var item in level.spawnableScrap.Where(item => item.spawnableItem.itemName == "Jar of pickles"))
            {
                item.rarity = 100;
            }
            
            HullManager.Instance.ExecuteAfterDelay(() => { DelayedReturnList(level); }, 12f);
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
        Plugin.Mls.LogInfo("Resetting spawnable items...");
        level.spawnableScrap.Clear();
        foreach (var item  in Plugin.NotModifiedSpawnableItemsWithRarity)
        {
            level.spawnableScrap.Add(item);
        }
    }

}