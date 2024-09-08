using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class SpikeTrapEvent : HullEvent
{
    public SpikeTrapEvent() {
        ID = "SpikeTrap";
        Weight = 10;
        Description = "Spawns additional spike traps inside.";
        MessagesList = new List<string>() {
            { "High security compound" },
            { "They rigged this place up" },
            { "They call this place Takeshi's castle" },
            { "Locals call this place Sen's fortress" },
            { "What is that noise?" }
        };
        shortMessagesList = new List<string>() {
            { "SPIKES" },
            { "TRAPS" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsTrapUnitSpawnable(Util.getTrapUnitByType(typeof(SpikeRoofTrap)))) return false;
        levelModifier.AddTrapUnit(Util.getTrapUnitByType(typeof(SpikeRoofTrap)), Plugin.SpikeTrapScale);
        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}