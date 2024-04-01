using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class EnemyBountyEvent : HullEvent
{
    public override string ID() => "EnemyBounty";

    public override int GetWeight() => 30;
    public override string GetDescription() => "Company pays money for killing the enemies";
    public static List<string> MessagesList = new() {
        { "Company bounty: neutralize threats" },
        { "The company pays you for killing monsters" },
        { "Bounty assigned: kill enemies" },
        { "Company bounty: RIP AND TEAR" }
    };
    public static List<string> shortMessagesList = new() {
        { "KILL BOUNTY" },
        { "COMPANY BOUNTY" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        EventsHandler.BountyIsActive = true;
        EventsHandler.BountyFirstKill = true;
        HullManager.AddChatEventMessage(this);
        return true;
    }
}