using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;
using UnityEngine.UIElements.Collections;

namespace HullBreakerCompany.Events.Integrated.SCP;

public class EggmanEvent : HullEvent
{
    public EggmanEvent() {
        ID = "Eggman";
        Weight = 10;
        Description = "Increases SCP3199 outside spawn frequency.";
        MessagesList = new List<string>() {
            { "Dominated by hostile life form" },
            { "Aggressive outside species" },
            { "Eggs spotted outside the facility" }
        };
        shortMessagesList = new List<string>() {
            { "EGGMAN" }
        };
    }

    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable("SCP3199"))
        {
            return false;
        }
        levelModifier.AddOutsideEnemyComponentRarity("SCP3199", 100);
        levelModifier.AddOutsideEnemyComponentMaxCount("SCP3199", 4);
        levelModifier.AddEnemyComponentPower("SCP3199", 1);
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