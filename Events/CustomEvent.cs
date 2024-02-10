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
    
    public List<string> EnemySpawnList = new ();
    public List<string> OutsideSpawnList = new ();
    
    public int Rarity = 1;
    public override bool Execute(SelectableLevel level, Dictionary<Type, int> componentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        foreach (var enemy in EnemySpawnList.TakeWhile(enemy => enemy != "off"))
        {
            if (Plugin.EnemyBase.TryGetValue(enemy, out var value))
            {
                componentRarity.Add(value, Rarity);
            } else Plugin.Mls.LogError($"Enemy {enemy} not found in EnemyBase");
        }

        foreach (var enemy in OutsideSpawnList.TakeWhile(enemy => enemy != "off"))
        {
            if (Plugin.EnemyBase.TryGetValue(enemy, out var value))
            {
                outsideComponentRarity.Add(value, Rarity);
            } else Plugin.Mls.LogError($"Enemy {enemy} not found in EnemyBase");
        }

        HullManager.SendChatEventMessage(Plugin.UseShortChatMessages ? GetShortMessage() : GetMessage());
        return true;
    }
}