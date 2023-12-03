using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;
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
        return _message;
    }
    
    public override string GetShortMessage()
    {
        return _shortMessage;
    }
    
    public List<string> EnemySpawnList = new List<string>();
    public List<string> OutsideSpawnList = new List<string>();
    
    public int Rarity = 1;
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        foreach (var enemy in EnemySpawnList.Where(enemy => enemy != "off"))
        {
            enemyComponentRarity.Add(Plugin.EnemyBase[enemy], Rarity);
        }
        foreach (var outside in OutsideSpawnList.Where(outside => outside != "off"))
        {
            outsideComponentRarity.Add(Plugin.EnemyBase[outside], Rarity);
        }
        HullManager.SendChatEventMessage(this);
    }
}