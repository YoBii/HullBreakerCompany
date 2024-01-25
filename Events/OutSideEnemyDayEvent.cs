using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;
using UnityEngine;

namespace HullBreakerCompany.Events;

public class OutSideEnemyDayEvent : HullEvent
{
    public override string ID() => "OutSideEnemyDay";
    public override int GetWeight() => 3;
    public override string GetDescription() => "Increased amount of enemies on the surface during the daytime";
    public static List<String> MessagesList = new() {
        { "This place was shrouded in darkness for weeks" },
        { "Hihghly populated surface area" },
        { "Due to frequent weather events wildlife roams the surface during daytime" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=red>SILENCE SEASON</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        level.outsideEnemySpawnChanceThroughDay = new AnimationCurve(new Keyframe(0f, 512f));
        
        HullManager.SendChatEventMessage(this);
    }
}