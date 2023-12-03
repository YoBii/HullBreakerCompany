using System.Collections.Generic;
using UnityEngine;

namespace HullBreakerCompany.Hull;

public class SelectableLevelState
{
    private int _minScrap;
    private int _maxScrap;
    private int _minTotalScrapValue;
    private int _maxTotalScrapValue;
    private int _maxEnemyPowerCount;
    private int _maxOutsideEnemyPowerCount;
    private int _maxDaytimeEnemyPowerCount;
    private List<SpawnableItemWithRarity> _spawnableScrap;
    private AnimationCurve _enemySpawnChanceThroughoutDay;
    private AnimationCurve _outsideEnemySpawnChanceThroughDay;
    private AnimationCurve _daytimeEnemySpawnChanceThroughDay;
    
    // private List<SpawnableEnemyWithRarity> _enemies;
    // private List<SpawnableEnemyWithRarity> _outsideEnemies;
    // private List<SpawnableEnemyWithRarity> _daytimeEnemies;
    
    public SelectableLevelState(SelectableLevel level) {
        
        _minScrap = level.minScrap;
        
        _maxScrap = level.maxScrap - 1;
        
        _minTotalScrapValue = level.minTotalScrapValue - 1;

        _maxTotalScrapValue = level.maxTotalScrapValue - 1;

        _maxEnemyPowerCount = level.maxEnemyPowerCount - 1;

        _maxOutsideEnemyPowerCount = level.maxOutsideEnemyPowerCount - 1;

        _maxDaytimeEnemyPowerCount = level.maxDaytimeEnemyPowerCount - 1;
        
        _spawnableScrap = level.spawnableScrap;
        
        _enemySpawnChanceThroughoutDay = level.enemySpawnChanceThroughoutDay;
        
        _outsideEnemySpawnChanceThroughDay = level.outsideEnemySpawnChanceThroughDay;
        
        _daytimeEnemySpawnChanceThroughDay = level.daytimeEnemySpawnChanceThroughDay;
        
        //Should debug this
        // _enemies = level.Enemies;
        //
        // _outsideEnemies = level.OutsideEnemies;
        //
        // _daytimeEnemies = level.DaytimeEnemies;
    }

    public void RestoreState(SelectableLevel level) {
        
        level.minScrap = _minScrap;
        
        level.maxScrap = _maxScrap + 1;
        
        level.minTotalScrapValue = _minTotalScrapValue + 1;

        level.maxTotalScrapValue = _maxTotalScrapValue + 1;

        level.maxEnemyPowerCount = _maxEnemyPowerCount + 1;

        level.maxOutsideEnemyPowerCount = _maxOutsideEnemyPowerCount + 1;

        level.maxDaytimeEnemyPowerCount = _maxDaytimeEnemyPowerCount + 1;
        
        level.spawnableScrap = _spawnableScrap;
        
        level.enemySpawnChanceThroughoutDay = _enemySpawnChanceThroughoutDay;
        
        level.outsideEnemySpawnChanceThroughDay = _outsideEnemySpawnChanceThroughDay;
        
        level.daytimeEnemySpawnChanceThroughDay = _daytimeEnemySpawnChanceThroughDay;

        //Should debug this
        // level.Enemies = _enemies;
        //
        // level.OutsideEnemies = _outsideEnemies;
        //
        // level.DaytimeEnemies = _daytimeEnemies;
    }
}