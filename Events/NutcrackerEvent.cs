using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class NutcrackerEvent : HullEvent
{
    public override string ID() => "Nutcracker";
    public override int GetWeight() => 5;
    public override string GetDescription() => "Increased chance of NutCracker spawn";
    public static List<String> MessagesList = new() {
        { "Detected signs of paranormal activity" },
        { "Several anomalies detected" },
        { "Something's moving but not alive.." },
        { "Keep your head down" },
        { "Protect your nuts!" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        if (level.Enemies.All(unit => unit.enemyType.enemyPrefab.GetComponent<NutcrackerEnemyAI>() == null)) {
            Plugin.Mls.LogWarning($"Can't spawn NutcrackerEnemyAI on this moon.");
            return false;
        }
        
        enemyComponentRarity.Add(typeof(NutcrackerEnemyAI), 256);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}