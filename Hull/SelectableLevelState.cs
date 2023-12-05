using System;
using System.Collections.Generic;
using UnityEngine;

namespace HullBreakerCompany.Hull;

public class SelectableLevelState {
    
    public int MinScrap;
    public int MaxScrap;
    public int MinTotalScrapValue;
    public int MaxTotalScrapValue;
    public int MaxEnemyPowerCount = 8;
    public int MaxOutsideEnemyPowerCount = 15;
    public int MaxDaytimeEnemyPowerCount = 20;
    public AnimationCurve EnemySpawnChanceThroughoutDay;
    public AnimationCurve OutsideEnemySpawnChanceThroughDay;
    public AnimationCurve DaytimeEnemySpawnChanceThroughDay;
    
    public List<SpawnableEnemyWithRarity> EnemyList= new List<SpawnableEnemyWithRarity>();
    public SelectableLevelState(SelectableLevel level) {
        
        MinScrap = level.minScrap;
        MaxScrap = level.maxScrap;
        MinTotalScrapValue = level.minTotalScrapValue;
        MaxTotalScrapValue = level.maxTotalScrapValue;
        MaxEnemyPowerCount = level.maxEnemyPowerCount;
        MaxOutsideEnemyPowerCount = level.maxOutsideEnemyPowerCount;
        MaxDaytimeEnemyPowerCount = level.maxDaytimeEnemyPowerCount;
        EnemySpawnChanceThroughoutDay = level.enemySpawnChanceThroughoutDay;
        OutsideEnemySpawnChanceThroughDay = level.outsideEnemySpawnChanceThroughDay;
        DaytimeEnemySpawnChanceThroughDay = level.daytimeEnemySpawnChanceThroughDay;
        
        foreach (var enemy in level.Enemies) {
            var clone = new SpawnableEnemyWithRarity {
                enemyType = enemy.enemyType,
                rarity = enemy.rarity
            };
            EnemyList.Add(clone);
        }
        
        // Debug
        // foreach (var enemy in EnemyList)
        // {
        //     Plugin.Mls.LogInfo($"stored enemy: {enemy.enemyType} rarity: {enemy.rarity}");
        // }
    }

    public void RestoreState(SelectableLevel level) {
        
        level.minScrap = MinScrap;
        level.maxScrap = MaxScrap;
        level.minTotalScrapValue = MinTotalScrapValue;
        level.maxTotalScrapValue = MaxTotalScrapValue;
        level.maxEnemyPowerCount = MaxEnemyPowerCount;
        level.maxOutsideEnemyPowerCount = MaxOutsideEnemyPowerCount;
        level.maxDaytimeEnemyPowerCount = MaxDaytimeEnemyPowerCount;
        level.enemySpawnChanceThroughoutDay = EnemySpawnChanceThroughoutDay;
        level.outsideEnemySpawnChanceThroughDay = OutsideEnemySpawnChanceThroughDay;
        level.daytimeEnemySpawnChanceThroughDay = DaytimeEnemySpawnChanceThroughDay;
        Plugin.Mls.LogInfo("");
        Plugin.Mls.LogInfo("clearing enemy list");
        Plugin.Mls.LogInfo("");

       
        level.Enemies.Clear();
        
        foreach (var enemy in EnemyList) {
            var clone = new SpawnableEnemyWithRarity {
                enemyType = enemy.enemyType,
                rarity = enemy.rarity
            };
            level.Enemies.Add(clone);
        }
        
        // Debug
        // foreach (var enemy in level.Enemies)
        // {
        //     Plugin.Mls.LogInfo($"restored enemy: {enemy.enemyType} rarity: {enemy.rarity}");
        // }
        
    }
}