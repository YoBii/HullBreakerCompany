using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class ArachnophobiaEvent : HullEvent
{
    public override string ID() => "Arachnophobia";
    public override int GetWeight() => 20;
    public override string GetDescription() => "Increased chance of spider spawning";
    public static List<String> MessagesList = new() {
        { "Hostile life form detected"},
        { "A few species dominate this moon"},
        { "This place is crawling.." },
        { "They have 8 eyes.." },
        { "Natural spider habitat" },
        { "Don't get spun up!" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>HIGH POPULATION</color>";
    public override bool Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        if (level.Enemies.All(unit => unit.enemyType.enemyPrefab.GetComponent<SandSpiderAI>() == null)) {
            Plugin.Mls.LogWarning($"Can't spawn SandSpiderAI on this moon.");
            return false;
        }
        
        enemyComponentRarity.Add(typeof(SandSpiderAI), 256);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}