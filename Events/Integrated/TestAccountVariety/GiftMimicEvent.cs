using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class GiftMimicEvent : HullEvent
{
    public GiftMimicEvent() {
        ID = "GiftMimic";
        Weight = 15;
        Description = "Spawns additional GiftMimics inside. Uses LandmineScale config divided by 3.";
        MessagesList = new List<string>() {
            { "High security compound" },
            { "They rigged this place up" },
            { "Don't get fooled." },
            { "Is this a gift.. for me?" }
        };
        shortMessagesList = new List<string>() {
            { "GIFT?" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsTrapUnitSpawnable("GiftMimic")) return false;
        levelModifier.AddTrapUnit("GiftMimic", Plugin.LandmineScale / 3);
        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}