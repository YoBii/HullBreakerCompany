using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class PropulsionMineEvent : HullEvent
{
    public PropulsionMineEvent() {
        ID = "PropulsionMine";
        Weight = 15;
        Description = "Spawns additional propulsion mines inside. Uses LandmineScale config.";
        MessagesList = new List<string>() {
            { "High security compound" },
            { "They rigged this place up" },
            { "Watch your step!" },
            { "Expect special mines" },
            { "Riddled with propulsion mines" }
        };
        shortMessagesList = new List<string>() {
            { "YEET" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsTrapUnitSpawnable("Yeetmine")) return false;
        levelModifier.AddTrapUnit("Yeetmine", Plugin.LandmineScale);
        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}