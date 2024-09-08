using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;
using UnityEngine.UIElements.Collections;

namespace HullBreakerCompany.Events.Integrated.Surfaced;

public class UrchinEvent : HullEvent
{
    public UrchinEvent() {
        ID = "Urchin";
        Weight = 10;
        Description = "Increases Urching spawn frequency. Spawns up to ten early.";
        MessagesList = new List<string>() {
            { "Dominated by hostile life form" },
            { "Dominant species detected" },
            { "Urchin Invasion" },
            { "Exponential growth" },
            { "They will not stop breeding" }
        };
        shortMessagesList = new List<string>() {
            { "SPIKY" },
            { "BALLS" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsDaytimeEnemySpawnable("Urchin"))
        {
            return false;
        }
        levelModifier.AddDaytimeEnemyComponentRarity("Urchin", 10000);
        levelModifier.AddDaytimeEnemyComponentMaxCount("Urchin", 10);
        levelModifier.AddDaytimeEnemySpawnChanceThroughoutDay(10);
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