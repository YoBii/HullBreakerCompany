using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class LandMineEvent : HullEvent
{
    public LandMineEvent() {
        ID = "LandMine";
        Weight = 30;
        Description = "Spawns additional landmines inside.";
        MessagesList = new List<string>() {
            { "High security compound" },
            { "They rigged this place up" },
            { "Watch your step!" },
            { "Expect mines" },
            { "Lots of mines might block your path" }
        };
        shortMessagesList = new List<string>() {
            { "LANDMINES" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsTrapUnitSpawnable(Util.getTrapUnitByType(typeof(Landmine)))) return false;
        levelModifier.AddTrapUnit(Util.getTrapUnitByType(typeof(Landmine)), Plugin.LandmineScale);
        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}