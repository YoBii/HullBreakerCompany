using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class EnemyBountyEvent : HullEvent
{
    public EnemyBountyEvent() {
        ID = "EnemyBounty";
        Weight = 30;
        Description = "Company rewards enemy kills with bonus credits.";
        MessagesList = new List<string>() {
            { "Company bounty: neutralize threats" },
            { "The company pays you for killing monsters" },
            { "Bounty assigned: kill enemies" },
            { "Company bounty: RIP AND TEAR" }
        };
        shortMessagesList = new List<string>() {
            { "KILL BOUNTY" },
            { "COMPANY BOUNTY" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        EventsHandler.BountyIsActive = true;
        EventsHandler.BountyFirstKill = true;
        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "green");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}