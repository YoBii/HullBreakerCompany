using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using UnityEngine;

namespace HullBreakerCompany.Events;

public class OutSideEnemyDayEvent : HullEvent
{
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        string message = "<color=white>Increased amount of enemies on the surface during the daytime</color>";
        level.minScrap = 28;
        level.maxScrap = 28;
        level.outsideEnemySpawnChanceThroughDay = new AnimationCurve(new Keyframe(0f, 512f));
        
        HUDManager.Instance.AddTextToChatOnServer(message);
    }
}