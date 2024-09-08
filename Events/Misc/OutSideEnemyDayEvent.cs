using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;
using UnityEngine;

namespace HullBreakerCompany.Events.Misc;

public class OutSideEnemyDayEvent : HullEvent
{
    public OutSideEnemyDayEvent() {
        ID = "OutSideEnemyDay";
        Weight = 10;
        Description = "Increased amount of outside enemies during daytime. Eclipsed without eclipse.";
        MessagesList = new List<string>() {
            { "This place was shrouded in darkness for weeks" },
            { "A prolonged eclipse has disrupted wild life's circadian rhythm" },
            { "Highly populated surface area" },
            { "Due to frequent weather events wildlife roams the surface during daytime" }
        };
        shortMessagesList = new List<string>() {
            { "SILENCE SEASON" },
            { "OUTSIDE PARTY" },
            { "ECLIPSED?" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        levelModifier.AddOutsideEnemySpawnChanceThroughoutDay(512);

        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}