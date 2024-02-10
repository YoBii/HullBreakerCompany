using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class HoarderBugEvent : HullEvent
{
    public override string ID() => "HoarderBug";
    public override int GetWeight() => 50;
    public override string GetDescription() => "Increased chance of hoarder bug spawn";
    public static List<String> MessagesList = new() {
        { "Residues of pestice detected in the atmosphere" },
        { "Scrap is.. moving?"},
        { "A few species dominate this moon"},
        { "The competition is already here" },
        { "It's not stealing. They don't own a thing!" },
        { "Friend or foe?" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>CAPTURE THE SCRAP</color>";
    public override bool Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        if (level.Enemies.All(unit => unit.enemyType.enemyPrefab.GetComponent<HoarderBugAI>() == null)) {
            Plugin.Mls.LogWarning($"Can't spawn HoarderBugAI on this moon.");
            return false;
        }
        
        enemyComponentRarity.Add(typeof(HoarderBugAI), 256);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}