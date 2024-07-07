using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;
using UnityEngine;

namespace HullBreakerCompany.Events;

public class CustomEvent : HullEvent
{
    private string _id;
    private int _weight;
    private string _message;
    private string _shortMessage;
    
    public override string ID() 
    { 
        return _id; 
    }

    public void SetID(string value)
    {
        _id = value;
    }
    public void SetWeight(int value)
    {
        _weight = value;
    }

    public override int GetWeight()
    {
        return _weight;
    }
    
    public void SetMessage(string value)
    {
        _message = value;
    }
    
    public void SetShortMessage(string value)
    {
        _shortMessage = value;
    }
    
    public override string GetMessage()
    {
        return "<color=white>" + _message + "</color>";
    }
    
    public override string GetShortMessage()
    {
        return "<color=white>" + _shortMessage + "</color>";
    }
    
    //public List<string> EnemySpawnList = new ();
    //public List<string> OutsideSpawnList = new ();
    public Dictionary<string, List<int>> EnemySpawnList = new ();
    public Dictionary<string, List<int>> OutsideEnemySpawnList = new ();
    public Dictionary<string, List<int>> DaytimeEnemySpawnList = new ();
    public Dictionary<string, int> ScrapSpawnList = new ();

    public int addPower;
    public int addOutsidePower;
    public int addDaytimePower;

    public int overrideSpawnRate;
    public int overrideOutsideSpawnRate;
    public int overrideDaytimeSpawnRate;

    public override bool Execute(SelectableLevel level, LevelModifier levelModifier) {
        if(EnemySpawnList.Count > 0 && !EnemySpawnList.Any(enemy => levelModifier.IsEnemySpawnable(enemy.Key))) {
            Plugin.Mls.LogWarning($"Event {ID()} has SpawnableEnemies defined but none of them are spawnable in this level.");
            return false;
        }
        if (OutsideEnemySpawnList.Count > 0 && !OutsideEnemySpawnList.Any(enemy => levelModifier.IsOutsideEnemySpawnable(enemy.Key))) {
            Plugin.Mls.LogWarning($"Event {ID()} has SpawnableOutsideEnemies defined but none of them are spawnable in this level.");
            return false;
        }        
        if (DaytimeEnemySpawnList.Count > 0 && !DaytimeEnemySpawnList.Any(enemy => levelModifier.IsDaytimeEnemySpawnable(enemy.Key))) {
            Plugin.Mls.LogWarning($"Event {ID()} has SpawnableDaytimeEnemies defined but none of them are spawnable in this level.");
            return false;
        }
        if (ScrapSpawnList.Count > 0 && !ScrapSpawnList.Any(scrap => levelModifier.IsScrapSpawnable(scrap.Key))) {
            Plugin.Mls.LogWarning($"Event {ID()} has SpawnableScrap defined but none of them are spawnable in this level.");
            return false;
        }

        foreach (var enemy in EnemySpawnList) {
            levelModifier.AddEnemyComponentRarity(enemy.Key, enemy.Value[0]);
            if (enemy.Value[1] > -1) levelModifier.AddEnemyComponentMaxCount(enemy.Key, enemy.Value[1]);
            if (enemy.Value[2] > -1) levelModifier.AddEnemyComponentPower(enemy.Key, enemy.Value[2]);
        }

        foreach (var enemy in OutsideEnemySpawnList) {
            levelModifier.AddOutsideEnemyComponentRarity(enemy.Key, enemy.Value[0]);
            if (enemy.Value[1] > -1) levelModifier.AddOutsideEnemyComponentMaxCount(enemy.Key, enemy.Value[1]);
            if (enemy.Value[2] > -1) levelModifier.AddOutsideEnemyComponentPower(enemy.Key, enemy.Value[2]);
        }

        foreach (var enemy in DaytimeEnemySpawnList) {
            levelModifier.AddDaytimeEnemyComponentRarity(enemy.Key, enemy.Value[0]);
            if (enemy.Value[1] > -1) levelModifier.AddDaytimeEnemyComponentMaxCount(enemy.Key, enemy.Value[1]);
            if (enemy.Value[2] > -1) levelModifier.AddDaytimeEnemyComponentPower(enemy.Key, enemy.Value[2]);
        }

        ScrapSpawnList = CalculateScrapRarities(ScrapSpawnList, levelModifier);
        if (ScrapSpawnList.Count == 0) return false;
        levelModifier.AddSpawnableScrapRarityDict(ScrapSpawnList);

        if (addPower > 0) levelModifier.AddMaxEnemyPower(addPower);
        if (addOutsidePower > 0) levelModifier.AddMaxOutsideEnemyPower(addOutsidePower);
        if (addDaytimePower > 0) levelModifier.AddMaxDaytimeEnemyPower(addDaytimePower);

        if (overrideSpawnRate > 0) levelModifier.AddEnemySpawnChanceThroughoutDay(overrideSpawnRate);
        if (overrideOutsideSpawnRate > 0) levelModifier.AddOutsideEnemySpawnChanceThroughoutDay(overrideOutsideSpawnRate);
        if (overrideDaytimeSpawnRate > 0) levelModifier.AddDaytimeEnemySpawnChanceThroughoutDay(overrideDaytimeSpawnRate);

        HullManager.AddChatEventMessage(this);
        return true;
    }
}