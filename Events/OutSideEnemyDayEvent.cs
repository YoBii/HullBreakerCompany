using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;
using UnityEngine;

namespace HullBreakerCompany.Events;

public class OutSideEnemyDayEvent : HullEvent
{
    public override string ID() => "OutSideEnemyDay";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Increased amount of enemies on the surface during the daytime";
    public override string GetMessage() => "<color=white>Increased amount of enemies on the surface during the daytime</color>";
    public override string GetShortMessage() => "<color=white>SILENCE SEASON</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        level.minScrap = 28;
        level.maxScrap = 28;
        level.outsideEnemySpawnChanceThroughDay = new AnimationCurve(new Keyframe(0f, 512f));
        
        HullManager.SendChatEventMessage(this);
    }
}