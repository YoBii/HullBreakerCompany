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
    
    public List<string> EnemySpawnList = new ();
    public List<string> OutsideSpawnList = new ();
    
    public int Rarity = 1;
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        foreach (var enemy in EnemySpawnList.TakeWhile(enemy => enemy != "off"))
        {
            componentRarity.Add(Plugin.EnemyBase[enemy], Rarity);
        }
        
        foreach (var enemy in OutsideSpawnList.TakeWhile(enemy => enemy != "off"))
        {
            outsideComponentRarity.Add(Plugin.EnemyBase[enemy], Rarity);
        }
        
        HullManager.SendChatEventMessage("<color=white>"+ GetMessage() +"</color>");
    }
}