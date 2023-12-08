using System;
using System.Collections.Generic;
using UnityEngine;

namespace HullBreakerCompany.Hull
{
    public class SelectableLevelState
    {
        public int MinScrap, MaxScrap, MinTotalScrapValue, MaxTotalScrapValue;
        public int MaxEnemyPowerCount = 8, MaxOutsideEnemyPowerCount = 15, MaxDaytimeEnemyPowerCount = 20;
        public AnimationCurve EnemySpawnChanceThroughoutDay, OutsideEnemySpawnChanceThroughDay, DaytimeEnemySpawnChanceThroughDay;

        public List<SpawnableEnemyWithRarity> EnemyList = new ();
        public List<SpawnableEnemyWithRarity> OutsideEnemyList = new ();
        public List<SpawnableEnemyWithRarity> DaytimeEnemyList = new ();

        public SelectableLevelState(SelectableLevel level)
        {
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

            CloneEnemies(level.Enemies, EnemyList);
            CloneEnemies(level.OutsideEnemies, OutsideEnemyList);
            CloneEnemies(level.DaytimeEnemies, DaytimeEnemyList);
        }

        public void RestoreState(SelectableLevel level)
        {
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

            level.Enemies.Clear();
            CloneEnemies(EnemyList, level.Enemies);

            level.OutsideEnemies.Clear();
            CloneEnemies(OutsideEnemyList, level.OutsideEnemies);

            level.DaytimeEnemies.Clear();
            CloneEnemies(DaytimeEnemyList, level.DaytimeEnemies);
            
            
        }

        private void CloneEnemies(List<SpawnableEnemyWithRarity> source, List<SpawnableEnemyWithRarity> destination)
        {
            foreach (var enemy in source)
            {
                var clone = new SpawnableEnemyWithRarity
                {
                    enemyType = enemy.enemyType,
                    rarity = enemy.rarity
                };
                destination.Add(clone);
            }
        }
    }
}