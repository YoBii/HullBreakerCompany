using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class MaskedEvent : HullEvent
{
    public override string ID() => "Masked";
    public override int GetWeight() => 5;
    public override string GetDescription() => "Increased chance of masked enemy spawn";
    public static List<String> MessagesList = new() {
        { "Detected signs of paranormal activity" },
        { "Several anomalies detected" },
        { "Phantom of the Opera" },
        { "Who's the new guy?"}
    };
    public static List<String> shortMessagesList = new() {
        { "PARANORMAL" },
        { "HAUNTED" },
        { "TRUST ISSUES" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        if (level.Enemies.All(unit => unit.enemyType.enemyPrefab.GetComponent<MaskedPlayerEnemy>() == null)) {
            Plugin.Mls.LogWarning($"Can't spawn MaskedPlayerEnemy on this moon.");
            return false;
        }
        
        enemyComponentRarity.Add(typeof(MaskedPlayerEnemy), 256);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}