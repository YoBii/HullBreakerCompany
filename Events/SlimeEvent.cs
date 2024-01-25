using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class SlimeEvent : HullEvent
{
    public override string ID() => "Slime";
    public override int GetWeight() => 20;
    public override string GetDescription() => "Increased chance of slime spawn";
    public static List<String> MessagesList = new() {
        { "Hostile life form detected"},
        { "A few species dominate this moon"},
        { "Don't get lost in the sauce" },
        { "Shapeless creature detected. Water content 99.9%" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>HIGH POPULATION</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        if (level.Enemies.All(unit => unit.enemyType.enemyPrefab.GetComponent<BlobAI>() == null)) return;
        
        enemyComponentRarity.Add(typeof(BlobAI), 48);
        HullManager.SendChatEventMessage(this);
    }
}