using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class HullBreakEvent : HullEvent
{
    public override string ID() => "HullBreak";
    public override int GetWeight() => 7;
    public override string GetDescription() => "Getting money for visiting this moon";
    public override string GetMessage() => "<color=green>Take a break, the company is sending money for visiting the moon</color>";
    public override string GetShortMessage() => "<color=white>TAKE A BREAK</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        HullManager.Instance.AddMoney(120);
        HullManager.SendChatEventMessage(GetMessage());
    }
}