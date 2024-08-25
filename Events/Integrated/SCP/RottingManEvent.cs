using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;
using UnityEngine.UIElements.Collections;

namespace HullBreakerCompany.Events.Integrated.SCP;

public class RottingManEvent: HullEvent
{
    public override string ID() => "RottingMan";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Increases SCP106 spawn frequency and can spawn more of them.";
    public static List<string> MessagesList = new() {
        { "Reports of paranormal activity" },
        { "Paranormal signature detected" },
        { "Stay away from any elderly 'rotting' humanoids.." },
        { "No physical interaction with SCP-106 is allowed at any time." }
    };
    public static List<string> shortMessagesList = new() {
        { "ROTTINGMAN" }
    };
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
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