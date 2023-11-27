using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class EnemyBountyEvent : HullEvent
{
    public override string ID() => "EnemyBounty";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        const string message = "<color=white>Company pays money for killing the enemies!</color>";
        Plugin.BountyIsActive = true;
        HullManager.SendChatMessage(message);
    }
}