using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;
using UnityEngine;

namespace HullBreakerCompany.Events;

public class OutSideEnemyDayEvent : HullEvent
{
    public override string ID() => "OutSideEnemyDay";
    public override int GetWeight() => 5;
    public override string GetDescription() => "Increased amount of enemies on the surface during the daytime";
    public override string GetMessage() => "<color=white>Increased amount of enemies on the surface during the daytime</color>";
    public override string GetShortMessage() => "<color=red>SILENCE SEASON</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        level.outsideEnemySpawnChanceThroughDay = new AnimationCurve(new Keyframe(0f, 512f));
        
        HullManager.SendChatEventMessage(GetMessage());
    }
}