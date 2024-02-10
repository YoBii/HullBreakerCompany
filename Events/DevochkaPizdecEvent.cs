using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class DevochkaPizdecEvent : HullEvent
{
    public override string ID() => "DevochkaPizdec";
    public override int GetWeight() => 5;
    public override string GetDescription() => "Increased chance of phantom girl spawn";
    public static List<String> MessagesList = new() {
        { "Detected signs of paranormal activity" },
        { "Several anomalies detected" },
        { "Workers were loosing their mind before they shut this place down" },
        { "Don't loose your head now!" }
    };
    public static List<String> shortMessagesList = new() {
        { "PARANORMAL" },
        { "HAUNTED" },
        { "PARANOID" },
        { "PLAYTIME "}
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        if (level.Enemies.All(unit => unit.enemyType.enemyPrefab.GetComponent<DressGirlAI>() == null)) {
            Plugin.Mls.LogWarning($"Can't spawn DressGirlAI on this moon.");
            return false;
        }

        
        enemyComponentRarity.Add(typeof(DressGirlAI), 256);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}