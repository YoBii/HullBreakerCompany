using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class LizardsEvent : HullEvent
{
    public override string ID() => "Lizards";
    public override int GetWeight() => 15;
    public override string GetDescription() => "Increased chance of puffers spawn";
    public static List<String> MessagesList = new() {
        { "Hostile life form detected" },
        { "A few species dominate this moon" },
        { "Don't get too close!" },
        { "They are more afraid of you than you are of them" },
        { "Don't inhale the spores!" }
    };
    public static List<String> shortMessagesList = new() {
        { "HIGH POPULATION" },
        { "DOMINANT SPECIES" },
        { "SPORE DUST" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        if (level.Enemies.All(unit => unit.enemyType.enemyPrefab.GetComponent<PufferAI>() == null)) {
            Plugin.Mls.LogWarning($"Can't spawn PufferAI on this moon.");
            return false;
        }
        
        enemyComponentRarity.Add(typeof(PufferAI), 128);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}