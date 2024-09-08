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
    private List<string> MessageList = new List<string>();
    private List<string> ShortMessageList = new List<string>();
    
    public override string GetID() 
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
    
    public void AddMessage(string msg)
    {
        MessageList.Add(msg);
    }
    
    public void AddShortMessage(string msg)
    {
        ShortMessageList.Add(msg);
    }
    
    public override string GetMessage()
    {
        return "<color=white>" + MessageList[UnityEngine.Random.Range(0, MessageList.Count)] + "</color>";
    }
    public string GetReadableMessages() {
        return string.Join("; ", MessageList);
    }
    
    public override string GetShortMessage()
    {
        return "<color=white>" + ShortMessageList[UnityEngine.Random.Range(0, ShortMessageList.Count)] + "</color>";
    }
    public string GetReadableShortMessage() {
        return string.Join("; ", ShortMessageList);
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
        if (!SimulateExecution(level, levelModifier, EnemySpawnList, OutsideEnemySpawnList, DaytimeEnemySpawnList, ScrapSpawnList)) return false;

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

        if (ScrapSpawnList.Count > 0) {
            ScrapSpawnList = CalculateScrapRarities(ScrapSpawnList, levelModifier);
            if (ScrapSpawnList != null && ScrapSpawnList.Count > 0) {
                levelModifier.AddSpawnableScrapRarityDict(ScrapSpawnList);
            }
        }

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