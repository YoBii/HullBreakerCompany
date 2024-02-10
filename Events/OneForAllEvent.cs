using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class OneForAllEvent : HullEvent
{
    public override string ID() => "OneForAll";
    public override int GetWeight() => 5;
    public override string GetDescription() => "The ship will fly into orbit in an hour if one of the workers dies";
    public override string GetMessage() => "<color=white>You have been selected for team building measures!</color>";
    public override string GetShortMessage() => "<color=red>ONE FOR ALL!</color>";
    public override bool Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        Plugin.OneForAllIsActive = true;
        HullManager.AddChatEventMessage(this);
        return true;
    }
}