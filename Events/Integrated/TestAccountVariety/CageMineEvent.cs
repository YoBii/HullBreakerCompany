using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class CageMineEvent : HullEvent
{
    public CageMineEvent() {
        ID = "PrisonMine";
        Weight = 15;
        Description = "Spawns additional cage/prison mines inside. Uses TurretScale config.";
        MessagesList = new List<string>() {
            { "High security compound" },
            { "They rigged this place up" },
            { "Don't get trapped." },
            { "Behind bars.." },
            { "Cages used to trap wildlife are stored here" }
        };
        shortMessagesList = new List<string>() {
            { "CAGE" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsTrapUnitSpawnable("CageMine")) return false;
        levelModifier.AddTrapUnit("CageMine", Plugin.TurretScale);
        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}