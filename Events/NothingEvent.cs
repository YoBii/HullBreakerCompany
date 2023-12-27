using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class NothingEvent : HullEvent
{
    public override string ID() => "Nothing";
    public override int GetWeight() => 60;
    public override string GetDescription() => "Nothing happens";
    public override string GetMessage() => "<color=white>---</color>";
    public override string GetShortMessage() => "<color=white>---</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        HullManager.SendChatEventMessage(this);
    }
}