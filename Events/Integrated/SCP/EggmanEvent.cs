using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;
using UnityEngine.UIElements.Collections;

namespace HullBreakerCompany.Events.Integrated.SCP;

public class EggmanEvent : HullEvent
{
    public override string ID() => "Eggman";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Increases SCP3199 outside spawn frequency.";
    public static List<string> MessagesList = new() {
        { "Dominated by hostile life form" },
        { "Aggressive outside species" },
        { "Eggs spotted outside the facility" }
    };
    public static List<string> shortMessagesList = new() {
        { "EGGMAN" }
    };
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
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