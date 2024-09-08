using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;
using UnityEngine.UIElements.Collections;

namespace HullBreakerCompany.Events.Integrated.SCP;

public class RottingManEvent: HullEvent
{
    public RottingManEvent() {
        ID = "RottingMan";
        Weight = 7;
        Description = "Increases SCP106 spawn frequency and can spawn more of them.";
        MessagesList = new List<string>() {
            { "Reports of paranormal activity" },
            { "Paranormal signature detected" },
            { "Stay away from any elderly 'rotting' humanoids.." },
            { "No physical interaction with SCP-106 is allowed at any time." }
        };
        shortMessagesList = new List<string>() {
            { "ROTTINGMAN" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable("SCP106Obj2"))
        {
            return false;
        }
        levelModifier.AddOutsideEnemyComponentRarity("SCP106Obj2", 100);
        levelModifier.AddOutsideEnemyComponentMaxCount("SCP106Obj2", 3);
        levelModifier.AddEnemyComponentPower("SCP106Obj2", 1);
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