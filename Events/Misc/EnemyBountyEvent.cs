using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class EnemyBountyEvent : HullEvent
{
    public override string ID() => "EnemyBounty";

    public override int GetWeight() => 30;
    public override string GetDescription() => "Company rewards enemy kills with bonus credits.";
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
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
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