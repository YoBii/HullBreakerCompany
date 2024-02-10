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
    public static List<String> MessagesList = new() {
        { "Company bounty: neutralize threats" },
        { "The company pays you for killing monsters" },
        { "Bounty assigned: kill enemies" },
        { "Company bounty: RIP AND TEAR" }
    };
    public static List<String> shortMessagesList = new() {
        { "KILL BOUNTY" },
        { "COMPANY BOUNTY" },
        { "RIP AND TEAR" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        Plugin.BountyIsActive = true;
        Plugin.BountyFirstKill = true;
        HullManager.AddChatEventMessage(this);
        return true;
    }
}