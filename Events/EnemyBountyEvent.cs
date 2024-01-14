using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class EnemyBountyEvent : HullEvent
{
    public override string ID() => "EnemyBounty";
    
    public override int GetWeight() => 50;
    public override string GetDescription() => "Company pays money for killing the enemies";
    public override string GetMessage() => "<color=white>Company bounty active: RIP AND TEAR</color>";
    public override string GetShortMessage() => "<color=white>KILL BOUNTY</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        Plugin.BountyIsActive = true;
        HullManager.SendChatEventMessage(this);
    }
}