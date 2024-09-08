using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;
using UnityEngine.UIElements.Collections;

namespace HullBreakerCompany.Events.Integrated.SCP;

public class InvisibleGuestEvent : HullEvent
{
    public InvisibleGuestEvent() {
        ID = "InvisibleGuest";
        Weight = 15;
        Description = "Increases SCP966 spawn frequency. Spawns up to three.";
        MessagesList = new List<string>() {
            { "Reports of paranormal activity" },
            { "Paranormal signature detected" },
            { "It's wearing you down.." },
            { "Did you see that?!?!" }
        };
        shortMessagesList = new List<string>() {
            { "SLUGGISH" },
            { "GUEST" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable("scp966"))
        {
            return false;
        }
        levelModifier.AddEnemyComponentRarity("scp966", 100);
        levelModifier.AddEnemyComponentMaxCount("scp966", 3);
        levelModifier.AddEnemyComponentPower("scp966", 1);
        if (Plugin.ColoredEventMessages)
        {
            HullManager.AddChatEventMessageColored(this, "red");
        }
        else
        {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}