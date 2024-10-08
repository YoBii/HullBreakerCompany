﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace HullBreakerCompany.Hull;

public abstract class HullEvent
{
    public string ID = "Default";
    public int Weight = 0;
    public string Description = "Default description"; 
    public List<string> MessagesList = ["Default event message"];
    public List<string> shortMessagesList = ["MESSAGE"];
    public virtual string GetID() => ID;
    public virtual int GetWeight() => Weight;
    public virtual string GetDescription() => Description;
    public virtual string GetMessage() {
        if (Plugin.UniqueEventMessages) {
            return MessagesList.Last();
        } else {
            return MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
        }
    }
    public virtual string GetShortMessage() {
        if (Plugin.UniqueEventMessages) {
            return shortMessagesList.Last();
        } else {
            return shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
        }
    }
    public virtual bool Execute(SelectableLevel level, LevelModifier levelModifier) { return true; }
    public virtual bool SimulateExecution(SelectableLevel level, LevelModifier levelModifier, Dictionary<string, List<int>> enemies, Dictionary<string, List<int>> OutsideEnemies, Dictionary<string, List<int>> DaytimeEnemies, Dictionary<string, int> scrap) { 
        if (enemies != null && enemies.Count > 0 && enemies.Any(enemy => !levelModifier.IsEnemySpawnable(enemy.Key))) {
            //Plugin.Mls.LogWarning($"{ID()}: Enemies defined but none of them are spawnable in this level.");
            return false;
        }
        if (OutsideEnemies != null && OutsideEnemies.Count > 0 && OutsideEnemies.Any(enemy => !levelModifier.IsOutsideEnemySpawnable(enemy.Key))) {
            //Plugin.Mls.LogWarning($"{ID()}: Outside enemies defined but none of them are spawnable in this level.");
            return false;
        }
        if (DaytimeEnemies != null && DaytimeEnemies.Count > 0 && DaytimeEnemies.Any(enemy => !levelModifier.IsDaytimeEnemySpawnable(enemy.Key))) {
            //Plugin.Mls.LogWarning($"{ID()}: Daytime enemies defined but none of them are spawnable in this level.");
            return false;
        }
        if (scrap != null && scrap.Count > 0 && scrap.All(item => !levelModifier.IsScrapSpawnable(item.Key))) {
            Plugin.Mls.LogWarning($"{GetID()}: has scrap defined but none of them are spawnable in this level.");
            return false;
        }
        return true;
    }
    public virtual Dictionary<string, int> CalculateScrapRarities(Dictionary<string, int> inputScrap, LevelModifier levelModifier, bool logging = true) {
        var totalRarityWeight = 0;
        var totalEffectiveRarityWeight = 0;
        Dictionary<string, int> newScrapToSpawn = new Dictionary<string, int>();
        Plugin.Mls.LogInfo($"{GetID()}: Scrap rarities (% of total) [{string.Join(", ", inputScrap.Select(scrap => scrap.Key + ":" + scrap.Value))}]");
        foreach (var scrap in inputScrap) {
            totalRarityWeight += scrap.Value;
            if (levelModifier.IsScrapSpawnable(scrap.Key, false)) {
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
            Plugin.Mls.LogInfo($"Recalculated scrap rarities to compensate for items that are not in this moon's loot table..");
            Plugin.Mls.LogInfo($"{GetID()}: New rarities (% of total) [{string.Join(", ", newScrapToSpawn.Select(scrap => scrap.Key + ":" + scrap.Value))}]");
        }
        return newScrapToSpawn;
    }
}