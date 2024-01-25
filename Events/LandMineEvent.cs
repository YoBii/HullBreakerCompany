using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class LandMineEvent : HullEvent
{
    public override string ID() => "LandMine";
    public override int GetWeight() => 30;
    public override string GetDescription() => "Increased chance of landmines spawning";
    public static List<String> MessagesList = new() {
        { "High security compound" },
        { "They rigged this place up"},
        { "Watch your step!"},
        { "Expect mines" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>FORTIFIED</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        Plugin.addLandminesToLevelUnits(level);
        HullManager.SendChatEventMessage(this);
    }
}