using System;
using System.Collections.Generic;
using System.Linq;

namespace HullBreakerCompany.Hull;

public abstract class HullEvent
{ 
    public abstract string ID();
    public virtual int GetWeight()
    {
        return 1;
    }
    public virtual string GetDescription() => "Default description";
    public virtual string GetMessage() => "Default message";
    public virtual string GetShortMessage() => "Short message";
    public virtual bool Execute(SelectableLevel level, LevelModifier levelModifier) { return true; }
    public virtual Dictionary<string, int> CalculateScrapRarities(Dictionary<string, int> inputScrap, LevelModifier levelModifier) {
        var totalRarityWeight = 0;
        var totalEffectiveRarityWeight = 0;
        Dictionary<string, int> newScrapToSpawn = new Dictionary<string, int>();
        Plugin.Mls.LogInfo($"{ID()}: Rarities (% of total) [{string.Join(", ", inputScrap.Select(scrap => scrap.Key + ":" + scrap.Value))}]");
        foreach (var scrap in inputScrap) {
            totalRarityWeight += scrap.Value;
            if (levelModifier.IsScrapSpawnable(scrap.Key)) {
                totalEffectiveRarityWeight += scrap.Value;
                newScrapToSpawn.TryAdd(scrap.Key, scrap.Value);
            }
        }
        Dictionary <string, int> tmp = new (newScrapToSpawn);
        foreach (var scrap in inputScrap) {
            if (newScrapToSpawn.ContainsKey(scrap.Key)) {
                newScrapToSpawn[scrap.Key] = (int)Math.Round(scrap.Value / (double)totalEffectiveRarityWeight * totalRarityWeight);
            }
        }
        if (newScrapToSpawn.Count() != inputScrap.Count()) {
            Plugin.Mls.LogInfo($"Recalculating scrap rarities to compensate for items that are not in this moon's loot table..");
            Plugin.Mls.LogInfo($"{ID()}: New rarities (% of total) [{string.Join(", ", newScrapToSpawn.Select(scrap => scrap.Key + ":" + scrap.Value))}]");
        }
        return newScrapToSpawn;
    }
}