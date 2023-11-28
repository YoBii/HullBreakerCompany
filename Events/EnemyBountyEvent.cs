using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class EnemyBountyEvent : HullEvent
{
    public override string ID() => "EnemyBounty";
    
    public override int GetWeight() => 50;
    public override string GetDescription() => "Company pays money for killing the enemies / 60 per enemy";
    public override string GetMessage() => "<color=white>Company pays money for killing the enemies!</color>";
    public override string GetShortMessage() => "<color=white>ENEMY BOUNTY</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        Plugin.BountyIsActive = true;
        HullManager.SendChatEventMessage(this);
    }
}