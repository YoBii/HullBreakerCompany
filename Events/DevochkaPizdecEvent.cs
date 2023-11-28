using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class DevochkaPizdecEvent : HullEvent
{
    public override string ID() => "DevochkaPizdec";
    public override int GetWeight() => 5;
    public override string GetDescription() => "Increased chance of phantom girl spawn";
    public override string GetMessage() => "<color=white>A lot of workers are going crazy here</color>";
    public override string GetShortMessage() => "<color=white>COTARD SYNDROME</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        componentRarity.Add(typeof(DressGirlAI), 1024);
        HullManager.SendChatEventMessage(this);
    }
}