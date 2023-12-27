using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class SlimeEvent : HullEvent
{
    public override string ID() => "Slime";
    public override int GetWeight() => 20;
    public override string GetDescription() => "Increased chance of slime spawn";
    public override string GetMessage() => "<color=white>Large number of life forms detected, likely hostile</color>";
    public override string GetShortMessage() => "<color=white>HIGH POPULATION</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        enemyComponentRarity.Add(typeof(BlobAI), 48);
        HullManager.SendChatEventMessage(this);
    }
}