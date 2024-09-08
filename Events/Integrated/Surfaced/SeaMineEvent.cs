using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class SeaMineEvent : HullEvent
{
    public SeaMineEvent() {
        ID = "SeaMine";
        Weight = 15;
        Description = "Spawns additional seamines inside. Uses LandmineScale config.";
        MessagesList = new List<string>() {
            { "High security compound" },
            { "They rigged this place up" },
            { "Watch your head!" },
            { "Expect mines" },
            { "Lots of mines might block your path" }
        };
        shortMessagesList = new List<string>() {
            { "SEAMINES" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsTrapUnitSpawnable("Old Seamine")) return false;
        levelModifier.AddTrapUnit("Old Seamine", Plugin.LandmineScale);
        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}