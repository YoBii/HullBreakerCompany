using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class TurretEvent : HullEvent
{
    public override string ID() => "Turret";
    public override int GetWeight() => 5;
    public override string GetDescription() => "Increased chance of turrets spawning";
    public static List<String> MessagesList = new() {
        { "High security compound" },
        { "It's right around the corner!" },
        { "Keep moving and don't get stuck!" },
        { "The company recommends bringing a bulletproof vest" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>HIGH SECURITY</color>";
    public override bool Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        HullManager.AddChatEventMessage(this);
        Plugin.addTurretsToLevelUnits(level);
        return true;
    }
}